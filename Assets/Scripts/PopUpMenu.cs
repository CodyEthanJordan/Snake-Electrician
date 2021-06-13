using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpMenu : MonoBehaviour
{
    public GameObject popUpCanvas;

    public void myFunction()
    {
        popUpCanvas.SetActive(false);
    }

    

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (popUpCanvas.active == false) {

                popUpCanvas.SetActive(true);
                GameObject.Find("playerSnake").GetComponent<SnakeController>().Pause();
            } else
            {
                popUpCanvas.SetActive(false);
                GameObject.Find("playerSnake").GetComponent<SnakeController>().Resume();
            }
        }
    }
    public void ContinueGame(string choice)
    {
        if (choice == "Return To Game")
        {
            popUpCanvas.SetActive(false);
            GameObject.Find("playerSnake").GetComponent<SnakeController>().Resume();
        }
    }
}
