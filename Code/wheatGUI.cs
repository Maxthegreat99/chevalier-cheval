using Godot;
using System;
using System.Threading.Tasks;
public partial class wheatGUI : MarginContainer
{
	Label wheatDisplayer;
	public Player player = new Player();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		initialize();
	}
	public async void initialize(){
		wheatDisplayer = (Label)await GetNodeAsync("wheatAmount");		
	}
	private async Task<Node> GetNodeAsync(String path){
		while (true){
			var node = GetNodeOrNull(path);
			if (node != null && node.IsInsideTree())
				return node;

			await ToSignal(GetTree(), "process_frame");
		}
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(player != null)
			wheatDisplayer.Text = player.wheatAmount.ToString();
	}
}
