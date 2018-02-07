using UnityEngine;

public class BattleState : MonoBehaviour {
    
	void Awake () {
        //SaveCharacters.Instance.LoadPlayerCharacters();
        GameStateManager.CurrentGameState = GameState.COMBAT;
	}
}