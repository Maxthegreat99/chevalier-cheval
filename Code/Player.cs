using Godot;
using System;


public enum playerState{
  IDLE,
  RUN,
  SPRINT,
  SPRINTJUMP,
  ATTACK,
  JUMP,
  FALL,
  LANDED
}
public enum playerBools{
  isInSprint,
  lookingRight,
  isStartJump,
  isInSprintLanded
}
public enum playerVars{
  wheat,
  acceleration,
  friction,
  speed,
  gravity,
  additionalGravity,
  jump,
  jumpRelease
}

public class Player : KinematicBody2D{
	[Export] public playerState playerCurrentState = playerState.IDLE;
	[Export] public bool[] playerBoolean = {false,true,false,false};
	[Export] public float[] playerVariables = {0f,35f,20f,250f,4f,6f,500f,250f};
  Vector2 initPos = new Vector2(0,0);
  Vector2 velocity = new Vector2(0,0);
  
	playerHitboxes hitboxes = new playerHitboxes();
	RectangleShape2D PlayerCollision = new RectangleShape2D();
	AnimatedSprite PlayerSprite = new AnimatedSprite();
	Label GUIwheat = new Label();
	/* other variables */
	float differenceOfOriginATTACK = 32f;
	Vector2 PlayerCollisionExtentsSprint = new Vector2(74,32);
	Vector2 PlayerCollisionExtentsNormal = new Vector2(34,64);

  	public override void _Ready(){
		/* get required nodes */
		PlayerSprite = (AnimatedSprite)GetNode("playerSprite");
		hitboxes = (playerHitboxes)GetNode("playerHitboxes");
		GUIwheat = (Label)GetNode("Camera2D/MarginContainer/NinePatchRect/HBoxContainer/wheat");
		PlayerCollision = (RectangleShape2D)( (CollisionShape2D)GetNode("playerCollision")  ).Shape;
		
		/* set default animation settings */
		Position = initPos;
		PlayerSprite.Animation = "default";
		PlayerSprite.Playing = true;
		
	}

}
