//Title: Shark Camera Fix
//Author: King of the Bill

deactivatePackage(sharkCameraFix);
package sharkCameraFix
{
	function holeSharkKill(%obj,%col)
	{
		if( !isObject(%obj) || %obj.isDisabled() )
			return;
			
		// we can no longer damage the guy release him
		if( miniGameCanDamage( %obj, %col ) != 1 )
		{
			%col.disMount();
			
			if( %col.client )
				%col.client.setControlObject( %col );
		
			return;
		}
		
		if(%obj.getMountedObject(0) == %col)
		{
			if(%col.getClassName() $= "Player" )
			{
				%col.damage(%obj.hFakeProjectile, %col.getposition(), 1000, $DamageType::SharkHoleBite);

				
				%col.client.camera.setFlyMode();
			}
			else
			{
				if(%obj.hIsInfected && getRandom(0,2))
				{
					holeZombieInfect(%obj,%col);
					%col.dismount();
					%col.playThread(0,root);
					%obj.playThread(1,root);
					%obj.hIgnore = 0;
					%obj.hEating = 0;
				}
				else
				{
					%col.kill();
					%obj.playThread(1,root);
					%obj.hEating = 0;
				}
			}
		}
		%obj.startHoleLoop();
	}
};
activatePackage(sharkCameraFix);