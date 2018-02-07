using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DisableCursor : MonoBehaviour
{
	void Start ()
    {
#if !UNITY_EDITOR
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
#endif
    }

    void Update()
    {
        if (EventSystem.current != null)
        {
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                EventSystem.current.SetSelectedGameObject(EventSystem.current.firstSelectedGameObject);
            }
        }
    }
}