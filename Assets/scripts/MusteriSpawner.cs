using UnityEngine;
using System.Collections;

public class MusteriSpawner : MonoBehaviour
{
    public GameObject musteriPrefab; // M��teri prefab�
    public Transform[] spawnNoktalari; // M��terilerin spawn olaca�� noktalar
    public Transform[] kasalar; // Kasalar�n referans�
    public KasaSistemi kasaSistemi; // Kasa Sistemi referans�
    public float spawnSuresi = 10f; // Ka� saniyede bir m��teri spawn olacak
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

            // Rastgele bir spawn noktas� se�
            Transform spawnNoktasi = spawnNoktalari[Random.Range(0, spawnNoktalari.Length)];

            // Yeni m��teri olu�tur
            GameObject yeniMusteri = Instantiate(musteriPrefab, spawnNoktasi.position, Quaternion.identity);

            // M��terinin istedi�i bal�k t�r�n� rastgele belirle
            MusteriSistemi musteriScript = yeniMusteri.GetComponent<MusteriSistemi>();
            string[] balikTurleri = { "Fish_Common", "Fish_Rare", "Fish_Legendary" };
            musteriScript.istedigiBalikTuru = balikTurleri[Random.Range(0, balikTurleri.Length)];

            //  **Para sistemini m��teriye ba�la**
            musteriScript.paraSistemi = paraSistemi;

            // M��teriyi do�ru kasaya y�nlendir
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
