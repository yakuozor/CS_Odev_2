using UnityEngine;
using System.Collections.Generic;

public class KasaSistemi : MonoBehaviour
{
    public Transform[] fishStoragePositions; // Kasalar�n Transform'lar�
    public Transform player; // Oyuncunun Transform'u
    public float storageRange = 7f; // Oyuncunun kasalara ne kadar yak�n olmas� gerekti�i
    public FishingSystem fishingSystem; // FishingSystem referans�

    public Dictionary<Transform, string> kasaBalikTuru = new Dictionary<Transform, string>(); // Hangi kasa hangi bal��� kabul ediyor
    private Dictionary<Transform, List<Transform>> kasaBalikKonumlari = new Dictionary<Transform, List<Transform>>(); // Her kasadaki konumlar
    private Dictionary<Transform, int> kasaBalikSayisi = new Dictionary<Transform, int>(); // Kasadaki bal�k say�lar�
    private int maxFishPerKasa = 5; // Her kasaya maksimum 5 bal�k konabilir

    void Start()
    {
        foreach (Transform kasa in fishStoragePositions)
        {
            if (kasa.name.ToLower().Contains("common"))
                kasaBalikTuru[kasa] = "Fish_Common";
            else if (kasa.name.ToLower().Contains("rare"))
                kasaBalikTuru[kasa] = "Fish_Rare";
            else if (kasa.name.ToLower().Contains("legendary"))
                kasaBalikTuru[kasa] = "Fish_Legendary";

            //  Kasan�n i�indeki "KasaSpot" noktalar�n� al
            List<Transform> spotlar = new List<Transform>();
            foreach (Transform child in kasa)
            {
                if (child.name.ToLower().Contains("kasaspot"))
                {
                    spotlar.Add(child);
                }
            }

            
            if (spotlar.Count == 0)
            {
                Debug.LogError($"HATA: {kasa.name} kasas�nda KasaSpot bulunamad�! L�tfen KasaSpot1, KasaSpot2... ekleyin.");
            }

            kasaBalikKonumlari[kasa] = spotlar;
            kasaBalikSayisi[kasa] = 0;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && fishingSystem.fishCount > 0)
        {
            Transform closestStorage = GetClosestStorage();
            if (closestStorage != null && Vector3.Distance(player.position, closestStorage.position) <= storageRange)
            {
                MoveFishToStorage(closestStorage);
            }
        }
    }

    void MoveFishToStorage(Transform kasa)
    {
        if (fishingSystem.fishCount > 0 && kasaBalikSayisi[kasa] < maxFishPerKasa)
        {
            // Ask�daki ilk bal��� al
            Transform hangerSpot = fishingSystem.fishHangerPositions[fishingSystem.fishCount - 1];
            GameObject fish = hangerSpot.GetChild(0).gameObject;
            string fishTag = fish.tag;

            if (fishTag == kasaBalikTuru[kasa])
            {
                //  Bo� olan ilk konumu bul
                Transform spot = GetNextAvailableSpot(kasa);
                if (spot != null)
                {
                    fish.transform.SetParent(spot);
                    fish.transform.position = spot.position;
                    fish.transform.rotation = Quaternion.Euler(0, 90, 0); // Y ekseninde 90 derece d�nd�r

                    kasaBalikSayisi[kasa]++;
                    fishingSystem.DecreaseFishCount(); // `fishCount` g�ncelleme y�ntemi

                    Debug.Log($"Bal�k {kasa.name} kasas�na kondu! T�r: {fishTag}, Kasa ��eri�i: {kasaBalikSayisi[kasa]} bal�k.");
                }
                else
                {
                    Debug.Log("Bu kasa dolu!");
                }
            }
            else
            {
                Debug.Log($"Bu kasa sadece {kasaBalikTuru[kasa]} kabul eder! {fishTag} buraya konamaz.");
            }
        }
        else
        {
            Debug.Log("Kasa dolu veya ge�ersiz!");
        }
    }

    Transform GetNextAvailableSpot(Transform kasa)
    {
        if (!kasaBalikKonumlari.ContainsKey(kasa))
        {
            Debug.LogError($"HATA: {kasa.name} kasas�n�n i�inde KasaSpot noktalar� yok!");
            return null;
        }

        foreach (Transform spot in kasaBalikKonumlari[kasa])
        {
            if (spot.childCount == 0) // E�er bu noktada bal�k yoksa
            {
                return spot;
            }
        }
        return null; // E�er t�m noktalar doluysa
    }

    Transform GetClosestStorage()
    {
        Transform closestStorage = null;
        float minDistance = Mathf.Infinity;

        foreach (Transform storage in fishStoragePositions)
        {
            float distance = Vector3.Distance(player.position, storage.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestStorage = storage;
            }
        }

        return closestStorage;
    }

    public bool KasadaBalikVarMi(Transform kasa, string balikTuru)
    {
        return kasaBalikSayisi.ContainsKey(kasa) && kasaBalikSayisi[kasa] > 0 && kasaBalikTuru[kasa] == balikTuru;
    }

    public void BalikEksilt(Transform kasa)
    {
        if (kasaBalikSayisi.ContainsKey(kasa) && kasaBalikSayisi[kasa] > 0)
        {
            //  KasaSpot'tan bal��� sil
            Transform spot = kasaBalikKonumlari[kasa][kasaBalikSayisi[kasa] - 1];

            if (spot.childCount > 0)
            {
                Destroy(spot.GetChild(0).gameObject); // �lk bal�k nesnesini yok et
            }

            kasaBalikSayisi[kasa]--; // Bal�k say�s�n� azalt
            Debug.Log($"Kasadan bal�k eksildi! {kasa.name} - Kalan Bal�k: {kasaBalikSayisi[kasa]}");
        }
    }



}
