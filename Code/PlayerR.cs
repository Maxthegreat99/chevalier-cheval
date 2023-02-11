using Godot;
using System;

public class Player : KinematicBody2D{
    /* player inital position */
    Vector2 intialPosition = new Vector2(-44,556);
    /* player variables */
    int wheatAmount = 0; 
    int acceleration = 35;
    int friction = 20;
    bool lookingRight = true;
    bool isAttack = false;
    bool isSprint = false;
    bool isInSprint = false;
    bool isJump = false;
    int speed = 250;
    float gravity = 4f;
    float AdditionalGravity = 6f;
    public float jump = 500f;
    float jumpRelease = 250f;
	Vector2 velocity;

    /* node varibles */
    playerHitboxes hitboxes = new playerHitboxes();
	RectangleShape2D playerCollision = new RectangleShape2D();
	AnimatedSprite playerSprite = new AnimatedSprite();
    Label GUIwheat = new Label();
    /* other variables */
    float differenceOfOriginATTACK = 25.5f;
    Vector2 PlayerCollisionExtentsSprint = new Vector2(60,26);
    Vector2 PlayerCollisionExtentsNormal = new Vector2(27,52);

    public override void _Ready(){
        /* get required nodes */
		playerSprite = (AnimatedSprite)GetNode("playerSprite");
		hitboxes = (playerHitboxes)GetNode("playerHitboxes");
		GUIwheat = (Label)GetNode("Camera2D/MarginContainer/NinePatchRect/HBoxContainer/wheat");
		playerCollision = (RectangleShape2D)( (CollisionShape2D)GetNode("playerCollision")  ).Shape;
        
        /* set default animation settings */
        playerSprite.Animation = "default";
		playerSprite.Playing = true;
		
	}
    /* changes player sprite depending on the action the player pressed */
    public void changePlayerSprite(int action){
    	if(lookingRight == true && IsOnFloor())
			playerSprite.FlipH = false;

		if(lookingRight == false && IsOnFloor())
			playerSprite.FlipH = true;

		if(IsOnFloor() == false)
			playerSprite.Animation = "jump";

        if(isSprint && Input.IsActionJustPressed("ui_shift")){
            playerSprite.Animation = "sprintStart";
            isInSprint = true;
        }
        else if(isInSprint){
            playerSprite.Animation = "sprint";
            isInSprint = false;
        }
        if(isSprint && Input.IsActionJustReleased("ui_shift")){
            playerSprite.Animation = "sprintStart";
            isSprint = false;
        }
    }
    /* changes the player collision attack depending on the frame playing */
    public void changePlayerAttackCollision(){
        if(isAttack && lookingRightRight){
            switch(playerSprite.Frame){
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
				    playerSprite.Animation = "default";
                    break;
                default:
                    hitboxes.CurrentShape.Position = new Vector2(666,666);
                    break;

            }
        }
        else if(isAttack){
            switch(playerSprite.Frame){
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
				    playerSprite.Animation = "default";
                    break;
                default:
                    hitboxes.CurrentShape.Position = new Vector2(666,666);
                    break;
            }
        }
    }
    /* moves the player depending on actions and returns the action pressed */
    public void movePlayer(int action){

        return action;
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


    }
}