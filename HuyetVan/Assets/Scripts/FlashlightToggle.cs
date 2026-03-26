using UnityEngine;

public class FlashlightToggle : MonoBehaviour
{
    public GameObject denPin; // Biến để chứa cái đèn của ông
    private bool dangBat = false; // Mặc định vô game là tắt đèn

    void Start()
    {
        // Tắt đèn ngay khi vừa vào game
        if (denPin != null)
        {
            denPin.SetActive(false);
        }
    }

    void Update()
    {
        // Kiểm tra xem người chơi có bấm phím F không
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Đảo ngược trạng thái (đang tắt thành bật, đang bật thành tắt)
            dangBat = !dangBat; 
            
            // Áp dụng trạng thái đó cho cái đèn
            denPin.SetActive(dangBat);
        }
    }
}