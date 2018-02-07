using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayerGuide : MonoBehaviour {
    
    [SerializeField]private EventSystem _eventSystem;
    [SerializeField] private GameObject _subjectButtons;
    [SerializeField] private GameObject _firstSubjectButton;
    [SerializeField] private GameObject _returnButton;
    private GameObject _guideToActivate;

    public void ChooseSubject(GameObject guideToActivate)
    {
        _guideToActivate = guideToActivate;
        _subjectButtons.SetActive(false);
        _guideToActivate.SetActive(true);
        _returnButton.SetActive(true);
        _eventSystem.SetSelectedGameObject(_returnButton);
    }

    public void ReturnToGuide()
    {
        _guideToActivate.SetActive(false);
        _returnButton.SetActive(false);
        _subjectButtons.SetActive(true);
        _eventSystem.SetSelectedGameObject(_firstSubjectButton);
    }
}
