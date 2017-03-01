if($FK::Pref::Karts::ForceDefault)
	exec("Add-Ons/Gamemode_SpeedKart/karts/speedkart.cs");
else
	exec("./karts/Karts.cs");

exec("./modter/main.cs");
exec("./zones/main.cs");
exec("./novelty/novelty.cs");
exec("./pgdie/pgdie.cs");
exec("./achievements.cs");
exec("./lists.cs");
exec("./setmusic.cs");
exec("./bouncyplayer.cs");