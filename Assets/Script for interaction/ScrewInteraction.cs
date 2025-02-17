using UnityEngine;

public class ScrewInteraction : MonoBehaviour
{
    public Transform screw; // Assign the screw in the Inspector
    public Transform screwdriver; // Assign the screwdriver in the Inspector
    public float rotationSpeed = 80f; // Adjust rotation speed
    public float unscrewDistance = 0.05f; // How far the screw moves out when unscrewed
    public float screwBackDistance = 0.05f; // How far the screw moves back when inserted
    public AudioSource audioSource;  // Assign an AudioSource in the Inspector
    public AudioClip screwRemovedClip; // Assign a sound clip for unscrewing

    private bool isInteracting = false;
    private bool isUnscrewed = false; // Track if the screw is already unscrewed
    private float unscrewedAmount = 0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Screwdriver"))
        {
            Debug.Log("Screwdriver detected near the screw.");
            isInteracting = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Screwdriver"))
        {
            Debug.Log("Screwdriver moved away from the screw.");
            isInteracting = false;
        }
    }

    private void Update()
    {
        if (isInteracting && !isUnscrewed)
        {
            // Rotate the screw
            screw.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

            // Move the screw outward gradually
            unscrewedAmount += Time.deltaTime * 0.01f;
            screw.position += screw.up * Time.deltaTime * 0.01f;

            // Check if the screw has been fully unscrewed
            if (unscrewedAmount >= unscrewDistance)
            {
                Debug.Log("Screw successfully unscrewed!");
                isUnscrewed = true;
                unscrewedAmount = 0f;

                // ✅ Play audio when the screw is fully removed
                if (audioSource && screwRemovedClip)
                {
                    audioSource.PlayOneShot(screwRemovedClip);
                }
            }
        }
        // Handle screwing back logic
        else if (isInteracting && isUnscrewed)
        {
            // Rotate the screw in the opposite direction
            screw.Rotate(Vector3.down * rotationSpeed * Time.deltaTime);

            // Move the screw back into place gradually
            unscrewedAmount += Time.deltaTime * 0.01f;
            screw.position -= screw.up * Time.deltaTime * 0.01f;

            // Check if the screw has been fully screwed back into place
            if (unscrewedAmount >= screwBackDistance)
            {
                Debug.Log("Screw successfully inserted!");
                isUnscrewed = false; // Mark the screw as re-inserted
                Destroy(screw.gameObject); // Optional: Remove the screw if needed
            }
        }
    }
}
