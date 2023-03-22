using Godot;
using System;

public partial class LifeGUI : MarginContainer
{
	public Player player = new Player();
	public int lifeAmount;
	public AnimatedSprite2D initialSprite = new AnimatedSprite2D();
	public AnimatedSprite2D[] lifeSprites = new AnimatedSprite2D[1]; 
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = (Player)GetNode("/root/"+GetParent().GetParent().GetParent().Name+"/player");
		lifeAmount = ((Player)GetNode("/root/"+GetParent().GetParent().GetParent().Name+"/player")).life;
		Array.Resize(ref lifeSprites,lifeAmount);
		initialSprite = (AnimatedSprite2D)GetNode("lifeSprite");
		addSprites();
		
	}
	public int addSprites(){
		initialSprite.Play("full");
		lifeSprites[0] = initialSprite;
		for(int i = 1; i < lifeAmount;i++){
			lifeSprites[i] = (AnimatedSprite2D)initialSprite.Duplicate();
			lifeSprites[i].Play("full");
			AddChild(lifeSprites[i]);
			lifeSprites[i].AnimationFinished += _on_life_sprite_animation_finished;
			lifeSprites[i].Position=new Vector2(lifeSprites[i-1].Position.X + 100,lifeSprites[i-1].Position.Y);
		}
		return 0;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void _on_life_sprite_animation_finished()
	{
		player.updateHealth();
	}

}



