using UnityEngine;

public class Ball : MonoBehaviour
{
    public float ballRadius = 0.25f; // Set to match your sphere collider's radius
    private Rigidbody rb;
    public float velocityThreshold = 0.25f; // Define velocity threshold


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // if (rb.position.y >= 0.1f)
        // {
        //     Vector3 vel = rb.velocity;
        //     vel.y = 0f; // Correct assignment
        //     rb.velocity = vel;
        //     print("No Bounce");
        // }




        //SetBallStill();


        // Vector3 linearVelocity = rb.velocity;
        // Vector3 angularVelocity = new Vector3(
        //     -linearVelocity.z / ballRadius,
        //     0,
        //     linearVelocity.x / ballRadius
        // );

        // rb.angularVelocity = -angularVelocity;

    }


    // private void SetBallStill()
    // {

    //     if (rb.velocity.magnitude <= velocityThreshold)
    //     {
    //         rb.velocity *= 0.1f;
    //     }
    //     if (rb.velocity.magnitude <= velocityThreshold / 10)
    //     {
    //         rb.velocity = Vector3.zero;
    //     }

    // }

    // void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.gameObject.CompareTag("Ball"))
    //     {
    //         Vector3 collisionNormal = collision.contacts[0].normal;

    //         Vector3 cueBallVelocity = rb.velocity;

    //         float impactFactor = Vector3.Dot(cueBallVelocity.normalized, -collisionNormal);

    //         impactFactor = Mathf.Clamp(impactFactor, 0.1f, 0.8f);

    //         rb.velocity *= (1f - impactFactor);
    //     }
    // }

}