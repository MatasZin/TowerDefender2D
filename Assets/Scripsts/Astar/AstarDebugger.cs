using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarDebugger : MonoBehaviour
{
    [SerializeField]
    private TileScript goal, start;

    [SerializeField]
    private Sprite blankSprite;

    [SerializeField]
    private GameObject arroePrefab;

    [SerializeField]
    private GameObject debugTile;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    //void Update()
    //{
    //    ClickTile();

    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        Astar.GetPath(start.GridPosition, goal.GridPosition);
    //    }
    //}

    private void ClickTile()
    {
        if (Input.GetMouseButtonDown(1))
        {
            /*float xPos = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
            float xRounded = RoundX(xPos);
            float yPos = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
            float yRounded = RoundY(yPos);
            Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            Debug.Log(xRounded + " " + yRounded);
            Vector2 newVec = new Vector2(xRounded, yRounded);
            Debug.Log(newVec);*/
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if(hit.collider != null)
            {
                TileScript tmp = hit.collider.GetComponent<TileScript>();

                if(tmp != null)
                {
                    if(start == null)
                    {
                        start = tmp;
                        CreateDebugTile(start.WorldPosition, Color.grey);
                    }
                    else if(goal == null)
                    {
                        goal = tmp;
                        CreateDebugTile(goal.WorldPosition, new Color32(255, 135, 0, 255));
                    }
                }
            }
        }
    }

    public void DebugPath(HashSet<Node> openList, HashSet<Node> closedList, Stack<Node> finalPath)
    {
        foreach (Node node in openList)
        {
            if(node.TileRef != start && node.TileRef != goal)
            {
                CreateDebugTile(node.TileRef.WorldPosition, Color.cyan, node);
            }
            PointToParent(node, node.TileRef.GridPosition);
        }

        foreach (Node node in closedList)
        {
            if (node.TileRef != start && node.TileRef != goal && !finalPath.Contains(node))
            {
                CreateDebugTile(node.TileRef.WorldPosition, Color.blue, node);
            }
            PointToParent(node, node.TileRef.GridPosition);
        }

        foreach (Node node in finalPath)
        {
            CreateDebugTile(node.TileRef.WorldPosition, Color.green, node);

        }
    }

    private void PointToParent(Node node, Point position)
    {
        if (node.Parent != null)
        {

            GameObject arrow = Instantiate(arroePrefab, new Vector2(node.GridPostion.X, node.GridPostion.Y), Quaternion.identity);
            arrow.GetComponent<SpriteRenderer>().sortingOrder = 3;
            //right
            if ((node.GridPostion.X < node.Parent.GridPostion.X) && (node.GridPostion.Y == node.Parent.GridPostion.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 0);
            }
            //Top right
            else if ((node.GridPostion.X < node.Parent.GridPostion.X) && (node.GridPostion.Y < node.Parent.GridPostion.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0,0, 45);
            }
            //top
            else if ((node.GridPostion.X == node.Parent.GridPostion.X) && (node.GridPostion.Y < node.Parent.GridPostion.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0,0, 90);
            }
            //top left
            else if ((node.GridPostion.X > node.Parent.GridPostion.X) && (node.GridPostion.Y < node.Parent.GridPostion.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0,0, 135);
            }
            //left
            else if ((node.GridPostion.X > node.Parent.GridPostion.X) && (node.GridPostion.Y == node.Parent.GridPostion.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0,0, 180);
            }
            //bottom left
            else if ((node.GridPostion.X > node.Parent.GridPostion.X) && (node.GridPostion.Y > node.Parent.GridPostion.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 225);
            }
            //bottom 
            else if ((node.GridPostion.X == node.Parent.GridPostion.X) && (node.GridPostion.Y > node.Parent.GridPostion.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 270);
            }
            //bottom right
            else if ((node.GridPostion.X < node.Parent.GridPostion.X) && (node.GridPostion.Y > node.Parent.GridPostion.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 315);
            }
        }
    }

    private float RoundX(float x)
    {
        int xFlat = Convert.ToInt32(Mathf.Floor(x));
        float xRemain = x - xFlat;
        if(xRemain > 0.5)
        {
            x = xFlat + 1;
        } else
        {
            x = xFlat + (float) 0.5;
        }
        return x;
    }

    private float RoundY(float y)
    {
        int yFlat = Convert.ToInt32(Mathf.Floor(y));
        float yRemain = y - yFlat;
        if (yRemain > 0.5)
        {
            y = yFlat + 1;
        }
        else 
        {
            y = yFlat + (float)0.5;
        }
        return y;
    }

    private void CreateDebugTile(Vector3 worldPos, Color32 color, Node node = null)
    {
        GameObject debugtile = Instantiate(debugTile, worldPos, Quaternion.identity);

        if(node != null)
        {
            DebugTile tmp = debugtile.GetComponent<DebugTile>();
            tmp.G.text += node.G;
            tmp.H.text += node.H;
            tmp.F.text += node.F;
        }

        debugTile.GetComponent<SpriteRenderer>().color = color;
    }
}
