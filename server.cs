if($GameModeArg !$= "Add-Ons/GameMode_FASTKarts/gamemode.txt" )
{
	warn("GameMode_FASTKarts cannot be used in custom games");
	warn("We'll just load the karts instead...");
	exec("config/server/FASTKarts/prefs.cs");
	exec("./addons/karts/Karts.cs");
	return;
}

function FK_SystemPrefs()
{
	$FK::Pref::System::Version = 7.1;
	$FK::Pref::System::Format = 2;
}

function FK_SetDefaultPrefs()
{
	echo("=== Applying default FASTKarts prefs ===");
	$FK::Pref::Rounds::PerTrack = 6;
	$FK::Pref::Rounds::AllowNormal = true;
	$FK::Pref::Rounds::AllowRocket = false;
	$FK::Pref::Rounds::AllowBouncy = false;
	
	$FK::Pref::Tracks::LoadRandomly = false;
	$FK::Pref::Tracks::Voting = 1;
	
	$FK::Pref::Tips::TipSeconds = 60;
	
	$FK::Pref::Gameplay::Achievements = true;
	$FK::Pref::Gameplay::CrumbleDeath = true;
	$FK::Pref::Gameplay::PGDieEffects = true;
	$FK::Pref::Gameplay::PGDieSounds = true;
	$FK::Pref::Gameplay::007Yells = true;
	$FK::Pref::Gameplay::VehiclelessWins = false;
	$FK::Pref::Gameplay::AnnounceRecords = false;
	$FK::Pref::Gameplay::ShowFullTime = false;
	
	$FK::Pref::Karts::AllowSpeedKart = true;
	$FK::Pref::Karts::Allow64 = true;
	$FK::Pref::Karts::Allow7 = true;
	$FK::Pref::Karts::AllowBlocko = true;
	$FK::Pref::Karts::AllowBuggy = true;
	$FK::Pref::Karts::AllowClassic = true;
	$FK::Pref::Karts::AllowClassicGT = true;
	$FK::Pref::Karts::AllowFormula = true;
	$FK::Pref::Karts::AllowHotrod = true;
	$FK::Pref::Karts::AllowHyperion = true;
	$FK::Pref::Karts::AllowJeep = true;
	$FK::Pref::Karts::AllowLeMans = true;
	$FK::Pref::Karts::AllowMuscle = true;
	$FK::Pref::Karts::AllowVintage = true;
	$FK::Pref::Karts::AllowHover = true;
	$FK::Pref::Karts::AllowOriginal = true;
	$FK::Pref::Karts::AllowDefault = true;
	$FK::Pref::Karts::AllowSuperKart = true;
	$FK::Pref::Karts::AllowSuperATV = true;
	$FK::Pref::Karts::AllowSuperHover = true;
	$FK::Pref::Karts::AllowSuperJetski = true;
	$FK::Pref::Karts::AllowSuperPlane = true;
	$FK::Pref::Karts::HornSounds = true;
	$FK::Pref::Karts::HornMS = 50;
	$FK::Pref::Karts::EngineSounds = true;
	$FK::Pref::Karts::UpDownLeaning = false;
	$FK::Pref::Karts::OldNames = false;
	$FK::Pref::Karts::ForceDefault = false;
	
	$FK::Pref::Administration::HideCommands = false;
	$DM::Pref::Administration::AdminTags = true;
	$DM::Pref::Administration::FloodProtection = true;
	
	$FK::Pref::Exec::Addon1 = "";
	$FK::Pref::Exec::Addon2 = "";
	$FK::Pref::Exec::Addon3 = "";
	$FK::Pref::Exec::Addon4 = "";
	$FK::Pref::Exec::Addon5 = "";
	$FK::Pref::Exec::Addon6 = "";
	$FK::Pref::Exec::Addon7 = "";
	$FK::Pref::Exec::Addon8 = "";
	$FK::Pref::Exec::Addon9 = "";
	$FK::Pref::Exec::Addon10 = "";
	
	$FK::Preset::Defaults = false;
	$FK::Preset::SpeedKart = false;
	$FK::Preset::Everything = false;
	echo("");
}

