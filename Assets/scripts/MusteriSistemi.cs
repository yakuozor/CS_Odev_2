using UnityEngine;
using System.Collections;

public class MusteriSistemi : MonoBehaviour
{
    public string istedigiBalikTuru; // M��terinin almak istedi�i bal�k t�r�
    public float hareketHizi = 2f; // M��terinin y�r�me h�z�
    public Transform hedefKasa; // M��terinin gidece�i kasa
    public KasaSistemi kasaSistemi; // Kasa sistemi referans�
    public ParaSistemi paraSistemi; 
    public int odemeMiktari = 10; 
    private bool kasayaUlasti = false;

    void Update()
    {
        if (!kasayaUlasti && hedefKasa != null)
        {
            // Kasaya do�ru hareket et
            transform.position = Vector3.MoveTowards(transform.position, hedefKasa.position, hareketHizi * Time.deltaTime);

            // Kasaya ula�t� m�?
            if (Vector3.Distance(transform.position, hedefKasa.position) < 1f)
            {
                kasayaUlasti = true;
                StartCoroutine(BalikSatinAl());
            }
        }
    }

    IEnumerator BalikSatinAl()
    {
        yield return new WaitForSeconds(1f); // Bekleme s�resi

        // Kasada bal�k
        if (kasaSistemi.KasadaBalikVarMi(hedefKasa, istedigiBalikTuru))
        {
            // Bal��� kasadan eksilt
            kasaSistemi.BalikEksilt(hedefKasa);

            
            if (paraSistemi != null)
            {
                paraSistemi.ParaEkle(odemeMiktari);
            }

            Debug.Log($"{istedigiBalikTuru} bal��� sat�ld�! {odemeMiktari} coin kazand�n.");
        }
        else
        {
            Debug.Log($"M��teri {istedigiBalikTuru} almak istedi ama stokta yok. �zg�n �ekilde gitti.");
        }

        // M��teriyi yok et
        Destroy(gameObject, 1f);
    }
}
