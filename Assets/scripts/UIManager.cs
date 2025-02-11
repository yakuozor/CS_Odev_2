using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI paraText;
    public TextMeshProUGUI balikTutmaText; // "Balýk tutma bölgesindesin" yazýsý
    public TextMeshProUGUI balikDurumText; // "Bekleniyor", "Balýk kaçtý", vb.
    public ParaSistemi paraSistemi;

    void Start()
    {
        ParaGuncelle();
        balikTutmaText.gameObject.SetActive(false);
        balikDurumText.gameObject.SetActive(false);
    }

    public void ParaGuncelle()
    {
        if (paraText != null)
        {
            paraText.text = $"Para: {paraSistemi.toplamPara}";
        }
    }

    public void BalikTutmaAlaninda(bool aktif)
    {
        balikTutmaText.gameObject.SetActive(aktif);
    }

    public void BalikDurumGuncelle(string mesaj)
    {
        balikDurumText.gameObject.SetActive(true);
        balikDurumText.text = mesaj;
        Invoke("BalikDurumKapat", 2f); // 2 saniye sonra kaybolsun
    }

    void BalikDurumKapat()
    {
        balikDurumText.gameObject.SetActive(false);
    }
}
