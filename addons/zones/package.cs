if(isPackage(BrickZoneEvents))
	deactivatePackage(BrickZoneEvents);
package BrickZoneEvents
{
	function serverCmdAddEvent(%client, %delay, %input, %target, %a, %b, %output, %par1, %par2, %par3, %par4)
	{
		Parent::serverCmdAddEvent(%client, %delay, %input, %target, %a, %b, %output, %par1, %par2, %par3, %par4);
		%brick = %client.wrenchBrick;
		if(!isObject(%brick))
			return;
		
		%brick.checkForZoneEvents();
	}
	
	function fxDtsBrick::onPlant(%this)
	{
		Parent::onPlant(%this);
		if(!isObject(%this))
			return;
		
		%this.checkForZoneEvents();
		%this.LoadZoneParameters();
	}
	
	function fxDtsBrick::onLoadPlant(%this)
	{
		Parent::onLoadPlant(%this);
		if(!isObject(%this))
			return;
		
		%this.checkForZoneEvents();
		%this.LoadZoneParameters();
	}

	//brick functions
	function fxDTSBrick::onDeath(%this)
	{
		Parent::onDeath(%this);
		%this.setBrickZone(false, -1); //delete the zone
	}

	function fxDTSBrick::onRemove(%this)
	{
		Parent::onRemove(%this);
		%this.setBrickZone(false, -1); //delete the zone
	}

	function fxDTSBrick::onFakeDeath(%this)
	{
		Parent::onFakeDeath(%this);
		
		if(%this.isZoneBrick == 1)
		{
			%this.physicalZone.deactivate();
			%this.trigger.active = false;
		}
	}

	function fxDTSBrick::onClearFakeDeath(%this)
	{
		Parent::onClearFakeDeath(%this);
		if(%this.isZoneBrick == 1)
		{
			%this.physicalZone.activate();
			%this.trigger.active = true;
		}
	}
	
	function fxDTSBrick::onColorChange(%this)
	{
		Parent::onColorChange(%this);
		if(%this.isZoneBrick == 1)
		{
			%color = getColorIDTable(%this.colorID);
			%rgb = getWords(%color, 0, 2);
			%a = getWord(%color, 3) / 0.5;

			%this.PhysicalZone.setWaterColor(%rgb SPC %a);
		}
	}

	//We need to notify the correct zone when somebody dies inside a zone. - call onPlayerLeaveBrick
	function gameConnection::onDeath(%client,%source,%killer,%type,%location)
	{
		if(isObject(%client.player))
		{
			for(%i=0;%i<%client.player.insidezoneidx;%i++)
			{
				if(IsObject(%client.player.insidezone[%i]))
					%client.player.insidezone[%i].onPlayerLeaveBrick(%client);
			}
		}
		
		Parent::onDeath(%client,%source,%killer,%type,%location);
	}
};
activatePackage(BrickZoneEvents);