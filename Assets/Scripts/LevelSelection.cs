using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelSelection : MonoBehaviour
{
    public Button[] lvlButtons;
    void Start()
    {
        lvlButtons[0].interactable = false;
    }
}
