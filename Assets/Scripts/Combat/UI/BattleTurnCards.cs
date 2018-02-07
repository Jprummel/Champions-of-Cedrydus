using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BattleTurnCards : MonoBehaviour {

    public delegate void OnCardSpawnComplete(bool activeness);
    public static OnCardSpawnComplete OnCardAnimationsComplete;
    public delegate void OnSwitchCards();
    public static OnSwitchCards onSwitchCards;
    public delegate void OnConfirmCards();
    public static OnConfirmCards onConfirmCards;

    [SerializeField] private GameObject _initiator;
    [SerializeField] private List<GameObject> _switchUI = new List<GameObject>();
    [SerializeField] private List<BattleTurnCard> _cards = new List<BattleTurnCard>();
    [SerializeField] private List<Transform> _positions = new List<Transform>();

    //private Tween _rotateTween;

    private float _switchTime = 0.3f;

    private bool _canSwitch = false;

    private void Awake()
    {
        SetFirstTurnCard();

        onConfirmCards += ShowCards;
        OnCardAnimationsComplete += ToggleSwitchUI;
        onSwitchCards += SwitchCards;
    }

    void SetFirstTurnCard()
    {
        _cards[Random.Range(0, _cards.Count)].First = true;
    }

    public void ShowCards()
    {
        onSwitchCards = null;
        onConfirmCards = null;
        for (int i = 0; i < _cards.Count; i++)
        {
            StartCoroutine(Rotate(i));
        }
    }

    IEnumerator Rotate(int i)
    {
        TweenCallback callback = () => RotateCallback(i);

       /* _rotateTween = */_cards[i].transform.DORotate(new Vector3(0, 90, 0), 0.5f).OnComplete(callback);
        yield return new WaitForSeconds(2.5f);
        gameObject.SetActive(false);
        for (int j = 0; j < _cards.Count; j++)
        {
            if (_cards[j].First)
            {
                if(_cards[j].transform.position == _positions[0].position)
                {
                    ShowCombatActions.OnShowCombatUI(true);
                }
                else
                {
                    ShowCombatActions.OnShowCombatUI(false);
                }
            }
        }
    }

    private void RotateCallback(int i)
    {
        _cards[i].ShowCard();
        _cards[i].transform.DORotate(Vector3.zero, 0.5f);
    }

    public void SwitchCards()
    {
        StartCoroutine(SwitchCardPositions());
    }

    private void ToggleSwitchUI(bool activeness)
    {
        for (int i = 0; i < _switchUI.Count; i++)
        {
            _switchUI[i].SetActive(activeness);
        }
        _canSwitch = true;
    }

    IEnumerator SwitchCardPositions()
    {
        if (_canSwitch)
        {
            _canSwitch = false;
            RectTransform c1 = _cards[0].GetComponent<RectTransform>();
            RectTransform c2 = _cards[1].GetComponent<RectTransform>();

            Vector2 aPos1 = c1.anchoredPosition;
            Vector2 aPos2 = c2.anchoredPosition;

            for (int i = 0; i < _cards.Count; i++)
            {
                _cards[i].First ^= true; // = !_cards[i].First;

                if (i == 0)
                {
                    _cards[i].GetComponent<RectTransform>().DOAnchorPosX(aPos2.x, _switchTime);
                }
                else
                {
                    _cards[i].GetComponent<RectTransform>().DOAnchorPosX(aPos1.x, _switchTime);
                }
            }

            yield return new WaitForSeconds(_switchTime + 0.3f);
            _canSwitch = true;
        }
    }

    private void OnDisable()
    {
        onConfirmCards -= ShowCards;
        OnCardAnimationsComplete -= ToggleSwitchUI;
        onSwitchCards -= SwitchCards;
    }
}