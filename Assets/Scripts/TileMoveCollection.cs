using System.Collections.Generic;
using System.Linq;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UniRx;
using UnityEngine;

public class TileMoveCollection
{
    private List<TileMove> tileMoves = new List<TileMove>();

    public TileMove MoveTile(Tile tile, Vector3 relative)
    {
        var tileMove = tileMoves.FirstOrDefault(tM => tM.Tile == tile);
        if (tileMove == null)
        {
            tileMove = new TileMove
                       {
                           Tile = tile,
                           StartPosition = tile.Position
                       };
            tileMoves.Add(tileMove);
        }
        tileMove.Relative += relative;
        return tileMove;
    }

    public void Clear()
    {
        tileMoves.Clear();
    }

    public IObservable<Tile> ApplyMoves()
    {
        return tileMoves.Select(x => x.Move()).Merge();
    }

    private List<TweenerCore<Vector3, Vector3, VectorOptions>> tweens = new List<TweenerCore<Vector3, Vector3, VectorOptions>>();
}