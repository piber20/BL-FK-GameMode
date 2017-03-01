datablock AudioProfile(SpeedKartlightCrashSound)
{
   filename    = "./SK_lightCrash.wav";
   description = AudioDefault3d;
   preload = true;
};

datablock AudioProfile(SpeedKartheavyCrashSound)
{
   filename    = "./SK_heavyCrash.wav";
   description = AudioDefault3d;
   preload = true;
};

datablock AudioProfile(SpeedKartIdleSound1)
{
	filename = "./SK_idle1.wav";
	description = AudioCloseLooping3d;
	preload = true;
};

datablock AudioProfile(SpeedKartIdleSound2)
{
	filename = "./SK_idle2.wav";
	description = AudioCloseLooping3d;
	preload = true;
};

datablock AudioProfile(SpeedKartIdleSound3)
{
	filename = "./SK_idle3.wav";
	description = AudioCloseLooping3d;
	preload = true;
};

datablock AudioProfile(SpeedKartIdleSound4)
{
	filename = "./SK_idle4.wav";
	description = AudioCloseLooping3d;
	preload = true;
};

datablock AudioProfile(SpeedKartIdleSound5)
{
	filename = "./SK_idle5.wav";
	description = AudioCloseLooping3d;
	preload = true;
};

datablock AudioProfile(SpeedKartIdleSound6)
{
	filename = "./SK_idle6.wav";
	description = AudioCloseLooping3d;
	preload = true;
};

datablock AudioProfile(SpeedKartIdleSound7)
{
	filename = "./SK_idle7.wav";
	description = AudioCloseLooping3d;
	preload = true;
};

datablock AudioProfile(SpeedKartIdleSound8)
{
	filename = "./SK_idle8.wav";
	description = AudioCloseLooping3d;
	preload = true;
};

datablock AudioProfile(SpeedKartEngineSound1)
{
	filename = "./SK_engine1.wav";
	description = AudioCloseLooping3d;
	preload = true;
};

datablock AudioProfile(SpeedKartEngineSound2)
{
	filename = "./SK_engine2.wav";
	description = AudioCloseLooping3d;
	preload = true;
};

datablock AudioProfile(SpeedKartEngineSound3)
{
	filename = "./SK_engine3.wav";
	description = AudioCloseLooping3d;
	preload = true;
};

datablock AudioProfile(SpeedKartEngineSound4)
{
	filename = "./SK_engine4.wav";
	description = AudioCloseLooping3d;
	preload = true;
};

datablock AudioProfile(SpeedKartEngineSound5)
{
	filename = "./SK_engine5.wav";
	description = AudioCloseLooping3d;
	preload = true;
};

datablock AudioProfile(SpeedKartEngineSound6)
{
	filename = "./SK_engine6.wav";
	description = AudioCloseLooping3d;
	preload = true;
};

datablock AudioProfile(SpeedKartEngineSound7)
{
	filename = "./SK_engine7.wav";
	description = AudioCloseLooping3d;
	preload = true;
};

datablock AudioProfile(SpeedKartEngineSound8)
{
	filename = "./SK_engine8.wav";
	description = AudioCloseLooping3d;
	preload = true;
};

datablock AudioProfile(SpeedKartHornSound1)
{
   filename    = "./SK_horn1.wav";
   description = AudioClose3d;
   preload = true;
};

datablock AudioProfile(SpeedKartHornSound2)
{
   filename    = "./SK_horn2.wav";
   description = AudioClose3d;
   preload = true;
};

datablock AudioProfile(SpeedKartHornSound3)
{
   filename    = "./SK_horn3.wav";
   description = AudioClose3d;
   preload = true;
};

datablock AudioProfile(SpeedKartHornSound4)
{
   filename    = "./SK_horn4.wav";
   description = AudioClose3d;
   preload = true;
};

datablock AudioProfile(SpeedKartHornSound5)
{
   filename    = "./SK_horn5.wav";
   description = AudioClose3d;
   preload = true;
};

datablock WheeledVehicleTire(SpeedKartTire)
{
   // Tires act as springs and generate lateral and longitudinal
   // forces to move the vehicle. These distortion/spring forces
   // are what convert wheel angular velocity into forces that
   // act on the rigid body.
   shapeFile = "./speedkarttire.dts";

   mass = 10;
   radius = 1;
   staticFriction = 5;
   kineticFriction = 5;
   restitution = 0.5;

   // Spring that generates lateral tire forces
   lateralForce = 6000;
   lateralDamping = 500;
   lateralRelaxation = 0.3;

   // Spring that generates longitudinal tire forces
   longitudinalForce = 5000;
   longitudinalDamping = 2000;
   longitudinalRelaxation = 0.1;
};

datablock WheeledVehicleTire(SpeedKartIITire : SpeedKartTire)
{
   shapeFile = "./speedkartiitire.dts";
};

datablock WheeledVehicleTire(SpeedKartATVTire : SpeedKartTire)
{
   shapeFile = "./speedkartatvtire.dts";
};

datablock WheeledVehicleTire(SpeedKart64Tire : SpeedKartTire)
{
   shapeFile = "./speedkart64tire.dts";
};

datablock WheeledVehicleTire(SpeedKartBlockoTire : SpeedKartTire)
{
   shapeFile = "./speedkartblockotire.dts";
};

datablock WheeledVehicleTire(SpeedKartHoverTire : SpeedKartTire)
{
   shapeFile = "./speedkarthovertire.dts";
};

datablock WheeledVehicleTire(SpeedKartHoverIITire : SpeedKartTire)
{
   shapeFile = "./speedkarthoveriitire.dts";
};

datablock WheeledVehicleTire(SpeedKartOriginalTire : jeepTire)
{
   shapeFile = "./speedkartoriginaltire.dts";
   kineticFriction = 9;
};

