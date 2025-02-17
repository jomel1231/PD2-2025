using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TweezerInteraction : MonoBehaviour
{
    private Transform currentScrew; // The screw being picked up
    private bool isHoldingScrew = false;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object is a screw and the tweezer is not holding one
        if (other.CompareTag("Screw") && !isHoldingScrew)
        {
            isHoldingScrew = true;
            currentScrew = other.transform; // Assign the screw being picked up
            PickUpScrew(currentScrew);
        }
    }

    private void PickUpScrew(Transform screwTransform)
    {
        // Attach the screw to the tweezer and position it accordingly
        screwTransform.SetParent(transform);
        screwTransform.localPosition = Vector3.zero; // Adjust as needed
        screwTransform.localRotation = Quaternion.identity; // Adjust as needed
        Debug.Log("Screw picked up with tweezer.");
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the tweezer is releasing the screw
        if (other.CompareTag("Screw") && isHoldingScrew)
        {
            isHoldingScrew = false;
            PlaceScrewInSocket(currentScrew);
        }
    }

    private void PlaceScrewInSocket(Transform screwTransform)
    {
        // Find the first available socket and place the screw there
        Collider[] sockets = Physics.OverlapSphere(transform.position, 1f, 1 << LayerMask.NameToLayer("Socket"));
        if (sockets.Length > 0)
        {
            Transform socket = sockets[0].transform;
            screwTransform.SetParent(socket);
            screwTransform.position = socket.position;
            screwTransform.rotation = socket.rotation;
            Debug.Log($"Screw placed in a socket: {socket.name}");
        }
        else
        {
            Debug.Log("No available socket found for the screw.");
        }
    }
}
