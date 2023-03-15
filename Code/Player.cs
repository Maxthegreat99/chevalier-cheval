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
	public CollisionShape2D attackBox1 = new CollisionShape2D();
	public CollisionShape2D attackBox2 = new CollisionShape2D();
	public CollisionShape2D currentBox = new CollisionShape2D();
	[Export] public Vector2 knockback = new Vector2(300,100);
	[Export] public Vector2 attack = new Vector2(500,150);
	[Export]public playerSingle player = new playerSingle();
	[Export]public playerNormal playerN = new playerNormal();
	[Export]public playerSprint playerS = new playerSprint();
	[Export] public bool isSprint = false;
	int returned = 0;
	playerDie playerdie = new playerDie();
	playerIdle playeridle = new playerIdle();
	playerAttack playerattack = new playerAttack();
	playerChangeMode playerchange = new playerChangeMode();
	playerRun playerrun = new playerRun();
	public bool isAttack = false;
	public bool pauseState = false;
	PlayerCommand command;
	public override void _Ready()
	{
		node = (CharacterBody2D)CallDeferred("get_node","/root/"+GetParent().Name+"/player");
		playerhurtbox = (Area2D)GetNode("playerHurtbox");
		hurtboxshape = (CollisionShape2D)GetNode("playerHurtbox/CollisionShape2D");
		playersprite = (AnimatedSprite2D)GetNode("playerSprite");
		playerCol = (CollisionShape2D)GetNode("playerCollision");
		iframetimer = (Timer)GetNode("iframeTime");
		attackBox1 = (CollisionShape2D)GetNode("hitboxList/sprite1Shape");
		attackBox2 = (CollisionShape2D)GetNode("hitboxList/sprite2Shape");
		currentBox = (CollisionShape2D)GetNode("playerHitboxes/currentShape");
		playerN.init(node,currentBox,attackBox2,attackBox1,playerhurtbox,hurtboxshape,playersprite,speed,sprintmultiplier,jump,playerCol,
		iframetimer,knockback,attack);
		player.init(node,currentBox,attackBox2,attackBox1,playerhurtbox,hurtboxshape,playersprite,speed,sprintmultiplier,jump,playerCol,
		iframetimer,knockback,attack);
		playerS.init(node,currentBox,attackBox2,attackBox1,playerhurtbox,hurtboxshape,playersprite,speed,sprintmultiplier,jump,playerCol,
		iframetimer,knockback,attack);

		player = playerN;


	}
	public override void _Process(double delta)
	{
		if(!pauseState){
			if(!isSprint)
				command = GetPlayerCommand();
			else
				command = GerPlayerSprintCommand();
		}
		executeCommand(command,delta);

		if(!IsOnFloor() && !isAttack && !player.falling){
			player.falling = true;
			if(!isSprint)
				player.playerSprite.Play("jump");
			else
				player.playerSprite.Play("sprintJump");
		}
		if(IsOnFloor() && player.falling){
			player.falling = false;
			player.playerState = playerstates.NONE;
		}
		if(player.direction == 1)
			player.playerSprite.FlipH = false;
		
		else
			player.playerSprite.FlipH = true;

		if(!isAttack && IsOnFloor() && Input.IsActionJustPressed("ui_up"))
			player.Jump();
		
		player.velocity.Y += 6;
		if(Velocity.Y > 0)
			player.velocity.Y += 6;
		
		if(player.velocity.Y > 500)
			player.velocity.Y = 500;
		if(player.velocity.Abs().X > player.maxSpeed)
			player.velocity.X = player.maxSpeed * player.direction;
		if(!isAttack && player.playerState != playerstates.IDLE)
			changeSpriteSpeed();
		else
			player.playerSprite.SpeedScale =1;
		if(player.iframeTimer.TimeLeft > 0)
			player.iFrameAnimation();

	}
	public void changeSpriteSpeed(){
		float maxSpeed = player.maxSpeed;
		float currentSpeed = player.velocity.Abs().X;

		float spriteSpeed = maxSpeed/currentSpeed;
		float spriteScale = 1/spriteSpeed;
		if(spriteScale < 0.1f){
			spriteScale = 0.1f;
		}
		player.playerSprite.SpeedScale = spriteScale;
	}
	public void executeCommand(PlayerCommand command,double delta){
		command.delta = delta;
		command.isOnFloor = IsOnFloor();

		returned = command.execute(player);

		if(isAttack && returned == 1){
			isAttack = false;
			pauseState = false;
			player.Idle(delta);
		}
	}
	public PlayerCommand GetPlayerCommand(){
		if(Input.IsActionJustPressed("ui_attack") && IsOnFloor() && !isAttack){
			isAttack = true;
			pauseState = true;
			return playerattack;
		}
		if(Input.IsActionJustPressed("ui_shift") && IsOnFloor() || isAttack){
			playerS.direction = playerN.direction;
			playerS.velocity = playerN.velocity;
			playerS.maxSpeed = playerN.maxSpeed;
			isSprint = true;
			player = playerS;
			return playerchange;
		}
		if(Input.IsActionPressed("ui_left")){
			player.direction = -1;
			return playerrun;
		}
		if(Input.IsActionPressed("ui_right")){
			player.direction = 1;
			return playerrun;
		}

		return playeridle;
		
	}
	public PlayerCommand GerPlayerSprintCommand(){
		if(Input.IsActionJustPressed("ui_shift") && IsOnFloor()){
			playerN.direction = playerS.direction;
			playerN.velocity = playerS.velocity;
			playerN.maxSpeed = playerS.maxSpeed;
			isSprint = false;
			player = playerN;
			return playerchange;
		}
		return playerrun;
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


