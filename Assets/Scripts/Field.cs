using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class Field : MonoBehaviour
{
    public FieldFactory FieldFactory;
    public int Height = 5;
    private bool isReady;
    private TileMoveCollection tileMoves = new TileMoveCollection();
    public List<Tile> Tiles;
    public int Width = 10;

    public void AddTile(Tile tile, Vector3 position)
    {
        tile.transform.SetParent(transform, false);
        tile.Clicked.Subscribe
            (
             OnTileClicked,
             error =>
             {
             },
             () =>
             {
                 Tiles.Remove(tile);
             });
        Tiles.Add(tile);
        tile.transform.localPosition = position;
    }

    private void Start()
    {
        FieldFactory.CreateField(this, Width, Height).Subscribe
            (
             x => Debug.Log(x),
             () =>
             {
                 Debug.Log("Done");
                 RecalculateTiles();
                 isReady = true;
             });

        transform.position = new Vector2(-Width / 2f, -Height / 2f);
    }

    private void RecalculateTiles()
    {
        foreach (var tile in Tiles)
        {
            tile.GrabNeighbours();
        }
    }

    private void OnTileClicked(Tile tile)
    {
        if (!isReady)
        {
            return;
        }
        isReady = false;
        Debug.Log("Tile down at " + transform.localPosition + " Id: " + tile.Id);
        var chain = Tile.GetChain(tile);
        chain.Sort(ByY);
        Observable.Interval(TimeSpan.FromSeconds(0.1f))
                  .Zip(chain.ToObservable(), (n, p) => p)
                  .Do(DestoryTile)
                  .SelectMany(x => MoveTiles(x, chain))
                  .Subscribe
            (
             x =>
             {
             },
             error =>
             {
             },
             OnAnimationEnd);
    }

    private void OnAnimationEnd()
    {
        tileMoves.Clear();
        RecalculateTiles();
        isReady = true;
    }

    private IObservable<Tile> MoveTiles(Tile x, IEnumerable<Tile> chain)
    {
        var tileToMove = Tiles.Except(chain)
                              .Where(newTile => newTile.Position.x == x.Position.x && newTile.Position.y > x.Position.y);
        foreach (var tile1 in tileToMove)
        {
            tileMoves.MoveTile(tile1, Vector3.down);
        }
        return tileMoves.ApplyMoves();
    }

    private void DestoryTile(Tile tile)
    {
        Debug.Log("Remove " + tile.Position);
        Destroy(tile.gameObject);
    }

    private int ByY(Tile x, Tile y)
    {
        return y.transform.position.y.CompareTo(x.transform.position.y);
    }

    public void AddRow()
    {
        if (!isReady)
        {
            return;
        }
        isReady = false;
        foreach (var tile in Tiles)
        {
            tileMoves.MoveTile(tile, Vector3.up);
        }

        for (int i = 0; i < Width; i++)
        {
            var tile = FieldFactory.TileFactory.GetTile();
            AddTile(tile, new Vector3(i, 0));
        }

        tileMoves.ApplyMoves().Subscribe(x => { }, error => { }, OnAnimationEnd);
    }
}