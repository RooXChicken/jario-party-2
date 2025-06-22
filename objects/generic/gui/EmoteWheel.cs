using Godot;
using System;
using System.Collections.Generic;

public partial class EmoteWheel : Node2D
{
	private float joystickDeadzone = 0.06f;

	private List<Sprite2D> emotes;
	private CanvasGroup emoteGroup;
	private Sprite2D selector;

	private double step = 0;

	public override void _Ready()
	{
		emotes = new List<Sprite2D>();
		selector = GetNode<Sprite2D>("SelectorSprite");
		emoteGroup = GetNode<CanvasGroup>("EmoteGroup");

		foreach(Node2D _node in emoteGroup.GetChildren())
		{
			if(_node is Sprite2D)
				emotes.Add((Sprite2D)_node);
		}

		if(emotes.Count <= 0)
			return;

		step = 360.0/emotes.Count;
		for(int i = 0; i < emotes.Count; i++)
			emotes[i].Position = new Vector2((float)Math.Sin(Mathf.DegToRad(i*step)), (float)Math.Cos(Mathf.DegToRad(i*step))) * 60;
	}

	public override void _Process(double delta)
	{
		Vector2 joyInput = new Vector2(Input.GetAxis("left", "right"), Input.GetAxis("up", "down"));

		//forced deadzone (evil mode)
		if(Math.Abs(joyInput.X) < joystickDeadzone) joyInput.X = 0;
		if(Math.Abs(joyInput.Y) < joystickDeadzone) joyInput.Y = 0;

		emoteGroup.Modulate = new Color(1, 1, 1, joyInput.Length());
		selector.Modulate = new Color(1, 1, 1, joyInput.Length()/2);

		selector.Position = joyInput*60;

		foreach(Sprite2D _sprite in emotes)
		{
			float _distance = _sprite.Position.DistanceTo(selector.Position);
			// GD.Print(_distance);
			if(_distance < 32)
				_sprite.Scale = new Vector2(1.4f, 1.4f);
			else
				_sprite.Scale = new Vector2(1.0f, 1.0f);
		}
	}
}
