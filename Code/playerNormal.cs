using System;
using Godot;
public enum playerstates{
    IDLE,
    RUN,
    ATTACK,
    DEATH,
    NONE
}
public partial class playerNormal : playerSingle{
    public int direction = 1;
    

    public override int Run(double delta)
    {
        if(playerState != playerstates.RUN && !falling){
            playerState = playerstates.RUN;
            playerSprite.Play("run");
        }

         velocity.X += ((speed * (float)delta * 1000) * direction) * acceleration;     
        return 0;
    }
    public override int Idle(double delta)
    {
        if(playerState != playerstates.IDLE && !falling){
            playerState = playerstates.IDLE;
            playerSprite.Play("default");
        }
        if(velocity.Sign().X != direction || velocity.X == 0){
            velocity.X = 0;   
        }
        else
            velocity.X -= ((speed * (float)delta * 1000) * direction) * friction;
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
    public override int Hurt()
    {
        if(iframeTimer.TimeLeft == 0){
            velocity.X = knockback.X * (direction * -1);
            velocity.Y = -knockback.Y;
            iframeTimer.Start();
            drainHealth();
        }
        return 0;
    }
    

}