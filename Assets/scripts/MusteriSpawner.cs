using UnityEngine;
using System.Collections;

public class MusteriSpawner : MonoBehaviour
{
    public GameObject musteriPrefab; // Müþteri prefabý
    public Transform[] spawnNoktalari; // Müþterilerin spawn olacaðý noktalar
    public Transform[] kasalar; // Kasalarýn referansý
    public KasaSistemi kasaSistemi; // Kasa Sistemi referansý
    public float spawnSuresi = 10f; // Kaç saniyede bir müþteri spawn olacak
    public ParaSistemi paraSistemi; 

    void Start()
    {
        StartCoroutine(MusteriOlustur());
    }

    IEnumerator MusteriOlustur()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnSuresi);

            // Rastgele bir spawn noktasý seç
            Transform spawnNoktasi = spawnNoktalari[Random.Range(0, spawnNoktalari.Length)];

            // Yeni müþteri oluþtur
            GameObject yeniMusteri = Instantiate(musteriPrefab, spawnNoktasi.position, Quaternion.identity);

            // Müþterinin istediði balýk türünü rastgele belirle
            MusteriSistemi musteriScript = yeniMusteri.GetComponent<MusteriSistemi>();
            string[] balikTurleri = { "Fish_Common", "Fish_Rare", "Fish_Legendary" };
            musteriScript.istedigiBalikTuru = balikTurleri[Random.Range(0, balikTurleri.Length)];

            //  **Para sistemini müþteriye baðla**
            musteriScript.paraSistemi = paraSistemi;

            // Müþteriyi doðru kasaya yönlendir
            foreach (Transform kasa in kasalar)
            {
                if (musteriScript.istedigiBalikTuru == kasaSistemi.kasaBalikTuru[kasa])
                {
                    musteriScript.hedefKasa = kasa;
                    musteriScript.kasaSistemi = kasaSistemi;
                    break;
                }
            }
        }
    }
}
