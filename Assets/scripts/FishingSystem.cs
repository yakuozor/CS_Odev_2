using UnityEngine;
using System.Collections;

public class FishingSystem : MonoBehaviour
{
    public Transform fishingSpot; // Balýk tutma alaný
    public GameObject[] fishPrefabs; 
    public Transform[] fishHangerPositions; // Balýklarýn asýlacaðý noktalar
    public Transform player; // Oyuncunun Transform'u
    public float fishingRange = 4f; // Oyuncunun balýk tutma alanýna ne kadar yakýn olmasý gerektiði

    private bool isFishing = false; // Oyuncu þu an balýk tutuyor mu?
    private bool fishOnHook = false; // Balýk oltaya takýldý mý?
    public int fishCount = 0; // Askýlýktaki balýk sayýsý
    private int maxFish = 5; // Maksimum balýk sayýsý
    private bool isInFishingZone = false; // Oyuncu Fishing Spot içinde
    private UIManager uiManager; 

    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
    }

    void Update()
    {
        if (fishCount >= maxFish && Input.GetMouseButtonDown(0))
        {
            Debug.Log("Askýlýk dolu! Yeni balýk tutulamaz.");
            return;
        }

        // Oyuncu Fishing Spot içindeyse balýk tutabilir
        if (Input.GetMouseButtonDown(0) && !isFishing && fishCount < maxFish && isInFishingZone)
        {
            StartCoroutine(CatchFish());
        }

        // Sað týk ile balýðý çek
        if (Input.GetMouseButtonDown(1) && fishOnHook)
        {
            AddFishToHanger();
        }
    }

    IEnumerator CatchFish()
    {
        isFishing = true;
        Debug.Log("Olta atýldý, bekleniyor...");
        if (uiManager != null) uiManager.BalikDurumGuncelle("Olta atýldý, bekleniyor...");

        yield return new WaitForSeconds(Random.Range(2, 5));

        if (Random.Range(0, 100) < 70)
        {
            Debug.Log("Balýk oltaya geldi! Sað týk ile çek.");
            if (uiManager != null) uiManager.BalikDurumGuncelle("Balýk oltaya geldi! Sað týk ile çek.");
            fishOnHook = true;
        }
        else
        {
            Debug.Log("Balýk kaçtý...");
            if (uiManager != null) uiManager.BalikDurumGuncelle("Balýk kaçtý...");
            isFishing = false;
        }
    }

    void AddFishToHanger()
    {
        if (fishCount < maxFish)
        {
            GameObject selectedFish = fishPrefabs[Random.Range(0, fishPrefabs.Length)];
            Transform hangerSpot = fishHangerPositions[fishCount];

            GameObject fish = Instantiate(selectedFish, hangerSpot.position, hangerSpot.rotation);
            fish.transform.SetParent(hangerSpot);

            fishCount++;

            Fish fishScript = fish.GetComponent<Fish>();
            Debug.Log("Balýk asýldý! Fiyat: " + fishScript.price + " coin");

            isFishing = false;
            fishOnHook = false;
        }
        else
        {
            Debug.Log("Askýlýk dolu! Yeni balýk tutulamaz.");
        }
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FishingArea"))
        {
            isInFishingZone = true;
            Debug.Log("Fishing Spot'a GÝRÝLDÝ! Artýk balýk tutabilirsin.");
            if (uiManager != null) uiManager.BalikTutmaAlaninda(true);
        }
    }

    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("FishingArea"))
        {
            isInFishingZone = false;
            Debug.Log("Fishing Spot'tan ÇIKILDI! Artýk balýk tutamazsýn.");
            if (uiManager != null) uiManager.BalikTutmaAlaninda(false);
        }
    }

    public void DecreaseFishCount()
    {
        if (fishCount > 0)
        {
            fishCount--;
        }
    }
}
