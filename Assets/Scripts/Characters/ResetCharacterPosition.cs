using PlayerCharacters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class ResetCharacterPosition : MonoBehaviour {

    //public delegate void ResetPlayerPosition(Transform target);
    //public static event ResetPlayerPosition OnResetPosition;

    public static string TargetName;

    private void Start()
    {
        StartCoroutine(LateStart());
    }

    IEnumerator LateStart()
    {
        yield return new WaitForEndOfFrame();

        foreach (PlayerCharacter player in GameObject.FindObjectsOfType(typeof(PlayerCharacter)))
        {
            if (player.CharacterName == TargetName)
            {
                PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();

                playerMovement.PositionOnMap = new int2(0, 6);
                playerMovement.transform.position = playerMovement.MapScript.BoardToWorldPos(playerMovement.PositionOnMap);

                TargetName = string.Empty;
                break;
            }
        }
    }
}
