//Register inputs
registerInputEvent("fxDTSBrick","onPlayerEnterBrick","Self fxDTSBrick" TAB "Player Player" TAB "Client GameConnection" TAB "MiniGame MiniGame");
registerInputEvent("fxDTSBrick","onBotEnterBrick","Self fxDTSBrick" TAB "Bot Bot" TAB "Driver(Client) GameConnection" TAB "Driver(Player) Player" TAB "MiniGame MiniGame");
registerInputEvent("fxDTSBrick","onVehicleEnterBrick","Self fxDTSBrick" TAB "Vehicle Vehicle" TAB "Driver(Client) GameConnection" TAB "Driver(Player) Player" TAB "MiniGame MiniGame");

registerInputEvent("fxDTSBrick","onPlayerInBrick", "Self fxDTSBrick" TAB "Player Player" TAB "Client GameConnection" TAB "MiniGame MiniGame");
registerInputEvent("fxDTSBrick","onBotInBrick", "Self fxDTSBrick" TAB "Bot Bot" TAB "Driver(Client) GameConnection" TAB "Driver(Player) Player" TAB "MiniGame MiniGame");
registerInputEvent("fxDTSBrick","onVehicleInBrick","Self fxDTSBrick" TAB "Vehicle Vehicle" TAB "Driver(Client) GameConnection" TAB "Driver(Player) Player" TAB "MiniGame MiniGame");

registerInputEvent("fxDTSBrick","onPlayerLeaveBrick","Self fxDTSBrick" TAB "Player Player" TAB "Client GameConnection" TAB "MiniGame MiniGame");
registerInputEvent("fxDTSBrick","onBotLeaveBrick","Self fxDTSBrick" TAB "Bot Bot" TAB "Driver(Client) GameConnection" TAB "Driver(Player) Player" TAB "MiniGame MiniGame");
registerInputEvent("fxDTSBrick","onVehicleLeaveBrick","Self fxDTSBrick" TAB "Vehicle Vehicle" TAB "Driver(Client) GameConnection" TAB "Driver(Player) Player" TAB "MiniGame MiniGame");

//Register outputs
registerOutputEvent("fxDTSBrick", "setBrickZone", "bool 0" TAB "list NONE 7 Up 1 Down 2 North 3 South 4 East 5 West 6 Center 0" TAB "int 0.0 400.0 10");
registerOutputEvent("fxDTSBrick", "setGravity", "float -5 15 0.1 1");
registerOutputEvent("fxDTSBrick", "setDrag", "int -50 50 0");
registerOutputEvent("fxDTSBrick", "setVelocityMod", "float -5 5 0.1 1");
registerOutputEvent("fxDTSBrick", "setAppliedForce", "vector 40000 -40000 0");

registerOutputEvent("fxDTSBrick", "setIsWater", "bool 0");
registerOutputEvent("fxDTSBrick", "setWaterViscosity", "int -200 200 40");
registerOutputEvent("fxDTSBrick", "setWaterDensity", "int -40 40 1");

//Input Functions
//Entering
function fxDTSBrick::onPlayerEnterBrick(%this, %obj)
{
	if(isObject(%this.trigger) && %this.trigger.istriggerBricktrigger == true)
	{
		//store this brick on the player just in case he dies we can call onplayerleavezone.
		%obj.numinzones++;
		%obj.insidezone[%obj.insidezoneidx] = %this; 
		%obj.insidezoneidx++;
		
		$InputTarget_["Self"] = %this;
		$InputTarget_["Player"] = %obj;
		$InputTarget_["Client"] = %obj.client;
		if(%this.getGroup().client.minigame)
			$InputTarget_["MiniGame"] = %this.getGroup().client.minigame;
		
		%this.processInputEvent("onPlayerEnterBrick",%obj.client);
	}
}

function fxDTSBrick::onBotEnterBrick(%this, %obj)
{
	if(isObject(%this.trigger) && %this.trigger.istriggerBricktrigger == true)
	{
		$InputTarget_["Self"] = %this;
		$InputTarget_["Bot"] = %obj;
		if(isObject(%obj.getControllingClient()))
		{
			$InputTarget_["Driver(Client)"] = %obj.getControllingClient();
			$InputTarget_["Driver(Player)"] = %obj.getControllingObject();
		}
		if(%this.getGroup().client.minigame)
			$InputTarget_["MiniGame"] = %this.getGroup().client.minigame;
		
		if(isObject(%obj.getControllingClient()))
			%this.processInputEvent("onBotEnterBrick",%obj.getControllingClient());
	}
}

