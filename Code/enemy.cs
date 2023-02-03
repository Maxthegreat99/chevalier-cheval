using Godot;
using System;

public class enemy : Area2D
{
	Player player = new Player();
	Area2D hitboxes = new playerHitboxes();
	public override void _Ready()
	{
		player = (Player) GetNode("/root/main/player");
		hitboxes = (Area2D)GetNode("/root/main/player/playerHitboxes");
	}
	public override void _Process(float delta){
		if(OverlapsArea(hitboxes)){
			if(player.isAttack){
				QueueFree();
			}
		}
	}
}
