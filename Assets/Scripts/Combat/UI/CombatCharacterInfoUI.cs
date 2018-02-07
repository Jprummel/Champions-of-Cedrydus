using System.Collections.Generic;
using UnityEngine;

public class CombatCharacterInfoUI : MonoBehaviour
{
#pragma warning disable 0414
    [SerializeField] private List<GameObject> _characterInfoContainers = new List<GameObject>();
#pragma warning restore 0414
    [SerializeField] private List<LevelUI> _levelContainers = new List<LevelUI>();
    [SerializeField] private List<NameUI> _nameContainers = new List<NameUI>();
    [SerializeField] private List<HealthUI> _healthContainers = new List<HealthUI>();
	
	void Start ()
    {
        for (int i = 0; i < BattleStateMachine.BattleCharacters.Count; i++)
        {
            Character character = BattleStateMachine.BattleCharacters[i];

            if (character.Initiator)
            {
                //Sets UI elements for the initiator
                CombatStatComparisonUI.OnShowInitiatorStats(character);
                _healthContainers[0].Character = character;
                _levelContainers[0].Character = character;
                _nameContainers[0].Character = character;
            }
            else
            {
                //Sets UI elements for the initiated
                CombatStatComparisonUI.OnShowInitiatedStats(character);
                _healthContainers[1].Character = character;
                _levelContainers[1].Character = character;
                _nameContainers[1].Character = character;
            }
        }

        if(HealthUI.OnHealthEvent != null)
        {
            HealthUI.OnHealthEvent();
        }

        if(LevelUI.OnLevelEvent != null)
        {
            LevelUI.OnLevelEvent();
        }

        if(NameUI.OnNameEvent != null)
        {
            NameUI.OnNameEvent();
        }
    }
}