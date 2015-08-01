using System;
using UniRx;
using UnityEngine;

public class FieldFactory : ScriptableObject
{
    public TileFactory TileFactory;

    public IObservable<Vector3> CreateField(Field field, int width, int height)
    {
        var tiles = CreatePositions(width, height);
        return Observable.Interval(TimeSpan.FromSeconds(0.05f)).Zip(tiles, (n, p) => p).Do
            (
             position =>
             {
                 var tile = TileFactory.GetTile();
                 field.AddTile(tile, position);
             });}

    private IObservable<Vector3> CreatePositions(int width, int height)
    {
        return Observable.Create<Vector3>
            (
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