using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SwitchHighlighter : MonoBehaviour {

    [SerializeField]private List<Image> _highlightObjects = new List<Image>();
    [SerializeField] private Sprite _highlightSprite;
    [SerializeField] private Sprite _normalSprite;
    private Sequence _highlightSqn;

	void OnEnable()
    {
        StartCoroutine(SwitchHighlights());
    }

    IEnumerator SwitchHighlights()
    {
        _highlightObjects[0].sprite = _highlightSprite;
        _highlightObjects[1].sprite = _normalSprite;
        yield return new WaitForSeconds(0.5f);
        _highlightObjects[0].sprite = _normalSprite;
        _highlightObjects[1].sprite = _highlightSprite;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(SwitchHighlights());
    }
}