using UnityEngine;
using System.Collections;

public class DoubleDoorController : MonoBehaviour
{
    [Header("2 cánh cửa")]
    public Transform doorLeft;
    public Transform doorRight;

    [Header("Cài đặt")]
    public float openAngle   = 90f;
    public float openSpeed   = 2f;
    public float interactRange = 2.5f;
    public KeyCode interactKey = KeyCode.E;

    [Header("UI Hint (tuỳ chọn)")]
    public GameObject interactHint;

    private bool _isOpen    = false;
    private bool _isMoving  = false;

    private Quaternion _leftClosed,  _leftOpen;
    private Quaternion _rightClosed, _rightOpen;
    private Transform  _player;

    void Start()
    {
        // Lưu rotation ban đầu
        _leftClosed  = doorLeft.rotation;
        _rightClosed = doorRight.rotation;

        // Cánh trái mở vào trong = xoay âm Y
        // Cánh phải mở vào trong = xoay dương Y
        _leftOpen  = Quaternion.Euler(doorLeft.eulerAngles  + new Vector3(0, -openAngle, 0));
        _rightOpen = Quaternion.Euler(doorRight.eulerAngles + new Vector3(0,  openAngle, 0));

        _player = GameObject.FindWithTag("Player").transform;

        if (interactHint != null)
            interactHint.SetActive(false);
    }

    void Update()
    {
        if (_player == null) return;

        float dist    = Vector3.Distance(transform.position, _player.position);
        bool  inRange = dist <= interactRange;

        if (interactHint != null)
            interactHint.SetActive(inRange && !_isOpen);

        if (inRange && Input.GetKeyDown(interactKey) && !_isMoving)
            StartCoroutine(ToggleDoors());
    }

    IEnumerator ToggleDoors()
    {
        _isMoving = true;

        Quaternion leftTarget  = _isOpen ? _leftClosed  : _leftOpen;
        Quaternion rightTarget = _isOpen ? _rightClosed : _rightOpen;

        // Xoay cả 2 cánh cùng lúc
        while (Quaternion.Angle(doorLeft.rotation, leftTarget) > 0.1f)
        {
            doorLeft.rotation  = Quaternion.Lerp(doorLeft.rotation,  leftTarget,  openSpeed * Time.deltaTime);
            doorRight.rotation = Quaternion.Lerp(doorRight.rotation, rightTarget, openSpeed * Time.deltaTime);
            yield return null;
        }

        doorLeft.rotation  = leftTarget;
        doorRight.rotation = rightTarget;

        _isOpen   = !_isOpen;
        _isMoving = false;
    }
}