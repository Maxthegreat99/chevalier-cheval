using Godot;
using System;

public partial class wheatGUI : MarginContainer
{
	Label wheatDisplayer = new Label();
	public Player player = new Player();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		wheatDisplayer = (Label)CallDeferred("get_node","wheatAmount");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(player != null)
			wheatDisplayer.Text = player.wheatAmount.ToString();
	}
}
