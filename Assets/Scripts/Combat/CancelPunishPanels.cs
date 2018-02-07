using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class CancelPunishPanels : MonoBehaviour {

    [SerializeField] private List<GameObject> _itemSelection = new List<GameObject>();
    [SerializeField] private GameObject _firstSelected;
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown(InputStrings.CONTROLLER_B))
        {
            DeactivatePanels();
        }
    }

    void DeactivatePanels()
    {
        for (int i = 0; i < _itemSelection.Count; i++)
        {
            _itemSelection[i].SetActive(false); 
        }
        EventSystem.current.SetSelectedGameObject(_firstSelected);
    }
}
