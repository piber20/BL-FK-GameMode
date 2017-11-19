//Title: Achievements
//Author: DarkLight
//Blockland has never had any Achievements, and now it does.

//(this addon was made before steam blockland...)

function sendLockedAchievementToClient(%client, %bitmapImage, %name, %text, %cat)
{
	%name = StripMLControlChars(%name);
	%text = StripMLControlChars(%text);
	
	if(%name $= "")
		return echo("Missing Var! (sendAchievementToClient)");
	
	if(%cat $= "")
		return echo("Missing category! (sendAchievementToClient)");
	
	if(%client.hasAchievementsMod)
		commandtoclient(%client, 'AddAch', %bitmapImage, %name, %text, %cat);
}

function unlockClientAchievement(%client, %name)
{
	if(!isObject(%client))
		return;
	
	%name = StripMLControlChars(%name);
	if(%name $= "")
		return echo("Missing Var! (unlockClientAchievement)");
	
	if(%client.unLockedAchievements[%name])
		return;
	
	if(%client.hasAchievementsMod)
		commandtoclient(%client, 'UnlockAch', %name);
	
	if($Pref::Server::FASTKarts::Achievements)
	{
		if($Pref::Server::FASTKarts::QuietAchievements)
			messageClient(%this, '', "\c3" @ %client.name @ " <bitmap:base/client/ui/ci/star> \c3" @ %name @ " \c6- " @ FK_getAchievementDescription(%name));
		else
			messageAll('', "\c3" @ %client.name @ " <bitmap:base/client/ui/ci/star> \c3" @ %name @ " \c6- " @ FK_getAchievementDescription(%name));
	}
	
	%client.unLockedAchievements[%name] = true;
	
	%file = new FileObject();
	%file.openForAppend("config/server/FASTKarts/achievements/" @ %client.BL_ID @ ".txt");
	%file.writeLine(%name);
	%file.close();
	%file.delete();
}

function loadClientAchievements(%client)
{
	%file = new FileObject();
	%file.openForRead("config/server/FASTKarts/achievements/" @ %client.BL_ID @ ".txt");
	
	while(!%file.isEOF())
	{
		%line = %file.readLine();
		if(%line !$= "")
		{
			%data['Name'] = getField(%line, 0);
			%client.unLockedAchievements[%data['Name']] = true;
			
			if(%client.hasAchievementsMod)
				commandtoclient(%client, 'UnlockAch', %data['Name']);
		}
	}
	%file.close();
	%file.delete();
}

function clearClientAchievements(%client)
{
	if(!%client.hasAchievementsMod)
		return;
	
	commandtoclient(%client, 'clearAch');
}

package AchClientEnterGame
{
	function GameConnection::AutoAdminCheck(%this)
	{
		commandtoClient(%this, 'IHaveAchievementsMod');
		Parent::AutoAdminCheck(%this);
	}
	
	function GameConnection::onClientEnterGame(%this)
	{
		Parent::onClientEnterGame(%this);
		
		if($Pref::Server::FASTKarts::Achievements)
		{
			if(%client.hasAchievementsMod)
				messageClient(%this, '', "\c3This gamemode has achievements that are compatable with your achievement mod's interface.");
		}
		
		clearClientAchievements(%this);
		sendAchievements(%this);
		loadClientAchievements(%this);
	}
};

function serverCmdIHaveAchievementsMod(%client)
{
	%client.hasAchievementsMod = true;
}

activatepackage(AchClientEnterGame);

//exec("./AchPack_1/default_pack.cs");
//exec("./AchPack_1/map_related.cs");
//exec("./AchPack_1/admin_related.cs");