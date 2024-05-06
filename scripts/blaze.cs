using Godot;
using System;

public partial class blaze : Sprite2D
{
	
	float rotation = 0f;
	bool rotating = false;
	int rotations = 0;
	
	[Signal]
	public delegate void SpinCompleteEventHandler(int spins);
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (rotating){
			rotation += 10f;
			Rotation += 10 * (float)delta;
		}
		if (rotation > 360f){
			rotating = false;
			rotation = 0f;
			Rotation = 0f;
			//rotation_ended();
		}
	}
	
		
	private void _on_button_pressed()
	{
		rotating = true;
		rotations += 1;
		EmitSignal(SignalName.SpinComplete, rotations);
	}
}