function fxDTSBrick::onVehicleEnterBrick(%this, %obj)
{
	if(isObject(%this.trigger) && %this.trigger.istriggerBricktrigger == 1)
	{
		%DriverClient = "";
		%DriverPlayer = "";
		
		//Figure Out Driver---------------------------------
		if(isObject(%obj.getControllingClient()))
			%DriverClient = %obj.getControllingClient();
		else//the vehicle may be skiis or there is no driver..
		{
			if(isObject(%obj.client))
				%DriverClient = %obj.client;
			//otherwise leave the DriverClient empty...
		}
		%DriverPlayer = %DriverClient.player;
	
		$InputTarget_["Self"] = %this;
		$InputTarget_["Vehicle"] = %obj;
		
		$InputTarget_["Driver(Client)"] = %DriverClient;
		$InputTarget_["Driver(Player)"] = %DriverPlayer;
		
		$InputTarget_["MiniGame"] = %this.getGroup().client.minigame;

		if(isObject(%DriverClient))//driver client.
			%this.processInputEvent("onVehicleEnterBrick",%DriverClient);
		else
			%this.processInputEvent("onVehicleEnterBrick",%this.getGroup().client);
	}
}

//Inside
function fxDTSBrick::onPlayerInBrick(%this, %obj)
{
	//We Have to reset because this function is called many times each tick depending on how many objects are inside, this clears the slate to start off.
	$InputTarget_["Player"] = null;
	$InputTarget_["Client"] = null;
	
	if(isObject(%this.trigger) && %this.trigger.istriggerBricktrigger == true)
	{
		if(%this.getGroup().client.minigame)
			$InputTarget_["MiniGame"] = %this.getGroup().client.minigame;
		
		$InputTarget_["Self"] = %this;
		
		if(isObject(%obj))
		{
			if(%obj.getClassName() $= "Player")
			{	
				$InputTarget_["Player"] = %obj;
				$InputTarget_["Client"] = %obj.client;
			}
		}
		
		%this.processInputEvent("onPlayerInBrick",%this.getGroup().client);
	}
}
	
function fxDTSBrick::onBotInBrick(%this,%obj)
{
	//We have to reset because this function is called many times each tick depending on how many objects are inside, this clears the slate to start off.
	$InputTarget_["Bot"] = null;
	
	if(isObject(%this.trigger) && %this.trigger.istriggerBricktrigger == true)
	{
		if(%this.getGroup().client.minigame)
			$InputTarget_["MiniGame"] = %this.getGroup().client.minigame;
		
		$InputTarget_["Self"] = %this;
		
		if(isObject(%obj))
		{
			if(%obj.getClassName() $= "AIPlayer")
				$InputTarget_["Bot"] = %obj;
			if(isObject(%obj.getControllingClient()))
			{
				$InputTarget_["Driver(Client)"] = %obj.getControllingClient();
				$InputTarget_["Driver(Player)"] = %obj.getControllingObject();
			}
		}
		
		%this.processInputEvent("onBotInBrick",%this.getGroup().client);
	}
}

function fxDTSBrick::onVehicleInBrick(%this, %obj)
{
	//We have to reset because this function is called many times each tick depending on how many objects are inside, this clears the slate to start off.
	$InputTarget_["Vehicle"] = null;
	$InputTarget_["Driver(Client)"] = null;
	$InputTarget_["Driver(Player)"] = null;
	
	if(isObject(%this.trigger) && %this.trigger.istriggerBricktrigger == 1)
	{
		if(%this.getGroup().client.minigame)
			$InputTarget_["MiniGame"] = %this.getGroup().client.minigame;
		
		$InputTarget_["Self"] = %this;
		
		if(isObject(%obj))
		{
			if(%obj.getClassName() $= "WheeledVehicle" || %obj.getClassName() $= "FlyingVehicle")
			{
				if($Pref::Server::BrickZoneEvents::DoImpulseVehicles == 1)
					ImpulseVehicleWithZone(%obj,%this);
				
				$InputTarget_["Vehicle"] = %obj;
				if(isObject(%obj.getControllingObject()))
				{
					$InputTarget_["Driver(Client)"] = %obj.getControllingClient();
					$InputTarget_["Driver(Player)"] = %obj.getControllingClient().Player;
				}
			}
		}
		
		if(isObject(%obj.getControllingClient()))//driver client.
			%this.processInputEvent("onVehicleInBrick", %obj.getControllingClient());
		else
			%this.processInputEvent("onVehicleInBrick", %this.getGroup().client);
	}
}

//Leaving
function fxDTSBrick::onPlayerLeaveBrick(%this, %obj)
{
	if(isObject(%this.trigger) && %this.trigger.istriggerBricktrigger == true)
	{
		%obj.numinzones--;
		//remove this bricks reference on the player.
		for(%i=0; %i< %obj.insidezoneidx; %i++)
		{
			if(%obj.insidezone[%i] == %this)
				%obj.insidezone[%i] = ""; 
		}
		if(%obj.numinzones <= 0)
		{
			%obj.insidezoneidx=0;
			%obj.insidezone = "";
		}
	
		$InputTarget_["Self"] = %this;
		$InputTarget_["Player"] = %obj;
		$InputTarget_["Client"] = %obj.client;

		if(%this.getGroup().client.minigame)
			$InputTarget_["MiniGame"] = %this.getGroup().client.minigame;

		%this.processInputEvent("onPlayerLeaveBrick", %obj.client);
	}
}
	
