//command to flip vehicles

function servercmdflip(%client)
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
		
		%vehicle.setangularvelocity(10);
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