using UnityEngine;
using TMPro;

public class Door_of_House : MonoBehaviour
{
    public TextMeshProUGUI vungChu; 
    private bool dungGan = false;
    private Balo baloNhanVat;
    private bool daMoCua = false; 

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !daMoCua)
        {
            baloNhanVat = other.GetComponentInParent<Balo>(); 
            if (baloNhanVat != null)
            {
                dungGan = true;
                if (vungChu != null) vungChu.gameObject.SetActive(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dungGan = false;
            if (vungChu != null) vungChu.gameObject.SetActive(false);
        }
    }

    // Đưa phần kiểm tra chữ xuống Update để nó soi liên tục mỗi giây
    void Update()
    {
        if (dungGan && !daMoCua && baloNhanVat != null)
        {
            // 1. LIÊN TỤC CẬP NHẬT DÒNG CHỮ DỰA THEO BALO
            if (vungChu != null)
            {
                if (baloNhanVat.coChiaKhoaChinh == true)
                    vungChu.text = "Bấm [E] để mở cửa";
                else
                    vungChu.text = "Bạn cần Chìa Khóa cửa chính để mở";
            }

            // 2. CHỜ NGƯỜI CHƠI BẤM E
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Phải có chìa mới cho vặn ổ khóa
                if (baloNhanVat.coChiaKhoaChinh == true)
                {
                    transform.Rotate(0, 90f, 0); 
                    daMoCua = true; 
                    if (vungChu != null) vungChu.gameObject.SetActive(false); 
                }
            }
        }
    }
}