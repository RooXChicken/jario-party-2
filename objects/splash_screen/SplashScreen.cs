using Godot;
using System;

public partial class SplashScreen : Node2D
{
	public override void _Ready()
	{
		//preload the nex scene in the background
		ResourceLoader.LoadThreadedRequest("res://objects/title_screen/title_screen.tscn");
	}

	public override void _Process(double delta)
	{
		if(Input.IsActionJustPressed("select"))
			LoadTitle();
	}

	public void LoadTitle()
	{
		//add the preloaded scene
		GetParent().AddChild(((PackedScene)ResourceLoader.LoadThreadedGet("res://objects/title_screen/title_screen.tscn")).Instantiate());

		//remove the splashscreen scene
		QueueFree();
	}
}
