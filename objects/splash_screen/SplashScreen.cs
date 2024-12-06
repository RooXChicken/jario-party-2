using Godot;
using System;

public partial class SplashScreen : Node2D
{
	private AnimationPlayer anim;
	private AudioStreamPlayer2D titleThemePlayer;

	public override void _Ready()
	{
		//get animation player
		anim = GetNode<AnimationPlayer>("AnimationPlayer");

		//get stream player
		titleThemePlayer = GetNode<AudioStreamPlayer2D>("TitleTheme");

		//preload the nex scene in the background
		//ResourceLoader.LoadThreadedRequest("res://objects/title_screen/title_screen.tscn");
	}

	public override void _Process(double delta)
	{
		//skip to title
		if(Input.IsActionJustPressed("select") && anim.CurrentAnimationPosition < 2)
		{
			anim.Seek(2);

			// if(!titleThemePlayer.Playing)
			// 	titleThemePlayer.Play();
		}
	}

	public void playSong()
	{
		titleThemePlayer.Play();
	}

	// public void LoadTitle()
	// {
	// 	//add the preloaded scene
	// 	GetParent().AddChild(((PackedScene)ResourceLoader.LoadThreadedGet("res://objects/title_screen/title_screen.tscn")).Instantiate());

	// 	//remove the splashscreen scene
	// 	QueueFree();
	// }
}
