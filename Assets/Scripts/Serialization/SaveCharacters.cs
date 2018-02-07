/*
	SaveCharacters.cs
	Created 11/10/2017 9:35:28 AM
	Project DriveBy RPG by BaseGames
*/
using Data;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using PlayerCharacters;

namespace Serialization
{
    public class SaveCharacters : MonoBehaviour
    {
        private static SaveCharacters s_Instance = null;

        public static SaveCharacters Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = FindObjectOfType(typeof(SaveCharacters)) as SaveCharacters;
                }

                if (s_Instance == null)
                {
                    GameObject obj = new GameObject("SaveCharacters");
                    s_Instance = obj.AddComponent(typeof(SaveCharacters)) as SaveCharacters;
                }

                return s_Instance;
            }

            set { }
        }

        void OnApplicationQuit()
        {
            s_Instance = null;
        }

        private void Awake()
        {
            if (s_Instance != null && s_Instance != this)
            {
                Destroy(gameObject);
            }

            s_Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void SavePlayerCharacters()
        {

            List<PlayerCharacter> players = new List<PlayerCharacter>();
            Dictionary<string, Stats> playerStats = new Dictionary<string, Stats>();

            foreach (PlayerCharacter player in GameObject.FindObjectsOfType(typeof(PlayerCharacter)))
            {
                players.Add(player);
            }

            for (int i = 0; i < players.Count; i++)
            {
                Stats statList = new Stats
                {
                    CharacterName = players[i].CharacterName,
                    ClassName = players[i].ClassName,
                    Attack = players[i].Attack,
                    Defense = players[i].Defense,
                    Tech = players[i].Tech,
                    Speed = players[i].Speed,
                    CurrentHP = players[i].CurrentHP,
                    MaxHP = players[i].MaxHP,

                    BonusAttack = players[i].BonusAttack,
                    BonusDefense = players[i].BonusDefense,
                    BonusTech = players[i].BonusTech,
                    BonusSpeed = players[i].BonusSpeed,
                    BonusMaxHP = players[i].BonusMaxHP,

                    Level = players[i].Level,
                    Gold = players[i].Gold,
                    CurrentXP = players[i].CurrentXP,
                    RequiredXP = players[i].RequiredXP,
                    TotalEarnedXP = players[i].TotalXPEarned,

                    TurnsBuffed = players[i].TurnsBuffed,
                    TurnsDebuffed = players[i].TurnsDebuffed,
                    TurnsMovementImpaired = players[i].TurnsMovementImpaired,
                    TurnsInventoryLocked = players[i].TurnsInventoryLocked,
                    TurnsDead = players[i].TurnsDead,
                    KnockedOut = players[i].KnockedOut,

                    PositionInMap = players[i].gameObject.GetComponent<PlayerMovement>().PositionOnMap,
                    BoardPosition = players[i].BoardPosition,

                    Inventory = players[i]._PlayerInventory
                };
                if(PlayersInGame.PlayersDict != null)
                {
                    if (!PlayersInGame.PlayersDict.ContainsKey(players[i].CharacterName)) // If dictionary did not contain this character / stats
                    {
                        PlayersInGame.PlayersDict.Add(players[i].CharacterName, statList); //Add entry to dictionary, characters name is key and stat lists the value
                    }
                    else if (PlayersInGame.PlayersDict.ContainsKey(players[i].CharacterName))
                    {
                        PlayersInGame.PlayersDict[players[i].CharacterName] = statList; //Override old stat values if key already existed
                    }
                }
                else
                {
                    if (!playerStats.ContainsKey(players[i].CharacterName)) // If dictionary did not contain this character / stats
                    {
                        playerStats.Add(players[i].CharacterName, statList); //Add entry to dictionary, characters name is key and stat lists the value
                    }
                    else if (playerStats.ContainsKey(players[i].CharacterName))
                    {
                        playerStats[players[i].CharacterName] = statList; //Override old stat values if key already existed
                    }
                    PlayersInGame.PlayersDict = playerStats;
                }
            }
            Serializer.Save("characterdata.dat", PlayersInGame.PlayersDict);
        }
        
        public void LoadPlayerCharacters()
        {
            SpawnPlayers.HasLoaded = true;
            List<PlayerCharacter> players = new List<PlayerCharacter>();
            PlayersInGame.PlayersDict = Serializer.Load<Dictionary<string, Stats>>("characterdata.dat");

            foreach (PlayerCharacter player in GameObject.FindObjectsOfType(typeof(PlayerCharacter)))
            {
                players.Add(player);
            }

            if (PlayersInGame.PlayersDict != null)
            {
                foreach (var key in PlayersInGame.PlayersDict.Keys.ToList())
                {
                    for (int i = 0; i < players.Count; i++)
                    {
                        if (players[i].CharacterName == key)
                        {
                            //General stats
                            players[i].CharacterName    = PlayersInGame.PlayersDict[key].CharacterName;
                            players[i].ClassName        = PlayersInGame.PlayersDict[key].ClassName;
                            players[i].Level            = PlayersInGame.PlayersDict[key].Level;
                            players[i].Gold             = PlayersInGame.PlayersDict[key].Gold;
                            players[i].CurrentXP        = PlayersInGame.PlayersDict[key].CurrentXP;
                            players[i].RequiredXP       = PlayersInGame.PlayersDict[key].RequiredXP;
                            players[i].TotalXPEarned    = PlayersInGame.PlayersDict[key].TotalEarnedXP;

                            //Combat stats
                            players[i].Attack       = PlayersInGame.PlayersDict[key].Attack;
                            players[i].Defense      = PlayersInGame.PlayersDict[key].Defense;
                            players[i].Tech         = PlayersInGame.PlayersDict[key].Tech;
                            players[i].Speed        = PlayersInGame.PlayersDict[key].Speed;
                            players[i].CurrentHP    = PlayersInGame.PlayersDict[key].CurrentHP;
                            players[i].MaxHP        = PlayersInGame.PlayersDict[key].MaxHP;

                            //Bonus stats
                            players[i].BonusAttack = PlayersInGame.PlayersDict[key].BonusAttack;
                            players[i].BonusDefense = PlayersInGame.PlayersDict[key].BonusDefense;
                            players[i].BonusTech = PlayersInGame.PlayersDict[key].BonusTech;
                            players[i].BonusSpeed = PlayersInGame.PlayersDict[key].BonusTech;
                            players[i].BonusMaxHP = PlayersInGame.PlayersDict[key].BonusMaxHP;

                            //Buffs / Debuffs and effects
                            players[i].TurnsBuffed = PlayersInGame.PlayersDict[key].TurnsBuffed;
                            players[i].TurnsDebuffed = PlayersInGame.PlayersDict[key].TurnsDebuffed;
                            players[i].TurnsMovementImpaired = PlayersInGame.PlayersDict[key].TurnsMovementImpaired;
                            players[i].TurnsInventoryLocked = PlayersInGame.PlayersDict[key].TurnsInventoryLocked;
                            players[i].TurnsDead = PlayersInGame.PlayersDict[key].TurnsDead;
                            players[i].KnockedOut = PlayersInGame.PlayersDict[key].KnockedOut;

                            players[i].gameObject.GetComponent<PlayerMovement>().PositionOnMap = PlayersInGame.PlayersDict[key].PositionInMap;
                            players[i].BoardPosition = PlayersInGame.PlayersDict[key].BoardPosition;

                            players[i]._PlayerInventory = PlayersInGame.PlayersDict[key].Inventory;
                        }
                    }
                }
            }
        }

        public void ClearSave()
        {
            if(PlayersInGame.PlayersDict != null)
            {
                PlayersInGame.PlayersDict.Clear();
            }
            Serializer.DeleteSave("characterdata.dat");
            Serializer.DeleteSave("OngoingBattles.dat");
            Serializer.DeleteSave(InlineStrings.MINIBOSSSAVEFILE);
        }
    }
}