using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerControlUI : MonoBehaviour
{
   
    public Image powBadge;

    public Image mainAbility;
    public Image meleeAbility;

    public Sprite r_key;
    public Sprite e_key;

    public TextMeshProUGUI ability_desc;

    public Transform mainContainer;

    public void intialized(Sprite powSprite, bool keyboard, string description)
    {
     
        powBadge.sprite = powSprite;
        ability_desc.text = description;
        if (keyboard)
        {
            mainAbility.sprite = r_key;
            meleeAbility.sprite = e_key;
        }

        mainContainer.gameObject.active = false;
        

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
