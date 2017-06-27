function FK_getAchievementDescription(%achievement)
{
	%description = "\c0ERROR: Got a description for an achievement that has none, or the achievement doesn't exist.";
	
	if(%achievement $= "Hugs Make Things Better")
		%description = "Use the hug command.";
	if(%achievement $= "I Think it Burns")
		%description = "Jump in to a pool of water.";
	if(%achievement $= "Fallure")
		%description = "Fall ten times and die.";
	if(%achievement $= "Where am I")
		%description = "Die far away from the spawn.";
	
	if(%achievement $= "Self Help")
		%description = "Use the help command.";
	if(%achievement $= "Cheater")
		%description = "Try to teleport.";
	
	if(%achievement $= "Treasure")
		%description = "Find a treasure chest.";
	if(%achievement $= "Treasure Hunter")
		%description = "Find all treasure chests in a map.";
	if(%achievement $= "Road Kill")
		%description = "Kill a player by running them over with a vehicle.";
	if(%achievement $= "Getting Started")
		%description = "Complete your first lap.";
	if(%achievement $= "Winner")
		%description = "Win your first race.";
	
	return %description;
}

function GameConnection::FK_getAchievementListString(%this, %achievement)
{
	if(%this.unLockedAchievements[%achievement])
		messageClient(%this, '', "<bitmap:base/client/ui/ci/star> \c3" @ %achievement @ " \c6- " @ FK_getAchievementDescription(%achievement));
	else
		messageClient(%this, '', "    \c7" @ %achievement @ " - " @ FK_getAchievementDescription(%achievement));
}

function serverCmdAchievements(%client)
{
	if(!$Pref::Server::FASTKarts::Achievements)
		return;
	
	messageClient(%this, '', "\c6All the achievements in the gamemode:");
	%client.FK_getAchievementListString("Hugs Make Things Better");
	%client.FK_getAchievementListString("I Think it Burns");
	%client.FK_getAchievementListString("Fallure");
	%client.FK_getAchievementListString("Where am I");
	
	%client.FK_getAchievementListString("Self Help");
	%client.FK_getAchievementListString("Cheater");
	
	%client.FK_getAchievementListString("Treasure");
	%client.FK_getAchievementListString("Treasure Hunter");
	%client.FK_getAchievementListString("Road Kill");
	%client.FK_getAchievementListString("Getting Started");
	%client.FK_getAchievementListString("Winner");
}

function sendAchievements(%client) //overwriting the original add-on, we're taking over!
{
	//the achievements gui is glitched where the scroll bar wont allow players to scroll beyond what's in the general group
	//we're going to set all of them to be in the general category and if a workaround is found later on we can easily just change these
	//and no, releasing a fixed version of the mod will NOT work as there will still be people with the older version, and not many people will have the mod in the first place
	%general = "General";
	%deathmatch = "General";
	%special = "General";
	
	//Edited Originals
	sendLockedAchievementToClient(%client, "hugger",	"Hugs Make Things Better",	FK_getAchievementDescription("Hugs Make Things Better"),	%general);
	sendLockedAchievementToClient(%client, "censored",	"I Think it Burns",			FK_getAchievementDescription("I Think it Burns"),			%deathmatch);
	sendLockedAchievementToClient(%client, "fail",		"Fallure",					FK_getAchievementDescription("Fallure"),					%deathmatch);
	sendLockedAchievementToClient(%client, "unknown",	"Where am I",				FK_getAchievementDescription("Where am I"),					%deathmatch);
	
	//Edited Steam Ports
	sendLockedAchievementToClient(%client, "pa",		"Self Help",				FK_getAchievementDescription("Self Help"),					%general);
	sendLockedAchievementToClient(%client, "pacman",	"Cheater",					FK_getAchievementDescription("Cheater"),					%special);
	
	//Added
	sendLockedAchievementToClient(%client, "moneysign",	"Treasure",					FK_getAchievementDescription("Treasure"),					%general);
	sendLockedAchievementToClient(%client, "lego",		"Treasure Hunter",			FK_getAchievementDescription("Treasure Hunter"),			%general);
	sendLockedAchievementToClient(%client, "unknown",	"Road Kill",				FK_getAchievementDescription("Road Kill"),					%deathmatch);
	sendLockedAchievementToClient(%client, "number1",	"Getting Started",			FK_getAchievementDescription("Getting Started"),			%general);
	sendLockedAchievementToClient(%client, "number1",	"Winner",					FK_getAchievementDescription("Winner"),						%general);
}

