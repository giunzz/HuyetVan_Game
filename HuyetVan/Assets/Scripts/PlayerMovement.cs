using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (controller == null)
            Debug.LogError("❌ KHÔNG TÌM THẤY CharacterController!");
        else
            Debug.Log("✅ CharacterController OK");
    }

    // ✅ THÊM HÀM NÀY: reset velocity mỗi khi script được enable lại
    void OnEnable()
    {
        velocity = Vector3.zero;
    }

    void Update()
    {
        if (controller == null) return;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (x != 0 || z != 0)
            Debug.Log($"Input X={x}, Z={z}");

        // ✅ FIX: chỉ reset velocity.y khi đang đứng trên mặt đất
        if (controller.isGrounded)
            velocity.y = -2f;

        Vector3 move = transform.right * x + transform.forward * z;

        if (move != Vector3.zero)
            Debug.Log($"Move vector: {move}");

        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}