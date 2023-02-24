using Godot;
using System;

public enum enemystate{
	ATTACK,
	FOLLOW,
	CYCLE,
	DIE,
	NONE
}
public class FarmerEnemy : KinematicBody2D{
	[Export] public int walkDistance = 60;
	[Export] public int speed = 100;
	Vector2 detectionArea = new Vector2(20,20);
	Vector2 attackArea = new Vector2(10,10);
	Vector2 hitboxArea = new Vector2(5,5);
	farmer myFarmer;

	public override void _Ready()
	{
		
		myFarmer = new farmer(detectionArea,attackArea,hitboxArea,Name);
		myFarmer.enemySprite = (AnimatedSprite)GetNode("AnimatedSprite");
		myFarmer.enemySprite.Playing = true;
		myFarmer.walkDistance = walkDistance;
		myFarmer.speed = speed;
	}
	public override void _Process(float delta){
		myFarmer.walkCycle(delta);
		myFarmer.velocity.y += 4;
	}
	public override void _PhysicsProcess(float delta)
	{
		myFarmer.velocity = MoveAndSlide(myFarmer.velocity,new Vector2(0,-1));
	}
}
public class farmer : enemy {
	public float walkDistance {get; set;}
	public int direction { get; set;}
	public int speed {get; set;}
	public Vector2 velocity = new Vector2();
	public float differenceOriginATK = -22f;
	public enemystate enemyState = enemystate.NONE;
	public Vector2 attackHitboxesLength = new Vector2();
	public Vector2 attackHitboxesPos = new Vector2();
	public CollisionShape2D weaponCollision = new CollisionShape2D();
	private float distanceCovered = 0;
 	public farmer(Vector2 DetectLengths, Vector2 AttackLengths, Vector2 HitboxLengths,string nodeName) : 
	base( DetectLengths,AttackLengths,HitboxLengths,nodeName){
		attackHitboxesLength = new Vector2(30,20);
		attackHitboxesPos = new Vector2(-60,2);
		weaponCollision.Disabled = true;
		RectangleShape2D enemyWeaponArea = new RectangleShape2D();
		enemyWeaponArea.Extents = attackHitboxesLength;
		weaponCollision.Shape = enemyWeaponArea;
		AddToGroup("enemyHitbox");
		GetNode("/root/main/"+ nodeName).AddChild(weaponCollision,true); 
	 }

	public override int attack(){
		velocity.y = 0;
		velocity.x = 0;
		/* set the animation,state and position */
		if(enemyState != enemystate.ATTACK){
			if(direction > 0)
				enemySprite.Position = new Vector2(enemySprite.Position.x + differenceOriginATK, enemySprite.Position.y);
			if(direction < 0)
				enemySprite.Position = new Vector2(enemySprite.Position.x - differenceOriginATK, enemySprite.Position.y);
			enemyState = enemystate.ATTACK;
			enemySprite.Animation = "attack";
		}
		if(enemySprite.Frame == 13){
			weaponCollision.Disabled = false;
			if(direction < 0){
				weaponCollision.Position = attackHitboxesPos;
			}
			else{
				weaponCollision.Position = new Vector2(attackHitboxesPos.x * -1,attackHitboxesPos.y);
			}
		}
		if(enemySprite.Frame == 18){
			weaponCollision.Disabled = true;
		}
		if(enemySprite.Frame == 24){
			if(direction < 0)
				enemySprite.Position = new Vector2(enemySprite.Position.x + differenceOriginATK, enemySprite.Position.y);
			if(direction > 0)
				enemySprite.Position = new Vector2(enemySprite.Position.x - differenceOriginATK, enemySprite.Position.y);
			return 1;
		}
		return 0;

	}
	public override int death()
	{
		/* set the animation,state and position */
		if(enemyState != enemystate.DIE){
			enemyState = enemystate.DIE;
			enemySprite.Animation = "death";
		}
		if(enemySprite.Frame == 43){
			return 1;
		}
		return 0;
	}
	public override int walkCycle(float delta)
	{
		
		if(enemyState != enemystate.CYCLE){
			enemyState = enemystate.CYCLE;
			enemySprite.Animation = "walking";
		}
		
		
		velocity.x = walkDistance * delta * direction * speed;
		distanceCovered += walkDistance * delta * speed;

		if(distanceCovered >= walkDistance){
			direction *= -1;
			distanceCovered = 0;
			return 1;
		}
		
		return 0;
		
	}


}
public class enemy : KinematicBody2D
{
	public Vector2 detectLengths = new Vector2();
	public Vector2 attackLengths = new Vector2();
	public Vector2 hitboxLengths = new Vector2();

	public RectangleShape2D detectArea{get; set;}
	public RectangleShape2D attackArea{get; set;}
	public RectangleShape2D hitboxArea{get; set;}

	public CollisionShape2D detectCollision = new CollisionShape2D();
	public CollisionShape2D attackCollision = new CollisionShape2D();
	public CollisionShape2D hitboxCollision = new CollisionShape2D();

	public AnimatedSprite enemySprite = new AnimatedSprite();

	public enemy(Vector2 DetectLengths, Vector2 AttackLengths, Vector2 HitboxLengths,string nodeName){
		detectLengths = DetectLengths;
		attackLengths = AttackLengths;
		hitboxLengths = HitboxLengths;

		
		detectArea = new RectangleShape2D();
		attackArea = new RectangleShape2D();
		hitboxArea = new RectangleShape2D();

		detectArea.Extents = detectLengths;
		attackArea.Extents = attackLengths;
		hitboxArea.Extents = hitboxLengths;

		detectCollision.Shape = detectArea;
		attackCollision.Shape = attackArea;
		hitboxCollision.Shape = hitboxArea;

		detectCollision.Position = attackCollision.Position = hitboxCollision.Position = new Vector2(0,0);

		hitboxCollision.AddToGroup("enemyHitbox");

		hitboxCollision.Name = "hitboxVollision";
		detectCollision.Name = "detectCollision";
		attackCollision.Name = "attackCollision";

		GetNode("/root/main/"+ nodeName).AddChild(detectCollision);
		GetNode("/root/main/"+ nodeName).AddChild(attackCollision);
		GetNode("/root/main/"+ nodeName).AddChild(hitboxCollision);
	
	}

	public virtual int attack() { return 0;}
	public virtual void follow() { }
	public virtual int walkCycle(float delta) { return 0;}
	public virtual int death() {return 0; }
}

