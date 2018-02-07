/*
	OngoingBattles.cs
	Created 12/6/2017 1:39:15 PM
	Project Boardgame by Base Games
*/
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Combat
{
    [System.Serializable]
    public class Battle
    {
        public string BattleID;
        public GameObject Character1;
        public GameObject Character2;
    }

	public class OngoingBattles : MonoBehaviour 
	{
        public Dictionary<string, Battle> ongoingBattles = new Dictionary<string, Battle>();
        public static Dictionary<string, float> EnemiesInBattleHP = new Dictionary<string, float>();

        public void RemoveBattle(string battleID)
        {
            ongoingBattles.Remove(battleID);
            if (HasEnemy(battleID))
            {
                EnemiesInBattleHP.Remove(battleID);
            }
        }

        public void RemoveBattlesFromTarget(PlayerCharacter target)
        {
            List<string> ongoingBattleKeys = ongoingBattles.Keys.ToList();
            foreach (var key in ongoingBattleKeys)
            {
                Character key1 = ongoingBattles[key].Character1.GetComponent<Character>();
                Character key2 = ongoingBattles[key].Character2.GetComponent<Character>();

                if (key1.CharacterName != null && key1.CharacterName == target.CharacterName)
                {
                    RemoveBattle(ongoingBattles[key].BattleID);
                }
                else if(key2.CharacterName != null && key2.CharacterName == target.CharacterName)
                {
                    RemoveBattle(ongoingBattles[key].BattleID);
                }
            }
        }

        public void AddBattle(GameObject char1, GameObject char2, string battleID)
        {
            Battle newBattle = new Battle()
            {
                BattleID = battleID,
                Character1 = char1,
                Character2 = char2
            };

            ongoingBattles.Add(newBattle.BattleID, newBattle);
        }

        public bool HasEnemy(string enemyKey)
        {
            if (EnemiesInBattleHP != null)
            {
                if (EnemiesInBattleHP.ContainsKey(enemyKey))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public Battle GetBattle(string battle)
        {
            foreach (var key in ongoingBattles)
            {
                if(battle == key.Value.BattleID)
                {
                    return key.Value;
                }
            }
            return null;
        }

        public List<GameObject> LoadBattleCharacters(string battleID)
        {
            List<GameObject> characters = new List<GameObject>();
            Battle battleToLoad = ongoingBattles[battleID];

            characters.Add(battleToLoad.Character1);
            characters.Add(battleToLoad.Character2);

            return characters;
        }
	}
}