using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class ShowRecoveryText : MonoBehaviour
{
    [SerializeField] private Text _recoveryText;

    private void OnEnable()
    {
        Character.OnUsedRecovery += ShowText;
    }

    private void OnDisable()
    {
        Character.OnUsedRecovery -= ShowText;
    }

    private void ShowText(string playerName)
    {
        _recoveryText.text = playerName + " used Recovery!";

        Sequence showSequence = DOTween.Sequence();
        showSequence.Append(_recoveryText.DOFade(1f, .5f));
        showSequence.AppendInterval(3f);
        showSequence.Append(_recoveryText.DOFade(0f, .5f));

        showSequence.SetLoops(1);
	}
}