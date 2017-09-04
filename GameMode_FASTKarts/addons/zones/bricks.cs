if($Pref::Server::BrickZoneEvents::EnableBricks)
{
	//trampoline
	datablock fxDTSBrickData (brickzoneTrampData : brick8x8fData)
	{
		category = "Special";
		subCategory = "Interactive";
		uiName = "Trampoline";
		
		isPremadeZoneBrick = true;
		zoneDirection = 1;
		zoneAddition = 1;
		zoneAppliedForce = "0 0 30000";
		zoneDrag = 0;
		zoneBrickColliding = true;
		zoneBrickRendering = true;
		zoneBrickRayCasting = true;
		zoneBrickShapeFX = 0;
		zoneWater = false;
		zoneWaterViscosity = 0;
		zoneGravity = 1;
	};

	//mud
	datablock fxDTSBrickData (brickzonemudData : brick8x8fData)
	{
		category = "Special";
		subCategory = "Interactive";
		uiName = "Mud";
		
		isPremadeZoneBrick = true;
		zoneDirection = 1;
		zoneAddition = 1;
		zoneAppliedForce = "0 0 0";
		zoneDrag = 11;
		zoneBrickColliding = true;
		zoneBrickRendering = true;
		zoneBrickRayCasting = true;
		zoneBrickShapeFX = 0;
		zoneWater = false;
		zoneWaterViscosity = 0;
		zoneGravity = 1;
	};

	//airvent
	datablock fxDTSBrickData (brickzoneAirventData : brick6x6fData)
	{
		category = "Special";
		subCategory = "Interactive";
		uiName = "Air Vent";
		
		isPremadeZoneBrick = true;
		zoneDirection = 1;
		zoneAddition = 10;
		zoneAppliedForce = "0 0 4000";
		zoneDrag = 0;
		zoneBrickColliding = true;
		zoneBrickRendering = true;
		zoneBrickRayCasting = true;
		zoneBrickShapeFX = 0;
		zoneWater = false;
		zoneWaterViscosity = 0;
		zoneGravity = 1;
	};

	//boosters
	datablock fxDTSBrickData (brickzoneBoosterData)
	{
		brickFile = "./Booster.blb";
		category = "Special";
		subCategory = "Interactive";
		uiName = "Booster";
		iconName = "Add-Ons/GameMode_FASTKarts/addons/zones/Booster";
		
		isPremadeZoneBrick = true;
		zoneDirection = 1;
		zoneAddition = 1;
		zoneAppliedForce = "7000 0 0";
		zoneDrag = 0;
		zoneBrickColliding = true;
		zoneBrickRendering = true;
		zoneBrickRayCasting = true;
		zoneBrickShapeFX = 0;
		zoneWater = false;
		zoneWaterViscosity = 0;
		zoneGravity = 1;
	};

	datablock fxDTSBrickData (brickzoneVehicleBoosterData)
	{
		brickFile = "./Booster.blb";
		category = "Special";
		subCategory = "Interactive";
		uiName = "Vehicle Booster";
		iconName = "Add-Ons/GameMode_FASTKarts/addons/zones/Booster";
		
		isPremadeZoneBrick = true;
		zoneDirection = 1;
		zoneAddition = 8;
		zoneAppliedForce = "20000 0 0";
		zoneDrag = 0;
		zoneBrickColliding = true;
		zoneBrickRendering = true;
		zoneBrickRayCasting = true;
		zoneBrickShapeFX = 0;
		zoneWater = false;
		zoneWaterViscosity = 0;
		zoneGravity = 1;
	};

	//force gate
	datablock fxDTSBrickData (brickzoneForceGateData : brick1x4x5data)
	{
		category = "Special";
		subCategory = "Interactive";
		uiName = "Force Gate";
		
		isPremadeZoneBrick = true;
		zoneDirection = 0;
		zoneAddition = 0;
		zoneAppliedForce = "0 9000 0";
		zoneDrag = 0;
		zoneBrickColliding = false;
		zoneBrickRendering = false;
		zoneBrickRayCasting = false;
		zoneBrickShapeFX = 0;
		zoneWater = false;
		zoneWaterViscosity = 0;
		zoneGravity = 1;
	};

	//conveyor
	datablock fxDTSBrickData (brickzoneConveyorBeltData)
	{
		brickFile = "./ConveyorBelt.blb";
		category = "Special";
		subCategory = "Interactive";
		uiName = "ConveyorBelt";
		iconName = "Add-Ons/GameMode_FASTKarts/addons/zones/ConveyorBelt";
		
		isPremadeZoneBrick = true;
		zoneDirection = 1;
		zoneAddition = 1;
		zoneAppliedForce = "3700 0 0";
		zoneDrag = 0;
		zoneBrickColliding = true;
		zoneBrickRendering = true;
		zoneBrickRayCasting = true;
		zoneBrickShapeFX = 0;
		zoneWater = false;
		zoneWaterViscosity = 0;
		zoneGravity = 1;
	};

	//no gravity
	datablock fxDTSBrickData (brickzonenoGravData : brick8xCubeData)
	{
		subCategory = "Gravity";
		uiName = "Zero Gravity Zone";
		isWaterBrick = true;
		
		isPremadeZoneBrick = true;
		zoneDirection = 0;
		zoneAddition = 0;
		zoneAppliedForce = "0 0 0";
		zoneDrag = 0;
		zoneBrickColliding = false;
		zoneBrickRendering = false;
		zoneBrickRayCasting = false;
		zoneBrickShapeFX = 0;
		zoneWater = false;
		zoneWaterViscosity = 0;
		zoneGravity = 0;
	};

	datablock fxDTSBrickData (brickzonenoGrav32xData : brick32xCubeData)
	{
		subCategory = "Gravity";
		uiName = "Zero Gravity Zone 32x";
		isWaterBrick = true;
		
		isPremadeZoneBrick = true;
		zoneDirection = 0;
		zoneAddition = 0;
		zoneAppliedForce = "0 0 0";
		zoneDrag = 0;
		zoneBrickColliding = false;
		zoneBrickRendering = false;
		zoneBrickRayCasting = false;
		zoneBrickShapeFX = 0;
		zoneWater = false;
		zoneWaterViscosity = 0;
		zoneGravity = 0;
	};

	datablock fxDTSBrickData (brickzonenoGrav64xData : brick64xCubeData)
	{
		subCategory = "Gravity";
		uiName = "Zero Gravity Zone 64x";
		isWaterBrick = true;
		
		isPremadeZoneBrick = true;
		zoneDirection = 0;
		zoneAddition = 0;
		zoneAppliedForce = "0 0 0";
		zoneDrag = 0;
		zoneBrickColliding = false;
		zoneBrickRendering = false;
		zoneBrickRayCasting = false;
		zoneBrickShapeFX = 0;
		zoneWater = false;
		zoneWaterViscosity = 0;
		zoneGravity = 0;
	};

	//sludge
	datablock fxDTSBrickData (brickzoneSludgeData : brick8xWaterData)
	{
		uiName = "8x8 Sludge Water";
		isWaterBrick = true;
		
		isPremadeZoneBrick = true;
		zoneDirection = 0;
		zoneAddition = 0;
		zoneAppliedForce = "0 0 0";
		zoneDrag = 0;
		zoneBrickColliding = false;
		zoneBrickRendering = true;
		zoneBrickRayCasting = false;
		zoneBrickShapeFX = 2;
		zoneWater = true;
		zoneWaterViscosity = 180;
		zoneGravity = 1;
	};

	datablock fxDTSBrickData (brickzoneSludge32xData : brick32xWaterData)
	{
		uiName = "32x32 Sludge Water";
		isWaterBrick = true;
		
		isPremadeZoneBrick = true;
		zoneDirection = 0;
		zoneAddition = 0;
		zoneAppliedForce = "0 0 0";
		zoneDrag = 0;
		zoneBrickColliding = false;
		zoneBrickRendering = true;
		zoneBrickRayCasting = false;
		zoneBrickShapeFX = 2;
		zoneWater = true;
		zoneWaterViscosity = 180;
		zoneGravity = 1;
	};

	//waterfall
	datablock fxDTSBrickData (brickzoneWaterFallData : brick8xWaterData)
	{
		uiName = "8x8 WaterFall";
		isWaterBrick = true;
		
		isPremadeZoneBrick = true;
		zoneDirection = 0;
		zoneAddition = 0;
		zoneAppliedForce = "0 0 -7000";
		zoneDrag = 0;
		zoneBrickColliding = false;
		zoneBrickRendering = true;
		zoneBrickRayCasting = false;
		zoneBrickShapeFX = 2;
		zoneWater = true;
		zoneWaterViscosity = 0;
		zoneGravity = 1;
	};

	datablock fxDTSBrickData (brickzoneWater2x8FallData : brick8xWaterData)
	{
		brickFile = "./8xCube2x8.blb";
		uiName = "2x8 WaterFall";
		iconName = "Add-Ons/GameMode_FASTKarts/addons/zones/8xCube2x8";
		isWaterBrick = true;
		
		isPremadeZoneBrick = true;
		zoneDirection = 0;
		zoneAddition = 0;
		zoneAppliedForce = "0 0 -7000";
		zoneDrag = 0;
		zoneBrickColliding = false;
		zoneBrickRendering = false;
		zoneBrickRayCasting = false;
		zoneBrickShapeFX = 2;
		zoneWater = true;
		zoneWaterViscosity = 0;
		zoneGravity = 1;
	};

	//ladder
	datablock fxDTSBrickData (brickzoneLadderup : brick1x2x5data)
	{
		category = "Special";
		subCategory = "Interactive";
		uiName = "1x2x5 Ladder Helper";
		
		isPremadeZoneBrick = true;
		zoneDirection = 0;
		zoneAddition = 0;
		zoneAppliedForce = "0 1000 3000";
		zoneDrag = 2;
		zoneBrickColliding = false;
		zoneBrickRendering = false;
		zoneBrickRayCasting = false;
		zoneBrickShapeFX = 0;
		zoneWater = false;
		zoneWaterViscosity = 0;
		zoneGravity = 1;
	};
}

