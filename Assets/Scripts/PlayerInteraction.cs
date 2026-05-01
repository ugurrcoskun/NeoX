using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Etkileşim Ayarları")]
    public float pickupRange = 3f;
    public Transform holdPosition; // Objenin tutulacağı nokta
    public string interactableTag = "Interactable";

    private GameObject heldObject;
    private Rigidbody heldObjectRb;
    private GameObject currentHoverObject;

    void Update()
    {
        HandleHover();
        
        // Farenin sol tıkı ile alma/bırakma
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (heldObject == null)
            {
                TryPickup();
            }
            else
            {
                DropObject();
            }
        }
    }

    private void HandleHover()
    {
        if (heldObject != null) return;

        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
        {
            if (hit.collider.CompareTag(interactableTag))
            {
                if (currentHoverObject != hit.collider.gameObject)
                {
                    currentHoverObject = hit.collider.gameObject;
                }
            }
            else
            {
                currentHoverObject = null;
            }
        }
        else
        {
            currentHoverObject = null;
        }
    }

    private void TryPickup()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
        {
            if (hit.collider.CompareTag(interactableTag))
            {
                heldObject = hit.collider.gameObject;
                heldObjectRb = heldObject.GetComponent<Rigidbody>();

                if (heldObjectRb != null)
                {
                    heldObjectRb.isKinematic = true; // Fiziği durdur
                    heldObjectRb.useGravity = false;
                }

                if (holdPosition != null)
                {
                    heldObject.transform.SetParent(holdPosition);
                    heldObject.transform.localPosition = Vector3.zero;
                    heldObject.transform.localRotation = Quaternion.identity;
                }
            }
        }
    }

    private void DropObject()
    {
        if (heldObject != null)
        {
            heldObject.transform.SetParent(null); // Serbest bırak

            if (heldObjectRb != null)
            {
                heldObjectRb.isKinematic = false; // Fiziği tekrar başlat
                heldObjectRb.useGravity = true;
                heldObjectRb = null;
            }

            heldObject = null;
        }
    }
}