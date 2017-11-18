function FK_BuildTrackList()
{
	%pattern = "Add-Ons/FASTKarts_*/save.bls";
	
	$FK::numTracks = 0;
	
	%file = findFirstFile(%pattern);
	while(%file !$= "")
	{
		$FK::Track[$FK::numTracks] = %file;
		
		%origin = "";
		%type = "";
		%configFileLocation = filePath(%file) @ "/config.txt";
		if(isFile(%configFileLocation))
		{
			%configFile = new fileObject();
			%configFile.openForRead(%configFileLocation);

			while(!%configFile.isEOF())
			{
				%line = trim(%configFile.readLine());

				if(%line $= "")
					continue;

				%firstWord = getWord(%line, 0);

				switch$(%firstWord)
				{
					case "origin":
						%origin = getWord(%line, getWordCount(%line)-1);
					case "type":
						%type = getWord(%line, getWordCount(%line)-1);
				}
			}
			
			%configFile.close();
			%configFile.delete();
		}
		
		if(%origin !$= "")
		{
			%origin = strReplace(%origin, "_", " ");
			$FK::TrackOrigin[$FK::numTracks] = %origin;
		}
		else
			$FK::TrackOrigin[$FK::numTracks] = "Unknown";

		if(%type $= "Campaign" || %type $= "Lapped" || %type $= "Battle") //battle is currently unused, does absolutely nothing
			$FK::TrackType[$FK::numTracks] = %type;
		else
			$FK::TrackType[$FK::numTracks] = "Campaign";
		
		$FK::numTracks++;
		%file = findNextFile(%pattern);
	}
}

function FK_NextTrack()
{
	SM_StopSong();
	
	echo("Loading next track...");
	$FK::ResetCount = 0;
	$FK::FinalLap = "?";
	
	if(($Pref::Server::FASTKarts::RandomTracks && !$FK::BypassRandom) || $FK::ForceRandom)
	{
		%i = getRandom(1, $FK::numTracks);

		if(%i <= $FK::CurrentTrack)
			%i--;
		
		$FK::CurrentTrack = %i - 1;
		$FK::ForceRandom = false;
	}
	$FK::BypassRandom = false;
	
	if(FK_areTracksRestricted())
	{
		for(%a = 0; %a < $FK::numTracks; %a++)
		{
			if(FK_trackCanLoad(%a))
			{
				%tracksExist = true;
				
				if(!%foundFirstTrack)
				{
					%firstToLoad = %a;
					%foundFirstTrack = true;
				}
				
				if(!%foundNextTrack)
				{
					%trackToLoad = %a;
					
					if(%a > $FK::CurrentTrack)
						%foundNextTrack = true;
				}
				
				%lastToLoad = %a;
			}
		}
		
		if(%tracksExist)
		{
			if($FK::CurrentTrack >= %lastToLoad && !%foundNextTrack)
				%trackToLoad = %firstToLoad;
			
			$FK::CurrentTrack = %trackToLoad - 1;
			$FK::BypassOrigin = false;
			$FK::BypassType = false;
		}
		else //should be impossible to get here but just in case
		{
			messageAll('', "\c5No FASTKarts tracks available!");
			messageAll('', "\c5You can find where tracks are hosted by typing this command into chat: \c3/download");
			return;
		}
	}
	
	$FK::CurrentTrack = mFloor($FK::CurrentTrack);
	$FK::CurrentTrack++;
	$FK::CurrentTrack = $FK::CurrentTrack % $FK::numTracks;
	
	$FK::VoteInProgress = false;
	$FK::VoteTrack = "";
	$FK::VoteNextRound = false;
	for(%a = 0; %a < $DefaultMinigame.numMembers; %a++)
	{
		%member = $DefaultMinigame.member[%a];
		if(isObject(%member))
			%member.FK_isRockingVote = false;
	}
	
	FK_LoadTrack_Phase1($FK::Track[$FK::CurrentTrack]);
}

