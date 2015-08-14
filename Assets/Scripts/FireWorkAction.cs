using System.Linq;
using UniRx;

public class FireWorkAction : TileAction
{
    public override IObservable<Tile> ApplyTiles()
    {
        return Observable.Create<Tile>
            (
             observable =>
             {
                 var tile = GetComponent<Tile>();
                 var field = FindObjectOfType<Field>();
                 var tiles = field.Tiles.Where(x => x.Position.x == tile.Position.x);
                 foreach (var tile1 in tiles)
                 {
                     observable.OnNext(tile1);
                 }
                 observable.OnCompleted();
                 return Disposable.Empty;
             });

    }
}