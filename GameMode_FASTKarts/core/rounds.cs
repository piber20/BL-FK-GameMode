function FK_ResetSuicideGambling()
{
	//scramble the suicide prevention timer, so people can't game it with client scripts
	$FK::SuicideBufferTime = 6000 + getRandom() * 4000;
	//clear suicide gambling winner
	$FK::LastSuicider = 0;
	
	if(isEventPending($FK::SuicideGamblingEvent))
		cancel($FK::SuicideGamblingEvent);
	$FK::SuicideGamblingEvent = 0;
}

// more time to fly out of a burning car
$CorpseTimeoutValue = 7000;

//special event to explode vehicles that are left in the garage
registerOutputEvent("fxDTSBrick", "explodeNearVehicle", "");
function fxDTSBrick::explodeNearVehicle(%obj)
{
	%vehicle = %obj.vehicle;
	if(!isObject(%vehicle))
		return;
	
	%delta = vectorSub(%vehicle.getPosition(), %obj.getPosition());
	//echo("len = " @ vectorLen(%delta));
	if(vectorLen(%delta) < 5) //7.5)
	{
		%vehicle.finalExplosion(); //damage(%vehicle, %vehicle.getPosition(), 10000, $DamageType::Default);
		%client = %vehicle.getControllingClient();
		if(isObject(%client))
			%client.FK_LastKartUsed = ""; //so the idle kicker will kick people who are intentionally stalling the server by spawning a kart, going in it, and not moving
	}
}

//special event to win the race, displays race time
function GameConnection::winRace(%client, %laps)
{
	$FK::FinalLap = %laps;
	%mg = %client.miniGame;

	if(!isObject(%mg))
		%mg = $DefaultMiniGame;

	if(!isObject(%mg))
		return;

	%player = %client.player;
	if(!isObject(%player))
		return;
	
	%bypass = true;
	%vehicle = %player.getObjectMount();
	if(!isObject(%vehicle))
	{
		%bypass = false;
		
		if($FK::RoundType $= "BOUNCY")
			%bypass = true;
		
		if($Pref::Server::FASTKarts::VehiclelessWins && %client.FASTKartsLap == %laps)
			%bypass = true;
	}
	
	if(!%bypass)
		return;
	
	%laps = mfloor(%laps);
	if(%laps < 1)
		%laps = 1;
	
	if(isObject(%vehicle) && %vehicle.FASTKartsLap <= $FK::StartingLap)
		%vehicle.FASTKartsLap = $FK::StartingLap;
	if(%client.FASTKartsLap <= $FK::StartingLap)
		%client.FASTKartsLap = $FK::StartingLap;
	
	if($FK::RoundType $= "BOUNCY")
	{
		%client.FASTKartsLap++;
	}
	else
	{
		%vehicle.FASTKartsLap++;
		%client.FASTKartsLap = %vehicle.FASTKartsLap;
	}
	if(%client.FASTKartsLap > %laps)
	{
		SM_StopSong();
		$FK::TrackCompleted = true;
		%client.FKWinner = true;
		%mg.chatMessageAll(0, "\c3" @ %client.getPlayerName() @ " \c5WON THE RACE IN \c6" @ FK_getTimeLeft(%mg, true) @ "\c5!");
		echo(%client.getPlayerName() @ " won the race");
		FK_doTimeRecord(%mg);
		%mg.scheduleReset(7000);
		
		//get input event ready
		for(%i = 0; %i < getWordCount($FK::onWinRaceList); %i++)
		{
			%brick = getWord($FK::onWinRaceList, %i);
			if(isObject(%brick))
			{
				$inputTarget_Self = %brick;
				$inputTarget_Player = %player;
				$inputTarget_Client = %client;
				$inputTarget_Minigame = %mg;
				%brick.processInputEvent("onWinRace", %client);
			}
		}
	}
	else
	{
		if(%client.FASTKartsLap == %laps)
		{
			if(!$FK::FinalLapReached)
			{
				$FK::FinalLapReached = true;
				%mg.chatMessageAll(0, "\c3" @ %client.getPlayerName() @ " \c5reached the final lap!");
				if($Pref::Server::FASTKarts::EnableGlobalMusic)
					SM_PlaySong("", $SM::MusicPlaying, false, 1.2);
			}
		}
		
		if(%client.FASTKartsLap != 1)
			messageClient(%client, '', "\c5Completed lap " @ %client.FASTKartsLap - 1 @ " of " @ %laps @ ".");
		else
			messageClient(%client, '', "\c5Started race.");
		
		echo(%client.getPlayerName() @ " completed a lap");
	}
	
	%client.setScore();
}
registerOutputEvent("GameConnection", "winRace", "int 1 99 50");
registerInputEvent("fxDtsBrick", "onWinRace", "Self fxDtsBrick" TAB "Player player" TAB "Client gameConnection" TAB "Minigame minigame");

