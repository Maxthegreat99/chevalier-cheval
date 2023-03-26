using Godot;
using System;

public partial class LifeGUI : MarginContainer
{
	public Player player;
	public int lifeAmount = 0;
	public AnimatedSprite2D initialSprite;
	public AnimatedSprite2D[] lifeSprites = new AnimatedSprite2D[1]; 
	// Called when the node enters the scene tree for the first time.
	
	public void addPlayer(Player playerPassed){
		player = playerPassed;
		lifeAmount = player.life; 
		Array.Resize(ref lifeSprites,lifeAmount);
		initialSprite = (AnimatedSprite2D)CallDeferred("get_node","lifeSprite");
		addSprites();
	}
	public int addSprites(){
		initialSprite.Play("full");
		lifeSprites[0] = initialSprite;
		lifeSprites[0].AnimationFinished += _on_life_sprite_animation_finished;
		for(int i = 1; i < lifeAmount;i++){
			lifeSprites[i] = (AnimatedSprite2D)initialSprite.Duplicate();
			AddChild(lifeSprites[i]);
			lifeSprites[i].Play("full");
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








