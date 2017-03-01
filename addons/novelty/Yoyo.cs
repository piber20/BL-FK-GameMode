//////////
// item //
//////////
datablock ItemData(YoyoItem)
{
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system

	 // Basic Item Properties
	shapeFile = "./Yoyo.dts";
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui stuff
	uiName = "Yo-yo";
	iconName = "./icon_Yoyo";
	doColorShift = true;
	colorShiftColor = "0 1 1 1.000";

	 // Dynamic properties defined by the scripts
	image = YoyoImage;
	canDrop = true;
};

////////////////
//weapon image//
////////////////
datablock ShapeBaseImageData(YoyoImage)
{
   // Basic Item properties
   shapeFile = "./Yoyo.dts";
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
   item = YoyoItem;
   projectile = emptyProjectile;
   projectileType = Projectile;
   ammo = " ";

   //melee particles shoot from eye node for consistancy
   melee = false;
   //raise your arm up or not
   armReady = true;

   doColorShift = true;
   colorShiftColor = YoyoItem.colorShiftColor;//"0.400 0.196 0 1.000";

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
	stateTransitionOnTriggerDown[1]  = "Fire";
	stateAllowImageChange[1]         = true;
	stateSequence[1]	= "Ready";

	stateName[2] = "Fire";
	stateTimeoutValue[2]            = 1.1;
	stateTransitionOnTimeout[2]     = "Reload";
	stateSequence[2]		= "fire";
	stateScript[2]		= "OnFire";

	stateName[3]			= "Reload";
	stateSequence[3]                = "Reload";
	stateTransitionOnTriggerUp[3]     = "Ready";
	stateSequence[3]	= "Ready";
};