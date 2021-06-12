using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level_Select : MonoBehaviour
{
    public GameObject levelHolder;
    public GameObject levelIcon;
    public int numberOfLevels = 10;
    public Image image;
    // Start is called before the first frame update
    void Start()
    {
        Rect panelDimensions = levelHolder.GetComponent<RectTransform>().rect;
        Rect iconDimensions = levelIcon.GetComponent<RectTransform>().rect;
        int maxInARow = Mathf.FloorToInt(panelDimensions.width / iconDimensions.width);
        int maxInACol = Mathf.FloorToInt(panelDimensions.height / iconDimensions.height);
        int amountPerPage = maxInARow * maxInACol;
        int totalPages = Mathf.CeilToInt((float)numberOfLevels / amountPerPage);
        LoadPanels(totalPages);

    }
    void LoadPanels(int numberOfPanels)
    {
        Debug.Log(numberOfPanels);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
