using System;
using Godot;

public class Command{
    public Command() {}
    public virtual void execute() {} 
}
public partial class EnemyCommand : Command{

    public double delta {get; set; }
    public int frames {get; set;}
    public virtual int execute(enemy Enemy) {return 0; }
}
public partial class EnemyAttackCommand : EnemyCommand{
    public override int execute(enemy Enemy)
    {
        return Enemy.attack();
    }
}
public partial class PlayerCommand : Command{

    public double delta {get; set;}
    public bool isOnFloor {get;set;}

    public virtual int execute( playerSingle player){return 0;}
}
public partial class playerRun : PlayerCommand{
    public override int execute (playerSingle player ){
        return player.Run(delta);
    }
}
public partial class playerIdle : PlayerCommand{
    public override int execute(playerSingle player){
        return player.Idle(delta);
    }
}
public partial class playerAttack : PlayerCommand{
    public override int execute(playerSingle player)
    {
        return player.Attack(isOnFloor);
    }
}
public partial class playerDie : PlayerCommand{
    public override int execute(playerSingle player)
    {
        return player.Death();
    }
}
public partial class playerChangeMode : PlayerCommand{
    public override int execute(playerSingle player)
    {
        return player.changeMode(delta);
    }
}
public partial class EnemyFollow : EnemyCommand{
    public override int execute(enemy Enemy)
    {
        Enemy.follow(delta);
        return 0;
    }
}
public partial class EnemyCycle : EnemyCommand{
    public override int execute(enemy Enemy)
    {
        return Enemy.walkCycle(delta);
    }
}
public partial class EnemyDeath : EnemyCommand{
    public override int execute(enemy Enemy)
    {
        return Enemy.death();
    }
}
public partial class EnemyIdle : EnemyCommand{
    public override int execute(enemy Enemy)
    {
        return Enemy.idle(frames);
    }
}