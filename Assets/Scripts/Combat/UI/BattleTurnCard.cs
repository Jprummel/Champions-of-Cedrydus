using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleTurnCard : MonoBehaviour {

    private Image _image;

    [SerializeField]private RectTransform _startPos;
    private RectTransform _rt;
    [SerializeField]private bool _first;
    public bool First
    {
        get { return _first; }
        set { _first = value; }
    }

    private void Start()
    {
        _image = GetComponent<Image>();
        _rt = GetComponent <RectTransform>();
        StartCoroutine(LateStart());
    }

    IEnumerator LateStart()
    {
        yield return new WaitForEndOfFrame();
        SpawnAnimation();
    }

    public void ShowCard()
    {
        if (_first)
            _image.sprite = Resources.Load<Sprite>(InlineStrings.FIRSTTURN);
        else
            _image.sprite = Resources.Load<Sprite>(InlineStrings.SECONDTURN);
    }

    void SpawnAnimation()
    {
        transform.DOScale(Vector2.one, 1f).SetEase(Ease.OutExpo).OnComplete(MoveToStartPos);
    }

    void MoveToStartPos()
    {
       _rt.DOAnchorPos(_startPos.anchoredPosition, 0.75f).SetEase(Ease.OutExpo).OnComplete(()=> BattleTurnCards.OnCardAnimationsComplete(true));
    }
}