datablock WheeledVehicleSpring(SpeedKartSpring)
{
   // Wheel suspension properties
   length = 0.42;	// Suspension travel
   force = 3500;	// Spring force
   damping = 1200;	// Spring damping
   antiSwayForce = 9;// Lateral anti-sway force
};

datablock WheeledVehicleSpring(SpeedKartJetskiSpring : SpeedKartSpring)
{
   length = 0.9;
   antiSwayForce = 0;
};

datablock WheeledVehicleSpring(SpeedKartOriginalSpring : SpeedKartSpring)
{
   length = 0.4;
   force = 4000;
};

datablock WheeledVehicleSpring(SpeedKartDefaultSpring : SpeedKartSpring)
{
   length = 0.35;
   force = 4000;
   damping = 900;
   antiSwayForce = 3;
};

/////////////
// Vehicle //
/////////////
if($FK::Pref::Karts::UpDownLeaning)
{
	$FK::lookUpLimit = 0.6;
	$FK::lookDownLimit = 0.48;
}
else
{
	$FK::lookUpLimit = 0.5;
	$FK::lookDownLimit = 0.5;
}

datablock WheeledVehicleData(SpeedKartVehicle)
{
	category = "Vehicles";
	displayName = "SpeedKart";
	shapeFile = "./speedkart.dts";
	emap = true;
	minMountDist = 3;

   numMountPoints = 5;
   mountThread[0] = "sit";
   mountThread[1] = "crouch";
   mountThread[2] = "crouch";
   mountThread[3] = "crouch";
   mountThread[4] = "crouch";

	maxDamage = 200.00;
	destroyedLevel = 200.00;
	energyPerDamagePoint = 160;
	speedDamageScale = 1.04;
	collDamageThresholdVel = 20.0;
	collDamageMultiplier   = 0.02;

	massCenter = "0 -0.27 0.28";
	massBox = "0 0 0";

	maxSteeringAngle = 0.85; // Maximum steering angle, should match animation
	integration = 4; // Force integration time: TickSec/Rate
	tireEmitter = VehicleTireEmitter; // All the tires use the same dust emitter

	// 3rd person camera settings
	cameraRoll = false;		// Roll the camera with the vehicle
	cameraMaxDist = 9;		// Far distance from vehicle
	cameraOffset = 6;		// Vertical offset from camera mount point
	cameraLag = 0;		// Velocity lag of camera
	cameraDecay = 0;		// Decay per sec. rate of velocity lag
	cameraTilt = 0.3;
	collisionTol = 0.1;		// Collision distance tolerance
	contactTol = 0.1;

	useEyePoint = false;

	defaultTire	= SpeedKartTire;
	defaultSpring	= SpeedKartSpring;

	numWheels = 4;

	// Rigid Body
	mass = 300;
	density = 1.5;
	drag = 2.8;
	bodyFriction = 0.1;
	bodyRestitution = 0.1;
	minImpactSpeed = 5;	// Impacts over this invoke the script callback
	softImpactSpeed = 10;	// Play SoftImpact Sound
	hardImpactSpeed = 15;	// Play HardImpact Sound
	groundImpactMinSpeed = 5;

	// Engine
	engineTorque = 7000;	// Engine power
	engineBrake = 1000;	// Braking when throttle is 0
	brakeTorque = 16000;	// When brakes are applied
	maxWheelSpeed = 44.5;	// Engine scale by current speed / max speed

	rollForce		= 900;
	yawForce		= 600;
	pitchForce	= 1000;
	rotationalDrag	= 0;

	// Advanced Steering
	steeringAutoReturn = true;
	steeringAutoReturnRate = 0.9;
	steeringAutoReturnMaxSpeed = 10;
	steeringUseStrafeSteering = true;
	steeringStrafeSteeringRate = 0.1;

	// Energy
	maxEnergy = 0;
	jetForce = 0;
	minJetEnergy = 0;
	jetEnergyDrain = 0;

	splash = vehicleSplash;
	splashVelocity = 2;
	splashAngle = 60;
	splashFreqMod = 150;
	splashVelEpsilon = 0.5;
	bubbleEmitTime = 1.6;
	splashEmitter[0] = vehicleFoamDropletsEmitter;
	splashEmitter[1] = vehicleFoamEmitter;
	splashEmitter[2] = vehicleBubbleEmitter;
	mediumSplashSoundVelocity = 10.0;
	hardSplashSoundVelocity = 20.0;
	exitSplashSoundVelocity = 5.0;

	//jetSound = "";
	//SpeedKartEngineSound = "";
	//squealSound = "";
	softImpactSound = SpeedKartlightCrashSound;
	hardImpactSound = SpeedKartheavyCrashSound;
	//wheelImpactSound = "";
	UsedToBeShit = true;
	SpeedKartHornSound = SpeedKartHornSound1;
	SpeedKartIdleSound = SpeedKartIdleSound8; //the speedkart from the original mod used engine sounds closer to 1, however the superkart engine sounds are a little better i feel, so we're using those as the defaults.
	SpeedKartEngineSound = SpeedKartEngineSound8;

	uiName = "SpeedKart";
	rideable = true;
		lookUpLimit = $FK::lookUpLimit;
		lookDownLimit = $FK::lookDownLimit;

	paintable = true;

   damageEmitter[0] = VehicleBurnEmitter;
	damageEmitterOffset[0] = "0 0 0";
	damageLevelTolerance[0] = 0.99;

   damageEmitter[1] = VehicleBurnEmitter;
	damageEmitterOffset[1] = "0 0 0";
	damageLevelTolerance[1] = 1;

   numDmgEmitterAreas = 1;

   initialExplosionProjectile = SpeedKartExplosionProjectile;
   initialExplosionOffset = 0; //offset only uses a z value for now

   burnTime = 4000;

   finalExplosionProjectile = vehicleFinalExplosionProjectile;
   finalExplosionOffset = 0; //offset only uses a z value for now

   minRunOverSpeed = 4; //how fast you need to be going to run someone over (do damage)
   runOverDamageScale = 4; //when you run over someone, speed * runoverdamagescale = damage amt
   runOverPushScale = 0; //how hard a person you're running over gets pushed

   //protection for passengers
   protectPassengersBurn = true; //protect passengers from the burning effect of explosions?
   protectPassengersRadius = true; //protect passengers from radius damage (explosions) ?
   protectPassengersDirect = false; //protect passengers from direct damage (bullets) ?
   
   FKDescription = "A good starter.";
};

