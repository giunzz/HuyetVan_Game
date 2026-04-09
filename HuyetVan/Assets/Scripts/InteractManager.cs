using UnityEngine;

public class InteractManager : MonoBehaviour
{
    [Header("Cài đặt")]
    public float interactRange = 3f;
    public Transform holdPoint;

    private GameObject _heldObject;
    private Rigidbody _heldRb;

    public static bool HasSample = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_heldObject != null)
            {
                DropObject();
                return;
            }

            Camera mainCam = Camera.main;
            if (mainCam == null)
            {
                Debug.LogWarning("Không tìm thấy Main Camera!");
                return;
            }

            Ray ray = new Ray(mainCam.transform.position, mainCam.transform.forward);
            RaycastHit hit;

            int interactableLayer = LayerMask.GetMask("Interactable");
            Debug.DrawRay(ray.origin, ray.direction * interactRange, Color.red, 2f);

            if (Physics.Raycast(ray, out hit, interactRange, interactableLayer))
            {
                string tag = hit.collider.tag.Trim();
                Debug.Log("Interact: " + tag);

                switch (tag)
                {
                    case "Pickup":   PickupObject(hit.collider.gameObject); break;
                    case "Scalpel":  HandleScalpelPickup(hit.collider.gameObject); break;
                    case "QTip":     HandleQTipPickup(hit.collider.gameObject); break;
                    case "Corpse":   HandleCorpse(); break;
                   case "NewCorpse":
                                    HandleQTipUse(hit.collider.gameObject);
                                    ShowCorpseRevealDialogue();
                                    break;
                    case "Microscope": HandleMicroscope(hit.collider.gameObject); break;
                    case "ExitDoor": HandleExitDoor(); break;
                    default: Debug.Log("Không có tương tác: " + tag); break;
                }
            }
        }

        if (_heldObject != null)
            MoveHeldObject();
    }
        void ShowCorpseRevealDialogue()
    {
        if (MonologueManager.Instance != null)
        {
            MonologueManager.Instance.Show(
                "Mai...? Không... Không thể nào! Chuyện quái gì đang xảy ra thế này?!\n" +
                "Hồ sơ ghi là người vô gia cư cơ mà! Bọn khốn, các người đã làm gì vợ tôi?!"
            );
        }
    }

    // ================= PICKUP OBJECT (đồ thường) =================
    void PickupObject(GameObject obj)
    {
        _heldObject = obj;
        _heldRb = _heldObject.GetComponent<Rigidbody>();

        if (_heldRb == null)
            _heldRb = _heldObject.AddComponent<Rigidbody>();

        _heldRb.useGravity = false;
        _heldRb.isKinematic = true;
        _heldRb.linearVelocity = Vector3.zero;
        _heldRb.angularVelocity = Vector3.zero;

        _heldObject.transform.SetParent(holdPoint);
        _heldObject.transform.localPosition = Vector3.zero;
        _heldObject.transform.localRotation = Quaternion.identity;
    }

    void MoveHeldObject()
    {
        _heldObject.transform.position = Vector3.Lerp(
            _heldObject.transform.position,
            holdPoint.position,
            Time.deltaTime * 15f
        );

        _heldObject.transform.rotation = Quaternion.Lerp(
            _heldObject.transform.rotation,
            holdPoint.rotation,
            Time.deltaTime * 15f
        );
    }

    void DropObject()
    {
        if (_heldObject == null) return;

        _heldObject.transform.SetParent(null);

        if (_heldRb != null)
        {
            _heldRb.useGravity = true;
            _heldRb.isKinematic = false;
        }

        _heldObject = null;
        _heldRb = null;
    }

    // ================= INVENTORY PICKUP =================
    void HandleScalpelPickup(GameObject obj)
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddScalpelToBag();
            obj.SetActive(false);
        }
    }

    void HandleQTipPickup(GameObject obj)
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddQTipToBag();
            obj.SetActive(false);
        }
    }

    // ================= USE QTIP =================
    void HandleQTipUse(GameObject corpse)
    {
        bool isHoldingQTip = false;

        if (InventoryManager.Instance != null)
        {
            isHoldingQTip = InventoryManager.Instance.IsQTipEquipped();
        }

        if (!isHoldingQTip)
        {
            Debug.Log("Cần cầm QTip!");
            return;
        }

        Debug.Log("🧪 Lấy mẫu!");

        Vector3 attachPos = corpse.transform.position + Vector3.up * 1f;

        HasSample = true;
        if (InventoryManager.Instance != null) InventoryManager.Instance.SetQTipHasSample();
        Debug.Log("✅ Đã lấy mẫu!");
    }

    // ================= CORPSE =================
    void HandleCorpse()
    {
        bool isHoldingScalpel = false;

        if (InventoryManager.Instance != null)
        {
            isHoldingScalpel = InventoryManager.Instance.IsScalpelEquipped();
        }

        if (!isHoldingScalpel)
        {
            if (MonologueManager.Instance != null)
            {
                MonologueManager.Instance.Show(
                    "Mình không thể mổ bằng tay không được.\nDao mổ của mình đâu?"
                );
            }
            return;
        }

        Debug.Log("Bắt đầu mổ xác!");

        if (MonologueManager.Instance != null)
        {
            MonologueManager.Instance.Show("Đã sẵn sàng. Bắt đầu phẫu thuật.");
        }

        if (Autopsy2DMiniGame.Instance != null)
        {
            Autopsy2DMiniGame.Instance.OpenMiniGame();
        }
    }

    // ================= MICROSCOPE =================
    void HandleMicroscope(GameObject micro)
    {
        if (!HasSample)
        {
            Debug.Log("❌ Cần lấy mẫu trước!");
            return;
        }

        Debug.Log("🔬 Xét nghiệm");

        Microscope microscope = micro.GetComponent<Microscope>();
        if (microscope != null)
        {
            microscope.Interact();
        }
    }

    // ================= EXIT =================
    void HandleExitDoor()
    {
        if (MonologueManager.Instance != null)
        {
            MonologueManager.Instance.Show(
                "Chưa xong việc, không thể rời đi."
            );
        }
    }
}