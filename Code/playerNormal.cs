using System;
using Godot;
public enum playerstates{
    IDLE,
    RUN,
    ATTACK,
    DEATH,
    RUNSPRINT,
    NONE
}
public partial class playerNormal : playerSingle{
    
    public override int Run(double delta)
    {
        float knockbackMultiplier = 1;
        if(playerState != playerstates.RUN && !falling){
            playerState = playerstates.RUN;
            playerSprite.Play("run");
        }
        float acce = acceleration;
        if(falling)
            acce = acceleration/3;
        if(iframeTimer.TimeLeft > 0){
            knockbackMultiplier = 0.1f;
            direction = knockbackDir;
        }
        velocity.X += (((speed * (float)delta * 1000) * direction) * acceleration)* knockbackMultiplier ;     
        return 0;
    }
    public override int Idle(double delta)
    {
        if(playerState != playerstates.IDLE && !falling){
            playerState = playerstates.IDLE;
            playerSprite.Play("default");
        }
        if(velocity.Sign().X != direction && iframeTimer.TimeLeft == 0 || velocity.X == 0 && iframeTimer.TimeLeft == 0){
            velocity.X = 0;   
        }
        else{
            float knockbackMultiplier = 1;
            float fric = friction;
            if(falling)
                fric = friction/3;
            if(iframeTimer.TimeLeft > 0){
                knockbackMultiplier = 0.1f;
                direction = knockbackDir;
            }
            velocity.X -= (((speed * (float)delta * 1000) * direction) * fric) * knockbackMultiplier;
        }
        return 0;
    }
    public override int Jump()
    {
        if(!falling){
            velocity.Y -= jump;
        }
        
        return 0;
    }
    public override int Death()
    {
        if(playerState != playerstates.DEATH){
            playerState = playerstates.DEATH;
            playerSprite.Play("death");
        }
        velocity.Y = 0;
        velocity.X = 0;
        if(playerSprite.Frame == 39){
            return 1;
        }
        return 0;
    }
    public override int changeMode(){
        maxSpeed = 250;
        ((CapsuleShape2D)playerCollision.Shape).Radius= 34;
        ((CapsuleShape2D)playerCollision.Shape).Height = 128;
        playerCollision.RotationDegrees = 0;
        hurtboxShape.Shape = playerCollision.Shape; 
        return 0;  
    }

    

}