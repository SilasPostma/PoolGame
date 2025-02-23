using System.Collections.Generic;
using UnityEngine;

public class TableInitiator : MonoBehaviour
{
    public float tableLength = 12f;
    public float tableWidth = 6f;
    public GameObject collParent;
    public GameObject edgePiece;
    public GameObject pocketPiece;
    public int pocketAmount;

    private List<GameObject> tableColliders = new List<GameObject>();
    private List<GameObject> pockets = new List<GameObject>();

    void Start()
    {
        tableLength--;
        CreateTable();
    }

    private void CreateTable()
    {
        float halfLength = tableLength / 2f;
        float halfWidth = tableWidth / 2f;

        List<Vector3> pocketPositions = GenerateRandomPocketPositions(halfLength, halfWidth);

        // Place edges and pockets
        PlaceEdgesAndPockets(halfLength, halfWidth, pocketPositions);
    }

    private List<Vector3> GenerateRandomPocketPositions(float halfLength, float halfWidth)
    {
        List<Vector3> pocketPositions = new List<Vector3>();
        List<Vector3> possiblePositions = new List<Vector3>
        {
            new Vector3(-halfLength, 0, halfWidth), // Top-left corner
            new Vector3(halfLength, 0, halfWidth),  // Top-right corner
            new Vector3(-halfLength, 0, -halfWidth), // Bottom-left corner
            new Vector3(halfLength, 0, -halfWidth)  // Bottom-right corner
        };

        for (int i = 0; i < pocketAmount && possiblePositions.Count > 0; i++)
        {
            int index = Random.Range(0, possiblePositions.Count);
            pocketPositions.Add(possiblePositions[index]);
            possiblePositions.RemoveAt(index);
        }

        return pocketPositions;
    }

    private void PlaceEdgesAndPockets(float halfLength, float halfWidth, List<Vector3> pocketPositions)
    {
        Vector3[] edgePositions = {
            new Vector3(0, 0, halfWidth),    // Top edge
            new Vector3(0, 0, -halfWidth),   // Bottom edge
            new Vector3(-halfLength - 1f, 0, 0), // Left edge
            new Vector3(halfLength + 1f, 0, 0)   // Right edge
        };

        Vector3[] edgeScales = {
            new Vector3(tableLength + 1, 1, 1), // Horizontal edges
            new Vector3(1, 1, tableWidth+ 1)  // Vertical edges
        };

        for (int i = 0; i < edgePositions.Length; i++)
        {
            bool isHorizontal = i < 2; // First two edges are horizontal
            Vector3 position = edgePositions[i];

            Vector3 scale = isHorizontal ? edgeScales[0] : edgeScales[1];

            CreateEdge(position, scale);
        }

        foreach (Vector3 pocketPosition in pocketPositions)
        {
            Quaternion pocketRotation = DeterminePocketRotation(pocketPosition, halfLength, halfWidth);
            CreatePocket(pocketPosition, pocketRotation);
        }
    }

    private Quaternion DeterminePocketRotation(Vector3 position, float halfLength, float halfWidth)
    {
        if (position.x == -halfLength) return Quaternion.Euler(0, 90, 0); // Left side
        if (position.x == halfLength) return Quaternion.Euler(0, -90, 0); // Right side
        if (position.z == halfWidth) return Quaternion.Euler(0, 180, 0); // Top side
        return Quaternion.identity; // Bottom side (default rotation)
    }

    private void CreateEdge(Vector3 position, Vector3 scale)
    {
        GameObject edge = Instantiate(edgePiece, position, Quaternion.identity, collParent.transform);
        edge.transform.localScale = scale;
        tableColliders.Add(edge);
    }

    private void CreatePocket(Vector3 position, Quaternion rotation)
    {
        GameObject pocket = Instantiate(pocketPiece, position, rotation, collParent.transform);
        pockets.Add(pocket);
    }
}
