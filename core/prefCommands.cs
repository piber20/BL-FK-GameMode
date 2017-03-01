function serverCmdSetRandomTracks(%client, %i)
{
	if(!%client.isSuperAdmin)
		return;
	
	if(%i $= "ON")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 has set the maps to be loaded in a random order.', %client.getPlayerName());
		$FK::Pref::Tracks::LoadRandomly = true;
	}
	else if(%i $= "OFF")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 has set the maps to not load randomly. Maps will be loaded in a numbered order.', %client.getPlayerName());
		$FK::Pref::Tracks::LoadRandomly = false;
	}
	else
	{
		messageClient(%client, '', "\c0Usage: /setTrackRotation <on/off>");
		return;
	}
	
	export("$FK::Pref::*", "config/server/FASTKarts/prefs.cs");
}

function serverCmdSetTipTime(%client, %i)
{
	if(!%client.isSuperAdmin)
		return;
	
	if(mfloor(%i) !$= %i)
	{
		messageClient(%client, '', "\c6You must type a number.");
		return;
	}
	
	%i = mfloor(%i);
	
	if(%i > 240)
	{
		messageClient(%client, '', "\c6Type a lower number. Use 0 to disable tips.");
		return;
	}
	else if(%i <= 0)
	{
		messageAll('MsgAdminForce', '\c3%1\c2 disabled info tips.', %client.getPlayerName());
		$FK::Pref::Tips::TipSeconds = -1;
	}
	else
	{
		messageAll('MsgAdminForce', '\c3%1\c2 set info tips to appear every %2 seconds.', %client.getPlayerName(), %i);
		$FK::Pref::Tips::TipSeconds = %i;
	}
	
	export("$FK::Pref::*", "config/server/FASTKarts/prefs.cs");
}

function serverCmdSetRoundLimit(%client, %i)
{
	if(!%client.isSuperAdmin)
		return;
	
	%i = mfloor(%i);
	
	if(%i <= 0)
	{
		messageAll('MsgAdminForce', '\c3%1\c2 disabled the round limit. Tracks now have infinite rounds.', %client.getPlayerName(), %i);
		$FK::Pref::Rounds::PerTrack = -1;
	}
	else
	{
		messageAll('MsgAdminForce', '\c3%1\c2 set the round limit to %2 rounds per track.', %client.getPlayerName(), %i);
		$FK::Pref::Rounds::PerTrack = %i;
	}
	
	export("$FK::Pref::*", "config/server/FASTKarts/prefs.cs");
}

function serverCmdHideAdminCommands(%client, %i)
{
	if(!%client.isSuperAdmin)
		return;
	
	if(%i $= "ON")
	{
		messageClient(%client, '', "\c6You've hidden the admin commands to normal players.");
		$FK::Pref::Administration::HideCommands = true;
	}
	else if(%i $= "OFF")
	{
		messageClient(%client, '', "\c6You've allowed normal players to see the admin command lists.");
		$FK::Pref::Administration::HideCommands = false;
	}
	else
	{
		messageClient(%client, '', "\c0Usage: /hideAdminCommands <on or off>");
		return;
	}
	
	export("$FK::Pref::*", "config/server/FASTKarts/prefs.cs");
}

function serverCmdSetCrumbleDeath(%client, %i)
{
	if(!%client.isSuperAdmin)
		return;
	
	if(%i $= "ON")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 enabled the crumble death effect.', %client.getPlayerName());
		$FK::Pref::Gameplay::CrumbleDeath = true;
	}
	else if(%i $= "OFF")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 disabled the crumble death effect.', %client.getPlayerName());
		$FK::Pref::Gameplay::CrumbleDeath = false;
	}
	else
	{
		messageClient(%client, '', "\c0Usage: /setCrumbleDeath <on or off>");
		return;
	}
	
	export("$FK::Pref::*", "config/server/FASTKarts/prefs.cs");
}

function serverCmdSetTrackVoting(%client, %i)
{
	if(!%client.isSuperAdmin)
		return;
	
	if(%i $= "ON")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 enabled track voting.', %client.getPlayerName());
		$FK::Pref::Tracks::Voting = 1;
	}
	else if(%i $= "ROUND")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 restricted track voting to only apply at round end.', %client.getPlayerName());
		$FK::Pref::Tracks::Voting = 2;
	}
	else if(%i $= "OFF")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 disabled track voting.', %client.getPlayerName());
		$FK::Pref::Tracks::Voting = 0;
		$FK::VoteInProgress = false;
		$FK::VoteTrack = "";
		$FK::VoteNextRound = false;
	}
	else
	{
		messageClient(%client, '', "\c0Usage: /setTrackVoting <on/round/off>");
		return;
	}
	
	export("$FK::Pref::*", "config/server/FASTKarts/prefs.cs");
}

