using Godot;
using System;
public enum lifeState{
	HURT = 1,
	HEALED = 2,
	INHURT = -1,
	INHEAL = -2
}
public partial class UI_animHandler : Control
{
	AnimatedSprite2D lifeSprite;
	LifeGUI life;
	Player player;

	
	public int init(AnimatedSprite2D lifeSpriteNode,Player playerNode,LifeGUI lifeNode){
		lifeSprite = lifeSpriteNode;
		life = lifeNode;
		player = playerNode;
		return 0;
	}
	public int hurtHealth(){
		int curretHealth = player.player.currentHealth;
		life.lifeSprites[curretHealth-1].Play("damage");
		return 0;
	}
	public int refillHealth(){
		int currentHealth = player.player.currentHealth;
		life.lifeSprites[currentHealth-1].Play("healing");
		return 0;
	}
	public int setHealthEmpty(int i){
		life.lifeSprites[i].Play("empty");
		return 0;
	}
	public int setHealthFull(int i){
		life.lifeSprites[i].Play("full");
		return 0;
	}
}
