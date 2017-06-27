//Title: PGDie
//Author: Bushido and Aloshi
//unintrusive and ridiculous
//edited to integrate 007 death yells and to remove explosion sound from deaths

exec("./007deathyells/sounds.cs");

datablock AudioProfile(injureHit1Sound)
{
	filename    = "./takehit2.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(injureHit2Sound)
{
	filename    = "./die1.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(injureGag0Sound)
{
	filename    = "./gag01.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock AudioProfile(injureGag1Sound)
{
	filename    = "./gag12.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock AudioProfile(injureGag2Sound)
{
	filename    = "./gag22.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock AudioProfile(injureGag3Sound)
{
	filename    = "./gag32.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock AudioProfile(injureGag4Sound)
{
	filename    = "./gag42.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock AudioProfile(injureGag5Sound)
{
	filename    = "./gag52.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock AudioProfile(injureGag6Sound)
{
	filename    = "./gag62.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock AudioProfile(injureGag7Sound)
{
	filename    = "./gag72.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock AudioProfile(injureNullSound)
{
	filename    = "./null.wav";
	description = AudioClose3d;
	preload = true;
};

//particle fx
//////////////////////////////////////////

//muzzle flash effects
datablock ParticleData(injureFlashParticle)
{
	dragCoefficient      = 3;
	gravityCoefficient   = -0.5;
	inheritedVelFactor   = 0.2;
	constantAcceleration = 0.0;
	lifetimeMS           = 100;
	lifetimeVarianceMS   = 15;
	textureName          = "base/data/particles/star1";
	spinSpeed		= 10.0;
	spinRandomMin		= -500.0;
	spinRandomMax		= 500.0;
	colors[0]     = "0.6 0.6 0.3 0.9";
	colors[1]     = "0.9 0.5 0.0 0.0";
	sizes[0]      = 7.5;
	sizes[1]      = 7.0;

	useInvAlpha = false;
};
datablock ParticleEmitterData(injureFlashEmitter)
{
   ejectionPeriodMS = 70;
   periodVarianceMS = 0;
   ejectionVelocity = 1.0;
   velocityVariance = 1.0;
   ejectionOffset   = 0.0;
   thetaMin         = 0;
   thetaMax         = 90;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = "injureFlashParticle";
};

datablock ParticleData(injureExplosionParticle)
{
	dragCoefficient      = 2;
	gravityCoefficient   = 1;
	inheritedVelFactor   = 0.2;
	constantAcceleration = 0.0;
	lifetimeMS           = 700;
	lifetimeVarianceMS   = 100;
	textureName          = "base/data/particles/cloud";
	spinSpeed		= 1000.0;
	spinRandomMin		= -5000.0;
	spinRandomMax		= 5000.0;
	colors[0]     = "0.9 0.9 0.9 0.1";
	colors[1]     = "0.9 0.5 0.6 0.0";
	sizes[0]      = 2.25;
	sizes[1]      = 0;

	useInvAlpha = true;
};
datablock ParticleEmitterData(injureExplosionEmitter)
{
   ejectionPeriodMS = 4;
   periodVarianceMS = 0;
   ejectionVelocity = 10;
   velocityVariance = 10.0;
   ejectionOffset   = 1.0;
   thetaMin         = 0;
   thetaMax         = 180;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = "injureExplosionParticle";

   useEmitterColors = true;
};

datablock ParticleData(injureExplosionParticle2)
{
	dragCoefficient		= 0.1;
	windCoefficient		= 0.0;
	gravityCoefficient	= 1.0;
	inheritedVelFactor	= 0.0;
	constantAcceleration	= 0.0;
	lifetimeMS		= 1000;
	lifetimeVarianceMS	= 500;
	spinSpeed		= 10.0;
	spinRandomMin		= -50.0;
	spinRandomMax		= 50.0;
	useInvAlpha		= true;
	animateTexture		= false;
	//framesPerSec		= 1;

	textureName		= "base/data/particles/chunk";
	//animTexName		= "~/data/particles/cloud";

	// Interpolation variables
	colors[0]	= "0.0 0.0 0.0 0.6";
	colors[1]	= "0.0 0.0 0.0 0.0";
	sizes[0]	= 0.1;
	sizes[1]	= 0.1;
	times[0]	= 0.0;
	times[1]	= 1.0;
};

datablock ParticleEmitterData(injureExplosionEmitter2)
{
   ejectionPeriodMS = 1;
   periodVarianceMS = 0;
   lifetimeMS       = 7;
   ejectionVelocity = 9;
   velocityVariance = 6.0;
   ejectionOffset   = 0.0;
   thetaMin         = 0;
   thetaMax         = 90;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = "injureExplosionParticle2";

   useEmitterColors = true;
   emitterNode = HalfEmitterNode;
};

datablock ExplosionData(injureExplosion)
{
   //explosionShape = "";
   lifeTimeMS = 150;

   emitter[0] = injureExplosionEmitter;
   emitter[1] = injureExplosionEmitter2;
   emitter[2] = injureFlashEmitter;
   //particleDensity = 30;
   //particleRadius = 1.0;

   faceViewer     = true;
   explosionScale = "1 1 1";

   shakeCamera = true;
   camShakeFreq = "7.0 8.0 7.0";
   camShakeAmp = "1.0 1.0 1.0";
   camShakeDuration = 0.5;
   camShakeRadius = 15.0;

   // Dynamic light
   lightStartRadius = 4;
   lightEndRadius = 3;
   lightStartColor = "0.45 0.3 0.1";
   lightEndColor = "0 0 0";
};

datablock ShapeBaseImageData(painHighImage)
{
   // Basic Item properties
   shapeFile = "base/data/shapes/empty.dts";
   emap = true;

   // Specify mount point & offset for 3rd person, and eye offset
   // for first person rendering.
   mountPoint = 6;
   offset = "0 0 -0.8";
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
//item = flamethrowerItem;
//ammo = " ";
//projectile = flamethrowerProjectile;
//projectileType = Projectile;

   //melee particles shoot from eye node for consistancy
   melee = true;
   //raise your arm up or not
   armReady = true;

   doColorShift = false;
   colorShiftColor = flamethrowerItem.colorShiftColor;

   // Images have a state system which controls how the animations
   // are run, which sounds are played, script callbacks, etc. This
   // state system is downloaded to the client so that clients can
   // predict state changes and animate accordingly.  The following
   // system supports basic ready->fire->reload transitions as
   // well as a no-ammo->dryfire idle state.

   // Initial start up state
	stateName[0]					= "Ready";
	stateTransitionOnTimeout[0]		= "FireB";
	stateTimeoutValue[0]			= 0.001;
	stateEmitter[0]					= "";
	stateEmitterTime[0]				= 0.015;

	stateName[1]					= "FireA";
	stateTransitionOnTimeout[1]		= "FireB";
	stateWaitForTimeout[1]			= True;
	stateTimeoutValue[1]			= 0.001;
	stateScript[1]					= "onAGH";
	stateEmitter[1]					= "";
	stateEmitterTime[1]				= 0.075;

	stateName[2]					= "Done";
	stateScript[2]					= "onDone";

	stateName[3]					= "FireB";
	stateTransitionOnTimeout[3]		= "Done";
	stateWaitForTimeout[3]			= True;
	stateTimeoutValue[3]			= 0.001;
	stateEmitter[3]					= "";
	stateEmitterTime[3]				= 0.850;
};

//function playpain()
//{
//}

//function PainLowImage::onMount(%this,%obj,%slot)
//{
//	%obj.unMountImage(%slot);
//}
//function PainMidImage::onMount(%this,%obj,%slot)
//{
//	%obj.unMountImage(%slot);
//}
//function PainHighImage::onMount(%this,%obj,%slot)
//{
//	%obj.unMountImage(%slot);
//}

datablock ProjectileData(injureProjectile)
{
	uiname							= injureProjectile;

	lifetime						= 10;
	fadeDelay						= 10;
	explodeondeath						= true;
	explosion						= injureExplosion;

};
datablock PlayerData(PlayerCrumbleDeath : PlayerStandardArmor) //edited to bounce
{
	uiName = "";
	minImpactSpeed = 1;
	groundImpactMinSpeed    = 0;
	groundImpactShakeFreq   = "0 0 0";
	groundImpactShakeAmp    = "0 0 0";
	groundImpactShakeDuration = 0;
	groundImpactShakeFalloff = 0;
};

package goreDie
{
	function Armor::damage(%this, %obj, %sourceObject, %position, %damage, %damageType)
	{
		if(%this.uiName $= "Rowboat" || %this.uiName $= "Tank Turret" || %this.uiName $= "Pirate Cannon")
		{
			Parent::damage(%this, %obj, %sourceObject, %position, %damage, %damageType);
			return;
		}
		
		if($Pref::Server::FASTKarts::PGDieEffects)
			%obj.spawnExplosion(injureProjectile,"0.7 0.7 0.7"); //not sure of arguments :V
		
		Parent::damage(%this, %obj, %sourceObject, %position, %damage, %damageType);
	
		if($Pref::Server::FASTKarts::PGDieEffects)
			serverPlay3D(injureHit1Sound,%obj.getPosition());
		
		if($Pref::Server::FASTKarts::PGDieSounds)
		{
			%sound = getRandom(1, 50);
			switch(%sound)
			{
				case 1:
					serverPlay3D(injureGag0Sound,%obj.getPosition());
				case 2:
					serverPlay3D(injureGag1Sound,%obj.getPosition());
				case 3:
					serverPlay3D(injureGag2Sound,%obj.getPosition());
				case 4:
					serverPlay3D(injureGag3Sound,%obj.getPosition());
				case 5:
					serverPlay3D(injureGag4Sound,%obj.getPosition());
				case 6:
					serverPlay3D(injureGag5Sound,%obj.getPosition());
				case 7:
					serverPlay3D(injureGag6Sound,%obj.getPosition());
				case 8:
					serverPlay3D(injureGag7Sound,%obj.getPosition());
				case 9:
					serverPlay3D(brickbreaksound,%obj.getPosition());
				case 10:
					serverPlay3D(injureHit2Sound,%obj.getPosition());
			}
		}
	}
	function Armor::onDisabled(%this, %obj, %enabled)
	{
		if(%this.uiName $= "Rowboat" || %this.uiName $= "Tank Turret" || %this.uiName $= "Pirate Cannon")
		{
			Parent::onDisabled(%this, %obj, %enabled);
			return;
		}
		
		if($Pref::Server::FASTKarts::PGDieEffects)
			%obj.spawnExplosion(injureProjectile,"1 1 1"); //not sure of arguments :V
		
		%model = fileName(%this.shapeFile);
		if($Pref::Server::FASTKarts::PGDieEffects)
		{
			if(%model $= "m.dts")
				%obj.changeDataBlock(PlayerCrumbleDeath);
		}
		
		Parent::onDisabled(%this, %obj, %enabled);
		
		if($Pref::Server::FASTKarts::CrumbleDeath)
		{
			if(%model !$= "m.dts" || %obj.type $= "DeathLimb")
				return;
			
			schedule(5,0,crumbleDeathStart,%obj);
			%obj.setActionThread(root);
		}
	}
	function Player::playDeathCry(%obj)
	{
		%data = %obj.getDatablock();
		if(%data.uiName $= "Rowboat" || %data.uiName $= "Tank Turret" || %data.uiName $= "Pirate Cannon")
		{
			Parent::playDeathCry(%obj);
			return;
		}
		
		if(!$Pref::Server::FASTKarts::RandomDeathYells)
			Parent::playDeathCry(%obj);
		
		if(%obj.type !$= "DeathLimb")
		{
			if($Pref::Server::FASTKarts::RandomDeathYells)
			{
				%r = getRandom(1, 39);
				serverplay3d("scream" @ %r, %obj.getPosition());
			}
			
			if($Pref::Server::FASTKarts::PGDieSounds)
			{
				%sound = getRandom(1, 20);
				switch(%sound)
				{
					case 1:
							serverPlay3D(injureGag0Sound,%obj.getPosition());
					case 2:
							serverPlay3D(injureGag1Sound,%obj.getPosition());
					case 3:
							serverPlay3D(injureGag2Sound,%obj.getPosition());
					case 4:
							serverPlay3D(injureGag3Sound,%obj.getPosition());
					case 5:
							serverPlay3D(injureGag4Sound,%obj.getPosition());
					case 6:
							serverPlay3D(injureGag5Sound,%obj.getPosition());
					case 7:
							serverPlay3D(injureGag6Sound,%obj.getPosition());
					case 8:
							serverPlay3D(injureGag7Sound,%obj.getPosition());
					case 9:
							serverPlay3D(brickbreaksound,%obj.getPosition());
					case 10:
							serverPlay3D(injureHit2Sound,%obj.getPosition());
				}
			}
		}
	}
	function Armor::onRemove(%data,%obj,%enabled)
	{
		Parent::onRemove(%data,%obj,%enabled);
		if(isObject(%obj.botA))
			%obj.botA.delete();
		if(isObject(%obj.botB))
			%obj.botB.delete();
		if(isObject(%obj.botC))
			%obj.botC.delete();
	}
	function PlayerCrumbleDeath::onImpact(%this, %obj, %collidedObject, %vec, %vecLen) //added to make them bounce
	{
		parent::onImpact(%this, %obj, %collidedObject, %vec, %vecLen);
		%veca = getword(%vec, 0) * 0.5;
		%vecb = getword(%vec, 1) * 0.5;
		%vecc = getword(%vec, 2) * 0.5;
		%obj.addvelocity(%veca SPC %vecb SPC %vecc);
	}
	function crumbleDeathStart(%obj)
	{
		if(!isObject(%obj))
			return;
		
		%scale = %obj.getScale();
		%pos = %obj.getPosition();
		//serverPlay3d(injureHit2sound,%pos);
		schedule(5,0,crumbleDeathAnim,%obj);
		spawnDeathLimbs(%obj,%scale);
		spawnDeathLimbs(%obj,%scale,"left");
		spawnDeathLimbs(%obj,%scale,"right");
		%obj.isDeathCrumbled=1;
		%obj.schedule(50,unmountImage,0);
	}
	function crumbleDeathAnim(%obj)
	{
		if(!isObject(%obj))
			return;
		
		//Node Coloring
		%obj.setNodeColor("pants","0.5 0.5 0.5 1");
		%obj.setNodeColor("LShoe","0.5 0.5 0.5 1");
		%obj.setNodeColor("RShoe","1 1 1 1");

		//Node Hiding
		crumbHideHead(%obj);
		crumbHideTorso(%obj);
		crumbHideLeftArm(%obj);
		crumbHideRightArm(%obj);
	}
	function spawnDeathLimbs(%obj,%scale,%side)
	{
		if(!isObject(%obj))
			return;
		
		%tra = %obj.getTransform();
		%vel = %obj.getVelocity();
		%vel = vectorAdd(%vel,"0 0 6");
		%fvec = %obj.getForwardVector();
		%rvec = getRightCrumbleVector(%obj);
		if(%side $= "left" || %side $= "right")
		{
			%pos = vectorAdd(%tra,vectorScale(%fvec,-1));
			%tra = %pos SPC getWords(%tra,3,6);
		}
		if(%side $= "left")
			%vel = vectorAdd(%vel,vectorScale(%rvec,-1));
		if(%side $= "right")
			%vel = vectorAdd(%vel,vectorScale(%rvec,1));
		if(%side $= "")
			%vel = vectorAdd(%vel,vectorScale(%fvec,-1));

		%bot = new AIPlayer()
		{
			dataBlock = PlayerCrumbleDeath;
			type = DeathLimb;
		};MissionCleanup.add(%bot);
		%bot.setTransform(%tra);
		%bot.setVelocity(%vel);
		%bot.setScale(%scale);
		%bot.kill();
		%bot.playthread(3,root);
		if(%side $= "")
		{
			%bot.schedule(50,stopthread,0);
			%bot.playthread(0,look); //crouch
			%bot.playthread(2,sit);
			%obj.botA = %bot;
		}
		else if(%side $= "right")
		{
			%bot.playthread(3,crouch);
			%obj.botB = %bot;
		}
		else
		{
			%bot.playthread(3,sit);
			%bot.playthread(2,look);
			%obj.botC = %bot;
		}

		%client = %obj.client;
		if(%client !$= "")
			applyDefaultCharacterPrefs(%obj);
		%bot.setNodeColor("LArm","0.5 0.5 0.5 1");
		%bot.setNodeColor("RArm","1 1 1 1");
		%bot.setNodeColor("chest","1 1 1 1");
		%bot.setNodeColor("LHand","0.5 0.5 0.5 1"); //Left Hand Color
		%bot.setNodeColor("RHand","1 1 1 1"); //Right Hand Color
		%bot.setNodeColor("headSkin","1 1 1 1"); //Head Color

		//Node Hiding
		crumbHideFeet(%bot);
		if(%side !$= "")
		{
			crumbHideHead(%bot);
			crumbHideTorso(%bot);
		}
		if(%side !$= "left")
			crumbHideLeftArm(%bot);
		if(%side !$= "right")
			crumbHideRightArm(%bot);
	}
	//Node Hiding
	function crumbHideHead(%obj)
	{
		if(!isObject(%obj))
			return;
		
		%obj.hideNode("plume");
		%obj.hideNode("triPlume");
		%obj.hideNode("septPlume");
		%obj.hideNode("visor");
		%obj.hideNode("helmet");
		%obj.hideNode("pointyHelmet");
		%obj.hideNode("flareHelmet");
		%obj.hideNode("scoutHat");
		%obj.hideNode("bicorn");
		%obj.hideNode("copHat");
		%obj.hideNode("knitHat");

		%obj.hideNode("headSkin");
	}
	function crumbHideTorso(%obj)
	{
		if(!isObject(%obj))
			return;
		
		%obj.hideNode("chest");
		%obj.hideNode("femchest");

		%obj.hideNode("armor");
		%obj.hideNode("bucket");
		%obj.hideNode("cape");
		%obj.hideNode("pack");
		%obj.hideNode("quiver");
		%obj.hideNode("tank");

		%obj.hideNode("epaulets");
		%obj.hideNode("epauletsRankA");
		%obj.hideNode("epauletsRankB");
		%obj.hideNode("epauletsRankC");
		%obj.hideNode("epauletsRankD");
		%obj.hideNode("ShoulderPads");
		%obj.hideNode("skirtTrimLeft");
		%obj.hideNode("skirtTrimRight");
	}
	function crumbHideLeftArm(%obj)
	{
		if(!isObject(%obj))
			return;
		
		%obj.hideNode("LArm");
		%obj.hideNode("LArmSlim");
		%obj.hideNode("LHand");
		%obj.hideNode("LHook");
	}
	function crumbHideRightArm(%obj)
	{
		if(!isObject(%obj))
			return;
		
		%obj.hideNode("RArm");
		%obj.hideNode("RArmSlim");
		%obj.hideNode("RHand");
		%obj.hideNode("RHook");
	}
	function crumbHideFeet(%obj)
	{
		if(!isObject(%obj))
			return;
		
		%obj.hideNode("pants");
		%obj.hideNode("skirtHip");

		%obj.hideNode("LShoe");
		%obj.hideNode("LPeg");
		%obj.hideNode("RShoe");
		%obj.hideNode("RPeg");
	}
	//Vector Code
	function getRightCrumbleVector(%obj)
	{
		return vectorCross(%obj.getForwardVector(),%obj.getUpVector());
	}
};
activatepackage(goreDie);