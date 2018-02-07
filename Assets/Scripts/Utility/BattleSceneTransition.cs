using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

[System.Serializable]
public class BattleTransition
{
    public float Distort;
    public Texture2D TransitionTexture;
}

public enum Transition
{
    None,
    Normal,
    Reverse
}

public class BattleSceneTransition : MonoBehaviour
{
    public delegate void OnTransitionStart(int transitionNumber = 0);
    public static OnTransitionStart TransitionIn;
    public static OnTransitionStart TransitionOut;

    public delegate void OnTransitionFinish();
    public static OnTransitionFinish TransitionCallback;

    [SerializeField] private float _transitionDuration = 2f;
    [SerializeField] private int _whichTransition;
    [SerializeField] private List<BattleTransition> _transitions;
    [SerializeField] private Material _transitionMaterial;

    [SerializeField] private Transition _transitionOnAwake;

    private void Awake()
    {
        switch (_transitionOnAwake)
        {
            case Transition.None:
                break;
            case Transition.Normal:
                StartTransition();
                break;
            case Transition.Reverse:
                ReverseTransition();
                break;
        }
    }

    private void OnEnable()
    {
        TransitionIn += StartTransition;
        TransitionOut += ReverseTransition;
    }

    private void StartTransition(int textureIndex = 0)
    {
        _transitionMaterial.SetFloat("_Cutoff", 0f);
        _transitionMaterial.SetTexture("_TransitionTex", _transitions[textureIndex].TransitionTexture);
        _transitionMaterial.SetFloat("_Distort", _transitions[textureIndex].Distort);

        Sequence transitionSequence = DOTween.Sequence();

        if (TransitionCallback != null)
        {
            transitionSequence.OnComplete(() => TransitionCallback());
        }

        transitionSequence.Append(_transitionMaterial.DOFloat(1f, "_Cutoff", _transitionDuration).SetEase(Ease.InOutQuint));
    }

    private void ReverseTransition(int textureIndex = 0)
    {
        _transitionMaterial.SetFloat("_Cutoff", 1f);
        _transitionMaterial.SetTexture("_TransitionTex", _transitions[textureIndex].TransitionTexture);
        _transitionMaterial.SetFloat("_Distort", _transitions[textureIndex].Distort);

        Sequence transitionSequence = DOTween.Sequence();

        if(TransitionCallback != null)
            transitionSequence.OnComplete(() => TransitionCallback());

        transitionSequence.Append(_transitionMaterial.DOFloat(0f, "_Cutoff", _transitionDuration).SetEase(Ease.InOutQuint));
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (_transitionMaterial != null)
            Graphics.Blit(src, dst, _transitionMaterial);
    }

    private void OnDisable()
    {
        _transitionMaterial.SetFloat("_Cutoff", 0f);
        TransitionIn -= StartTransition;
        TransitionOut -= ReverseTransition;
        TransitionCallback = null;
    }
}