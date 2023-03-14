using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelSelectController : MonoBehaviour
{

    public string[] scene_names;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void random_level()
    {
        string scene = scene_names[Random.Range(0, scene_names.Length)];
        SceneManager.LoadScene(scene);
    }
}
