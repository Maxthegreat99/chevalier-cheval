using Godot;
using System;

public class wheat : Area2D
{
	Player player = new Player();

	public override void _Ready()
	{
		player = (Player) GetNode("/root/main/player");
	}
	public override void _Process(float delta){
		if(OverlapsBody(player)){
			player.increaseWheat();
			QueueFree();
		}
	}
}
