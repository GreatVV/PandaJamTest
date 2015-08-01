using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerDownHandler
{
    public ColorType color;
    public string Id;
    public List<Tile> Neighbours = new List<Tile>();

    public IObservable<Tile> Clicked = new Subject<Tile>();

    public Vector3 Position
    {
        get
        {
            return transform.localPosition;
        }
        set
        {
            transform.localPosition = value;
        }
    }

    #region IPointerDownHandler Members

    public void OnPointerDown(PointerEventData eventData)
    {
        (Clicked as Subject<Tile>).OnNext(this);
    }

    void OnDestroy()
    {
        (Clicked as Subject<Tile>).OnCompleted();
    }


    #endregion

    public static List<Tile> GetChain(Tile tile, List<Tile> alreadyInChain = null)
    {
        if (tile.color == ColorType.Universal)
        {
            return new List<Tile>();
        }

        if (alreadyInChain == null)
        {
            alreadyInChain = new List<Tile>
                             {
                                 tile
                             };
        }

        var newTiles =
            tile.Neighbours.Except(alreadyInChain)
                .Where(x => x.color == tile.color || x.color == ColorType.Universal)
                .ToList();
        if (newTiles.Any())
        {
            alreadyInChain.AddRange(newTiles);
            foreach (var newTile in newTiles)
            {
                GetChain(newTile, alreadyInChain);
            }
        }
        return alreadyInChain;
    }

    public void GrabNeighbours()
    {
        Neighbours.Clear();
        var leftPosition = transform.position + Vector3.left;
        var rightPosition = transform.position + Vector3.right;
        var upPosition = transform.position + Vector3.up;
        var downPosition = transform.position + Vector3.down;
        var points = new[]
                     {
                         leftPosition,
                         rightPosition,
                         upPosition,
                         downPosition
                     };
        foreach (var vector3 in points)
        {
            var colliders = Physics.OverlapSphere(vector3, 0.3f);
            if (colliders.Any())
            {
                var tiles =
                    colliders.Where(x => x.GetComponent<Tile>() != null)
                             .Select(x => x.GetComponent<Tile>())
                             .Except(Neighbours);
                Neighbours.AddRange(tiles);
            }
        }
    }

    private void Awake()
    {
        Id = Guid.NewGuid().ToString();
    }
}