using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ShowActionText : MonoBehaviour {

    public delegate void ShowActionEvent(string option, Transform pos);
    public delegate void HideActionEvent();
    public static ShowActionEvent OnShowAction;
    public static HideActionEvent OnHideAction;

    [SerializeField]private Text _text;
    private static RectTransform _optionPosition;
    [SerializeField] private Image _background;

    private void OnEnable()
    {
        OnShowAction += OptionAppear;
        OnHideAction += HideOption;
    }

    private void Awake()
    {
        _optionPosition = GetComponent<RectTransform>();
    }

    public void OptionAppear(string option, Transform pos)
    {
        _background.enabled = true;
        _background.DOFade(1, 0.1f);
        _text.text = option + "!";
        _optionPosition.position = new Vector3(pos.position.x, pos.position.y + 3f, pos.position.z);
        
        _optionPosition.DOMoveY(pos.position.y + 2, 0.25f).SetEase(Ease.InQuart);
    }

    public void HideOption()
    {
        _background.enabled = false;
        //_background.DOFade(0, 0.1f);
        _text.text = string.Empty;
    }

    private void OnDisable()
    {
        OnShowAction -= OptionAppear;
        OnHideAction -= HideOption;
    }
}