using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ButtonController : MonoBehaviour
{

    public GameObject panel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        SceneManager.LoadScene(sceneName);
    }
    public void quitPressed() {
        Application.Quit();
    }
}
