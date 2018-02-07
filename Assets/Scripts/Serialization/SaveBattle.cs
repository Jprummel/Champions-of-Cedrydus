using System.Collections.Generic;
using UnityEngine;
using Combat;
using Serialization;

public class SaveBattle : MonoBehaviour
{

    private Enemy _enemyInBattle;
    float enemyHP;

    private static SaveBattle s_Instance = null;

    public static SaveBattle Instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = FindObjectOfType(typeof(SaveBattle)) as SaveBattle;
            }

            if (s_Instance == null)
            {
                GameObject obj = new GameObject("SaveCharacters");
                s_Instance = obj.AddComponent(typeof(SaveBattle)) as SaveBattle;
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

    public void SaveSpecificEnemy(string keyToSave, Enemy enemy)
    {
        enemyHP = enemy.CurrentHP;

        Dictionary<string, float> enemyInBattleHP = new Dictionary<string, float>();

        LoadEnemiesInBattle(keyToSave);

        if (OngoingBattles.EnemiesInBattleHP != null)
        {
            enemyInBattleHP = OngoingBattles.EnemiesInBattleHP;
        }
        if (!enemyInBattleHP.ContainsKey(keyToSave))
        {
            enemyInBattleHP.Add(keyToSave, enemyHP); // Add new battle
        }
        else
        {
            enemyInBattleHP[keyToSave] = enemyHP; //Overwrite HP value
        }
        OngoingBattles.EnemiesInBattleHP = enemyInBattleHP;
        Serializer.Save(InlineStrings.ONGOINGBATTLESSAVEFILE, OngoingBattles.EnemiesInBattleHP);
    }

    public void SaveEnemiesInBattle(string keyToSave, bool reset = false)
    {       
        foreach (Enemy enemy in GameObject.FindObjectsOfType(typeof(Enemy))) 
        {
            if (!reset)
                enemyHP = enemy.CurrentHP; //Sets the new hp value
            //else
                //enemyHP = enemy.MaxHP;
        }

        Dictionary<string, float> enemyInBattleHP = new Dictionary<string, float>();

        LoadEnemiesInBattle(keyToSave);

        if(OngoingBattles.EnemiesInBattleHP != null)
        {
            enemyInBattleHP = OngoingBattles.EnemiesInBattleHP;
        }


        foreach (Enemy enemy in GameObject.FindObjectsOfType(typeof(Enemy)))
        {
            if (!enemyInBattleHP.ContainsKey(keyToSave)) {
                enemyInBattleHP.Add(keyToSave, enemyHP); // Add new battle
            }
            else
            {
                enemyInBattleHP[keyToSave] = enemyHP; //Overwrite HP value
            }
        }
        OngoingBattles.EnemiesInBattleHP = enemyInBattleHP;
        Serializer.Save(InlineStrings.ONGOINGBATTLESSAVEFILE, OngoingBattles.EnemiesInBattleHP);
    }

    public void LoadEnemiesInBattle(string keyToLoad)
    {
        OngoingBattles.EnemiesInBattleHP = Serializer.Load<Dictionary<string, float>>(InlineStrings.ONGOINGBATTLESSAVEFILE);

        foreach (Enemy enemy in GameObject.FindObjectsOfType(typeof(Enemy)))
        {
            _enemyInBattle = enemy;
        }

        if(OngoingBattles.EnemiesInBattleHP != null)
        {
            if (OngoingBattles.EnemiesInBattleHP.ContainsKey(keyToLoad) && _enemyInBattle != null)
            {
                _enemyInBattle.CurrentHP = OngoingBattles.EnemiesInBattleHP[keyToLoad];
            }
        }
    }

    public void RemoveBattle(string keyToRemove)
    {
        SaveEnemiesInBattle(keyToRemove, true);

        if (OngoingBattles.EnemiesInBattleHP != null)
        {
            if (OngoingBattles.EnemiesInBattleHP.ContainsKey(keyToRemove))
            {
                OngoingBattles.EnemiesInBattleHP.Remove(keyToRemove);
            }
        }
    }
}