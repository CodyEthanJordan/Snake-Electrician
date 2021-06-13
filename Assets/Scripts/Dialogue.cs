using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public string[] sentences;
    private int index;
    public float typingSpeed;

    public GameObject continueButton;
    public string nextScene;
    //private AudioSource source;

    void Start()
    {
        //source = GetComponent<AudioSource>();
        StartCoroutine("Type");
    }

    void Update()
    {
        if(textDisplay.text == sentences[index])
        {
            continueButton.SetActive(true);
        }
        if (Input.anyKeyDown) {
            StopCoroutine("Type");
            textDisplay.text = sentences[index];
            
        }
        if (Input.anyKeyDown && continueButton.active == true)
        {
            NextSentence();
        }

    }
    IEnumerator Type()
    {
        foreach (char letter in sentences[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    void OnMouseDown()
    {
        StopCoroutine(Type());

    }

    public void NextSentence()
    {
        //source.Play();
        continueButton.SetActive(false);

        if(index < sentences.Length - 1)
        {
            index++;
            textDisplay.text = "";
            StartCoroutine("Type");
        } else
        {
            textDisplay.text = "";
            continueButton.SetActive(false);
            SceneManager.LoadScene(nextScene);
        }
    }
}
