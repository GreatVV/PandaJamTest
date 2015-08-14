using UnityEngine;

public class LoadSceneButton : MonoBehaviour
{
    public void LoadScene(string levelName)
    {
        Application.LoadLevel(levelName);
    }

}