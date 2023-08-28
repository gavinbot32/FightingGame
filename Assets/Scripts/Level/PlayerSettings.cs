using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlayerSettings", order = 1)]
public class PlayerSettings : ScriptableObject
{
    [Header("Player Refs")]
    public Color[] player_colors;
    public Sprite[] player_skins;
    public string[] player_strings;
    public Sprite[] player_badges;


    [Header("Ability Refs")]
    public GameObject[] attackPrefabs;
    public GameObject[] passivePrefabs;

}