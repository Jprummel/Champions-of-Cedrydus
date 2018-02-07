using UnityEngine;
using Inventory;
using DG.Tweening;

public class Repair : UsableItem
{

    public override void UseItem()
    {
        ExecuteUsableEffect();
    }

    public override void ExecuteUsableEffect()
    {
        base.ExecuteUsableEffect();
        //Half of players max hp is stored which is then used to heal the player
        int healingAmount = Mathf.RoundToInt(_user.MaxHP / 2);
        _user.CurrentHP += healingAmount;
        if(_user.CurrentHP > _user.MaxHP)
        {
            _user.CurrentHP = _user.MaxHP; // Makes sure player cant heal above his normal max hp
        }

        if (IndicateItemEffect.OnShowIndicator != null)
            IndicateItemEffect.OnShowIndicator(true, "Healed for " + healingAmount + " health.");

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

        RemoveItemFromInventory();
    }
}
