//Title: Bypass Vehicle Color Trust
//Author: piber20
//Bypasses the requirement of having trust to color vehicles.

if(isPackage(BypassVehicleColorTrustPackage))
	deactivatePackage(BypassVehicleColorTrustPackage);
package BypassVehicleColorTrustPackage
{
	function paintProjectile::onCollision(%this, %obj, %col, %bool, %pos, %normal)
	{
		parent::onCollision(%this, %obj, %col, %bool, %pos, %normal);
		if(isObject(%col))
		{
			if(%col.getclassname() $= "WheeledVehicle" || %col.getclassname() $= "FlyingVehicle")
			{
				if(%col.getWheelPowered(2))
					return;
				
				%id = %this.colorID;
				%color = getColorI(getColorIdTable(%id));
				%red = getWord(%color, 0);
				%red = %red / 255;
				%green = getWord(%color, 1);
				%green = %green / 255;
				%blue = getWord(%color, 2);
				%blue = %blue / 255;
				%alpha = 1;
				%col.setnodecolor("ALL", %red SPC %green SPC %blue SPC %alpha);
			}
		}
	}
};
activatePackage(BypassVehicleColorTrustPackage);