using UnityEngine;
using TMPro; 
using UnityEngine.SceneManagement; 

// GÜNCELLENDİ: Başlangıç Ekranı, Tam Ekran Butonu ve Platform Yükseklik Limiti eklendi.
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI Elemanları")]
    public TextMeshProUGUI puanText; 
    
    [Header("Oyun Bitti Ekranı")]
    public GameObject gameOverScreen; 
    public TextMeshProUGUI finalScoreText;
    
    // --- YENİ EKLENDİ: BAŞLANGIÇ EKRANI ---
    [Header("Başlangıç Ekranı")]
    [Tooltip("Oyun başlamadan önce gösterilecek panel (Hierarchy'den sürükle)")]
    public GameObject startScreen;
    // --- BİTTİ ---

    [Header("Genel Ayarlar")]
    public Transform player;
    public float fallThreshold = -10f; 

    [Header("Obje (Elma) Spawner Ayarları")]
    public GameObject puanPrefab; 
    public float spawnInterval = 8f; 
    public float minSpawnY = 1.0f; 
    public float maxSpawnY = 4.0f; 

    [Header("Platform Spawner Ayarları")]
    public GameObject platformPrefab;
    public Transform startingPlatform;
    public float minXGap = 2f;
    public float maxXGap = 10f;
    [Tooltip("Bir sonraki platformun ne kadar ALÇALABİLECEĞİ (örn: -2)")]
    public float minYChange = -2f;
    [Tooltip("Bir sonraki platformun ne kadar YÜKSELEBİLECEĞİ (Zıplama yüksekliğinden (örn: 2.5) DÜŞÜK olmalı!)")]
    public float maxYChange = 2.0f; // DİKKAT: Bunu Inspector'da çok yükseltirsen oyun imkansızlaşır!
    public float platformSpawnAhead = 30f;
    
    // --- Private Değişkenler ---
    private float nextSpawnX_Threshold;
    private int puan = 0;
    private Transform lastSpawnedPlatform;
    private float platformPrefabWidth;
    
    // --- YENİ EKLENDİ: OYUN DURUMU ---
    private bool isGameOver = false;
    private bool isGameStarted = false; // Oyunun başlayıp başlamadığını takip eder
    // --- BİTTİ ---

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }
    }

    void Start()
    {
        puan = 0;
        puanText.text = "Puan: " + puan.ToString();

        // --- YENİ EKLENDİ: OYUNU DURDUR VE EKRANI GÖSTER ---
        isGameOver = false;
        isGameStarted = false;
        Time.timeScale = 0f; // Oyunu dondur
        
        if (startScreen != null)
        {
            startScreen.SetActive(true); // Başlangıç ekranını göster
        }
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(false); // Oyun bitti ekranını gizle
        }
        if (puanText != null)
        {
            puanText.gameObject.SetActive(false); // Skoru gizle
        }
        // --- BİTTİ ---

        // Elma Spawner Başlangıcı
        if (player != null)
        {
            nextSpawnX_Threshold = player.position.x + spawnInterval;
        }
        else { Debug.LogError("GameManager: 'Player' transform'u atanmamış!"); }

        // Platform Spawner Başlangıcı
        if (startingPlatform != null)
        {
            lastSpawnedPlatform = startingPlatform;
        } 
        else { Debug.LogError("GameManager: 'Starting Platform' atanmamış!"); }
        
        if (platformPrefab != null)
        {
            platformPrefabWidth = platformPrefab.GetComponent<BoxCollider2D>().size.x;
        } 
        else { Debug.LogError("GameManager: 'Platform Prefab' atanmamış!"); }
    }
    
    void Update()
    {
        // --- YENİ EKLENDİ: OYUNU BAŞLATMA ---
        if (!isGameStarted)
        {
            // Oyunu başlatmak için Yukarı Ok (Zıplama) tuşuna bas
            // GÜNCELLEME: GetAxis, timeScale=0 iken çalışmaz. GetAxisRaw kullan.
            if (Input.GetAxisRaw("Vertical") > 0.1f)
            {
                StartGame();
            }
            return; // Oyun başlamadıysa Update'in geri kalanını çalıştırma
        }
        // --- BİTTİ ---

        if (player == null || isGameOver)
            return;

        if (player.position.y < fallThreshold)
        {
            GameOver(); 
            return; 
        }

        // 1. Elma Spawner'ını Kontrol Et
        if (player.position.x > nextSpawnX_Threshold)
        {
            nextSpawnX_Threshold += spawnInterval + Random.Range(-1.5f, 1.5f);
            SpawnPuan();
        }

        // 2. Platform Spawner'ını Kontrol Et
        CheckAndSpawnPlatform();
    }

    // --- YENİ EKLENEN FONKSİYONLAR ---

    // Oyunu başlatan ana fonksiyon
    public void StartGame()
    {
        if (isGameStarted) return; // Zaten başladıysa tekrar çağırma

        isGameStarted = true;
        Time.timeScale = 1f; // Oyunu başlat/zamanı akıt

        if (startScreen != null)
        {
            startScreen.SetActive(false); // Başlangıç ekranını gizle
        }
        if (puanText != null)
        {
            puanText.gameObject.SetActive(true); // Skoru göster
        }
    }

    // (Public) PlayerController'ın oyunun başlayıp başlamadığını bilmesi için
    public bool IsGameStarted()
    {
        return isGameStarted;
    }

    // Tam Ekran / Pencere Modu butonu için
    public void ToggleFullscreen()
    {
        // Ekran tam ekransa pencere moduna, değilse tam ekrana geçir
        Screen.fullScreen = !Screen.fullScreen;
        Debug.Log("Tam Ekran Modu: " + Screen.fullScreen);
    }
    
    // --- OYUN BİTTİ FONKSİYONLARI ---
    
    public void GameOver()
    {
        if (isGameOver) return; 

        isGameOver = true;
        Time.timeScale = 0f; 

        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
            if(finalScoreText != null)
            {
                finalScoreText.text = "Puanın: " + puan.ToString();
            }
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    // --- SPAWNER FONKSİYONLARI ---
    
    void SpawnPuan()
    {
        if (puanPrefab == null) return;
        float randomY = Random.Range(minSpawnY, maxSpawnY);
        Vector2 spawnPos = new Vector2(nextSpawnX_Threshold, randomY);
        Instantiate(puanPrefab, spawnPos, Quaternion.identity);
    }

    void CheckAndSpawnPlatform()
    {
        if (player == null || lastSpawnedPlatform == null) return;
        float distanceToSpawn = player.position.x + platformSpawnAhead;
        if (distanceToSpawn > lastSpawnedPlatform.position.x)
        {
            SpawnPlatform();
        }
    }

    void SpawnPlatform()
    {
        if (platformPrefab == null) return;

        float randomXGap = Random.Range(minXGap, maxXGap);
        float randomYChange = Random.Range(minYChange, maxYChange);
        
        Vector2 lastPos = lastSpawnedPlatform.position;
        
        // --- GÜNCELLENDİ: PLATFORM YÜKSEKLİK LİMİTİ ---
        // Yeni Y pozisyonunu hesapla
        float newY = lastPos.y + randomYChange;
        
        // Platformların sonsuza kadar yükselmesini/alçalmasını engellemek için
        // başlangıç platformunun +10 ve -10 birim yakınına kelepçele (clamp).
        float clampedY = Mathf.Clamp(newY, startingPlatform.position.y - 10f, startingPlatform.position.y + 10f);

        Vector2 newPos = new Vector2(
            lastPos.x + platformPrefabWidth + randomXGap,
            clampedY // Kelepçelenmiş (sınırlandırılmış) Y pozisyonunu kullan
        );
        // --- GÜNCELLEME BİTTİ ---

        GameObject newPlatform = Instantiate(platformPrefab, newPos, Quaternion.identity);
        lastSpawnedPlatform = newPlatform.transform;
    }
    
    public void PuanEkle(int miktar)
    {
        puan += miktar;
        puanText.text = "Puan: " + puan.ToString();
    }
}