using Godot;
using System;

public class Player : KinematicBody2D
{
	int acceleration = 35;
	int friction = 20;
	int lookingRight = 1;
	AnimatedSprite playerSprite = new AnimatedSprite();
	Timer jumpTimer = new Timer();
	Timer jumpTime = new Timer();
	[Export] public int speed = 250;
	[Export] public float gravity = 4f;
	[Export] public float AdditionalGravity = 6f;
	[Export] public float jump = 500f;
	[Export] public float jumpRelease = 250f;
	Vector2 velocity;
	public override void _Ready(){
		playerSprite = (AnimatedSprite)GetNode("playerSprite");
		playerSprite.Animation = "default";
		playerSprite.Playing = true;
	
	}
	public void ChangeSprite()
	{
		playerSprite.Scale = new Vector2(1,1);
		int actionPressed = 0;
		/* hanndles movement */
		if (Input.IsActionPressed("ui_right")){
			if(IsOnFloor() || playerSprite.FlipH == false){
				velocity.x = speed;
			}
			else
				velocity.x = speed/2;

			actionPressed = 1;
			lookingRight = 1;
			
		}
		
		else if (Input.IsActionPressed("ui_left")){
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
		if(Input.IsActionPressed("ui_shift")){
			playerSprite.Scale = new Vector2(1,0.5f);
			velocity.x *= 1.5f;
		}
		if(Input.IsActionJustPressed("ui_up") && IsOnFloor() == true )
			actionPressed = 3;
		/* changes sprite */
		switch(actionPressed){
			case(1):
			if(IsOnFloor() == true)
			{
				playerSprite.FlipH = false;
				playerSprite.Animation = "run";
			}
			break;
			case(2):
			if(IsOnFloor() == true){
				playerSprite.FlipH = true;
				playerSprite.Animation = "run";
			}
			break;
			case(3):
			playerSprite.Animation = "jump";
			if(lookingRight == 1)
				playerSprite.FlipH = false;
			if(lookingRight == 0)
				playerSprite.FlipH = true;
			break;
			default:
			if(IsOnFloor() == true){
				playerSprite.Animation = "default";
				if(lookingRight == 1)
					playerSprite.FlipH = false;
				if(lookingRight == 0)
					playerSprite.FlipH = true;
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
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(float delta)
	{

		ChangeSprite();
		float strength = Input.GetActionStrength("ui_left") - Input.GetActionStrength("ui_right");

		if(strength == 0)
			applyFriction();
		else
			applyAcceleration(strength);
		if(Input.IsActionJustPressed("ui_up") && IsOnFloor() == true )
			velocity.y = -jump;
		if(Input.IsActionJustReleased("ui_up") && !IsOnFloor() && velocity.y < -jumpRelease)
			velocity.y = -jumpRelease;
		if(!IsOnFloor() && velocity.y > 0)
			velocity.y += AdditionalGravity;


		velocity.y += gravity;
		
		if(velocity.y > 500 )
			velocity.y = 500;
		velocity = MoveAndSlide(velocity, new Vector2(0, -1));
		
	}

}
