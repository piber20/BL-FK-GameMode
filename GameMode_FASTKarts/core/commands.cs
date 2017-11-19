function serverCmdSetRound(%client, %i)
{
	if(!%client.isAdmin)
		return;
	
	if(mFloor(%i) !$= %i)
	{
		messageClient(%client, '', "Usage: /setRound <number>");
		return;
	}
	
	if($Pref::Server::FASTKarts::RoundLimit <= 0)
	{
		messageClient(%client, '', "Track rotation is off. Setting the round will do nothing.");
		return;
	}
	
	if(%i > $Pref::Server::FASTKarts::RoundLimit)
		%i = $Pref::Server::FASTKarts::RoundLimit;
	
	for(%aaa = 0; %aaa < $DefaultMinigame.numMembers; %aaa++)
	{
		%member = $DefaultMinigame.member[%aaa];
		if(isObject(%member))
		{
			commandToClient(%member, 'bottomPrint', "", 0, true);
			%camera = %member.camera;
			%camera.setFlyMode();
			%camera.mode = "Observer";
			%member.setControlObject(%camera);
		}
	}
	
	messageAll('MsgAdminForce', '\c3%1\c2 changed the round to \c3round %2\c2.', %client.getPlayerName(), %i);
	
	SM_StopSong();
	$FK::ResetCount = %i - 1;
	$DefaultMiniGame.scheduleReset();
}

function serverCmdNextRound(%client)
{
	if(!%client.isAdmin)
		return;
	
	messageAll('MsgAdminForce', '\c3%1\c2 skipped the round.', %client.getPlayerName());
	if($FK::ResetCount >= $Pref::Server::FASTKarts::RoundLimit && $Pref::Server::FASTKarts::RoundLimit > 0)
	{
		serverCmdNextTrack(%client);
		return;
	}
	
	for(%aaa = 0; %aaa < $DefaultMinigame.numMembers; %aaa++)
	{
		%member = $DefaultMinigame.member[%aaa];
		if(isObject(%member))
		{
			commandToClient(%member, 'bottomPrint', "", 0, true);
			%camera = %member.camera;
			%camera.setFlyMode();
			%camera.mode = "Observer";
			%member.setControlObject(%camera);
		}
	}
	
	SM_StopSong();
	$DefaultMiniGame.scheduleReset();
}
function serverCmdSkipRound(%client)
{
	serverCmdNextRound(%client);
}

function serverCmdSetTrack(%client, %i, %j, %k, %l, %m, %n, %o)
{
	if(!%client.isAdmin)
		return;
	
	%i = %i SPC %j SPC %k SPC %l SPC %m SPC %n SPC %o;
	%i = trim(%i);
	
	if(mFloor(%i) !$= %i)
		%i = FK_getTrackByName(%i);

	if(%i < 0 || %i > $FK::numTracks)
	{
		messageClient(%client, '', "Invalid track. Usage: /setTrack <number or track name>");
		return;
	}
	
	messageAll('MsgAdminForce', '\c3%1\c2 set the track to \c3%2\c2.', %client.getPlayerName(), FK_getTrackName(%i));
	
	SM_StopSong();
	$FK::CurrentTrack = %i - 1;
	$FK::BypassRandom = true;
	$FK::BypassOrigin = true;
	$FK::BypassType = true;
	FK_NextTrack();
}
function serverCmdSetMap(%client, %i, %j, %k, %l, %m, %n, %o)
{
	serverCmdSetTrack(%client, %i, %j, %k, %l, %m, %n, %o);
}

function serverCmdNextTrack(%client, %i)
{
	if(!%client.isAdmin)
		return;
	
	messageAll( 'MsgAdminForce', '\c3%1\c2 skipped the track', %client.getPlayerName());
	
	SM_StopSong();
	FK_NextTrack();
}
function serverCmdNextMap(%client, %i)
{
	serverCmdNextTrack(%client, %i);
}
function serverCmdSkipTrack(%client, %i)
{
	serverCmdNextTrack(%client, %i);
}
function serverCmdSkipMap(%client, %i)
{
	serverCmdNextTrack(%client, %i);
}

function serverCmdRandomTrack(%client)
{
	if(!%client.isAdmin)
		return;
	
	messageAll('MsgAdminForce', '\c3%1\c2 skipped to a random track.', %client.getPlayerName());
	
	for(%aaa = 0; %aaa < $DefaultMinigame.numMembers; %aaa++)
	{
		%member = $DefaultMinigame.member[%aaa];
		if(isObject(%member))
		{
			commandToClient(%member, 'bottomPrint', "", 0, true);
			%camera = %member.camera;
			%camera.setFlyMode();
			%camera.mode = "Observer";
			%member.setControlObject(%camera);
		}
	}
	
	SM_StopSong();
	$FK::ForceRandom = true;
	schedule(3000, 0, FK_NextTrack);
}

function serverCmdRandomMap(%client)
{
	serverCmdRandomTrack(%client);
}

