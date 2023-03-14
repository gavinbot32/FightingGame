using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WinManager : MonoBehaviour
{
    public Sprite winBadge;
    public Sprite[] badgeList;
    public int skinIndex;
    public Image winBadgeObj;
    // Start is called before the first frame update
    void Start()
    {
        skinIndex = PlayerPrefs.GetInt("skinIndex");
        PlayerPrefs.DeleteKey("skinIndex");
        winBadge = badgeList[skinIndex];
        winBadgeObj.sprite = winBadge;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
