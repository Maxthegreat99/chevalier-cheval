
using System;
using Godot;
public partial class farmer : enemy {
	public float walkDistance {get; set;}
	public bool enraged = false;
	public int speed {get; set;}
	public Vector2 velocity = new Vector2();
	public float differenceOriginATK = -23f;
	public enemystate enemyState = enemystate.NONE;
	public Area2D weaponArea = new Area2D();
	public Vector2 attackHitboxesPos = new Vector2();
	public CollisionShape2D weaponCollision = new CollisionShape2D();
	public Vector2 currentPosition = new Vector2();
	public Vector2 lastPosition = new Vector2();
	public CharacterBody2D player = new CharacterBody2D();
	public bool isStop = false;
	public bool stopRight = false;
	public bool stopLeft = false;
	public bool unstopFrame = false;
	private int framesRendered = 0;
	
 	public void initFarmerNode(string NodeName,string SceneName,Area2D detectArea,Area2D attackArea,Area2D hitboxArea,CollisionShape2D WeaponCollision,Vector2 weaponPos,Area2D WeaponArea,CharacterBody2D Player){

		nodeName = NodeName;
		sceneName = SceneName;
		
		detectBox = detectArea;
		attackBox = attackArea;
		hitboxBox = hitboxArea;

		weaponArea = WeaponArea;

		attackHitboxesPos = weaponPos;
		weaponCollision = WeaponCollision;
		weaponCollision.Disabled = true;

		player = Player;

	}
	public override int attack(){
		velocity.X = 0;
		/* set the animation,state and position */
		if(enemyState != enemystate.ATTACK){
			enemyState = enemystate.ATTACK;
			enemySprite.Play("attack");
		}
		if(player.GlobalPosition.X > currentPosition.X)
			playerDir = 1;
		if(player.GlobalPosition.X < currentPosition.X)
			playerDir = -1;
		if(direction != playerDir){
			direction = playerDir;
			enemySprite.Position = Vector2.Zero;
		}
		if(enemySprite.Position.X == 0){
			if(direction > 0)
				enemySprite.Position = new Vector2(enemySprite.Position.X - differenceOriginATK, enemySprite.Position.Y);
			if(direction < 0)
				enemySprite.Position = new Vector2(enemySprite.Position.X + differenceOriginATK, enemySprite.Position.Y);
		}
		if(enemySprite.Frame >= 13 && enemySprite.Frame < 16){
			weaponCollision.Disabled = false;
			if(direction < 0){
				weaponCollision.Position = attackHitboxesPos;
			}
			else{
				weaponCollision.Position = new Vector2(attackHitboxesPos.X * -1,attackHitboxesPos.Y);
			}
		}
		if(enemySprite.Frame == 16){
			weaponCollision.Disabled = true;
		}
		if(enemySprite.Frame == 22){
			enemySprite.Position = new Vector2(0,0);
			return 1;
		}
		return 0;

	}
	public override int death()
	{
		/* set the animation,state and position */
		if(enemyState != enemystate.DIE){
			enemyState = enemystate.DIE;
			enemySprite.Play("death");
			
		}
		if(enemySprite.Frame == 43){
			return 1;
		}
		return 0;
	}
	public override int walkCycle(double delta)
	{
		
		if(enemyState != enemystate.CYCLE){
			enemyState = enemystate.CYCLE;
			enemySprite.Play("walking");
			lastPosition.X = currentPosition.X;
			lastPosition.Y = currentPosition.Y;
		}
		
		
		velocity.X = ((walkDistance * speed) * (float)delta) * direction;

		float length = lastPosition.DistanceTo(currentPosition);
		if(length >walkDistance){
			direction *= -1;
			lastPosition.Y = currentPosition.Y;
			lastPosition.X = currentPosition.X;
			return 1;
			
		}
			
			
		return 0;
		
	}
	public override int idle(int frames){
		if(enemyState != enemystate.IDLE){
			enemyState = enemystate.IDLE;
			enemySprite.Play("idle");
		}
		velocity.X = 0;
		velocity.Y = 0;
		framesRendered += 1;
		if(framesRendered >= frames){
			framesRendered = 0;
			return 1;
		}

		return 0;
	}
	public override void follow(double delta){
		if(player.GlobalPosition.X > currentPosition.X)
			playerDir = 1;
		if(player.GlobalPosition.X < currentPosition.X)
			playerDir = -1;
		
		if(hitboxBox.GetOverlappingAreas().Count > 0 && enemyState != enemystate.IDLE && !unstopFrame){
			for(int i = 0;i < hitboxBox.GetOverlappingAreas().Count;i++){
				if(playerDir == 1){
					if( ((Area2D)hitboxBox.GetOverlappingAreas()[i]).IsInGroup("enemyStop") == true){
						stopRight = true;
						stopLeft = false;
						break;
					}
					else{
						stopRight = false;
						stopLeft = false;
					}
						
				}
				else{
					if( ((Area2D)hitboxBox.GetOverlappingAreas()[i]).IsInGroup("enemyStop") == true){
						stopLeft = true;
						stopRight = false;
						break;
					}
					else{
						stopRight= false;
						stopLeft = false;
					}
				}
			}
		}
		if(stopLeft || stopRight || isStop){
			if(stopRight && playerDir == 1){
				isStop = true;
			}
			else if(stopLeft && playerDir == -1){
				isStop = true;
			}
			else{ 
				isStop = false;
				unstopFrame = true;
			}
		}
		if(isStop){
			if(enemyState != enemystate.IDLE){
				enemyState = enemystate.IDLE;
				enemySprite.Play("idle");
				velocity.X =0;
				velocity.Y =0;
			}
		}
		else{
			if(enemyState != enemystate.FOLLOW){
				enemyState = enemystate.FOLLOW;
				enemySprite.Play("walking");
			}
			velocity.X = (((walkDistance * speed) * (float)delta) * playerDir) * 1.5f;
			direction = playerDir;

		}
		if(unstopFrame){
			for(int i = 0;i < hitboxBox.GetOverlappingAreas().Count;i++){
				if( ((Area2D)hitboxBox.GetOverlappingAreas()[i]).IsInGroup("enemyStop") == true){
					unstopFrame = true;
					break;
				}
				else 
					unstopFrame = false;
			}
		}	
			

	}
	public int detectHitboxes(){

		/* player is in detection box */
		if(detectBox.OverlapsBody(player) && !enraged){
			enraged = true;
			return 2;
		}
		if(attackBox.OverlapsBody(player)){
			return 1;
		}

		return 0;
	}

}
	


	



