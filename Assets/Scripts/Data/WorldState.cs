using UnityEngine;
using Serialization;

public class WorldState : MonoBehaviour {

    void Awake()
    {
        SaveCharacters.Instance.LoadPlayerCharacters();
        GameStateManager.CurrentGameState = GameState.WORLD;
    }
}