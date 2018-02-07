using UnityEngine;
using Serialization;

public class SaveLoadButtons : MonoBehaviour {

    public void Save()
    {
        SaveCharacters.Instance.SavePlayerCharacters();
    }

    public void Load()
    {
        SaveCharacters.Instance.LoadPlayerCharacters();
    }

    public void Clear()
    {
        SaveCharacters.Instance.ClearSave();
    }
}