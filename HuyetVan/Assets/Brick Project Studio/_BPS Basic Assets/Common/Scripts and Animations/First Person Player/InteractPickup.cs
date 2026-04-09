using UnityEngine;

public class InteractPickup : MonoBehaviour
{
    [Header("Item")]
    public string itemName = "Diary";

    [Header("UI")]
    public GameObject interactUI;

    [Header("Puzzle System")]
    public ApartmentPuzzle puzzle; // 🔥 kéo vào Inspector

    private bool isPlayerNear = false;
    private bool isPicked = false;

    void Start()
    {
        if (interactUI != null)
            interactUI.SetActive(false);
    }

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Pressed E | Near: " + isPlayerNear);
            Interact();
        }
    }

    void Interact()
    {
        if (isPicked) return;

        Debug.Log("Picked up: " + itemName);

        isPicked = true;

        if (interactUI != null)
            interactUI.SetActive(false);

        // 🔥 GỌI SYSTEM CHUẨN
        if (puzzle != null)
        {
            puzzle.OpenPuzzle();
        }
        else
        {
            Debug.LogError("Puzzle reference missing!");
        }

        // Ẩn object sau khi nhặt
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered");

            isPlayerNear = true;

            if (interactUI != null)
                interactUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited");

            isPlayerNear = false;

            if (interactUI != null)
                interactUI.SetActive(false);
        }
    }
}