//functions to create brick zones
function fxDtsBrick::LoadZoneParameters(%this)
{
	if(!isObject(%this))
		return;
	
	%brickData = %this.getDatablock();
	if(!%brickData.isPremadeZoneBrick)
		return;
	
	%brickID = %brickData.getID();
	
	//get the zone parameters
	%dir = mfloor(%brickData.zoneDirection);
	%add = mfloor(%brickData.zoneAddition);
	%force = %brickData.zoneAppliedForce;
	%drag = mfloor(%brickData.zoneDrag);
	%col = mfloor(%brickData.zoneBrickColliding);
	%rend = mfloor(%brickData.zoneBrickRendering);
	%ray = mfloor(%brickData.zoneBrickRayCasting);
	%shape = mfloor(%brickData.zoneBrickShapeFX);
	%water = mfloor(%brickData.zoneWater);
	%vis = mfloor(%brickData.zoneWaterViscosity);
	%grav = mfloor(%brickData.zoneGravity);
	
	//do some trickery to allow applied force to rotate with brick
	%forcex = getword(%force, 0);
	%forcey = getword(%force, 1);
	%forcez = getword(%force, 2);
	
	%forwardx = vectorScale(%this.getForwardVector(), mfloor(%forcex));
	%newxx = getword(%forwardx, 0);
	%newxy = getword(%forwardx, 1);
	
	%forwardy = vectorScale(%this.getForwardVector(), mfloor(%forcey));
	%newyx = getword(%forwardy, 0);
	%newyy = getword(%forwardy, 1);
	
	%newx = mfloor(%newxx + %newyy);
	%newy = mfloor(%newyx + %newxy);
	
	%force = mfloor(%newx) SPC mfloor(%newy) SPC mfloor(%forcez);
	
	//apply the parameters
	%this.setBrickZone(true, %dir, %add);
	%this.setAppliedForce(%force);
	%this.setDrag(%drag);
	%this.setColliding(%col);
	%this.setRendering(%rend);
	%this.setRayCasting(%ray);
	%this.setShapeFX(%shape);
	%this.schedule(100, "setIsWater", %water);
	%this.schedule(150, "setWaterViscosity", %vis);
	%this.setGravity(%grav);
}

