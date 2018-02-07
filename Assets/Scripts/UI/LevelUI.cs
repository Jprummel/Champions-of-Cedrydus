using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    public delegate void LevelEvent();
    public static LevelEvent OnLevelEvent;

    [SerializeField]private Character _character;
    public Character Character
    {
        get { return _character; }
        set { _character = value; }
    }

    [SerializeField]private Text _level;

    private void Awake()
    {
        OnLevelEvent += ShowLevel;
    }

    void ShowLevel()
    {
        _level.text = "Lv. " + _character.Level; //Character gets assigned at start of combat
    }

    private void OnDisable()
    {
        OnLevelEvent -= ShowLevel;
    }
}