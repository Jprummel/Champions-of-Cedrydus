using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Serialization;
using Utility;

public class DefeatedMiniBosses : MonoBehaviour {

    private static DefeatedMiniBosses s_Instance = null;

    public static DefeatedMiniBosses Instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = FindObjectOfType(typeof(DefeatedMiniBosses)) as DefeatedMiniBosses;
            }

            if (s_Instance == null)
            {
                GameObject obj = new GameObject("SaveCharacters");
                s_Instance = obj.AddComponent(typeof(DefeatedMiniBosses)) as DefeatedMiniBosses;
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

    public void SaveMiniBoss(int2 position)
    {
        List<int2> DefeatedBossPositions = new List<int2>();
        if (LoadMiniBossData() != null)
            DefeatedBossPositions = LoadMiniBossData();

        DefeatedBossPositions.Add(position);

        Serializer.Save(InlineStrings.MINIBOSSSAVEFILE, DefeatedBossPositions);
    }

    public List<int2> LoadMiniBossData()
    {
        List<int2> DefeatedBossPositions = Serializer.Load<List<int2>>(InlineStrings.MINIBOSSSAVEFILE);

        return DefeatedBossPositions;
    }

    public bool DefeatedBoss(int2 position)
    {
        List<int2> DefeatedMiniBosses = LoadMiniBossData();

        if(DefeatedMiniBosses != null)
        {
            for (int i = 0; i < DefeatedMiniBosses.Count; i++)
            {
                if (position == DefeatedMiniBosses[i])
                {
                    return true;
                }
            }
        }
        return false;
    }
}