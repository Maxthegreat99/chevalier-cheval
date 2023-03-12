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
	[Export] public Vector2 knockback = new Vector2(50,50);
	[Export] public Vector2 attack = new Vector2(100,150);
	[Export]public playerNormal player = new playerNormal();
	public override void _Ready()
	{
		node = (CharacterBody2D)CallDeferred("get_node","/root/"+GetParent().Name+"/player");
		playerhurtbox = (Area2D)GetNode("playerHurtbox");
		hurtboxshape = (CollisionShape2D)GetNode("playerHurtbox/CollisionShape2D");
		playersprite = (AnimatedSprite2D)GetNode("playerSprite");
		playerCol = (CollisionShape2D)GetNode("playerCollision");
		iframetimer = (Timer)GetNode("iframeTime");

		player.init(node,playerhurtbox,hurtboxshape,playersprite,speed,sprintmultiplier,jump,playerCol,iframetimer,knockback,attack);


	}
	public override void _Process(double delta)
	{
	
		if(!IsOnFloor() && player.playerState != playerstates.ATTACK && !player.falling){
			player.falling = true;
			player.playerSprite.Play("jump");
			
		}
		if(IsOnFloor()){
			player.falling = false;
			player.playerState = playerstates.NONE;
		}
		bool idle = false;
		if(Input.IsActionPressed("ui_left")){
			player.direction = -1;
		}
		else if(Input.IsActionPressed("ui_right")){
			player.direction = 1;
		}
		else
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
		if(player.velocity.Abs().X > 250){
			player.velocity.X = 250 * player.direction;
		}
		
	}
	public override void _PhysicsProcess(double delta)
	{

		Velocity = player.velocity;
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

}


