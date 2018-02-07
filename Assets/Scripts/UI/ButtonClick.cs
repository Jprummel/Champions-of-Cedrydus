using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour {

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() => ClickedButton() );
    }    

    public void ClickedButton()
    {
        Sequence buttonSequence = DOTween.Sequence();
        buttonSequence.Append(transform.DOScale(1.1f, 0.3f));
        buttonSequence.Append(transform.DOScale(1, 0.3f));
    }
}
