using UnityEngine;
using UnityEngine.UI;

public class NameUI : MonoBehaviour {

    public delegate void CharacterNameEvent();
    public static CharacterNameEvent OnNameEvent;

    [SerializeField]private Character _character;
    public Character Character
    {
        get { return _character; }
        set { _character = value; }
    }

    [SerializeField]private Text _name;

    private void Awake()
    {
        OnNameEvent += ShowName;
    }
    
    void ShowName()
    {
        _name.text = _character.CharacterName;
    }

    private void OnDisable()
    {
        OnNameEvent -= ShowName;
    }
}