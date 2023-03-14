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
	[Export] public int speed = 20;
	public farmer myFarmer;
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
		myFarmer.walkDistance = walkDistance;
		myFarmer.speed = speed;
		myFarmer.currentPosition = GlobalPosition;
		AddCollisionExceptionWith(GetNode("/root/"+sceneName+"/player"));
	}
	public override void _Process(double delta){
		myFarmer.velocity.Y += 4;
		if(myFarmer.velocity.Y > 0 && !IsOnFloor())
			myFarmer.velocity.Y += 6;
		if(myFarmer.velocity.Y > 500)
			myFarmer.velocity.Y = 500;
		myFarmer.currentPosition = GlobalPosition;
		if(myFarmer.direction == 1){
			myFarmer.enemySprite.FlipH = true;
		}
		if(myFarmer.direction == -1){
			myFarmer.enemySprite.FlipH = false;
		}
		executeCommand(delta);
	}
	public override void _PhysicsProcess(double delta)
	{
		Velocity = myFarmer.velocity;
		MoveAndSlide();
		myFarmer.velocity = Velocity;
	}
	public void executeCommand(double delta){
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
					((CollisionShape2D)GetNode("hitboxArea/hitboxShape")).Disabled = true;
					((CollisionShape2D)GetNode("weaponArea/farmerWeaponHitbox")).Disabled = true;
					((AnimatedSprite2D)GetNode("AnimatedSprite2D")).Position = new Vector2(0,0);
					return farmerDeath;
				}
			}
		}
		return null;
	}


}
