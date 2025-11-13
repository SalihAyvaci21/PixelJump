using UnityEngine;

// Bu script, toplanabilir puan nesneleri (elmalar) içindir.
public class Puan : MonoBehaviour
{
    public int puanMiktari = 1;

    // 'Is Trigger' olarak ayarlanmış Box Collider 2D ile çalışır
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Eğer çarpan nesne "Player" etiketine sahipse
        if (other.CompareTag("Player"))
        {
            // Player etiketini Player nesnesine eklemeyi unutma!
            // (Player'ı seç -> Inspector -> Tag -> Add Tag... -> "Player" -> Tekrar Player'ı seç -> Tag -> Player)

            // GameManager'daki PuanEkle fonksiyonunu çağır
            if (GameManager.instance != null)
            {
                GameManager.instance.PuanEkle(puanMiktari);
            }

            // Puan nesnesini yok et
            Destroy(gameObject);
        }
    }
}