using UnityEngine;
using System.Collections;
using UniRx;

public class LevelManager : MonoBehaviour
{
    public LevelDescription[] Levels;

    public static LevelDescription ChosenLevel;

    public Transform root;

    public LevelButton Prefab;

    public void Awake()
    {
        foreach (Transform childTransform in root)
        {
            Destroy(childTransform.gameObject);
        }

        foreach (var levelDescription in Levels)
        {
            var go = Instantiate(Prefab);
            go.Level = levelDescription;
            go.transform.SetParent(root, false);
            go.Button.OnClickAsObservable().Subscribe
                (
                 x =>
                 {
                     ChosenLevel = go.Level;
                     Application.LoadLevel("Game");
                 });
        }
    }
}