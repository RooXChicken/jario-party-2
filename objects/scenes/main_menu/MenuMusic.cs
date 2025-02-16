using Godot;
using System;

public partial class MenuMusic : AudioStreamPlayer
{
	private readonly int STAGE_COUNT = 3;

	private byte menuStage = 0;
	private AudioStreamSynchronized streamSync;

	private float[] volume;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//:3
		// what the fuck
		streamSync = (AudioStreamSynchronized)Stream;

		volume = new float[STAGE_COUNT];
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Input.IsActionJustPressed("select"))
			if(menuStage < STAGE_COUNT)
			{
				// streamSync.SetSyncStreamVolume(menuStage, 0);
				menuStage++;
			}
		if(Input.IsActionJustPressed("back"))
			if(menuStage > 0)
			{
				// streamSync.SetSyncStreamVolume(menuStage, -80);
				menuStage--;
			}

		for(byte i = 0; i < STAGE_COUNT; i++)
		{
			volume[i] = Mathf.Lerp(volume[i], (i > menuStage) ? -40.0f : 0.0f, 0.1f);
			streamSync.SetSyncStreamVolume(i, volume[i]);
		}

		GD.Print(volume[1]);
	}
}
