function serverCmdHelp(%client)
{
	messageClient(%client, '', "\c6FASTKarts GameMode v" @ $FK::Pref::System::Version @ ". Here is a list of \c3commands\c6.");
	if($FK::Pref::Tracks::Voting == 1 || $FK::Pref::Rounds::PerTrack > 0)
		messageClient(%client, '', "\c6 - \c3/trackList \c6- Lists every track.");
	if($FK::Pref::Tracks::Voting $= 1)
		messageClient(%client, '', "\c6 - \c3/voteTrack (number) \c6- Vote for a track to load.");
	messageClient(%client, '', "\c6 - \c3/trackRecord (number) \c6- Lists the records for the track provided.");
	if($FK::Pref::Gameplay::Achievements)
		messageClient(%client, '', "\c6 - \c3/achievements \c6- Check out your achievements.");
	messageClient(%client, '', "\c6 - \c3/flip \c6- Flips your kart over. You can also press your brick plant key.");
	if(!$FK::Pref::Administration::HideCommands || %client.isAdmin)
		messageClient(%client, '', "\c6 - \c3/fkAdmin \c6- Lists the commands only an administrator can use.");
	messageClient(%client, '', "\c6 - \c3/fkCredits \c6- View the credits");
	messageClient(%client, '', "\c6Your goal is to complete the race. Some tracks have more than one lap.");
	messageClient(%client, '', "\c6Click on the vehicle spawn, choose a speedkart, and color it with your paint can.");
}

function serverCmdTrackList(%client)
{
	for(%i = 0; %i < $FK::numTracks; %i++)
	{
		if(%i == $FK::CurrentTrack)
			messageClient(%client, '', "\c2>" @ %i @ ". \c6" @ FK_getTrackName(%i));
		else
			messageClient(%client, '', "\c2  " @ %i @ ". \c6" @ FK_getTrackName(%i));
		
		%tracksCanLoad++;
	}
	messageClient(%client, '', "\c6In total, there are \c2" @ %tracksCanLoad @ "\c6 tracks.");
	
	if($FK::Pref::Rounds::PerTrack > 0)
	{
		if($FK::Pref::Tracks::LoadRandomly)
			messageClient(%client, '', "\c6Currently, tracks are set to load \c4randomly\c6.");
		else
			messageClient(%client, '', "\c6Currently, tracks are set to load \c1in order\c6.");
	}
	else
	{
		if($FK::Pref::Tracks::Voting == 0)
			messageClient(%client, '', "\c6Currently, tracks \c0do not\c6 rotate.");
		else
			messageClient(%client, '', "\c6Currently, tracks are set to load only with \c5player votes\c6.");
	}
}
function serverCmdMapList(%client)
{
	serverCmdTrackList(%client);
}

