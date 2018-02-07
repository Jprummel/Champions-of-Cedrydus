using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WaveText : MonoBehaviour {

    public delegate void CallbackDelegate();
    public static CallbackDelegate OnTextComplete;
    public static bool IsTweening;
    public static GameObject WaveObject;
    private TweenCallback _sequenceCallback;
    private static Sequence _waveSequence;
    private static bool _canKill;
    
    [SerializeField]private List<Text> _winTextfields = new List<Text>();
    [SerializeField]private List<Text> _toBeContinuedTextfields = new List<Text>();
    [SerializeField]private List<Text> _levelUpTextfields = new List<Text>();

    private void Awake()
    {
        BattleStateMachine.OnBattleWon += MoveWinText;
        BattleStateMachine.OnBattleComplete += MoveDrawText;
        AddExperience.OnLevelUp += MoveLevelUpText;
        WaveObject = this.gameObject;
    }

    void ClearLists()
    {
        for (int i = 0; i < _winTextfields.Count; i++)
        {
            _winTextfields[i].text = string.Empty;
        }

        for (int i = 0; i < _toBeContinuedTextfields.Count; i++)
        {
            _toBeContinuedTextfields[i].text = string.Empty;
        }

        for (int i = 0; i < _levelUpTextfields.Count; i++)
        {
            _levelUpTextfields[i].text = string.Empty;
        }
    }

    private void MoveWinText()
    {
        _sequenceCallback = ()=>PostCombatRewardsScreen.OnPostCombatEvent();
        StartCoroutine(SplitString("Win!", _winTextfields, _sequenceCallback));
    }

    public static void KillSequence()
    {
        if (_canKill)
        {
            DOTween.Kill(1, true);
        }
    }
    
    private void MoveDrawText()
    {
        _sequenceCallback = () => BattleStateMachine.OnExit();
        StartCoroutine(SplitString("ToBeContinued!", _toBeContinuedTextfields, _sequenceCallback));
    }

    private void MoveLevelUpText()
    {
        _sequenceCallback += () => AddExperience.ActivateStatAllocation();
        _sequenceCallback += () => StatPointAllocation.OnStatAllocation(BattleStateMachine.CharacterThatWon);
        StartCoroutine(SplitString("LevelUp!", _levelUpTextfields, _sequenceCallback));
    }


    IEnumerator SplitString(string text, List<Text> list, TweenCallback callback = null)
    {
        bool addedCallbacks = false;
        _canKill = false;
        List<RectTransform> rts = new List<RectTransform>();
        
        callback += ClearLists;
        callback += () => _sequenceCallback = null;
        callback += () => IsTweening = false;

        for (int i = 0; i < list.Count; i++)
        {
            list[i].text = text[i].ToString();
            RectTransform rt = list[i].GetComponent<RectTransform>();
            rts.Add(rt);
            yield return new WaitForSeconds(0.02f);
        }

        IsTweening = true;

        for (int i = 0; i < rts.Count; i++)
        {
            _waveSequence = DOTween.Sequence();
            _waveSequence.SetId(1);
            _waveSequence.AppendInterval(0.5f);
            _waveSequence.Append(rts[i].DOAnchorPosY(35, 0.2f).SetEase(Ease.InFlash));
            _waveSequence.Append(rts[i].DOAnchorPosY(0, 0.2f).SetEase(Ease.OutFlash));
            _waveSequence.AppendInterval(0.3f);
            _waveSequence.SetLoops(3);

            if (!addedCallbacks)
            {
                addedCallbacks = true;
                _waveSequence.OnKill(callback);
                _waveSequence.OnComplete(callback);
            }
            yield return new WaitForSeconds(0.05f);
        }
        _canKill = true;
    }

    private void OnDisable()
    {
        BattleStateMachine.OnBattleWon -= MoveWinText;
        BattleStateMachine.OnBattleComplete -= MoveDrawText;
        AddExperience.OnLevelUp -= MoveLevelUpText;
        IsTweening = false;
    }
}