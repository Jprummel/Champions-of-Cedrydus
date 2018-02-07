using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetGameStatus : MonoBehaviour
{
    public void ChangeGameStatus(int isNewGame)
    {
        PlayerPrefs.SetInt(InlineStrings.ISNEWGAME, isNewGame);
    }

    public void ChangeDialogueStatus(int isDialogueDone)
    {
        PlayerPrefs.SetInt(InlineStrings.ISDIALOGUEDONE, isDialogueDone);
        PlayerPrefs.SetInt(InlineStrings.PLAYERTURNINDEX, 0);
        PlayerPrefs.SetInt(InlineStrings.ISBATTLEDIALOGUEDONE, 0);
    }
}
