using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class BodyTurner : MonoBehaviour
    {
        public Sprite[] Sprites;

        private SpriteRenderer sr;

        private static Dictionary<Tuple<Vector2Int, Vector2Int>, int> directions = new Dictionary<Tuple<Vector2Int, Vector2Int>, int>
        {
            { new Tuple<Vector2Int, Vector2Int>(Vector2Int.up, Vector2Int.right), 3 },
            { new Tuple<Vector2Int, Vector2Int>(Vector2Int.up, Vector2Int.down), 0 },
            { new Tuple<Vector2Int, Vector2Int>(Vector2Int.up, Vector2Int.left), 5 },
            { new Tuple<Vector2Int, Vector2Int>(Vector2Int.right, Vector2Int.down), 2 },
            { new Tuple<Vector2Int, Vector2Int>(Vector2Int.right, Vector2Int.left), 1 },
            { new Tuple<Vector2Int, Vector2Int>(Vector2Int.right, Vector2Int.up), 3 },
            { new Tuple<Vector2Int, Vector2Int>(Vector2Int.down, Vector2Int.left), 2 },
            { new Tuple<Vector2Int, Vector2Int>(Vector2Int.down, Vector2Int.up), 0 },
            { new Tuple<Vector2Int, Vector2Int>(Vector2Int.down, Vector2Int.right), 4 },
            { new Tuple<Vector2Int, Vector2Int>(Vector2Int.left, Vector2Int.up), 5 },
            { new Tuple<Vector2Int, Vector2Int>(Vector2Int.left, Vector2Int.right), 1 },
            { new Tuple<Vector2Int, Vector2Int>(Vector2Int.left, Vector2Int.down), 2 },
        };

        private void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
        }

        public void RenderDirection(Vector2Int upstream, Vector2Int downstream)
        {
            int i = directions[new Tuple<Vector2Int, Vector2Int>(upstream, downstream)];
            sr.sprite = Sprites[i];
        }
    }
}
