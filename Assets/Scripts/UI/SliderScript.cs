using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SliderScript : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI sliderValue;
    public TextMeshProUGUI sliderMin;
    public TextMeshProUGUI sliderMax;

    public int timeMin;
    public int timeMax;

    // Start is called before the first frame update
    void Start()
    {
        slider.SetValueWithoutNotify(PlayerPrefs.GetInt("roundTimer",100));
        sliderMin.text = timeMin.ToString();
        sliderMax.text = timeMax.ToString();
        sliderValue.text = slider.value.ToString();
        slider.minValue = timeMin;
        slider.maxValue = timeMax;
        slider.onValueChanged.AddListener((v) =>
        {
            sliderValue.text = v.ToString();
            PlayerPrefs.SetInt("roundTimer", (int)v);
        });
        
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
