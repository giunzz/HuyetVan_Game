using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float crouchSpeed = 2.5f;
    public float gravity = -9.81f;

    [Header("Crouch")]
    public float standHeight = 2f;
    public float crouchHeight = 1f;
    public Transform playerCamera;
    public float cameraStandY = 1.6f;
    public float cameraCrouchY = 1f;

    private CharacterController controller;
    private Vector3 velocity;

    private bool isCrouching = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (controller == null)
            Debug.LogError("❌ KHÔNG TÌM THẤY CharacterController!");
        else
            Debug.Log("✅ CharacterController OK");
    }

    void OnEnable()
    {
        velocity = Vector3.zero;
    }

    void Update()
    {
        if (controller == null) return;

        HandleCrouch();

        float currentSpeed = isCrouching ? crouchSpeed : speed;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (controller.isGrounded)
            velocity.y = -2f;

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * currentSpeed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleCrouch()
    {
        // Nhấn Ctrl để toggle
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = !isCrouching;

            if (isCrouching)
                Crouch();
            else
                StandUp();
        }
    }

    void Crouch()
    {
        controller.height = crouchHeight;
        controller.center = new Vector3(0, crouchHeight / 2f, 0);

        if (playerCamera != null)
            playerCamera.localPosition = new Vector3(0, cameraCrouchY, 0);

        Debug.Log("🪑 Đang ngồi");
    }

    void StandUp()
    {
        controller.height = standHeight;
        controller.center = new Vector3(0, standHeight / 2f, 0);

        if (playerCamera != null)
            playerCamera.localPosition = new Vector3(0, cameraStandY, 0);

        Debug.Log("🧍 Đứng dậy");
    }
}