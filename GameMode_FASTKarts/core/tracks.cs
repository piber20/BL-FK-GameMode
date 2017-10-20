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
			$FK::TrackOrigin[$FK::numTracks] = "FASTKarts";

		if(%type !$= "Campaign" && %type !$= "Lapped" && %type !$= "Battle")
		{
			%type = strReplace(%type, "_", " ");
			$FK::TrackType[$FK::numTracks] = %type;
		}
		else
			$FK::TrackType[$FK::numTracks] = "Campaign";
		
		$FK::numTracks++;
		%file = findNextFile(%pattern);
	}
}

function FK_NextTrack()
{
	echo("Loading next track...");
	$FK::ResetCount = 0;
	
	if(($Pref::Server::FASTKarts::RandomTracks && !$FK::BypassRandom) || $FK::ForceRandom)
	{
		%i = getRandom(1, $FK::numTracks);

		if(%i <= $FK::CurrentTrack)
			%i--;
		
		$FK::CurrentTrack = %i - 1;
		$FK::ForceRandom = false;
	}
	$FK::BypassRandom = false;
	
	if($Pref::Server::FASTKarts::ForceTrackOrigin > 0 && !$FK::BypassOrigin && $Pref::Server::FASTKarts::ForceTrackType > 0 && !$FK::BypassType)
	{
		for(%a = 0; %a < $FK::numTracks; %a++)
		{
			if($Pref::Server::FASTKarts::ForceTrackOrigin == 1)
				%origin = "SpeedKart";
			if($Pref::Server::FASTKarts::ForceTrackOrigin == 2)
				%origin = "SuperKart";
			if($Pref::Server::FASTKarts::ForceTrackOrigin == 3)
				%origin = "FASTKarts";
			
			if($Pref::Server::FASTKarts::ForceTrackType == 1)
				%type = "Campaign";
			if($Pref::Server::FASTKarts::ForceTrackType == 2)
				%type = "Lapped";
			if($Pref::Server::FASTKarts::ForceTrackType == 3)
				%type = "Battle";
			
			if($FK::TrackOrigin[%a] $= %origin && $FK::TrackType[%a] $= %type)
			{
				%allExists = true;
				
				if(!%allFoundFirstTrack)
				{
					%allFirstToLoad = %a;
					%allFoundFirstTrack = true;
				}
				
				if(!%allFoundNextTrack)
				{
					%allTrackToLoad = %a;
					
					if(%a > $FK::CurrentTrack)
						%allFoundNextTrack = true;
				}
				
				%allLastToLoad = %a;
			}
		}
		
		if(%allExists)
		{
			if($FK::CurrentTrack >= %allLastToLoad && !%allFoundNextTrack)
				%allTrackToLoad = %allFirstToLoad;
			
			$FK::CurrentTrack = %allTrackToLoad - 1;
			$FK::BypassOrigin = false;
			$FK::BypassType = false;
		}
		else
		{
			error("ERROR: No tracks have origin " @ %origin @ " AND the " @ %type @ " type. Setting type pref to 0");
			$Pref::Server::FASTKarts::ForceTrackType = 0;
			FK_NextTrack();
			return;
		}
	}
	else
	{
		if($Pref::Server::FASTKarts::ForceTrackOrigin !$= "" && !$FK::BypassOrigin)
		{
			for(%a = 0; %a < $FK::numTracks; %a++)
			{
				if($Pref::Server::FASTKarts::ForceTrackOrigin == 1)
					%origin = "SpeedKart";
				if($Pref::Server::FASTKarts::ForceTrackOrigin == 2)
					%origin = "SuperKart";
				if($Pref::Server::FASTKarts::ForceTrackOrigin == 3)
					%origin = "FASTKarts";
				
				if($FK::TrackOrigin[%a] $= %origin)
				{
					%originExists = true;
					
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
			
			if(%originExists)
			{
				if($FK::CurrentTrack >= %lastToLoad && !%foundNextTrack)
					%trackToLoad = %firstToLoad;
				
				$FK::CurrentTrack = %trackToLoad - 1;
			}
			else
			{
				error("ERROR: No tracks are in origin " @ %origin @ ". Setting origin pref to 0");
				$Pref::Server::FASTKarts::ForceTrackOrigin = 0;
			}
		}
		
		$FK::BypassOrigin = false;
		
		if($Pref::Server::FASTKarts::ForceTrackType !$= "" && !$FK::BypassType)
		{
			for(%b = 0; %b < $FK::numTracks; %b++)
			{
				if($Pref::Server::FASTKarts::ForceTrackType == 1)
					%type = "Campaign";
				if($Pref::Server::FASTKarts::ForceTrackType == 2)
					%type = "Lapped";
				if($Pref::Server::FASTKarts::ForceTrackType == 3)
					%type = "Battle";
				
				if($FK::TrackType[%b] $= %type)
				{
					%typeExists = true;
					
					if(!%typeFoundFirstTrack)
					{
						%typeFirstToLoad = %b;
						%typeFoundFirstTrack = true;
					}
					
					if(!%typeFoundNextTrack)
					{
						%typeTrackToLoad = %b;
						
						if(%b > $FK::CurrentTrack)
							%typeFoundNextTrack = true;
					}
					
					%typeLastToLoad = %b;
				}
			}
			
			if(%typeExists)
			{
				if($FK::CurrentTrack >= %typeLastToLoad && !%typeFoundNextTrack)
					%typeTrackToLoad = %typeFirstToLoad;
				
				$FK::CurrentTrack = %typeTrackToLoad - 1;
			}
			else
			{
				error("ERROR: No tracks have the " @ %type @ " type. Setting type pref to 0");
				$Pref::Server::FASTKarts::ForceTrackType = 0;
			}
		}
		
		$FK::BypassType = false;
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
	%loadMsg = "\c5Now loading " @ $FK::TrackTheme[$FK::CurrentTrack] SPC $FK::TrackSort[$FK::CurrentTrack] @ " track \c6" @ FK_getTrackName(%filename, 1);
	
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