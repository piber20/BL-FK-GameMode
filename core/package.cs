if(isPackage(GameModeFASTKartsPackage))
	deactivatePackage(GameModeFASTKartsPackage);
package GameModeFASTKartsPackage
{
	//this is called when save loading finishes 
	function GameModeInitialResetCheck()
	{
		Parent::GameModeInitialResetCheck();
		
		//if there is no track list, attempt to create it
		if($FK::numTracks == 0)
			FK_BuildTrackList();
		
		//if tracklist is still empty, there are no tracks
		if($FK::numTracks == 0)
		{
			messageAll('', "\c5No FASTKarts tracks available!");
			return;
		}
		
		if($FK::Initialized)
			return;
		
		$FK::Initialized = true;
		$FK::CurrentTrack = -1;
		
		FK_NextTrack();
	}
	
	//when we're done loading a new track, reset the minigame
	function ServerLoadSaveFile_End()
	{
		Parent::ServerLoadSaveFile_End();
		
		//new track has loaded, reset minigame
		if($DefaultMiniGame.numMembers > 0) //don't bother if no one is here (this also prevents starting at round 2 on server creation)
			$DefaultMiniGame.scheduleReset(); //don't do it instantly, to give people a little bit of time to ghost
		
		FK_ResetSuicideGambling();
	}

	function serverCmdmessageSent(%client, %text)
	{
		//strip and trim it and see if it's blank
		%text = stripMLControlChars(%text);
		%text = trim(%text);
		if(%text $= "")
			return;
		
		//catch flood protection
		if(%client.FK_isChatSpam(%text))
			return;
		
		//passed flood protection, record this message for future flood protection
		%client.lastMessageTime = getSimTime();
		%client.lastMessage = %text;
		
		//chat emotes compatability
		if(isFunction(peReplace) && strPos(%text, ":") >= 0)
		{
			%text = peReplace(%text);
		}
		
		//send the message
		echo(%client.getPlayerName() @ ": " @ %text);
		if(strPos(%text, "http://") < 0 && strPos(%text, "https://") < 0)
			messageAll('', %client.FK_buildChatPrefix() @ %client.FK_buildChatName() @ %client.FK_buildChatSuffix() @ "\c6: " @ %text);
		else //we parent to default chat if a link is detected because i am lazy and stupid
			parent::serverCmdmessageSent(%client, %text);
	}
	
	function serverCmdTeamMessageSent(%client, %text)
	{
		//strip and trim it and see if it's blank
		%text = stripMLControlChars(%text);
		%text = trim(%text);
		if(%text $= "")
			return;
		
		//catch flood protection
		if(%client.FK_isChatSpam(%text))
			return;
		
		//passed flood protection, record this message for future flood protection
		%client.lastMessageTime = getSimTime();
		%client.lastMessage = %text;
		
		//send the message
		echo(%client.getPlayerName() @ ": " @ %text);
		if(strPos(%text, "http://") < 0 && strPos(%text, "https://") < 0)
			messageAll('', %client.FK_buildChatPrefix() @ %client.FK_buildChatName() @ %client.FK_buildChatSuffix() @ "\c4: " @ %text);
		else //we parent to default chat if a link is detected because i am lazy and stupid
			parent::serverCmdTeamMessageSent(%client, %text);
	}
	
	//vehicles should explode in water
	function VehicleData::onEnterLiquid(%data, %obj, %coverage, %type)
	{
		Parent::onEnterLiquid(%data, %obj, %coverage, %type);
		
		%obj.damage(%obj, %obj.getPosition(), 10000, $DamageType::Fall);
		%obj.finalExplosion();
	}
	
	//players should die in water
	function Armor::onEnterLiquid(%data, %obj, %coverage, %type)
	{
		Parent::onEnterLiquid(%data, %obj, %coverage, %type);
		
		%client = %obj.client;
		if(isObject(%client))
		{
			if(%client.minigame != $DefaultMinigame)
				return;
		}
		
		%obj.hasShotOnce = true;
		%obj.invulnerable = false;
		%obj.damage(%obj, %obj.getPosition(), 10000, $DamageType::Fall);
	}
	
	//when vehicle spawns, it cannot move (event must enable it)
	//this solves the driving through the garage problem
	function WheeledVehicleData::onAdd(%data,%obj)
	{
		Parent::onAdd(%data, %obj);
		
		for(%i = 0; %i < %data.numWheels; %i++)
			%obj.setWheelPowered(%i, %on);
	}

	//also you cannot click-push a vehicle while it is in non-moving mode
	function vehicle::OnActivate(%vehicle, %activatingObj, %activatingClient, %pos, %vec)
	{
		//just check a wheel
		if(!%vehicle.getWheelPowered(2))
			return;
		
		Parent::OnActivate(%vehicle, %activatingObj, %activatingClient, %pos, %vec);
	}
	
	function FK_SuicideGamblingCheck()
	{
		//check for duplicate schedules
		if(isEventPending($FK::SuicideGambleEvent))  
			cancel($FK::SuicideGambleEvent);
		$FK::SuicideGambleEvent = 0;
		
		if(!isObject($FK::LastSuicider))
			return;
		
		if($FK::LastSuicider.getClassName() !$= "GameConnection")
			return;
		
		%player = $FK::LastSuicider.player;
		if(!isObject(%player))
			return;
		
		//last suicider is valid, give them a win emote
		%player.emote(winStarProjectile, 1);
		%player.playAudio(0, rewardSound);
	}

	//if you kill yourself in a vehicle, kill the vehicle
	function serverCmdSuicide(%client)
	{
		%player = %client.player;
		if(!isObject(%player))
			return;
		
		%vehicle = %player.getObjectMount();
		
		//prevent passengers from blowing up vehicles, we only want the driver to be able to do that.
		if(isObject(%vehicle))
		{
			%mountedObj = %vehicle.getMountNodeObject(0);
		
			if(%mountedObj != %player)
				%vehicle = 0;
		}
		
		//kill the vehicle we're in
		if(isObject(%vehicle))
		{
			//if wheels are not powered, we're probably at the start of the race, so don't allow suicide in a vehicle
			%elapsedTime = getSimTime() - %vehicle.poweredTime;
			%doBuzzer = false;
			if(%vehicle.getClassName() $= "AIPlayer")
				%doBuzzer = true;
			else if(!%vehicle.getWheelPowered(2) || %elapsedTime <  $FK::SuicideBufferTime )
				%doBuzzer = true;
			
			if(%doBuzzer)
			{
				//if it isn't running already, start the suicide gambling check
				//we don't have a "start race" function, so we're shoe-horning it in here
				if(!isEventPending($FK::SuicideGamblingEvent) && (%vehicle.poweredTime > 0))
				{  
					//10 seconds is maximum time of suicide prevention
					if(%elapsedTime < $FK::SuicideBufferTime)
						$FK::SuicideGamblingEvent = schedule(11000 - %elapsedTime, 0, FK_SuicideGamblingCheck);
				}
				
				//remember last person to hit the suicide buzzer
				$FK::LastSuicider = %client;
				
				//spam protect the buzzer enough to prevent serious problems
				if(getSimTime() - %player.lastSuicideBuzzerTime > 200)
				{
					%player.lastSuicideBuzzerTime = getSimTime();
					%player.playAudio(0, "Beep_No_Sound");
				}
				return;
			}
			
			//special reward for suicide gambling winner
			if($FK::LastSuicider == %client && !isEventPending($FK::SuicideGamblingEvent))
			{
				%player.playAudio(0, Scream38);
				$FK::LastSuicider = 0;
				return;
			}
			
			//if vehicle is on fire, do final explosion
			//otherwise kill vehicle
			if(%vehicle.getDamagePercent() >= 1.0)
				%vehicle.finalExplosion();
			else
			{
				%vehicle.damage(%vehicle, %vehicle.getPosition(), 10000, $DamageType::Default);        
				%player.burnPlayer(5);
			}
		}
		else
		{         
			//no vehicle, normal suicide
			
			//special reward for suicide gambling winner
			if($FK::LastSuicider == %client && !isEventPending($FK::SuicideGamblingEvent))
			{
				%player.playAudio(0, Scream38);
				$FK::LastSuicider = 0;
				return;
			}
			
			Parent::ServerCmdSuicide(%client);
			return;
		}
	}
	
	//resume death animation after corpse is booted out of burning vehicle
	function Armor::onUnmount(%data, %obj, %slot)
	{
		Parent::onUnmount(%data, %obj, %slot);
		
		if(%obj.getDamagePercent() >= 1)
			%obj.playthread(3, "death1");
	}
	
	//if smoeone gets back into a garage after the game starts, we don't want them to be able to press a button and respawn someone's cart from under them
	function fxDTSBrick::setVehicle(%obj, %data, %client)
	{
		if(isObject(%obj.vehicle))
		{
			//vehicle exists, if it is far from spawn, don't do this event
			%vec = vectorSub(%obj.vehicle.getPosition(), %obj.getPosition());
			%dist = vectorLen(%vec);
			if(%dist > 10)
				return;
		}
		
		Parent::setVehicle(%obj, %data, %client);
	}
	
	//total hack: when a vehicle is turned on record the start-of-race time
	// other options would be to make another event or add in a time offset 
	function fxDTSBrick::setVehiclePowered(%obj, %on, %client)
	{
		Parent::setVehiclePowered(%obj, %on, %client);
		
		if(%on)
		{
			if(!$FK::TrackStarted)
			{
				if(isObject($DefaultMiniGame))
				{
					echo("Race started...");
					$FK::TrackStarted = true;
					if($DefaultMiniGame.raceStartTime <= 0)
						$DefaultMiniGame.raceStartTime = getSimTime();
					
					if(!isObject(FK_FakeClient))
						new GameConnection(FK_FakeClient);
					
					FK_FakeClient.isAdmin = 1;
					FK_FakeClient.isSuperAdmin = 1;
					FK_FakeClient.minigame = $DefaultMinigame;
					
					serverCmdSetMiniGameData(FK_FakeClient, "BD" SPC 1); //brick damage, turned off at round start
					
					for(%a = 0; %a < $DefaultMinigame.numMembers; %a++)
					{
						%member = $DefaultMinigame.member[%a];
						%member.FASTKartsLap = mfloor($FK::StartingLap);
						if($FK::StartingLap != 0)
							messageClient(%member, '', "\c5Started race.");
						
						if($FK::RoundType $= "ROCKET")
						{
							%weapon0 = MegaNukeItem.getID();
							if(isObject(%member))
							{
								if(isObject(%member.player))
									%member.player.tool[0] = %weapon0;
								messageClient(%member, 'MsgItemPickup', '', 0, %weapon0);
							}
						}
						%member.setScore();
					}
				}
			}
		}
	}
	
	function MiniGameSO::Reset(%obj, %client)
	{
		//make sure this value is an number
		$FK::Pref::Rounds::PerTrack = mFloor($FK::Pref::Rounds::PerTrack);

		//handle our race time hack
		%obj.raceStartTime = 0;

		//count number of minigame resets, when we reach the limit, go to next track
		if(%obj.numMembers >= 0)
			$FK::ResetCount++;
		
		//force round number to the last round to effectively stop track rotation
		if($FK::ResetCount > $FK::Pref::Rounds::PerTrack && $FK::Pref::Rounds::PerTrack <= 0)
			$FK::ResetCount = $FK::Pref::Rounds::PerTrack;

		if($FK::VoteNextRound)
		{
			$FK::CurrentTrack = $FK::VoteTrack - 1;
			$FK::BypassRandom = true;
			$FK::ResetCount = 0;
			$FK::onWinRaceList = "";
			FK_NextTrack();
		}
		else if($FK::ResetCount > $FK::Pref::Rounds::PerTrack && $FK::Pref::Rounds::PerTrack > 0)
		{
			$FK::ResetCount = 0;
			$FK::onWinRaceList = "";
			FK_NextTrack();
		}
		else
		{
			if($FK::VoteInProgress)
			{
				messageAll('', "\c2The vote ended.");
				echo("The vote ended");
				$FK::VoteInProgress = false;
				$FK::VoteTrack = "";
				$FK::VoteNextRound = false;
			}
			
			echo("Beginning round...");
			FK_DetermineRoundType();
			
			if($FK::Pref::Rounds::PerTrack > 0)
				messageAll('', "\c5Beginning round " @ $FK::ResetCount @ " of " @ $FK::Pref::Rounds::PerTrack @ ".");
			else
				messageAll('', "\c5Beginning round.");
			messageAll('', $FK::RoundName);
			$FK::TrackStarted = false;
			$FK::TrackCompleted = false;
			Parent::Reset(%obj, %client);
			
			for(%a = 0; %a < $DefaultMinigame.numMembers; %a++)
			{
				%member = $DefaultMinigame.member[%a];
				if(isObject(%member))
				{
					%member.FASTKartsLap = 0;
					%member.FKWinner = false;
					%member.FK_isRockingVote = false;
					%member.FK_LastKartUsed = "";
					%member.setScore();
					if(FK_getRoundTypesAllowed() > 1)
						centerPrint(%member, "<font:palatino linotype:64>" @ $FK::RoundName, 7);
				}
			}
		}

		FK_ResetSuicideGambling();
	}
   
	function serverCmdAddEvent(%client, %delay, %input, %target, %a, %b, %output, %para1, %para2, %para3, %para4)
	{
		Parent::serverCmdAddEvent(%client, %delay, %input, %target, %a, %b, %output, %para1, %para2, %para3, %para4);
		%client.wrenchbrick.checkHasOnWinRaceEvent();
		//if(%client.wrenchbrick.checkHasOnWinRaceEvent())
		//{
		//	if(%input $= "onWinRace")
		//	{
		//		$FK::FinalLap = mfloor(%para1);
		//		echo("found amount of laps: " @ $FK::FinalLap);
		//	}
		//}
	}
	
	function fxDtsBrick::onPlant(%this)
	{
		Parent::onPlant(%this);
		%this.checkHasOnWinRaceEvent();
	}
	
	function fxDtsBrick::onLoadPlant(%this)
	{
		Parent::onLoadPlant(%this);
		%this.checkHasOnWinRaceEvent();
	}
	
	function fxDtsBrick::onDeath(%this)
	{
		Parent::onDeath(%this);
		%this.checkHasOnWinRaceEvent();
	}

	function fxDTSBrick::onRemove(%this)
	{
		Parent::onRemove(%this);
		%this.checkHasOnWinRaceEvent();
	}
	
	function GameConnection::spawnPlayer(%this)
	{
		Parent::spawnPlayer(%this);
		if(%this.minigame == $DefaultMinigame)
		{
			%player = %this.player;
			if(isObject(%player))
			{
				//nothing by default
				%weapon0 = 0;
				%weapon1 = 0;
				%weapon2 = 0;
				%weapon3 = 0;
				%weapon4 = 0;
				
				//get loadouts based on round type
				if($FK::RoundType $= "NORMAL")
				{
					%weapon0 = BangGunItem.getID();
					%weapon1 = BananaItem.getID();
					%weapon2 = FlagItem.getID();
					%weapon3 = PortableCameraItem.getID();
					%weapon4 = YoyoItem.getID();
				}
				else if($FK::RoundType $= "ROCKET")
					%weapon0 = MegaNukeEmptyItem.getID(); //temp filler item for when the real weapon is added after the race starts
				else if($FK::RoundType $= "BOUNCY")
					%player.changeDataBlock(BouncyPlayer);
				
				//apply loadout
				%player.tool[0] = %weapon0;
				messageClient(%this, 'MsgItemPickup', '', 0, %weapon0);
				%player.tool[1] = %weapon1;
				messageClient(%this, 'MsgItemPickup', '', 1, %weapon1);
				%player.tool[2] = %weapon2;
				messageClient(%this, 'MsgItemPickup', '', 2, %weapon2);
				%player.tool[3] = %weapon3;
				messageClient(%this, 'MsgItemPickup', '', 3, %weapon3);
				%player.tool[4] = %weapon4;
				messageClient(%this, 'MsgItemPickup', '', 4, %weapon4);
				
				%this.setScore();
			}
		}
	}
	
	function fxDTSBrick::onPlayerEnterBrick(%this, %obj)
	{
		Parent::onPlayerEnterBrick(%this, %obj);
		
		%client = %obj.client;
		if(!isObject(%client))
			return;
		
		if(%this.checkHasOnWinRaceEvent())
		{
			if($FK::RoundType $= "BOUNCY" || $FK::Pref::Gameplay::VehiclelessWins)
			{
				%obj.FK_ThisIsAPlayerNotAVehicle = true;
				%this.onVehicleEnterBrick(%obj); //very very very hacky trickery
				%obj.FK_ThisIsAPlayerNotAVehicle = false;
			}
		}
	}
	
	function fxDTSBrick::onVehicleEnterBrick(%this, %obj)
	{
		if(%obj.FK_ThisIsAPlayerNotAVehicle)
		{
			if(isObject(%this.trigger) && %this.trigger.istriggerBricktrigger == true)
			{
				$InputTarget_["Self"] = %this;
				$InputTarget_["Vehicle"] = %obj;
				$InputTarget_["Driver(Client)"] = %obj.client;
				$InputTarget_["Driver(Player)"] = %obj;
				if(%this.getGroup().client.minigame)
					$InputTarget_["MiniGame"] = %this.getGroup().client.minigame;
				
				%this.processInputEvent("onVehicleEnterBrick",%obj.client);
			}
		}
		else
			Parent::onVehicleEnterBrick(%this, %obj);
	}
	
	function gameConnection::setScore(%this, %score)
	{
		%score = %this.FASTKartsLap;
		if(!isObject(%this.player))
			%score = -1;
		
		parent::setScore(%this, %score);
	}
	
	function Armor::onMount(%this, %obj, %col, %slot)
	{
		Parent::onMount(%this, %obj, %col, %slot);
		if(%obj.getMountNode() != 0)
			return;
		
		%client = %obj.client;
		if(!isObject(%client))
			return;
		
		%vehicle = %obj.getObjectMount();
		if(!isObject(%vehicle))
			return;
		
		%data = %vehicle.getDatablock();
		if(getSubStr(%data.uiname, 0, 9) $= "Speedkart" || getSubStr(%data.uiname, 0, 9) $= "SuperKart")
		{
			//color
			%color = FK_getRoundColorString();
			
			//uiname
			%text = "<just:right>" @ %color @ %data.uiname @ " ";
			
			//health
			%text = %text @ "<br>\c6Health" @ %color @ ": " @ %data.maxDamage @ " ";
			
			//speed
			%text = %text @ "<br>\c6Max Wheel Speed" @ %color @ ": " @ %data.maxWheelSpeed @ " ";
			
			//engine
			%text = %text @ "<br>\c6Engine Power" @ %color @ ": " @ %data.engineTorque @ " ";
			
			//brake
			%text = %text @ "<br>\c6Brake Power" @ %color @ ": " @ %data.brakeTorque @ " ";
			
			//density
			%text = %text @ "<br>\c6Drift Density" @ %color @ ": " @ %data.density @ " ";
			
			//description
			%text = %text @ "<br>\c6" @ %data.FKDescription @ " ";
			
			centerPrint(%client, %text, 12);
			%client.FK_LastKartUsed = %data;
		}
	}
	
	function disconnect()
	{
		//Stops the ticks just incase you're not hosting a dedicated server. You should still restart blockland just to be safe and to minimise problems.
		if(!$Server::Dedicated)
		{
			cancel($FK::Tick);
			cancel($FK::TipTick);
		}
		
		parent::disconnect();
	}
};
activatePackage(GameModeFASTKartsPackage);

//loaded when support_preferences is detected
package GameModeFASTKartsSupportPreferencesPackage
{
	function serverCmdUpdatePref(%client, %varname, %newvalue, %announce)
	{
		Parent::serverCmdUpdatePref(%client, %varname, %newvalue, %announce);
		
		if(strPos(%varname, "$FK::Pref::") >= 0)
			export("$FK::Pref::*", "config/server/FASTKarts/prefs.cs");
	}
};