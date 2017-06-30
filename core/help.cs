function serverCmdHelp(%client)
{
	messageClient(%client, '', "\c6FASTKarts GameMode v" @ $FK::Version @ ". Here is a list of \c3commands\c6.");
	if($Pref::Server::FASTKarts::EnableTrackVoting == 1 || $Pref::Server::FASTKarts::RoundLimit > 0)
		messageClient(%client, '', "\c6 - \c3/trackList \c6- Lists every track.");
	if($Pref::Server::FASTKarts::EnableTrackVoting $= 1)
		messageClient(%client, '', "\c6 - \c3/voteTrack (number) \c6- Vote for a track to load.");
	messageClient(%client, '', "\c6 - \c3/trackRecord (number) \c6- Lists the records for the track provided.");
	if($Pref::Server::FASTKarts::Achievements)
		messageClient(%client, '', "\c6 - \c3/achievements \c6- Check out your achievements.");
	messageClient(%client, '', "\c6 - \c3/flip \c6- Flips your kart over. You can also press your brick plant key.");
	if(%client.isAdmin)
		messageClient(%client, '', "\c6 - \c3/fkAdmin \c6- Lists the commands only an administrator can use.");
	messageClient(%client, '', "\c6 - \c3/fkCredits \c6- View the credits");
	messageClient(%client, '', "\c6 - \c3/download \c6- Where to download the gamemode.");
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
	
	if($Pref::Server::FASTKarts::RoundLimit > 0)
	{
		if($Pref::Server::FASTKarts::RandomTracks)
			messageClient(%client, '', "\c6Currently, tracks are set to load \c4randomly\c6.");
		else
			messageClient(%client, '', "\c6Currently, tracks are set to load \c1in order\c6.");
	}
	else
	{
		if($Pref::Server::FASTKarts::EnableTrackVoting == 0)
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
	
	if($Pref::Server::FASTKarts::NormalRounds)
	{
		%timeNormal = $FK::Record_[%trackName, "NORMAL", "Time"];
		%nameNormal = $FK::Record_[%trackName, "NORMAL", "PlayerName"];
		%blidNormal = $FK::Record_[%trackName, "NORMAL", "PlayerBLID"];
		%kartNormal = $FK::Record_[%trackName, "NORMAL", "Kart"];
		
		if($Pref::Server::FASTKarts::ShowMilliseconds)
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
	if($Pref::Server::FASTKarts::RocketRounds)
	{
		%timeRocket = $FK::Record_[%trackName, "ROCKET", "Time"];
		%nameRocket = $FK::Record_[%trackName, "ROCKET", "PlayerName"];
		%blidRocket = $FK::Record_[%trackName, "ROCKET", "PlayerBLID"];
		%kartRocket = $FK::Record_[%trackName, "NORMAL", "Kart"];
		
		if($Pref::Server::FASTKarts::ShowMilliseconds)
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
	if($Pref::Server::FASTKarts::BouncyRounds)
	{
		%timeBouncy = $FK::Record_[%trackName, "BOUNCY", "Time"];
		%nameBouncy = $FK::Record_[%trackName, "BOUNCY", "PlayerName"];
		%blidBouncy = $FK::Record_[%trackName, "BOUNCY", "PlayerBLID"];
		
		if($Pref::Server::FASTKarts::ShowMilliseconds)
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
	if(!%client.isAdmin)
		return;
	
	messageClient(%client, '', "\c6The commands below can only be used by an \c2administrator\c6.");
	messageClient(%client, '', "\c6 - \c3/setTrack (number) \c6- Change the track to a number provided from /tracklist.");
	messageClient(%client, '', "\c6 - \c3/nextTrack \c6- Skips the current track.");
	messageClient(%client, '', "\c6 - \c3/randomTrack \c6- Skips to a random track.");
	messageClient(%client, '', "\c6 - \c3/setRound (number) \c6- Change the round to the number provided.");
	messageClient(%client, '', "\c6 - \c3/nextRound \c6- Skips the current round.");
	if(%client.bl_id == getNumKeyID())
		messageClient(%client, '', "\c6 - \c3/fkDebug \c6- Lists debug commands only you, the host, can use.");
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
	messageClient(%client, '', "<a:https://forum.blockland.us/index.php?topic=311104>Modified Modter</a>\c6 by piber20.");
}

function serverCmdDownload(%client)
{
	messageClient(%client, '', "\c6The \c2FASTKarts\c6 gamemode can be found at these locations:");
	messageClient(%client, '', "<a:http://piber20.com/d/bl/>piber20.com</a>");
	messageClient(%client, '', "<a:https://forum.blockland.us/index.php?topic=305706.0>Blockland Forums</a>");
	messageClient(%client, '', "<a:https://blocklandglass.com/addons/addon.php?id=496>Blockland Glass</a>");
	messageClient(%client, '', "<a:http://www.nexusmods.com/blockland/mods/46/?>NexusMods</a>");
	messageClient(%client, '', "<a:https://github.com/piber20/BL-FK-GameMode/releases>GitHub</a>");
}

function FK_TipTick()
{
	cancel($FK::TipTick);
	
	if(mfloor($Pref::Server::FASTKarts::TipSeconds) <= 0)
		return;
	else if(mfloor($Pref::Server::FASTKarts::TipSeconds) < 5)
		$Pref::Server::FASTKarts::TipSeconds = 5;
	
	if($DefaultMinigame.numMembers > 0)
	{
		%pick = 0;
		%tip[%pick++] = "\c5Tip\c6: Watch out! Touching water with your kart or otherwise will lead to death!";
		%tip[%pick++] = "\c6Want to host this gamemode yourself? Type this command into chat: \c3/download";
		%tip[%pick++] = "\c5Tip\c6: Vehicles can be suprisingly effective killing machines.";
		%tip[%pick++] = "\c5Tip\c6: Drifting can help you cut corners better.";
		%tip[%pick++] = "\c5Tip\c6: Disabling Strafe Controls can help you move your kart more precisely.";
		%tip[%pick++] = "\c5Tip\c6: Braking is key.";
		%tip[%pick++] = "\c5Tip\c6: Click the vehicle spawn to to spawn a speedkart.";
		%tip[%pick++] = "\c5Tip\c6: You can color your kart by simply spraying it with your paint can.";
		%tip[%pick++] = "\c5Tip\c6: Press your brick plant key to make your kart flip. This can get you unstuck or help you do some sick tricks.";
		%tip[%pick++] = "\c5Tip\c6: If you see some floating bricks that shouldn't be there, try typing \"FlushVBOCache();\" in your console.";
		
		if(FK_getKartsAllowed() > 1 && !$Pref::Server::FASTKarts::ForceSpeedkarts)
		{
			%tip[%pick++] = "\c5Tip\c6: Experiment with karts to see which one is right for you.";
			%tip[%pick++] = "\c5Tip\c6: Each kart has slightly different stats, some being faster but requiring more skill.";
			
			if($Pref::Server::FASTKarts::AllowSpeedKart)
				%tip[%pick++] = "\c5Tip\c6: The standard Speedkart is a good starter kart for beginners.";
		
			if($Pref::Server::FASTKarts::AllowOriginal)
				%tip[%pick++] = "\c5Tip\c6: The Speedkart Original is one of the fastest karts, but has a really strong grip which can make it hard to control.";
			
			if($Pref::Server::FASTKarts::AllowSuperHover)
				%tip[%pick++] = "\c5Tip\c6: The " @ SpeedKartHoverIIVehicle.uiName @ " is one of the fastest karts, but is very slippery which can make it hard to control.";
		}
		
		if($Pref::Server::FASTKarts::NoKartKillTime != 0)
			%tip[%pick++] = "\c5Tip\c6: If you lose your kart, try jumping off of ramps for a tiny speed boost.";
		
		if($Pref::Server::FASTKarts::Achievements)
			%tip[%pick++] = "\c5Tip\c6: Did you type \"/help\" into chat? You get a free achievement if you do!";
		else
			%tip[%pick++] = "\c5Tip\c6: Did you type \"/help\" into chat?";
		
		%random = getRandom(1, %pick);
		messageAll('', %tip[%random]);
	}
	
	$FK::TipTick = schedule(($Pref::Server::FASTKarts::TipSeconds * 1000), 0, FK_TipTick);
}