using UnityEngine;

public class ParaSistemi : MonoBehaviour
{
    public int toplamPara = 0; // Oyuncunun parasý
    private UIManager uiManager; // UI yöneticisini tutacak deðiþken

    void Start()
    {
        uiManager = FindObjectOfType<UIManager>(); // UIManager’i otomatik bul
    }

    public void ParaEkle(int miktar)
    {
        toplamPara += miktar;
        Debug.Log($"Para kazanýldý! Þu anki toplam para: {toplamPara} coin");

        
        if (uiManager != null)
        {
            uiManager.ParaGuncelle();
        }
    }
}
