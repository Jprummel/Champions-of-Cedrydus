using DG.Tweening;
using Inventory;
using UI;

public class SlowHack : UsableItem
{
    public override void UseItem()
    {
        PlayerTarget._TargetingMode = TargetingMode.Players;
        base.UseItem();
    }


    public override void ExecuteUsableEffect()
    {
        base.ExecuteUsableEffect();
        //Target player can only move 1 spot for 3 turns
        PlayerTarget.Target.TurnsMovementImpaired = 3;

        if (IndicateItemEffect.OnShowIndicator != null)
            IndicateItemEffect.OnShowIndicator(true, PlayerTarget.Target.CharacterName + " is slowed for 3 turns (can only move one space at a time).");

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