//Title: Brick Zone Events
//Author: MeltingPlastic, Crispy
//Dynamic physical zones around bricks. Using zones is now easier.
//Changed around by Crispy for some bigfixing (and probably bug adding!) and made much more user friendly.

$Pref::Server::BrickZoneEvents::TickRate = 100;
$Pref::Server::BrickZoneEvents::DoImpulseVehicles = true;
$Pref::Server::BrickZoneEvents::EnableBricks = true;

exec("./events.cs");
exec("./zones.cs"); 
exec("./bricks.cs"); 
exec("./package.cs");