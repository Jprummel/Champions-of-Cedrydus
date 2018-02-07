using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ShowCharacter : MonoBehaviour {

    public delegate void ShowCharacterEvent();
    public static ShowCharacterEvent OnCreateNextCharacter;
    public static ShowCharacterEvent FinishTweenEarly;

    [SerializeField] private List<Sprite> _characterSprites = new List<Sprite>();
    private Image _characterImage;
    private Sprite _imageToSwapTo;

    private void OnEnable()
    {
        OnCreateNextCharacter += ResetShownCharacter;
        FinishTweenEarly += FinishTween;
    }

    private void Awake()
    {
        _characterImage = GetComponent<Image>();
        _imageToSwapTo = _characterSprites[0];
    }

    public void ShowCharacterImage(int characterToShow)
    {
        SwitchCharacter(characterToShow);
    }

    void ResetShownCharacter()
    {
        SwitchCharacter(0);
    }

    void SwitchCharacter(int spriteIndex)
    {
        _imageToSwapTo = _characterSprites[spriteIndex];
        Sequence switchSequence = DOTween.Sequence();
        switchSequence.Append(_characterImage.DOFillAmount(0, 0.4f));
        switchSequence.AppendInterval(0.6f);
        switchSequence.AppendCallback(SwitchImage);
        switchSequence.Append(_characterImage.DOFillAmount(1, 0.4f));
    }

    void SwitchImage()
    {
        _characterImage.sprite = _imageToSwapTo;
        _characterImage.SetNativeSize();
    }

    public void FinishTween()
    {
        StopAllCoroutines();
        _characterImage.sprite = _imageToSwapTo;
        _characterImage.fillAmount = 1;
    }

    private void OnDisable()
    {
        OnCreateNextCharacter -= ResetShownCharacter;
        FinishTweenEarly -= FinishTween;
    }
}