function serverCmdTrackRecord(%client, %i)
{
	if(mFloor(%i) !$= %i)
		%i = $FK::CurrentTrack;

	if(%i < 0 || %i > $FK::numTracks)
	{
		messageClient(%client, '', "That track doesn't exist.");
		return;
	}
	
	%trackName = FK_getTrackName(%i);
	%trackName = strReplace(%trackName, " ", "");
	
	if($FK::Pref::Rounds::AllowNormal)
	{
		%timeNormal = $FK::Record_[%trackName, "NORMAL", "Time"];
		%nameNormal = $FK::Record_[%trackName, "NORMAL", "PlayerName"];
		%blidNormal = $FK::Record_[%trackName, "NORMAL", "PlayerBLID"];
		%kartNormal = $FK::Record_[%trackName, "NORMAL", "Kart"];
		
		if($FK::Pref::Gameplay::ShowFullTime)
			%stringNormal = FK_getFullTimeString(%timeNormal);
		else
		{
			%secondsNormal = mFloor(%timeNormal / 1000);
			%stringNormal = getTimeString(%secondsNormal);
		}
		
		%kartStringNormal = "";
		if(isObject(%kartNormal))
			%kartStringNormal = " with the \c3" @ %kartNormal.uiName @ "\c6";
		
		if(%timeNormal $= "" || %timeNormal == 0)
			messageClient(%client, '', "\c6There is no server-wide record for \c3" @ FK_getTrackName(%i) @ "\c6.");
		else if(%blidNormal $= "")
			messageClient(%client, '', "\c6The current server-wide record for \c3" @ FK_getTrackName(%i) @ "\c6 was done in \c3" @ %stringNormal @ "\c6" @ %kartStringNormal @ ".");
		else if(%nameNormal $= "")
			messageClient(%client, '', "\c6The current server-wide record for \c3" @ FK_getTrackName(%i) @ "\c6 was done in \c3" @ %stringNormal @ "\c6 by \c1BL_ID: " @ %blidNormal @ "\c6" @ %kartStringNormal @ ".");
		else
			messageClient(%client, '', "\c6The current server-wide record for \c3" @ FK_getTrackName(%i) @ "\c6 was done in \c3" @ %stringNormal @ "\c6 by \c3" @ %nameNormal @ "\c6" @ %kartStringNormal @ ".");
	}
	if($FK::Pref::Rounds::AllowRocket)
	{
		%timeRocket = $FK::Record_[%trackName, "ROCKET", "Time"];
		%nameRocket = $FK::Record_[%trackName, "ROCKET", "PlayerName"];
		%blidRocket = $FK::Record_[%trackName, "ROCKET", "PlayerBLID"];
		%kartRocket = $FK::Record_[%trackName, "NORMAL", "Kart"];
		
		if($FK::Pref::Gameplay::ShowFullTime)
			%stringRocket = FK_getFullTimeString(%timeRocket);
		else
		{
			%secondsRocket = mFloor(%timeRocket / 1000);
			%stringRocket = getTimeString(%secondsRocket);
		}
		
		%kartStringRocket = "";
		if(isObject(%kartRocket))
			%kartStringRocket = " with the \c0" @ %kartRocket.uiName @ "\c6";
		
		if(%timeRocket $= "" || %timeRocket == 0)
			messageClient(%client, '', "\c6There is no server-wide record for \c0" @ FK_getTrackName(%i) @ "\c6 in the \c0Rocket\c6 round type.");
		else if(%blidRocket $= "")
			messageClient(%client, '', "\c6In the \c0Rocket\c6 round type, the current server-wide record for \c0" @ FK_getTrackName(%i) @ "\c6 was done in \c0" @ %stringRocket @ "\c6" @ %kartStringRocket @ ".");
		else if(%nameRocket $= "")
			messageClient(%client, '', "\c6In the \c0Rocket\c6 round type, the current server-wide record for \c0" @ FK_getTrackName(%i) @ "\c6 was done in \c0" @ %stringRocket @ "\c6 by \c1BL_ID: " @ %blidRocket @ "\c6" @ %kartStringRocket @ ".");
		else
			messageClient(%client, '', "\c6In the \c0Rocket\c6 round type, the current server-wide record for \c0" @ FK_getTrackName(%i) @ "\c6 was done in \c0" @ %stringRocket @ "\c6 by \c0" @ %nameRocket @ "\c6" @ %kartStringRocket @ ".");
	}
	if($FK::Pref::Rounds::AllowBouncy)
	{
		%timeBouncy = $FK::Record_[%trackName, "BOUNCY", "Time"];
		%nameBouncy = $FK::Record_[%trackName, "BOUNCY", "PlayerName"];
		%blidBouncy = $FK::Record_[%trackName, "BOUNCY", "PlayerBLID"];
		
		if($FK::Pref::Gameplay::ShowFullTime)
			%stringBouncy = FK_getFullTimeString(%timeBouncy);
		else
		{
			%secondsBouncy = mFloor(%timeBouncy / 1000);
			%stringBouncy = getTimeString(%secondsBouncy);
		}
		
		if(%timeBouncy $= "" || %timeBouncy == 0)
			messageClient(%client, '', "\c6There is no server-wide record for \c2" @ FK_getTrackName(%i) @ "\c6 in the \c2Bouncy\c6 round type.");
		else if(%blidBouncy $= "")
			messageClient(%client, '', "\c6In the \c2Bouncy\c6 round type, the current server-wide record for \c2" @ FK_getTrackName(%i) @ "\c6 was done in \c2" @ %stringBouncy @ "\c6.");
		else if(%nameBouncy $= "")
			messageClient(%client, '', "\c6In the \c2Bouncy\c6 round type, the current server-wide record for \c2" @ FK_getTrackName(%i) @ "\c6 was done in \c2" @ %stringBouncy @ "\c6 by \c1BL_ID: " @ %blidBouncy @ "\c6.");
		else
			messageClient(%client, '', "\c6In the \c2Bouncy\c6 round type, the current server-wide record for \c2" @ FK_getTrackName(%i) @ "\c6 was done in \c2" @ %stringBouncy @ "\c6 by \c2" @ %nameBouncy @ "\c6.");
	}
}
function serverCmdMapRecord(%client, %i)
{
	serverCmdTrackRecord(%client, %i);
}

