datablock ProjectileData(BananaProjectile)
{
   projectileShapeName = "./BananaPeel.dts";
   directDamage        = 0;

   brickExplosionRadius = 0;
   brickExplosionImpact = false; //destroy a brick if we hit it directly?
   brickExplosionForce  = 0;
   brickExplosionMaxVolume = 0;
   brickExplosionMaxVolumeFloating = 0;

   muzzleVelocity      = 15;
   velInheritFactor    = 1;

   armingDelay         = 0;
   lifetime            = 15000;
   fadeDelay           = 3500;
   bounceElasticity    = 0.1;
   bounceFriction      = 0;
   isBallistic         = true;
   gravityMod = 1;
   explodeonDeath = true;

   hasLight    = false;
   lightRadius = 3.0;
   lightColor  = "0 0 0.5";

   uiName = "";
};

//////////
// item //
//////////
datablock ItemData(BananaItem)
{
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system

	 // Basic Item Properties
	shapeFile = "./Banana.dts";
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui stuff
	uiName = "Banana";
	iconName = "./icon_Banana";
	doColorShift = true;
	colorShiftColor = "1 1 0 1.000";

	 // Dynamic properties defined by the scripts
	image = BananaImage;
	canDrop = true;
};

////////////////
//weapon image//
////////////////
datablock ShapeBaseImageData(BananaImage)
{
   // Basic Item properties
   shapeFile = "./Banana.dts";
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

   projectile = BananaProjectile;
   projectileType = Projectile;

   // Add the WeaponImage namespace as a parent, WeaponImage namespace
   // provides some hooks into the inventory system.
   className = "WeaponImage";

   // Projectile && Ammo.
   item = BananaItem;
   ammo = " ";

   //melee particles shoot from eye node for consistancy
   melee = false;
   //raise your arm up or not
   armReady = true;
   minShotTime = 500;   //minimum time allowed between shots (needed to prevent equip/dequip exploit)

   doColorShift = true;
   colorShiftColor = BananaItem.colorShiftColor;//"0.400 0.196 0 1.000";

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

	stateName[2]		= "Prepare";
	stateTransitionOnTriggerDown[2] = "Fire";
	stateTransitionOnTriggerUp[2] = "Ready";
	stateTimeoutValue[2]	= 1;
	stateScript[2]		= "onPrepare";

	stateName[3] = "Fire";
	stateTimeoutValue[3]            = 1.1;
	stateTransitionOnTimeout[3]     = "Reload";
	stateSequence[3]		= "fire";
	stateScript[3]		= "OnFire";

	stateName[4]			= "Reload";
	stateSequence[4]                = "Reload";
	stateTransitionOnTriggerUp[4]     = "Ready";
	stateSequence[4]	= "Ready";
};

function BananaImage::onPrepare(%this, %obj, %slot)
{
   %obj.playthread(2, SpearReady);
}

function BananaImage::onFire(%this, %obj, %slot)
{
   Parent::onFire(%this, %obj, %slot);
   %obj.playthread(2, spearThrow);
   //%obj.tool[%obj.currTool] = 0;
   //%obj.weaponCount--;
   //messageClient(%obj.client,'MsgItemPickup','',%obj.currTool,0);
   //serverCmdUnUseTool(%obj.client);
}