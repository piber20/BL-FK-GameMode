//Title: Event_ChooseVehicle
//Author: piber20
//Lets the client choose a vehicle via a vehicle wrench gui.

function fxDTSBrick::chooseVehicle(%this, %vehicle, %client)
{
	%player = %client.player;
	if(!isObject(%player))
		return;
	
	if($FK::RoundType $= "BOUNCY")
	{
		centerPrint(%client, "\c2This is a Bouncy round. No karts allowed!", 7);
		return;
	}
	
	if(FK_getKartsAllowed() < 1)
	{
		$FK::Pref::Karts::AllowSpeedKart = true;
		SpeedKartVehicle.uiName = "SpeedKart";
		%this.setVehicle(FK_getFirstKartAllowed(), %client);
		return;
	}
	else if(FK_getKartsAllowed() == 1)
	{
		%this.setVehicle(FK_getFirstKartAllowed(), %client);
		return;
	}
	
	if(isObject(%player.chooseVehicleSpawnHandler) == false)
	{
		%player.chooseVehicleSpawnHandler = new fxDTSBrick()
		{
			client = %client;
			dataBlock = brickVehicleSpawnData;
			isPlanted = true;
			mountChooseVehicle = %player;
			position = "-10000 -10000 -9000";
		};
	}
	%client.wrenchBrick = %player.chooseVehicleSpawnHandler;
	%client.wrenchBrick.sendWrenchVehicleSpawnData(%client);
	%client.chooseVehicleSpawnBrick = %this;
	
	if(%vehicle == 0) //any
	{
		%client.chooseVehicleMode = 0;
		commandToClient(%client,'setWrenchData',"N _Select_Any_Vehicle");
		commandToClient(%client,'openWrenchVehicleSpawnDlg',"Set Vehicle",1);
	}
	else if(%vehicle == 1) //karts
	{
		%client.chooseVehicleMode = 1;
		commandToClient(%client,'setWrenchData',"N _Select_Any_Kart");
		commandToClient(%client,'openWrenchVehicleSpawnDlg',"Set Kart",1);
	}
}

if(isPackage(ChooseVehicleEventPackage))
	deactivatePackage(ChooseVehicleEventPackage);
package ChooseVehicleEventPackage
{
	function serverCmdSetWrenchData(%client, %data)
	{
		if(isObject(%client.chooseVehicleSpawnBrick))
		{
			if(isObject(%client.wrenchBrick.mountChooseVehicle))
			{
				%vehicle = getWord(getField(%data, 1), 1);
				if(%vehicle $= "0")
					%client.chooseVehicleSpawnBrick.setVehicle(0, %client);
				else if(%client.chooseVehicleMode == 0)
					%client.chooseVehicleSpawnBrick.setVehicle(%vehicle, %client);
				else if(%client.chooseVehicleMode == 1 && getSubStr(%vehicle.uiname, 0, 9) $= "Speedkart")
					%client.chooseVehicleSpawnBrick.setVehicle(%vehicle, %client);
				else if(%client.chooseVehicleMode == 1 && getSubStr(%vehicle.uiname, 0, 9) $= "Superkart")
					%client.chooseVehicleSpawnBrick.setVehicle(%vehicle, %client);
			}
			%client.chooseVehicleSpawnBrick = "";
			%client.chooseVehicleMode = "";
		}
		else
			Parent::serverCmdSetWrenchData(%client, %data);
	}
	
	function Player::delete(%this)
	{
		if(isObject(%this.chooseVehicleSpawnHandler))
			%this.chooseVehicleSpawnHandler.delete();
		Parent::delete(%this);
	}
};
activatePackage(ChooseVehicleEventPackage);

registerOutputEvent("fxDTSBrick", "chooseVehicle", "list ANY 0 Karts 1", 1);