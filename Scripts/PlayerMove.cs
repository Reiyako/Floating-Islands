using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : TacticsMove
{
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if(!moving)
        {
            FindSelectableTiles();
            //CheckKeys();
        }
        else
        {
            Move();
        }
    }

    //void CheckKeys()
    //{
    //    if(Input.GetMouseButtonUp(0))
    //    {
    //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

    //        RaycastHit hit;
    //        if(Physics.Raycast(ray, out hit))
    //        {
    //            if(hit.collider.tag == "Tile")
    //            {
    //                Tile t = hit.collider.GetComponent<Tile>();

    //                if(t.selectable)
    //                {
    //                    //move target
    //                    MoveToTile(t);
    //                }
    //            }
    //        }
    //    }

    //}
}
