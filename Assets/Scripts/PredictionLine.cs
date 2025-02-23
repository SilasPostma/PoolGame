using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredictionLine : MonoBehaviour
{
    public Transform cueBall; // The cue ball's Transform
    public Transform cue; // The cue's Transform
    public LayerMask collisionMask; // LayerMask for detecting collisions (balls, table edges)
    public LineRenderer lineRenderer; // The LineRenderer component for prediction line
    public LineRenderer collisionDirectionLine; // The LineRenderer component for the predicted direction after collision
    public int maxReflectionCount = 3; // Number of bounces to predict
    public float maxDistance = 10f; // Maximum prediction distance
    public Transform collCircle;
    public float ballRadius = 0.5f; // The radius of the cue ball (adjust this to your ball's actual radius)

    private Vector3 ballHitPos; // Declare ballHitPos as an instance variable
    private Vector3 endPoint; // Declare endPoint as an instance variable

    void Update()
    {
        GameManager gameManager = GameManager.Instance;
        DrawStraightPredictionLine();
    }

    void DrawStraightPredictionLine()
    {
        // Define the starting point and direction of the spherecast
        Vector3 startPoint = cueBall.position;// + (cueBall.position - cue.position).normalized * ballRadius;
        Vector3 direction = (cueBall.position - cue.position).normalized;
        ballHitPos = Vector3.zero; // Reset ballHitPos for each update
        endPoint = Vector3.zero;  // Reset endPoint to ensure no residual data

        List<Vector3> points = new List<Vector3>();
        points.Add(startPoint);

        // Perform a SphereCast to detect collisions
        RaycastHit hit;
        float lineLength;

        // Adjust the startPoint slightly to avoid initial clipping issues
        Vector3 adjustedStartPoint = startPoint;// + direction * 0.01f;

        if (Physics.SphereCast(adjustedStartPoint, ballRadius, direction, out hit, maxDistance, collisionMask))
        {
            // If a collision is detected, set the line length to the distance to the hit point
            lineLength = hit.distance + 0.01f; // Add a small buffer to ensure the line doesn't stop short
            ballHitPos = hit.collider.transform.position; // Get the position of the hit ball
            if (hit.transform.CompareTag("Ball"))
            {
                // If the hit object is a ball, calculate the endpoint of the cue ball's path and draw the deflection line
                endPoint = startPoint + direction * lineLength;

            }
        }
        else
        {
            // If no collision, the line continues to the max distance
            lineLength = maxDistance;
        }

        DrawDeflectionLine(); // Draw deflection line only if a ball is hit

        // Calculate the endpoint along the original direction based on the determined line length
        endPoint = startPoint + direction * lineLength;
        points.Add(endPoint);

        // Set the positions for the LineRenderer
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
        collCircle.position = endPoint;
    }

    void DrawDeflectionLine()
    {
        // Ensure the ballHitPos is valid and endPoint is calculated
        if (ballHitPos != Vector3.zero && endPoint != Vector3.zero)
        {
            // Calculate the deflection direction based on the ball's position and endpoint
            Vector3 deflectDir = -(endPoint - ballHitPos).normalized; // Calculate deflection direction
            collisionDirectionLine.positionCount = 2;

            collisionDirectionLine.SetPosition(0, endPoint + deflectDir * 0.5f);
            collisionDirectionLine.SetPosition(1, endPoint + deflectDir * 2f); // Length of 1 unit for visualization
            Debug.Log("Show");
        }
        else
        {
            Debug.Log("Hide");
            // If no deflection line should be drawn, hide it
            collisionDirectionLine.positionCount = 0;
        }
    }
}