function FK_LoadTrack_Phase1(%filename)
{
	//put everyone in observer mode
	%mg = $DefaultMiniGame;
	if(!isObject(%mg))
	{
		error("ERROR: FK_LoadTrack( " @ %filename  @ " ) - default minigame does not exist");
		return;
	}
	for(%i = 0; %i < %mg.numMembers; %i++)
	{
		%client = %mg.member[%i];
		%player = %client.player;
		if(isObject(%player))
			%player.delete();
		
		%camera = %client.camera;
		%camera.setFlyMode();
		%camera.mode = "Observer";
		%client.setControlObject(%camera);
	}
	
	//clear all bricks 
	// note: this function is deferred, so we'll have to set a callback to be triggered when it's done
	BrickGroup_888888.chaindeletecallback = "FK_LoadTrack_Phase2(\"" @ %filename @ "\");";
	BrickGroup_888888.chaindeleteall();
}

function FK_LoadTrack_Phase2(%filename)
{
	echo("Loading fastkarts track from " @ %filename);
	%loadMsg = "\c5Now loading " @ $FK::TrackType[$FK::CurrentTrack] SPC $FK::TrackOrigin[$FK::CurrentTrack] @ " track \c6" @ FK_getTrackName(%filename, 1);
	
	//load config file
	$FK::StartingLap = 1;
	$FK::trackMusic = "";
	$FK::trackCredits = "NONE";
	$FK::trackDescription = "NONE";
	$FK::trackEnvironment = 0;
	$FK::trackFile = %fileName;
	
	%configFilename = filePath(%fileName) @ "/config.txt";
	if(isFile(%configFilename))
	{
		%file = new fileObject();
		%file.openForRead(%configFilename);
		echo(" +- has config file");
		
		while(!%file.isEOF())
		{
			%line = trim(%file.readLine());

			if(%line $= "")
				continue;

			%firstWord = getWord(%line, 0);

			switch$(%firstWord)
			{
				case "startingLap":
					%startingLap = getWord(%line, getWordCount(%line) - 1);
				case "music":
					%music = getWord(%line, getWordCount(%line) - 1);
			}
		}

		if(%startingLap !$= "")
			$FK::StartingLap = mfloor(%startingLap);
		
		if(%music !$= "")
			$FK::trackMusic = %music;
		
		%file.close();
		%file.delete();
	}
	else
		echo(" +- no config file");
	
	//read and display credits file, if it exists
	// limited to one line
	%creditsFilename = filePath(%fileName) @ "/credits.txt";
	if(isFile(%creditsFilename))
	{
		%file = new FileObject();
		%file.openforRead(%creditsFilename);

		%line = %file.readLine();
		%line = stripMLControlChars(%line);
		if(%line !$= "")
		{
			echo(" +- has credits file");
			%loadMsg = %loadMsg @ "\c5, created by \c3" @ %line;
			$FK::trackCredits = "\c5Created by \c3" @ %line;
		}
		else
			echo(" +- has credits file, but it's blank");
		
		%file.close();
		%file.delete();
	}
	else
		echo(" +- no credits file");
	
	messageAll('', %loadMsg @ "\c5.");
	
	//read and display description file, if it exists
	// limited to one line
	//blockland glass forces descriptions to be in addons, sigh
	%descriptionFilename = filePath(%fileName) @ "/description.txt";
	if(isFile(%descriptionFilename))
	{
		%file = new FileObject();
		%file.openforRead(%descriptionFilename);

		%line = %file.readLine();
		%line = stripMLControlChars(%line);
		if(%line !$= "")
		{
			echo(" +- has description file");
			messageAll('', "\c5" @ %line);
			$FK::trackDescription = "\c5" @ %line;
		}
		else
			echo(" +- has description file, but it's blank");

		%file.close();
		%file.delete();
	}
	else
		echo(" +- no description file");
	
	//load environment if it exists
	%envFile = filePath(%fileName) @ "/environment.txt"; 
	if(isFile(%envFile))
	{  
		//echo("parsing env file " @ %envFile);
		//usage: GameModeGuiServer::ParseGameModeFile(%filename, %append);
		//if %append == 0, all minigame variables will be cleared 
		%res = GameModeGuiServer::ParseGameModeFile(%envFile, 1);
		echo(" +- has environment file");
		
		EnvGuiServer::getIdxFromFilenames();
		EnvGuiServer::SetSimpleMode();
		$FK::trackEnvironment = 1;
		
		if(!$EnvGuiServer::SimpleMode)     
		{
			EnvGuiServer::fillAdvancedVarsFromSimple();
			EnvGuiServer::SetAdvancedMode();
		}
	}
	else
		echo(" +- no environment file");
	
	//load save file
	schedule(10, 0, serverDirectSaveFileLoad, %fileName, 3, "", 2, 1);
}

