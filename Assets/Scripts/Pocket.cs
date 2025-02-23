using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pocket : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the pocket is the ball
        if (other.CompareTag("Ball")) // Assuming your ball has the tag "Ball"
        {
            Rigidbody ballRb = other.GetComponent<Rigidbody>();

            if (ballRb != null)
            {
                // Optionally stop the ball's physics (e.g., make it kinematic)
                ballRb.isKinematic = true;

                // Disable the ball or make it invisible, if desired
                //other.gameObject.OnScore();
                other.gameObject.SetActive(false);  // Ball disappears after entering pocket
                Debug.Log("Ball entered the pocket");

                // Access points and multiplier through the GameManager
                GameManager.Instance.points += 1;
                GameManager.Instance.mult += 1;

                // Update the score
                GameManager.Instance.ScoreUpdate(GameManager.Instance.points, GameManager.Instance.mult);
            }
        }
    }
}
