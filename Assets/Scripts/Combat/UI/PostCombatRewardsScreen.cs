using UnityEngine;
using UnityEngine.UI;

public class PostCombatRewardsScreen : MonoBehaviour {

    public delegate void PostCombatEvent();
    public static PostCombatEvent OnPostCombatEvent;

    public static bool EndOfCombat = true;
    
    [SerializeField] private GameObject _defaultEventSystem;
    [SerializeField] private GameObject _battleOptions;
    [SerializeField] private GameObject _postCombatRewardsPanel;
    [SerializeField] private Text _itemName;
    [SerializeField] private Text _xpToGive;
    [SerializeField] private Text _goldToGive;

	void Awake ()
    {
        OnPostCombatEvent += ShowCombatRewards;
	}

    void ShowCombatRewards()
    {
        _defaultEventSystem.SetActive(false);
        _battleOptions.SetActive(false);
        _postCombatRewardsPanel.SetActive(true);
        _xpToGive.text = PostCombatRewards.XPToGive + " XP";
        _goldToGive.text = PostCombatRewards.GoldToGive + " Gold";
        if (PostCombatRewards.DropItem) { _itemName.text = "Teleport Key"; }
    }

    public void CloseCombatRewards()
    {
        _postCombatRewardsPanel.SetActive(false);
        AddExperience.OnAddExperience(BattleStateMachine.CharacterThatWon, PostCombatRewards.XPToGive);
        BattleStateMachine.CharacterThatWon.Gold += PostCombatRewards.GoldToGive;
        if (PostCombatRewards.DropItem) BattleStateMachine.CharacterThatWon.GetComponent<PlayerCharacter>()._PlayerInventory.AddInventoryItem("Teleport Key");

        if (EndOfCombat && BattleStateMachine.PVPWon())
        {
            SetPunishmentUI.SetPunishmentActive(true);
        }
        else if (EndOfCombat)
        {
            BattleStateMachine.OnExit();
        }
    }

    private void OnDisable()
    {
        OnPostCombatEvent -= ShowCombatRewards; 
    }
}