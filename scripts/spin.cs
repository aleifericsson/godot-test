using Godot;
using System;

public partial class spin : RigidBody3D
{
	
	private float _speed = 20f;
	private float _angularSpeed = Mathf.Pi * 20;

	private int rotating = 0;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("LOADED");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{	
		var direction_x = 0;
		var direction_z = 0;
		if (Input.IsActionPressed("ui_left"))
		{
			direction_x = -1;
		}
		if (Input.IsActionPressed("ui_right"))
		{
			direction_x = 1;
		}
		if (Input.IsActionPressed("ui_up"))
		{
			direction_z = -1;
		}
		if (Input.IsActionPressed("ui_down"))
		{
			direction_z = 1;
		}
		float cool_speed = _speed * (float)delta;
		Position += new Vector3(cool_speed *direction_x, 0, cool_speed * direction_z) ;



		Rotation += new Vector3(0, _angularSpeed * (float)delta * rotating, 0); //converts delta to float then times by omega

		//var velocity = Vector3.Right.Rotated(Rotation, 0) * _speed;

		//Position += velocity * (float)delta;
	}

	private void _on_check_button_toggled(bool toggled_on)
	{
	if (toggled_on) {
		rotating = 1;
	}
	else{
		rotating = 0;
	}
}
}