function serverCmdSetAchievements(%client, %i)
{
	if(!%client.isSuperAdmin)
		return;
	
	if(%i $= "ON")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 enabled achievements.', %client.getPlayerName());
		$FK::Pref::Gameplay::Achievements = true;
		for(%a = 0; %a < $DefaultMinigame.numMembers; %a++)
		{
			%member = $DefaultMinigame.member[%a];
			if(isObject(%member))
			{
				clearClientAchievements(%member);
				sendAchievements(%member);
				loadClientAchievements(%member);
			}
		}
	}
	else if(%i $= "OFF")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 disabled achievements.', %client.getPlayerName());
		$FK::Pref::Gameplay::Achievements = false;
		for(%a = 0; %a < $DefaultMinigame.numMembers; %a++)
		{
			%member = $DefaultMinigame.member[%a];
			if(isObject(%member))
				clearClientAchievements(%member);
		}
	}
	else
	{
		messageClient(%client, '', "\c0Usage: /setAchievements <on/off>");
		return;
	}
	
	export("$FK::Pref::*", "config/server/FASTKarts/prefs.cs");
}

function serverCmdSetAddOn1(%client, %addon, %force)
{
	serverCmdSetAddOn(%client, 1, %addon, %force);
}

function serverCmdSetAddOn2(%client, %addon, %force)
{
	serverCmdSetAddOn(%client, 2, %addon, %force);
}

function serverCmdSetAddOn3(%client, %addon, %force)
{
	serverCmdSetAddOn(%client, 3, %addon, %force);
}

function serverCmdSetAddOn4(%client, %addon, %force)
{
	serverCmdSetAddOn(%client, 4, %addon, %force);
}

function serverCmdSetAddOn5(%client, %addon, %force)
{
	serverCmdSetAddOn(%client, 5, %addon, %force);
}

function serverCmdSetAddOn6(%client, %addon, %force)
{
	serverCmdSetAddOn(%client, 6, %addon, %force);
}

function serverCmdSetAddOn7(%client, %addon, %force)
{
	serverCmdSetAddOn(%client, 7, %addon, %force);
}

function serverCmdSetAddOn8(%client, %addon, %force)
{
	serverCmdSetAddOn(%client, 8, %addon, %force);
}

function serverCmdSetAddOn9(%client, %addon, %force)
{
	serverCmdSetAddOn(%client, 9, %addon, %force);
}

function serverCmdSetAddOn10(%client, %addon, %force)
{
	serverCmdSetAddOn(%client, 10, %addon, %force);
}

function serverCmdSetAddOn(%client, %num, %addon, %force)
{
	if(%client.bl_id != getNumKeyID())
		return;
	
	if(mfloor(%num) !$= %num)
	{
		messageClient(%client, '', "\c6No number provided. Must be a number from 1 to 10.");
		return;
	}
	
	%num = mfloor(%num);
	if(%num < 1 || %num > 10)
	{
		messageClient(%client, '', "\c6Must be a number from 1 to 10.");
		return;
	}
	
	%force = mfloor(%force);
	if(%addon !$= "")
	{
		if(!isFile("Add-Ons/" @ %addon @ "/server.cs") && !%force)
			messageClient(%client, '', "\c6No valid server.cs found. Either you do not have that Add-On or it has no server.cs.");
		else
		{
			if(%addon $= "Tool_NewDuplicator")
			{
				messageClient(%client, '', "\c6Tool_NewDuplicator is already automatically loaded if you have it.");
				return;
			}
			else if(%addon $= "Tool_Fill_Can")
			{
				messageClient(%client, '', "\c6Tool_Fill_Can is already automatically loaded if you have it.");
				return;
			}
			
			messageClient(%client, '', "\c6" @ %addon @ " will be loaded next time you start the gamemode.");
			$FK::Pref::Exec::Addon[%num] = %addon;
		}
	}
	else
	{
		if(%force)
		{
			messageClient(%client, '', "\c6Cleared pref. Add-on will not be loaded.");
			$FK::Pref::Exec::Addon[%num] = "";
		}
		else
		{
			messageClient(%client, '', "\c0Usage: /setAddOn" @ %num @ " <add-on name> <1 or 0 to ignore errors and force value change>");
			return;
		}
	}
	
	export("$FK::Pref::*", "config/server/FASTKarts/prefs.cs");
}

