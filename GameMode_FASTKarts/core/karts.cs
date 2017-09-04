function FK_getKartsAllowed()
{
	if($Pref::Server::FASTKarts::ForceSpeedkarts)
		return 14;
	
	%types = $Pref::Server::FASTKarts::AllowSpeedKart + $Pref::Server::FASTKarts::Allow64 + $Pref::Server::FASTKarts::Allow7 + $Pref::Server::FASTKarts::AllowBlocko + $Pref::Server::FASTKarts::AllowBuggy + $Pref::Server::FASTKarts::AllowClassic + $Pref::Server::FASTKarts::AllowClassicGT + $Pref::Server::FASTKarts::AllowFormula + $Pref::Server::FASTKarts::AllowHotrod + $Pref::Server::FASTKarts::AllowHover + $Pref::Server::FASTKarts::AllowHyperion + $Pref::Server::FASTKarts::AllowJeep + $Pref::Server::FASTKarts::AllowLeMans + $Pref::Server::FASTKarts::AllowMuscle + $Pref::Server::FASTKarts::AllowVintage + $Pref::Server::FASTKarts::AllowOriginal + $Pref::Server::FASTKarts::AllowDefault + $Pref::Server::FASTKarts::AllowSuperATV + $Pref::Server::FASTKarts::AllowSuperHover + $Pref::Server::FASTKarts::AllowSuperKart + $Pref::Server::FASTKarts::AllowSuperJetski + $Pref::Server::FASTKarts::AllowSuperPlane;
	return %types;
}

function FK_getFirstKartAllowed()
{
	if($Pref::Server::FASTKarts::AllowSpeedKart)
		return SpeedKartVehicle.getID();
	if($Pref::Server::FASTKarts::Allow64)
		return SpeedKart64Vehicle.getID();
	if($Pref::Server::FASTKarts::Allow7)
		return SpeedKart7Vehicle.getID();
	if($Pref::Server::FASTKarts::AllowBlocko)
		return SpeedKartBlockoVehicle.getID();
	if($Pref::Server::FASTKarts::AllowBuggy)
		return SpeedKartBuggyVehicle.getID();
	if($Pref::Server::FASTKarts::AllowClassic)
		return SpeedKartClassicVehicle.getID();
	if($Pref::Server::FASTKarts::AllowClassicGT)
		return SpeedKartClassicGTVehicle.getID();
	if($Pref::Server::FASTKarts::AllowFormula)
		return SpeedKartFormulaVehicle.getID();
	if($Pref::Server::FASTKarts::AllowHotrod)
		return SpeedKartHotrodVehicle.getID();
	if($Pref::Server::FASTKarts::AllowHyperion)
		return SpeedKartHyperionVehicle.getID();
	if($Pref::Server::FASTKarts::AllowJeep)
		return SpeedKartJeepVehicle.getID();
	if($Pref::Server::FASTKarts::AllowLeMans)
		return SpeedKartLeMansVehicle.getID();
	if($Pref::Server::FASTKarts::AllowMuscle)
		return SpeedKartMuscleVehicle.getID();
	if($Pref::Server::FASTKarts::AllowVintage)
		return SpeedKartVintageVehicle.getID();
	if($Pref::Server::FASTKarts::AllowHover)
		return SpeedKartHoverVehicle.getID();
	if($Pref::Server::FASTKarts::AllowOriginal)
		return SpeedKartOriginalVehicle.getID();
	if($Pref::Server::FASTKarts::AllowDefault)
		return SpeedKartDefaultVehicle.getID();
	if($Pref::Server::FASTKarts::AllowSuperKart)
		return SpeedKartIIVehicle.getID();
	if($Pref::Server::FASTKarts::AllowSuperATV)
		return SpeedKartATVVehicle.getID();
	if($Pref::Server::FASTKarts::AllowSuperHover)
		return SpeedKartHoverIIVehicle.getID();
	if($Pref::Server::FASTKarts::AllowSuperJetski)
		return SpeedKartJetskiVehicle.getID();
	if($Pref::Server::FASTKarts::AllowSuperPlane)
		return SpeedKartPlaneVehicle.getID();
	
	error("ERROR: No karts allowed?");
	return 0;
}

