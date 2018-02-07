using UnityEngine;

public class AddExperience : MonoBehaviour {

    public delegate void AddExperienceEvent(Character character, float xpToAdd);
    public static AddExperienceEvent OnAddExperience;
    public delegate void LevelUpEvent();
    public static LevelUpEvent OnLevelUp;
    [SerializeField] private GameObject _statAllocationPanel;
    private static GameObject statAllocationPanel;

	void Awake () {
        OnAddExperience += AddXP;
        statAllocationPanel = _statAllocationPanel;
	}

    void AddXP(Character character, float xpToAdd)
    {
        character.CurrentXP += xpToAdd;
        character.TotalXPEarned += xpToAdd;
        if (character.CurrentXP >= character.RequiredXP)
        {
            PostCombatRewardsScreen.EndOfCombat = false;
            LevelUp(character);
            OnLevelUp();
        }
        else
            PostCombatRewardsScreen.EndOfCombat = true;
    }

    public static void ActivateStatAllocation()
    {
        statAllocationPanel.SetActive(true);
    }

    void LevelUp(Character character)
    {
        character.Level++;
        character.GainClassStats();
        character.StatPoints += 2;
        float leftOverXp = character.CurrentXP - character.RequiredXP;
        character.RequiredXP = Mathf.RoundToInt(character.RequiredXP * 1.04f + 150);
        character.CurrentXP = leftOverXp;
        character.CurrentHP = character.MaxHP;
        if(character.CurrentXP > character.RequiredXP)
        {
            LevelUp(character);
        }
    }

    void OnDisable()
    {
        OnAddExperience -= AddXP;
    }
}