function serverCmdEnableRoundType(%client, %i)
{
	if(!%client.isSuperAdmin)
		return;
	
	if(%i $= "Normal")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 enabled the \c3Normal\c2 round type.', %client.getPlayerName());
		$FK::Pref::Rounds::AllowNormal = true;
	}
	else if(%i $= "Rocket")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 enabled the \c0Rocket\c2 round type.', %client.getPlayerName());
		$FK::Pref::Rounds::AllowRocket = true;
	}
	else if(%i $= "Bouncy")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 enabled the \c2Bouncy\c2 round type.', %client.getPlayerName());
		$FK::Pref::Rounds::AllowBouncy = true;
	}
	else if(%i $= "All")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 enabled \c3ALL\c2 round types.', %client.getPlayerName());
		$FK::Pref::Rounds::AllowNormal = true;
		$FK::Pref::Rounds::AllowRocket = true;
		$FK::Pref::Rounds::AllowBouncy = true;
	}
	else
	{
		messageClient(%client, '', "\c0Choose between \c3Normal\c0, \c0Rocket\c0, \c2Bouncy\c0, or \c3All\c0.");
		return;
	}
	
	export("$FK::Pref::*", "config/server/FASTKarts/prefs.cs");
}

function serverCmdDisableRoundType(%client, %i)
{
	if(!%client.isSuperAdmin)
		return;
	
	if(%i $= "Normal")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 disabled the \c3Normal\c2 round type.', %client.getPlayerName());
		$FK::Pref::Rounds::AllowNormal = false;
	}
	else if(%i $= "Rocket")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 disabled the \c0Rocket\c2 round type.', %client.getPlayerName());
		$FK::Pref::Rounds::AllowRocket = false;
	}
	else if(%i $= "Bouncy")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 disabled the \c2Bouncy\c2 round type.', %client.getPlayerName());
		$FK::Pref::Rounds::AllowBouncy = false;
	}
	else if(%i $= "All")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 disabled \c3ALL\c2 round types.', %client.getPlayerName());
		$FK::Pref::Rounds::AllowNormal = false;
		$FK::Pref::Rounds::AllowRocket = false;
		$FK::Pref::Rounds::AllowBouncy = false;
	}
	else
	{
		messageClient(%client, '', "\c0Choose between \c3Normal\c0, \c0Rocket\c0, \c2Bouncy\c0, or \c3All\c0.");
		return;
	}
	
	export("$FK::Pref::*", "config/server/FASTKarts/prefs.cs");
}

function serverCmdSetHornSounds(%client, %i)
{
	if(!%client.isSuperAdmin)
		return;
	
	if($FK::Pref::Karts::Allowed == 3)
		return;
	
	if(%i $= "ON")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 enabled kart horns.', %client.getPlayerName());
		$FK::Pref::Karts::HornSounds = true;
	}
	else if(%i $= "OFF")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 disabled kart horns.', %client.getPlayerName());
		$FK::Pref::Karts::HornSounds = false;
	}
	else
	{
		messageClient(%client, '', "\c0Usage: /setHornSounds <on/off>");
		return;
	}
	
	export("$FK::Pref::*", "config/server/FASTKarts/prefs.cs");
}

function serverCmdSetHornDelay(%client, %i)
{
	if(!%client.isSuperAdmin)
		return;
	
	if($FK::Pref::Karts::Allowed == 3)
		return;
	
	if(mfloor(%i) !$= %i)
	{
		messageClient(%client, '', "\c6You must type a number.");
		return;
	}
	
	%i = mfloor(%i);
	
	if(%i > 5000)
	{
		messageClient(%client, '', "\c6Type a lower number. Use \c3/setHornSounds off\c6 to disable horns.");
		return;
	}
	else if(%i < 0)
	{
		messageClient(%client, '', "\c6Type a higher number. Use \c3/setHornSounds off\c6 to disable horns.");
		return;
	}
	else
	{
		messageAll('MsgAdminForce', '\c3%1\c2 allowed kart horns to be played every %2 milliseconds.', %client.getPlayerName(), %i);
		$FK::Pref::Karts::HornMS = %i;
		FK_TipTick();
	}
	
	export("$FK::Pref::*", "config/server/FASTKarts/prefs.cs");
}

