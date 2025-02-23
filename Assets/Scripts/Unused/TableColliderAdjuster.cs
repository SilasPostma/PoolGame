using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableColliderAdjuster : MonoBehaviour
{
    public GameObject table; // The table object
    //public List<Vector3> pocketPositions;

    private List<BoxCollider> tableColliders = new List<BoxCollider>();

    public float width = 8.5f;
    public float length = 17f;


    public PocketGenerator pocketGenerator;

    void Start()
    {
        pocketGenerator = FindObjectOfType<PocketGenerator>();

        // Split the table collider into edges
        CreateTableColliders();

        // Adjust colliders around pockets
        AdjustCollidersForPockets();
    }

    void CreateTableColliders()
    {

        tableColliders.Add(CreateEdgeCollider(new Vector3(width, 0, 0), new Vector3(1, 1, width))); // Top
        tableColliders.Add(CreateEdgeCollider(new Vector3(-width, 0, 0), new Vector3(1, 1, width))); // Bottom
        tableColliders.Add(CreateEdgeCollider(new Vector3(0, 4.5f, 0), new Vector3(length, 1, 1))); // Left
        tableColliders.Add(CreateEdgeCollider(new Vector3(0, -4.5f, 0), new Vector3(length, 1, 1))); // Right
    }

    BoxCollider CreateEdgeCollider(Vector3 position, Vector3 size)
    {
        GameObject edge = new GameObject("TableEdge");
        edge.transform.SetParent(table.transform);
        edge.transform.localPosition = position;

        BoxCollider collider = edge.AddComponent<BoxCollider>();
        collider.size = size;

        return collider;
    }

    void AdjustCollidersForPockets()
    {
        foreach (Vector3 pocketPos in pocketGenerator.pocketPositions)
        {
            print("ja");
            foreach (BoxCollider collider in tableColliders)
            {
                if (collider.bounds.Contains(pocketPos))
                {
                    print("col");
                    collider.enabled = false; // Disable collider near pocket
                }
            }
        }
    }
}