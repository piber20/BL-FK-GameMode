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
			//default track config
			$FK::StartingLap = 1;
			$FK::TrackMusic = "";
			$FK::trackCredits = "NONE";
			$FK::trackDescription = "NONE";
			$FK::trackEnvironment = 0;
			$FK::trackFile = "Add-Ons/GameMode_FASTKarts/save.bls";
			
			//message
			messageAll('', "\c5No FASTKarts tracks available!");
			messageAll('', "\c5You can find where tracks are hosted by typing this command into chat: \c3/download");
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
	
	//record players who died if they never entered a kart and kick them
	function gameConnection::onDeath(%this, %killerPlayer, %killer, %damageType, %a)
	{
		parent::onDeath(%this, %killerPlayer, %killer, %damageType, %a);
		
		if(%this.minigame != $DefaultMinigame)
			return;
		
		if(%this.isAdmin)
			return;
		
		if(%this.FK_LastKartUsed $= "" && $FK::RoundType !$= "BOUNCY")
			%this.FK_RoundIdleCounter++;
		
		if($Pref::Server::FASTKarts::KickIdlePlayers)
		{
			if(%this.FK_RoundIdleCounter >= 3 && %this.FK_RoundIdleWarned)
			{
				MessageAll('',"\c3" @ %this.getPlayerName() @ "\c2(ID:" SPC %this.getBLID() @ ") was kicked for being idle for 3 rounds.");
				%this.delete("You were kicked from the server for being <a:www.urbandictionary.com/define.php?term=Afk>AFK</a>.\nYou may rejoin at any time.");
			}
			else if(%this.FK_RoundIdleCounter >= 2)
			{
				commandToClient(%this,'MessageBoxOK',"Alert!","You will be kicked from the game if you die again without entering a kart.");
				%this.FK_RoundIdleWarned = true;
			}
		}
	}
	
	//when vehicle spawns, it cannot move (event must enable it)
	//this solves the driving through the garage problem
	function WheeledVehicleData::onAdd(%data,%obj)
	{
		Parent::onAdd(%data, %obj);
		
		for(%i = 0; %i < %data.numWheels; %i++)
		{
			if($FK::TrackStarted) //karts spawned after the race begins will be powered
				%obj.setWheelPowered(%i, 1);
			else
				%obj.setWheelPowered(%i, 0);
		}
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
			else if(!%vehicle.getWheelPowered(2) || %elapsedTime < $FK::SuicideBufferTime)
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
					
					//give rockets if it's a rocket round
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
					
					//music
					if($Pref::Server::FASTKarts::EnableGlobalMusic)
					{
						%music = "";
						if($Pref::Server::FASTKarts::ObeyTrackMusic == 2 || ($Pref::Server::FASTKarts::ObeyTrackMusic == 1 && $FK::ResetCount <= 1))
							%music = $FK::TrackMusic;
						
						%play = "musicData_" @ %music;
						%play = strReplace(%play, "-", "DASH");
						%play = strReplace(%play, "'", "APOS");
						
						if(!isObject(%play) || %music $= "")
						{
							for(%b = 0; %b < DataBlockGroup.getCount(); %b++)
							{
								%datablock = DataBlockGroup.getObject(%b);
								if(%datablock.getClassName() $= "AudioProfile" && %datablock.uiName !$= "")
									%song[%songCount++] = %datablock;
							}
							%randomsong = getRandom(1, %songCount);
							%play = %song[%randomsong];
						}
						
						SM_PlaySong("", %play);
					}
				}
			}
		}
	}
	
	function MiniGameSO::Reset(%obj, %client)
	{
		SM_StopSong();
		
		//make sure this value is an number
		$Pref::Server::FASTKarts::RoundLimit = mFloor($Pref::Server::FASTKarts::RoundLimit);

		//handle our race time hack
		%obj.raceStartTime = 0;

		//count number of minigame resets, when we reach the limit, go to next track
		if(%obj.numMembers >= 0)
			$FK::ResetCount++;
		
		//force round number to the last round to effectively stop track rotation
		if($FK::ResetCount > $Pref::Server::FASTKarts::RoundLimit && ($Pref::Server::FASTKarts::RoundLimit <= 0 || $FK::numTracks <= 0))
			$FK::ResetCount = $Pref::Server::FASTKarts::RoundLimit;

		if($FK::VoteNextRound)
		{
			$FK::CurrentTrack = $FK::VoteTrack - 1;
			$FK::BypassRandom = true;
			$FK::ResetCount = 0;
			$FK::onWinRaceList = "";
			FK_NextTrack();
		}
		else if($FK::ResetCount > $Pref::Server::FASTKarts::RoundLimit && ($Pref::Server::FASTKarts::RoundLimit > 0 && $FK::numTracks > 0))
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
			
			if($Pref::Server::FASTKarts::RoundLimit > 0 && $FK::numTracks > 0)
				messageAll('', "\c5Beginning round " @ $FK::ResetCount @ " of " @ $Pref::Server::FASTKarts::RoundLimit @ ".");
			else
				messageAll('', "\c5Beginning round.");
			messageAll('', $FK::RoundName);
			$FK::TrackStarted = false;
			$FK::TrackCompleted = false;
			$FK::FinalLapReached = false;
			Parent::Reset(%obj, %client);
			
			if($FK::numTracks == 0)
			{
				messageAll('', "\c5No FASTKarts tracks available!");
				messageAll('', "\c5You can find where tracks are hosted by typing this command into chat: \c3/download");
				return;
			}
			
			for(%a = 0; %a < $DefaultMinigame.numMembers; %a++)
			{
				%member = $DefaultMinigame.member[%a];
				if(isObject(%member))
				{
					%member.FASTKartsLap = 0;
					%member.FKWinner = false;
					%member.FK_isRockingVote = false;
					%member.FK_LastKartUsed = "";
					%member.FK_HasSpawnedSpeedKart = false;
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
				%weapon0 = emptyItem.getID();
				%weapon1 = 0;
				%weapon2 = 0;
				%weapon3 = 0;
				%weapon4 = 0;
				
				//get loadouts based on round type
				if($FK::RoundType $= "NORMAL" && $Pref::Server::FASTKarts::EnableNoveltyItems)
				{
					%weapon0 = BangGunItem.getID();
					%weapon1 = BananaItem.getID();
					%weapon2 = FlagItem.getID();
					%weapon3 = PortableCameraItem.getID();
					%weapon4 = YoyoItem.getID();
				}
				else if($FK::RoundType $= "ROCKET")
					%weapon0 = MegaNukeEmptyItem.getID(); //temp filler item for when the real weapon is added after the race starts
				
				//change playertype based on round type
				if($FK::RoundType $= "BOUNCY")
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
				
				//make sure players aren't insta-killed when the race starts
				if($Pref::Server::FASTKarts::NoKartKillTime > 0)
					%player.noKartKillTime = $Pref::Server::FASTKarts::NoKartKillTime * 10;
				
				%this.setScore();
			}
		}
	}
	
	function serverCmdmessageSent(%client, %text)
	{
		//repeat check
		%trimText = trim(%text);
		if(%client.lastChatText $= %trimText)
		{
			%chatDelta = (getSimTime() - %client.lastChatTime) / getTimeScale();
			if(%chatDelta < 15000.0)
			{
				%client.spamMessageCount = $SPAM_MESSAGE_THRESHOLD;
				messageClient(%client, '', '\c5Do not repeat yourself.');
			}
		}
		%client.lastChatTime = getSimTime();
		%client.lastChatText = %trimText;
		
		//player talk animation
		%player = %client.Player;
		if(isObject(%player))
		{
			%player.playThread(3, talk);
			%player.schedule(strlen(%text) * 50.0, playThread, 3, root);
		}
		
		//text filtering
		%text = chatWhiteListFilter(%text);
		%text = StripMLControlChars(%text);
		%text = trim(%text);
		if(strlen(%text) <= 0.0)
			return;
		if($Pref::Server::MaxChatLen > 0.0)
		{
			if(strlen(%text) >= $Pref::Server::MaxChatLen)
				%text = getSubStr(%text, 0, $Pref::Server::MaxChatLen);
		}
		
		//links
		%protocol = "http://";
		%protocolLen = strlen(%protocol);
		%urlStart = strpos(%text, %protocol);
		if(%urlStart == -1.0)
		{
			%protocol = "https://";
			%protocolLen = strlen(%protocol);
			%urlStart = strpos(%text, %protocol);
		}
		if(%urlStart == -1.0)
		{
			%protocol = "ftp://";
			%protocolLen = strlen(%protocol);
			%urlStart = strpos(%text, %protocol);
		}
		if(%urlStart != -1.0)
		{
			%urlEnd = strpos(%text, " ", %urlStart + 1.0);
			%skipProtocol = 0;
			if(%protocol $= "http://")
				%skipProtocol = 1;
			if(%urlEnd == -1.0)
			{
				%fullUrl = getSubStr(%text, %urlStart, strlen(%text) - %urlStart);
				%url = getSubStr(%text, %urlStart + %protocolLen, strlen(%text) - %urlStart - %protocolLen);
			}
			else
			{
				%fullUrl = getSubStr(%text, %urlStart, %urlEnd - %urlStart);
				%url = getSubStr(%text, %urlStart + %protocolLen, %urlEnd - %urlStart - %protocolLen);
			}
			if(strlen(%url) > 0.0)
			{
				%url = strreplace(%url, "<", "");
				%url = strreplace(%url, ">", "");
				if (%skipProtocol)
					%newText = strreplace(%text, %fullUrl, "<a:" @ %url @ ">" @ %url @ "</a>\c6");
				else
					%newText = strreplace(%text, %fullUrl, "<a:" @ %protocol @ %url @ ">" @ %url @ "</a>\c6");
				%text = %newText;
			}
		}
		
		//etard
		if($Pref::Server::ETardFilter)
		{
			if(!chatFilter(%client, %text, $Pref::Server::ETardList, '\c5This is a civilized game.  Please use full words.'))
				return;
		}
		
		//get round color
		%color = FK_getRoundColorString();
		
		//chat emotes compatability
		if(isFunction(peReplace))
			%text = peReplace(%text, "\c6");
		
		//send the message
		chatMessageAll(%client, '\c7%1%2%3\c7%4\c6: %5', %client.clanPrefix, %color, %client.getPlayerName(), %client.clanSuffix, %text);
		echo(%client.getSimpleName() @ ": " @ %text);
	}
	
	function serverCmdTeamMessageSent(%client, %text)
	{
		//there are no teams in fastkarts, so we simply copy the normal chat but make it blue! amazing!
		
		//repeat check
		%trimText = trim(%text);
		if(%client.lastChatText $= %trimText)
		{
			%chatDelta = (getSimTime() - %client.lastChatTime) / getTimeScale();
			if(%chatDelta < 15000.0)
			{
				%client.spamMessageCount = $SPAM_MESSAGE_THRESHOLD;
				messageClient(%client, '', '\c5Do not repeat yourself.');
			}
		}
		%client.lastChatTime = getSimTime();
		%client.lastChatText = %trimText;
		
		//player talk animation
		%player = %client.Player;
		if(isObject(%player))
		{
			%player.playThread(3, talk);
			%player.schedule(strlen(%text) * 50.0, playThread, 3, root);
		}
		
		//text filtering
		%text = chatWhiteListFilter(%text);
		%text = StripMLControlChars(%text);
		%text = trim(%text);
		if(strlen(%text) <= 0.0)
			return;
		if($Pref::Server::MaxChatLen > 0.0)
		{
			if(strlen(%text) >= $Pref::Server::MaxChatLen)
				%text = getSubStr(%text, 0, $Pref::Server::MaxChatLen);
		}
		
		//links
		%protocol = "http://";
		%protocolLen = strlen(%protocol);
		%urlStart = strpos(%text, %protocol);
		if(%urlStart == -1.0)
		{
			%protocol = "https://";
			%protocolLen = strlen(%protocol);
			%urlStart = strpos(%text, %protocol);
		}
		if(%urlStart == -1.0)
		{
			%protocol = "ftp://";
			%protocolLen = strlen(%protocol);
			%urlStart = strpos(%text, %protocol);
		}
		if(%urlStart != -1.0)
		{
			%urlEnd = strpos(%text, " ", %urlStart + 1.0);
			%skipProtocol = 0;
			if(%protocol $= "http://")
				%skipProtocol = 1;
			if(%urlEnd == -1.0)
			{
				%fullUrl = getSubStr(%text, %urlStart, strlen(%text) - %urlStart);
				%url = getSubStr(%text, %urlStart + %protocolLen, strlen(%text) - %urlStart - %protocolLen);
			}
			else
			{
				%fullUrl = getSubStr(%text, %urlStart, %urlEnd - %urlStart);
				%url = getSubStr(%text, %urlStart + %protocolLen, %urlEnd - %urlStart - %protocolLen);
			}
			if(strlen(%url) > 0.0)
			{
				%url = strreplace(%url, "<", "");
				%url = strreplace(%url, ">", "");
				if (%skipProtocol)
					%newText = strreplace(%text, %fullUrl, "<a:" @ %url @ ">" @ %url @ "</a>\c6");
				else
					%newText = strreplace(%text, %fullUrl, "<a:" @ %protocol @ %url @ ">" @ %url @ "</a>\c6");
				%text = %newText;
			}
		}
		
		//etard
		if($Pref::Server::ETardFilter)
		{
			if(!chatFilter(%client, %text, $Pref::Server::ETardList, '\c5This is a civilized game.  Please use full words.'))
				return;
		}
		
		//get round color
		%color = FK_getRoundColorString();
		
		//chat emotes compatability
		if(isFunction(peReplace))
			%text = peReplace(%text, "\c4");
		
		//send the message
		chatMessageAll(%client, '\c7%1%2%3\c7%4\c4: %5', %client.clanPrefix, %color, %client.getPlayerName(), %client.clanSuffix, %text);
		echo("(T) " @ %client.getSimpleName() @ ": " @ %text);
	}
	
	function fxDTSBrick::onPlayerEnterBrick(%this, %obj)
	{
		Parent::onPlayerEnterBrick(%this, %obj);
		
		%client = %obj.client;
		if(!isObject(%client))
			return;
		
		if(%this.checkHasOnWinRaceEvent())
		{
			if($FK::RoundType $= "BOUNCY" || $Pref::Server::FASTKarts::VehiclelessWins)
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
			
			//drag
			%text = %text @ "<br>\c6Drag" @ %color @ ": " @ %data.drag @ " ";
			
			//description
			%text = %text @ "<br>\c6" @ %data.FKDescription @ " ";
			
			centerPrint(%client, %text, 12);
			%client.FK_LastKartUsed = %data;
			%client.FK_RoundIdleCounter = 0;
		}
	}
	
	function serverCmdleaveMiniGame(%client)
	{
		Parent::serverCmdleaveMiniGame(%client);
		bottomPrint(%client, "", 0, true);
		centerPrint(%client, "", 0);
	}
	
	function disconnect()
	{
		//Stops the ticks just incase you're not hosting a dedicated server. You should still restart blockland just to be safe and to minimise problems.
		if(!$Server::Dedicated)
		{
			cancel($FK::Tick);
			cancel($FK::TipTick);
			$FK::Initialized = "";
		}
		
		return parent::disconnect();
	}
};
activatePackage(GameModeFASTKartsPackage);