function serverCmdSetEngineSounds(%client, %i)
{
	if(!%client.isSuperAdmin)
		return;
	
	if($FK::Pref::Karts::Allowed == 3)
		return;
	
	if(%i $= "ON")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 enabled engine sounds.', %client.getPlayerName());
		$FK::Pref::Karts::EngineSounds = true;
	}
	else if(%i $= "OFF")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 disabled engine sounds.', %client.getPlayerName());
		$FK::Pref::Karts::EngineSounds = false;
	}
	else
	{
		messageClient(%client, '', "\c0Usage: /setEngineSounds <on/off>");
		return;
	}
	
	export("$FK::Pref::*", "config/server/FASTKarts/prefs.cs");
}

function serverCmdSetKartLeaning(%client, %i)
{
	if(!%client.isSuperAdmin)
		return;
	
	if($FK::Pref::Karts::Allowed == 3)
		return;
	
	if(%i $= "ON")
	{
		messageClient(%client, '', 'You allowed players to lean backward/forward in karts, you will need to restart the server for this change to occur.', %client.getPlayerName());
		$FK::Pref::Karts::UpDownLeaning = true;
	}
	else if(%i $= "OFF")
	{
		messageClient(%client, '', 'You restricted players from leaning backward/forward in karts, you will need to restart the server for this change to occur.', %client.getPlayerName());
		$FK::Pref::Karts::UpDownLeaning = false;
	}
	else
	{
		messageClient(%client, '', "\c0Usage: /setKartLeaning <on/off>");
		return;
	}
	
	export("$FK::Pref::*", "config/server/FASTKarts/prefs.cs");
}

function serverCmdSetAdminTags(%client, %i)
{
	if(!%client.isSuperAdmin)
		return;
	
	if(%i $= "ON")
	{
		messageClient(%client, '', "\c6You've enabled admin clan tags.");
		$FK::Pref::Administration::AdminTags = true;
	}
	else if(%i $= "OFF")
	{
		messageClient(%client, '', "\c6You've disabled admin clan tags.");
		$FK::Pref::Administration::AdminTags = false;
	}
	else
	{
		messageClient(%client, '', "\c0Usage: /setAdminTags <on/off>");
		return;
	}
	
	export("$FK::Pref::*", "config/server/FASTKarts/prefs.cs");
}

function serverCmdSetFloodProtection(%client, %i)
{
	if(!%client.isSuperAdmin)
		return;
	
	if(%i $= "ON")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 enabled flood protection.', %client.getPlayerName());
		$FK::Pref::Administration::FloodProtection = true;
	}
	else if(%i $= "OFF")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 disabled flood protection.', %client.getPlayerName());
		$FK::Pref::Administration::FloodProtection = false;
	}
	else
	{
		messageClient(%client, '', "\c0Usage: /setFloodProtection <on or off>");
		return;
	}
	
	export("$FK::Pref::*", "config/server/FASTKarts/prefs.cs");
}

