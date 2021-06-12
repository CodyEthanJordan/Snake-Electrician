using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class SnakeController : MonoBehaviour
    {
        public List<Vector2Int> Body = new List<Vector2Int>();
        public float TurnLength = 0.4f;
        public GameObject bodySegment;
        public GameObject headSegment;
        public LevelController lc;

        private float turnTimer = 0.5f;
        private Vector2Int nextDirection = Vector2Int.right;

        private int growth = 0;
        private int targetPlugs = 2;

        public static List<Vector2Int> DefaultBody = new List<Vector2Int>
        {
            new Vector2Int(3,0), new Vector2Int(2,0), new Vector2Int(1,0)
        };

        // Start is called before the first frame update
        void Start()
        {
            ResetBody();
            RerenderBody();
            CountGoalPlugs();
            lc = GameObject.Find("LevelController").GetComponent<LevelController>();
        }

        private void CountGoalPlugs()
        {
            var plugs = GameObject.FindGameObjectsWithTag("Goal");
            targetPlugs = plugs.Length;
        }

        private void ResetBody()
        {
            Body.Clear();
            foreach (var pos in DefaultBody)
            {
                Body.Add(pos);
            }
        }

        // Update is called once per frame
        void Update()
        {
            Vector2Int heading = new Vector2Int(Mathf.RoundToInt(Input.GetAxisRaw("Horizontal")), Mathf.RoundToInt(Input.GetAxisRaw("Vertical")));
            UpdateHeading(heading);

            turnTimer -= Time.deltaTime;
            if (turnTimer <= 0)
            {
                turnTimer = TurnLength;
                var newHead = Body[0] + nextDirection;
                CheckCollision(newHead);
                //var newHead = Body[Body.Count - 1] + nextDirection;
                //Body.Add(newHead);
                Body.Insert(0, newHead);
                if (growth == 0)
                {
                    Body.RemoveAt(Body.Count - 1);
                }
                else if(growth > 0)
                {
                    growth--;
                }
                else //negative
                {
                    growth++;
                    Body.RemoveAt(Body.Count - 1);
                    Body.RemoveAt(Body.Count - 1);
                    if(Body.Count <= 0)
                    {
                        Die();
                    }
                }
                RerenderBody();
                CheckTouching();
            }
        }

        public void Die()
        {
            Debug.LogError("You dead");
            ResetBody();
            turnTimer = TurnLength * 2;
            lc.Reload();
        }

        private void CheckTouching()
        {
            int plugsFound = 0;
            int waterTouched = 0;
            int pitsOver = 0;
            foreach (var segment in Body)
            {
                var offset = new Vector2(this.transform.position.x, this.transform.position.y);
                var colliders = Physics2D.OverlapCircleAll(segment + offset, 0.3f);
                foreach (var col in colliders)
                {
                    if (col.CompareTag("Goal"))
                    {
                        plugsFound++;
                    }
                    else if(col.CompareTag("Pit"))
                    {
                        pitsOver++;
                    }
                    else if(col.CompareTag("Water"))
                    {
                        waterTouched++;
                    }
                }
            }

            Debug.Log("Pits touching: " + pitsOver);
            if(pitsOver >= Body.Count)
            {
                Die();
            }

            if(waterTouched > 0 && plugsFound > 0)
            {
                Die(); //electrocuted, TODO: anim
            }

            if (plugsFound >= targetPlugs)
            {
                Debug.LogError("WIN");
                lc.GoNextLevel();
            }
        }

        private void UpdateHeading(Vector2Int heading)
        {
            if (heading == Vector2Int.zero || heading + nextDirection == Vector2Int.zero)
            {
                return; //can't go backwards and ignore no input
            }

            nextDirection = heading;
        }

        private void CheckCollision(Vector2Int pos)
        {
            var offset = new Vector2(this.transform.position.x, this.transform.position.y);
            var colliders = Physics2D.OverlapCircleAll(pos + offset, 0.3f);

            foreach (var col in colliders)
            {
                if (col.CompareTag("Food"))
                {
                    var value = col.gameObject.GetComponent<Food>().Value;
                    growth += value;

                    Destroy(col.gameObject);
                }
                else if (col.CompareTag("Goal") || col.CompareTag("Pit") || col.CompareTag("Water")) //TODO: make less stupid
                {

                }
                else
                {
                    Die();
                }
            }
        }

        private void RerenderBody()
        {
            foreach (Transform child in this.transform)
            {
                Destroy(child.gameObject);
            }


            for (int i = 0; i < Body.Count; i++)
            {
                var go = i == 0 ? headSegment : bodySegment;
                var pos = new Vector3(Body[i].x, Body[i].y, 0) + this.transform.position;
                Instantiate(go, pos, Quaternion.identity, this.transform);
            }
        }
    }
}