function FK_uiNameChanges()
{
	//removing ui names from default vehicles
	BallVehicle.uiName = "";
	FlyingWheeledJeepVehicle.uiName = "";
	HorseArmor.uiName = "";
	JeepVehicle.uiName = "";
	MagicCarpetVehicle.uiName = "";
	CannonTurret.uiName = "";
	RowBoatArmor.uiName = "";
	TankTurretPlayer.uiName = "";
	TankVehicle.uiName = "";

	//giving karts the old ui names
	if($Pref::Server::FASTKarts::OldKartNames)
	{
		SpeedKartIIVehicle.uiName = "SpeedKart II";
		SpeedKartATVVehicle.uiName = "SpeedKart ATV";
		SpeedKartHoverIIVehicle.uiName = "SpeedKart Hover II";
		SpeedKartJetskiVehicle.uiName = "SpeedKart Jetski";
		SpeedKartPlaneVehicle.uiName = "SpeedKart Plane";
	}

	//hiding karts
	if(FK_getKartsAllowed() < 1)
		$Pref::Server::FASTKarts::AllowSpeedKart = true;

	if(!$Pref::Server::FASTKarts::AllowSpeedKart)
		SpeedKartVehicle.uiName = "";
	if(!$Pref::Server::FASTKarts::Allow64)
		SpeedKart64Vehicle.uiName = "";
	if(!$Pref::Server::FASTKarts::Allow7)
		SpeedKart7Vehicle.uiName = "";
	if(!$Pref::Server::FASTKarts::AllowBlocko)
		SpeedKartBlockoVehicle.uiName = "";
	if(!$Pref::Server::FASTKarts::AllowBuggy)
		SpeedKartBuggyVehicle.uiName = "";
	if(!$Pref::Server::FASTKarts::AllowClassic)
		SpeedKartClassicVehicle.uiName = "";
	if(!$Pref::Server::FASTKarts::AllowClassicGT)
		SpeedKartClassicGTVehicle.uiName = "";
	if(!$Pref::Server::FASTKarts::AllowFormula)
		SpeedKartFormulaVehicle.uiName = "";
	if(!$Pref::Server::FASTKarts::AllowHotrod)
		SpeedKartHotrodVehicle.uiName = "";
	if(!$Pref::Server::FASTKarts::AllowHyperion)
		SpeedKartHyperionVehicle.uiName = "";
	if(!$Pref::Server::FASTKarts::AllowJeep)
		SpeedKartJeepVehicle.uiName = "";
	if(!$Pref::Server::FASTKarts::AllowLeMans)
		SpeedKartLeMansVehicle.uiName = "";
	if(!$Pref::Server::FASTKarts::AllowMuscle)
		SpeedKartMuscleVehicle.uiName = "";
	if(!$Pref::Server::FASTKarts::AllowVintage)
		SpeedKartVintageVehicle.uiName = "";
	if(!$Pref::Server::FASTKarts::AllowHover)
		SpeedKartHoverVehicle.uiName = "";
	if(!$Pref::Server::FASTKarts::AllowOriginal)
		SpeedKartOriginalVehicle.uiName = "";
	if(!$Pref::Server::FASTKarts::AllowDefault)
		SpeedKartDefaultVehicle.uiName = "";
	if(!$Pref::Server::FASTKarts::AllowSuperKart)
		SpeedKartIIVehicle.uiName = "";
	if(!$Pref::Server::FASTKarts::AllowSuperATV)
		SpeedKartATVVehicle.uiName = "";
	if(!$Pref::Server::FASTKarts::AllowSuperHover)
		SpeedKartHoverIIVehicle.uiName = "";
	if(!$Pref::Server::FASTKarts::AllowSuperJetski)
		SpeedKartJetskiVehicle.uiName = "";
	if(!$Pref::Server::FASTKarts::AllowSuperPlane)
		SpeedKartPlaneVehicle.uiName = "";
}