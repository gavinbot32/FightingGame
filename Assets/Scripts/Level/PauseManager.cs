using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseManager : MonoBehaviour
{

    GameManager manager;
    public GameObject canvas;
    private void Awake()
    {
        Time.timeScale = 1.0f;
         manager = FindObjectOfType<GameManager>();
    }

    public void onPause()
    {
        Time.timeScale = 0f;
        canvas.SetActive(true);
        manager.timerObj.SetActive(false);
        manager.playerContainerParent.gameObject.SetActive(false);
    }
    public void onResume()
    {
        Time.timeScale = 1.0f;
        canvas.SetActive(false);
        manager.timerObj.SetActive(true);
        manager.playerContainerParent.gameObject.SetActive(true);
    }
    
    public void onMainMenu()
    {
        SceneManager.LoadScene("Level_Select");
    }

    public void onOptions()
    {
        print("Options menu");
    }
}