function serverCmdFKAdmin(%client)
{
	if($FK::Pref::hideAdminCommands || !%client.isAdmin)
		return;
	
	messageClient(%client, '', "\c6Type one of the commands below to see what commands only an \c2administrator\c6 can use.");
	messageClient(%client, '', "\c6 - \c3/fkRounds \c6- These commands are related to rounds.");
	messageClient(%client, '', "\c6 - \c3/fkTracks \c6- These commands are related to tracks.");
	messageClient(%client, '', "\c6 - \c3/fkKarts \c6- These commands are related to the karts.");
	messageClient(%client, '', "\c6 - \c3/fkPrefs \c6- These commands let you change other gamemode prefs.");
	if(%client.bl_id == getNumKeyID())
		messageClient(%client, '', "\c6 - \c3/fkDebug \c6- Lists debug commands only you, the host, can use.");
}

function serverCmdFKRounds(%client)
{
	if($FK::Pref::Administration::HideCommands || !%client.isAdmin)
		return;
	
	messageClient(%client, '', "\c6Here is a list of commands under \c2fkControl\c6.");
	messageClient(%client, '', "\c6 - \c3/setRound (number) \c6- Change the round to the number provided.");
	messageClient(%client, '', "\c6 - \c3/nextRound \c6- Skips the current round.");
	messageClient(%client, '', "\c6 - \c3/setRoundLimit (number) \c6- Sets the amount of rounds per track. Setting it to 0 will disable track rotation.");
	messageClient(%client, '', "\c6 - \c3/enableRoundType (round type or all) \c6- Allows you to choose what rounds can be picked.");
	messageClient(%client, '', "\c6 - \c3/disableRoundType (round type or all) \c6- Allows you to choose what rounds will not be picked.");
}

function serverCmdFKTracks(%client)
{
	if($FK::Pref::Administration::HideCommands || !%client.isAdmin)
		return;
	
	messageClient(%client, '', "\c6Here is a list of commands under \c2fkControl\c6.");
	messageClient(%client, '', "\c6 - \c3/setTrack (number) \c6- Change the track to a number provided from /tracklist.");
	messageClient(%client, '', "\c6 - \c3/nextTrack \c6- Skips the current track.");
	messageClient(%client, '', "\c6 - \c3/setRandomTracks (on/off) \c6- Sets if tracks can load randomly.");
	messageClient(%client, '', "\c6 - \c3/setTrackVoting (on/round/off) \c6- Allows you to change if standard players can vote to change the track.");
}

