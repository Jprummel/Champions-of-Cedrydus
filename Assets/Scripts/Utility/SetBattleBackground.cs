using UnityEngine;
using System.Collections;
 
public class SetBattleBackground : MonoBehaviour
{
    public static Biome BattleBiome = Biome.FOREST;

    [SerializeField] private Sprite[] _backgroundSprites;
    [SerializeField] private SpriteRenderer _backgroundSR;

    private void Start()
    {
        _backgroundSR.sprite = _backgroundSprites[(int)BattleBiome];
    }
}