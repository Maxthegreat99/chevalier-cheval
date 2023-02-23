using Godot;
using System;

public enum enemystate{
	ATTACK,
	FOLLOW,
	CYCLE
}
public class farmer : enemy {
	public int walkDistance {get; set;}
	public int direction { get; set;}
	public int speed {get; set;}
	public Vector2 velocity = new Vector2();
	public int differenceOriginATK = 0;
	public enemystate enemyState = enemystate.CYCLE;
	
	
 	public farmer(Vector2 DetectLengths, Vector2 AttackLengths, Vector2 HitboxLengths, Vector2 GlobalPosition) : 
	base( DetectLengths,AttackLengths,HitboxLengths,GlobalPosition){ }

	public override void attack(){
		velocity.y = 0;
		velocity.x = 0;

		if(enemyState != enemystate.ATTACK)
			globalPosition.x += differenceOriginATK;
		enemyState = enemystate.ATTACK;
		

	}


}
public class enemy : Area2D
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

	public Vector2 globalPosition = new Vector2();
	public enemy(Vector2 DetectLengths, Vector2 AttackLengths, Vector2 HitboxLengths, Vector2 GlobalPosition){
		detectLengths = DetectLengths;
		attackLengths = AttackLengths;
		hitboxLengths = HitboxLengths;

		globalPosition = GlobalPosition;

		
		detectArea = new RectangleShape2D();
		attackArea = new RectangleShape2D();
		hitboxArea = new RectangleShape2D();

		detectArea.Extents = detectLengths;
		attackArea.Extents = attackLengths;
		hitboxArea.Extents = hitboxLengths;

		detectCollision.Shape = detectArea;
		attackCollision.Shape = attackArea;
		hitboxCollision.Shape = hitboxArea;

		detectCollision.Position = attackCollision.Position = hitboxCollision.Position = globalPosition;

		AddChild(detectCollision,true);
		AddChild(attackCollision,true);
		AddChild(hitboxCollision,true);
	
	}

	public virtual void attack() { }
	public virtual void follow() { }
	public virtual void walkCycke() { }
	public virtual void death() { }
}

