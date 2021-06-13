using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class MusicController : MonoBehaviour
    {
        public AudioClip[] Tracks;
        private int trackNum = 0;
        private AudioSource source;
        private float volume = 0.5f;

        private void Start()
        {
            source = GetComponent<AudioSource>();
            volume = source.volume;
            DontDestroyOnLoad(this.gameObject);
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.M))
            {
                ToggleMute();
            }

            if(!source.isPlaying)
            {
                trackNum = (trackNum + 1) % Tracks.Length;
                source.clip = Tracks[trackNum];
                source.Play();
            }
        }

        public void ToggleMute()
        {
            if(source.volume == 0)
            {
                source.volume = volume;
            }
            else
            {
                source.volume = 0;
            }
        }
    }
}
