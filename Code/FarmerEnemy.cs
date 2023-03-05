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
public partial class FarmerEnemy : CharacterBody2D{
	[Export] public int walkDistance = 300;
	[Export] public int speed = 10;
	farmer myFarmer;
	string sceneName;
	EnemyAttackCommand farmerAttack = new EnemyAttackCommand();
	EnemyCycle farmerCycle = new EnemyCycle();
	EnemyDeath farmerDeath = new EnemyDeath();
	EnemyFollow farmerFollow = new EnemyFollow();
	EnemyIdle farmerIdle = new EnemyIdle();
	bool pauseState = false;
	int frames = 58;
	bool becomeIdle = false;
	EnemyCommand command;
	EnemyCommand isDead;
	public override void _Ready()
	{
		sceneName = GetParent().Name;
		myFarmer = new farmer();
		myFarmer.initFarmerNode(Name,sceneName,(Area2D)GetNode("detectArea"),(Area2D)GetNode("attackArea"),(Area2D)GetNode("hitboxArea"),(CollisionShape2D)GetNode("weaponArea/farmerWeaponHitbox"),((CollisionShape2D)GetNode("weaponArea/farmerWeaponHitbox")).Position,(Area2D)GetNode("weaponArea"),(CharacterBody2D)GetNode("/root/"+sceneName+"/player"));
		myFarmer.enemySprite = (AnimatedSprite2D)GetNode("AnimatedSprite2D");
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
		if(myFarmer.direction == 1){
			myFarmer.enemySprite.FlipH = true;
		}
		if(myFarmer.direction == -1){
			myFarmer.enemySprite.FlipH = false;
		}
		executeCommand(delta);
	}
	public override void _PhysicsProcess(float delta)
	{
		MoveAndSlide(myFarmer.velocity,new Vector2(0,-1));
	}
	public void executeCommand(float delta){
		if(isDead == null){
			isDead = IsEnemyDead();
		}
		int returned;
		if(!pauseState){
			command = GetEnemyCommand();
		}
		command.delta = delta;
		command.frames = frames;
		if(isDead != null){
			returned = isDead.execute(myFarmer);
			if(returned == 1){
				QueueFree();
			}
		}
		else{
			returned = command.execute(myFarmer);
			if(myFarmer.enemyState == enemystate.ATTACK && returned == 1){
				pauseState = false;
			}
			if(myFarmer.enemyState == enemystate.CYCLE && returned == 1){
				becomeIdle = true;
			}
			if(myFarmer.enemyState == enemystate.IDLE && returned == 1){
				becomeIdle = false;
			}

		}
		
	}
	public EnemyCommand GetEnemyCommand(){

		int currentHitbox = myFarmer.detectHitboxes();
		if(myFarmer.isStop){
			return farmerFollow;
		}
		if(currentHitbox == 1){
			pauseState = true;
			return farmerAttack;
		}

		if(myFarmer.enraged){
			return farmerFollow;
		}
		if(becomeIdle == true){
			return farmerIdle;
		}
		return farmerCycle;
	}
	public EnemyCommand IsEnemyDead(){
		if(myFarmer.hitboxBox.GetOverlappingAreas().Count > 0){
			for(int i =0;i < myFarmer.hitboxBox.GetOverlappingAreas().Count;i++){
				if(((Area2D)myFarmer.hitboxBox.GetOverlappingAreas()[i]).IsInGroup("playerAttack")){
					pauseState = true;
					return farmerDeath;
				}
			}
		}
		return null;
	}
}