function FK_LoadPrefs()
{
	deleteVariables("$FK::Pref::*");
	FK_SetDefaultPrefs();
	if(isFile("config/server/FASTKarts/prefs.cs"))
	{
		echo("=== Loading saved FASTKarts prefs ===");
		exec("config/server/FASTKarts/prefs.cs");
		
		if($FK::Pref::System::Format != 2)
		{
			if(mfloor($FK::Pref::System::Format) < 2)
			{
				if($FK::Pref::System::Format $= "")
					warn("Saved FK prefs are in an unnumbered (probably older) format.");
				else
					warn("Saved FK prefs are from an older format.");
			}
			else if(mfloor($FK::Pref::System::Format) > 2)
				warn("Saved FK prefs are from an unknown newer format.");
			
			warn("Clearing old prefs and reloading defaults...");
			deleteVariables("$FK::Pref::*");
			FK_SetDefaultPrefs();
		}
		
		echo("");
	}
	
	if(isFile("config/server/FASTKarts/records.cs"))
	{
		echo("=== Loading saved FASTKarts records ===");
		deleteVariables("$FK::Record_*");
		exec("config/server/FASTKarts/records.cs");
		
		echo("");
	}
	
	FK_SystemPrefs();
	export("$FK::Pref::*", "config/server/FASTKarts/prefs.cs");
}

