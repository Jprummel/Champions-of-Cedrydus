using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Serialization;

public class SpawnPlayers : MonoBehaviour {

    public static bool HasLoaded;
    [SerializeField] private List<GameObject> _playerObjects = new List<GameObject>();
    private GameObject _player;

    private void Awake()
    {
        SpawnThePlayers();
    }

    private void SpawnThePlayers()
    {
        if (PlayersInGame.PlayersDict != null && PlayersInGame.PlayersDict.Count > 0)
        {
            foreach (var key in PlayersInGame.PlayersDict.Keys.ToList())
            {
                switch (PlayersInGame.PlayersDict[key].ClassName)
                {
                    case ClassNames.JUGGERNAUT:
                        _player = Instantiate(_playerObjects[0]);
                        break; 
                    case ClassNames.BOUNTYHUNTER:
                        _player = Instantiate(_playerObjects[1]);
                        break;
                    case ClassNames.TECHNOMANCER:
                        _player = Instantiate(_playerObjects[2]);
                        break;
                    case ClassNames.ENGINEER:
                        _player = Instantiate(_playerObjects[3]);
                        break;
                    default:
                        Debug.LogError("Classname not found");
                        break;
                }
                _player.GetComponent<PlayerCharacter>().CharacterName = key;
                _player.transform.SetParent(transform);
            }

            SaveCharacters.Instance.LoadPlayerCharacters();
        }
        else
        {
            for (int i = 0; i < _playerObjects.Count; i++)
            {
                _player = Instantiate(_playerObjects[i]);
                _player.transform.SetParent(transform);
            }
        }
    }
}
