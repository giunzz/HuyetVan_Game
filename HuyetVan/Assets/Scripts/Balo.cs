using UnityEngine;
using TMPro;

public class Balo : MonoBehaviour
{
    // --- KHU VỰC 1: MẢNH GƯƠNG ---
    public int soManhGuong = 0;
    public TextMeshProUGUI textDemGuong; 

    // --- KHU VỰC 2: 3 Ô CHỨA ĐỒ MỚI ---
    public bool coChiaKhoaChinh = false;
    public GameObject iconChiaKhoaChinh;

    public bool coChiaKhoaCuoi = false;
    public GameObject iconChiaKhoaCuoi;

    public bool coBanDo = false;
    public GameObject iconBanDo;

    // --- KHU VỰC 3: HỆ THỐNG BẢN ĐỒ ---
    public GameObject textHuongDanMap; // Chữ "Bấm M để mở Map"
    public GameObject uiHinhBanDo;     // Cái hình bản đồ to giữa màn hình
    private bool daBamM = false;       // Kiểm tra xem đã bấm M lần nào chưa

    void Start() 
    { 
        Time.timeScale = 1f; 
        
        // Dọn dẹp túi đồ lúc đầu game chống ăn gian
        coChiaKhoaChinh = false;
        coChiaKhoaCuoi = false;
        coBanDo = false;
        
        CapNhatUI(); 
    }

    // Cơ chế bấm M mở Map
    void Update()
    {
        // Chỉ cho phép bấm M khi TRONG TÚI ĐÃ CÓ BẢN ĐỒ
        if (coBanDo == true && Input.GetKeyDown(KeyCode.M))
        {
            // Tắt/bật cái ảnh bản đồ to trên màn hình
            bool dangHienThi = uiHinhBanDo.activeSelf;
            uiHinhBanDo.SetActive(!dangHienThi);

            // Nếu đây là lần đầu tiên bấm M, thì dập tắt dòng chữ hướng dẫn vĩnh viễn
            if (daBamM == false)
            {
                daBamM = true;
                if (textHuongDanMap != null) textHuongDanMap.SetActive(false);
            }
        }
    }

    public void NhatManhGuong() { soManhGuong++; CapNhatUI(); }
    void CapNhatUI() { if (textDemGuong != null) textDemGuong.text = "Mảnh gương: " + soManhGuong + "/9"; }

    public void NhatChiaKhoaChinh() { coChiaKhoaChinh = true; if(iconChiaKhoaChinh != null) iconChiaKhoaChinh.SetActive(true); }
    public void NhatChiaKhoaCuoi() { coChiaKhoaCuoi = true; if(iconChiaKhoaCuoi != null) iconChiaKhoaCuoi.SetActive(true); }

    public void NhatBanDo() 
    { 
        coBanDo = true; 
        if(iconBanDo != null) iconBanDo.SetActive(true); // Hiện ô Icon nhỏ trong kho

        // Lụm xong thì bật dòng chữ hướng dẫn "Bấm M" lên nhắc nhở
        if (textHuongDanMap != null && daBamM == false) 
        {
            textHuongDanMap.SetActive(true);
        }
    }
}