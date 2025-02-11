using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI paraText;
    public TextMeshProUGUI balikTutmaText; // "Bal�k tutma b�lgesindesin" yaz�s�
    public TextMeshProUGUI balikDurumText; // "Bekleniyor", "Bal�k ka�t�", vb.
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