datablock WheeledVehicleData(SpeedKartIIVehicle : SpeedKartVehicle)
{
   displayName = "SuperKart";
   shapeFile = "./speedkartii.dts";
   massCenter = "0 -0.37 0.4";
   defaultTire = SpeedKartIITire;
   density = 1.9;
   drag = 2.6;
   engineTorque = 10000;
   uiName = "SuperKart";
   initialExplosionProjectile = SpeedKartIIExplosionProjectile;
	SpeedKartIdleSound = SpeedKartIdleSound8;
	SpeedKartEngineSound = SpeedKartEngineSound8;
	
	FKDescription = "";
};

datablock WheeledVehicleData(SpeedKart64Vehicle : SpeedKartVehicle)
{
   displayName = "SpeedKart 64";
   shapeFile = "./speedkart64.dts";
   massCenter = "0 -0.275 0.15";
   defaultTire = SpeedKart64Tire;
   density = 1.1;
   drag = 3.3;
   brakeTorque = 20000;
   uiName = "SpeedKart 64";
   initialExplosionProjectile = SpeedKart64ExplosionProjectile;
   
   FKDescription = "";
   
	maxDamage = 160.00;
	destroyedLevel = 160.00;
};

datablock WheeledVehicleData(SpeedKart7Vehicle : SpeedKartVehicle)
{
   displayName = "SpeedKart 7";
   shapeFile = "./speedkart7.dts";
   massCenter = "0 -0.275 0.28";
   density = 2.3;
   drag = 3;
   brakeTorque = 14000;
   uiName = "SpeedKart 7";
   
   FKDescription = "";
   
	maxDamage = 240.00;
	destroyedLevel = 240.00;
};

datablock WheeledVehicleData(SpeedKartATVVehicle : SpeedKartVehicle)
{
   displayName = "SuperKart ATV";
   shapeFile = "./speedkartatv.dts";
   mountThread[0] = "root";
   massCenter = "0 -0.254 0.535";
   defaultTire = SpeedKartATVTire;
   density = 3.4;
   drag = 3.6;
   engineTorque = 9000;
   brakeTorque = 18000;
   uiName = "SuperKart ATV";
   initialExplosionProjectile = SpeedKartATVExplosionProjectile;
   SpeedKartHornSound = SpeedKartHornSound2;
	SpeedKartIdleSound = SpeedKartIdleSound1;
	SpeedKartEngineSound = SpeedKartEngineSound1;
	
	FKDescription = "";
	
	maxDamage = 320.00;
	destroyedLevel = 320.00;
};

datablock WheeledVehicleData(SpeedKartBlockoVehicle : SpeedKartVehicle)
{
   displayName = "SpeedKart Blocko";
   shapeFile = "./speedkartblocko.dts";
   massCenter = "0 -0.105 0.28";
   defaultTire = SpeedKartBlockoTire;
   density = 1.4;
   drag = 2.2;
   uiName = "SpeedKart Blocko";
   engineTorque = 8000;
   brakeTorque = 18000;
   maxWheelSpeed = 45.6;
   initialExplosionProjectile = SpeedKartBlockoExplosionProjectile;
	SpeedKartIdleSound = SpeedKartIdleSound1;
	SpeedKartEngineSound = SpeedKartEngineSound1;
	
	FKDescription = "";
	
	maxDamage = 280.00;
	destroyedLevel = 280.00;
};

datablock WheeledVehicleData(SpeedKartBuggyVehicle : SpeedKartVehicle)
{
   displayName = "SpeedKart Buggy";
   shapeFile = "./speedkartbuggy.dts";
   massCenter = "0 -0.275 0.28";
   density = 3.9;
   drag = 3.1;
   engineTorque = 10000;
   brakeTorque = 14000;
   uiName = "SpeedKart Buggy";
   SpeedKartHornSound = SpeedKartHornSound2;
	SpeedKartIdleSound = SpeedKartIdleSound2;
	SpeedKartEngineSound = SpeedKartEngineSound2;
	
	FKDescription = "";
	
	maxDamage = 280.00;
	destroyedLevel = 280.00;
};

datablock WheeledVehicleData(SpeedKartClassicVehicle : SpeedKartVehicle)
{
   displayName = "SpeedKart Classic";
   shapeFile = "./speedkartclassic.dts";
   massCenter = "0 -0.275 0.28";
   density = 2.2;
   drag = 2;
   engineTorque = 8000;
   uiName = "SpeedKart Classic";
   
   FKDescription = "";
};

datablock WheeledVehicleData(SpeedKartClassicGTVehicle : SpeedKartVehicle)
{
   displayName = "SpeedKart Classic GT";
   shapeFile = "./speedkartclassicgt.dts";
   massCenter = "0 -0.275 0.28";
   density = 2.8;
   drag = 3;
   engineTorque = 9500;
   brakeTorque = 18000;
   maxWheelSpeed = 45.8;
   uiName = "SpeedKart Classic GT";
   SpeedKartHornSound = SpeedKartHornSound3;
	SpeedKartIdleSound = SpeedKartIdleSound3;
	SpeedKartEngineSound = SpeedKartEngineSound3;
	
	FKDescription = "";
};

