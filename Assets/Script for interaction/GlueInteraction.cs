using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GlueInteraction : MonoBehaviour
{
    public Transform screen; // The screen to be glued
    public XRSocketInteractor screenSocket; // The socket where the screen will attach
    public Transform glue; // The glue 3D model
    public GameObject glueEffectPrefab; // Prefab of the glue particle or effect
    public float attachDistance = 0.1f; // The distance the screen will attach to the socket when close enough
    private bool isTouchingSocket = false;
    private bool isScreenGlued = false;
    private GameObject currentGlueEffect; // Current glue effect object

    void Update()
    {
        // Handle the logic for glueing the screen when glue is applied
        if (isTouchingSocket && !isScreenGlued && currentGlueEffect != null)
        {
            // Attach screen to the glue's position
            if (Vector3.Distance(currentGlueEffect.transform.position, screenSocket.transform.position) < attachDistance)
            {
                AttachScreenToSocket();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detect when glue touches the socket or chassis area
        if (other.CompareTag("Chassis"))
        {
            Debug.Log("Glue touched the chassis.");
            isTouchingSocket = true;

            // Instantiate the glue effect at the glue's position
            currentGlueEffect = Instantiate(glueEffectPrefab, glue.position, Quaternion.identity);
            currentGlueEffect.transform.SetParent(glue); // Make glue effect follow glue model
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Reset glue interaction when it leaves the chassis area
        if (other.CompareTag("Chassis"))
        {
            Debug.Log("Glue left the chassis.");
            isTouchingSocket = false;

            // Destroy glue effect if it leaves the area
            if (currentGlueEffect != null)
            {
                Destroy(currentGlueEffect);
                currentGlueEffect = null;
            }
        }
    }

    private void AttachScreenToSocket()
    {
        // Attach the screen to the socket position where the glue was applied
        screen.position = screenSocket.transform.position;
        screen.rotation = screenSocket.transform.rotation;
        screen.SetParent(screenSocket.transform); // Attach the screen to the socket
        isScreenGlued = true; // Mark the screen as glued

        // Optional: Reset glue effect
        Destroy(currentGlueEffect);
        currentGlueEffect = null;

        Debug.Log("Screen successfully glued to the socket.");
    }
}
