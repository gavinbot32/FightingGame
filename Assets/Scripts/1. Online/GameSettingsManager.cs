using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingsManager : MonoBehaviour
{

    public int inputIndex;
    public static GameSettingsManager instance;
    public string[] inputList;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(instance);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
