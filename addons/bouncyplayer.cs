//Title: Bouncy Player
//Author: ZSNO
//A bouncy playertype
//single playertype that has less of a bounce than the original and is also faster

datablock PlayerData(BouncyPlayer : PlayerStandardArmor)
{
	runForce = 300 * 270;
	maxForwardSpeed = 50;
	maxBackwardSpeed = 50;
	maxSideSpeed = 50;

	maxForwardCrouchSpeed = 50;
	maxBackwardCrouchSpeed = 50;
	maxSideCrouchSpeed = 50;
	
	minImpactSpeed = 1;
	groundImpactMinSpeed    = 0;
	groundImpactShakeFreq   = "0 0 0";
	groundImpactShakeAmp    = "0 0 0";
	groundImpactShakeDuration = 0;
	groundImpactShakeFalloff = 0;
	
	minJetEnergy = 0;
	jetEnergyDrain = 0;
	canJet = 0;

	uiName = "Bouncy Player";
	showEnergyBar = false;
};

package bounce
{
	function BouncyPlayer::onImpact(%this, %obj, %collidedObject, %vec, %vecLen)
	{
		parent::onImpact(%this, %obj, %collidedObject, %vec, %vecLen);
		%veca = getword(%vec, 0) * 0.9;
		%vecb = getword(%vec, 1) * 0.9;
		%vecc = getword(%vec, 2) * 0.9;
		%obj.addvelocity(%veca SPC %vecb SPC %vecc);
	}
};
activatepackage(bounce);