function serverCmdFKKarts(%client)
{
	if($FK::Pref::Administration::HideCommands || !%client.isAdmin)
		return;
	
	messageClient(%client, '', "\c6Here is a list of commands under \c2fkKarts\c6.");
	messageClient(%client, '', "\c6 - \c3/forceDefaultKarts (on/off) \c6- Allows you to force the default karts from the SpeedKart gamemode to be loaded. Requires a server restart.");
	if($FK::Pref::Karts::ForceDefault)
		messageClient(%client, '', "\c6Stop forcing default karts to enable more settings.");
	else
	{
		messageClient(%client, '', "\c6 - \c3/enableKart (name of kart) \c6- Allows players to spawn the kart provided. Requires a server restart.");
		messageClient(%client, '', "\c6 - \c3/disableKart (name of kart) \c6- Disallows players from spawning the kart provided. Requires a server restart.");
		messageClient(%client, '', "\c6 - \c3/setHornSounds (on/off) \c6- Enable or disable horn sounds.");
		messageClient(%client, '', "\c6 - \c3/setHornDelay (number) \c6- Sets how often players can play a horn sound.");
		messageClient(%client, '', "\c6 - \c3/setEngineSounds (on/off) \c6- Enable or disable engine sounds.");
		messageClient(%client, '', "\c6 - \c3/setKartLeaning (on/off) \c6- Enable or disable players from leaning backward/forward in karts. Requires a server restart.");
		messageClient(%client, '', "\c6 - \c3/setOldNames (on/off) \c6- Gives karts the older uinames they had.");
	}
}

function serverCmdFKPrefs(%client)
{
	if($FK::Pref::Administration::HideCommands || !%client.isAdmin)
		return;
	
	messageClient(%client, '', "\c6Here is a list of commands under \c2fkPrefs\c6.");
	messageClient(%client, '', "\c6 - \c3/setTipTime (seconds) \c6- Sets if info tips are shown. Setting it to 0 will disable tips.");
	messageClient(%client, '', "\c6 - \c3/hideAdminCommands (on/off) \c6- Sets if normal players can see these admin command lists.");
	messageClient(%client, '', "\c6 - \c3/setCrumbleDeath (on/off) \c6- Enables or disables the crumble death effect.");
	messageClient(%client, '', "\c6 - \c3/setAchievements (on/off) \c6- Sets if achievements are enabled.");
	messageClient(%client, '', "\c6 - \c3/setAdminTags (on/off) \c6- Choose if you want admins to have a special admin clan tag.");
	messageClient(%client, '', "\c6 - \c3/setFloodProtection (on/off) \c6- Choose if you want your chat to be protected by spammers or not.");
	if(%client.bl_id != getNumKeyID())
		messageClient(%client, '', "\c6 - \c3/setAddOn(1 to 10) (addon) (force) \c6- Allows you to load add-ons next time you start the gamemode.");
}

function serverCmdFKCredits(%client)
{
	messageClient(%client, '', "\c6The \c2FASTKarts\c6 gamemode was made by \c3piber20\c6 and \c3Mr Noobler\c6 using the default \c3SpeedKart\c6 gamemode as a base.");
	messageClient(%client, '', "\c6The overhauled \c2SpeedKarts\c6 were done by \c3Mr Noobler\c6, merging SuperKart and improving physics and skill-based gameplay.");
	messageClient(%client, '', "\c6A wealth of standalone add-ons have been integrated and modified for use in this gamemode:");
	messageClient(%client, '', "<a:https://forum.blockland.us/index.php?topic=260828>SpeedKart Vehicles</a>\c6 by Filipe1020 and Eksi. (overhauled)");
	messageClient(%client, '', "<a:https://forum.blockland.us/index.php?topic=290421>SuperKart Vehicles</a>\c6 by Filipe1020. (merged with speedkarts and overhauled)");
	messageClient(%client, '', "<a:https://forum.blockland.us/index.php?topic=305521>Choose Vehicle Event</a>\c6 by piber20.");
	messageClient(%client, '', "<a:https://forum.blockland.us/index.php?topic=305570>Bypass Vehicle Color Trust</a>\c6 by piber20.");
	messageClient(%client, '', "<a:https://forum.blockland.us/index.php?topic=288799>PGDie</a>\c6 by Hata, Bushido, and Aloshi. (merged with 007 death yells and expanded)");
	messageClient(%client, '', "<a:https://forum.blockland.us/index.php?topic=238071>007 Death Yells</a>\c6 by Arekan, Ipquarx, and Megaguy. (merged with pgdie and expanded)");
	messageClient(%client, '', "<a:http://orbs.daprogs.com/rtb/forum.returntoblockland.com/dlm/viewFilebb35.html?id=735>Achievements</a>\c6 by DarkLight. (overhauled)");
	messageClient(%client, '', "<a:https://blocklandglass.com/addons/addon.php?id=208>Brick Zone Events</a>\c6 by MeltingPlastic. (overhauled)");
	messageClient(%client, '', "<a:https://forum.blockland.us/index.php?topic=286400>siba's ModTer Pack</a>\c6 by siba.");
}