datablock WheeledVehicleData(SpeedKartFormulaVehicle : SpeedKartVehicle)
{
   displayName = "SpeedKart Formula";
   shapeFile = "./speedkartformula.dts";
   massCenter = "0 -0.275 0.28";
   density = 5.9;
   drag = 3.3;
   engineTorque = 10000;
   brakeTorque = 12000;
   uiName = "SpeedKart Formula";
   SpeedKartHornSound = SpeedKartHornSound4;
	SpeedKartIdleSound = SpeedKartIdleSound3;
	SpeedKartEngineSound = SpeedKartEngineSound4;
	
	FKDescription = "";
	
	maxDamage = 180.00;
	destroyedLevel = 180.00;
};

datablock WheeledVehicleData(SpeedKartHotrodVehicle : SpeedKartVehicle)
{
   displayName = "SpeedKart Hotrod";
   shapeFile = "./speedkarthotrod.dts";
   massCenter = "0 -0.16 0.27";
   density = 3.2;
   drag = 2.7;
   engineTorque = 14000;
   brakeTorque = 14000;
   uiName = "SpeedKart Hotrod";
   SpeedKartHornSound = SpeedKartHornSound3;
	SpeedKartIdleSound = SpeedKartIdleSound4;
	SpeedKartEngineSound = SpeedKartEngineSound5;
	
	FKDescription = "";
	
	maxDamage = 180.00;
	destroyedLevel = 180.00;
};

datablock WheeledVehicleData(SpeedKartHoverVehicle : SpeedKartVehicle)
{
   displayName = "SpeedKart Hover";
   shapeFile = "./speedkarthover.dts";
   massCenter = "0 -0.225 0.27";
   tireEmitter = playerJetGroundEmitter;
   defaultTire = SpeedKartHoverTire;
   density = 4.8;
   drag = 1.9;
   engineTorque = 8000;
   brakeTorque = 24000;
   uiName = "SpeedKart Hover";
   initialExplosionProjectile = vehicleExplosionProjectile;
   SpeedKartHornSound = SpeedKartHornSound3;
	SpeedKartIdleSound = SpeedKartIdleSound5;
	SpeedKartEngineSound = SpeedKartEngineSound6;
	
	FKDescription = "";
	
	maxDamage = 360.00;
	destroyedLevel = 360.00;
};

datablock WheeledVehicleData(SpeedKartHoverIIVehicle : SpeedKartVehicle)
{
   displayName = "SuperKart Hover";
   shapeFile = "./speedkarthoverii.dts";
   massCenter = "0 -0.211 0.405";
   tireEmitter = playerJetGroundEmitter;
   cameraMaxDist = 10;
   defaultTire = SpeedKartHoverIITire;
   density = 9.8;
   drag = 1.5;
   engineTorque = 16000;
   brakeTorque = 24000;
   maxWheelSpeed = 47.5;
   uiName = "SuperKart Hover";
   initialExplosionProjectile = vehicleExplosionProjectile;
   SpeedKartHornSound = SpeedKartHornSound3;
	SpeedKartIdleSound = SpeedKartIdleSound5;
	SpeedKartEngineSound = SpeedKartEngineSound6;
	
	FKDescription = "One of the fastest karts, but also the most slippery.";
	
	maxDamage = 400.00;
	destroyedLevel = 400.00;
};

datablock WheeledVehicleData(SpeedKartHyperionVehicle : SpeedKartVehicle)
{
   displayName = "SpeedKart Hyperion";
   shapeFile = "./speedkarthyperion.dts";
   massCenter = "0 -0.179 0.27";
   density = 3.2;
   drag = 2.3;
   engineTorque = 9000;
   brakeTorque = 20000;
   uiName = "SpeedKart Hyperion";
   SpeedKartHornSound = SpeedKartHornSound3;
	SpeedKartIdleSound = SpeedKartIdleSound3;
	SpeedKartEngineSound = SpeedKartEngineSound3;
	
	FKDescription = "";
	
	maxDamage = 180.00;
	destroyedLevel = 180.00;
};

datablock WheeledVehicleData(SpeedKartJeepVehicle : SpeedKartVehicle)
{
   displayName = "SpeedKart Jeep";
   shapeFile = "./speedkartjeep.dts";
   massCenter = "0 -0.13 0.24";
   density = 3.8;
   drag = 3.4;
   engineTorque = 9500;
   brakeTorque = 20000;
   maxWheelSpeed = 45.8;
   uiName = "SpeedKart Jeep";
   SpeedKartHornSound = SpeedKartHornSound2;
	SpeedKartIdleSound = SpeedKartIdleSound6;
	SpeedKartEngineSound = SpeedKartEngineSound2;
	
	FKDescription = "";
	
	maxDamage = 280.00;
	destroyedLevel = 280.00;
};

datablock WheeledVehicleData(SpeedKartJetskiVehicle : SpeedKartVehicle)
{
   displayName = "SuperKart Jetski";
   shapeFile = "./speedkartjetski.dts";
   mountThread[0] = "root";
   massCenter = "0 -0.132 0.9";
   tireEmitter = vehicleFoamEmitter;
   defaultTire = SpeedKartHoverIITire;
   defaultSpring = SpeedKartJetskiSpring;
   density = 0.9;
   drag = 1.8;
   engineTorque = 6000;
   brakeTorque = 12000;
   uiName = "SuperKart Jetski";
   initialExplosionProjectile = vehicleExplosionProjectile;
   
   FKDescription = "";
   
	maxDamage = 160.00;
	destroyedLevel = 160.00;
};

datablock WheeledVehicleData(SpeedKartLeMansVehicle : SpeedKartVehicle)
{
   displayName = "SpeedKart LeMans";
   shapeFile = "./speedkartlemans.dts";
   massCenter = "0 -0.231 0.27";
   density = 7.0;
   drag = 3.5;
   engineTorque = 8000;
   brakeTorque = 14000;
   maxWheelSpeed = 46.9;
   uiName = "SpeedKart LeMans";
   SpeedKartHornSound = SpeedKartHornSound3;
	SpeedKartIdleSound = SpeedKartIdleSound3;
	SpeedKartEngineSound = SpeedKartEngineSound3;
	
	FKDescription = "";
	
	maxDamage = 180.00;
	destroyedLevel = 180.00;
};

