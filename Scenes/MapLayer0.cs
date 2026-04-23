using Godot;
using System;

public partial class MapLayer0 : TileMapLayer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public Vector2 ChangeVectorToLocal(Vector2 vec)
	{
		Vector2 global = vec;
        Vector2 local = ToLocal(global);
        // Vector2I cell = LocalToMap(local);
        // Vector2 worldPos = MapToLocal(cell);


		return local;
	}

}
