using Godot;
using System;

public class playerHitboxes : Area2D
{
	KinematicBody2D player = new KinematicBody2D();

	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(float delta)
	{
	  
	}
}
