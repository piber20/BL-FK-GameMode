//command to flip vehicles
function servercmdflip(%client, %vector)
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
			%vector = 10; //original function
		
		%vehicle.setangularvelocity(%vector);
	}
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
				servercmdflip(%client, 10); //original function
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
					%vector = vectorScale(%vector, 10);
					servercmdflip(%client, %vector);
				}
				if(%a == 1)
				{
					//9
					%vector = %vehicle.getUpVector();
					%vector = vectorScale(%vector, -10);
					servercmdflip(%client, %vector);
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
					%vector = %vehicle.getForwardVector();
					%vector = vectorScale(%vector, -10);
					%vec0 = getWord(%vector, 0);
					%vec1 = getWord(%vector, 1);
					%vec2 = getWord(%vector, 2);
					%vec0 = %vec0 * -1;
					%vector = %vec1 SPC %vec0 SPC 0; //SPC %vec2;
					servercmdflip(%client, %vector);
				}
				if(%a == -1)
				{
					//2
					%vector = %vehicle.getForwardVector();
					%vector = vectorScale(%vector, 10);
					%vec0 = getWord(%vector, 0);
					%vec1 = getWord(%vector, 1);
					%vec2 = getWord(%vector, 2);
					%vec0 = %vec0 * -1;
					%vector = %vec1 SPC %vec0 SPC 0; //SPC %vec2;
					servercmdflip(%client, %vector);
				}
				if(%b == 1)
				{
					//4
					%vector = %vehicle.getForwardVector();
					%vector = vectorScale(%vector, -10);
					servercmdflip(%client, %vector);
				}
				if(%b == -1)
				{
					//6
					%vector = %vehicle.getForwardVector();
					%vector = vectorScale(%vector, 10);
					servercmdflip(%client, %vector);
				}
				//if(%c == 1)
				//{
					//3
				//}
				if(%c == 3)
				{
					//+
					servercmdflip(%client, -10); //mirror of original function
				}
				//if(%c == -1)
				//{
					//1
				//}
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