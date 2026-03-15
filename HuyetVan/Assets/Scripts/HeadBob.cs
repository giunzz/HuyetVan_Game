using UnityEngine;

public class HeadBob : MonoBehaviour
{
    public float bobSpeed = 10f;
    public float bobAmount = 0.05f;

    float defaultY;

    void Start()
    {
        defaultY = transform.localPosition.y;
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if(Mathf.Abs(horizontal) > 0 || Mathf.Abs(vertical) > 0)
        {
            float newY = defaultY + Mathf.Sin(Time.time * bobSpeed) * bobAmount;
            transform.localPosition = new Vector3(
                transform.localPosition.x,
                newY,
                transform.localPosition.z
            );
        }
        else
        {
            transform.localPosition = new Vector3(
                transform.localPosition.x,
                defaultY,
                transform.localPosition.z
            );
        }
    }
}