using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{

    static PopupManager instance;

    public GameObject panel;
    public static bool check;

    private void Awake()
    {
        instance = this;

        if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void closePanel()
    {
        panel.SetActive(false);
        check = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        panel.SetActive(!check);
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
