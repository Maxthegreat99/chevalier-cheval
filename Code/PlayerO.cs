using Godot;
using System;

public class asus : KinematicBody2D{
	/* Player inital position */
	Vector2 intialPosition = new Vector2(0,0);
	/* Player variables */
	int wheatAmount = 0; 
	int acceleration = 35;
	int friction = 20;
	bool lookingRight = true;
	[Export] public bool isAttack = false;
	bool isSprint = false;
	bool isInSprint = false;
	bool isInSprintJump = false;
	bool sprintLanded = false;
	bool isLanded = false;
	bool isJump = false;
	bool isFall = false;
	bool startJump = false;
	int speed = 250;
	float gravity = 4f;
	float additionalGravity = 6f;
	public float jump = 500f;
	float jumpRelease = 250f;
	int a = 0;
	int b = 0;
	int c = 0;
	int d = 0;

	Vector2 velocity = new Vector2(0,0);

	/* node varibles */
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
		Position = intialPosition;
		PlayerSprite.Animation = "default";
		PlayerSprite.Playing = true;
		
		
	}
	/* changes Player sprite depending on the action the Player pressed */
	public void changePlayerSprite(){
		if(velocity.x == 0 && IsOnFloor() && !startJump && !isLanded && !isInSprint && !isAttack){
			PlayerSprite.Animation = "default";
		}
		else if( IsOnFloor() && velocity.x < 0 || velocity.x > 0 && IsOnFloor())
			PlayerSprite.Animation = "run";

		else if(IsOnFloor() == false && !isSprint && !isFall){
			PlayerSprite.Animation = "jump";
			isFall = true;
		}
		if(IsOnFloor() && isFall)
			isLanded = true;
		
	
		if(startJump == true && a == 0){
			PlayerSprite.Animation = "jumpStart";
			a = 1;
		}
		else if(startJump && PlayerSprite.Frame == 5){
			PlayerSprite.Animation = "jump";
			startJump = false;
			isJump = true;
			a = 0;
		}
		else if(isLanded == true && b == 0){
			b = 1;
			PlayerSprite.Animation = "jumpEnd";
		}
		else if(PlayerSprite.Frame == 5 && isLanded == true){
			b = 0;
			isLanded = false;
			PlayerSprite.Animation = "default";
		}

		if(isSprint && Input.IsActionJustPressed("ui_shift")){
			PlayerSprite.Animation = "sprintStart";
			isInSprint = true;
			PlayerCollision.Extents = PlayerCollisionExtentsSprint;;
		}
		else if(isInSprint){
			PlayerSprite.Animation = "sprint";
			isInSprint = false;
		}
		else if(isSprint && Input.IsActionJustReleased("ui_shift")){
			isSprint = false;
			PlayerCollision.Extents = PlayerCollisionExtentsNormal;
		}
		else if(isInSprintJump && d == 0){
			PlayerSprite.Animation = "sprintJump";
			d = 1;
		}
		else if(sprintLanded == true){
			d = 0;
			sprintLanded = false;
			PlayerSprite.Animation = "sprint";
		}
		
		if(isAttack && c == 0){
			c = 1;
			PlayerSprite.Animation = "attack";
		}

		if(lookingRight == true && IsOnFloor())
			PlayerSprite.FlipH = false;

		if(lookingRight == false && IsOnFloor())
			PlayerSprite.FlipH = true;

	}
	/* controls Player movement */
	public void handleMovement(){
		if(Input.IsActionPressed("ui_right") && !isAttack && !startJump && !isLanded  || (isSprint && lookingRight) && Input.IsActionPressed("ui_right") ){
			if(IsOnFloor() || PlayerSprite.FlipH == false){
				velocity.x = speed;
			}
			else 
				velocity.x = speed/2;

			lookingRight = true;
		}
		else if(Input.IsActionPressed("ui_left") && !isAttack  && !startJump && !isLanded || (isSprint && !lookingRight) && Input.IsActionPressed("ui_left")){
			if(IsOnFloor() || PlayerSprite.FlipH == true){
				velocity.x = -speed;
			}
			else    
				velocity.x = -speed/2;
			lookingRight = false;
		}
		else 
			velocity.x = 0;

		bool isOnGroundOrSprint = (IsOnFloor() || isInSprintJump);
		if(Input.IsActionPressed("ui_shift") && isOnGroundOrSprint && !isAttack && (velocity.x > 0 || velocity.x < 0) ){
			velocity.x *= 1.5f;
			isSprint = true;
		}
		else
			isSprint = false;
		
		if(Input.IsActionJustPressed("ui_attack") && !isSprint && IsOnFloor() && !isAttack){
			isAttack = true;
			velocity.x = 0;
			velocity.y = 0;
			if(lookingRight)
				PlayerSprite.Position = new Vector2(PlayerSprite.Position.x + differenceOfOriginATTACK, PlayerSprite.Position.y);
			if(!lookingRight)
				PlayerSprite.Position = new Vector2 (PlayerSprite.Position.x - differenceOfOriginATTACK, PlayerSprite.Position.y);
		}


		if(Input.IsActionJustPressed("ui_up") && IsOnFloor() && isSprint){
			isInSprintJump = true;
			velocity.y = -jump/2;
		}
		if(Input.IsActionJustPressed("ui_up") && IsOnFloor() == true && !isAttack)
			startJump = true;
		if(isJump == true)
			velocity.y = -jump;
		if(isJump && Input.IsActionJustReleased("ui_up") && !IsOnFloor() && velocity.y < -jumpRelease && !isAttack)
			velocity.y = -jumpRelease;
		if(IsOnFloor() && isInSprintJump){
			sprintLanded = true;
			isInSprintJump = false;
		}
		if(IsOnFloor() && isJump)
			isLanded = true;

		if(velocity.y > 0 && !IsOnFloor())
			velocity.y += additionalGravity;
		
		velocity.y += gravity;
		if(velocity.y > 500)
			velocity.y = 500;

	}

	/* changes the Player collision attack depending on the frame playing */
	public void changePlayerAttackCollision(){
		if(isAttack && lookingRight){
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
					isAttack = false;
					PlayerSprite.Animation = "default";
					break;
				default:
					hitboxes.CurrentShape.Position = new Vector2(666,666);
					break;

			}
		}
		else if(isAttack){
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
					isAttack = false;
					PlayerSprite.Animation = "default";
					if(lookingRight)
						PlayerSprite.Position = new Vector2(PlayerSprite.Position.x - differenceOfOriginATTACK,PlayerSprite.Position.y);
					if(!lookingRight)
						PlayerSprite.Position = new Vector2( PlayerSprite.Position.x + differenceOfOriginATTACK, PlayerSprite.Position.y);
					c = 0;
					break;
				default:
					hitboxes.CurrentShape.Position = new Vector2(666,666);
					break;
			}
		}
	}

	public void applyFriction(){
		velocity.x = Mathf.MoveToward(velocity.x,0,friction);
	}
	public void applyAcceleration(float strength){
		velocity.x = Mathf.MoveToward(velocity.x,speed * strength,acceleration);
	}
	public void increaseWheat(){
		wheatAmount++;
		GUIwheat.Text = wheatAmount.ToString();
	}
	public override void _Process(float delta){
		handleMovement();
		changePlayerSprite();
		changePlayerAttackCollision();

	}
	public override void _PhysicsProcess(float delta){
		float strength = Input.GetActionStrength("ui_left") - Input.GetActionStrength("ui_right");

		if(strength == 0 && !isAttack)
			applyFriction();
			
		else if(!isAttack) 
			applyAcceleration(strength);
		velocity = MoveAndSlide(velocity, new Vector2(0,-1));
	}
}
