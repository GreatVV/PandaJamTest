using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;

public class Helper {

    [MenuItem("Assets/Create/TileFactory")]
    public static void CreateTileFactoryAsset()
    {
        CreateAsset<TileFactory>();
    }

    [MenuItem("Assets/Create/Field Factory")]
    public static void CreateFieldFactoryAsset()
    {
        CreateAsset<FieldFactory>();
    }

    public static void CreateAsset<T>() where T : ScriptableObject
    {
        T asset = ScriptableObject.CreateInstance<T>();

        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (path == "")
        {
            path = "Assets";
        }
        else if (Path.GetExtension(path) != "")
        {
            path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/" + typeof(T).ToString() + ".asset");

        AssetDatabase.CreateAsset(asset, assetPathAndName);

        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
}
