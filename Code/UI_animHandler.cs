using Godot;
using System;

public partial class UI_animHandler : Control
{
    AnimatedSprite2D lifeSprite = new AnimatedSprite2D();
    LifeGUI life = new LifeGUI();
    Player player = new Player();
    public int init(AnimatedSprite2D lifeSpriteNode,Player playerNode,LifeGUI lifeNode){
        lifeSprite = lifeSpriteNode;
        life = lifeNode;
        player = playerNode;
        return 0;
    }
    public int hurtHealth(){
        
        
        return 0;
    }
}