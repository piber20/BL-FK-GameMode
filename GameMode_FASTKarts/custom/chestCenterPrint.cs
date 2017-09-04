package chestCenterPrintPackage
{
	function fxDTSBrick::OnActivate(%obj, %player, %client, %pos, %vec) //added bits so treasure chest messages appear in center print
	{
		Parent::OnActivate(%obj, %player, %client, %pos, %vec);
		%data = %obj.getDataBlock();

		if(%data.getid() == brickTreasureChestData.getId())
		{
			%hash = sha1(%obj.getPosition() @ %obj.angleId);
			if(%client.foundTreasureChest_[%hash])
				centerprint(%client, "<just:right>\c3You already opened this treasure chest (" @ %client.numFoundTreasureChests @ " / " @ $TreasureChest::NumChests @ " found)", 2);
		}
	}
	
	function brickTreasureChestData::openTreasureChest(%data ,%obj, %player) //added bits so treasure chest messages appear in center print
	{
		Parent::openTreasureChest(%data ,%obj, %player);
		%client = %player.client;
		if(!isObject(%client))
			return;
		
		if(%client.numFoundTreasureChests >= $TreasureChest::NumChests)
		{
			if($TreasureChest::NumChests == 1)
			{
				centerprint(%client, "<just:right>\c3You found the treasure chest!", 2);
				%elapsedtime = getSimTime() - %client.treasureVictoryTime;
				if(%elapsedTime > 10000)
				{
					%client.treasureVictoryTime = getSimTime();
					messageAll('', "\c3" @ %client.getPlayerName() @ "\c0 found the treasure chest!");
				}
			}
			else
				centerprint(%client, "<just:right>\c3You found all " @ $TreasureChest::NumChests @ " treasure chests!", 2);
		}
		else
			centerprint(%client, "<just:right>\c3You found " @ %client.numFoundTreasureChests @ " / " @ $TreasureChest::NumChests @ " treasure chests!", 2);
	}

	function serverCmdTreasureStatus(%client) //added bits so treasure chest messages appear in center print
	{
		Parent::erverCmdTreasureStatus(%client);
		if($TreasureChest::NumChests == 1)
		{
			if(%client.numFoundTreasureChests >= 1)
				centerprint(%client, "<just:right>\c3You already found the treasure chest", 2);
			else
				centerprint(%client, "<just:right>\c3You haven't found the treasure chest yet", 2);
		}
		else
		{
			if(%client.numFoundTreasureChests >= $TreasureChest::NumChests)
				centerprint(%client, "<just:right>\c3You found all " @ $TreasureChest::NumChests @ " treasure chests!", 2);   
			else
				centerprint(%client, "<just:right>\c3You found " @ %client.numFoundTreasureChests @ " of " @ $TreasureChest::NumChests @ " treasure chests!", 2);   
		}
	}
};
activatePackage(chestCenterPrintPackage);