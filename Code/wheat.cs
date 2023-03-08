using Godot;
using System;

public partial class wheat : Area2D
{
	Player player = new Player();

	public override void _Ready()
	{
		player = (Player) GetNode("/root/main/player");
	}
	public override void _Process(double delta){
		if(OverlapsBody(player)){
			QueueFree();
		}
	}
}