function FK_RegisterRTBPrefs()
{
	RTB_registerPref("Rounds per track (-1 disables)",			"FASTKarts - Rounds",				"$FK::Pref::Rounds::PerTrack",					"int -1 9999",							"GameMode_FASTKarts",	6,		false,	false,	false);
	RTB_registerPref("Allow Normal round type",					"FASTKarts - Rounds",				"$FK::Pref::Rounds::AllowNormal",				"bool",									"GameMode_FASTKarts",	true,	false,	false,	false);
	RTB_registerPref("Allow Rocket round type",					"FASTKarts - Rounds",				"$FK::Pref::Rounds::AllowRocket",				"bool",									"GameMode_FASTKarts",	false,	false,	false,	false);
	RTB_registerPref("Allow Bouncy round type",					"FASTKarts - Rounds",				"$FK::Pref::Rounds::AllowBouncy",				"bool",									"GameMode_FASTKarts",	false,	false,	false,	false);
	
	RTB_registerPref("Load tracks in random order",				"FASTKarts - Tracks",				"$FK::Pref::Tracks::LoadRandomly",				"bool",									"GameMode_FASTKarts",	false,	false,	false,	false);
	RTB_registerPref("Allow player track voting",				"FASTKarts - Tracks",				"$FK::Pref::Tracks::Voting",					"list Disallow 0 Allow 1 Round_Only 2",	"GameMode_FASTKarts",	1,		false,	false,	false);
	
	RTB_registerPref("Show tip every X seconds (-1 disables)",	"FASTKarts - Tips",					"$FK::Pref::Tips::TipSeconds",					"int -1 99999",							"GameMode_FASTKarts",	60,		false,	false,	false);
	
	RTB_registerPref("Enable achievements system",				"FASTKarts - Gameplay",				"$FK::Pref::Gameplay::Achievements",			"bool",									"GameMode_FASTKarts",	true,	false,	false,	false);
	RTB_registerPref("Enable crumble death effect",				"FASTKarts - Gameplay",				"$FK::Pref::Gameplay::CrumbleDeath",			"bool",									"GameMode_FASTKarts",	true,	false,	false,	false);
	RTB_registerPref("Enable PGDie death effects",				"FASTKarts - Gameplay",				"$FK::Pref::Gameplay::PGDieEffects",			"bool",									"GameMode_FASTKarts",	true,	false,	false,	false);
	RTB_registerPref("Enable PGDie death sounds",				"FASTKarts - Gameplay",				"$FK::Pref::Gameplay::PGDieSounds",				"bool",									"GameMode_FASTKarts",	true,	false,	false,	false);
	RTB_registerPref("Enable random death yells",				"FASTKarts - Gameplay",				"$FK::Pref::Gameplay::007Yells",				"bool",									"GameMode_FASTKarts",	true,	false,	false,	false);
	RTB_registerPref("Allow vehicleless race completions",		"FASTKarts - Gameplay",				"$FK::Pref::Gameplay::VehiclelessWins",			"bool",									"GameMode_FASTKarts",	false,	false,	false,	false);
	RTB_registerPref("Announce new records",					"FASTKarts - Gameplay",				"$FK::Pref::Gameplay::AnnounceRecords",			"bool",									"GameMode_FASTKarts",	false,	false,	false,	false);
	RTB_registerPref("Show full time on race completion",		"FASTKarts - Gameplay",				"$FK::Pref::Gameplay::ShowFullTime",			"bool",									"GameMode_FASTKarts",	false,	false,	false,	false);
	
	RTB_registerPref("Allow SpeedKart",							"FASTKarts - Karts",				"$FK::Pref::Karts::AllowSpeedKart",				"bool",									"GameMode_FASTKarts",	true,	true,	false,	false);
	RTB_registerPref("Allow SpeedKart 64",						"FASTKarts - Karts",				"$FK::Pref::Karts::Allow64",					"bool",									"GameMode_FASTKarts",	true,	true,	false,	false);
	RTB_registerPref("Allow SpeedKart 7",						"FASTKarts - Karts",				"$FK::Pref::Karts::Allow7",						"bool",									"GameMode_FASTKarts",	true,	true,	false,	false);
	RTB_registerPref("Allow SpeedKart Blocko",					"FASTKarts - Karts",				"$FK::Pref::Karts::AllowBlocko",				"bool",									"GameMode_FASTKarts",	true,	true,	false,	false);
	RTB_registerPref("Allow SpeedKart Buggy",					"FASTKarts - Karts",				"$FK::Pref::Karts::AllowBuggy",					"bool",									"GameMode_FASTKarts",	true,	true,	false,	false);
	RTB_registerPref("Allow SpeedKart Classic",					"FASTKarts - Karts",				"$FK::Pref::Karts::AllowClassic",				"bool",									"GameMode_FASTKarts",	true,	true,	false,	false);
	RTB_registerPref("Allow SpeedKart Classic GT",				"FASTKarts - Karts",				"$FK::Pref::Karts::AllowClassicGT",				"bool",									"GameMode_FASTKarts",	true,	true,	false,	false);
	RTB_registerPref("Allow SpeedKart Formula",					"FASTKarts - Karts",				"$FK::Pref::Karts::AllowFormula",				"bool",									"GameMode_FASTKarts",	true,	true,	false,	false);
	RTB_registerPref("Allow SpeedKart Hotrod",					"FASTKarts - Karts",				"$FK::Pref::Karts::AllowHotrod",				"bool",									"GameMode_FASTKarts",	true,	true,	false,	false);
	RTB_registerPref("Allow SpeedKart Hyperion",				"FASTKarts - Karts",				"$FK::Pref::Karts::AllowHyperion",				"bool",									"GameMode_FASTKarts",	true,	true,	false,	false);
	RTB_registerPref("Allow SpeedKart Jeep",					"FASTKarts - Karts",				"$FK::Pref::Karts::AllowJeep",					"bool",									"GameMode_FASTKarts",	true,	true,	false,	false);
	RTB_registerPref("Allow SpeedKart LeMans",					"FASTKarts - Karts",				"$FK::Pref::Karts::AllowLeMans",				"bool",									"GameMode_FASTKarts",	true,	true,	false,	false);
	RTB_registerPref("Allow SpeedKart Muscle",					"FASTKarts - Karts",				"$FK::Pref::Karts::AllowMuscle",				"bool",									"GameMode_FASTKarts",	true,	true,	false,	false);
	RTB_registerPref("Allow SpeedKart Vintage",					"FASTKarts - Karts",				"$FK::Pref::Karts::AllowVintage",				"bool",									"GameMode_FASTKarts",	true,	true,	false,	false);
	RTB_registerPref("Allow SpeedKart Hover",					"FASTKarts - Karts",				"$FK::Pref::Karts::AllowHover",					"bool",									"GameMode_FASTKarts",	true,	true,	false,	false);
	RTB_registerPref("Allow SpeedKart Original",				"FASTKarts - Karts",				"$FK::Pref::Karts::AllowOriginal",				"bool",									"GameMode_FASTKarts",	true,	true,	false,	false);
	RTB_registerPref("Allow SpeedKart Default",					"FASTKarts - Karts",				"$FK::Pref::Karts::AllowDefault",				"bool",									"GameMode_FASTKarts",	true,	true,	false,	false);
	if($FK::Pref::Karts::OldNames)
	{
		RTB_registerPref("Allow SpeedKart II",					"FASTKarts - Karts",				"$FK::Pref::Karts::AllowSuperKart",				"bool",									"GameMode_FASTKarts",	true,	true,	false,	false);
		RTB_registerPref("Allow SpeedKart ATV",					"FASTKarts - Karts",				"$FK::Pref::Karts::AllowSuperATV",				"bool",									"GameMode_FASTKarts",	true,	true,	false,	false);
		RTB_registerPref("Allow SpeedKart Hover II",			"FASTKarts - Karts",				"$FK::Pref::Karts::AllowSuperHover",			"bool",									"GameMode_FASTKarts",	true,	true,	false,	false);
		RTB_registerPref("Allow SpeedKart Jetski",				"FASTKarts - Karts",				"$FK::Pref::Karts::AllowSuperJetski",			"bool",									"GameMode_FASTKarts",	true,	true,	false,	false);
		RTB_registerPref("Allow SpeedKart Plane",				"FASTKarts - Karts",				"$FK::Pref::Karts::AllowSuperPlane",			"bool",									"GameMode_FASTKarts",	true,	true,	false,	false);
	}
	else
	{
		RTB_registerPref("Allow SuperKart",						"FASTKarts - Karts",				"$FK::Pref::Karts::AllowSuperKart",				"bool",									"GameMode_FASTKarts",	true,	true,	false,	false);
		RTB_registerPref("Allow SuperKart ATV",					"FASTKarts - Karts",				"$FK::Pref::Karts::AllowSuperATV",				"bool",									"GameMode_FASTKarts",	true,	true,	false,	false);
		RTB_registerPref("Allow SuperKart Hover",				"FASTKarts - Karts",				"$FK::Pref::Karts::AllowSuperHover",			"bool",									"GameMode_FASTKarts",	true,	true,	false,	false);
		RTB_registerPref("Allow SuperKart Jetski",				"FASTKarts - Karts",				"$FK::Pref::Karts::AllowSuperJetski",			"bool",									"GameMode_FASTKarts",	true,	true,	false,	false);
		RTB_registerPref("Allow SuperKart Plane",				"FASTKarts - Karts",				"$FK::Pref::Karts::AllowSuperPlane",			"bool",									"GameMode_FASTKarts",	true,	true,	false,	false);
	}
	RTB_registerPref("Horn Sounds",								"FASTKarts - Karts",				"$FK::Pref::Karts::HornSounds",					"bool",									"GameMode_FASTKarts",	true,	false,	false,	false);
	RTB_registerPref("Horn Delay",								"FASTKarts - Karts",				"$FK::Pref::Karts::HornMS",						"int 0 9999",							"GameMode_FASTKarts",	50,		false,	false,	false);
	RTB_registerPref("Engine Sounds",							"FASTKarts - Karts",				"$FK::Pref::Karts::EngineSounds",				"bool",									"GameMode_FASTKarts",	true,	false,	false,	false);
	RTB_registerPref("Back and Forward leaning in karts",		"FASTKarts - Karts",				"$FK::Pref::Karts::UpDownLeaning",				"bool",									"GameMode_FASTKarts",	false,	true,	false,	false);
	RTB_registerPref("Use Old Names",							"FASTKarts - Karts",				"$FK::Pref::Karts::OldNames",					"bool",									"GameMode_FASTKarts",	false,	true,	false,	false);
	RTB_registerPref("Force default SpeedKarts",				"FASTKarts - Karts",				"$FK::Pref::Karts::ForceDefault",				"bool",									"GameMode_FASTKarts",	false,	true,	false,	false);
	
	RTB_registerPref("Hide admin command lists",				"FASTKarts - Administration",		"$FK::Pref::Administration::HideCommands",		"bool",									"GameMode_FASTKarts",	false,	false,	false,	false);
	RTB_registerPref("Give admins special clan tags",			"FASTKarts - Administration",		"$FK::Pref::Administration::AdminTags",			"bool",									"GameMode_FASTKarts",	true,	false,	false,	false);
	RTB_registerPref("Enable chat flood protection",			"FASTKarts - Administration",		"$FK::Pref::Administration::FloodProtection",	"bool",									"GameMode_FASTKarts",	true,	false,	false,	false);
	
	RTB_registerPref("Defaults (WARNING: resets everything!)",	"FASTKarts - Presets",				"$FK::Preset::Defaults",						"bool",									"GameMode_FASTKarts",	false,	true,	false,	false);
	RTB_registerPref("I <3 SpeedKart",							"FASTKarts - Presets",				"$FK::Preset::SpeedKart",						"bool",									"GameMode_FASTKarts",	false,	false,	false,	false);
	RTB_registerPref("Give me everything.",						"FASTKarts - Presets",				"$FK::Preset::Everything",						"bool",									"GameMode_FASTKarts",	false,	false,	false,	false);
	
	RTB_registerPref("First add-on to load",					"FASTKarts - AddOns",				"$FK::Pref::Exec::Addon1",						"string 0 200",							"GameMode_FASTKarts",	"",		true,	false,	false);
	RTB_registerPref("Second add-on to load",					"FASTKarts - AddOns",				"$FK::Pref::Exec::Addon2",						"string 0 200",							"GameMode_FASTKarts",	"",		true,	false,	false);
	RTB_registerPref("Third add-on to load",					"FASTKarts - AddOns",				"$FK::Pref::Exec::Addon3",						"string 0 200",							"GameMode_FASTKarts",	"",		true,	false,	false);
	RTB_registerPref("Fourth add-on to load",					"FASTKarts - AddOns",				"$FK::Pref::Exec::Addon4",						"string 0 200",							"GameMode_FASTKarts",	"",		true,	false,	false);
	RTB_registerPref("Fifth add-on to load",					"FASTKarts - AddOns",				"$FK::Pref::Exec::Addon5",						"string 0 200",							"GameMode_FASTKarts",	"",		true,	false,	false);
	RTB_registerPref("Sixth add-on to load",					"FASTKarts - AddOns",				"$FK::Pref::Exec::Addon6",						"string 0 200",							"GameMode_FASTKarts",	"",		true,	false,	false);
	RTB_registerPref("Seventh add-on to load",					"FASTKarts - AddOns",				"$FK::Pref::Exec::Addon7",						"string 0 200",							"GameMode_FASTKarts",	"",		true,	false,	false);
	RTB_registerPref("Eighth add-on to load",					"FASTKarts - AddOns",				"$FK::Pref::Exec::Addon8",						"string 0 200",							"GameMode_FASTKarts",	"",		true,	false,	false);
	RTB_registerPref("Ninth add-on to load",					"FASTKarts - AddOns",				"$FK::Pref::Exec::Addon9",						"string 0 200",							"GameMode_FASTKarts",	"",		true,	false,	false);
	RTB_registerPref("Tenth add-on to load",					"FASTKarts - AddOns",				"$FK::Pref::Exec::Addon10",						"string 0 200",							"GameMode_FASTKarts",	"",		true,	false,	false);
}

