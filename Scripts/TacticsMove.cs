using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticsMove : MonoBehaviour
{
    List<Tile> selectableTiles = new List<Tile>();
    GameObject[] tiles;

    Stack<Tile> path = new Stack<Tile>();
    Tile currentTile;

    public bool moving = false;
    public int move = 1;
    public float moveSpeed = 2;

    Vector3 velocity = new Vector3();
    Vector3 heading = new Vector3();

    Vector3 up = Vector3.zero,
    right = new Vector3(0, 90, 0),
    down = new Vector3(0, 180, 0),
    left = new Vector3(0, 270, 0),
    currentDirection = Vector3.zero;

    Vector3 nextPos, destination, direction;
    float speed = 6f;
    float rayLength = 1f;
    bool canMove;


    protected void Init()
    {
        tiles = GameObject.FindGameObjectsWithTag("Tile");
        currentDirection = up;
        nextPos = Vector3.forward;
        destination = transform.position;

    }

    public void GetCurrentTile()
    {
        currentTile = GetTargetTile(gameObject);
        currentTile.current = true;
        currentTile.walkable = false;
    }

    public Tile GetTargetTile(GameObject target)
    {
        RaycastHit hit;
        Tile tile = null;

        if (Physics.Raycast(target.transform.position, -Vector3.up, out hit, 4))
        {
            tile = hit.collider.GetComponent<Tile>();
        }
        return tile;
    }

    public void ComputeAdjacencyList()
    {
        foreach (GameObject tile in tiles)
        {
            Tile t = tile.GetComponent<Tile>();
            t.FindNeighbor();
        }
    }

    public void FindSelectableTiles()
    {
        ComputeAdjacencyList();
        GetCurrentTile();

        Queue<Tile> process = new Queue<Tile>();
        process.Enqueue(currentTile);
        currentTile.visited = true;

        while (process.Count > 0)
        {
            Tile t = process.Dequeue();

            selectableTiles.Add(t);
            t.selectable = true;

            if (t.distance < move)
            {
                foreach (Tile tile in t.adjacencyList)
                {
                    if (!tile.visited)
                    {
                        tile.parent = t;
                        tile.visited = true;
                        tile.distance = 1 + t.distance;
                        process.Enqueue(tile);
                    }
                }
            }
        }
    }

    public void MoveToTile(Tile tile)
    {
        path.Clear();
        tile.target = true;
        moving = true;

        Tile next = tile;
        while(next != null)
        {
            path.Push(next);
            next = next.parent;
        }
    }

    //public void Move()
    //{
    //    transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);

    //    //Up -left
    //    if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
    //    {
    //        //nextPos = Vector3.forward;
    //        nextPos = new Vector3(0, 0, 4);
    //        currentDirection = up;
    //        canMove = true;
    //    }
    //    //Down right
    //    if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
    //    {
    //        //nextPos = Vector3.back;
    //        nextPos = new Vector3(0, 0, -4);
    //        currentDirection = down;
    //        canMove = true;
    //    }
    //    //Right Up
    //    if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
    //    {
    //        //nextPos = Vector3.right;
    //        nextPos = new Vector3(4, 0, 0);
    //        currentDirection = right;
    //        canMove = true;
    //    }
    //    //Left Down
    //    if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
    //    {
    //        //nextPos = Vector3.left;
    //        nextPos = new Vector3(-4, 0, 0);
    //        currentDirection = left;
    //        canMove = true;
    //    }


    //    if (Vector3.Distance(destination, transform.position) <= 0.00001f)
    //    {
    //        transform.localEulerAngles = currentDirection;

    //        if (canMove)
    //        {
    //            if (Valid())
    //            {
    //                destination = transform.position + nextPos;
    //                direction = nextPos;
    //                canMove = false;
    //            }
    //        }
    //    }
    //}

    //bool Valid()
    //{
    //    Ray myRay = new Ray(transform.position + new Vector3(0, 0.25f, 0), transform.forward);
    //    RaycastHit hit;

    //    Debug.DrawRay(myRay.origin, myRay.direction);

    //    if (Physics.Raycast(myRay, out hit, rayLength))
    //    {
    //        if (hit.collider.tag == "Wall")
    //        {
    //            return false;
    //        }
    //    }
    //    return true;
    //}

    public void Move()
    {
        if (path.Count > 0)
        {
            Tile t = path.Peek();
            Vector3 target = t.transform.position;

            target.y = 1;

            if (Vector3.Distance(transform.position, target) >= 0.05f)
            {
                CalculateHeading(target);
                SetHorizontalVelocity();

                transform.forward = heading;
                transform.position += velocity * Time.deltaTime;
            }
            else
            {
                //tile center reached
                transform.position = target;
                path.Pop();
            }
        }
        else
        {
            RemoveSelectableTiles();
            moving = false;
        }

    }

    void SetHorizontalVelocity()
    {
        velocity = heading * moveSpeed;
    }

    public void CalculateHeading(Vector3 target)
    {
        heading = target - transform.position;
        heading.Normalize();
    }

    protected void RemoveSelectableTiles()
    {
        if(currentTile != null)
        {
            currentTile.current = false;
            currentTile = null;
        }

        foreach(Tile tile in selectableTiles)
        {
            tile.Reset();
        }

        selectableTiles.Clear();
    }
}
