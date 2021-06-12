using System;
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
        public GameObject tailSegment;
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
                GoDirection(nextDirection);
                RerenderBody();
                CheckTouching();
            }
        }

        private void GoDirection(Vector2Int heading)
        {
            var newHead = Body[0] + heading;
            var success = CheckCollision(Body[0], newHead);
            if(!success)
            {
                return;
            }
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
        }

        public void Die()
        {
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

        private Vector2Int GetNextHeadingClockwise(Vector2Int heading)
        {
            if (heading == Vector2Int.up)
            {
                return Vector2Int.right;
            }
            else if (heading == Vector2Int.right)
            {
                return Vector2Int.down;
            }
            else if (heading == Vector2Int.down)
            {
                return Vector2Int.left;
            }
            else if (heading == Vector2Int.left)
            {
                return Vector2Int.up;
            }

            throw new NotImplementedException();
        }

        public bool Passable(Vector2Int pos)
        {
            var offset = new Vector2(this.transform.position.x, this.transform.position.y);
            var colliders = Physics2D.OverlapCircleAll(pos + offset, 0.3f);
            foreach (var col in colliders)
            {
                if(col.CompareTag("Wall"))
                {
                    return false;
                }
            }
            return true;
        }

        private bool CheckCollision(Vector2Int current, Vector2Int target)
        {
            var offset = new Vector2(this.transform.position.x, this.transform.position.y);
            var colliders = Physics2D.OverlapCircleAll(target + offset, 0.3f);

            foreach (var col in colliders)
            {
                if (col.CompareTag("Food"))
                {
                    var value = col.gameObject.GetComponent<Food>().Value;
                    growth += value;

                    Destroy(col.gameObject);
                }
                else if (col.CompareTag("Boost"))
                {
                    Destroy(col.gameObject);
                    TurnLength *= 0.75f;
                }
                else if (col.CompareTag("Goal") || col.CompareTag("Pit") || col.CompareTag("Water")) //TODO: make less stupid
                {

                }
                else if(col.CompareTag("Wall"))
                {
                    var heading = target - current;
                    var newHeading = GetNextHeadingClockwise(heading);
                    if(Passable(current+newHeading))
                    {
                        nextDirection = newHeading;
                        GoDirection(newHeading);
                        return false;
                    }
                    else if(Passable(current - newHeading))
                    {
                        nextDirection = -newHeading;
                        GoDirection(nextDirection);
                        return false;
                    }
                    else
                    {
                        Die();
                    }
                }
                else
                {
                    Die(); //idk
                }
            }

            return true;
        }

        private void RerenderBody()
        {
            foreach (Transform child in this.transform)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < Body.Count; i++)
            {
                var pos = new Vector3(Body[i].x, Body[i].y, 0) + this.transform.position;
                if(i == 0)
                {
                    var go =  Instantiate(headSegment, pos, Quaternion.identity, this.transform);
                    var ht = go.GetComponent<HeadTurner>();
                    ht.RenderDirection(nextDirection);
                }
                else if(i == Body.Count - 1)
                {
                    var go =  Instantiate(tailSegment, pos, Quaternion.identity, this.transform);
                    var ht = go.GetComponent<HeadTurner>();
                    ht.RenderDirection(Body[Body.Count-2] - Body[Body.Count-1]);
                }
                else 
                {
                    var go =  Instantiate(bodySegment, pos, Quaternion.identity, this.transform);
                }
               
            }
        }
    }
}
