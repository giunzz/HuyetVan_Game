using UnityEngine;

public class InteractManager : MonoBehaviour
{
    [Header("Cài đặt")]
    public float interactRange = 3f;
    public Transform holdPoint;

    [Header("State")]
    public static bool HasScalpel = false;

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

            // 1. Lấy Camera chính làm điểm xuất phát của tia Raycast (Thay vì tâm nhân vật)
            Camera mainCam = Camera.main;
            if (mainCam == null)
            {
                Debug.LogWarning("Không tìm thấy Main Camera trong scene!");
                return;
            }

            // Tạo tia bắn từ vị trí Camera hướng thẳng về phía trước
            Ray ray = new Ray(mainCam.transform.position, mainCam.transform.forward);
            RaycastHit hit;

            int interactableLayer = LayerMask.GetMask("Interactable");
            Debug.DrawRay(ray.origin, ray.direction * interactRange, Color.red, 2f);

            if (Physics.Raycast(ray, out hit, interactRange, interactableLayer))
            {
                string tag = hit.collider.tag.Trim();
                Debug.Log("Interact: " + tag + " | Object: " + hit.collider.name);

                switch (tag)
                {
                    case "Pickup":   PickupObject(hit.collider.gameObject); break;
                    case "Door":     break;
                    case "Corpse":   HandleCorpse(); break;
                    case "ExitDoor": HandleExitDoor(); break;
                    case "Scalpel":  HandleScalpelPickup(hit.collider.gameObject); break; 
                    case "QTip": PickupObject(hit.collider.gameObject); break;
                    case "NewCorpse": HandleQTipUse(hit.collider.gameObject); break;
                    case "Microscope": HandleMicroscope(hit.collider.gameObject); break;
                    default: Debug.Log("Không có tương tác: " + tag); break;
                }
            }
            else
            {
                Debug.Log("Tia Raycast không trúng vật thể nào trong tầm với.");
            }
        }

        // Di chuyển vật đang cầm trên tay
        if (_heldObject != null)
            MoveHeldObject();
    }
        void HandleMicroscope(GameObject micro)
    {
        if (!HasSample)
        {
            Debug.Log("❌ Cần lấy mẫu trước!");
            return;
        }

        Debug.Log("🔬 Bắt đầu xét nghiệm");

        Microscope microscope = micro.GetComponent<Microscope>();
        if (microscope != null)
        {
            microscope.Interact();
        }
    }
    // ── PICKUP ──────────────────────────────────────────
    void PickupObject(GameObject obj)
    {
        _heldObject = obj;
        _heldRb = _heldObject.GetComponent<Rigidbody>();

        if (_heldRb == null)
            _heldRb = _heldObject.AddComponent<Rigidbody>();

        _heldRb.useGravity      = false;
        _heldRb.isKinematic     = true;
        _heldRb.linearVelocity  = Vector3.zero;
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
    void HandleQTipUse(GameObject corpse)
    {
        if (_heldObject == null || _heldObject.tag != "QTip")
        {
            Debug.Log("Cần cầm Q-tip!");
            return;
        }

        Debug.Log("🧪 Lấy mẫu!");

        Vector3 attachPos = corpse.transform.position + Vector3.up * 1f;

        _heldObject.transform.SetParent(corpse.transform);
        _heldObject.transform.position = attachPos;
        _heldObject.transform.rotation = Quaternion.identity;

        if (_heldRb != null)
        {
            _heldRb.isKinematic = true;
            _heldRb.useGravity = false;
        }

        _heldObject = null;
        _heldRb = null;

        // ✅ QUAN TRỌNG
        HasSample = true;

        Debug.Log("✅ Đã lấy mẫu!");

        // 👉 bật microscope
        if (Microscope.Instance != null)
        {
            Microscope.Instance.EnableMicroscope();
        }
    }
    void DropObject()
    {
        if (_heldObject == null) return;

        _heldObject.transform.SetParent(null);

        if (_heldRb != null)
        {
            _heldRb.useGravity      = true;
            _heldRb.isKinematic     = false;
            _heldRb.linearVelocity  = Vector3.zero;
            _heldRb.angularVelocity = Vector3.zero;
        }

        _heldObject = null;
        _heldRb     = null;
    }

    // ── XỬ LÝ NHẶT DAO MỔ BỎ TÚI ĐỒ ──────────────────────
    void HandleScalpelPickup(GameObject obj)
    {
        if (InventoryManager.Instance != null)
        {
            // Báo cho túi đồ cộng dao vào
            InventoryManager.Instance.AddScalpelToBag();

            // Làm con dao ngoài thế giới biến mất (không cần xóa, chỉ cần tắt)
            obj.SetActive(false); 
        }
        else
        {
            Debug.LogError("⚠️ Không tìm thấy InventoryManager trong scene! Bạn đã tạo object quản lý nó chưa?");
        }
    }

    // ── TƯƠNG TÁC ────────────────────────────────────────
    void HandleCorpse()
    {
        // KIỂM TRA XEM CÓ ĐANG CẦM DAO TRÊN TAY HAY KHÔNG THÔNG QUA TÚI ĐỒ
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
                    "Mình không thể mổ bằng tay không được.\nDao mổ của mình để đâu rồi?"
                );
            }
            else
            {
                Debug.Log("Cần gọi UI: Mình không thể mổ bằng tay không được...");
            }
        }
        else
        {
            Debug.Log("Bắt đầu mổ xác!");
            if (MonologueManager.Instance != null)
            {
                MonologueManager.Instance.Show("Đã sẵn sàng. Bắt đầu phẫu thuật thôi.");
            }
            
            // LIÊN KẾT GỌI MINI GAME 2D Ở ĐÂY
            if (Autopsy2DMiniGame.Instance != null)
            {
                Autopsy2DMiniGame.Instance.OpenMiniGame();
            }
            else
            {
                Debug.LogWarning("⚠️ Chưa tìm thấy Autopsy2DMiniGame trong scene!");
            }
        }
    }

    void HandleExitDoor()
    {
        if (MonologueManager.Instance != null)
        {
            MonologueManager.Instance.Show(
                "Chưa xong việc, mình không thể rời đi.\nNguyên nhân cái chết vẫn còn là ẩn số."
            );
        }
        else
        {
            Debug.Log("Chưa xong việc, mình không thể rời đi...");
        }
    }
}