using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smite : Ability
{

    public Transform target;
    public float startY;
    public GameManager manager;
    public Collider2D col;
    private int hitOrder;
    public GameObject particleSystem;
    private void Awake()
    {
        audio = GetComponent<AudioSource>();
        manager = FindObjectOfType<GameManager>();
    }


    // Start is called before the first frame update
    void Start()
    {

        float closest = 100;
        foreach(PlayerController player in manager.players)
        {
            if (player != owner.GetComponent<PlayerController>())
            {
                if (Vector3.Distance(owner.transform.position, player.transform.position) < closest)
                {
                    closest = Vector3.Distance(owner.transform.position, player.transform.position);
                    target = player.transform;
                }
            }
        }

        transform.position = new Vector3(target.transform.position.x, startY, transform.position.z);
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onHit(PlayerController player,Vector3 pos)
    {
        audio.PlayOneShot(playlist[Random.Range(0, 2)]);

        pos = new Vector3(pos.x, pos.y - 0.5f, pos.z);
        hitOrder++;
        player.takeDamage(damage / hitOrder);
        player.currentAttacker = owner;
        conditionSet(player);
        GameObject p = Instantiate(particleSystem, pos, Quaternion.identity);
        Destroy(p, 2);
    }

}
