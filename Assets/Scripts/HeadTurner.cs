using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class HeadTurner : MonoBehaviour
    {
        public Sprite[] Sprites;

        private SpriteRenderer sr;

        private void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
        }

        public void RenderDirection(Vector2Int dir)
        {
            if(dir == Vector2Int.up)
            {
                sr.sprite = Sprites[0];
            }
            else if(dir == Vector2Int.right)
            {
                sr.sprite = Sprites[1];
            }
            else if(dir == Vector2Int.down)
            {
                sr.sprite = Sprites[2];
            }
            else
            {
                sr.sprite = Sprites[3];
            }
        }
    }
}
