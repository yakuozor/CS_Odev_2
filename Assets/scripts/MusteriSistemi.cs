using UnityEngine;
using System.Collections;

public class MusteriSistemi : MonoBehaviour
{
    public string istedigiBalikTuru; // Müþterinin almak istediði balýk türü
    public float hareketHizi = 2f; // Müþterinin yürüme hýzý
    public Transform hedefKasa; // Müþterinin gideceði kasa
    public KasaSistemi kasaSistemi; // Kasa sistemi referansý
    public ParaSistemi paraSistemi; 
    public int odemeMiktari = 10; 
    private bool kasayaUlasti = false;

    void Update()
    {
        if (!kasayaUlasti && hedefKasa != null)
        {
            // Kasaya doðru hareket et
            transform.position = Vector3.MoveTowards(transform.position, hedefKasa.position, hareketHizi * Time.deltaTime);

            // Kasaya ulaþtý mý?
            if (Vector3.Distance(transform.position, hedefKasa.position) < 1f)
            {
                kasayaUlasti = true;
                StartCoroutine(BalikSatinAl());
            }
        }
    }

    IEnumerator BalikSatinAl()
    {
        yield return new WaitForSeconds(1f); // Bekleme süresi

        // Kasada balýk
        if (kasaSistemi.KasadaBalikVarMi(hedefKasa, istedigiBalikTuru))
        {
            // Balýðý kasadan eksilt
            kasaSistemi.BalikEksilt(hedefKasa);

            
            if (paraSistemi != null)
            {
                paraSistemi.ParaEkle(odemeMiktari);
            }

            Debug.Log($"{istedigiBalikTuru} balýðý satýldý! {odemeMiktari} coin kazandýn.");
        }
        else
        {
            Debug.Log($"Müþteri {istedigiBalikTuru} almak istedi ama stokta yok. Üzgün þekilde gitti.");
        }

        // Müþteriyi yok et
        Destroy(gameObject, 1f);
    }
}
