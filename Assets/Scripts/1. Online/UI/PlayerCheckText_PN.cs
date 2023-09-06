using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerCheckText_PN : MonoBehaviour
{

    public CharCursor_PN owner;
    public Image checkBox;
    public Image checkMark;
    public TextMeshProUGUI playerText;

    // Start is called before the first frame update
    void Start()
    {
        checkBox.color = owner.cursorColor;
        checkMark.color = owner.cursorColor;
        playerText.color = owner.cursorColor;

        playerText.text = ("Player " + owner.playerIndex.ToString());
        checkMark.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        checkMark.gameObject.SetActive(owner.selectDone);
        
    }
}
