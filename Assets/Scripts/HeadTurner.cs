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
        public Sprite[] ShockedSprites;

        public bool shocked = false;

        private SpriteRenderer sr;

        private void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
        }

        public void ToggleShock()
        {
            shocked = !shocked;
        }

        public void RenderDirection(Vector2Int dir)
        {
            if(transform.childCount > 0)
            {
                //transform.GetChild(0).GetComponent<HeadTurner>().RenderDirection(dir);
            }
            
            if (!shocked)
            {
                if (dir == Vector2Int.up)
                {
                    sr.sprite = Sprites[0];
                }
                else if (dir == Vector2Int.right)
                {
                    sr.sprite = Sprites[1];
                }
                else if (dir == Vector2Int.down)
                {
                    sr.sprite = Sprites[2];
                }
                else
                {
                    sr.sprite = Sprites[3];
                }
            }
            else
            {
                if (dir == Vector2Int.up)
                {
                    sr.sprite = ShockedSprites[0];
                }
                else if (dir == Vector2Int.right)
                {
                    sr.sprite = ShockedSprites[1];
                }
                else if (dir == Vector2Int.down)
                {
                    sr.sprite = ShockedSprites[2];
                }
                else
                {
                    sr.sprite = ShockedSprites[3];
                }
            }
        }
    }
}
