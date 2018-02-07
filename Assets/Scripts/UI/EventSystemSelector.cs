using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemSelector : MonoBehaviour {

    private EventSystem _eventSystem;
    [SerializeField] private GameObject _objectToSelect;

	void Awake () {
        _eventSystem = GetComponent<EventSystem>();
	}

    private void OnEnable()
    {
        _eventSystem.firstSelectedGameObject = _objectToSelect;
    }
}