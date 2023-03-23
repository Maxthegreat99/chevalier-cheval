using Godot;
using System;

public partial class main : Node2D
{
	public UI_animHandler UI_animator;
	public override void _Ready()
	{
		UI_animator = new UI_animHandler();
		UI_animator.init((AnimatedSprite2D)CallDeferred("get_node","root/Screen/GUI/Life/lifeSprite"),(Player)CallDeferred("get_node","player"),(LifeGUI)CallDeferred("get_node","root/Screen/GUI/Life"));
		LifeGUI lifeUi = (LifeGUI)CallDeferred("get_node","root/Screen/GUI/Life");
		Player player = (Player)CallDeferred("get_node","player");
		wheatGUI wheatUI = (wheatGUI)CallDeferred("get_node","root/GUI/wheat");
		wheatUI.player = player;
		lifeUi.addPlayer(player);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
