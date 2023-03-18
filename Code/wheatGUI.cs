using Godot;
using System;

public partial class wheatGUI : MarginContainer
{
	Label wheatDisplayer = new Label();
	Player player = new Player();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = ((Player)GetNode("/root/"+GetParent().GetParent().GetParent().Name+"/player"));
		wheatDisplayer = (Label)GetNode("wheatAmount");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		wheatDisplayer.Text = player.wheatAmount.ToString();
	}
}
