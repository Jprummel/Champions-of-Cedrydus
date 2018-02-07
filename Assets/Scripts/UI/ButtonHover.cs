using UnityEngine;
using UnityEngine.EventSystems;
using Utility;

public class ButtonHover : MonoBehaviour, ISelectHandler
{
    // When selected.
    public void OnSelect(BaseEventData eventData)
    {
        // Do something.
        SoundManager.instance.PlaySound(SoundsDatabase.AudioClips["ButtonHoverSound"], volume: 0.1f, pitch: Random.Range(0.99f, 1.01f));
    }
}
