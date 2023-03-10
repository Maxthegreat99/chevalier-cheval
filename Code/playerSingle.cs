using System;
using Godot;

public partial class playerSingle : CharacterBody2D{
    public Area2D playerHurtbox {get; set;}
    public virtual int Run() {return 0;}
    public virtual int Idle() {return 0;}
    public virtual int Jump() { return 0;}
    public virtual int Attck() { return 0;}
    public virtual int Death() { return 0;}
}