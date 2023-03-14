using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmiteBolt : MonoBehaviour
{
    private Smite owner;
    
    private void Awake()
    {
        owner = GetComponentInParent<Smite>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            owner.onHit(collision.GetComponent<PlayerController>(),transform.position);

        }
    }

}