function fxDTSBrick::checkForZoneEvents(%this)
{
	if(!isObject(%this))
		return;
	if(isObject(%this.physicalZone))
		return;
	if(isObject(%this.trigger))
		return;
	if(%this.numEvents <= 0)
		return;
	
	for(%i = 0; %i < %this.numEvents; %i++)
	{
		%input = %this.eventInput[%i];
		
		if(
			(%input $= "onPlayerEnterBrick") ||
			(%input $= "onBotEnterBrick") ||
			(%input $= "onVehicleEnterBrick") ||
			(%input $= "onPlayerLeaveBrick") ||
			(%input $= "onBotLeaveBrick") ||
			(%input $= "onVehicleLeaveBrick") ||
			(%input $= "onPlayerInBrick") ||
			(%input $= "onBotInBrick") ||
			(%input $= "onVehicleInBrick")
		)
			%zoneEvents++;
	}
	
	if(%zoneEvents > 0)
	{
		%brickData = %this.getDatablock();
		
		if(%brickData.isPremadeZoneBrick)
			%this.LoadZoneParameters();
		else
			%this.setBrickZone();
	}
}

function fxDTSBrick::setBrickZone(%this, %active, %dir, %add)
{
	if(!isObject(%this))
		return;
	
	//delete old physical zones and triggers belonging to the brick
	if(isObject(%this.physicalZone))
		%this.physicalZone.delete();
	if(isObject(%this.trigger))
		%this.trigger.delete();
	
	//default values
	if(%active $= "")
		%active = true;
	if(%dir $= "")
		%dir = 0;
	if(%add $= "")
		%add = 0;
	
	if(%dir <= -1 || %dir >= 7) //delete the zone if it exists and do nothing else
	{
		%this.physicalZone = null;
		%this.trigger = null;
		%this.IsZoneBrick = 0;
		return;
	}
	
	if(%dir == 0)
	{
		%xOffset = 0;
		%yOffset = 0;
		%zOffset = 0;
		
		%zoneXa = getWord(%this.getWorldBox(), 0) - %add/10;
		%zoneYa = getWord(%this.getWorldBox(), 1) - %add/10;
		%zoneZa = getWord(%this.getWorldBox(), 2) - %add/10;
		
		%zoneXsize = getWord(%this.getWorldBox(), 3) - getWord(%this.getWorldBox(), 0) + %add/5;
		%zoneYsize = getWord(%this.getWorldBox(), 4) - getWord(%this.getWorldBox(), 1) + %add/5;
		%zoneZsize = (getWord(%this.getWorldBox(), 5) - getWord(%this.getWorldBox(), 2)) + %add/5;
		
		%zoneYa = %zoneYa + %zoneYsize;
	}
	if(%dir == 1)
	{	
		%add = %add * 0.5555;
		%xOffset = 0;
		%yOffset = 0;
		%zOffset = %add;
		
		%zoneXa = getWord(%this.getWorldBox(), 0);
		%zoneYa = getWord(%this.getWorldBox(), 1);
		%zoneZa = getWord(%this.getWorldBox(), 2);
		
		%zoneXsize = getWord(%this.getWorldBox(), 3) - getWord(%this.getWorldBox(), 0);
		%zoneYsize = getWord(%this.getWorldBox(), 4) - getWord(%this.getWorldBox(), 1);
		%zoneZsize = (getWord(%this.getWorldBox(), 5) - getWord(%this.getWorldBox(), 2)) + %zOffset;
		
		%zoneYa = %zoneYa + %zoneYsize;
	}
	if(%dir == 2)
	{
		%add = %add * 0.5555;
		%xOffset = 0;
		%yOffset = 0;
		%zOffset = %add;
		
		%zoneXsize = getWord(%this.getWorldBox(), 3) - getWord(%this.getWorldBox(), 0);
		%zoneYsize = getWord(%this.getWorldBox(), 4) - getWord(%this.getWorldBox(), 1);
		%zoneZsize = ((getWord(%this.getWorldBox(), 5) - getWord(%this.getWorldBox(), 2)) + %zOffset);
		
		%zoneXa = getWord(%this.getWorldBox(), 0);
		%zoneYa = getWord(%this.getWorldBox(), 1);
		%zoneZa = getWord(%this.getWorldBox(), 5);
		
		%zoneZa = %zoneZa - %zoneZsize;
		%zoneYa = %zoneYa + %zoneYsize;
	}
	if(%dir == 3)
	{
		%add = %add * 0.5;
		%xOffset = 0;
		%yOffset = %add;
		%zOffset = 0;
		
		%zoneXa = getWord(%this.getWorldBox(), 0);
		%zoneYa = getWord(%this.getWorldBox(), 1);
		%zoneZa = getWord(%this.getWorldBox(), 2);
	
		%zoneXsize = getWord(%this.getWorldBox(), 3) - getWord(%this.getWorldBox(), 0);
		%zoneYsize = (getWord(%this.getWorldBox(), 4) - getWord(%this.getWorldBox(), 1)) + %yOffset;
		%zoneZsize = (getWord(%this.getWorldBox(), 5) - getWord(%this.getWorldBox(), 2));
		
		%zoneYa = %zoneYa + %zoneYsize;
	}
	if(%dir == 4)
	{
		%add = %add * 0.5;
		%xOffset = 0;
		%yOffset = %add;
		%zOffset = 0;
		
		%zoneXa = getWord(%this.getWorldBox(), 0);
		%zoneYa = getWord(%this.getWorldBox(), 4);
		%zoneZa = getWord(%this.getWorldBox(), 2);
		
		%zoneXsize = getWord(%this.getWorldBox(), 3) - getWord(%this.getWorldBox(), 0);
		%zoneYsize = (getWord(%this.getWorldBox(), 4) - getWord(%this.getWorldBox(), 1)) + %yOffset;
		%zoneZsize = (getWord(%this.getWorldBox(), 5) - getWord(%this.getWorldBox(), 2));
	}
	if(%dir == 5)
	{
		%add = %add * 0.5;
		%xOffset = %add;
		%yOffset = 0;
		%zOffset = 0;
		
		%zoneXa = getWord(%this.getWorldBox(), 0);
		%zoneYa = getWord(%this.getWorldBox(), 1);
		%zoneZa = getWord(%this.getWorldBox(), 2);
		
		%zoneXsize = getWord(%this.getWorldBox(), 3) - getWord(%this.getWorldBox(), 0) + %xOffset;
		%zoneYsize = getWord(%this.getWorldBox(), 4) - getWord(%this.getWorldBox(), 1);
		%zoneZsize = (getWord(%this.getWorldBox(), 5) - getWord(%this.getWorldBox(), 2));
		
		%zoneYa = %zoneYa + %zoneYsize;
	}
	if(%dir == 6)
	{
		%add = %add * 0.5;
		%xOffset = %add;
		%yOffset = 0;
		%zOffset = 0;
		
		%zoneXa = getWord(%this.getWorldBox(), 3);
		%zoneYa = getWord(%this.getWorldBox(), 1);
		%zoneZa = getWord(%this.getWorldBox(), 2);
		
		%zoneXsize = getWord(%this.getWorldBox(), 3) - getWord(%this.getWorldBox(), 0) + %xOffset;
		%zoneYsize = getWord(%this.getWorldBox(), 4) - getWord(%this.getWorldBox(), 1);
		%zoneZsize = (getWord(%this.getWorldBox(), 5) - getWord(%this.getWorldBox(), 2));
		
		%zoneYa = %zoneYa + %zoneYsize;
		%zoneXa = %zoneXa - %zoneXsize;
	}
	
	%polySuffA = 0;
	%zoneShape = "0.0 0.0 0.0 1.0 0.0 0.0 0.0 -1.0 0.0 0.0 0.0 1.0";
	
	%pz = new PhysicalZone()
	{
		position = %zoneXa SPC %zoneYa SPC %zoneZa;
		velocityMod = "1";
		gravityMod = "1";
		extraDrag = "0";
		isWater = "0";
		waterViscosity = "40";
		waterDensity = "1";
		waterColor = "0.200000 0.600000 0.600000 0.300000";
		appliedForce = "0 0 0";
		polyhedron = %zoneShape;
	};
	missionCleanup.add(%pz);

	//Create a new trigger
	%tr = new trigger()
	{
		position = %zoneXa SPC %zoneYa SPC %zoneZa;
		polyhedron = %zoneShape;
		dataBlock = BrickZoneTrigger;
	};
	missionCleanup.add(%tr);
		
	//we already have created both the physicalzone and the trigger 
	//- now we stretch them to the sizes depicted above.
	
	%pz.setScale(%zoneXsize SPC %zoneYsize SPC %zoneZsize);
	%tr.setScale(%zoneXsize SPC %zoneYsize SPC %zoneZsize);
	
	if(!isObject(%pz))
	{
		error("Zone Events ERROR: Could not create a physical zone!, There are probably too many in the server!");
		return;
	}

	//Important Variables right here:!!
	%this.IsZoneBrick = 1;
	%this.physicalZone = %pz;
	
	%this.trigger = %tr;		
	%this.trigger.active = mfloor(%active);
	%tr.istriggerBricktrigger = true;
	%tr.triggerBrick = %this;   //this helps us find the brick the trigger is connected to.
	
	if(mfloor(%active) == true)
		%this.physicalZone.activate();
	else
		%this.physicalZone.deactivate();
}