using UnityEngine;
using Serialization;
public class LoadAtStart : MonoBehaviour {

	void Awake () {
        SaveCharacters.Instance.LoadPlayerCharacters();
    }
}