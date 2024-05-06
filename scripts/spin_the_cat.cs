using Godot;
using System;

public partial class butt_name : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
	

private void _on_blaze_spin_complete(int spins)
{
	if (spins == 1){
			Text = ("spin the cat again");
		}
		else{
			Text = ("spin the cat again x" + spins.ToString());
		}
}
}
