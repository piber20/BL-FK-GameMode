if($Pref::Server::FASTKarts::ForceSpeedkarts)
	exec("Add-Ons/Gamemode_SpeedKart/karts/speedkart.cs");
else
	exec("./karts/Karts.cs");

exec("./zones/main.cs");
exec("./novelty/novelty.cs");
exec("./pgdie/pgdie.cs");
exec("./achievements.cs");
exec("./lists.cs");
exec("./setmusic.cs");
exec("./bouncyplayer.cs");
exec("./sibamodter/main.cs");
exec("./modifiedmodter/main.cs");