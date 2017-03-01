package AdminAlertPackage
{
	function serverCmdDropPlayerAtCamera(%client)
	{
		Parent::serverCmdDropPlayerAtCamera(%client);
		
		if(!isObject(%client.player))
			return;
		
		if(%client.isAdmin)
		{
			%elapsedtime = getSimTime() - %client.lastAdminAlertTeleport;
			if(%elapsedTime > 5000)
			{
				%client.lastAdminAlertTeleport = getSimTime();
				messageAll('', "\c3" @ %client.name @ "\c5 just teleported!");
			}
		}
	}
	
	function serverCmdFind(%client, %target)
	{
		Parent::serverCmdFind(%client, %target);
		
		if(!isObject(%client.player) || !isObject(%target.player))
			return;
		
		%targetclient = findClientByName(%target);
		if(%client.isAdmin && isObject(%targetclient))
		{
			%elapsedtime = getSimTime() - %client.lastAdminAlertFind;
			if(%elapsedTime > 5000)
			{
				%client.lastAdminAlertFind = getSimTime();
				messageAll('', "\c3" @ %client.name @ "\c5 just teleported to \c3" @ %targetclient.name @ "\c5!");
			}
		}
	}
	
	function serverCmdFetch(%client, %target)
	{
		Parent::serverCmdFetch(%client, %target);
		
		if(!isObject(%client.player) || !isObject(%target.player))
			return;
		
		%targetclient = findClientByName(%target);
		if(%client.isAdmin && isObject(%targetclient))
		{
			%elapsedtime = getSimTime() - %client.lastAdminAlertFetch;
			if(%elapsedTime > 5000)
			{
				%client.lastAdminAlertFetch = getSimTime();
				messageAll('', "\c3" @ %client.name @ "\c5 just teleported \c3" @ %targetclient.name @ "\c5 to themself!");
			}
		}
	}
	
	function serverCmdWarp(%client)
	{
		Parent::serverCmdWarp(%client);
		
		if(!isObject(%client.player))
			return;

		if(%client.isAdmin)
		{
			%elapsedtime = getSimTime() - %client.lastAdminAlertWarp;
			if(%elapsedTime > 5000)
			{
				%client.lastAdminAlertWarp = getSimTime();
				messageAll('', "\c3" @ %client.name @ "\c5 just warped!");
			}
		}
	}
};
activatePackage(AdminAlertPackage);