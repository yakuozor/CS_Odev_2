using UnityEngine;
using System.Collections;

public class FishingSystem : MonoBehaviour
{
    public Transform fishingSpot; // Bal�k tutma alan�
    public GameObject[] fishPrefabs; 
    public Transform[] fishHangerPositions; // Bal�klar�n as�laca�� noktalar
    public Transform player; // Oyuncunun Transform'u
    public float fishingRange = 4f; // Oyuncunun bal�k tutma alan�na ne kadar yak�n olmas� gerekti�i

    private bool isFishing = false; // Oyuncu �u an bal�k tutuyor mu?
    private bool fishOnHook = false; // Bal�k oltaya tak�ld� m�?
    public int fishCount = 0; // Ask�l�ktaki bal�k say�s�
    private int maxFish = 5; // Maksimum bal�k say�s�
    private bool isInFishingZone = false; // Oyuncu Fishing Spot i�inde
    private UIManager uiManager; 

    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
    }

    void Update()
    {
        if (fishCount >= maxFish && Input.GetMouseButtonDown(0))
        {
            Debug.Log("Ask�l�k dolu! Yeni bal�k tutulamaz.");
            return;
        }

        // Oyuncu Fishing Spot i�indeyse bal�k tutabilir
        if (Input.GetMouseButtonDown(0) && !isFishing && fishCount < maxFish && isInFishingZone)
        {
            StartCoroutine(CatchFish());
        }

        // Sa� t�k ile bal��� �ek
        if (Input.GetMouseButtonDown(1) && fishOnHook)
        {
            AddFishToHanger();
        }
    }

    IEnumerator CatchFish()
    {
        isFishing = true;
        Debug.Log("Olta at�ld�, bekleniyor...");
        if (uiManager != null) uiManager.BalikDurumGuncelle("Olta at�ld�, bekleniyor...");

        yield return new WaitForSeconds(Random.Range(2, 5));

        if (Random.Range(0, 100) < 70)
        {
            Debug.Log("Bal�k oltaya geldi! Sa� t�k ile �ek.");
            if (uiManager != null) uiManager.BalikDurumGuncelle("Bal�k oltaya geldi! Sa� t�k ile �ek.");
            fishOnHook = true;
        }
        else
        {
            Debug.Log("Bal�k ka�t�...");
            if (uiManager != null) uiManager.BalikDurumGuncelle("Bal�k ka�t�...");
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
            Debug.Log("Bal�k as�ld�! Fiyat: " + fishScript.price + " coin");

            isFishing = false;
            fishOnHook = false;
        }
        else
        {
            Debug.Log("Ask�l�k dolu! Yeni bal�k tutulamaz.");
        }
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FishingArea"))
        {
            isInFishingZone = true;
            Debug.Log("Fishing Spot'a G�R�LD�! Art�k bal�k tutabilirsin.");
            if (uiManager != null) uiManager.BalikTutmaAlaninda(true);
        }
    }

    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("FishingArea"))
        {
            isInFishingZone = false;
            Debug.Log("Fishing Spot'tan �IKILDI! Art�k bal�k tutamazs�n.");
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