datablock WheeledVehicleData(SpeedKartMuscleVehicle : SpeedKartVehicle)
{
   displayName = "SpeedKart Muscle";
   shapeFile = "./speedkartmuscle.dts";
   massCenter = "0 -0.27 0.31";
   density = 4.9;
   drag = 2.4;
   engineTorque = 12000;
   brakeTorque = 14000;
   uiName = "SpeedKart Muscle";
   SpeedKartHornSound = SpeedKartHornSound5;
	SpeedKartIdleSound = SpeedKartIdleSound4;
	SpeedKartEngineSound = SpeedKartEngineSound5;
	
	FKDescription = "";
	
	maxDamage = 280.00;
	destroyedLevel = 280.00;
};

datablock WheeledVehicleData(SpeedKartOriginalVehicle : SpeedKartVehicle)
{
   displayName = "SpeedKart Original";
   shapeFile = "./speedkartoriginal.dts";
   massCenter = "0 -0.28 0.2825";
   maxSteeringAngle = 1.1;
   defaultTire = SpeedKartOriginalTire;
   defaultSpring = SpeedKartOriginalSpring;
   density = 2.1;
   drag = 2.3;
   engineTorque = 12000;
   engineBrake = 2000;
   brakeTorque = 100000;
   maxWheelSpeed = 44.1;
   UsedToBeShit = false;
   uiName = "SpeedKart Original";
   initialExplosionProjectile = SpeedKartOriginalExplosionProjectile;
	SpeedKartIdleSound = SpeedKartIdleSound1;
	SpeedKartEngineSound = SpeedKartEngineSound1;
	
	FKDescription = "One of the fastest karts, but the extreme grip takes some getting used to.";
	
	maxDamage = 360.00;
	destroyedLevel = 360.00;
};

datablock WheeledVehicleData(SpeedKartPlaneVehicle : SpeedKartVehicle)
{
   displayName = "SuperKart Plane";
   shapeFile = "./speedkartplane.dts";
   massCenter = "0 0.011 0.22";
   tireEmitter = "";
   defaultTire = SpeedKartHoverIITire;
   density = 7.8;
   drag = 1.9;
   engineTorque = 7500;
   brakeTorque = 12000;
   // forwardThrust		= 500;
   // reverseThrust		= 500;
   // lift			= 0;
   // maxForwardVel		= 45;
   // maxReverseVel		= 45;
   // horizontalSurfaceForce	= 100;
   // verticalSurfaceForce	= 100;
   // stallSpeed		= 0;
   uiName = "SuperKart Plane";
   initialExplosionProjectile = vehicleExplosionProjectile;
   
   FKDescription = "";
   
	maxDamage = 420.00;
	destroyedLevel = 420.00;
};

datablock WheeledVehicleData(SpeedKartVintageVehicle : SpeedKartVehicle)
{
   displayName = "SpeedKart Vintage";
   shapeFile = "./speedkartvintage.dts";
   massCenter = "0 -0.16 0.27";
   density = 2;
   drag = 2.4;
   brakeTorque = 26000;
   uiName = "SpeedKart Vintage";
   SpeedKartHornSound = SpeedKartHornSound5;
	SpeedKartIdleSound = SpeedKartIdleSound7;
	SpeedKartEngineSound = SpeedKartEngineSound7;
	
	FKDescription = "";
	
	maxDamage = 220.00;
	destroyedLevel = 220.00;
};

datablock WheeledVehicleData(SpeedKartDefaultVehicle : SpeedKartVehicle)
{
	displayName = "SpeedKart Default";
	shapeFile = "./SpeedKart.dts";
	massCenter = "0 -0.3 0.3";
	maxSteeringAngle = 0.6;
	//defaultTire = SpeedKartDefaultTire;
	defaultSpring = SpeedKartDefaultSpring;

	// Rigid Body
	mass = 200;
	density = 5.0;
	drag = 4.5;
	bodyFriction = 0.6;
	bodyRestitution = 0.6;

	// Engine
	engineTorque = 3200;
	engineBrake = 700;
	brakeTorque = 3200;
	maxWheelSpeed = 45;

   // Advanced Steering
   steeringAutoReturnRate = 1;
   steeringStrafeSteeringRate = 0.08;

   uiName = "SpeedKart Default";
   
   SpeedKartHornSound = SpeedKartHornSound1;
   SpeedKartIdleSound = SpeedKartIdleSound1;
   SpeedKartEngineSound = SpeedKartEngineSound1;
   
   FKDescription = "Closest to the default speedkarts.";
};

function SpeedKartVehicle::onAdd(%this,%obj)
{
   Parent::onAdd(%this,%obj);
   %obj.hideNode(lhand);
   %obj.hideNode(rhand);
   %obj.hideNode(lhook);
   %obj.hideNode(rhook);
   speedCheck(%this, %obj);
}

function SpeedKartIIVehicle::onAdd(%this,%obj)
{
   Parent::onAdd(%this,%obj);
   %obj.hideNode(lhand);
   %obj.hideNode(rhand);
   %obj.hideNode(lhook);
   %obj.hideNode(rhook);
   speedCheck(%this, %obj);
}

function SpeedKart64Vehicle::onAdd(%this,%obj)
{
   Parent::onAdd(%this,%obj);
   %obj.hideNode(lhand);
   %obj.hideNode(rhand);
   %obj.hideNode(lhook);
   %obj.hideNode(rhook);
   speedCheck(%this, %obj);
}

