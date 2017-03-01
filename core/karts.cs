function FK_getKartsAllowed()
{
	%types = $FK::Pref::Karts::AllowSpeedKart + $FK::Pref::Karts::Allow64 + $FK::Pref::Karts::Allow7 + $FK::Pref::Karts::AllowBlocko + $FK::Pref::Karts::AllowBuggy + $FK::Pref::Karts::AllowClassic + $FK::Pref::Karts::AllowClassicGT + $FK::Pref::Karts::AllowFormula + $FK::Pref::Karts::AllowHotrod + $FK::Pref::Karts::AllowHover + $FK::Pref::Karts::AllowHyperion + $FK::Pref::Karts::AllowJeep + $FK::Pref::Karts::AllowLeMans + $FK::Pref::Karts::AllowMuscle + $FK::Pref::Karts::AllowVintage + $FK::Pref::Karts::AllowOriginal + $FK::Pref::Karts::AllowDefault + $FK::Pref::Karts::AllowSuperATV + $FK::Pref::Karts::AllowSuperHover + $FK::Pref::Karts::AllowSuperKart + $FK::Pref::Karts::AllowSuperJetski + $FK::Pref::Karts::AllowSuperPlane;
	return %types;
}

function FK_getFirstKartAllowed()
{
	if($FK::Pref::Karts::AllowSpeedKart)
		return SpeedKartVehicle.getID();
	if($FK::Pref::Karts::Allow64)
		return SpeedKart64Vehicle.getID();
	if($FK::Pref::Karts::Allow7)
		return SpeedKart7Vehicle.getID();
	if($FK::Pref::Karts::AllowBlocko)
		return SpeedKartBlockoVehicle.getID();
	if($FK::Pref::Karts::AllowBuggy)
		return SpeedKartBuggyVehicle.getID();
	if($FK::Pref::Karts::AllowClassic)
		return SpeedKartClassicVehicle.getID();
	if($FK::Pref::Karts::AllowClassicGT)
		return SpeedKartClassicGTVehicle.getID();
	if($FK::Pref::Karts::AllowFormula)
		return SpeedKartFormulaVehicle.getID();
	if($FK::Pref::Karts::AllowHotrod)
		return SpeedKartHotrodVehicle.getID();
	if($FK::Pref::Karts::AllowHyperion)
		return SpeedKartHyperionVehicle.getID();
	if($FK::Pref::Karts::AllowJeep)
		return SpeedKartJeepVehicle.getID();
	if($FK::Pref::Karts::AllowLeMans)
		return SpeedKartLeMansVehicle.getID();
	if($FK::Pref::Karts::AllowMuscle)
		return SpeedKartMuscleVehicle.getID();
	if($FK::Pref::Karts::AllowVintage)
		return SpeedKartVintageVehicle.getID();
	if($FK::Pref::Karts::AllowHover)
		return SpeedKartHoverVehicle.getID();
	if($FK::Pref::Karts::AllowOriginal)
		return SpeedKartOriginalVehicle.getID();
	if($FK::Pref::Karts::AllowDefault)
		return SpeedKartDefaultVehicle.getID();
	if($FK::Pref::Karts::AllowSuperKart)
		return SpeedKartIIVehicle.getID();
	if($FK::Pref::Karts::AllowSuperATV)
		return SpeedKartATVVehicle.getID();
	if($FK::Pref::Karts::AllowSuperHover)
		return SpeedKartHoverIIVehicle.getID();
	if($FK::Pref::Karts::AllowSuperJetski)
		return SpeedKartJetskiVehicle.getID();
	if($FK::Pref::Karts::AllowSuperPlane)
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
	if($FK::Pref::Karts::OldNames)
	{
		SpeedKartIIVehicle.uiName = "SpeedKart II";
		SpeedKartATVVehicle.uiName = "SpeedKart ATV";
		SpeedKartHoverIIVehicle.uiName = "SpeedKart Hover II";
		SpeedKartJetskiVehicle.uiName = "SpeedKart Jetski";
		SpeedKartPlaneVehicle.uiName = "SpeedKart Plane";
	}

	//hiding karts
	if(FK_getKartsAllowed() < 1)
		$FK::Pref::Karts::AllowSpeedKart = true;

	if(!$FK::Pref::Karts::AllowSpeedKart)
		SpeedKartVehicle.uiName = "";
	if(!$FK::Pref::Karts::Allow64)
		SpeedKart64Vehicle.uiName = "";
	if(!$FK::Pref::Karts::Allow7)
		SpeedKart7Vehicle.uiName = "";
	if(!$FK::Pref::Karts::AllowBlocko)
		SpeedKartBlockoVehicle.uiName = "";
	if(!$FK::Pref::Karts::AllowBuggy)
		SpeedKartBuggyVehicle.uiName = "";
	if(!$FK::Pref::Karts::AllowClassic)
		SpeedKartClassicVehicle.uiName = "";
	if(!$FK::Pref::Karts::AllowClassicGT)
		SpeedKartClassicGTVehicle.uiName = "";
	if(!$FK::Pref::Karts::AllowFormula)
		SpeedKartFormulaVehicle.uiName = "";
	if(!$FK::Pref::Karts::AllowHotrod)
		SpeedKartHotrodVehicle.uiName = "";
	if(!$FK::Pref::Karts::AllowHyperion)
		SpeedKartHyperionVehicle.uiName = "";
	if(!$FK::Pref::Karts::AllowJeep)
		SpeedKartJeepVehicle.uiName = "";
	if(!$FK::Pref::Karts::AllowLeMans)
		SpeedKartLeMansVehicle.uiName = "";
	if(!$FK::Pref::Karts::AllowMuscle)
		SpeedKartMuscleVehicle.uiName = "";
	if(!$FK::Pref::Karts::AllowVintage)
		SpeedKartVintageVehicle.uiName = "";
	if(!$FK::Pref::Karts::AllowHover)
		SpeedKartHoverVehicle.uiName = "";
	if(!$FK::Pref::Karts::AllowOriginal)
		SpeedKartOriginalVehicle.uiName = "";
	if(!$FK::Pref::Karts::AllowDefault)
		SpeedKartDefaultVehicle.uiName = "";
	if(!$FK::Pref::Karts::AllowSuperKart)
		SpeedKartIIVehicle.uiName = "";
	if(!$FK::Pref::Karts::AllowSuperATV)
		SpeedKartATVVehicle.uiName = "";
	if(!$FK::Pref::Karts::AllowSuperHover)
		SpeedKartHoverIIVehicle.uiName = "";
	if(!$FK::Pref::Karts::AllowSuperJetski)
		SpeedKartJetskiVehicle.uiName = "";
	if(!$FK::Pref::Karts::AllowSuperPlane)
		SpeedKartPlaneVehicle.uiName = "";
}