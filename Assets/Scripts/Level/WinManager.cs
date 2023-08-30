using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class WinManager : MonoBehaviour
{
    public int skinIndex;
    public SpriteRenderer[] skinSprites;
    public string winName;
    public Sprite sprite;
    public PlayerSettings settings;
    public TextMeshProUGUI tmpro;
    public Gradient gradient;
    public ParticleSystem ps;
    public SpriteRenderer bgSR;
    // Start is called before the fafasdirst frame update
    void Start()
    {
        var main = ps.main;
        skinIndex = PlayerPrefs.GetInt("skinIndex");
        PlayerPrefs.DeleteKey("skinIndex");
        sprite = settings.player_skins[skinIndex];
        winName = settings.player_strings[skinIndex];
        gradient = settings.player_gradients[skinIndex];
        main.startColor = new ParticleSystem.MinMaxGradient(gradient);
        tmpro.text = (winName + " WINS!");
        foreach (SpriteRenderer sp in skinSprites)
        {
            sp.sprite = sprite;
        }
        bgSR.color = settings.skin_colors[skinIndex];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
