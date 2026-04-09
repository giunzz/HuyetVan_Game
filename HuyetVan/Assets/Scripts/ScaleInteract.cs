using UnityEngine;

public class ScaleInteract : MonoBehaviour
{
    private bool isNear = false;

    void OnTriggerEnter(Collider other) { if (other.CompareTag("Player")) isNear = true; }
    void OnTriggerExit(Collider other) { if (other.CompareTag("Player")) isNear = false; }

    void Update()
    {
        if (isNear && Input.GetKeyDown(KeyCode.E))
        {
            StoryManager.Instance.InteractWithScale();
        }
    }
}