//TRACK VOTING COMMANDS
function serverCmdVoteTrack(%client, %i, %j, %k, %l, %m, %n, %o)
{
	%i = %i SPC %j SPC %k SPC %l SPC %m SPC %n SPC %o;
	%i = trim(%i);
	
	if(!$Pref::Server::FASTKarts::EnableTrackVoting)
	{
		messageClient(%client, '', "Track voting is disabled.");
		return;
	}
	
	if($FK::VoteInProgress)
	{
		messageClient(%client, '', "There's already a vote in progress. Type /cv to see what it is and /rtv to rock it.");
		return;
	}
	
	if(mFloor(%i) !$= %i)
		%i = FK_getTrackByName(%i);

	if(%i < 0 || %i > $FK::numTracks)
	{
		messageClient(%client, '', "Invalid track. Usage: /vt <number or track name>");
		return;
	}
	
	if(%i == $FK::CurrentTrack)
	{
		messageClient(%client, '', "You're playing in that track right now.");
		return;
	}
	
	if(!FK_trackCanLoad(%i))
	{
		messageClient(%client, '', "You cannot vote for that track.");
		return;
	}
	
	echo(%client.getPlayerName() @ " started a vote to change the track to " @ FK_getTrackName(mfloor(%i)) @ ".");
	messageAll('MsgAdminForce', '\c3%1\c2 voted to set the track to \c3%2\c2.', %client.getPlayerName(), FK_getTrackName(mfloor(%i)));
	messageAll('MsgAdminForce', '\c2Type \c3/rtv\c2 to rock the vote!');
	$FK::VoteInProgress = true;
	$FK::VoteTrack = mfloor(%i);
	%client.FK_isRockingVote = true;
	
	if($DefaultMinigame.numMembers == 1)
	{
		echo("Votes resulted in the track changing next round.");
		messageAll('MsgAdminForce', '\c2Enough people rocked the vote that the track will change \c3next round\c2!');
		$FK::VoteNextRound = true;
	}
}
function serverCmdVT(%client, %i, %j, %k, %l, %m, %n, %o)
{
	serverCmdVoteTrack(%client, %i, %j, %k, %l, %m, %n, %o);
}
function serverCmdVoteMap(%client, %i, %j, %k, %l, %m, %n, %o)
{
	serverCmdVoteTrack(%client, %i, %j, %k, %l, %m, %n, %o);
}
function serverCmdVM(%client, %i, %j, %k, %l, %m, %n, %o)
{
	serverCmdVoteTrack(%client, %i, %j, %k, %l, %m, %n, %o);
}

function serverCmdCurrentVote(%client)
{
	if(!$FK::VoteInProgress)
	{
		messageClient(%client, '', "There's no vote in progress.");
		return;
	}
	
	messageClient(%client, '', "\c6A vote is in progress to set the track to \c3" @ FK_getTrackName($FK::VoteTrack) @ "\c6.");
	
	if(%client.FK_isRockingVote)
		messageClient(%client, '', "\c6You're rocking the vote right now.");
	else
		messageClient(%client, '', "\c6Type \c3/rtv\c6 to rock the vote!");
}

function serverCmdCV(%client)
{
	serverCmdCurrentVote(%client);
}

function serverCmdRockTheVote(%client)
{
	if(!$FK::VoteInProgress)
	{
		messageClient(%client, '', "There's no vote in progress.");
		return;
	}
	
	if($FK::VoteNextRound)
	{
		messageClient(%client, '', "There has already been a successful vote to change the track.");
		%client.FK_isRockingVote = true; //just in case
		FK_CheckVotes();
		return;
	}
	
	if(%client.FK_isRockingVote)
	{
		messageClient(%client, '', "You've already rocked the vote.");
		return;
	}
	
	echo(%client.getPlayerName() @ " rocked the vote.");
	messageAll('MsgAdminForce', '\c3%1\c2 rocked the vote!', %client.getPlayerName());
	%client.FK_isRockingVote = true;
	FK_CheckVotes();
}

function serverCmdRTV(%client)
{
	serverCmdRockTheVote(%client);
}

function FK_CheckVotes()
{
	for(%a = 0; %a < $DefaultMinigame.numMembers; %a++)
	{
		%member = $DefaultMinigame.member[%a];
		if(isObject(%member))
		{
			if(%member.FK_isRockingVote)
				%rockers++;
		}
	}
	
	if($Pref::Server::FASTKarts::PercentVotesNeeded > 100)
		$Pref::Server::FASTKarts::PercentVotesNeeded = 100;
	if($Pref::Server::FASTKarts::PercentVotesNeeded < 0)
		$Pref::Server::FASTKarts::PercentVotesNeeded = 0;
	
	%percent = $Pref::Server::FASTKarts::PercentVotesNeeded * 0.01;
	
	%votesNeeded = $DefaultMinigame.numMembers * %percent;
	if(%rockers >= %votesNeeded && !$FK::VoteNextRound)
	{
		echo("Votes resulted in the track changing next round.");
		messageAll('MsgAdminForce', '\c2Enough people rocked the vote that the track will change \c3next round\c2!');
		$FK::VoteNextRound = true;
		return;
	}
}