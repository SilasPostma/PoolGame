using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PocketGenerator : MonoBehaviour
{
    public GameObject pocketPrefab; // Prefab for the pocket
    public int pocketCount = 6; // Total number of pockets
    public float tableWidth = 10f;
    public float tableLength = 20f;
    public float pocketInset = 0.5f; // How far pockets are inset from the edges

    public List<Vector3> pocketPositions = new List<Vector3>();

    void Awake()
    {
        GeneratePockets();
    }

    void GeneratePockets()
    {
        for (int i = 0; i < pocketCount; i++)
        {
            // Randomize positions along table edges
            float x = Random.Range(-tableWidth / 2 + pocketInset, tableWidth / 2 - pocketInset);
            float z = Random.Range(-tableLength / 2 + pocketInset, tableLength / 2 - pocketInset);

            // Ensure pockets are on edges (simplified example)
            if (Random.value > 0.5f)
                x = Mathf.Sign(x) * (tableWidth / 2 - pocketInset);
            else
                z = Mathf.Sign(z) * (tableLength / 2 - pocketInset);

            Vector3 pocketPos = new Vector3(x, 0.3f, z);
            pocketPositions.Add(pocketPos);

            // Spawn the pocket prefab
            Instantiate(pocketPrefab, pocketPos, Quaternion.identity);
        }
    }
}