function serverCmdEnableKart(%client, %i, %j, %k)
{
	if(!%client.isSuperAdmin)
		return;
	
	if(%i $= "SpeedKart" && %j $= "" && %k $= "")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 enabled the \c3SpeedKart\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::AllowSpeedKart = true;
	}
	else if(%i $= "SpeedKart" && %j $= "64" && %k $= "")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 enabled the \c3SpeedKart 64\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::Allow64 = true;
	}
	else if(%i $= "SpeedKart" && %j $= "7" && %k $= "")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 enabled the \c3SpeedKart 7\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::Allow7 = true;
	}
	else if(%i $= "SpeedKart" && %j $= "Blocko" && %k $= "")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 enabled the \c3SpeedKart Blocko\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::AllowBlocko = true;
	}
	else if(%i $= "SpeedKart" && %j $= "Buggy" && %k $= "")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 enabled the \c3SpeedKart Buggy\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::AllowBuggy = true;
	}
	else if(%i $= "SpeedKart" && %j $= "Classic" && %k $= "")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 enabled the \c3SpeedKart Classic\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::AllowClassic = true;
	}
	else if(%i $= "SpeedKart" && %j $= "Classic" && %k $= "GT")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 enabled the \c3SpeedKart Classic GT\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::AllowClassicGT = true;
	}
	else if(%i $= "SpeedKart" && %j $= "Formula" && %k $= "")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 enabled the \c3SpeedKart Formula\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::AllowFormula = true;
	}
	else if(%i $= "SpeedKart" && %j $= "Hotrod" && %k $= "")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 enabled the \c3SpeedKart Hotrod\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::AllowHotrod = true;
	}
	else if(%i $= "SpeedKart" && %j $= "Hyperion" && %k $= "")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 enabled the \c3SpeedKart Hyperion\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::AllowHyperion = true;
	}
	else if(%i $= "SpeedKart" && %j $= "Jeep" && %k $= "")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 enabled the \c3SpeedKart Jeep\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::AllowJeep = true;
	}
	else if(%i $= "SpeedKart" && %j $= "LeMans" && %k $= "")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 enabled the \c3SpeedKart LeMans\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::AllowLeMans = true;
	}
	else if(%i $= "SpeedKart" && %j $= "Muscle" && %k $= "")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 enabled the \c3SpeedKart Muscle\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::AllowMuscle = true;
	}
	else if(%i $= "SpeedKart" && %j $= "Vintage" && %k $= "")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 enabled the \c3SpeedKart Vintage\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::AllowVintage = true;
	}
	else if(%i $= "SpeedKart" && %j $= "Hover" && %k $= "")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 enabled the \c3SpeedKart Hover\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::AllowHover = true;
	}
	else if(%i $= "SpeedKart" && %j $= "Original" && %k $= "")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 enabled the \c3SpeedKart Original\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::AllowOriginal = true;
	}
	else if(%i $= "SpeedKart" && %j $= "Default" && %k $= "")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 enabled the \c3SpeedKart Default\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::AllowDefault = true;
	}
	else if(%i $= "SpeedKart" && %j $= "II" && %k $= "")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 enabled the \c3SpeedKart II\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::AllowSuperKart = true;
	}
	else if(%i $= "SpeedKart" && %j $= "ATV" && %k $= "")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 enabled the \c3SpeedKart ATV\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::AllowSuperATV = true;
	}
	else if(%i $= "SpeedKart" && %j $= "Hover" && %k $= "II")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 enabled the \c3SpeedKart Hover II\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::AllowSuperHover = true;
	}
	else if(%i $= "SpeedKart" && %j $= "Jetski" && %k $= "")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 enabled the \c3SpeedKart Jetski\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::AllowSuperJetski = true;
	}
	else if(%i $= "SpeedKart" && %j $= "Plane" && %k $= "")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 enabled the \c3SpeedKart Plane\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::AllowSuperPlane = true;
	}
	else if(%i $= "SuperKart" && %j $= "" && %k $= "")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 enabled the \c3SuperKart\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::AllowSuperKart = true;
	}
	else if(%i $= "SuperKart" && %j $= "ATV" && %k $= "")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 enabled the \c3SuperKart ATV\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::AllowSuperATV = true;
	}
	else if(%i $= "SuperKart" && %j $= "Hover" && %k $= "")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 enabled the \c3SuperKart Hover\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::AllowSuperHover = true;
	}
	else if(%i $= "SuperKart" && %j $= "Jetski" && %k $= "")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 enabled the \c3SuperKart Jetski\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::AllowSuperJetski = true;
	}
	else if(%i $= "SuperKart" && %j $= "Plane" && %k $= "")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 enabled the \c3SuperKart Plane\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::AllowSuperPlane = true;
	}
	else if(%i $= "All" && %j $= "" && %k $= "")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 enabled \c3ALL\c2 karts.', %client.getPlayerName());
		$FK::Pref::Karts::AllowSpeedKart = true;
		$FK::Pref::Karts::Allow64 = true;
		$FK::Pref::Karts::Allow7 = true;
		$FK::Pref::Karts::AllowBlocko = true;
		$FK::Pref::Karts::AllowBuggy = true;
		$FK::Pref::Karts::AllowClassic = true;
		$FK::Pref::Karts::AllowClassicGT = true;
		$FK::Pref::Karts::AllowFormula = true;
		$FK::Pref::Karts::AllowHotrod = true;
		$FK::Pref::Karts::AllowHyperion = true;
		$FK::Pref::Karts::AllowJeep = true;
		$FK::Pref::Karts::AllowLeMans = true;
		$FK::Pref::Karts::AllowMuscle = true;
		$FK::Pref::Karts::AllowVintage = true;
		$FK::Pref::Karts::AllowHover = true;
		$FK::Pref::Karts::AllowOriginal = true;
		$FK::Pref::Karts::AllowDefault = true;
		$FK::Pref::Karts::AllowSuperKart = true;
		$FK::Pref::Karts::AllowSuperATV = true;
		$FK::Pref::Karts::AllowSuperHover = true;
		$FK::Pref::Karts::AllowSuperJetski = true;
		$FK::Pref::Karts::AllowSuperPlane = true;
	}
	else
	{
		messageClient(%client, '', "\c0Type the name of a kart.");
		return;
	}
	
	export("$FK::Pref::*", "config/server/FASTKarts/prefs.cs");
}