function SpeedKart7Vehicle::onAdd(%this,%obj)
{
   Parent::onAdd(%this,%obj);
   %obj.hideNode(lhand);
   %obj.hideNode(rhand);
   %obj.hideNode(lhook);
   %obj.hideNode(rhook);
   speedCheck(%this, %obj);
}

function SpeedKartATVVehicle::onAdd(%this,%obj)
{
   Parent::onAdd(%this,%obj);
   %obj.hideNode(lhand);
   %obj.hideNode(rhand);
   %obj.hideNode(lhook);
   %obj.hideNode(rhook);
   speedCheck(%this, %obj);
}

function SpeedKartBlockoVehicle::onAdd(%this,%obj)
{
   Parent::onAdd(%this,%obj);
   %obj.hideNode(lhand);
   %obj.hideNode(rhand);
   %obj.hideNode(lhook);
   %obj.hideNode(rhook);
   speedCheck(%this, %obj);
}

function SpeedKartBuggyVehicle::onAdd(%this,%obj)
{
   Parent::onAdd(%this,%obj);
   %obj.hideNode(lhand);
   %obj.hideNode(rhand);
   %obj.hideNode(lhook);
   %obj.hideNode(rhook);
   speedCheck(%this, %obj);
}

function SpeedKartClassicVehicle::onAdd(%this,%obj)
{
   Parent::onAdd(%this,%obj);
   %obj.hideNode(lhand);
   %obj.hideNode(rhand);
   %obj.hideNode(lhook);
   %obj.hideNode(rhook);
   speedCheck(%this, %obj);
}

function SpeedKartClassicGTVehicle::onAdd(%this,%obj)
{
   Parent::onAdd(%this,%obj);
   %obj.hideNode(lhand);
   %obj.hideNode(rhand);
   %obj.hideNode(lhook);
   %obj.hideNode(rhook);
   speedCheck(%this, %obj);
}

function SpeedKartFormulaVehicle::onAdd(%this,%obj)
{
   Parent::onAdd(%this,%obj);
   %obj.hideNode(lhand);
   %obj.hideNode(rhand);
   %obj.hideNode(lhook);
   %obj.hideNode(rhook);
   speedCheck(%this, %obj);
}

function SpeedKartHotrodVehicle::onAdd(%this,%obj)
{
   Parent::onAdd(%this,%obj);
   %obj.hideNode(lhand);
   %obj.hideNode(rhand);
   %obj.hideNode(lhook);
   %obj.hideNode(rhook);
   speedCheck(%this, %obj);
}

function SpeedKartHoverVehicle::onAdd(%this,%obj)
{
   Parent::onAdd(%this,%obj);
   %obj.hideNode(lhand);
   %obj.hideNode(rhand);
   %obj.hideNode(lhook);
   %obj.hideNode(rhook);
   speedCheck(%this, %obj);
}

function SpeedKartHoverIIVehicle::onAdd(%this,%obj)
{
   Parent::onAdd(%this,%obj);
   %obj.hideNode(lhand);
   %obj.hideNode(rhand);
   %obj.hideNode(lhook);
   %obj.hideNode(rhook);
   speedCheck(%this, %obj);
}

function SpeedKartHyperionVehicle::onAdd(%this,%obj)
{
   Parent::onAdd(%this,%obj);
   %obj.hideNode(lhand);
   %obj.hideNode(rhand);
   %obj.hideNode(lhook);
   %obj.hideNode(rhook);
   speedCheck(%this, %obj);
}

function SpeedKartJeepVehicle::onAdd(%this,%obj)
{
   Parent::onAdd(%this,%obj);
   %obj.hideNode(lhand);
   %obj.hideNode(rhand);
   %obj.hideNode(lhook);
   %obj.hideNode(rhook);
   speedCheck(%this, %obj);
}

function SpeedKartJetskiVehicle::onAdd(%this,%obj)
{
   Parent::onAdd(%this,%obj);
   %obj.hideNode(lhand);
   %obj.hideNode(rhand);
   %obj.hideNode(lhook);
   %obj.hideNode(rhook);
   speedCheck(%this, %obj);
}

function SpeedKartLeMansVehicle::onAdd(%this,%obj)
{
   Parent::onAdd(%this,%obj);
   %obj.hideNode(lhand);
   %obj.hideNode(rhand);
   %obj.hideNode(lhook);
   %obj.hideNode(rhook);
   speedCheck(%this, %obj);
}

function SpeedKartMuscleVehicle::onAdd(%this,%obj)
{
   Parent::onAdd(%this,%obj);
   %obj.hideNode(lhand);
   %obj.hideNode(rhand);
   %obj.hideNode(lhook);
   %obj.hideNode(rhook);
   speedCheck(%this, %obj);
}

function SpeedKartPlaneVehicle::onAdd(%this,%obj)
{
   Parent::onAdd(%this,%obj);
   %obj.hideNode(lhand);
   %obj.hideNode(rhand);
   %obj.hideNode(lhook);
   %obj.hideNode(rhook);
   speedCheck(%this, %obj);
}

function SpeedKartVintageVehicle::onAdd(%this,%obj)
{
   Parent::onAdd(%this,%obj);
   %obj.hideNode(lhand);
   %obj.hideNode(rhand);
   %obj.hideNode(lhook);
   %obj.hideNode(rhook);
   speedCheck(%this, %obj);
}

function SpeedKartDefaultVehicle::onAdd(%this,%obj)
{
   Parent::onAdd(%this,%obj);
   %obj.hideNode(lhand);
   %obj.hideNode(rhand);
   %obj.hideNode(lhook);
   %obj.hideNode(rhook);
   speedCheck(%this, %obj);
}

