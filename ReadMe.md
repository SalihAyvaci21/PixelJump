# 👾 PixelJump 👾

Bu proje, Unity ve C# kullanılarak sıfırdan geliştirilmiş, prosedürel olarak platformlar üreten 2D bir "sonsuz koşu" oyunudur. Unity temellerini, C# ile script yazmayı ve oyun döngüsü (game loop) oluşturmayı öğrenmek için harika bir başlangıç projesidir.

## ✨ Özellikler

- **Prosedürel Platform Üretimi:** Oyuncu ilerledikçe, belirlenen yükseklik ve mesafe kurallarına göre platformlar ve puanlar (elmalar) rastgele oluşturulur.
    
- **Tam Animasyon Desteği:** Karakter `Idle` (Durma), `Run` (Koşma), `Jump` (Zıplama) ve `Fall` (Düşme) animasyonları için bir "Animator State Machine" kullanır.
    
- **Gelişmiş Oyuncu Kontrolleri:** Sadece zıplama değil; sağa/sola hareket, hızlı düşme ve karakterin baktığı yönü değiştirme (`Flip`).
    
- **Oyun Döngüsü:** Başlangıç ekranı, oyun içi skor takibi, düşme tespiti, "Oyun Bitti" ekranı ve "Yeniden Başlat" butonu.
    
- **Paralaks Arkaplan:** Kamera hareketine bağlı olarak hareket eden, "çocuk" (child) nesne olarak eklenmiş bir arkaplan.
    
- **UI (Arayüz) Yönetimi:** `GameManager` üzerinden kontrol edilen `StartScreen`, `GameOverScreen` ve `Fullscreen` (Tam Ekran) butonu.
    

## 🕹️ Kontroller

- **Sol/Sağ Ok Tuşları:** Hareket et
    
- **Yukarı Ok Tuşu:** Zıpla
    
- **Aşağı Ok Tuşu (Havadayken):** Hızlı Düş
    

##  Görseller

![Oyun İçi GIF](Images/OyunGif.gif)

![Unity Görsel](Images/UnityPhoto.png)

| Başlangıç Ekranı                                 | Oyun İçi                                     | Oyun Bitti                                    |
| ------------------------------------------------ | -------------------------------------------- | --------------------------------------------- |
| _(start-screen.png dosyanızı buraya sürükleyin)_ | _(gameplay.png dosyanızı buraya sürükleyin)_ | _(game-over.png dosyanızı buraya sürükleyin)_ |

## 🛠️ Teknik

- **Oyun Motoru:** Unity 202x.x
    
- **Dil:** C#
    
- **Platform:** WebGL (veya Standalone PC)