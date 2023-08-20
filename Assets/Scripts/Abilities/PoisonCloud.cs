using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonCloud : MonoBehaviour
{


    public PoisonEmiter abilParent;
    public GameObject sprite;
    public Color[] color_list;
    private SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        sr.color = color_list[Random.Range(0,color_list.Length+1)];
        float size = Random.Range(0.25f, 0.5f);
        sprite = GetComponentInChildren<SpriteRenderer>().gameObject;
        sprite.transform.localScale = new Vector2(size, size);
        sprite.transform.localPosition = new Vector3(sprite.transform.localPosition.x, 0, sprite.transform.localPosition.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {

        if (other.gameObject != abilParent.owner.gameObject)
        {
            if (other.CompareTag("Player"))
            {
                abilParent.conditionSet(other.gameObject.GetComponentInChildren<PlayerController>());
            }

        }
    }
   

}
