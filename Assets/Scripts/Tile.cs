using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerDownHandler
{
    public string Id;

    public ColorType color;
    public List<Tile> Neighbours = new List<Tile>();

    public static List<Tile> GetChain(Tile tile, List<Tile> alreadyInChain = null)
    {
        if (tile.color == ColorType.Universal)
        {
            return new List<Tile>();
        }

        if (alreadyInChain == null)
        {
            alreadyInChain = new List<Tile>()
                             {
                                 tile
                             };
        }

        var newTiles = tile.Neighbours.Except(alreadyInChain).Where(x=>x.color == tile.color || x.color == ColorType.Universal).ToList();
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
                var tiles = colliders.Where(x => x.GetComponent<Tile>() != null).Select(x=>x.GetComponent<Tile>()).Except(Neighbours);
                Neighbours.AddRange(tiles);
            }
        }
    }


    void Awake()
    {
        Id = Guid.NewGuid().ToString();
    }
   

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Tile down at " + transform.localPosition + " Id: " + Id);
        var chain = GetChain(this).ToObservable();
        Observable.Interval(TimeSpan.FromSeconds(0.1f)).Zip(chain, (n, p) => p).Subscribe(
                                                                                           x =>
                                                                                           {
                                                                                               x.GetComponent<MeshRenderer>()
                                                                                                   .material.color =
                                                                                                   Color.black;
                                                                                           });


    }
}