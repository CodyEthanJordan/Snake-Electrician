using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Assets.Scripts
{
    public class LevelController : MonoBehaviour
    {
        public string NextLevelName;
        public Image BlackFade;
        public float FadeDuration = 0.3f;

        private void Start()
        {
            BlackFade = GameObject.Find("BlackFade").GetComponent<Image>();
            StartCoroutine(FadeIn());
        }

        IEnumerator FadeIn()
        {
            for (float ft = 1f; ft >= 0; ft -= 0.1f)
            {
                var color = BlackFade.color;
                color.a = ft;
                BlackFade.color = color;
                yield return new WaitForSeconds(FadeDuration / 10.0f);;
            }
        }

        IEnumerator FadeOut()
        {
            for (float ft = 0f; ft <= 1; ft += 0.1f)
            {
                var color = BlackFade.color;
                color.a = ft;
                BlackFade.color = color;
                yield return new WaitForSeconds(FadeDuration / 10.0f);;
            }
            SceneManager.LoadScene(NextLevelName);
        }

        public void Reload()
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

        public void GoNextLevel()
        {
            StartCoroutine(FadeOut());
        }
    }
}
