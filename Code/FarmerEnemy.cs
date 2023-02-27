using Godot;
using System;

public enum enemystate{
	ATTACK,
	FOLLOW,
	CYCLE,
	DIE,
	NONE,
	IDLE
}
public class FarmerEnemy : KinematicBody2D{
	[Export] public int walkDistance = 300;
	[Export] public int speed = 10;
	Vector2 detectionArea = new Vector2(20,20);
	Vector2 attackArea = new Vector2(10,10);
	Vector2 hitboxArea = new Vector2(5,5);
	farmer myFarmer;
	string sceneName;

	public override void _Ready()
	{
		sceneName = GetParent().Name;
		myFarmer = new farmer();
		myFarmer.initFarmerNode(detectionArea,attackArea,hitboxArea,Name,sceneName,(Area2D)GetNode("detectArea"),(Area2D)GetNode("attackArea"),(Area2D)GetNode("hitboxArea"),(CollisionShape2D)GetNode("weaponArea/farmerWeaponHitbox"),((CollisionShape2D)GetNode("weaponArea/farmerWeaponHitbox")).Position,(Area2D)GetNode("weaponArea"),(KinematicBody2D)GetNode("/root/"+sceneName+"/player"));
		myFarmer.enemySprite = (AnimatedSprite)GetNode("AnimatedSprite");
		myFarmer.enemySprite.Playing = true;
		myFarmer.walkDistance = walkDistance;
		myFarmer.speed = speed;
		myFarmer.currentPosition = GlobalPosition;
		AddCollisionExceptionWith(GetNode("/root/"+sceneName+"/player"));
	}
	public override void _Process(float delta){
		myFarmer.velocity.y += 4;
		if(myFarmer.velocity.y > 0 && !IsOnFloor())
			myFarmer.velocity.y += 6;
		if(myFarmer.velocity.y > 500)
			myFarmer.velocity.y = 500;
		myFarmer.currentPosition = GlobalPosition;
	}
	public override void _PhysicsProcess(float delta)
	{
		MoveAndSlide(myFarmer.velocity,new Vector2(0,-1));
	}
}

