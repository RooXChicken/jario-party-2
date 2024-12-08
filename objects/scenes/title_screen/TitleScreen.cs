using Godot;
using System;

public partial class TitleScreen : Node2D
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
		ResourceLoader.LoadThreadedRequest("res://objects/scenes/game_setup/game_setup.tscn");
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

		if(Input.IsActionJustPressed("back"))
		{
			Node scene = ((PackedScene)ResourceLoader.LoadThreadedGet("res://objects/scenes/game_setup/game_setup.tscn")).Instantiate();
			scene.GetNode<Sprite2D>("TransitionGroup/PreviousScreenSprite").Texture = ImageTexture.CreateFromImage(GetViewport().GetTexture().GetImage());

			GetParent().AddChild(scene);
			QueueFree();
		}
	}

	public void playLoopingShine()
	{
		anim.Play("logo_shine_loop");
	}

	// public void LoadTitle()
	// {
	// 	//add the preloaded scene
	// 	GetParent().AddChild(((PackedScene)ResourceLoader.LoadThreadedGet("res://objects/title_screen/title_screen.tscn")).Instantiate());

	// 	//remove the splashscreen scene
	// 	QueueFree();
	// }
}
