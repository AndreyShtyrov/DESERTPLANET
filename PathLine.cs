using DesertPlanet.source;
using DesertPlanet.source.Field;
using DesertPlanet.source.Interfaces;
using Godot;
using System;
using System.Collections.Generic;

public partial class PathLine : Node2D
{
	public TileMap TileMap { get; set; }

	public GameMode GameMode { get; set; }

	public Color ColorLine { get; set; } = new Color(0, 0, 0, 1);

	public Vector2I LabelShift { get; set; } = new Vector2I(20, 0);
	public IOwnedToken ownedToken { get; set; }

	public int LineWidth { get; set; } = 2;
	private Nullable<Vector2I> prevPosition { get; set; }	
	private Label Label { get; set; }

	private PathNode[,] PathGrid;

	private List<Vector2> Path { get; set; }
	public override void _Ready()
	{
		Label = GetNode<Label>("EnergyValue");
	}

	public void SetData(IOwnedToken token)
	{
		ownedToken = token;
		Path = new List<Vector2>();
		PathGrid = GameMode.GetPaths(token.Id);
		prevPosition = null;
		Visible = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!Visible)
			return;
		var position = TileMap.LocalToMap(TileMap.GetLocalMousePosition());
		if (prevPosition == position)
			return;
		if (!GameMode.Map.InBound(position))
			return;
        prevPosition = position;
		var node = PathGrid[prevPosition.Value.X, prevPosition.Value.Y];
		if (!node.IsReachable)
			return;

        RebuildPathLine(new Vector2I(prevPosition.Value.X, prevPosition.Value.Y));
	}

	private void RebuildPathLine(Vector2I position)
	{
		var node = PathGrid[position.X, position.Y];
		var path = new List<Vector2>() { TileMap.ToGlobal(TileMap.MapToLocal(position))};
		var arrow = node;
		while (arrow.Energy > 0)
		{
			path.Add(TileMap.ToGlobal(TileMap.MapToLocal(arrow.Arrow)));
			arrow = PathGrid[arrow.Arrow.X, arrow.Arrow.Y];
        }
        Label.Text = node.Energy.ToString();
        Path = path;
		Label.Position = TileMap.ToGlobal(TileMap.MapToLocal(position) + LabelShift);
		QueueRedraw();
	}

    public override void _Draw()
    {
		if (Path == null) return;
		if (!Visible) return;
		if (Path.Count == 0) return;

		var prevPoint = Path[0];
        for(int i = 1; i < Path.Count; i++)
		{
			DrawLine(prevPoint, Path[i], ColorLine, LineWidth);
			prevPoint = Path[i];
		}
    }
}
