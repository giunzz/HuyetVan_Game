using UnityEngine;

public class PickupObject : MonoBehaviour
{
    public Transform holdPoint;
    public float pickupRange = 5f;

    private GameObject heldObject;
    private Rigidbody heldRb;

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
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        // 🔴 VẼ TIA RAY (nhìn trong Scene)
        Debug.DrawRay(ray.origin, ray.direction * pickupRange, Color.red, 2f);

        if (Physics.Raycast(ray, out hit, pickupRange))
        {
            // 🔵 XEM RAY ĐANG CHẠM CÁI GÌ
            Debug.Log("Hit: " + hit.collider.name);

            if (hit.collider.CompareTag("Pickup"))
            {
                Debug.Log("ĐÚNG OBJECT → PICKUP");

                heldObject = hit.collider.gameObject;
                heldRb = heldObject.GetComponent<Rigidbody>();

                heldRb.useGravity = false;
                heldRb.isKinematic = true;

                heldObject.transform.position = holdPoint.position;
                heldObject.transform.parent = holdPoint;
            }
            else
            {
                Debug.Log("Sai tag: " + hit.collider.tag);
            }
        }
        else
        {
            Debug.Log("KHÔNG HIT GÌ");
        }
    }
    void DropObject()
    {
        heldRb.useGravity = true;
        heldRb.isKinematic = false;

        heldObject.transform.parent = null;

        heldObject = null;
        heldRb = null;
    }
}