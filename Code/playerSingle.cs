using System;
using Godot;

public partial class playerSingle : gameActor{
    public Area2D playerHurtbox {get; set;}
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
    public void init(CharacterBody2D node,Area2D playerhurtbox,CollisionShape2D hurtboxshape,
                        AnimatedSprite2D playersprite, int speed,float sprintmultiplier,int jump,
                        CollisionShape2D playerCol,Timer iframetimer,Vector2 knockback,Vector2 attack)
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

    }
    public virtual int Run(double delta) {return 0;}
    public virtual int Idle(double delta) {return 0;}
    public virtual int Jump() { return 0;}
    public virtual int Attack() { return 0;}
    public int Hurt(int Direction) { 
        if(iframeTimer.TimeLeft == 0){
            
            velocity.X += knockback.X * (Direction);
            GD.Print(Direction);
            GD.Print(velocity.X);
            velocity.Y += -knockback.Y;
            iframeTimer.Start();
            drainHealth();
        }
        return 0;
    }
    public int checkHitboxes(){
        
        if(playerHurtbox.GetOverlappingAreas().Count > 0){
            for(int i = 0; i < playerHurtbox.GetOverlappingAreas().Count;i++){
                if(playerHurtbox.GetOverlappingAreas()[i].IsInGroup("wheat")){
                    increaseWheatCount();
                    playerHurtbox.GetOverlappingAreas()[i].QueueFree();                   
                }
                if(playerHurtbox.GetOverlappingAreas()[i].IsInGroup("enemyHitbox")){
                    int Direction = 0;
                    if(((CharacterBody2D)playerHurtbox.GetOverlappingAreas()[i].GetParent()).Position.X > node.Position.X){
                        Direction = 1;
                    }
                    else {
                        Direction = -1;
                    }
                    Hurt(Direction);
                }
            }
        }
        return 0;
    }
    public virtual int Death() { return 0;}
    public int drainHealth(){return 0;}
    public int refillHealth(){return 0;}
    public int increaseWheatCount(){return 0;}
    public int iFrameAnimation() {return 0;}
}