function speedCheck(%this, %obj)
{
	if(!isObject(%obj))
		return;

	if(!$FK::Pref::Karts::EngineSounds)
	{
		%obj.stopAudio(3);
		return;
	}
	
	%speed = vectorLen(%obj.getVelocity());
	if(%speed < 0)
		%speed = 0;

	if(%speed < 10)
	{
	    if(%this.SpeedKartIdleSound !$= "")
			%obj.playAudio(3, %this.SpeedKartIdleSound);
		else if(isObject(SpeedKartIdleSound1))
			%obj.playAudio(3, SpeedKartIdleSound1);
	}
	else
	{
		if(%this.SpeedKartEngineSound !$= "")
			%obj.playAudio(3, %this.SpeedKartEngineSound);
		else if(isObject(SpeedKartEngineSound1))
			%obj.playAudio(3, SpeedKartEngineSound1);
	}

	schedule(100,0,"speedCheck",%this,%obj);
}

datablock DebrisData(SpeedKartTireDebris)
{
   emitters = "JeepTireDebrisTrailEmitter";

	shapeFile = "./speedkarttire.dts";
	lifetime = 2.0;
	minSpinSpeed = -400.0;
	maxSpinSpeed = 200.0;
	elasticity = 0.3;
	friction = 0.1;
	numBounces = 4;
	staticOnMaxBounce = true;
	snapOnMaxBounce = false;
	fade = true;

	gravModifier = 1;
};

datablock ExplosionData(SpeedKartExplosion : vehicleExplosion)
{
   debris = SpeedKartTireDebris;
   debrisNum = 4;
   debrisNumVariance = 0;
   debrisPhiMin = 0;
   debrisPhiMax = 360;
   debrisThetaMin = 40;
   debrisThetaMax = 85;
   debrisVelocity = 14;
   debrisVelocityVariance = 3;
};

datablock ProjectileData(SpeedKartExplosionProjectile : vehicleExplosionProjectile)
{
   directDamage        = 0;
   radiusDamage        = 0;
   damageRadius        = 0;
   explosion           = SpeedKartExplosion;

   directDamageType  = $DamageType::VehicleExplosion;
   radiusDamageType  = $DamageType::VehicleExplosion;

   explodeOnDeath		= 1;

   armingDelay         = 0;
   lifetime            = 0;

   uiName = "SpeedKart Explosion";
};

datablock DebrisData(SpeedKartIITireDebris)
{
   shapeFile = "./speedkartIItire.dts";
};

datablock ExplosionData(SpeedKartIIExplosion : SpeedKartExplosion)
{
   debris = SpeedKartIITireDebris;
};

datablock ProjectileData(SpeedKartIIExplosionProjectile : SpeedKartExplosionProjectile)
{
   explosion = SpeedKartIIExplosion;
   uiName = "SpeedKart II Explosion";
};

datablock DebrisData(SpeedKart64TireDebris)
{
   shapeFile = "./speedkart64tire.dts";
};

datablock ExplosionData(SpeedKart64Explosion : SpeedKartExplosion)
{
   debris = SpeedKart64TireDebris;
};

datablock ProjectileData(SpeedKart64ExplosionProjectile : SpeedKartExplosionProjectile)
{
   explosion = SpeedKart64Explosion;
   uiName = "SpeedKart 64 Explosion";
};

datablock DebrisData(SpeedKartATVTireDebris)
{
   shapeFile = "./speedkartatvtire.dts";
};

datablock ExplosionData(SpeedKartATVExplosion : SpeedKartExplosion)
{
   debris = SpeedKartATVTireDebris;
};

datablock ProjectileData(SpeedKartATVExplosionProjectile : SpeedKartExplosionProjectile)
{
   explosion = SpeedKartATVExplosion;
   uiName = "SpeedKart ATV Explosion";
};

datablock DebrisData(SpeedKartBlockoTireDebris)
{
   shapeFile = "./speedkartblockotire.dts";
};

datablock ExplosionData(SpeedKartBlockoExplosion : SpeedKartExplosion)
{
   debris = SpeedKartBlockoTireDebris;
};

datablock ProjectileData(SpeedKartBlockoExplosionProjectile : SpeedKartExplosionProjectile)
{
   explosion = SpeedKartBlockoExplosion;
   uiName = "SpeedKart Blocko Explosion";
};

datablock DebrisData(SpeedKartOriginalTireDebris : jeepTireDebris)
{
   shapeFile = "./speedkartoriginaltire.dts";
};

datablock ExplosionData(SpeedKartOriginalExplosion : jeepExplosion)
{
   debris = SpeedKartOriginalTireDebris;
};

datablock ProjectileData(SpeedKartOriginalExplosionProjectile : jeepExplosionProjectile)
{
   explosion = SpeedKartOriginalExplosion;
   uiName = "SpeedKart Original Explosion";
};