function fxDTSBrick::checkHasOnWinRaceEvent(%this)
{
	if(!isObject(%this))
	{
		$FK::onWinRaceList = removeItemFromList($FK::onWinRaceList, %this);
		return;
	}
	if(%this.numEvents <= 0)
	{
		$FK::onWinRaceList = removeItemFromList($FK::onWinRaceList, %this);
		return;
	}
	
	for(%i = 0; %i < %this.numEvents; %i++)
	{
		%input = %this.eventInput[%i];
		if(%input $= "onWinRace")
			%onWinRaceEvents++;
		
		%output = %this.eventOutput[%i];
		if(%output $= "winRace")
		{
			%laps = %this.eventOutputParameter[%i, 1];
			$FK::FinalLap = %laps;
		}
	}
	
	if(%onWinRaceEvents > 0)
	{
		$FK::onWinRaceList = addItemToList($FK::onWinRaceList, %this);
		return true;
	}
	else
	{
		$FK::onWinRaceList = removeItemFromList($FK::onWinRaceList, %this);
		return false;
	}
}

function FK_getTimeLeft(%mg, %win)
{
	//if race start time is not available, use time since last reset
	%startTime = %mg.raceStartTime;
	if(%startTime <= 0)
		%startTime = %mg.lastResetTime;

	%elapsedTime = getSimTime() - %startTime;
	if(%win)
		$FK::WinnerTime = %elapsedTime;
	
	if(%win && $Pref::Server::FASTKarts::ShowMilliseconds)
		%string = FK_getFullTimeString(%elapsedTime);
	else
	{
		%elapsedTime = mFloor(%elapsedTime / 1000);
		%string = getTimeString(%elapsedTime);
	}
	
	return %string;
}

function FK_getFullTimeString(%time)
{
	%seconds = mFloor(%time / 1000);
	%string = getTimeString(%seconds);
	
	%microSeconds = %time - (%seconds * 1000);
	if(%microSeconds < 10)
		%string = %string @ ".00" @ %microSeconds;
	else if(%microSeconds < 100)
		%string = %string @ ".0" @ %microSeconds;
	else
		%string = %string @ "." @ %microSeconds;
	
	return %string;
}

function FK_doTimeRecord(%mg)
{
	for(%a = 0; %a < %mg.numMembers; %a++)
	{
		%member = %mg.member[%a];
		if(isObject(%member))
		{
			if(%member.FKWinner)
			{
				$FK::WinnerBLID = %member.bl_id;
				$FK::WinnerName = %member.getPlayerName();
				if(isObject(%member.FK_LastKartUsed))
					$FK::WinnerKart = %member.FK_LastKartUsed.getName();
			}
		}
	}
	if($FK::WinnerBLID $= "")
		return;
	if($FK::WinnerName $= "")
		return;
	if($FK::WinnerTime $= "")
		return;
	
	%trackName = FK_getTrackName($FK::CurrentTrack);
	%trackName = strReplace(%trackName, " ", "");
	
	%oldRecord = $FK::Record_[%trackName, $FK::RoundType, "Time"];
	
	%newRecord = false;
	if(%oldRecord $= "" || %oldRecord == 0)
		%newRecord = true;
	else if(%oldRecord > $FK::WinnerTime)
		%newRecord = true;
	
	if(%newRecord)
	{
		if($Pref::Server::FASTKarts::AnnounceRecords)
		{
			if($Pref::Server::FASTKarts::ShowMilliseconds)
				%string = FK_getFullTimeString(%oldRecord);
			else
			{
				%oldSeconds = mFloor(%oldRecord / 1000);
				%string = getTimeString(%oldSeconds);
			}
			
			if(FK_getRoundTypesAllowed() > 1)
			{
				if($FK::RoundType $= "NORMAL")
				{
					%typeString = " in the \c3Normal\c5 round type";
					%color = "\c3";
				}
				if($FK::RoundType $= "ROCKET")
				{
					%typeString = " in the \c0Rocket\c5 round type";
					%color = "\c0";
				}
				if($FK::RoundType $= "BOUNCY")
				{
					%typeString = " in the \c2Bouncy\c5 round type";
					%color = "\c2";
				}
			}
			else
				%typeString = "";
			
			%oldBLID = $FK::Record_[%trackName, $FK::RoundType, "PlayerBLID"];
			if(%oldRecord $= "" || %oldRecord == 0)
				%mg.chatMessageAll(0, "\c5They set a new server-wide record" @ %typeString @ "!");
			else if(%oldBLID $= "")
				%mg.chatMessageAll(0, "\c5They beat the server-wide record of " @ %color @ %string @ "\c5" @ %typeString @ "!");
			else if(%oldBLID == $FK::WinnerBLID)
				%mg.chatMessageAll(0, "\c5They beat their own server-wide record of " @ %color @ %string @ "\c5" @ %typeString @ "!");
			else
			{
				%oldName = $FK::Record_[%trackName, $FK::RoundType, "PlayerName"];
				
				if(%oldName $= "")
					%mg.chatMessageAll(0, "\c5They beat \c1BL_ID: " @ %oldBLID @ "\c5's server-wide record of " @ %color @ %string @ "\c5" @ %typeString @ "!");
				else
					%mg.chatMessageAll(0, "\c5They beat " @ %color @ %oldName @ "\c5's server-wide record of " @ %color @ %string @ "\c5" @ %typeString @ "!");
			}
		}
		
		$FK::Record_[%trackName, $FK::RoundType, "Time"] = $FK::WinnerTime;
		$FK::Record_[%trackName, $FK::RoundType, "PlayerName"] = $FK::WinnerName;
		$FK::Record_[%trackName, $FK::RoundType, "PlayerBLID"] = $FK::WinnerBLID;
		if(isObject($FK::WinnerKart))
			$FK::Record_[%trackName, $FK::RoundType, "Kart"] = $FK::WinnerKart;
		
		export("$FK::Record_*", "config/server/FASTKarts/records.cs");
		
		$FK::WinnerTime = "";
		$FK::WinnerName = "";
		$FK::WinnerBLID = "";
		$FK::WinnerKart = "";
	}
}

