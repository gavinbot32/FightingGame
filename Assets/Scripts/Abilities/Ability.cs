using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    [Header("UI Stuff")]
    public string ability_name;
    public Sprite badge;
    public string description;

    public float damage;
    public PlayerController owner;
    public Vector2 spawnOffset;
    public float lifetime;
    public Rigidbody2D rig;
    public int speed;
    public float rof;
    public AudioSource audio;
    public AudioClip[] playlist;
    public Condition condition;

    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        audio = GetComponent<AudioSource>();
    }



    public void conditionSet(PlayerController player)
    {
        if (condition != null)
        {
            bool hasCon = false;
            foreach (Condition con in player.gameObject.GetComponentsInChildren<Condition>())
            {
                if (con.name == condition.name)
                {
                    hasCon = true;
                }
            }
            if (hasCon == false)
            {
                player.setCondition(condition);
            }
        }
    }

    public void lifeCheck()
    {
        if(lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
