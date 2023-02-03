using Godot;
using System;

public class Player : KinematicBody2D
{
	[Export] public Vector2 initialPosition;
	/* movement */
	[Export] public int acceleration = 35;
	[Export] public int friction = 20;
	[Export] public int lookingRight = 1;
	[Export] public bool isAttack = false;
	playerHitboxes hitboxes = new playerHitboxes();
	AnimatedSprite playerSprite = new AnimatedSprite();
	[Export] public int speed = 250;
	[Export] public float gravity = 4f;
	[Export] public float AdditionalGravity = 6f;
	[Export] public float jump = 500f;
	[Export] public float jumpRelease = 250f;
	Vector2 velocity;


	public override void _Ready(){
		playerSprite = (AnimatedSprite)GetNode("playerSprite");
		hitboxes = (playerHitboxes)GetNode("playerHitboxes");
		playerSprite.Animation = "default";
		playerSprite.Playing = true;
		initialPosition.x = -44;
		initialPosition.y = 356;
		Position = initialPosition;
	
	}
	public void ChangeSprite()
	{
		playerSprite.Scale = new Vector2(1,1);
		int actionPressed = 0;
		/* hanndles movement */
		if (Input.IsActionPressed("ui_right") && !isAttack){
			if(IsOnFloor() || playerSprite.FlipH == false){
				velocity.x = speed;
			}
			else
				velocity.x = speed/2;

			actionPressed = 1;
			lookingRight = 1;
			
		}
		
		else if (Input.IsActionPressed("ui_left") && !isAttack){
			if(IsOnFloor() || playerSprite.FlipH == true){
				velocity.x = -speed;
			}
			else
				velocity.x = -speed/2;
			actionPressed = 2;
			lookingRight = 0;
		}
		else
			velocity.x = 0;
		if(Input.IsActionPressed("ui_shift") && !isAttack){
			playerSprite.Scale = new Vector2(1,0.5f);
			velocity.x *= 1.5f;
		}
		if(Input.IsActionJustPressed("ui_attack") && !Input.IsActionPressed("ui_shift") && IsOnFloor() && !isAttack){
			actionPressed = 4;
			playerSprite.Animation = "attack";
			velocity.x = 0;
			velocity.y = 0;
		}
		if(Input.IsActionJustPressed("ui_up") && IsOnFloor() == true && !isAttack)
			actionPressed = 3;
		/* changes sprite */
		switch(actionPressed){
			case(1):
			if(IsOnFloor() == true)
				playerSprite.Animation = "run";
			break;
			case(2):
			if(IsOnFloor() == true)
				playerSprite.Animation = "run";
			break;
			case(3):
			playerSprite.Animation = "jump";
			break;
			case(4):
			isAttack = true;
			break;
			default:
			if(IsOnFloor() == true && !isAttack){
				playerSprite.Animation = "default";
			}
			break;
		}


	}
	public void applyFriction(){
		velocity.x = Mathf.MoveToward(velocity.x,0,friction);
	}
	public void applyAcceleration(float strength){
		velocity.x = Mathf.MoveToward(velocity.x,speed * strength,acceleration);
	}
	public override void _Process(float delta){

		if(isAttack == true){
			if(lookingRight == 1 && playerSprite.Frame == 4){
				hitboxes.CurrentShape.Shape = hitboxes.attackColisions[0].Shape;
				hitboxes.CurrentShape.Position = hitboxes.attackColisions[0].Position;
			}
			else if(lookingRight == 1 && playerSprite.Frame == 5){
				hitboxes.CurrentShape.Shape = hitboxes.attackColisions[1].Shape;
				hitboxes.CurrentShape.Position = hitboxes.attackColisions[1].Position;
			}
			else if(lookingRight == 0 && playerSprite.Frame == 4){
				hitboxes.CurrentShape.Shape = hitboxes.attackColisions[2].Shape;
				hitboxes.CurrentShape.Position = hitboxes.attackColisions[2].Position;
			}
			else if(lookingRight == 0 && playerSprite.Frame == 5){
				hitboxes.CurrentShape.Shape = hitboxes.attackColisions[3].Shape;
				hitboxes.CurrentShape.Position = hitboxes.attackColisions[3].Position;
			}
			else if(playerSprite.Frame == 6){
				isAttack = false;
				playerSprite.Animation = "default";
			}
			else{
				Vector2 pos = new Vector2();
				pos.y = 666;
				pos.x = 666;
				hitboxes.CurrentShape.Position = pos;
			}
		}
		if(lookingRight == 1 && IsOnFloor())
			playerSprite.FlipH = false;
		if(lookingRight == 0 && IsOnFloor())
			playerSprite.FlipH = true;
		if(IsOnFloor() == false)
			playerSprite.Animation = "jump";

	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(float delta)
	{
		ChangeSprite();
		float strength = Input.GetActionStrength("ui_left") - Input.GetActionStrength("ui_right");

		if(strength == 0 && !isAttack)
			applyFriction();
		else if(!isAttack) 
			applyAcceleration(strength);
		if(Input.IsActionJustPressed("ui_up") && IsOnFloor() == true && !isAttack)
			velocity.y = -jump;
		if(Input.IsActionJustReleased("ui_up") && !IsOnFloor() && velocity.y < -jumpRelease && !isAttack)
			velocity.y = -jumpRelease;
		if(!IsOnFloor() && velocity.y > 0)
			velocity.y += AdditionalGravity;


		velocity.y += gravity;
		
		if(velocity.y > 500 )
			velocity.y = 500;
		velocity = MoveAndSlide(velocity, new Vector2(0, -1));
		
	}

}
