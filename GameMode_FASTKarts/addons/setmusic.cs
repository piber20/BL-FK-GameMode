function serverCmdsetmusic(%c)
{
	%p = %c.player;
	if(!isObject(%p))
		return;
	if (isObject(%p.setMusicHandler) == false)
	{
		// Need to make the GUI system
		
		%p.setMusicHandler = new fxDTSBrick() {
			client = %c;
			dataBlock = brickMusicData;
			isPlanted = true;
			isMusic = true;
			mount = %p;
			position = "-10000 -10000 -10000";
		};
	}
	%c.wrenchBrick = %p.setMusicHandler;
	%c.wrenchBrick.sendWrenchSoundData(%c);
	commandToClient(%c,'setWrenchData',"N _Select_A_Song_To_Play");
	commandToClient(%c,'openWrenchSoundDlg',"Set Music",1);
}

function serverCmdboombox(%c)
{
	serverCmdsetmusic(%c);
}

function serverCmdstereo(%c)
{
	serverCmdsetmusic(%c);
}

function serverCmdmusic(%c)
{
	serverCmdsetmusic(%c);
}

package SetMusic
{
	function serverCmdSetWrenchData(%c,%data)
	{
		if (%c.wrenchBrick.isMusic)
		{
			if (isObject(%c.wrenchBrick.mount))
			{
				if (getWord(getField(%data,1),1) $= "0")
					%c.wrenchBrick.mount.stopAudio(0);
				else
					%c.wrenchBrick.mount.playAudio(0,getWord(getField(%data,1),1));
			}
		}
		else
		{
			Parent::serverCmdSetWrenchData(%c,%data);
		}
	}
	
	function Player::delete(%this)
	{
		if (isObject(%this.setMusicHandler))
			%this.setMusicHandler.delete();
		Parent::delete(%this);
	}
};
activatePackage(SetMusic);