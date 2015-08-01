using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileFactory : ScriptableObject
{
    public List<Tile> Prefabs = new List<Tile>();

    public Tile GetTile(ColorType color = ColorType.None)
    {
        if (color == ColorType.None)
        {
            color = new[]
                    {
                        ColorType.Red,
                        ColorType.Blue,
                        ColorType.Green,
                        ColorType.Violet,
                        ColorType.LightBlue,
                        ColorType.Orange,
                    }.Random();
        }

        var prefab = Prefabs.FirstOrDefault(x => x.color == color);
        if (prefab == null)
        {
            prefab = Prefabs.Random();
            Debug.LogWarning("No prefab for color "+color);
        }
        return Instantiate(prefab);
    }
}