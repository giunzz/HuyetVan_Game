using UnityEngine;
using TMPro;

public class BanDo_S3 : MonoBehaviour
{
    public TextMeshProUGUI vungChu;
    private bool dungGan = false;
    private Balo baloNhanVat;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            baloNhanVat = other.GetComponentInParent<Balo>();
            if (baloNhanVat != null)
            {
                dungGan = true;
                if (vungChu != null) { vungChu.gameObject.SetActive(true); vungChu.text = ""; }
            }
        }
    }

    void OnTriggerExit(Collider other) { /* Code tắt chữ giống các bản trước */ }

    void Update()
    {
        if (dungGan && Input.GetKeyDown(KeyCode.E) && baloNhanVat != null)
        {
            baloNhanVat.NhatBanDo(); 
            if (vungChu != null) vungChu.gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}