function FK_TipTick()
{
	cancel($FK::TipTick);
	
	if(mfloor($FK::Pref::Tips::TipSeconds) <= 0)
		return;
	else if(mfloor($FK::Pref::Tips::TipSeconds) < 5)
		$FK::Pref::Tips::TipSeconds = 5;
	
	if($DefaultMinigame.numMembers > 0)
	{
		%pick = 0;
		%tip[%pick++] = "\c5Tip\c6: Watch out! Touching water with your kart or otherwise will lead to death!";
		%tip[%pick++] = "\c6The gamemode can be downloaded at <a:http://piber20.com/d/bl/>piber20.com</a>\c6 or <a:https://forum.blockland.us/index.php?topic=305706.0>Blockland Forums</a>\c6.";
		%tip[%pick++] = "\c5Tip\c6: If you lose your kart, try jumping off of ramps for a tiny speed boost.";
		%tip[%pick++] = "\c5Tip\c6: Vehicles can be suprisingly effective killing machines.";
		%tip[%pick++] = "\c5Tip\c6: Each kart has slightly different stats, some being faster but requiring more skill.";
		%tip[%pick++] = "\c5Tip\c6: The standard Speedkart is a good starter kart for beginners.";
		%tip[%pick++] = "\c5Tip\c6: Drifting can help you cut corners better.";
		%tip[%pick++] = "\c5Tip\c6: Disabling Strafe Controls can help you be more precise, but requires you to adjust to it.";
		%tip[%pick++] = "\c5Tip\c6: Braking is key.";
		%tip[%pick++] = "\c5Tip\c6: Experiment with karts to see which one is right for you.";
		%tip[%pick++] = "\c5Tip\c6: Click the vehicle spawn to to spawn a speedkart.";
		%tip[%pick++] = "\c5Tip\c6: You can color your kart by simply spraying it with your paint can.";
		%tip[%pick++] = "\c5Tip\c6: Press your brick plant key to make your kart flip. This can get you unstuck or help you do some sick tricks.";
		%tip[%pick++] = "\c5Tip\c6: The Speedkart Original is one of the fastest karts, but has a really strong grip which can make it hard to control.";
		%tip[%pick++] = "\c5Tip\c6: If you see some odd floating bricks, try typing \"FlushVBOCache();\" in your console.";
		if($FK::Pref::Karts::Allowed != 2)
			%tip[%pick++] = "\c5Tip\c6: The Speedkart Hover II is one of the fastest karts, but is very slippery which can make it hard to control.";
		if($FK::Pref::Gameplay::Achievements)
			%tip[%pick++] = "\c5Tip\c6: Did you type \"/help\" into chat? You get a free achievement if you do!";
		else
			%tip[%pick++] = "\c5Tip\c6: Did you type \"/help\" into chat?";
		
		%random = getRandom(1, %pick);
		//echo("(TIP)" @ %tip[%random]);
		messageAll('', %tip[%random]);
	}
	
	$FK::TipTick = schedule(($FK::Pref::Tips::TipSeconds * 1000), 0, FK_TipTick);
}