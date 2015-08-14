using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public Button Button;
    private LevelDescription _level;

    [SerializeField]
    private Text label;

    public LevelDescription Level
    {
        get
        {
            return _level;
        }
        set
        {
            _level = value;
            label.text = string.Format("Level {0}x{1}", _level.Width, _level.Height);
        }
    }

    void Awake()
    {
        if (!Button)
        {
            Button = GetComponent<Button>();
        }
    }
}

