using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class CharacterSelectionManager : MonoBehaviour
{

    public int rowCount;
    public int colCount;

    public GameObject canvas;

    public CharacterCell[] a, b, c, d;
    public CharacterCell[,] rows;

    public GameObject playerTextPrefab;
    public TextMeshProUGUI errorTxt;
    public Transform textContainerParent;

    private void Awake()
    {
        canvas = this.gameObject;
        rows = new CharacterCell[3, 4]{
            {a[0],a[1],a[2],a[3]},
            {b[0],b[1],b[2],b[3]},
            {c[0],c[1],c[2],c[3]},};
    }

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
