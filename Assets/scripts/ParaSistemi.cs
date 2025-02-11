using UnityEngine;

public class ParaSistemi : MonoBehaviour
{
    public int toplamPara = 0; // Oyuncunun paras�
    private UIManager uiManager; // UI y�neticisini tutacak de�i�ken

    void Start()
    {
        uiManager = FindObjectOfType<UIManager>(); // UIManager�i otomatik bul
    }

    public void ParaEkle(int miktar)
    {
        toplamPara += miktar;
        Debug.Log($"Para kazan�ld�! �u anki toplam para: {toplamPara} coin");

        
        if (uiManager != null)
        {
            uiManager.ParaGuncelle();
        }
    }
}