package HandsBlockagePackage
{
   function Armor::onMount(%this,%obj,%col,%slot)
   {
      Parent::onMount(%this,%obj,%col,%slot);
      if(%obj.getMountNode() != 0)
         return;
		
		%client = %obj.client;
		if(!isObject(%client))
			return;
		
		%vehicle = %obj.getObjectMount();
		if(!isObject(%vehicle))
			return;
		
      if(%vehicle.getDatablock().UsedToBeShit == 1)
      {
         %t = %obj.activeThread[1];
         if(!(%t $= "armReadyRight" || %t $= "armReadyBoth"))
         {
            %col.unhideNode($rhand[%client.rhand]);
            %col.setNodeColor($rhand[%client.rhand],%client.rhandcolor);
            %obj.hideNode("rhand");
            %obj.hideNode("rhook");
         }

         if(!(%t $= "armReadyLeft" || %t $= "armReadyBoth"))
         {
            %col.unhideNode($lhand[%client.lhand]);
            %col.setNodeColor($lhand[%client.lhand],%client.lhandcolor);
            %obj.hideNode("lhand");
            %obj.hideNode("lhook");
         }
      }
   }

   function Armor::onUnMount(%this,%obj,%col,%slot)
   {	
      Parent::onUnMount(%this,%obj,%col,%slot);
      if(!isObject(%col))
         return;

      if(%obj.getMountNode() != 0)
         return;

      if(%col.getDatablock().UsedToBeShit == 1)
      {
         %t = %obj.activeThread[1];

         if(!(%t $= "armReadyRight" || %t $= "armReadyBoth"))
         {
            %obj.unhideNode($rhand[%obj.client.rhand]);
            %col.hideNode("rhand");
            %col.hideNode("rhook");
         }

         if(!(%t $= "armReadyLeft" || %t $= "armReadyBoth"))
         {
            %obj.unhideNode($lhand[%obj.client.lhand]);
            %col.hideNode("lhand");
            %col.hideNode("lhook");
         }
      }
   }

   function GameConnection::applyBodyParts(%this)
   {
      Parent::applyBodyParts(%this);
      %player = %this.player;

      if(isObject(%player) == 0 || isObject(%veh = %player.getObjectMount()) == 0)
         return;

      if(%player.getMountNode() != 0)
         return;

      if(%veh.getDatablock().UsedToBeShit == 1)
      {
         %player.hideNode("lhand");
         %player.hideNode("rhand");
         %player.hideNode("lhook");
         %player.hideNode("rhook");
         %veh.hideNode("lhand");
         %veh.hideNode("rhand");
         %veh.hideNode("lhook");
         %veh.hideNode("rhook");
         %veh.unhideNode($lhand[%this.lhand]);
         %veh.unhideNode($rhand[%this.rhand]);
         %veh.setNodeColor($lhand[%this.lhand],%this.lhandColor);
         %veh.setNodeColor($rhand[%this.rhand],%this.rhandColor);
      }
   }

   function Player::playThread(%player,%slot,%thread)
   {
      Parent::playThread(%player,%slot,%thread);
      if(%player.getMountNode() != 0)
         return;

      %veh = %player.getObjectMount();
      %player.activeThread[%slot] = %thread;

      if(isObject(%veh) && %veh.getDatablock().UsedToBeShit == 1)
      {
         if(%slot == 1)
         {
            if(%thread $= "armReadyRight")
            {
               %veh.hideNode("rhand");
               %veh.hideNode("rhook");
               %player.unHideNode($rhand[%player.client.rhand]);
               %veh.unhideNode($lhand[%player.client.lhand]);
               %veh.setNodeColor($lhand[%player.client.lhand],%player.client.lhandColor);
               %player.hideNode("lhand");
               %player.hideNode("lhook");
            }
            else if(%thread $= "armReadyLeft")
            {
               %veh.hideNode("lhand");
               %veh.hideNode("lhook");
               %player.unHideNode($lhand[%player.client.lhand]);
               %veh.unhideNode($rhand[%player.client.rhand]);
               %veh.setNodeColor($rhand[%player.client.rhand],%player.client.rhandColor);
               %player.hideNode("rhand");
               %player.hideNode("rhook");
            }
            else if(%thread $= "armReadyBoth")
            {
               %veh.hideNode("lhand");
               %veh.hideNode("lhook");
               %player.unHideNode($lhand[%player.client.lhand]);
               %veh.hideNode("rhand");
               %veh.hideNode("rhook");
               %player.unHideNode($rhand[%player.client.rhand]);
            }
            else if(%thread $= "root")
            {
               %player.hideNode("lhand");
               %player.hideNode("rhand");
               %player.hideNode("lhook");
               %player.hideNode("rhook");
               %veh.unhideNode($lhand[%player.client.lhand]);
               %veh.unhideNode($rhand[%player.client.rhand]);
               %veh.setNodeColor($lhand[%player.client.lhand],%player.client.lhandColor);
               %veh.setNodeColor($rhand[%player.client.rhand],%player.client.rhandColor);
            }
         }
      }
   }
};
activatePackage(HandsBlockagePackage);

package hugPackage
{
   function Armor::onMount(%this,%obj,%col,%slot)
   {
      Parent::onMount(%this,%obj,%col,%slot);
      if(%col.dataBlock $= SpeedKartOriginalVehicle)
         %obj.playThread(2, armReadyBoth);
   }

   function Armor::onUnMount(%this,%obj,%col,%slot)
   {
      Parent::onUnMount(%this,%obj,%col,%slot);
      if(%col.dataBlock $= SpeedKartOriginalVehicle)
         %obj.playThread(2, root);
   }
};
activatepackage(hugPackage);

package hornPackage
{
	function armor::onTrigger(%db,%obj,%slot,%val)
	{
		if(!$FK::Pref::Karts::HornSounds)
			return Parent::onTrigger(%db,%obj,%slot,%val);
		
		if(%obj.getClassName()$="Player")
		{
			if(%slot == 0 && %obj.isMounted())
			{
				if(strStr(strLwr(%obj.getObjectMount().getDatablock().getName()), "SpeedKart") != 1)
				{ 
					if(%val)
					{
						if(getSimTime() < (%obj.lastHornClick + $FK::Pref::Karts::HornMS))
							return;
						
						%obj.playHornNoise();
						return;
					}
				}
			}
		}
		return Parent::onTrigger(%db,%obj,%slot,%val);
	}
};
ActivatePackage(hornPackage);

function Player::playHornNoise(%player)
{
	%vehicle = %player.getObjectMount();
	if(!isObject(%vehicle))
		return;
	
	%vehicleDatablock = %vehicle.getDatablock();
	%mountedObj = %vehicle.getMountNodeObject(0);
	
	if(%mountedObj != %player)
		return;
	
	if(isObject(%vehicleDatablock.SpeedKartHornSound))
		%player.playAudio(1, %vehicleDatablock.SpeedKartHornSound);
	else
	{
		if(isObject(SpeedKartHornSound1))
			%player.playAudio(1, SpeedKartHornSound1);
	}
	%player.lastHornClick = getSimTime();
}