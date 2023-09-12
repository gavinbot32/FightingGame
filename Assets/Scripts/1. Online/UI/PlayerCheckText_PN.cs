using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class PlayerCheckText_PN : MonoBehaviour
{

    public CharCursor_PN owner;
    public Image checkBox;
    public Image checkMark;
    public TextMeshProUGUI playerText;
    public string playerName;
    public int index;
    private CharSelect_PN charSelect;

    private void Awake()
    {
        charSelect = FindObjectOfType<CharSelect_PN>();
    }

    // Start is called before the first frame update
    void Start()
    {
         
        checkMark.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        checkMark.gameObject.SetActive(owner.selectDone);

    }
    public void initialize(int index)
    {
        index = index;
        owner = charSelect.cursor_PNs[index];
        print(owner);
        checkBox.color = owner.cursorColor;
        checkMark.color = owner.cursorColor;
        playerText.color = owner.cursorColor;
        print("got this far");
        playerText.text = (playerName);

    }
}
