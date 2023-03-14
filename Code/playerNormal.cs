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
        hurtboxShape.RotationDegrees = playerCollision.RotationDegrees; 
        return 0;  
    }
    public override int Attack(bool isOnFloor){
        if(playerState != playerstates.ATTACK){
            playerState = playerstates.ATTACK;
            playerSprite.Offset = new Vector2((float)(32 * direction),0);
            playerSprite.Play("attack");
            if(attackBox1.Position.Sign().X != direction) {
                if(direction == 1){
                    attackBox1.Position = new Vector2 (-direction * attackBox1.Position.X,attackBox1.Position.Y);
                }
                else
                    attackBox1.Position = new Vector2 (direction * attackBox1.Position.X,attackBox1.Position.Y);

            }
            if(attackBox2.Position.Sign().X != direction){
                if(direction == 1)
                    attackBox2.Position = new Vector2(-direction * attackBox2.Position.X,attackBox2.Position.Y);
                else
                    attackBox2.Position = new Vector2(direction * attackBox2.Position.X,attackBox2.Position.Y);
            }
            velocity.X += attack.X * direction;
            velocity.Y += -attack.Y;
            maxSpeed = attack.X;
        }
        if(playerSprite.Frame == 4){
            currentBox.Disabled = false;
            currentBox.Shape = attackBox1.Shape;
            currentBox.Position = attackBox1.Position;
            ((CapsuleShape2D)hurtboxShape.Shape).Radius =55;
            ((CapsuleShape2D)hurtboxShape.Shape).Height = 110;
            hurtboxShape.Position = new Vector2(15 * direction,10);
            playerCollision.Shape = hurtboxShape.Shape;
            playerCollision.Position = hurtboxShape.Position;
            playerCollision.RotationDegrees = hurtboxShape.RotationDegrees;
        }
        if(playerSprite.Frame == 5){
            currentBox.Position = attackBox2.Position;
            currentBox.Shape = attackBox2.Shape;
            ((CapsuleShape2D)hurtboxShape.Shape).Radius = 40;
            ((CapsuleShape2D)hurtboxShape.Shape).Height = 138;
            hurtboxShape.RotationDegrees = 90;
            hurtboxShape.Position = new Vector2(30 * direction,-5);
            playerCollision.Shape = hurtboxShape.Shape;
            playerCollision.Position = hurtboxShape.Position;
            playerCollision.RotationDegrees = hurtboxShape.RotationDegrees;
        }
        if(playerSprite.Frame == 7){
            ((CapsuleShape2D)hurtboxShape.Shape).Radius = 48;
            hurtboxShape.Position = new Vector2(30*direction,5);
            playerCollision.Shape = hurtboxShape.Shape;
            playerCollision.Position = hurtboxShape.Position;
            playerCollision.RotationDegrees = hurtboxShape.RotationDegrees;
        }
        if(playerSprite.Frame == 10 && !isOnFloor && hurtboxShape.Position.Y != -5){
            playerSprite.Frame = 8;
        }
        else if(playerSprite.Frame == 10 && isOnFloor){
            currentBox.Position = attackBox2.Position;
            currentBox.Shape = attackBox2.Shape;
            ((CapsuleShape2D)hurtboxShape.Shape).Radius = 40;
            ((CapsuleShape2D)hurtboxShape.Shape).Height = 138;
            hurtboxShape.RotationDegrees = 90;
            hurtboxShape.Position = new Vector2(30 * direction,-5);
            playerCollision.Shape = hurtboxShape.Shape;
            playerCollision.Position = hurtboxShape.Position;
            playerCollision.RotationDegrees = hurtboxShape.RotationDegrees;
        }
        if(playerSprite.Frame == 12){
            currentBox.Shape = attackBox1.Shape;
            currentBox.Position = attackBox1.Position;
            ((CapsuleShape2D)hurtboxShape.Shape).Radius =55;
            ((CapsuleShape2D)hurtboxShape.Shape).Height = 110;
            hurtboxShape.Position = new Vector2(15 * direction,10);
            playerCollision.Shape = hurtboxShape.Shape;
            playerCollision.Position = hurtboxShape.Position;
            playerCollision.RotationDegrees = hurtboxShape.RotationDegrees;
        }
        if(playerSprite.Frame == 13){
            currentBox.Disabled = true;
            ((CapsuleShape2D)hurtboxShape.Shape).Radius =34;
            ((CapsuleShape2D)hurtboxShape.Shape).Height = 128;
            hurtboxShape.RotationDegrees = 0;
            hurtboxShape.Position = new Vector2(0,0);
            playerCollision.Shape = hurtboxShape.Shape;
            playerCollision.Position = hurtboxShape.Position;
            playerCollision.RotationDegrees = hurtboxShape.RotationDegrees;
        }
        if(playerSprite.Frame == 14){
            playerSprite.Offset = new Vector2(0,0);
            maxSpeed = 250;
            return 1;
        }
        return 0;
    }

    

}