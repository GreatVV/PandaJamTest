using UniRx;
using UnityEngine;

public abstract class TileAction : MonoBehaviour
{
    public abstract IObservable<Tile> ApplyTiles();
}