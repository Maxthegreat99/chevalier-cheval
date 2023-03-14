using System;
using Godot;

public partial class playerSingle : gameActor{
    public Area2D playerHurtbox {get; set;}
    public int direction = 1; 
    public CollisionShape2D hurtboxShape {get; set;}
    public AnimatedSprite2D playerSprite {get;set;}
    public int speed {get; set;}
    public float sprintSpeedMultiplier {get; set;}
    public int jump {get; set;}
    public CharacterBody2D node {get;set;}
    public float acceleration = 0.06f;
    public float friction = 0.08f;
    public bool falling = false;
    public Vector2 velocity = Vector2.Zero;
    public CollisionShape2D playerCollision {get; set;}
    public Timer iframeTimer {get; set;}
    public Vector2 knockback {get;set;}
    public Vector2 attack {get;set;}
    public playerstates playerState = playerstates.NONE;
    public int knockbackDir = 0;
    public byte iframeColorVar = 0;
    public int colorChanger = 0;
    public float maxSpeed = 250;
    public CollisionShape2D attackBox1 = new CollisionShape2D();
    public CollisionShape2D attackBox2 = new CollisionShape2D();
    public CollisionShape2D currentBox = new CollisionShape2D();
    public void init(CharacterBody2D node,CollisionShape2D currentBox,CollisionShape2D attackBox2,CollisionShape2D attackBox1,
                    Area2D playerhurtbox,CollisionShape2D hurtboxshape,AnimatedSprite2D playersprite, int speed,float sprintmultiplier,
                    int jump,CollisionShape2D playerCol,Timer iframetimer,Vector2 knockback,Vector2 attack)
    {
        this.node = node;
        playerHurtbox = playerhurtbox;
        hurtboxShape = hurtboxshape;
        playerSprite = playersprite;
        this.speed = speed;
        sprintSpeedMultiplier = sprintmultiplier;
        this.jump = jump;
        playerCollision = playerCol;
        iframeTimer = iframetimer;
        this.knockback = knockback;
        this.attack = attack;
        this.attackBox1 = attackBox1;
        this.currentBox = currentBox;
        this.attackBox2 = attackBox2;

    }
    public virtual int Run(double delta) {return 0;}
    public virtual int Idle(double delta) {return 0;}
    public virtual int Jump() { return 0;}
    public virtual int Attack(bool isOnFloor) { return 0;}
    public int Hurt(int Direction) { 
        if(iframeTimer.TimeLeft == 0){
            
            velocity.X += knockback.X * (Direction);
            velocity.Y = velocity.Y/2 + -knockback.Y;
            iframeTimer.Start();
            drainHealth();
        }
        return 0;
    }

    public virtual int Death() { return 0;}
    public int drainHealth(){return 0;}
    public int refillHealth(){return 0;}
    public int increaseWheatCount(){return 0;}
    public int iFrameAnimation() {
        if(colorChanger == 0){
            iframeColorVar += 7;
        }       
        else{
            iframeColorVar -= 7;
        }
        if(iframeColorVar >= 255){
            colorChanger = 1;
        }
        else if(iframeColorVar <= 0)
        {
            colorChanger = 0;
        }
        playerSprite.Modulate = Color.Color8((byte)playerSprite.Modulate.R8,iframeColorVar,iframeColorVar);
        return 0;
    }
    public virtual int changeMode() {return 0;}
}