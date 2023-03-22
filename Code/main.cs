using Godot;
using System;

public partial class main : Node2D
{
	public UI_animHandler UI_animator = new UI_animHandler();
	public override void _Ready()
	{
		UI_animator.init((AnimatedSprite2D)GetNode("root/Screen/GUI/Life/lifeSprite"),(Player)GetNode("player"),(LifeGUI)GetNode("root/Screen/GUI/Life"));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
