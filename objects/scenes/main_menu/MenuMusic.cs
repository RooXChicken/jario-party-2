using Godot;
using System;

public partial class MenuMusic : AudioStreamPlayer
{

	private byte menuStage = 0;
	private AudioStreamSynchronized streamSync;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//:3
		GD.Print(Stream.GetType());
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
		streamSync = (AudioStreamSynchronized)Stream;
		if(Input.IsActionJustPressed("select"))
			if(menuStage < 3)
			{
				streamSync.SetSyncStreamVolume(menuStage, 0);
				menuStage++;
			}
		if(Input.IsActionJustPressed("back"))
			if(menuStage > 0)
			{
				streamSync.SetSyncStreamVolume(menuStage, -80);
				menuStage--;
			}
	}
}
