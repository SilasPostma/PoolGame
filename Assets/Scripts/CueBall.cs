using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueBall : MonoBehaviour
{
    private Camera mainCamera;
    public Rigidbody ballRigidbody; // The Rigidbody attached to the pool ball
    public Transform cueTransform; // The Transform of the pool cue
    public float maxPullBackDistance = 2f; // Maximum distance the cue can be pulled back
    public float forceMultiplier = 10f; // Multiplier for the force applied to the ball

    private Vector3 initialCuePosition;
    private bool isDragging = false;
    public float distanceFromBall = -3.0f; // Adjust this distance as needed

    private Vector3 initialClick;
    private Vector3 directionToCursor;
    private Vector3 constrainedDirection;

    public float maxForce;
    public float minForce;

    void Start()
    {
        initialCuePosition = cueTransform.position;
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Handle cue positioning and ball shooting if game is in "Aiming" state
        if (GameManager.Instance.CurrentState == GameState.Aiming)
        {
            PositionAndRotateCue();
        }
    }

    private void PositionAndRotateCue()
    {
        Vector3 mouseWorldPosition = GetMouseWorldPosition();

        if (Input.GetMouseButtonDown(0) && !isDragging)
        {
            StartDragging(mouseWorldPosition);
        }

        if (isDragging && Input.GetMouseButton(0))
        {
            UpdateCuePosition(mouseWorldPosition);
        }
        else
        {
            ResetCuePosition(mouseWorldPosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            StopDragging(mouseWorldPosition);
        }
    }

    private void StartDragging(Vector3 mouseWorldPosition)
    {
        isDragging = true;
        initialClick = GetMouseWorldPosition(); // Store the initial click position

        directionToCursor = -(mouseWorldPosition - transform.position).normalized;
        constrainedDirection = new Vector3(directionToCursor.x, 0, directionToCursor.z).normalized;
    }

    private void UpdateCuePosition(Vector3 mouseWorldPosition)
    {
        float dist = Vector3.Dot(GetMouseWorldPosition() - initialClick, constrainedDirection) < 0
            ? 0
            : (GetMouseWorldPosition() - initialClick).magnitude;

        // Ensure the cue only moves backward (when the cursor moves further away)
        cueTransform.position = transform.position - constrainedDirection * (distanceFromBall - Mathf.Clamp(dist, 0, maxPullBackDistance));
    }

    private void ResetCuePosition(Vector3 mouseWorldPosition)
    {
        Vector3 directionToCursor = -(mouseWorldPosition - transform.position).normalized;
        Vector3 constrainedDirection = new Vector3(directionToCursor.x, 0, directionToCursor.z).normalized;

        cueTransform.position = transform.position - constrainedDirection * distanceFromBall;
        cueTransform.rotation = Quaternion.LookRotation(constrainedDirection, Vector3.up);
    }

    private void StopDragging(Vector3 mouseWorldPosition)
    {
        isDragging = false;
        float dist = Vector3.Dot(GetMouseWorldPosition() - initialClick, constrainedDirection) < 0.2f
            ? 0
            : (GetMouseWorldPosition() - initialClick).magnitude + minForce;

        ApplyForceToBall(Mathf.Clamp(dist, 0, maxForce), -directionToCursor);

        GameManager.Instance.shotTime = Time.time;
        GameManager.Instance.SetGameState(GameState.Shooting); // Change state to Shooting
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = mainCamera.WorldToScreenPoint(transform.position).z;
        return mainCamera.ScreenToWorldPoint(mouseScreenPosition);
    }

    private void ApplyForceToBall(float pullBackAmount, Vector3 shotDirection)
    {
        if (ballRigidbody != null && pullBackAmount > 0)
        {
            ballRigidbody.AddForce(shotDirection * pullBackAmount * forceMultiplier, ForceMode.Impulse);
        }
    }
}
