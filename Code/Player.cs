using Godot;
using System;

public enum playerFlags{
  idle = 0,
  run = 1 << 0,
  sprint = 1 << 1,
  sprintJump = 1 << 2,
  attack = 1 << 3,
  jump = 1 << 4,
  fall = 1 << 5,
  landed = 1 << 6,
  MOVE = 1 << 7,
  BACKWARDSLOW = 1 << 8,
  NOTURN = 1 << 9,
  startJump = 1 << 10
  

}
public enum playerState{
  IDLE = playerFlags.idle,
  RUN = playerFlags.run | playerFlags.MOVE,
  SPRINT = playerFlags.sprint | playerFlags.MOVE | playerFlags.NOTURN,
  SPRINTJUMP =  playerFlags.sprintJump | playerFlags.NOTURN | playerFlags.MOVE,
  ATTACK = playerFlags.attack,
  JUMP = playerFlags.jump | playerFlags.BACKWARDSLOW | playerFlags.NOTURN | playerFlags.MOVE,
  FALL = playerFlags.fall | playerFlags.BACKWARDSLOW | playerFlags.NOTURN | playerFlags.MOVE,
  LANDED = playerFlags.landed,
  STARTJUMP = playerFlags.startJump
}
public enum playerBools{
  isInSprint,
  lookingRight,
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
  jumpRelease,
  ANIMATIONS
}
public enum playerAnim{
  ATTACK,
  DEFAULT,
  JUMP,
  SPRINT,
  RUN,
  JUMPEND,
  JUMPSTART,
  SPRINTJUMP,
  SPRINTSTART
}

public class Player : KinematicBody2D{
	[Export] public playerState playerCurrentState = playerState.IDLE;
	public bool[] playerBoolean = {false,true,false};
  public float[] playerVariables = {0f,35f,20f,250f,4f,6f,500f,250f};
  [Export] public playerAnim playersAnimations = playerAnim.DEFAULT;
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
  Vector2 previousPos = new Vector2();
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

	previousPos = Position;
		
	}

  public void changePlayerSprite(){
	  if(playerCurrentState == playerState.IDLE && playersAnimations != playerAnim.DEFAULT ){
		PlayerSprite.Animation = "default";
		playersAnimations = playerAnim.DEFAULT;
	  }
	  else if(playerCurrentState == playerState.RUN && playersAnimations != playerAnim.RUN){
		PlayerSprite.Animation = "run";
		playersAnimations = playerAnim.RUN;
	  }
	else if(playerCurrentState == playerState.FALL && playersAnimations != playerAnim.JUMP){
	  PlayerSprite.Animation = "jump";
	  playersAnimations = playerAnim.JUMP;
	}
	else if(playerCurrentState == playerState.STARTJUMP && playersAnimations != playerAnim.JUMPSTART ){
	  PlayerSprite.Animation = "jumpStart";
	  playersAnimations = playerAnim.JUMPSTART;
	}
	else if(playerCurrentState == playerState.LANDED && playersAnimations != playerAnim.JUMPEND){
	  PlayerSprite.Animation = "jumpEnd";
	  playersAnimations = playerAnim.JUMPEND;
	}
	else if(playerCurrentState == playerState.SPRINT && playerBoolean[(int)playerBools.isInSprint] == false){
	  PlayerSprite.Animation = "sprintStart";
	  playerBoolean[(int)playerBools.isInSprint] = true;
	  playersAnimations = playerAnim.SPRINTSTART;
	}
	else if(playerCurrentState == playerState.SPRINT && playerBoolean[(int)playerBools.isInSprint] == true && playersAnimations != playerAnim.SPRINTSTART){
	  PlayerSprite.Animation = "sprint";
	  playersAnimations = playerAnim.SPRINT;
	}
	else if(playerCurrentState == playerState.SPRINTJUMP && playersAnimations != playerAnim.SPRINTJUMP){
	  PlayerSprite.Animation = "sprintJump";
	  playersAnimations = playerAnim.SPRINTJUMP;
	}
	else if(playerCurrentState == playerState.ATTACK && playersAnimations != playerAnim.ATTACK){
	  PlayerSprite.Animation = "attack";
	  playersAnimations = playerAnim.ATTACK;
	}

  }
  public void handlePlayerState(){
	
  }
	public override void _Process(float delta)
	{
		 
		changePlayerSprite();
	}

}
