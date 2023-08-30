using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelSelectController : MonoBehaviour
{

    public string[] scene_names;
    public PlayerSettings settings;
    public PlayerSettings tempSettings;
    public AbilityCell abilPrefab;
    public Transform abilParent;
    public List<AbilityCell> cells;
    public List<GameObject> abilityList;
    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject ability in settings.attackPrefabs)
        {
            GameObject cell = Instantiate(abilPrefab.gameObject, abilParent);
            cell.GetComponent<AbilityCell>().prefab = ability.GetComponent<Ability>();
            cells.Add(cell.GetComponent<AbilityCell>());
        }
        foreach(GameObject abil in settings.attackPrefabs)
        {
            abilityList.Add(abil);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadScene(string sceneName)
    {

        abilityCount();

        SceneManager.LoadScene(sceneName);
    }

    private void abilityCount()
    {
        foreach(AbilityCell cell in cells)
        {
            if (!cell.toggle)
            {
                foreach(GameObject abil in settings.attackPrefabs)
                {
                    if(cell.ability_name == abil.GetComponent<Ability>().ability_name)
                    {
                        abilityList.Remove(abil);
                    }
                }
            }
        }

        tempSettings.attackPrefabs = abilityList.ToArray();
    }

    public void random_level()
    {
        string scene = scene_names[Random.Range(0, scene_names.Length)];
        SceneManager.LoadScene(scene);
    }
}
