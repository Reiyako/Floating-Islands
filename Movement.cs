using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Vector3 up = Vector3.zero,
    right = new Vector3(0, 90, 0),
    down = new Vector3(0, 180, 0),
    left = new Vector3(0, 270, 0),
    currentDirection = Vector3.zero;

    Vector3 nextPos, destination, direction;
    float speed = 5f;
    float rayLength = 1f;
    bool canMove;
    
    void Start()
    {
        currentDirection = up;
        nextPos = Vector3.forward;
        destination = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);

        //Up
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            nextPos = Vector3.forward;
            currentDirection = up;
            canMove = true;
        }
        //Down
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            nextPos = Vector3.back;
            currentDirection = down;
            canMove = true;
        }
        //Right
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            nextPos = Vector3.right;
            currentDirection = right;
            canMove = true;
        }
        //Left
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            nextPos = Vector3.left;
            currentDirection = left;
            canMove = true;
        }


        if(Vector3.Distance(destination, transform.position) <= 0.00001f)
        {
            transform.localEulerAngles = currentDirection;

            if (canMove)
            {
                if (Valid())
                {
                    destination = transform.position + nextPos;
                    direction = nextPos;
                    canMove = false;
                }
            }
        }
    }

    bool Valid()
    {
        Ray myRay = new Ray(transform.position + new Vector3(0, 0.25f, 0), transform.forward);
        RaycastHit hit;

        Debug.DrawRay(myRay.origin, myRay.direction);

        if(Physics.Raycast(myRay, out hit, rayLength))
        {
            if(hit.collider.tag == "Wall")
            {
                return false;
            }
        }
        return true;
    }
}
