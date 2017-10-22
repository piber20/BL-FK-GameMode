//core files
exec("./tracks.cs");
exec("./karts.cs");
exec("./commands.cs");
exec("./rounds.cs");
exec("./package.cs");
exec("./achievements.cs");
exec("./help.cs");
exec("./debug.cs");

function FK_Tick()
{
	cancel($FK::Tick);
	
	for(%a = 0; %a < $DefaultMinigame.numMembers; %a++)
	{
		%member = $DefaultMinigame.member[%a];
		if(isObject(%member))
		{
			%player = %member.player;
			if(isObject(%player))
			{
				%player.setShapeNameColor(FK_getRoundShapeNameColor());
				
				if($Pref::Server::FASTKarts::NoKartKillTime > -1)
				{
					if($FK::TrackStarted && $FK::RoundType !$= "BOUNCY")
					{
						%vehicle = %player.getObjectMount();
						if(isObject(%vehicle))
							%player.noKartKillTime = $Pref::Server::FASTKarts::NoKartKillTime * 10;
						else
						{
							if($Pref::Server::FASTKarts::NoKartKillTime == 0)
								%player.kill();
							else
							{
								%player.noKartKillTime--;
								if(%player.noKartKillTime <= 0)
								{
									%player.kill();
									centerPrint(%member, "", 5);
								}
								else
								{
									%seconds = mceil(%player.noKartKillTime * 0.1);
									%color = FK_getRoundColorString();
									%text = "<just:right>" @ "\c6You will be killed in<br>" @ %color @ %seconds @ " second" @ (%seconds > 1 ? "s" : "") @ " \c6if you<br>\c6do not enter a kart.";
									centerPrint(%member, %text, 5);
								}
							}
						}
					}
				}
			}
			
			%member.FK_setBottomPrintInfo();
		}
	}
	
	$FK::Tick = schedule(100, 0, FK_Tick);
}

function gameConnection::FK_setBottomPrintInfo(%this)
{
	%mg = %this.miniGame;

	if(!isObject(%mg))
		%mg = $DefaultMiniGame;

	if(!isObject(%mg))
		return;
	
	if($FK::ResetCount == 0)
	{
		//loading message
		%this.FKBottomPrint = "<just:center>";
		%this.FKBottomPrint = %this.FKBottomPrint @ "\c6Loading \c4" @ FK_getTrackName($FK::CurrentTrack) @ "\c6...";
	}
	else
	{
		//colors
		%color = FK_getRoundColorString();
		
		//left
		%this.FKBottomPrint = "<just:left>";
		
		//round
		if($Pref::Server::FASTKarts::RoundLimit > 0 && $FK::numTracks > 0)
			%this.FKBottomPrint = %this.FKBottomPrint @ "\c6Round" @ %color @ ": " @ mfloor($FK::ResetCount) @ "/" @ mfloor($Pref::Server::FASTKarts::RoundLimit) @ "   ";
		
		//time
		if(!$FK::TrackCompleted)
		{
			if($FK::TrackStarted)
				%this.FKBottomPrint = %this.FKBottomPrint @ "\c6Time" @ %color @ ": " @ FK_getTimeLeft(%mg);
			else
			{
				if((getSimTime() > (%mg.lastResetTime + 7000)) || FK_getRoundTypesAllowed() <= 1)
				{
					if($FK::RoundType $= "BOUNCY")
						centerPrint(%this, %color @ "This is a Bouncy round. No karts allowed!", 1);
					else if(!%this.FK_HasSpawnedSpeedKart)
						centerPrint(%this, %color @ "Spawn a vehicle by clicking the vehicle spawn and choosing a vehicle.", 1);
					else if(!isObject(%this.player.getObjectMount()))
						centerPrint(%this, %color @ "Enter a vehicle by jumping on top of it.", 1);
				}
				else
					centerPrint(%this, "<font:palatino linotype:64>" @ $FK::RoundName, 1);
				
				%this.FKBottomPrint = %this.FKBottomPrint @ "\c6Time" @ %color @ ": 0:00";
			}
		}
		
		//right
		%this.FKBottomPrint = %this.FKBottomPrint @ "<just:right>";
		
		//laps
		if(!$FK::TrackCompleted && $FK::TrackStarted && ($FK::TrackType[$FK::CurrentTrack] $= "Lapped" || $FK::FinalLap > 1))
			%this.FKBottomPrint = %this.FKBottomPrint @ "\c6Lap" @ %color @ ": " @ %this.FASTKartsLap @ "/" @ $FK::FinalLap @ " ";
		
		//next line
		%this.FKBottomPrint = %this.FKBottomPrint @ "<br><just:left>";
		
		//track
		if($FK::numTracks > 0)
			%this.FKBottomPrint = %this.FKBottomPrint @ "\c6Track" @ %color @ ": " @ FK_getTrackName($FK::CurrentTrack);
	}
		
	//completed building it, send it.
	commandToClient(%this, 'bottomPrint', %this.FKBottomPrint, 0, true);
}