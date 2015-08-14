using System;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

public class FieldFactory : ScriptableObject
{
    public TileFactory TileFactory;

    public float delayBetweenCreation = 0.02f;

    public IObservable<Vector3> CreateField(Field field, int width, int height)
    {
        var tiles = CreatePositions(width, height);
        return Observable.Interval(TimeSpan.FromSeconds(delayBetweenCreation)).Zip(tiles, (n, p) => p).Do
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
                 for (var i = 0; i < width; i++)
                 {
                     var currentHeight = Random.Range(2, height);
                     for (var j = 0; j < currentHeight; j++)
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