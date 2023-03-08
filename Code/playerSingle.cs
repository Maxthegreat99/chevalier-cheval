using System;
using Godot;

public partial class playerSingle : gameActor{
    public Area2D playerHurtbox {get; set;}
    public CollisionShape2D hurtboxShape {get; set;}
    public AnimatedSprite2D playerSprite {get;set;}
    public int speed {get; set;}
    public int sprintSpeedMultiplier {get; set;}
    public int jump {get; set;}
    //public int acceleration?
    public Vector2 velocity = Vector2.Zero;
    public CollisionShape2D playerCollision {get; set;}
    
    public virtual int Run(double delta) {return 0;}
    public virtual int Idle() {return 0;}
    public virtual int Jump() { return 0;}
    public virtual int Attack() { return 0;}
    public virtual int Hurt() { return 0;}
    public virtual int Death() { return 0;}
    public int drainHealth(){return 0;}
    public int refillHealth(){return 0;}
    public int increaseWheatCount(){return 0;}
}