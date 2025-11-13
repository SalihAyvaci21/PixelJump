using UnityEngine;

// GÜNCELLENDİ: 'Idle' (boşta durma) animasyonu, 'Run' (koşma) animasyonu
// ve karakterin sağa/sola dönmesi (Flip) eklendi.
public class PlayerController : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    [Tooltip("Oyuncunun sağa/sola hareket hızı")]
    public float moveSpeed = 5f;
    [Tooltip("Zıplama kuvveti")]
    public float jumpForce = 7f;
    [Tooltip("Hızlı düşüş (aşağı ok) için uygulanacak hız")]
    public float fastFallSpeed = 10f; 

    private Rigidbody2D rb;
    private bool isGrounded;
    private Animator anim;
    
    // --- YENİ EKLENDİ ---
    private float moveInput; // Yatay (sağ/sol) girdiyi saklamak için
    private bool isFacingRight = true; // Karakterin yüzünün sağa dönük olduğunu varsay
    // --- BİTTİ ---

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        isGrounded = true;
    }

    void Update()
    {
        // --- Oyun Başlangıç Kontrolü ---
        if (GameManager.instance == null || !GameManager.instance.IsGameStarted())
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }
        // --- BİTTİ ---

        // --- GÜNCELLENDİ: Girdileri Update'te Oku ---
        
        // Yatay (sağ/sol) girdiyi (-1 ile +1 arası) oku ve sakla
        moveInput = Input.GetAxis("Horizontal"); 
        
        // Dikey (yukarı/aşağı) girdiyi oku
        float verticalInput = Input.GetAxis("Vertical"); 
        
        // --- BİTTİ ---


        // --- ANİMATÖR GÜNCELLEMELERİ ---
        
        // Animatöre dikey hızı (vSpeed) gönder (Jump/Fall için)
        anim.SetFloat("vSpeed", rb.linearVelocity.y);
        
        // YENİ EKLENDİ: Animatöre yatay hızı (moveSpeed) gönder (Idle/Run için)
        // Mathf.Abs() kullanarak negatif değerleri (sol) pozitife çeviririz
        anim.SetFloat("moveSpeed", Mathf.Abs(moveInput));
        
        // --- BİTTİ ---

        // Zıplama (Yukarı Ok)
        if (isGrounded && verticalInput > 0.1f) 
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
        }

        // Hızlı Düşme (Aşağı Ok)
        if (!isGrounded && verticalInput < -0.1f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -fastFallSpeed);
        }
    }

    void FixedUpdate()
    {
        // --- Oyun Başlangıç Kontrolü ---
        if (GameManager.instance == null || !GameManager.instance.IsGameStarted())
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        // --- BİTTİ ---

        // --- GÜNCELLENDİ: Fiziği Girdiye Göre Güncelle ---
        
        // Fiziği, Update'te okuduğumuz 'moveInput' girdisine göre güncelle
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        
        // --- BİTTİ ---
        
        // --- YENİ EKLENDİ: Karakteri Döndürme (Flip) ---
        // Eğer sağa bakarken sola basılırsa VEYA sola bakarken sağa basılırsa
        if ((isFacingRight && moveInput < 0f) || (!isFacingRight && moveInput > 0f))
        {
            Flip();
        }
        // --- BİTTİ ---
    }

    // --- YENİ EKLENDİ: Flip Fonksiyonu ---
    private void Flip()
    {
        // Yönü tersine çevir
        isFacingRight = !isFacingRight;
        
        // Karakterin 'scale' (ölçek) değerini al
        Vector3 localScale = transform.localScale;
        
        // X eksenindeki ölçeği -1 ile çarp (ters çevir)
        localScale.x *= -1f;
        
        // Yeni ölçeği karaktere geri uygula
        transform.localScale = localScale;
    }
    // --- BİTTİ ---

    // --- FİZİK METODLARI ---
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
            anim.SetBool("isGrounded", true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = false;
            anim.SetBool("isGrounded", false);
        }
    }
}