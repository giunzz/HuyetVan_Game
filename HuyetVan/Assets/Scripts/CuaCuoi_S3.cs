using UnityEngine;

public class CuaCuoi_S3 : MonoBehaviour
{
    private bool dungGan = false;
    private Balo baloNhanVat;
    private bool daMoCua = false;

    // THÊM: Biến chứa cái ổ khóa
    public GameObject oKhoa; 

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !daMoCua)
        {
            baloNhanVat = other.GetComponentInParent<Balo>();
            if (baloNhanVat != null) dungGan = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) dungGan = false;
    }

    void Update()
    {
        if (dungGan && Input.GetKeyDown(KeyCode.E) && !daMoCua && baloNhanVat != null)
        {
            if (baloNhanVat.coChiaKhoaCuoi == true)
            {
                transform.Rotate(0, 90f, 0); 
                daMoCua = true;

                // THÊM: Cho ổ khóa tàng hình (rớt mất)
                if (oKhoa != null) oKhoa.SetActive(false); 
            }
        }
    }
}