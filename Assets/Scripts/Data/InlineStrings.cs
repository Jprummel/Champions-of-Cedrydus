using UnityEngine;

public class InlineStrings : MonoBehaviour
{
    #region CARDSTRINGS
    public const string FIRSTTURN = "BattleTurnCards/FirstTurn";
    public const string SECONDTURN = "BattleTurnCards/SecondTurn";
    #endregion

    #region TAGS
    public const string PLAYERTAG = "Player";
    public const string SPINNERTAG = "Spinner";
    public const string TURNMANAGERTAG = "TurnManager";
    public const string HEALINGPOPUPTAG = "HealingPopUp";
    public const string ONGOINGBATTLESTAG = "OngoingBattles";
    public const string MAPTAG = "Map";
    public const string SPAWNPLAYERSTAG = "SpawnPlayers";
    public const string BATTLECHARACTERSTAG = "BattleCharacters";
    public const string BATTLEACTIONSTAG = "BattleActions";
    public const string ATTACKERTAG = "Attacker";
    public const string DEFENDERTAG = "Defender";
    public const string TOWNTILETAG = "TownTile";
    public const string CHARACTERCREATIONMANAGER = "CharacterCreationManager";
    public const string BOSSTAG = "The Boss";
    #endregion

    #region SCENENAMES
    public const string MAPSCENE = "Map";
    public const string MAINMENUSCENE = "MainMenu";
    public const string BATTLESCENE = "BattleScene";
    public const string CHARACTERCREATIONSCENE = "CharacterCreation";
    public const string WINSCENE = "WinScene";
    #endregion

    #region PLAYERPREFS
    public const string PLAYERTURNINDEX = "PlayerIndex";
    public const string ISNEWGAME = "IsNewGame";
    public const string ISDIALOGUEDONE = "IsDialogueDone";
    public const string ISBATTLEDIALOGUEDONE = "IsBattleDialogueDone";
    #endregion

    #region SERIALIZATION
    public const string CHARACTERSAVEFILE = "characterdata.dat";
    public const string ONGOINGBATTLESSAVEFILE = "OngoingBattles.dat";
    public const string MINIBOSSSAVEFILE = "MiniBossProgress.dat";
    public const string ENEMYTYPE = "Enemy";
    #endregion
}