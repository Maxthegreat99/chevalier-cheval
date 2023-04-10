using Godot;
using System;
using System.Threading.Tasks;
public partial class main : Node2D
{
	public UI_animHandler UI_animator;
	public override void _Ready()
	{
		initialize();
	}
	public async void initialize(){
		UI_animator = new UI_animHandler();
		UI_animator.init((AnimatedSprite2D) await GetNodeAsync("/root/Screen/GUI/Life/lifeSprite"),(Player)await GetNodeAsync("/root/player"),(LifeGUI)await GetNodeAsync("/root/Screen/GUI/Life"));
		LifeGUI lifeUi = (LifeGUI)await GetNodeAsync("/root/Screen/GUI/Life");
		wheatGUI wheatUI = (wheatGUI)await GetNodeAsync("/root/Screen/GUI/wheat");
		wheatUI.player = (Player)await GetNodeAsync("/root/player");
		lifeUi.addPlayer((Player)await GetNodeAsync("/root/player"));
	}

	private async Task<Node> GetNodeAsync(String path){
		while (true){
			var node = GetNodeOrNull(path);
			if (node != null && node.IsInsideTree())
				return node;

			await ToSignal(GetTree(), "process_frame");
		}
	}
}