function FK_getTrackName(%displayName, %fromPath)
{
	if(!%fromPath)
		%displayName = $FK::Track[%displayName];
	
	%displayName = strReplace(%displayName, "Add-Ons/FASTKarts_", "");
	%displayName = strReplace(%displayName, "/save.bls", "");
	%displayName = strReplace(%displayName, "_", " ");
	
	return %displayName;
}

function FK_getTrackTypesAllowed()
{
	%types = $Pref::Server::FASTKarts::CampaignTrackType + $Pref::Server::FASTKarts::LappedTrackType + $Pref::Server::FASTKarts::BattleTrackType;
	return %types;
}

function FK_isAllowedType(%type)
{
	if($FK::BypassType)
		return true;
	
	if(FK_getTrackTypesAllowed() <= 0) //just in case they disable all the track type prefs
	{
		error("ERROR: All allowed track types were disabled. Reenabling them all...");
		$Pref::Server::FASTKarts::CampaignTrackType = true;
		$Pref::Server::FASTKarts::LappedTrackType = true;
		$Pref::Server::FASTKarts::BattleTrackType = true;
	}
	
	if(%type $= "Campaign")
	{
		if($Pref::Server::FASTKarts::CampaignTrackType)
			return true;
	}
	else if(%type $= "Lapped")
	{
		if($Pref::Server::FASTKarts::LappedTrackType)
			return true;
	}
	else if(%type $= "Battle")
	{
		if($Pref::Server::FASTKarts::BattleTrackType)
			return true;
	}
	
	return false;
}

function FK_getTrackOriginsAllowed()
{
	%origins = $Pref::Server::FASTKarts::SpeedKartTracks + $Pref::Server::FASTKarts::SuperKartTracks + $Pref::Server::FASTKarts::FASTKartsTracks + $Pref::Server::FASTKarts::OtherTracks;
	return %origins;
}

function FK_isAllowedOrigin(%origin)
{
	if($FK::BypassOrigin)
		return true;
	
	if(FK_getTrackOriginsAllowed() <= 0) //just in case they disable all the track origin prefs
	{
		error("ERROR: All allowed track origins were disabled. Reenabling them all...");
		$Pref::Server::FASTKarts::SpeedKartTracks = true;
		$Pref::Server::FASTKarts::SuperKartTracks = true;
		$Pref::Server::FASTKarts::FASTKartsTracks = true;
		$Pref::Server::FASTKarts::OtherTracks = true;
	}
	
	if(%origin $= "SpeedKart")
	{
		if($Pref::Server::FASTKarts::SpeedKartTracks)
			return true;
	}
	else if(%origin $= "SuperKart")
	{
		if($Pref::Server::FASTKarts::SuperKartTracks)
			return true;
	}
	else if(%origin $= "FASTKarts")
	{
		if($Pref::Server::FASTKarts::FASTKartsTracks)
			return true;
	}
	else
	{
		if($Pref::Server::FASTKarts::OtherTracks)
			return true;
	}
	
	return false;
}

function FK_trackCanLoad(%num)
{
	%origin = $FK::TrackOrigin[%num];
	%type = $FK::TrackType[%num];
	
	if(FK_isAllowedOrigin(%origin) && FK_isAllowedType(%type))
		return true;
	
	return false;
}

function FK_areTracksRestricted()
{
	if(FK_getTrackTypesAllowed() < 3 && FK_getTrackOriginsAllowed() < 4)
		return true;
	
	return false;
}