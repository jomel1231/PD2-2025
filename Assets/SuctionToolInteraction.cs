using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SuctionToolInteraction : MonoBehaviour
{
    public XRGrabInteractable suctionTool; // Reference to the XR Grab Interactable
    public GameObject phoneScreen;         // Reference to the phone screen
    private bool isAttached = false;       // Check if screen is attached to the suction tool

    private void OnTriggerEnter(Collider other)
    {
        // Check if the suction tool collides with the phone screen
        if (other.CompareTag("PhoneScreen") && !isAttached) // Only interact if not already attached
        {
            StartCoroutine(DetachAndAttachScreen(other.gameObject));
        }
    }

    IEnumerator DetachAndAttachScreen(GameObject screen)
    {
        // Detach the screen from the phone (disabling XR Grab)
        screen.GetComponent<XRGrabInteractable>().enabled = false; // Disable grabbing for the screen
        screen.GetComponent<Collider>().enabled = false; // Disable the collider so it doesn’t interact further

        // Parent the screen to the suction tool for 3 seconds
        screen.transform.parent = suctionTool.transform; // Attach the screen to the suction tool
        isAttached = true;

        // Optional: Add a sound or effect here when suction tool attaches
        // PlaySuctionSound();

        // Wait for 3 seconds while the screen is attached to the suction tool
        yield return new WaitForSeconds(3f);

        // After 3 seconds, remove the screen from the suction tool
        screen.transform.parent = null; // Unparent the screen from the suction tool
        screen.GetComponent<Collider>().enabled = true; // Re-enable the screen collider
        screen.GetComponent<XRGrabInteractable>().enabled = true; // Re-enable XR grabbing

        // Start the countdown for the screen to fall within 3 minutes
        StartCoroutine(ScreenFallAfterTime(screen, 1f)); // 180 seconds (3 minutes)
        isAttached = false;
    }

    IEnumerator ScreenFallAfterTime(GameObject screen, float delay)
    {
        yield return new WaitForSeconds(delay);

        // Apply gravity or physics effect to make the screen fall
        if (screen != null && screen.GetComponent<Rigidbody>() != null)
        {
            Rigidbody rb = screen.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = screen.AddComponent<Rigidbody>(); // Add Rigidbody if not already present
            }

            yield return new WaitForSeconds(0.1f); // Small delay to allow transition
            rb.isKinematic = false;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ; // Allow physics interactions
            rb.useGravity = true; // Enable gravity so the screen falls
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Table"))
        {
            // Ensure the table has the "Table" tag and the collider is correctly set
            Rigidbody rb = phoneScreen.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Debug.Log("Screen collided with table");

                rb.isKinematic = true; // Stop falling when it lands on the table
                rb.useGravity = false; // Turn off gravity to keep it stationary
            }
        }
    }
}
