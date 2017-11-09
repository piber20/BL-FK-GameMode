//command to flip vehicles

function servercmdflip(%client, %vector)
{
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

if(isPackage(flipVehicleCommandPackage))
	deactivatePackage(flipVehicleCommandPackage);
package flipVehicleCommandPackage
{
	function servercmdplantbrick(%client, %a, %b, %c, %d, %e, %f)
	{
		servercmdflip(%client);
		parent::servercmdplantbrick(%client, %a, %b, %c, %d, %e, %f);
	}
};
activatePackage(flipVehicleCommandPackage);