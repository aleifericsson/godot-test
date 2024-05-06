using Godot;
using System;

public partial class CamControl : Camera3D
{
	// Modifier keys' speed multiplier
	private const float SHIFT_MULTIPLIER = 2.5f;
	private const float ALT_MULTIPLIER = 1.0f / SHIFT_MULTIPLIER;

	[Export(PropertyHint.Range, "0,1")]
	public float Sensitivity { get; set; } = 0.25f;

	// Mouse state
	private Vector2 _mousePosition = new Vector2(0.0f, 0.0f);
	private float _totalPitch = 0.0f;

	// Movement state
	private Vector3 _direction = new Vector3(0.0f, 0.0f, 0.0f);
	private Vector3 _velocity = new Vector3(0.0f, 0.0f, 0.0f);
	private int _acceleration = 30;
	private int _deceleration = -10;
	private float _velMultiplier = 4.0f;

	// Keyboard state
	private bool _w = false;
	private bool _s = false;
	private bool _a = false;
	private bool _d = false;
	private bool _q = false;
	private bool _e = false;
	private bool _shift = false;
	private bool _alt = false;

	public override void _Input(InputEvent @event)
	{
		// Receives mouse motion
		if (@event is InputEventMouseMotion eventMouseMotion)
		{
			_mousePosition = eventMouseMotion.Relative;
		}

		// Receives mouse button input
		if (@event is InputEventMouseButton eventMouseButton)
		{
			switch (eventMouseButton.ButtonIndex)
			{
				case MouseButton.Right:
					Input.MouseMode = eventMouseButton.Pressed ? Input.MouseModeEnum.Captured : Input.MouseModeEnum.Visible;
					break;
				case MouseButton.WheelUp:
					_velMultiplier = Mathf.Clamp(_velMultiplier * 1.1f, 0.2f, 20.0f);
					break;
				case MouseButton.WheelDown:
					_velMultiplier = Mathf.Clamp(_velMultiplier / 1.1f, 0.2f, 20.0f);
					break;
			}
		}

		// Receives key input
		if (@event is InputEventKey keyEvent)
		{
			switch (keyEvent.Keycode)
			{
				case Key.W:
					_w = keyEvent.Pressed;
					break;
				case Key.S:
					_s = keyEvent.Pressed;
					break;
				case Key.A:
					_a = keyEvent.Pressed;
					break;
				case Key.D:
					_d = keyEvent.Pressed;
					break;
				case Key.Q:
					_q = keyEvent.Pressed;
					break;
				case Key.E:
					_e = keyEvent.Pressed;
					break;
				case Key.Shift:
					_shift = keyEvent.Pressed;
					break;
				case Key.Alt:
					_alt = keyEvent.Pressed;
					break;
			}
		}
	}

	// Updates mouselook and movement every frame
	public void _Process(float delta)
	{
		_UpdateMouselook();
		_UpdateMovement(delta);
	}

	// Updates camera movement
	private void _UpdateMovement(float delta)
	{
		// Computes desired direction from key states
		_direction = new Vector3((_d ? 1.0f : 0.0f) - (_a ? 1.0f : 0.0f),
								  (_e ? 1.0f : 0.0f) - (_q ? 1.0f : 0.0f),
								  (_s ? 1.0f : 0.0f) - (_w ? 1.0f : 0.0f));

		// Computes the change in velocity due to desired direction and "drag"
		// The "drag" is a constant acceleration on the camera to bring its velocity to 0
		var offset = _direction.Normalized() * _acceleration * _velMultiplier * delta +
					 _velocity.Normalized() * _deceleration * _velMultiplier * delta;

		// Compute modifiers' speed multiplier
		var speedMulti = 1.0f;
		if (_shift) speedMulti *= SHIFT_MULTIPLIER;
		if (_alt) speedMulti *= ALT_MULTIPLIER;

		// Checks if we should bother translating the camera
		if (_direction == Vector3.Zero && offset.LengthSquared() > _velocity.LengthSquared())
		{
			// Sets the velocity to 0 to prevent jittering due to imperfect deceleration
			_velocity = Vector3.Zero;
		}
		else
		{
			// Clamps speed to stay within maximum value (_velMultiplier)
			_velocity.X = Mathf.Clamp(_velocity.X + offset.X, -_velMultiplier, _velMultiplier);
			_velocity.Y = Mathf.Clamp(_velocity.Y + offset.Y, -_velMultiplier, _velMultiplier);
			_velocity.Z = Mathf.Clamp(_velocity.Z + offset.Z, -_velMultiplier, _velMultiplier);

			Translate(_velocity * delta * speedMulti);
		}
	}

	// Updates mouse look 
	private void _UpdateMouselook()
	{
		// Only rotates mouse if the mouse is captured
		if (Input.MouseMode == Input.MouseModeEnum.Captured)
		{
			_mousePosition *= Sensitivity;
			var yaw = _mousePosition.X;
			var pitch = _mousePosition.Y;
			_mousePosition = new Vector2(0, 0);

			// Prevents looking up/down too far
			pitch = Mathf.Clamp(pitch, -90 - _totalPitch, 90 - _totalPitch);
			_totalPitch += pitch;

			RotateY(Mathf.DegToRad(-yaw));
			RotateObjectLocal(new Vector3(1, 0, 0), Mathf.DegToRad(-pitch));
		}
	}
}
