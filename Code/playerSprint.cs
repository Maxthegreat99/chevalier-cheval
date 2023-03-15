using Godot;
using System;

public partial class playerSprint : playerSingle{
    public override int Run(double delta)
    {
        float knockbackMultiplier = 1;
        if(playerState != playerstates.RUNSPRINT && !falling){
            playerState = playerstates.RUNSPRINT;
            playerSprite.Play("sprint");
        }
        float acce = acceleration/5;
        if(falling)
            acce = acceleration/10;
        if(iframeTimer.TimeLeft > 0){
            knockbackMultiplier = 0.1f;
            direction = knockbackDir;
        }
        velocity.X += (((speed * (float)delta * 1000) * direction) * acceleration) * knockbackMultiplier;  
 
        return 0;
    }
        public override int Jump()
    {
        if(!falling){
            velocity.Y -= jump/1.2f;
        }
        
        return 0;
    }
    public override int changeMode(){
        maxSpeed = maxSpeed * sprintSpeedMultiplier;
        ((CapsuleShape2D)playerCollision.Shape).Radius = 32;
        ((CapsuleShape2D)playerCollision.Shape).Height = 158;
        playerCollision.RotationDegrees = 90;
        hurtboxShape.Shape = playerCollision.Shape;
        hurtboxShape.RotationDegrees = playerCollision.RotationDegrees; 
        return 0;
    }
}