if(isPackage(FKAchievementsPackage))
	deactivatePackage(FKAchievementsPackage);
package FKAchievementsPackage
{
	function GameConnection::onDeath(%this, %killerPlayer, %killer, %damageType, %damageLoc)
	{
		%player = %this.player;
		if(%player.getType() & $TypeMasks::PlayerObjectType)
		{
			%playerPosition = %player.getPosition();
			
			if(%killer != %this && isObject(%killer))
			{
				if(%damageType == $DamageType::Vehicle)
					unlockClientAchievement(%killer, "Road Kill");
			}
			
			if(%damageType == $DamageType::Fall) 
			{
				%this.lavaDeaths++;
				unlockClientAchievement(%this, "I Think it Burns");
				
				if(%this.lavaDeaths >= 10)
					unlockClientAchievement(%this, "Fallure");
			}
	
			if(getWord(%playerPosition, 0) >= 1000
			|| getWord(%playerPosition, 1) >= 1000
			|| getWord(%playerPosition, 2) >= 1000
			|| getWord(%playerPosition, 0) <= -1000
			|| getWord(%playerPosition, 1) <= -1000
			|| getWord(%playerPosition, 2) <= -1000)
				unlockClientAchievement(%this, "Where am I");
		}
		
		parent::onDeath(%this, %killerPlayer, %killer, %damageType, %damageLoc);
	}
	
	function serverCmdHug(%client)
	{
		parent::serverCmdHug(%client);
		
		if(isObject(%client.player))
			unlockClientAchievement(%client, "Hugs Make Things Better");
	}
	
	function serverCmdHelp(%client)
	{
		parent::serverCmdHelp(%client);
		unlockClientAchievement(%client, "Self Help");
	}
	
	function serverCmdDropPlayerAtCamera(%client)
	{
		Parent::serverCmdDropPlayerAtCamera(%client);
		unlockClientAchievement(%client, "Cheater");
	}
	
	function serverCmdFind(%client, %target)
	{
		Parent::serverCmdFind(%client, %target);
		unlockClientAchievement(%client, "Cheater");
	}
	
	function serverCmdFetch(%client, %target)
	{
		Parent::serverCmdFetch(%client, %target);
		unlockClientAchievement(%client, "Cheater");
	}
	
	function serverCmdWarp(%client)
	{
		Parent::serverCmdWarp(%client);
		unlockClientAchievement(%client, "Cheater");
	}
	
	function brickTreasureChestData::openTreasureChest(%data ,%obj, %player)
	{
		Parent::openTreasureChest(%data ,%obj, %player);
		%client = %player.client;
		if(!isObject(%client))
			return;
		
		if(%client.numFoundTreasureChests >= $TreasureChest::NumChests)
			unlockClientAchievement(%client, "Treasure Hunter");
		else
			unlockClientAchievement(%client, "Treasure");
	}
	
	function GameConnection::winRace(%this, %laps)
	{
		Parent::winRace(%this, %laps);
		if(%this.FASTKartsLap != 1) //if they did not just complete lap 0
			unlockClientAchievement(%this, "Getting Started");
		if(%this.FASTKartsLap > %laps)
		{
			unlockClientAchievement(%this, "Winner");
			
			//do steam achievements
			if(%this.isLocal()) //if this is a local client, give shitty achievement
				steamGetAchievement("ACH_SPEEDKART_PROBABLY_CHEATED", "speedKartsAchievement");
			else
			{
				//remote client, send them an achievement message
				if(clientGroup.getCount() > 1)
					commandToClient(%this, 'GetAchievement', "ACH_SPEEDKART_WIN");
			}
		}
	}
};
activatePackage(FKAchievementsPackage);