using System.Collections.Generic;
using UnityEngine;

public class EnemyDatabase : MonoBehaviour {

    [SerializeField] private List<GameObject> _enemies = new List<GameObject>();
    private static List<GameObject> enemies = new List<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < _enemies.Count; i++)
        {
            enemies.Add(_enemies[i]);
        }
    }

    public static GameObject ReturnEnemy(int minValue,int maxValue)
    {
        //Maxvalue should always be 1 higher than the enemy you want (random.range(0,5) for example takes a value between 0 and 5 and never reaches 5
        if(maxValue > enemies.Count)
        {
            maxValue = enemies.Count; //Failsafe, if maxvalue given is higher than there are enemies in the list, lower maxvalue
        }

        List<GameObject> correspondingEnemies = new List<GameObject>();
        correspondingEnemies.Add(enemies[minValue]);
        correspondingEnemies.Add(enemies[maxValue - 1]);

        int probability = Random.Range(0, 101);

        if(probability < 25)
        {
            return correspondingEnemies[1];
        }
        else
        {
            return correspondingEnemies[0];
        }
    }
}