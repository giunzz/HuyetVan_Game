using UnityEngine;
using TMPro; 

public class HuongDanManager : MonoBehaviour
{
    public GameObject vungChuHuongDan;

    void Start()
    {
        vungChuHuongDan.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            vungChuHuongDan.SetActive(false);
            
            Destroy(this); 
        }
    }
}