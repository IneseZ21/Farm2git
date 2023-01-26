using UnityEngine;

/// <summary>
/// Interact Manager that handles how player interact with spawned objects, such as grabbing and throwing.
/// </summary>
public class ObjectInteract : MonoBehaviour
{
    [Header("Interact Settings")]
    [Space(5)]
    [SerializeField] private int throwForce = 850;
    [SerializeField] private float pickUpDistance = 4f;

    [Space(10)] // 10 pixels of spacing here.

    [Header("Interact References")]
    [Space(5)]
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private Transform objectGrabPointTransform;
    [SerializeField] private LayerMask pickUpLayerMask;

    private ObjectInteractable objectInteractable;

    void OnGrab()
    {
        if (objectInteractable == null)
        {
            if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, pickUpDistance, pickUpLayerMask))
            {
                if (raycastHit.transform.TryGetComponent(out objectInteractable))
                {
                    objectInteractable.Grab(objectGrabPointTransform);
                }
            }
        }
        else
        {
            objectInteractable.Drop();
            objectInteractable = null;
        }
    }

    void OnThrow()
    {
        if (objectInteractable != null)
        {
            objectInteractable.Throw(throwForce);
            objectInteractable = null;
        }
    }
}
