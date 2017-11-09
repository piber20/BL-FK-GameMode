//Title: Server Music
//Author: Zapk
//Reliable global music, changed by Admins; no client mods needed!
//edits were done for the purpose of the fastkarts gamemode

//	+---------------------------+
//	|	Server Music			|
//	+---------------------------+
//	|	Version: 3.0			|
//	|	Released: October 2		|
//	|	Author: Zapk | 12270	|
//	+---------------------------+

//	Musical Functions
function SM_StopSong()
{
	$SM::MusicPlaying = "";
	if(isObject(SM_Music))
		SM_Music.delete();
}

function SM_PlaySong(%client, %profile, %announce, %pitch)
{
	SM_StopSong();
	
	if(!$Pref::Server::FASTKarts::EnableGlobalMusic)
		return;
	if(!isObject(%profile))
		return;
	
	%profile = %profile.getName();
	$SM::MusicPlaying = %profile;
	
	if(%announce $= "")
		%announce = $Pref::Server::FASTKarts::AnnounceGlobalMusic;
	
	%oldTimescale = getTimescale();
	if(%pitch $= "")
		%pitch = getTimescale();
	if(%pitch < 0.2)
		%pitch = 0.2;
	if(%pitch > 2)
		%pitch = 2;
	
	if(%announce)
	{
		%message = strReplace(%profile, "musicData_", "");
		%message = strReplace(%message, "_", " ");
		%message = strReplace(%message, "DASH", "-");
		%message = strReplace(%message, "APOS", "'");
		if(isObject(%client))
			messageAll('', "\c4" @ %client.name SPC "\c6changed the song to\c4" SPC %message);
		else
			messageAll('', "\c6Now playing song\c4" SPC %message @ "\c6.");
	}
	
	setTimescale(%pitch);
	new AudioEmitter(SM_Music)
	{
		position = "0 0 0";
		profile = %profile;
		useProfileDescription = "0";
		description = "AudioLooping2D";
		type = "0";
		volume = "1.5";
		outsideAmbient = "1";
		ReferenceDistance = "4";
		maxDistance = "9001";
		isLooping = "1";
	};
	schedule(50, 0, "setTimescale", %oldTimescale);
}

//	Server Commands
//this was moved to setmusic.cs
// function serverCmdMusic(%client)
// {
	// if($DM::IsMusicless)
		// return;
	
	// if(!%client.isAdmin)
	// {
		// messageClient(%client, '', "\c6You are not an admin!");
		// return;
	// }
	// if(isObject(%client.serverMusicHandler))
	// {
		// %client.serverMusicHandler.delete();
	// }
	// if(!isObject(%client.serverMusicHandler))
	// {
		// %client.serverMusicHandler = new fxDTSBrick()
		// {
			// client = %client;
			// dataBlock = brickMusicData;
			// isPlanted = true;
			// isServerMusic["Admin"] = true;
			// mount = %client.player;
			// position = "-10000 -10000 -10000";
		// };
	// }
	// %client.wrenchBrick = %client.serverMusicHandler;
	// %client.wrenchBrick.sendWrenchSoundData(%client);
	// commandToClient(%client,'openWrenchSoundDlg',"Server Music",1);
// }

//	Package
if(isPackage(ServerMusic))
	deactivatePackage(ServerMusic);
package ServerMusic
{
	function serverCmdSetWrenchData(%client,%data)
	{
		if(%client.wrenchBrick.isServerMusic["Admin"])
		{
			if(isObject(%client.wrenchBrick.mount))
			{
				if(getWord(getField(%data,1),1) $= "0")
				{
					if(isObject(SM_Music))
						messageAll('', "\c4" @ %client.name SPC "\c6stopped the music.");
					
					SM_StopSong();
				}
				else
					SM_PlaySong(%client, getWord(getField(%data,1),1).getName());
			}
			%client.serverMusicHandler.delete();
		}
		else
			Parent::serverCmdSetWrenchData(%client,%data);
	}
};
activatePackage(ServerMusic);