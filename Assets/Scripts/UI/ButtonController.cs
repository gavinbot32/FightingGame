using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{

    public GameObject panel;
    public Button onlineButton;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (onlineButton != null)
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                onlineButton.interactable = false;
            }
            else
            {
                onlineButton.interactable = true;
            }
        }
    }

    public void onOptionClick()
    {
        panel.SetActive(!panel.activeSelf);
    }
    public void closePanel()
    {
        panel.SetActive(false);
    }
    public void loadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
    public void loadScene(string sceneName)
    {
        if(sceneName == "OnlineMenu")
        {
            if(NetworkManager.instance != null)
            {
                Destroy(NetworkManager.instance.gameObject);
            }
        }
        SceneManager.LoadScene(sceneName);
    }
    public void quitPressed() {
        Application.Quit();
    }
}
