using System.Collections;
using UnityEngine;
using TMPro;

public class KichBanIntro_S3 : MonoBehaviour
{
    public CanvasGroup manHinhDenGroup; // Chứa nguyên cái bảng đen
    public TextMeshProUGUI textDocThoai; // Chứa dòng chữ
    
    [Header("Cài đặt Thời gian (Giây)")]
    public float thoiGianDoc = 5f; // Cho người chơi 4 giây để đọc chữ
    public float tocDoFade = 1f;   // Tốc độ mờ đi (1 giây)

    void Start()
    {
        // Vừa vào game là bôi đen toàn màn hình, giấu chữ đi
        manHinhDenGroup.alpha = 1f; 
        textDocThoai.color = new Color(textDocThoai.color.r, textDocThoai.color.g, textDocThoai.color.b, 0f); // Chữ tàng hình

        // Bắt đầu bấm máy quay!
        StartCoroutine(ChayKichBan());
    }

    IEnumerator ChayKichBan()
    {
        // 1. Chờ 1 giây cho yên tĩnh tĩnh lặng
        yield return new WaitForSeconds(1f);

        // 2. Chữ từ từ HIỆN LÊN
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * tocDoFade;
            textDocThoai.color = new Color(textDocThoai.color.r, textDocThoai.color.g, textDocThoai.color.b, t);
            yield return null;
        }

        // 3. ĐỨNG IM cho người chơi đọc (4 giây)
        yield return new WaitForSeconds(thoiGianDoc);

        // 4. Chữ từ từ MỜ ĐI
        t = 1f;
        while (t > 0f)
        {
            t -= Time.deltaTime * tocDoFade;
            textDocThoai.color = new Color(textDocThoai.color.r, textDocThoai.color.g, textDocThoai.color.b, t);
            yield return null;
        }

        // 5. Màn hình đen từ từ SÁNG LÊN (biến mất) để lộ ra game
        t = 1f;
        while (t > 0f)
        {
            t -= Time.deltaTime * tocDoFade;
            manHinhDenGroup.alpha = t;
            yield return null;
        }

        // 6. Xong phim, tắt luôn cái UI này cho nhẹ máy
        manHinhDenGroup.gameObject.SetActive(false);
    }
}