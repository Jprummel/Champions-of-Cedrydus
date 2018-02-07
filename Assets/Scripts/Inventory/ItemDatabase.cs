using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Inventory;

public class ItemDatabase : MonoBehaviour
{
    [SerializeField] private List<GameObject> _databaseItems = new List<GameObject>();

    public static Dictionary<string, GameObject> DatabaseItems = new Dictionary<string, GameObject>();

    private void Awake()
    {
        BuildItemDatabase();
    }

    private void BuildItemDatabase()
    {
        DatabaseItems = new Dictionary<string, GameObject>();
        for (int i = 0; i < _databaseItems.Count; i++)
        {
            DatabaseItems.Add(_databaseItems[i].name, _databaseItems[i]);
        }
    }
}