//core files
exec("./tracks.cs");
exec("./karts.cs");
exec("./commands.cs");
exec("./prefCommands.cs");
exec("./rounds.cs");
exec("./package.cs");
exec("./achievements.cs");
exec("./help.cs");
exec("./debug.cs");

function FK_Tick()
{
	cancel($FK::Tick);
	
	for(%a = 0; %a < $DefaultMinigame.numMembers; %a++)
	{
		%member = $DefaultMinigame.member[%a];
		if(isObject(%member))
		{
			%member.FK_setBottomPrintInfo();
			
			%player = %member.player;
			if(isObject(%player))
				%player.setShapeNameColor(FK_getRoundShapeNameColor());
		}
	}
	
	//catch if a pref preset wants to load and then apply it
	if($FK::Preset::Defaults)
	{
		$FK::Preset::Defaults = false;
		FK_SetDefaultPrefs();
		export("$FK::Pref::*", "config/server/FASTKarts/prefs.cs");
	}
	else if($FK::Preset::SpeedKart)
	{
		$FK::Preset::SpeedKart = false;
		
		echo("=== Applying I <3 SpeedKart FASTKarts pref preset ===");
		
		$FK::Pref::Rounds::PerTrack = 8;
		$FK::Pref::Rounds::AllowNormal = true;
		$FK::Pref::Rounds::AllowRocket = false;
		$FK::Pref::Rounds::AllowBouncy = false;
		
		$FK::Pref::Tracks::LoadRandomly = false;
		$FK::Pref::Tracks::Voting = 0;
		
		$FK::Pref::Gameplay::Achievements = false;
		$FK::Pref::Gameplay::CrumbleDeath = false;
		$FK::Pref::Gameplay::PGDieEffects = false;
		$FK::Pref::Gameplay::PGDieSounds = false;
		$FK::Pref::Gameplay::007Yells = false;
		$FK::Pref::Gameplay::VehiclelessWins = true;
		$FK::Pref::Gameplay::AnnounceRecords = false;
		$FK::Pref::Gameplay::ShowFullTime = false;
		
		$FK::Pref::Karts::HornSounds = false;
		$FK::Pref::Karts::HornMS = 50;
		$FK::Pref::Karts::EngineSounds = false;
		
		export("$FK::Pref::*", "config/server/FASTKarts/prefs.cs");
	}
	else if($FK::Preset::Everything)
	{
		$FK::Preset::Everything = false;
		
		echo("=== Applying Give me everything. FASTKarts pref preset ===");
		
		$FK::Pref::Rounds::PerTrack = 6;
		$FK::Pref::Rounds::AllowNormal = true;
		$FK::Pref::Rounds::AllowRocket = true;
		$FK::Pref::Rounds::AllowBouncy = true;
		
		$FK::Pref::Tracks::LoadRandomly = true;
		$FK::Pref::Tracks::Voting = 1;
		
		$FK::Pref::Gameplay::Achievements = true;
		$FK::Pref::Gameplay::CrumbleDeath = true;
		$FK::Pref::Gameplay::PGDieEffects = true;
		$FK::Pref::Gameplay::PGDieSounds = true;
		$FK::Pref::Gameplay::007Yells = true;
		$FK::Pref::Gameplay::VehiclelessWins = true;
		$FK::Pref::Gameplay::AnnounceRecords = true;
		$FK::Pref::Gameplay::ShowFullTime = true;
		
		$FK::Pref::Karts::HornSounds = true;
		$FK::Pref::Karts::HornMS = 50;
		$FK::Pref::Karts::EngineSounds = true;
		
		export("$FK::Pref::*", "config/server/FASTKarts/prefs.cs");
	}
	
	$FK::Tick = schedule(100, 0, FK_Tick);
}

