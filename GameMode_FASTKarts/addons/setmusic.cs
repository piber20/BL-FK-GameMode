//parts of zapk's server music 3.0 have been injected into this

function serverCmdsetmusic(%client, %arg)
{
	%doGlobal = false;
	if(!$Pref::Server::FASTKarts::EnableBoombox)
		%doGlobal = true;
	if(%arg $= "global" || %arg $= "server")
		%doGlobal = true;
	
	if(%doGlobal) //global music
	{
		if(!$Pref::Server::FASTKarts::EnableGlobalMusic)
			return;
		
		if(!%client.isAdmin)
			return;
		
		if(isObject(%client.serverMusicHandler))
			%client.serverMusicHandler.delete();
		
		if(!isObject(%client.serverMusicHandler))
		{
			%client.serverMusicHandler = new fxDTSBrick()
			{
				client = %client;
				dataBlock = brickMusicData;
				isPlanted = true;
				isServerMusic["Admin"] = true;
				mount = %client.player;
				position = "-10000 -10000 -10000";
			};
		}
		%client.wrenchBrick = %client.serverMusicHandler;
		%client.wrenchBrick.sendWrenchSoundData(%client);
		commandToClient(%client,'setWrenchData',"N _Select_A_Song_To_Play");
		commandToClient(%client,'openWrenchSoundDlg',"Set Server Music",1);
	}
	else //player music
	{
		if(!$Pref::Server::FASTKarts::EnableBoombox)
			return;
		
		%player = %client.player;
		if(!isObject(%player))
			return;
		if(isObject(%player.setMusicHandler) == false)
		{
			%player.setMusicHandler = new fxDTSBrick() {
				client = %client;
				dataBlock = brickMusicData;
				isPlanted = true;
				isMusic = true;
				mount = %player;
				position = "-10000 -10000 -10000";
			};
		}
		%client.wrenchBrick = %player.setMusicHandler;
		%client.wrenchBrick.sendWrenchSoundData(%client);
		commandToClient(%client,'setWrenchData',"N _Select_A_Song_To_Play");
		commandToClient(%client,'openWrenchSoundDlg',"Set Music",1);
	}
}

function serverCmdboombox(%client, %arg)
{
	serverCmdsetmusic(%client, %arg);
}

function serverCmdstereo(%client, %arg)
{
	serverCmdsetmusic(%client, %arg);
}

function serverCmdmusic(%client, %arg)
{
	serverCmdsetmusic(%client, %arg);
}

package SetMusic
{
	function serverCmdSetWrenchData(%client,%data)
	{
		if(%client.wrenchBrick.isMusic)
		{
			if(isObject(%client.wrenchBrick.mount))
			{
				if(getWord(getField(%data,1),1) $= "0")
					%client.wrenchBrick.mount.stopAudio(0);
				else
					%client.wrenchBrick.mount.playAudio(0,getWord(getField(%data,1),1));
			}
		}
		else
			Parent::serverCmdSetWrenchData(%client,%data);
	}
	
	function Player::delete(%this)
	{
		if(isObject(%this.setMusicHandler))
			%this.setMusicHandler.delete();
		Parent::delete(%this);
	}
};
activatePackage(SetMusic);