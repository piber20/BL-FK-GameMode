//zone datablock
datablock TriggerData(BrickZoneTrigger)
{
	tickPeriodMS = $Pref::Server::BrickZoneEvents::TickRate;
};

//Trigger Methods
//on tick trigger gives no info on what is in the zone...
//an array is used to store object values for each trigger.
function BrickZoneTrigger::onTickTrigger(%this,%trigger)
{
	if(%trigger.Active == true)
	{
		for(%n = 0; %n < 10; %n = %n + 1) 
		{
			%CurrentObject = %trigger.ObjArray[%n];
			
			if(isObject(%CurrentObject))
			{
				if(%CurrentObject.getClassName() $= "Player")
					%trigger.triggerBrick.onPlayerInBrick(%CurrentObject);
				else if(%CurrentObject.getClassName() $= "AIPlayer")
					%trigger.triggerBrick.onBotInBrick(%CurrentObject);
				else if((%CurrentObject.getClassName() $= "WheeledVehicle") || (%CurrentObject.getClassName() $= "FlyingVehicle"))
					%trigger.triggerBrick.onVehicleInBrick(%CurrentObject);
				else
					%trigger.ObjArray[%n] = null; //remove it as this object should not be in the list
			}
		}
	}
}

function BrickZoneTrigger::onEnterTrigger(%this,%trigger,%obj)
{
	if(%trigger.Active == true)
	{
		if(%obj.getClassName() $= "Player")
			%trigger.triggerBrick.onPlayerEnterBrick(%obj);
		else if(%obj.getClassName() $= "AIPlayer")
			%trigger.triggerBrick.onBotEnterBrick(%obj);
		else if((%obj.getClassName() $= "WheeledVehicle") || (%obj.getClassName() $= "FlyingVehicle"))
			%trigger.triggerBrick.onVehicleEnterBrick(%obj);
		else
			return;
		
		//find an empty slot..
		for(%n = 0; %n < 10; %n++)
		{
			//If the Slot is empty
			if(!isobject(%trigger.ObjArray[%n]))
			{
				%trigger.ObjArray[%n] = %obj; //Put our object in that empty slot.
				%n = 10; //stop the loop.
			}
		}
	}
}

function BrickZoneTrigger::onLeaveTrigger(%this,%trigger,%obj)
{
	if(%trigger.Active == true)
	{
		if(%obj.getClassName() $= "Player")
			%trigger.triggerBrick.onPlayerLeaveBrick(%obj);
		else if(%obj.getClassName() $= "AIPlayer")
			%trigger.triggerBrick.onBotLeaveBrick(%obj);
		else if((%obj.getClassName() $= "WheeledVehicle") || (%obj.getClassName() $= "FlyingVehicle"))
			%trigger.triggerBrick.onVehicleLeaveBrick(%obj);
		else
			return;
		
		//find the slot that is already holding this object!
		for(%n = 0; %n < 10; %n++)
		{	
			//If the Slot is Full with the match
			if(%trigger.ObjArray[%n] == %Obj)
				%trigger.ObjArray[%n] = null; //remove it from the list.
		}
	}
}

//vehicle movement in zones
function ImpulseVehicleWithZone(%obj, %brick)
{
	if(!%brick.isZoneBrick)
		return;
	if((VectorLen(%brick.physicalzone.appliedForce) == 0) && (%brick.physicalzone.gravitymod == 1) && (%brick.physicalzone.extraDrag == 0))
		return;
	
	%ZoneGravityMod = %brick.physicalzone.gravitymod * 10;
	%DragFactor = %brick.physicalzone.extraDrag / 30;
	%VehicleMassFactor = %obj.GetDatablock().mass * 0.004;
	%TimingFactor = 200 * 0.5;
	%ZoneForceImpulseScale = %TimingFactor * 0.01 * 0.5;
	%GravTrim = 1;
	
	%ZoneForceImpulseX = getWord(%brick.physicalzone.appliedForce,0) * %ZoneForceImpulseScale;
	%ZoneForceImpulseY = getWord(%brick.physicalzone.appliedForce,1) * %ZoneForceImpulseScale;
	%ZoneForceImpulseZ = getWord(%brick.physicalzone.appliedForce,2) * %ZoneForceImpulseScale;
	%ZoneForceImpulse = %ZoneForceImpulseX SPC %ZoneForceImpulseY SPC %ZoneForceImpulseZ;

	%ZoneGravityImpulseX = 0;
	%ZoneGravityImpulseY = 0;
	%ZoneGravityImpulseZ = ((%ZoneGravityMod - 1) * -1) * %VehicleMassFactor * %TimingFactor * %GravTrim; //if grav mod is 1 then nothing changes!
	%ZoneGravityImpulse = %ZoneGravityImpulseX SPC %ZoneGravityImpulseY SPC %ZoneGravityImpulseZ;
	
	%ResultantImpulse = VectorAdd(%ZoneForceImpulse, %ZoneGravityImpulse);
	%obj.applyImpulse(getWords(%obj.getTransform(), 0, 2), %ResultantImpulse);
	
	//drag stuff
	if(%DragFactor != 0)
	{
		//velocity modifiers..
		%ZoneDragVel = VectorScale(%obj.getVelocity(), %DragFactor * -1);
		if(VectorLen(%ZoneDragVel) >= VectorLen(%obj.getVelocity())) //maximum drag
			%ZoneDragVel = VectorScale(%obj.getVelocity(), -1);
		
		%obj.setVelocity(%ZoneDragVel);
	}
}