using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PlayerContainerUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public Image healthBarFill;
    public Image chargeBarFill;
    public Image badge;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void intialized(Color color,Sprite badgeSprite)
    {
        badge.sprite = badgeSprite;
        scoreText.color = color;
        healthBarFill.color = color;
        
        scoreText.text = "0";
        healthBarFill.fillAmount = 1;
        chargeBarFill.fillAmount = 0;
    }

    public void updateHealthBar(int curHp, int maxHp)
    {
        healthBarFill.fillAmount = ((float)curHp / (float)maxHp);
    }
    public void updateChargeBar(int curCharge, int maxCharge)
    {
        chargeBarFill.fillAmount = ((float)curCharge / (float)maxCharge);
    }
    public void updateChargeBar(float curCharge, float maxCharge)
    {
        chargeBarFill.fillAmount = ((float)curCharge / (float)maxCharge);
    }
    public void updateScore(int score)
    {
        scoreText.text = score.ToString();
    }

}
