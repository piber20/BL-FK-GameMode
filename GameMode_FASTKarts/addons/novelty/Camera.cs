datablock ParticleData(cameraFlashParticle)
{
	dragCoefficient      = 3;
	gravityCoefficient   = 0;
	inheritedVelFactor   = 0.2;
	constantAcceleration = 0.0;
	lifetimeMS           = 25;
	lifetimeVarianceMS   = 15;
	textureName          = "base/data/particles/star1";
	spinSpeed		= 10.0;
	spinRandomMin		= -500.0;
	spinRandomMax		= 500.0;
	colors[0]     = "1 1 1 0.9";
	colors[1]     = "1 1 1 0.4";
	sizes[0]      = 0.5;
	sizes[1]      = 1.0;

	useInvAlpha = false;
};

datablock ParticleEmitterData(cameraFlashEmitter)
{
   ejectionPeriodMS = 3;
   periodVarianceMS = 0;
   ejectionVelocity = 1.0;
   velocityVariance = 1.0;
   ejectionOffset   = 0.0;
   thetaMin         = 0;
   thetaMax         = 90;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = "cameraFlashParticle";
};

datablock ProjectileData(CameraProjectile)
{
   projectileShapeName = "base/data/shapes/empty.dts";
   directDamage        = 0;

   brickExplosionRadius = 0;
   brickExplosionImpact = false;          //destroy a brick if we hit it directly?
   brickExplosionForce  = 0;
   brickExplosionMaxVolume = 0;          //max volume of bricks that we can destroy
   brickExplosionMaxVolumeFloating = 0;  //max volume of bricks that we can destroy if they aren't connected to the ground

   muzzleVelocity      = 0;
   velInheritFactor    = 0;

   armingDelay         = 00;
   lifetime            = 100;
   fadeDelay           = 3500;
   bounceElasticity    = 0.5;
   bounceFriction      = 0.20;
   isBallistic         = false;
   gravityMod = 0.0;

   hasLight    = true;
   lightRadius = 15.0;
   lightColor  = "1 1 1";

   uiName = "Camera Flash";
};

//////////
// item //
//////////
datablock ItemData(PortableCameraItem)
{
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system

	 // Basic Item Properties
	shapeFile = "./Camera.dts";
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui stuff
	uiName = "Camera";
	iconName = "./icon_Camera";
	doColorShift = true;
	colorShiftColor = "1 1 1 1.000";

	 // Dynamic properties defined by the scripts
	image = PortableCameraImage;
	canDrop = true;
};

////////////////
//weapon image//
////////////////
datablock ShapeBaseImageData(PortableCameraImage)
{
   // Basic Item properties
   shapeFile = "./Camera.dts";
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
   item = PortableCameraItem;
   projectile = CameraProjectile;
   projectileType = Projectile;
   ammo = " ";

   //melee particles shoot from eye node for consistancy
   melee = false;
   //raise your arm up or not
   armReady = true;

   doColorShift = true;
   colorShiftColor = PortableCameraItem.colorShiftColor;//"0.400 0.196 0 1.000";

   //casing = " ";

   // Images have a state system which controls how the animations
   // are run, which sounds are played, script callbacks, etc. This
   // state system is downloaded to the client so that clients can
   // predict state changes and animate accordingly.  The following
   // system supports basic ready->fire->reload transitions as
   // well as a no-ammo->dryfire idle state.

   // Initial start up state
	stateName[0]                     = "Activate";
	stateTimeoutValue[0]             = 0.15;
	stateTransitionOnTimeout[0]       = "Ready";
	stateSound[0]					= weaponSwitchSound;

	stateName[1]                     = "Ready";
	stateTransitionOnTriggerDown[1]  = "Prepare";
	stateAllowImageChange[1]         = true;
	stateSequence[1]	= "Ready";

	stateName[2] = "Prepare";
	stateTimeoutValue[2]            = 0.8;
	stateTransitionOnTimeout[2]     = "Fire";
	stateSequence[2]		= "fire";

	stateName[3] = "Fire";
	stateTimeoutValue[3]            = 1.1;
	stateEmitter[3]					= cameraFlashEmitter;
	stateEmitterTime[3]				= 0.2;
	stateEmitterNode[3]				= "muzzleNode";
	stateTransitionOnTimeout[3]     = "Reload";
	stateScript[3]		= "OnFire";

	stateName[4]			= "Reload";
	stateSequence[4]                = "Reload";
	stateTransitionOnTriggerUp[4]     = "Ready";
	stateSequence[4]	= "Ready";
};

function CameraProjectile::OnExplode(%this, %obj)
{
	%obj.delete();
}