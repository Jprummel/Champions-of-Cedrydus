using UnityEngine;

public class SetPlayerPositions : MonoBehaviour {

    [SerializeField] private Transform _initiator;
    [SerializeField] private Transform _initiated;
    
    private void Start()
    {
        GameObject characters = GameObject.FindWithTag(InlineStrings.BATTLECHARACTERSTAG);

        int index = 0;

        foreach (Transform child in characters.transform)
        {
            Character character = BattleStateMachine.BattleCharacters[index];

            index++;

            if (character.Initiator)
            {
                child.transform.position = _initiator.position;
                //CombatStatComparisonUI.OnShowInitiatorStats(character);
                //LevelUI.OnCombatLevelEvent(character);
                //NameUI.OnNameEvent(character);
            }
            else
            {
                child.transform.position = _initiated.position;
                //CombatStatComparisonUI.OnShowInitiatedStats(character);
                //LevelUI.OnCombatLevelEvent(character);
                //NameUI.OnNameEvent(character);
            }
        }          
    }
}