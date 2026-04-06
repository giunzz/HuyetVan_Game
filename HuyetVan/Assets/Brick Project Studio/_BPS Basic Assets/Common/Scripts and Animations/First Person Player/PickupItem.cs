using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public string itemName = "Diary";
    private bool isPlayerNear = false;

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            Pickup();
        }
    }

    void Pickup()
    {
        Debug.Log("Picked up: " + itemName);

        // TODO: inventory
        // InventoryManager.Instance.AddItem(itemName);

        Destroy(gameObject); // 🔥 tốt hơn SetActive(false)
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            Debug.Log("Press E to pick up");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
        }
    }
}