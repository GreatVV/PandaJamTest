using System.Linq;
using UniRx;
using UnityEngine;

public class BombAction : TileAction
{
    public GameObject ExplosionEffect;

    public override IObservable<Tile> ApplyTiles()
    {
        return Observable.Create<Tile>
            (
             observable =>
             {
                 var explosion = Instantiate(ExplosionEffect, transform.position, Quaternion.identity);
                 Destroy(explosion, 1f);
                 var colliders = Physics.OverlapSphere(transform.position, 1f);
                 var tiles = colliders.Where(x => x.GetComponent<Tile>()).Select(x=>x.GetComponent<Tile>());
                 if (tiles.Any())
                 {
                     foreach (var tile in tiles)
                     {
                         observable.OnNext(tile);
                     }
                     observable.OnCompleted();
                 }
                 else
                 {
                     observable.OnCompleted();
                 }
                 return Disposable.Empty;
             });
    }
}