function serverCmdDisableKart(%client, %i, %j, %k)
{
	if(!%client.isSuperAdmin)
		return;
	
	if(%i $= "SpeedKart" && %j $= "" && %k $= "")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 disabled the \c3SpeedKart\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::AllowSpeedKart = false;
	}
	else if(%i $= "SpeedKart" && %j $= "64" && %k $= "")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 disabled the \c3SpeedKart 64\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::Allow64 = false;
	}
	else if(%i $= "SpeedKart" && %j $= "7" && %k $= "")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 disabled the \c3SpeedKart 7\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::Allow7 = false;
	}
	else if(%i $= "SpeedKart" && %j $= "Blocko" && %k $= "")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 disabled the \c3SpeedKart Blocko\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::AllowBlocko = false;
	}
	else if(%i $= "SpeedKart" && %j $= "Buggy" && %k $= "")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 disabled the \c3SpeedKart Buggy\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::AllowBuggy = false;
	}
	else if(%i $= "SpeedKart" && %j $= "Classic" && %k $= "")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 disabled the \c3SpeedKart Classic\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::AllowClassic = false;
	}
	else if(%i $= "SpeedKart" && %j $= "Classic" && %k $= "GT")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 disabled the \c3SpeedKart Classic GT\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::AllowClassicGT = false;
	}
	else if(%i $= "SpeedKart" && %j $= "Formula" && %k $= "")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 disabled the \c3SpeedKart Formula\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::AllowFormula = false;
	}
	else if(%i $= "SpeedKart" && %j $= "Hotrod" && %k $= "")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 disabled the \c3SpeedKart Hotrod\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::AllowHotrod = false;
	}
	else if(%i $= "SpeedKart" && %j $= "Hyperion" && %k $= "")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 disabled the \c3SpeedKart Hyperion\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::AllowHyperion = false;
	}
	else if(%i $= "SpeedKart" && %j $= "Jeep" && %k $= "")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 disabled the \c3SpeedKart Jeep\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::AllowJeep = false;
	}
	else if(%i $= "SpeedKart" && %j $= "LeMans" && %k $= "")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 disabled the \c3SpeedKart LeMans\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::AllowLeMans = false;
	}
	else if(%i $= "SpeedKart" && %j $= "Muscle" && %k $= "")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 disabled the \c3SpeedKart Muscle\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::AllowMuscle = false;
	}
	else if(%i $= "SpeedKart" && %j $= "Vintage" && %k $= "")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 disabled the \c3SpeedKart Vintage\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::AllowVintage = false;
	}
	else if(%i $= "SpeedKart" && %j $= "Hover" && %k $= "")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 disabled the \c3SpeedKart Hover\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::AllowHover = false;
	}
	else if(%i $= "SpeedKart" && %j $= "Original" && %k $= "")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 disabled the \c3SpeedKart Original\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::AllowOriginal = false;
	}
	else if(%i $= "SpeedKart" && %j $= "Default" && %k $= "")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 disabled the \c3SpeedKart Default\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::AllowDefault = false;
	}
	else if(%i $= "SpeedKart" && %j $= "II" && %k $= "")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 disabled the \c3SpeedKart II\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::AllowSuperKart = false;
	}
	else if(%i $= "SpeedKart" && %j $= "ATV" && %k $= "")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 disabled the \c3SpeedKart ATV\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::AllowSuperATV = false;
	}
	else if(%i $= "SpeedKart" && %j $= "Hover" && %k $= "II")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 disabled the \c3SpeedKart Hover II\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::AllowSuperHover = false;
	}
	else if(%i $= "SpeedKart" && %j $= "Jetski" && %k $= "")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 disabled the \c3SpeedKart Jetski\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::AllowSuperJetski = false;
	}
	else if(%i $= "SpeedKart" && %j $= "Plane" && %k $= "")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 disabled the \c3SpeedKart Plane\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::AllowSuperPlane = false;
	}
	else if(%i $= "SuperKart" && %j $= "" && %k $= "")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 disabled the \c3SuperKart\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::AllowSuperKart = false;
	}
	else if(%i $= "SuperKart" && %j $= "ATV" && %k $= "")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 disabled the \c3SuperKart ATV\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::AllowSuperATV = false;
	}
	else if(%i $= "SuperKart" && %j $= "Hover" && %k $= "")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 disabled the \c3SuperKart Hover\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::AllowSuperHover = false;
	}
	else if(%i $= "SuperKart" && %j $= "Jetski" && %k $= "")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 disabled the \c3SuperKart Jetski\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::AllowSuperJetski = false;
	}
	else if(%i $= "SuperKart" && %j $= "Plane" && %k $= "")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 disabled the \c3SuperKart Plane\c2 kart.', %client.getPlayerName());
		$FK::Pref::Karts::AllowSuperPlane = false;
	}
	else if(%i $= "All" && %j $= "" && %k $= "")
	{
		messageAll('MsgAdminForce', '\c3%1\c2 disabled \c3ALL\c2 karts.', %client.getPlayerName());
		$FK::Pref::Karts::AllowSpeedKart = false;
		$FK::Pref::Karts::Allow64 = false;
		$FK::Pref::Karts::Allow7 = false;
		$FK::Pref::Karts::AllowBlocko = false;
		$FK::Pref::Karts::AllowBuggy = false;
		$FK::Pref::Karts::AllowClassic = false;
		$FK::Pref::Karts::AllowClassicGT = false;
		$FK::Pref::Karts::AllowFormula = false;
		$FK::Pref::Karts::AllowHotrod = false;
		$FK::Pref::Karts::AllowHyperion = false;
		$FK::Pref::Karts::AllowJeep = false;
		$FK::Pref::Karts::AllowLeMans = false;
		$FK::Pref::Karts::AllowMuscle = false;
		$FK::Pref::Karts::AllowVintage = false;
		$FK::Pref::Karts::AllowHover = false;
		$FK::Pref::Karts::AllowOriginal = false;
		$FK::Pref::Karts::AllowDefault = false;
		$FK::Pref::Karts::AllowSuperKart = false;
		$FK::Pref::Karts::AllowSuperATV = false;
		$FK::Pref::Karts::AllowSuperHover = false;
		$FK::Pref::Karts::AllowSuperJetski = false;
		$FK::Pref::Karts::AllowSuperPlane = false;
	}
	else
	{
		messageClient(%client, '', "\c0Type the name of a kart.");
		return;
	}
	
	export("$FK::Pref::*", "config/server/FASTKarts/prefs.cs");
}

