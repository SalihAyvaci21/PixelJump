using UnityEngine;

// Bu script, kameranın bir hedefi (oyuncuyu)
// X ekseninde (yatayda) takip etmesini sağlar.
public class CameraController : MonoBehaviour
{
    // Inspector panelinden Player nesnesini buraya sürükle
    [Header("Takip Edilecek Hedef")]
    public Transform target;

    // LateUpdate, tüm Update ve FixedUpdate işlemleri bittikten sonra çalışır.
    // Bu, kameranın titremesini (jitter) engeller ve takip işlemleri için en ideal yerdir.
    void LateUpdate()
    {
        // Eğer bir hedefimiz (target) atanmışsa
        if (target != null)
        {
            // Kameranın yeni pozisyonunu belirle
            Vector3 newPosition = new Vector3(
                target.position.x,        // X: Oyuncunun X pozisyonu olsun
                transform.position.y,     // Y: Kameranın kendi Y pozisyonu kalsın (zıplarken oynamasın)
                transform.position.z      // Z: Kameranın kendi Z pozisyonu kalsın (-10)
            );

            // Kameranın pozisyonunu bu yeni pozisyona ayarla
            transform.position = newPosition;
        }
    }
}