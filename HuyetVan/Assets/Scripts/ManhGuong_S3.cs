using UnityEngine;
using TMPro;

public class ManhGuong_S3 : MonoBehaviour
{
    public TextMeshProUGUI vungChu; 
    private bool dungGan = false;
    private Balo baloNhanVat;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Dùng GetComponentInParent: Tìm script Balo ở cả thằng cha lẫn thằng con
            baloNhanVat = other.GetComponentInParent<Balo>();

            // Chỉ khi CHẮC CHẮN tìm thấy Balo thì mới cho phép tương tác
            if (baloNhanVat != null) 
            {
                dungGan = true;
                if (vungChu != null) 
                {
                    vungChu.gameObject.SetActive(true);
                    vungChu.text = "Bấm [E] để nhặt mảnh gương";
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dungGan = false;
            baloNhanVat = null; // Đi xa thì reset bộ nhớ
            
            if (vungChu != null)
            {
                vungChu.gameObject.SetActive(false);
            }
        }
    }

    void Update()
    {
        // Thêm khóa an toàn: baloNhanVat != null
        if (dungGan && Input.GetKeyDown(KeyCode.E) && baloNhanVat != null)
        {
            baloNhanVat.NhatManhGuong(); 
            
            if (vungChu != null)
            {
                vungChu.gameObject.SetActive(false); 
            }
            
            Destroy(gameObject); // Lần này thì bốc hơi chắc chắn 100%
        }
    }
}