if(!$FK::Initialized)
{
	echo("Starting up FASTKarts Gamemode...");
	FK_SystemPrefs();
	echo("Version: " @ $FK::Pref::System::Version);
	echo("Pref Format: " @ $FK::Pref::System::Format);
	echo("");
	FK_LoadPrefs();
	
	echo("=== Executing add-ons manually that the host might have ===");
	
	if(isFile("Add-Ons/Tool_NewDuplicator/server.cs"))
		exec("Add-Ons/Tool_NewDuplicator/server.cs");
	else
		warn("Tool_NewDuplicator was not found");
	
	echo("");
	
	echo("=== Executing implemented FASTKarts add-ons ===");
	exec("./addons/main.cs");
	echo("");
	
	echo("=== Executing custom made FASTKarts add-ons ===");
	exec("./custom/main.cs");
	echo("");
}

echo("=== Executing FASTKarts core files ===");
exec("./core/main.cs");
echo("");
echo("=== Finished executing FASTKarts files ===");
echo("");

if(!$FK::Initialized)
{
	echo("=== Executing add-ons that the host wanted ===");
	
	if($FK::Pref::Exec::Addon1 !$= "" && isFile("Add-Ons/" @ $FK::Pref::Exec::Addon1 @ "/server.cs"))
		exec("Add-Ons/" @ $FK::Pref::Exec::Addon1 @ "/server.cs");
	
	if($FK::Pref::Exec::Addon2 !$= "" && isFile("Add-Ons/" @ $FK::Pref::Exec::Addon2 @ "/server.cs"))
		exec("Add-Ons/" @ $FK::Pref::Exec::Addon2 @ "/server.cs");
	
	if($FK::Pref::Exec::Addon3 !$= "" && isFile("Add-Ons/" @ $FK::Pref::Exec::Addon3 @ "/server.cs"))
		exec("Add-Ons/" @ $FK::Pref::Exec::Addon3 @ "/server.cs");
	
	if($FK::Pref::Exec::Addon4 !$= "" && isFile("Add-Ons/" @ $FK::Pref::Exec::Addon4 @ "/server.cs"))
		exec("Add-Ons/" @ $FK::Pref::Exec::Addon4 @ "/server.cs");
	
	if($FK::Pref::Exec::Addon5 !$= "" && isFile("Add-Ons/" @ $FK::Pref::Exec::Addon5 @ "/server.cs"))
		exec("Add-Ons/" @ $FK::Pref::Exec::Addon5 @ "/server.cs");
	
	if($FK::Pref::Exec::Addon6 !$= "" && isFile("Add-Ons/" @ $FK::Pref::Exec::Addon6 @ "/server.cs"))
		exec("Add-Ons/" @ $FK::Pref::Exec::Addon6 @ "/server.cs");
	
	if($FK::Pref::Exec::Addon7 !$= "" && isFile("Add-Ons/" @ $FK::Pref::Exec::Addon7 @ "/server.cs"))
		exec("Add-Ons/" @ $FK::Pref::Exec::Addon7 @ "/server.cs");
	
	if($FK::Pref::Exec::Addon8 !$= "" && isFile("Add-Ons/" @ $FK::Pref::Exec::Addon8 @ "/server.cs"))
		exec("Add-Ons/" @ $FK::Pref::Exec::Addon8 @ "/server.cs");
	
	if($FK::Pref::Exec::Addon9 !$= "" && isFile("Add-Ons/" @ $FK::Pref::Exec::Addon9 @ "/server.cs"))
		exec("Add-Ons/" @ $FK::Pref::Exec::Addon9 @ "/server.cs");
	
	if($FK::Pref::Exec::Addon10 !$= "" && isFile("Add-Ons/" @ $FK::Pref::Exec::Addon10 @ "/server.cs"))
		exec("Add-Ons/" @ $FK::Pref::Exec::Addon10 @ "/server.cs");
	
	//chat emotes compatability
	if(isPackage(chatEmotesServer))
		deactivatePackage(chatEmotesServer);
	//we call peReplace in our own package if it exists
	
	echo("");
	
	//support_preferences compatatbility
	if($RTB::Hooks::ServerControl)
	{
		FK_RegisterRTBPrefs();
		echo("Making sure FASTKarts' prefs.cs changes are loaded...");
		FK_LoadPrefs();
		
		if(isPackage(GameModeFASTKartsSupportPreferencesPackage))
			deactivatePackage(GameModeFASTKartsSupportPreferencesPackage);
		if(isFunction(serverCmdUpdatePref))
			activatePackage(GameModeFASTKartsSupportPreferencesPackage);
	}
	
	//$FK::Initialized = true; //GameModeInitialResetCheck does this already
	FK_Tick();
	FK_TipTick();
	FK_uiNameChanges();
}

function FK_exec()
{
	setmodpaths(getmodpaths());
	exec("Add-Ons/Gamemode_FASTKarts/server.cs");
}