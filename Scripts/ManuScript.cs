using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ManuScript
{
    [MenuItem("Tool/Assign Tile Material")]
    public static void AssignTileMaterial()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        Material material = Resources.Load<Material>("Tile");

        foreach(GameObject t in tiles)
        {
            t.GetComponent<Renderer>().material = material;
        }
    }

    [MenuItem("Tool/Assign Tile Script")]
    public static void AssignTileScript()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        

        foreach (GameObject t in tiles)
        {
            t.AddComponent<Tile>();
        }

    }
}
