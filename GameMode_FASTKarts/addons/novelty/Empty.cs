datablock ProjectileData(emptyProjectile)
{
   projectileShapeName = "base/data/shapes/empty.dts";
   directDamage        = 0;
   directDamageType    = $DamageType::Gun;
   radiusDamageType    = $DamageType::Gun;

   brickExplosionRadius = 0;
   brickExplosionImpact = true;          //destroy a brick if we hit it directly?
   brickExplosionForce  = 0;
   brickExplosionMaxVolume = 0;          //max volume of bricks that we can destroy
   brickExplosionMaxVolumeFloating = 0;  //max volume of bricks that we can destroy if they aren't connected to the ground

   impactImpulse	     = 0;
   verticalImpulse	  = 0;
   //explosion           = Explosion;
   //particleEmitter     = ""; //bulletTrailEmitter;

   muzzleVelocity      = 0;
   velInheritFactor    = 0;

   armingDelay         = 0;
   lifetime            = 0;
   fadeDelay           = 0;
   bounceElasticity    = 0;
   bounceFriction      = 0;
   isBallistic         = false;
   gravityMod = 0.0;

   hasLight    = false;
   lightRadius = 3.0;
   lightColor  = "0 0 0.5";
};

//////////
// item //
//////////
datablock ItemData(emptyItem)
{
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system

	 // Basic Item Properties
	shapeFile = "base/data/shapes/empty.dts";
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui stuff
	uiName = "   ";
	iconName = "./Icon_Empty";
	doColorShift = true;
	colorShiftColor = "1 1 1 1.000";

	 // Dynamic properties defined by the scripts
	image = EmptyImage;
	canDrop = false;
};

////////////////
//weapon image//
////////////////
datablock ShapeBaseImageData(EmptyImage)
{
   // Basic Item properties
   shapeFile = "base/data/shapes/empty.dts";
   emap = true;

   // Specify mount point & offset for 3rd person, and eye offset
   // for first person rendering.
   mountPoint = 0;
   offset = "0 0 0";
   eyeOffset = 0; //"0.7 1.2 -0.5";
   rotation = eulerToMatrix( "0 0 0" );

   // When firing from a point offset from the eye, muzzle correction
   // will adjust the muzzle vector to point to the eye LOS point.
   // Since this weapon doesn't actually fire from the muzzle point,
   // we need to turn this off.  
   correctMuzzleVector = true;

   // Add the WeaponImage namespace as a parent, WeaponImage namespace
   // provides some hooks into the inventory system.
   className = "WeaponImage";

   // Projectile && Ammo.
   item = emptyItem;
   ammo = " ";
   projectile = emptyProjectile;
   projectileType = Projectile;

   //melee particles shoot from eye node for consistancy
   melee = false;
   //raise your arm up or not
   armReady = true;

   doColorShift = true;
   colorShiftColor = emptyItem.colorShiftColor;//"0.400 0.196 0 1.000";

   //casing = " ";

   // Images have a state system which controls how the animations
   // are run, which sounds are played, script callbacks, etc. This
   // state system is downloaded to the client so that clients can
   // predict state changes and animate accordingly.  The following
   // system supports basic ready->fire->reload transitions as
   // well as a no-ammo->dryfire idle state.

   // Initial start up state
	stateName[0]                     = "Activate";
	stateTimeoutValue[0]             = 0.01;
	stateTransitionOnTimeout[0]       = "Ready";

	stateName[1]                     = "Ready";
	stateTransitionOnTriggerDown[1]  = "Fire";
	stateAllowImageChange[1]         = true;
	stateSequence[1]	= "Ready";

	stateName[2]                    = "Fire";
	stateTransitionOnTimeout[2]     = "Reload";
	stateTimeoutValue[2]            = 0.01;
	stateFire[2]                    = true;
	stateAllowImageChange[2]        = false;
	stateSequence[2]                = "Fire";
	stateScript[2]                  = "onFire";
	stateWaitForTimeout[2]			= true;
	stateAllowImageChange[2]         = true;

	stateName[3]			= "Reload";
	stateSequence[3]                = "Reload";
	stateTransitionOnTriggerUp[3]     = "Ready";
	stateSequence[3]	= "Ready";
};

function EmptyImage::OnFire(%this, %obj, %slot)
{
	%obj.playthread(2, activate);
}