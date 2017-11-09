//function that flips vehicles
function doVehicleFlip(%client, %vector)
{
	if(!$Pref::Server::FASTKarts::KartFlipping)
		return;
	
	if(!isObject(%client.player))
		return;
	
	%vehicle = %client.player.getObjectMount();
	if(!isObject(%vehicle))
		return;
	
	if(%vehicle.getclassname() $= "WheeledVehicle" || %vehicle.getclassname() $= "FlyingVehicle")
	{
		if(!%vehicle.getWheelPowered(2))
			return;
		
		%mountedObj = %vehicle.getMountNodeObject(0);
		if(%mountedObj != %client.player)
			return;
		
		if(%vector $= "")
			%vector = $Pref::Server::FASTKarts::KartFlippingMultiplier; //original function
		
		%vehicle.setangularvelocity(%vector);
	}
}

//command to flip vehicles
function servercmdflip(%client)
{
	if(!$Pref::Server::FASTKarts::KartFlipping)
		return;
	
	if(!isObject(%client.player))
		return;
	
	%vehicle = %client.player.getObjectMount();
	if(!isObject(%vehicle))
		return;
	
	doVehicleFlip(%client, $Pref::Server::FASTKarts::KartFlippingMultiplier); //original function
}

//this package allows players to change vehicle orientation in the air
if(isPackage(flipVehicleCommandPackage))
	deactivatePackage(flipVehicleCommandPackage);
package flipVehicleCommandPackage
{
	function serverCmdPlantBrick(%client, %a, %b, %c, %d, %e, %f)
	{
		if($Pref::Server::FASTKarts::KartFlipping)
		{
			%vehicle = %client.player.getObjectMount();
			if(isObject(%vehicle))
			{
				//enter
				doVehicleFlip(%client, $Pref::Server::FASTKarts::KartFlippingMultiplier); //original function
			}
		}
		parent::serverCmdPlantBrick(%client, %a, %b, %c, %d, %e, %f);
	}
	
	function serverCmdRotateBrick(%client, %a, %b, %c, %d, %e, %f)
	{
		if($Pref::Server::FASTKarts::KartFlipping)
		{
			%vehicle = %client.player.getObjectMount();
			if(isObject(%vehicle))
			{
				if(%a == -1)
				{
					//7
					%vector = %vehicle.getUpVector();
					%vector = vectorScale(%vector, $Pref::Server::FASTKarts::KartFlippingMultiplier);
					doVehicleFlip(%client, %vector);
				}
				if(%a == 1)
				{
					//9
					%vector = %vehicle.getUpVector();
					%vector = vectorScale(%vector, -$Pref::Server::FASTKarts::KartFlippingMultiplier);
					doVehicleFlip(%client, %vector);
				}
			}
		}
		parent::serverCmdRotateBrick(%client, %a, %b, %c, %d, %e, %f);
	}
	
	function serverCmdShiftBrick(%client, %a, %b, %c, %d, %e, %f)
	{
		if($Pref::Server::FASTKarts::KartFlipping)
		{
			%vehicle = %client.player.getObjectMount();
			if(isObject(%vehicle))
			{
				if(%a == 1)
				{
					//8
					%isUpright = getWord(%vehicle.getUpVector(), 2);
					%vector = %vehicle.getForwardVector();
					if(%isUpright >= 0)
						%vector = vectorScale(%vector, -$Pref::Server::FASTKarts::KartFlippingMultiplier);
					else
						%vector = vectorScale(%vector, $Pref::Server::FASTKarts::KartFlippingMultiplier);
					%vec0 = getWord(%vector, 0);
					%vec1 = getWord(%vector, 1);
					%vec2 = getWord(%vector, 2);
					%vec0 = %vec0 * -1;
					%vector = %vec1 SPC %vec0 SPC 0; //SPC %vec2;
					%vector = vectorScale(vectorNormalize(%vector), $Pref::Server::FASTKarts::KartFlippingMultiplier);
					doVehicleFlip(%client, %vector);
				}
				if(%a == -1)
				{
					//2
					%isUpright = getWord(%vehicle.getUpVector(), 2);
					%vector = %vehicle.getForwardVector();
					if(%isUpright >= 0)
						%vector = vectorScale(%vector, $Pref::Server::FASTKarts::KartFlippingMultiplier);
					else
						%vector = vectorScale(%vector, -$Pref::Server::FASTKarts::KartFlippingMultiplier);
					%vec0 = getWord(%vector, 0);
					%vec1 = getWord(%vector, 1);
					%vec2 = getWord(%vector, 2);
					%vec0 = %vec0 * -1;
					%vector = %vec1 SPC %vec0 SPC 0; //SPC %vec2;
					%vector = vectorScale(vectorNormalize(%vector), $Pref::Server::FASTKarts::KartFlippingMultiplier);
					doVehicleFlip(%client, %vector);
				}
				if(%b == 1)
				{
					//4
					%vector = %vehicle.getForwardVector();
					%vector = vectorScale(%vector, -$Pref::Server::FASTKarts::KartFlippingMultiplier);
					doVehicleFlip(%client, %vector);
				}
				if(%b == -1)
				{
					//6
					%vector = %vehicle.getForwardVector();
					%vector = vectorScale(%vector, $Pref::Server::FASTKarts::KartFlippingMultiplier);
					doVehicleFlip(%client, %vector);
				}
				if(%c == 1)
				{
					//3
					%vector = %vehicle.getUpVector();
					%vector = vectorScale(%vector, -$Pref::Server::FASTKarts::KartFlippingMultiplier);
					doVehicleFlip(%client, %vector);
				}
				if(%c == 3)
				{
					//+
					doVehicleFlip(%client, -$Pref::Server::FASTKarts::KartFlippingMultiplier); //mirror of original function
				}
				if(%c == -1)
				{
					//1
					%vector = %vehicle.getUpVector();
					%vector = vectorScale(%vector, $Pref::Server::FASTKarts::KartFlippingMultiplier);
					doVehicleFlip(%client, %vector);
				}
				//if(%c == -3)
				//{
					//5
				//}
			}
		}
		parent::serverCmdShiftBrick(%client, %a, %b, %c, %d, %e, %f);
	}
	
	//function serverCmdCancelBrick(%client, %a, %b, %c, %d, %e, %f)
	//{
		//if($Pref::Server::FASTKarts::KartFlipping)
		//{
			//%vehicle = %client.player.getObjectMount();
			//if(isObject(%vehicle))
			//{
				//0
			//}
		//}
		//parent::serverCmdCancelBrick(%client, %a, %b, %c, %d, %e, %f);
	//}
};
activatePackage(flipVehicleCommandPackage);