function fxDTSBrick::onBotLeaveBrick(%this, %obj)
{	
	if(isObject(%this.trigger) && %this.trigger.istriggerBricktrigger == true)
	{
		$InputTarget_["Self"] = %this;
		$InputTarget_["Bot"] = %obj;
		if(isObject(%obj.getControllingClient()))
		{
			$InputTarget_["Driver(Client)"] = %obj.getControllingClient();
			$InputTarget_["Driver(Player)"] = %obj.getControllingObject();
		}
		if(%this.getGroup().client.minigame)
			$InputTarget_["MiniGame"] = %this.getGroup().client.minigame;
		
		if(isObject(%obj.getControllingClient()))
			%this.processInputEvent("onBotLeaveBrick",%obj.getControllingClient());
	}
}

function fxDTSBrick::onVehicleLeaveBrick(%this,%obj)
{
	if(isObject(%this.trigger) && %this.trigger.istriggerBricktrigger == 1)
	{
		%DriverClient = "";
		%DriverPlayer = "";
		
		//Figure Out Driver---------------------------------
		if(isObject(%obj.getControllingClient()))
			%DriverClient = %obj.getControllingClient();
		else//the vehicle may be skiis or there is no driver..
		{
			if(isObject(%obj.client))
				%DriverClient = %obj.client;
			//otherwise leave the DriverClient empty...
		}
		%DriverPlayer = %DriverClient.player;
	
		$InputTarget_["Self"] = %this;
		$InputTarget_["Vehicle"] = %obj;
		
		$InputTarget_["Driver(Client)"] = %DriverClient;
		$InputTarget_["Driver(Player)"] = %DriverPlayer;
		
		$InputTarget_["MiniGame"] = %this.getGroup().client.minigame;

		if(isObject(%DriverClient))//driver client.
			%this.processInputEvent("onVehicleLeaveBrick",%DriverClient);
		else
			%this.processInputEvent("onVehicleLeaveBrick",%this.getGroup().client);
	}
}

//Output Functions
function fxDTSBrick::setGravity(%this, %force)
{
	if(isObject(%this) && !isObject(%this.physicalZone) && !isObject(%this.trigger))
	{
		if(%brickData.isPremadeZoneBrick)
			%this.LoadZoneParameters();
		else
			%this.setBrickZone();
	}
	
	if(%this.isZoneBrick == 1)
		%this.physicalZone.gravityMod = %force;
}

function fxDTSBrick::setDrag(%this, %drag)
{
	if(isObject(%this) && !isObject(%this.physicalZone) && !isObject(%this.trigger))
	{
		if(%brickData.isPremadeZoneBrick)
			%this.LoadZoneParameters();
		else
			%this.setBrickZone();
	}
	
	if(%this.isZoneBrick == 1)
		%this.physicalZone.extraDrag = %drag;
}

function fxDTSBrick::setVelocityMod(%this, %vel)
{
	if(isObject(%this) && !isObject(%this.physicalZone) && !isObject(%this.trigger))
	{
		if(%brickData.isPremadeZoneBrick)
			%this.LoadZoneParameters();
		else
			%this.setBrickZone();
	}
	
	if(%this.isZoneBrick == 1)
		%this.physicalZone.velocityMod = %vel;
}

function fxDTSBrick::setAppliedForce(%this, %vector)
{
	if(isObject(%this) && !isObject(%this.physicalZone) && !isObject(%this.trigger))
	{
		if(%brickData.isPremadeZoneBrick)
			%this.LoadZoneParameters();
		else
			%this.setBrickZone();
	}
	
	if(%this.isZoneBrick == 1)
		%this.physicalZone.setAppliedForce(%vector);
}

//Water Related Outputs
function fxDTSBrick::setIsWater(%this, %water)
{
	if(isObject(%this) && !isObject(%this.physicalZone) && !isObject(%this.trigger))
	{
		if(%brickData.isPremadeZoneBrick)
			%this.LoadZoneParameters();
		else
			%this.setBrickZone();
	}
	
	if(%this.isZoneBrick == 1)
	{
		//This helps when using Trader's water events
		%this.physicalZone.iswaterBrickwater = 1;
		%this.physicalZone.waterBrick = %this;
		
		%color = getColorIDTable(%this.colorID);
		%rgb = getWords(%color, 0, 2);
		%a = getWord(%color, 3) / 0.5;
		%this.physicalZone.isWater = mfloor(%water);
		%this.PhysicalZone.setWaterColor(%rgb SPC %a);
	}
}

function fxDTSBrick::setWaterViscosity(%this, %viscosity)
{
	if(isObject(%this) && !isObject(%this.physicalZone) && !isObject(%this.trigger))
	{
		if(%brickData.isPremadeZoneBrick)
			%this.LoadZoneParameters();
		else
			%this.setBrickZone();
	}
	
	if(%this.isZoneBrick == 1)
		%this.physicalZone.waterViscosity = mfloor(%viscosity);
}

function fxDTSBrick::setWaterDensity(%this,%density)
{
	if(isObject(%this) && !isObject(%this.physicalZone) && !isObject(%this.trigger))
	{
		if(%brickData.isPremadeZoneBrick)
			%this.LoadZoneParameters();
		else
			%this.setBrickZone();
	}
	
	if(%this.isZoneBrick == 1)
		%this.physicalZone.waterDensity = %density;
}