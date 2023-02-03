using Godot;
using System;

public class playerHitboxes : Area2D
{
	[Export] public CollisionShape2D[] attackColisions = new CollisionShape2D[4];
	[Export] public CollisionShape2D CurrentShape = new CollisionShape2D();
	public override void _Ready(){
		CurrentShape = (CollisionShape2D) GetNode( "currentShape");
		attackColisions[0] = (CollisionShape2D) GetNode("/root/main/player/hitboxList/sprite1Shape");
		attackColisions[1] = (CollisionShape2D) GetNode("/root/main/player/hitboxList/sprite2shape");
		attackColisions[2] = (CollisionShape2D) GetNode("/root/main/player/hitboxList/sprite1ShapeRev");
		attackColisions[3] = (CollisionShape2D) GetNode("/root/main/player/hitboxList/sprite2shapeRev");

	}

}





