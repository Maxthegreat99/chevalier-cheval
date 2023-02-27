using System;

public class EnemyCommand{

    public float delta {get; set; }
    public int frames {get; set;}
    public virtual int execute(enemy Enemy) {return 0; }
}
public class EnemyAttackCommand : EnemyCommand{
    public override int execute(enemy Enemy)
    {
        return Enemy.attack();
    }
}
public class EnemyFollow : EnemyCommand{
    public override int execute(enemy Enemy)
    {
        Enemy.follow(delta);
        return 0;
    }
}
public class EnemyCycle : EnemyCommand{
    public override int execute(enemy Enemy)
    {
        return Enemy.walkCycle(delta);
    }
}
public class EnemyDeath : EnemyCommand{
    public override int execute(enemy Enemy)
    {
        return Enemy.death();
    }
}
public class EnemyIdle : EnemyCommand{
    public override int execute(enemy Enemy)
    {
        return Enemy.idle(frames);
    }
}