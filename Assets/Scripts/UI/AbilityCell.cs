using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityCell : MonoBehaviour
{
    public Ability prefab;
    public Sprite badge;
    public bool toggle;
    public Image buttonImg;
    public Image checkImg;
    public Image badgeObj;
    public float alpha;
    public string ability_name;
    // Start is called before the first frame update
    void Start()
    {
        badge = prefab.badge;
        badgeObj.sprite = badge;
        alpha = buttonImg.color.a;
        ability_name = prefab.ability_name;
    }

    // Update is called once per frame
    void Update()
    {
        checkImg.gameObject.SetActive(toggle);
        if (!toggle)
        {
            buttonImg.color = new Color(buttonImg.color.r, buttonImg.color.b, buttonImg.color.g, 0f);
        }
        else
        {
            buttonImg.color = new Color(buttonImg.color.r, buttonImg.color.b, buttonImg.color.g, alpha);
        }

    }

   

    public void abilityToggle()
    {
        toggle = !toggle;
    }


}
