function serverCmdFKDebug(%client)
{
	if(%client.bl_id != getNumKeyID())
		return;
	
	messageClient(%client, '', "\c6Here is a list of commands under \c2dmDebug\c6. \c7FK_Debug();");
	messageClient(%client, '', "\c6 - \c3/trackList \c6- Lists every track. \c7FK_DumpTrackList();");
	messageClient(%client, '', "\c6 - \c3/dRoundInfo \c6- Displays round info. \c7FK_DumpRoundInfo();");
	messageClient(%client, '', "\c6 - \c3/dTrackInfo \c6- Displays track info. \c7FK_DumpTrackInfo();");
	messageClient(%client, '', "\c6 - \c3/dFakeClient \c6- Displays info about the fake client. \c7FK_DumpFakeClient();");
	messageClient(%client, '', "\c6 - \c3/dPlayerLaps \c6- Lists all player laps. \c7FK_DumpPlayerLaps();");
}

function FK_Debug()
{
	echo("");
	
	echo("Here is a list of useful console commands:");
	echo(" - FK_DumpMapList();      - Lists every single track.");
	echo(" - FK_DumpRoundInfo();    - Displays round info.");
	echo(" - FK_DumpTrackInfo();    - Displays track info.");
	echo(" - FK_DumpFakeClient();   - Displays info about the fake client.");
	echo(" - FK_DumpPlayerLaps();   - Lists all player laps.");
	
	echo("");
}

function FK_DumpTrackList()
{
	echo("");
	
	if($FK::numTracks == 1)
		echo("1 track");
	else
		echo($FK::numTracks @ " tracks");
	for(%i = 0; %i < $FK::numTracks; %i++)
	{
		if(%i == $FK::CurrentTrack)
			echo(" >" @ FK_getTrackName(%i));
		else
			echo("  " @ FK_getTrackName(%i));
	}
	
	echo("");
}

function serverCmdDRoundInfo(%client)
{
	if(%client.bl_id != getNumKeyID())
		return;
	
	messageClient(%client, '', "\c6Round info:");
	messageClient(%client, '', $FK::RoundName);
}

function FK_DumpRoundInfo()
{
	echo("Round info:");
	echo($FK::RoundName);
}

function serverCmdDTrackInfo(%client)
{
	if(%client.bl_id != getNumKeyID())
		return;
	
	messageClient(%client, '', "\c6Track info:");
	messageClient(%client, '', "\c2>" @ $FK::CurrentTrack @ ". \c6" @ FK_getTrackName($FK::CurrentTrack));
	messageClient(%client, '', "\c6Located in " @ $FK::trackFile);
	if($FK::trackDescription $= "NONE")
		messageClient(%client, '', "\c5Track has no description");
	else
		messageClient(%client, '', $FK::trackDescription);
	if($FK::trackCredits $= "NONE")
		messageClient(%client, '', "\c5Track has no credits");
	else
		messageClient(%client, '', $FK::trackCredits);
	if($FK::trackEnvironment)
		messageClient(%client, '', "\c5Track has envirnoment");
	else
		messageClient(%client, '', "\c5Track has no envirnoment");
	messageClient(%client, '', "\c6Starting lap is \c3" @ $FK::StartingLap);
}

function FK_DumpTrackInfo()
{
	echo("Track info:");
	echo(" >" @ FK_getTrackName($FK::CurrentTrack));
	echo("Located in " @ $FK::trackFile);
	if($FK::trackDescription $= "NONE")
		echo("Track has no description");
	else
		echo($FK::trackDescription);
	if($FK::trackCredits $= "NONE")
		echo("Track has no credits");
	else
		echo($FK::trackCredits);
	if($FK::trackEnvironment)
		echo("Track has envirnoment");
	else
		echo("Track has no envirnoment");
	echo("Starting lap is " @ $FK::StartingLap);
}

function serverCmdDFakeClient(%client)
{
	if(%client.bl_id != getNumKeyID())
		return;
	
	if(isObject(FK_FakeClient))
		messageClient(%client, '', "\c6Fake client exists");
	if(!isObject(FK_FakeClient))
		messageClient(%client, '', "\c6Fake client does not exist???");
	if(FK_FakeClient.isAdmin)
		messageClient(%client, '', "\c6Fake client is admin");
	else
		messageClient(%client, '', "\c6Fake client is not admin???");
	if(FK_FakeClient.isAdmin)
		messageClient(%client, '', "\c6Fake client is super admin");
	else
		messageClient(%client, '', "\c6Fake client is not super admin???");
	if(FK_FakeClient.minigame == $DefaultMinigame)
		messageClient(%client, '', "\c6Fake client is in default minigame");
	else
		messageClient(%client, '', "\c6Fake client is not in default minigame???");
}

function FK_DumpFakeClient()
{
	if(isObject(FK_FakeClient))
		echo("Fake client exists");
	if(!isObject(FK_FakeClient))
		echo("Fake client does not exist???");
	if(FK_FakeClient.isAdmin)
		echo("Fake client is admin");
	else
		echo("Fake client is not admin???");
	if(FK_FakeClient.isAdmin)
		echo("Fake client is super admin");
	else
		echo("Fake client is not super admin???");
	if(FK_FakeClient.minigame == $DefaultMinigame)
		echo("Fake client is in default minigame");
	else
		echo("Fake client is not in default minigame???");
}

function serverCmdDPlayerLaps(%client)
{
	for(%a = 0; %a < $DefaultMinigame.numMembers; %a++)
	{
		%member = $DefaultMinigame.member[%a];
		if(isObject(%member))
			messageClient(%client, '', "\c3" @ %member.FASTKartsLap @ "   \c6" @ %member.getPlayerName());
	}
}

function FK_DumpPlayerLaps()
{
	for(%a = 0; %a < $DefaultMinigame.numMembers; %a++)
	{
		%member = $DefaultMinigame.member[%a];
		if(isObject(%member))
			echo(%member.FASTKartsLap @ "   " @ %member.getPlayerName());
	}
}