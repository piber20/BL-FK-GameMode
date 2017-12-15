datablock ItemData(MegaNukeItemX1 : MegaNukeItem)
{
	uiName = "Rockets x1";
	iconName = "./icon_rockets_x1";
	image = MegaNukeImageX1;
	canDrop = false;
};

datablock ItemData(MegaNukeItemX2 : MegaNukeItemX1)
{
	uiName = "Rockets x2";
	iconName = "./icon_rockets_x2";
	image = MegaNukeImageX2;
};

datablock ItemData(MegaNukeItemX3 : MegaNukeItemX2)
{
	uiName = "Rockets x3";
	iconName = "./icon_rockets_x3";
	image = MegaNukeImageX3;
};

datablock ShapeBaseImageData(MegaNukeImageX1 : MegaNukeImage)
{
	item = MegaNukeItemX1;
};

datablock ShapeBaseImageData(MegaNukeImageX2 : MegaNukeImageX1)
{
	item = MegaNukeItemX2;
};

datablock ShapeBaseImageData(MegaNukeImageX3 : MegaNukeImageX2)
{
	item = MegaNukeItemX3;
};

function MegaNukeImageX1::OnFire(%this, %obj, %slot)
{
	if((%obj.lastRocketShotTime + %this.minShotTime) > getSimTime())
		return;
		
	%obj.lastRocketShotTime = getSimTime();
	Parent::OnFire(%this, %obj, %slot);
	%obj.tool[%obj.currTool] = 0;
	%obj.weaponCount--;
	messageClient(%obj.client,'MsgItemPickup','',%obj.currTool,0);
	serverCmdUnUseTool(%obj.client);
}

function MegaNukeImageX2::OnFire(%this, %obj, %slot)
{
	if((%obj.lastRocketShotTime + %this.minShotTime) > getSimTime())
		return;
		
	%obj.lastRocketShotTime = getSimTime();
	Parent::OnFire(%this, %obj, %slot);
	%obj.tool[%obj.currTool] = MegaNukeItemX1.getID();
	messageClient(%obj.client, 'MsgItemPickup', '', %obj.currTool, MegaNukeItemX1.getID());
}

function MegaNukeImageX3::OnFire(%this, %obj, %slot)
{
	if((%obj.lastRocketShotTime + %this.minShotTime) > getSimTime())
		return;
		
	%obj.lastRocketShotTime = getSimTime();
	Parent::OnFire(%this, %obj, %slot);
	%obj.tool[%obj.currTool] = MegaNukeItemX2.getID();
	messageClient(%obj.client, 'MsgItemPickup', '', %obj.currTool, MegaNukeItemX2.getID());
}