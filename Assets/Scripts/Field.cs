using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Field : MonoBehaviour
{
    public int Height = 5;
    public TileFactory TileFactory;
    public List<Tile> Tiles;
    public int Width = 10;

    private void Start()
    {
        var tiles = CreatePositions(Width, Height);
        Observable.Interval(TimeSpan.FromSeconds(0.05f)).Zip(tiles, (n, p) => p).Do(
                                                                                   x =>
                                                                                   {
                                                                                       var tile = TileFactory.GetTile();
                                                                                       tile.transform.SetParent(
                                                                                                                transform,
                                                                                                                false);
                                                                                       Tiles.Add(tile);
                                                                                       tile.transform.localPosition = x;
                                                                                   })
                  .Subscribe(x => Debug.Log(x),
                             () =>
                             {
                                 Debug.Log("Done");
                                 foreach (var tile in Tiles)
                                 {
                                     tile.GrabNeighbours();
                                 }
                             });

        transform.position = new Vector2(-Width/2f, -Height/2f);
    }

    private IObservable<Vector3> CreatePositions(int width, int height)
    {
        return Observable.Create<Vector3>(
                                          observer =>
                                          {
                                              for (var j = 0; j < height; j++)
                                              {
                                                  for (var i = 0; i < width; i++)
                                                  {

                                                      var pos = new Vector2(i, j);
                                                      observer.OnNext(pos);

                                                  }
                                              }
                                              observer.OnCompleted();
                                              return Disposable.Empty;
                                          });
    }
}