function serverCmdSetOldNames(%client, %i)
{
	if(!%client.isSuperAdmin)
		return;
	
	if(%i $= "ON")
	{
		messageClient(%client, '', "\c6You've enabled the old kart names.");
		$FK::Pref::Karts::OldNames = true;
	}
	else if(%i $= "OFF")
	{
		messageClient(%client, '', "\c6You've disabled the old kart names.");
		$FK::Pref::Karts::OldNames = false;
	}
	else
	{
		messageClient(%client, '', "\c0Usage: /setOldNames <on/off>");
		return;
	}
	
	export("$FK::Pref::*", "config/server/FASTKarts/prefs.cs");
}

function serverCmdForceDefaultKarts(%client, %i)
{
	if(!%client.isSuperAdmin)
		return;
	
	if(%i $= "ON")
	{
		messageClient(%client, '', "\c6You've forced default karts to be loaded.");
		$FK::Pref::Karts::ForceDefault = true;
	}
	else if(%i $= "OFF")
	{
		messageClient(%client, '', "\c6You've stopped forcing default karts to be loaded.");
		$FK::Pref::Karts::ForceDefault = false;
	}
	else
	{
		messageClient(%client, '', "\c0Usage: /forceDefaultKarts <on/off>");
		return;
	}
	
	export("$FK::Pref::*", "config/server/FASTKarts/prefs.cs");
}