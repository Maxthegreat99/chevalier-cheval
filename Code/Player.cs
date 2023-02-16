using Godot;
using System;


public enum playerState{
  IDLE,
  RUN,
  SPRINT,
  SPRINTJUMP,
  ATTACK,
  JUMP,
  FALL,
  LANDED
}
public enum playerBools{
  isInSprint,
  lookingRight,
  isStartJump,
  isInSprintLanded
}
public enum playerVars{
  wheat,
  acceleration,
  friction,
  speed,
  gravity,
  additionalGravity,
  jump,
  jumpRelease
}

public class Player : KinematicBody2D{
	[Export] public playerVars playerCurrentState = IDLE;
	[Export] public bool[] playerBoolean = {false,true,false,false};
	[Export] public float[] playerVariables = {0f,35f,20f,250f,4f,6f,2f};

}
