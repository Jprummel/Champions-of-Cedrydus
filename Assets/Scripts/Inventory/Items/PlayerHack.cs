using UnityEngine;
using Inventory;
using UI;
using Serialization;
using DG.Tweening;

public class PlayerHack : UsableItem
{
    public override void UseItem()
    {
        PlayerTarget.CanSelectAnyone = true;
        PlayerTarget._TargetingMode = TargetingMode.Players;
        base.UseItem();
    }

    public override void ExecuteUsableEffect()
    {
        base.ExecuteUsableEffect();

        string indicationText;

        //Halves stats if target is enemy, Doubles stats if target is player (does not affect HP)
        if(PlayerTarget.Target == _user)
        {
            _user.BonusAttack = _user.Attack;
            _user.BonusDefense = _user.Defense;
            _user.BonusTech = _user.Tech;
            _user.BonusSpeed = _user.Speed;
            _user.TurnsBuffed = 3;
            _user.CalculateTotalStats();
            OverWorldStatUI.OnShowStats(_user);

            indicationText = "Doubled your stats for 3 turns.";
        }
        else
        {
            PlayerTarget.Target.BonusAttack -= Mathf.RoundToInt(PlayerTarget.Target.Attack / 2);
            PlayerTarget.Target.BonusDefense -= Mathf.RoundToInt(PlayerTarget.Target.Defense / 2);
            PlayerTarget.Target.BonusTech -= Mathf.RoundToInt(PlayerTarget.Target.Tech / 2);
            PlayerTarget.Target.BonusSpeed -= Mathf.RoundToInt(PlayerTarget.Target.Speed / 2);
            _user.TurnsDebuffed = 3;
            PlayerTarget.Target.CalculateTotalStats();

            indicationText = "Halved " + PlayerTarget.Target.CharacterName + "'s stats for 3 turns";
        }

        if (IndicateItemEffect.OnShowIndicator != null)
            IndicateItemEffect.OnShowIndicator(true, indicationText);

        Sequence lockSequence = DOTween.Sequence();
        lockSequence.AppendInterval(2f);
        lockSequence.AppendCallback(() => CompleteUsage());
        lockSequence.SetLoops(1);
    }

    private void CompleteUsage()
    {
        if (IndicateItemEffect.OnShowIndicator != null)
            IndicateItemEffect.OnShowIndicator(false);

        EnableSpinner();
        SaveCharacters.Instance.SavePlayerCharacters();
        RemoveItemFromInventory();
    }
}
