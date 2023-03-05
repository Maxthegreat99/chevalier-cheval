using System;

public partial class EnemyCommand{

    public float delta {get; set; }
    public int frames {get; set;}
    public virtual int execute(enemy Enemy) {return 0; }
}
public partial class EnemyAttackCommand : EnemyCommand{
    public override int execute(enemy Enemy)
    {
        return Enemy.attack();
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