function FK_DetermineRoundType()
{
	if(FK_getRoundTypesAllowed() == 0)
		$Pref::Server::FASTKarts::NormalRounds = true;
	
	if(FK_getRoundTypesAllowed() > 1)
	{
		//force the round to be normal if it wasnt normal before
		if($FK::RoundType !$= "NORMAL" && $Pref::Server::FASTKarts::NormalRounds)
			%random = 0;
		else //determine what the randomiser can pick
			%random = getRandom(0, FK_getRoundTypesAllowed() - 1);
		
		%types = -1;
		if($Pref::Server::FASTKarts::NormalRounds)
			%round[%types++] = 0;
		if($Pref::Server::FASTKarts::RocketRounds)
			%round[%types++] = 1;
		if($Pref::Server::FASTKarts::BouncyRounds)
			%round[%types++] = 2;
		
		//apply the round type
		if(%round[%random] == 0)
			$FK::RoundType = "NORMAL";
		else if(%round[%random] == 1)
			$FK::RoundType = "ROCKET";
		else if(%round[%random] == 2)
			$FK::RoundType = "BOUNCY";
	}
	else
	{
		if($Pref::Server::FASTKarts::NormalRounds)
			$FK::RoundType = "NORMAL";
		else if($Pref::Server::FASTKarts::RocketRounds)
			$FK::RoundType = "ROCKET";
		else if($Pref::Server::FASTKarts::BouncyRounds)
			$FK::RoundType = "BOUNCY";
	}
	
	if($FK::RoundType $= "NORMAL")
		$FK::RoundName = "\c3Normal round.";
	else if($FK::RoundType $= "ROCKET")
		$FK::RoundName = "\c0Rockets!";
	else if($FK::RoundType $= "BOUNCY")
		$FK::RoundName = "\c2Bouncy players!";
	
	//fake client stuff to change fall damage setting
	if(!isObject(FK_FakeClient))
		new GameConnection(FK_FakeClient);
	
	FK_FakeClient.isAdmin = 1;
	FK_FakeClient.isSuperAdmin = 1;
	FK_FakeClient.minigame = $DefaultMinigame;
	
	serverCmdSetMiniGameData(FK_FakeClient, "BD" SPC 0); //brick damage, will be turned on when the race starts
	
	if($FK::RoundType $= "BOUNCY")
		serverCmdSetMiniGameData(FK_FakeClient, "FD" SPC 0); //fall damage
	else
		serverCmdSetMiniGameData(FK_FakeClient, "FD" SPC 1);
}

function FK_getRoundTypesAllowed()
{
	%types = $Pref::Server::FASTKarts::NormalRounds + $Pref::Server::FASTKarts::RocketRounds + $Pref::Server::FASTKarts::BouncyRounds;
	return %types;
}

function FK_getRoundColorString()
{
	if($FK::RoundType $= "ROCKET")
		return "\c0";
	else if($FK::RoundType $= "BOUNCY")
		return "\c2";
	else //if($FK::RoundType $= "NORMAL")
		return "\c3";
}

function FK_getRoundShapeNameColor()
{
	if($FK::RoundType $= "ROCKET")
		return "1 0 0 1";
	else if($FK::RoundType $= "BOUNCY")
		return "0 1 0 1";
	else //if($FK::RoundType $= "NORMAL")
		return "1 1 0 1";
}