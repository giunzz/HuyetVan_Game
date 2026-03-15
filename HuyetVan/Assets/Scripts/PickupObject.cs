using UnityEngine;

public class PickupObject : MonoBehaviour
{
    public Transform holdPoint;
    GameObject heldObject;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
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

    void TryPickup()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position,
                            Camera.main.transform.forward,
                            out hit, 5f))
        {
            if (hit.collider.CompareTag("Pickable"))
            {
                heldObject = hit.collider.gameObject;

                Rigidbody rb = heldObject.GetComponent<Rigidbody>();
                rb.isKinematic = true;

                heldObject.transform.position = holdPoint.position;
                heldObject.transform.parent = holdPoint;
            }
        }
    }

    void DropObject()
    {
        heldObject.transform.parent = null;

        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        rb.isKinematic = false;

        heldObject = null;
    }
}