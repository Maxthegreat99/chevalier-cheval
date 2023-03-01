using Godot;
using System;
public class enemy : KinematicBody2D
{

	public AnimatedSprite enemySprite = new AnimatedSprite();
	public Area2D detectBox = new Area2D();
	public Area2D attackBox = new Area2D();
	public Area2D hitboxBox = new Area2D();
	public string nodeName;
	public string sceneName;

	public void initNode(string NodeName,string SceneName,Area2D detectArea,Area2D attackArea,Area2D hitboxArea){


		nodeName = NodeName;
		sceneName = SceneName;

		detectBox = detectArea;
		attackBox = attackArea;
		hitboxBox = hitboxArea;

	
	}

	public virtual int attack() { return 0;}
	public virtual void follow(float delta) { }
	public virtual int walkCycle(float delta) { return 0;}
	public virtual int death() {return 0; }
	public virtual int idle(int frames) {return 0; } 
}