function gameConnection::FK_setBottomPrintInfo(%this)
{
	%mg = %this.miniGame;

	if(!isObject(%mg))
		%mg = $DefaultMiniGame;

	if(!isObject(%mg))
		return;
	
	if($FK::ResetCount == 0)
	{
		//loading message
		%this.FKBottomPrint = "<just:center>";
		%this.FKBottomPrint = %this.FKBottomPrint @ "\c6Loading track\c4: " @ FK_getTrackName($FK::CurrentTrack) @ "\c6.";
	}
	else
	{
		//colors
		%color = FK_getRoundColorString();
		
		//left
		%this.FKBottomPrint = "<just:left>";
		
		//round
		if($FK::Pref::Rounds::PerTrack > 0)
			%this.FKBottomPrint = %this.FKBottomPrint @ "\c6Round" @ %color @ ": " @ mfloor($FK::ResetCount) @ "/" @ mfloor($FK::Pref::Rounds::PerTrack) @ "   ";
		
		//time
		if(!$FK::TrackCompleted)
		{
			if($FK::TrackStarted)
				%this.FKBottomPrint = %this.FKBottomPrint @ "\c6Time" @ %color @ ": " @ FK_getTimeLeft(%mg);
			else
				%this.FKBottomPrint = %this.FKBottomPrint @ "\c6Time" @ %color @ ": 0:00";
		}
		
		//right
		%this.FKBottomPrint = %this.FKBottomPrint @ "<just:right>";
		
		//laps
		if(!$FK::TrackCompleted && mfloor(%this.FASTKartsLap) != 0)
			%this.FKBottomPrint = %this.FKBottomPrint @ "\c6Lap" @ %color @ ": " @ %this.FASTKartsLap @ " ";// @ "/" @ $FK::FinalLap @ " ";
		
		//next line
		%this.FKBottomPrint = %this.FKBottomPrint @ "<br><just:left>";
		
		//track
		%this.FKBottomPrint = %this.FKBottomPrint @ "\c6Track" @ %color @ ": " @ FK_getTrackName($FK::CurrentTrack);
	}
		
	//completed building it, send it.
	commandToClient(%this, 'bottomPrint', %this.FKBottomPrint, 0, true);
}

function gameConnection::FK_isChatSpam(%this, %text)
{
	%elapsedSpamTime = getSimTime() - %this.lastSpamMessageTime;
	%elapsedTime = getSimTime() - %this.lastMessageTime;
	
	//catch repeated messages
	if(%text $= %this.lastMessage)
	{
		messageClient(%this, '', "\c5Do not repeat yourself.");
		if(%elapsedSpamTime >= 5000)
		{
			%this.lastSpamMessageTime = getSimTime();
			%elapsedSpamTime = getSimTime() - %this.lastSpamMessageTime;
		}
	}
	
	//catch fast messages
	if(%elapsedTime < 1000)
	{
		messageClient(%this, '', "\c5Slow down.");
		if(%elapsedSpamTime >= 5000)
		{
			%this.lastSpamMessageTime = getSimTime();
			%elapsedSpamTime = getSimTime() - %this.lastSpamMessageTime;
		}
	}
	
	//apply flood protection
	if(%elapsedSpamTime < 5000 && $FK::Pref::Administration::FloodProtection)
	{
		if(%elapsedSpamTime < 1000)
			%secondsLeft = 5;
		else if(%elapsedSpamTime < 2000)
			%secondsLeft = 4;
		else if(%elapsedSpamTime < 3000)
			%secondsLeft = 3;
		else if(%elapsedSpamTime < 4000)
			%secondsLeft = 2;
		else if(%elapsedSpamTime < 5000)
			%secondsLeft = 1;
		
		messageClient(%this, '', "\c3FLOOD PROTECTION: \c0You must wait another " @ %secondsLeft @ " seconds.");
		return 1;
	}
	else
		return 0;
}

function gameConnection::FK_buildChatPrefix(%this)
{
	%prefix = "";
	
	//winner star
	if(%this.FKWinner)
		%prefix = %prefix @ "<bitmap:base/client/ui/ci/star> ";
	
	//administration rank letter
	if($FK::Pref::Administration::AdminTags)
	{
		//please don't edit these. it's your server but i made sure that these wouldn't be too attention grabby.
		if(%this.bl_id == getNumKeyID() || %this.isHost)
			%prefix = %prefix @ "\c5h ";
		else if(%this.isSuperAdmin)
			%prefix = %prefix @ "\c2sa ";
		else if(%this.isAdmin)
			%prefix = %prefix @ "\c2a ";
		else if(%this.isMod || %this.isModerator) //for moderator mods
			%prefix = %prefix @ "\c4m ";
	}
	
	//coloring of real clan tag
	%prefix = %prefix @ "\c7" @ %this.clanPrefix;
	
	return %prefix;
}

function gameConnection::FK_buildChatSuffix(%this)
{
	%suffix = "";
	
	//coloring of real clan tag
	%suffix = %suffix @ "\c7" @ %this.clanSuffix;
	
	return %suffix;
}

function gameConnection::FK_buildChatName(%this)
{
	%name = "";
	
	%name = %name @ FK_getRoundColorString();
	
	//actual player name
	%name = %name @ %this.getPlayerName();
	
	return %name;
}