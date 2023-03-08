using System;
using Godot;
public enum playerstates{
    IDLE,
    RUN,
    JUMP,
    ATTACK,
    DEATH
}
public partial class playerNormal : playerSingle{
    public int direction = 1;
    public playerstates playerState;
    public override int Run(double delta)
    {
        if(playerState != playerstates.RUN){
            playerState = playerstates.RUN;
            playerSprite.Play("run");
        }

         velocity.X = (speed * (float)delta) * direction;     
        return 0;
    }
    public override int Idle()
    {
        if(playerState != playerstates.IDLE){
            playerState = playerstates.IDLE;
            playerSprite.Play("default");
        }
        velocity.Y = 0;
        velocity.X = 0;
        return 0;
    }
    
}