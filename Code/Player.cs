using Godot;
using System;

public partial class Player : CharacterBody2D{
	public CharacterBody2D node = new CharacterBody2D();
	public Area2D playerhurtbox = new Area2D();
	public CollisionShape2D hurtboxshape = new CollisionShape2D();
	public AnimatedSprite2D playersprite = new AnimatedSprite2D();
	[Export] public int speed = 20; 
	[Export] public float sprintmultiplier = 1.5f;
	[Export] public int jump = 250;                       
	public CollisionShape2D playerCol = new CollisionShape2D();
	public Timer iframetimer = new Timer();
	[Export] public Vector2 knockback = new Vector2(300,100);
	[Export] public Vector2 attack = new Vector2(100,150);
	[Export]public playerSingle player = new playerSingle();
	[Export]public playerNormal playerN = new playerNormal();
	[Export]public playerSprint playerS = new playerSprint();
	[Export] public bool isSprint = false;
	public override void _Ready()
	{
		node = (CharacterBody2D)CallDeferred("get_node","/root/"+GetParent().Name+"/player");
		playerhurtbox = (Area2D)GetNode("playerHurtbox");
		hurtboxshape = (CollisionShape2D)GetNode("playerHurtbox/CollisionShape2D");
		playersprite = (AnimatedSprite2D)GetNode("playerSprite");
		playerCol = (CollisionShape2D)GetNode("playerCollision");
		iframetimer = (Timer)GetNode("iframeTime");

		playerN.init(node,playerhurtbox,hurtboxshape,playersprite,speed,sprintmultiplier,jump,playerCol,iframetimer,knockback,attack);
		player.init(node,playerhurtbox,hurtboxshape,playersprite,speed,sprintmultiplier,jump,playerCol,iframetimer,knockback,attack);
		playerS.init(node,playerhurtbox,hurtboxshape,playersprite,speed,sprintmultiplier,jump,playerCol,iframetimer,knockback,attack);

		player = playerN;


	}
	public override void _Process(double delta)
	{
		if(Input.IsActionJustPressed("ui_shift") && IsOnFloor() && player.playerState != playerstates.ATTACK ){
			
			if(isSprint == true){
				playerN.direction = playerS.direction;
				playerN.maxSpeed = playerS.maxSpeed;
				playerN.velocity = playerS.velocity;
				player = playerN;
				isSprint = false;
				player.changeMode();
			}
			else if(isSprint == false){
				playerS.direction = playerN.direction;
				playerS.maxSpeed = playerN.maxSpeed;
				playerS.velocity = playerN.velocity;
				player = playerS;
				isSprint = true;
				player.changeMode();
			}
		}
	
		if(!IsOnFloor() && player.playerState != playerstates.ATTACK && !player.falling){
			player.falling = true;
			if(!isSprint)
				player.playerSprite.Play("jump");
			else
				player.playerSprite.Play("sprintJump");
		}
		if(IsOnFloor()){
			player.falling = false;
			player.playerState = playerstates.NONE;
		}
		bool idle = false;
		if(Input.IsActionPressed("ui_left") && !isSprint){
			player.direction = -1;
		}
		else if(Input.IsActionPressed("ui_right") && !isSprint){
			player.direction = 1;
		}
		else if(!isSprint)
			idle = true;
		
		if(player.direction == 1){
			player.playerSprite.FlipH = false;
		}
		else
			player.playerSprite.FlipH = true;
		player.velocity.Y += 6;
		if(Velocity.Y > 0){
			player.velocity.Y += 6;
		}
		if(idle == true){
			player.Idle(delta);
		}
		else
			player.Run(delta);
	   if(Input.IsActionJustPressed("ui_up") && IsOnFloor() ){
			player.Jump();
		}
		if(player.velocity.Abs().X > player.maxSpeed){
			player.velocity.X = player.maxSpeed * player.direction;
		}
		if(player.iframeTimer.TimeLeft > 0){
			player.iFrameAnimation();
		}
		
	}
	public override void _PhysicsProcess(double delta)
	{

		Velocity = player.velocity;
		GD.Print(player.velocity);
		MoveAndSlide();
		player.velocity = Velocity;
	}
	private void _on_player_hurtbox_area_entered(Area2D area)
	{
		
		if(area.IsInGroup("wheat")){
			player.increaseWheatCount();
			area.QueueFree();                   
		}
		if(area.IsInGroup("enemyHitbox")){
			int Direction = 0;
			if(area.GlobalPosition > GlobalPosition){
				Direction = -1;
			}
			else {
				Direction = 1;
			}
			player.Hurt(Direction);
			player.knockbackDir = Direction;
		}
		
		
	}
    private void _on_iframe_time_timeout()
    {
	    playersprite.Modulate = Color.Color8(255,255,255);
        player.colorChanger = 0;
        player.iframeColorVar = 255;
    }


}


