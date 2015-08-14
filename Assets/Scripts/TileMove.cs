using System.Linq;
using DG.Tweening;
using UniRx;
using UnityEngine;

public class TileMove
{
    public Vector3 Relative;
    public Vector3 StartPosition;
    public Tile Tile;

    public IObservable<Tile> Move()
    {
        return Observable.Create<Tile>
            (
             observable =>
             {
                 if (!Tile)
                 {
                     observable.OnCompleted();
                     return Disposable.Empty;
                 }

                 var tween = DOTween.TweensByTarget(Tile.transform);
                 if (tween == null)
                 {
                     var tweener = Tile.transform.DOLocalMove(FinishPosition, 0.1f, true);
                     tweener.OnComplete
                         (
                          () =>
                          {
                              observable.OnNext(Tile);
                              observable.OnCompleted();
                          });
                     tweener.Kill(true);
                 }
                 else
                 {
                     if (tween.Count > 1)
                     {
                         Debug.LogWarningFormat("More then 1 tween for {0}", Tile.Id);
                     }
                     
                     var targetTween = tween.First();
                     targetTween.OnComplete
                        (
                         () =>
                         {
                             observable.OnNext(Tile);
                             observable.OnCompleted();
                         });
                 }
                 return Disposable.Empty;
             });
    }

    public Vector3 FinishPosition
    {
        get
        {
            return StartPosition + Relative;
        }
    }
}