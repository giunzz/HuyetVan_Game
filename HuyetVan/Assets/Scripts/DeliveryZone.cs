using UnityEngine;

public class DeliveryZone : MonoBehaviour
{
    private bool hasItem = false;

    void OnTriggerEnter(Collider other)
    {
        if (hasItem) return;

        if (StoryManager.Instance.ReportDelivery(other.gameObject.name))
        {
            hasItem = true;
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;
            other.transform.rotation = Quaternion.identity;
        }
    }
}