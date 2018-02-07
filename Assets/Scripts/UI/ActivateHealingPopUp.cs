using UnityEngine;

public class ActivateHealingPopUp : MonoBehaviour {

    [SerializeField] private GameObject _healingPopUp;
	
    public void EnablePopUp()
    {
        _healingPopUp.SetActive(true);
    }
}