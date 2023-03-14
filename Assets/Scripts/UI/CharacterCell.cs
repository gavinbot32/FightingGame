using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCell : MonoBehaviour
{

    public int index;
    public bool avaliable;
    public GameObject block_panel;
    // Start is called before the first frame update
    void Start()
    {
        block_panel.SetActive(false);
        if (!avaliable)
        {
            block_panel.SetActive(true);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
