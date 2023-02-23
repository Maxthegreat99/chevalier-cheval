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
  IDLE = playerFlags.idle | playerFlags.MOVE,
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
  jumpRelease
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
	public bool[] playerBoolean = {false,false,false};
	public float[] playerVariables = {0f,35f,20f,250f,4f,6f,300f,100f};
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
		else if(playerCurrentState == playerState.FALL && playersAnimations != playerAnim.JUMP || playerCurrentState == playerState.JUMP && playersAnimations != playerAnim.JUMP){
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
		if(((int)playerCurrentState & (int) playerFlags.MOVE ) == (int)playerFlags.MOVE && IsOnFloor() && Input.IsActionPressed("ui_attack") && playerCurrentState != playerState.SPRINT){
			playerCurrentState = playerState.ATTACK;
		}
		else if(((int)playerCurrentState & (int) playerFlags.MOVE ) == (int)playerFlags.MOVE && IsOnFloor() && Input.IsActionJustPressed("ui_up") && playerCurrentState != playerState.SPRINT){
			playerCurrentState = playerState.STARTJUMP;
		}
		else if(playerCurrentState == playerState.STARTJUMP && playersAnimations == playerAnim.JUMPSTART && PlayerSprite.Frame == 5){
			playerCurrentState = playerState.JUMP;
		}
		else if(playerCurrentState == playerState.SPRINT && !IsOnFloor()){
			playerCurrentState = playerState.SPRINTJUMP;
		}
		else if(((int)playerCurrentState & (int) playerFlags.MOVE ) == (int)playerFlags.MOVE && !IsOnFloor() && velocity.y > 0 && playerCurrentState != playerState.SPRINTJUMP && playerCurrentState != playerState.JUMP){
			playerCurrentState = playerState.FALL;
		}
		else if(playerCurrentState == playerState.JUMP && IsOnFloor() || playerCurrentState == playerState.FALL && IsOnFloor() ){
			
			playerCurrentState = playerState.LANDED;
		}
		else if(((int)playerCurrentState & (int) playerFlags.MOVE ) == (int)playerFlags.MOVE && Input.IsActionPressed("ui_shift") && IsOnFloor() ){
			playerCurrentState = playerState.SPRINT;
		}
		else if(((int)playerCurrentState & (int) playerFlags.MOVE ) == (int)playerFlags.MOVE && IsOnFloor() && (Input.IsActionPressed("ui_right") || Input.IsActionPressed("ui_left") )){
			playerCurrentState = playerState.RUN;
		}
		else if(previousPos == Position && IsOnFloor() ){
			playerCurrentState = playerState.IDLE;
		}

	}
	public void handleMovement(){
		if(Input.IsActionPressed("ui_right") && ((int)playerCurrentState & (int) playerFlags.MOVE ) == (int)playerFlags.MOVE){
			if( ((int)playerCurrentState & (int) playerFlags.BACKWARDSLOW ) == (int)playerFlags.BACKWARDSLOW && ((int)playerCurrentState & (int) playerFlags.NOTURN ) == (int)playerFlags.NOTURN ){
				if(playerBoolean[(int)playerBools.lookingRight]==true){
					velocity.x = playerVariables[(int)playerVars.speed];
				}
				else
					velocity.x = playerVariables[(int)playerVars.speed]/2;
				
			}
			else if(((int)playerCurrentState & (int) playerFlags.NOTURN ) == (int)playerFlags.NOTURN){
				if(playerBoolean[(int)playerBools.lookingRight] == true){
					velocity.x = playerVariables[(int)playerVars.speed];
				}
			}
			else{
				velocity.x = playerVariables[(int)playerVars.speed];
				playerBoolean[(int)playerBools.lookingRight] = true;
			}

		}
		else if(Input.IsActionPressed("ui_left") && ((int)playerCurrentState & (int) playerFlags.MOVE ) == (int)playerFlags.MOVE){
			if( ((int)playerCurrentState & (int) playerFlags.BACKWARDSLOW ) == (int)playerFlags.BACKWARDSLOW && ((int)playerCurrentState & (int) playerFlags.NOTURN ) == (int)playerFlags.NOTURN ){
				if(playerBoolean[(int)playerBools.lookingRight]==false){
					velocity.x = -playerVariables[(int)playerVars.speed];
				}
				else
					velocity.x = -playerVariables[(int)playerVars.speed]/2;
				
			}
			else if(((int)playerCurrentState & (int) playerFlags.NOTURN ) == (int)playerFlags.NOTURN){
				if(playerBoolean[(int)playerBools.lookingRight] == false){
					velocity.x = -playerVariables[(int)playerVars.speed];
				}
			}
			else{
				velocity.x = -playerVariables[(int)playerVars.speed];
				playerBoolean[(int)playerBools.lookingRight] = false;
			}

		}
		else {
			velocity.x = 0;
			playerCurrentState = playerState.IDLE;
		}
		if(playerCurrentState == playerState.SPRINT || playerCurrentState == playerState.SPRINTJUMP)
			velocity.x *= 1.5f;
		
		if(playerCurrentState == playerState.ATTACK){
			velocity.x = 0;
			velocity.y = 0;
			if(playerBoolean[(int)playerBools.lookingRight] && playersAnimations != playerAnim.ATTACK){
				PlayerSprite.Position = new Vector2(PlayerSprite.Position.x + differenceOfOriginATTACK, PlayerSprite.Position.y);
			}
			else if(!playerBoolean[(int)playerBools.lookingRight] && playersAnimations != playerAnim.ATTACK){
				PlayerSprite.Position = new Vector2 (PlayerSprite.Position.x - differenceOfOriginATTACK, PlayerSprite.Position.y);
			}
		}
		if(playerCurrentState == playerState.SPRINTJUMP && IsOnFloor()){
			velocity.y = -playerVariables[(int)playerVars.jump]/2;
		}
		if(playerCurrentState == playerState.JUMP && IsOnFloor()){
			velocity.y = -playerVariables[(int)playerVars.jump];
		} 
		if(playerCurrentState == playerState.JUMP && Input.IsActionJustReleased("ui_up") && velocity.y < -playerVariables[(int)playerVars.jumpRelease] )
			velocity.y = -playerVariables[(int)playerVars.jumpRelease];

		if(velocity.y > 0 && !IsOnFloor()){
			velocity.y += playerVariables[(int)playerVars.additionalGravity];
		}

		velocity.y += playerVariables[(int)playerVars.gravity];
		
		if(velocity.y > 500)
			velocity.y = 500;
	}
		public void changePlayerAttackCollision(){
		if(playerCurrentState == playerState.ATTACK && playerBoolean[(int)playerBools.lookingRight]){
			switch(PlayerSprite.Frame){
				case 4:
				case 12:
					hitboxes.CurrentShape.Shape = hitboxes.attackColisions[0].Shape;
					hitboxes.CurrentShape.Position = hitboxes.attackColisions[0].Position;
					break;
				case 5:
				case 6:
				case 7:
				case 8:
				case 9:
				case 10:
				case 11:
					hitboxes.CurrentShape.Shape = hitboxes.attackColisions[1].Shape;
					hitboxes.CurrentShape.Position = hitboxes.attackColisions[1].Position;
					break;
				case 14:
					playerCurrentState = playerState.IDLE;
					PlayerSprite.Animation = "default";
					playersAnimations = playerAnim.DEFAULT;
					if(playerBoolean[(int)playerBools.lookingRight])
						PlayerSprite.Position = new Vector2(PlayerSprite.Position.x - differenceOfOriginATTACK,PlayerSprite.Position.y);
					if(!playerBoolean[(int)playerBools.lookingRight])
						PlayerSprite.Position = new Vector2( PlayerSprite.Position.x + differenceOfOriginATTACK, PlayerSprite.Position.y);
					break;
				default:
					hitboxes.CurrentShape.Position = new Vector2(666,666);
					break;

			}
		}
		else if(playerCurrentState == playerState.ATTACK){
			switch(PlayerSprite.Frame){
				case 4:
				case 12:
					hitboxes.CurrentShape.Shape = hitboxes.attackColisions[2].Shape;
					hitboxes.CurrentShape.Position = hitboxes.attackColisions[2].Position;
					break;
				case 5:
				case 6:
				case 7:
				case 8:
				case 9:
				case 10:
				case 11:
					hitboxes.CurrentShape.Shape = hitboxes.attackColisions[3].Shape;
					hitboxes.CurrentShape.Position = hitboxes.attackColisions[3].Position;
					break;
				case 14:
					playerCurrentState = playerState.IDLE;
					PlayerSprite.Animation = "default";
					playersAnimations = playerAnim.DEFAULT;
					if(playerBoolean[(int)playerBools.lookingRight])
						PlayerSprite.Position = new Vector2(PlayerSprite.Position.x - differenceOfOriginATTACK,PlayerSprite.Position.y);
					if(!playerBoolean[(int)playerBools.lookingRight])
						PlayerSprite.Position = new Vector2( PlayerSprite.Position.x + differenceOfOriginATTACK, PlayerSprite.Position.y);
					
					break;
				default:
					hitboxes.CurrentShape.Position = new Vector2(666,666);
					break;
			}
		}
	}
	public override void _Process(float delta)
	{
		if(playerCurrentState == playerState.SPRINTJUMP && IsOnFloor()){
			if(Input.IsActionPressed("ui_shift")){
				playerCurrentState = playerState.SPRINT;
			}
			else{
				playerCurrentState = playerState.IDLE;
				PlayerCollision.Extents = PlayerCollisionExtentsNormal;
			}
		}
		if(playerCurrentState == playerState.LANDED && playersAnimations == playerAnim.JUMPEND && PlayerSprite.Frame == 5)
			playerCurrentState = playerState.IDLE;
		if(playerCurrentState == playerState.SPRINT && Input.IsActionJustReleased("ui_shift"))
			PlayerCollision.Extents = PlayerCollisionExtentsNormal;
		if(playerBoolean[(int)playerBools.lookingRight] == true && IsOnFloor())
			PlayerSprite.FlipH = false;
		if(playerBoolean[(int)playerBools.lookingRight] == false && IsOnFloor())
			PlayerSprite.FlipH = true;
		
		handlePlayerState();
		if(Input.IsActionJustPressed("ui_shift") && playerCurrentState == playerState.SPRINT)
			PlayerCollision.Extents = PlayerCollisionExtentsSprint;
		handleMovement();
		changePlayerSprite();
		changePlayerAttackCollision();
	}
	public override void _PhysicsProcess(float delta)
	{
		velocity = MoveAndSlide(velocity,new Vector2(0,-1));
	}

}
