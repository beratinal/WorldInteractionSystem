# LLM Kullanım Dokümantasyonu (AI Interaction Log)

Bu doküman, "World Interaction System" projesinin geliştirilmesi sürecinde Yapay Zekalar ile yapılan teknik iş birliğini, alınan kod desteklerini ve bu kodların projeye nasıl entegre edildiğini şeffaf bir şekilde belgelemektedir.

## Özet
- **Toplam Prompt Sayısı:** 12
- **Kullanılan Araç:** Gemini, Chat GPT, Deep Seek
- **Yaklaşım:** Pair Programming & Technical Consultation
- **En Çok Yardım Alınan Konular:**
  - SOLID Prensipleri ve Modüler Mimari (Interface kullanımı)
  - Unity Fizik Motoru Optimizasyonları (Character Controller)
  - UI 

---
Kod yazdırmak için Gemini, İngilizce çeviri yapmak için ise Chat GPT ve Deep Seek kullandım.
---
Projeyi geliştirirken maalesef tek tek bu kayıtları tutmadığım için bazı promptların saati kaymış olabilir. (Sonradan promptların saatini görebileceğmi düşünmüştüm. Göremiyormuşum. Tekrar yapay zekalara sormak durumunda kaldım.) 



## Prompt 1: Temel Mimari ve Interface Yapısı

**Araç:** Gemini
**Tarih/Saat:** 2026-02-01 14:00

**Prompt:**
> "I want to create a modular interaction system in Unity. Instead of checking tags like 'Door' or 'Item', I want a generic system using Interfaces. Create a solid architecture where I can easily add new interactable types (Doors, Chests, Switches) without changing the player script."

**Alınan Cevap (Özet):**
> `IInteractable` arayüzü (Interface) önerildi. Oyuncu tarafında `InteractionDetector` scriptinin sadece bu arayüze sahip objelerle iletişim kurduğu, bağımlılığın (coupling) düşük olduğu bir yapı sunuldu.

**Nasıl Kullandım:**
- [ ] Direkt kullandım
- [x] Adapte ettim
- [ ] Reddettim

**Açıklama:**
> AI'ın önerdiği interface yapısını inceledim ve SOLID prensiplerine uygunluğunu doğruladım. Kodun temelini aldım ancak `Interact()` metodunun dönüş tiplerini kendi ihtiyaçlarıma (void yerine bool) göre revize ettim. Bu sayede etkileşimin başarılı olup olmadığını kontrol edebildim.

---

## Prompt 2: Raycast

**Araç:** Gemini
**Tarih/Saat:** 2026-02-01 15:30

**Prompt:**
> "Write a script using Physics. Raycast that detects objects implementing the `IInteractable` interface. It should handle UI prompt visibility (show 'Press E' when looking at an object) and input handling."

**Alınan Cevap (Özet):**
> `Camera.main` referansı üzerinden Ray atan ve `TryGetComponent` ile interface kontrolü yapan bir `InteractionDetector` sınıfı oluşturuldu.

**Nasıl Kullandım:**
- [ ] Direkt kullandım
- [x] Adapte ettim
- [ ] Reddettim

**Açıklama:**
> Kodun Raycast mantığını direkt kullandım. Ancak performans optimizasyonu için `Update` içinde sürekli `GetComponent` çağırmak yerine, son algılanan objeyi önbelleğe alan (caching) bir yapı ekledim. Ayrıca Raycast Layer Mask ekleyerek sadece etkileşim katmanındaki objelerin algılanmasını sağladım.

---

## Prompt 3: ScriptableObject ile Envanter

**Araç:** Gemini
**Tarih/Saat:** 2026-02-01 16:45

**Prompt:**
> "I need a data driven inventory system. Suggest a way to store Item data (Name, Icon, Prefab) efficiently without hardcoding details into scripts."

**Alınan Cevap (Özet):**
> Verilerin ScriptableObject (SO) olarak tutulması önerildi. `ItemDataSO` sınıfı ve bunu kullanan bir `InventoryManager` yapısı kurgulandı.

**Nasıl Kullandım:**
- [x] Direkt kullandım
- [ ] Adapte ettim
- [ ] Reddettim

**Açıklama:**
> ScriptableObject mimarisi Unity'nin veri yönetimi için en uygun yapı olduğundan öneriyi direkt uyguladım. Bu sayede kod yazmadan Unity Editör üzerinden yeni eşyalar (Kırmızı Anahtar, Mavi Anahtar) oluşturabildim.

---

## Prompt 4: UI

**Araç:** Gemini
**Tarih/Saat:** 2026-02-01 18:10

**Prompt:**
> "Create a UI script that listens to the Inventory. When an item is added, it should instantiate a UI slot prefab in a Grid Layout Group and populate it with the item's icon."

**Alınan Cevap (Özet):**
> `InventoryUI` sınıfı oluşturuldu. Bir döngü ile mevcut slotların temizlenip listenin yeniden çizildiği bir yöntem sunuldu.

**Nasıl Kullandım:**
- [ ] Direkt kullandım
- [x] Adapte ettim
- [ ] Reddettim

**Açıklama:**
> AI'ın verdiği kod her eklemede tüm listeyi silip baştan yapıyordu. Bunun performans maliyetini araştırdım. Şimdilik proje küçük olduğu için kabul ettim ancak listeyi `List<GameObject>` olarak tutup sadece yeni ekleneni instantiate etme yöntemini de notlarıma aldım.

---

## Prompt 5: Kod açıklaması

**Araç:** Chat GPT
**Tarih/Saat:** 2026-02-01 18:50

**Prompt:**
> "Oyuncu etkileşim event’lerini dinleyerek TextMeshPro UI güncelleyen bir Interaction Prompt UI scriptini açıkla. Bu yapının mimari avantajlarını anlat.

**Alınan Cevap (Özet):**
> UI katmanının gameplay logic’ten ayrılması, OnEnable/OnDisable içinde event aboneliklerinin yönetimi ve IInteractable interface’i üzerinden veri çekmenin avantajları açıklandı.

**Nasıl Kullandım:**
- [x] Direkt kullandım
- [ ] Adapte ettim
- [ ] Reddettim

**Açıklama:**
> Script mimari olarak temiz olduğu için yapısal bir değişiklik yapmadım. Event tabanlı UI yaklaşımının doğruluğunu teyit etmek amacıyla kullandım.

---

## Prompt 6: Sandık ve Eşya Spawn Mantığı

**Araç:** Gemini
**Tarih/Saat:** 2026-02-01 20:12

**Prompt:**
> "Extend the ChestInteractable script. It needs to spawn item inside itself when opened. item have to spawns once."

**Alınan Cevap (Özet):**
> `Instantiate` metodu kullanılarak sandık içine obje yaratma mantığı eklendi. `m_IsOpen` bool değişkeni ile sandığın tekrar açılması engellendi.

**Nasıl Kullandım:**
- [ ] Direkt kullandım
- [x] Adapte ettim
- [ ] Reddettim

**Açıklama:**
> Kod mantıklıydı ancak eşyanın sandığın içinde "gömülü" çıkmaması için Unity Editör'de sandık prefabının içine boş bir `SpawnPoint` objesi yerleştirdim ve kodun bu transformu referans almasını sağladım.

---

## Prompt 7: UnityEvents ile Switch Sistemi

**Araç:** Gemini
**Tarih/Saat:** 2026-02-01 20:25

**Prompt:**
> "I want a Switch interactable. Instead of hardcoding it to open a specific door, I want to be able to drag and drop any object (Light, Door, Particle) in the Inspector to be triggered."

**Alınan Cevap (Özet):**
> C# Delegates yerine `UnityEngine.Events.UnityEvent` kullanılması önerildi. Bu sayede Inspector üzerinden kod yazmadan olay bağlama imkanı sağlandı.

**Nasıl Kullandım:**
- [x] Direkt kullandım
- [ ] Adapte ettim
- [ ] Reddettim

**Açıklama:**
> UnityEvent yapısının Observer pattern'in pratik bir uygulaması olduğunu araştırıp öğrendim ve Level Design aşamasında esneklik sağladığı için projeye dahil ettim.

---

## Prompt 8: Highlight System

**Araç:** Gemini
**Tarih/Saat:** 2026-02-01 20:38

**Prompt:**
> "I need an Interaction Highlight system. Changing the material color to yellow looks bad and breaks the texture. Suggest a better way to make objects glow."

**Alınan Cevap (Özet):**
> Materyalin Albedo rengini değiştirmek yerine `Emission` kanalını manipüle eden veya mevcut rengin parlaklığını (Brightness) artıran bir shader manipülasyonu önerildi.

**Nasıl Kullandım:**
- [x] Direkt kullandım
- [ ] Adapte ettim
- [ ] Reddettim

**Açıklama:**
> "Brightness Boost" yönteminin dokuyu bozmadan en iyi sonucu verdiğini test ederek doğruladım ve uyguladım.

---

## Prompt 9: Veri Kalıcılığı

**Araç:** Gemini
**Tarih/Saat:** 2026-02-01 21:00

**Prompt:**
> "How can I implement a Save system where the game remembers which doors are opened and which keys are collected after restarting? It needs to be simple but expandable."

**Alınan Cevap (Özet):**
> Her etkileşimli nesneye benzersiz bir `ID` (String) atanması ve durumlarının (1 veya 0) `PlayerPrefs` üzerinde tutulması önerildi.

**Nasıl Kullandım:**
- [ ] Direkt kullandım
- [ ] Adapte ettim
- [x] Reddettim

**Açıklama:**
> Projenin test edilebilirliğini düşüreceğini düşündüğümden uygulamadım. Prefabları sıfırlayacak ayrı bir kod yazmaya üşendim.

---

## Prompt 10: Character Controller Fizik Düzeltmeleri

**Araç:** Gemini
**Tarih/Saat:** 2026-02-01 21:18

**Prompt:**
> "My character floats in the air sometimes. Also, implement a physics-based jump."

**Alınan Cevap (Özet):**
> Karakter yerdeyken (`isGrounded`) Y eksenindeki hızın 0 yerine negatif bir değere (`-2f`) sabitlenmesi gerektiği, bunun karakteri yere "yapıştırdığı" açıklandı. Zıplama için `Mathf.Sqrt(h * -2 * g)` formülü verildi.

**Nasıl Kullandım:**
- [x] Direkt kullandım
- [ ] Adapte ettim
- [ ] Reddettim

**Açıklama:**
> Verilen zıplama formülünün kinematik fizik temellerini araştırdım. Yerçekimi sabiti ve istenen yükseklik arasındaki ilişkiyi anladıktan sonra kodu projeye entegre ettim.

---

## Prompt 11: UI/UX İyileştirmeleri (Crosshair & EventSystem)

**Araç:** Gemini
**Tarih/Saat:** 2026-02-01 21:35

**Prompt:**
> "I need a Crosshair in the center of the screen. Also, clarify if I need an `EventSystem` in every scene or if it should be part of the UI prefab."

**Alınan Cevap (Özet):**
> Canvas üzerinde `RaycastTarget` özelliği kapalı bir Image (Knob) oluşturularak Crosshair yapılması tarif edildi. `EventSystem`'in UI Prefab'ının içine gömülmesi (Nested Prefab) stratejisi önerildi, böylece her sahnede tekrar kurulmasına gerek kalmadı.

**Nasıl Kullandım:**
- [x] Direkt kullandım
- [ ] Adapte ettim
- [ ] Reddettim

**Açıklama:**
> EventSystem'i prefab içine gömme fikri, projenin ölçeklenebilirliği (yeni levellar eklerken hata yapmayı önleme) açısından kritik bir mimari tavsiyeydi ve uygulandı.

---

## Prompt 12: Crosshair UI ve Commit Mesajı

**Araç:** Chat GPT
**Tarih/Saat:** 2026-02-01 21:45

**Prompt:**
> "Unity projesine bir crosshair ekledim. 'Add crosshair' Git commit mesajı olarak yeterli ve doğru mu? Ayrıca crosshair UI eklerken dikkat edilmesi gereken noktaları açıkla."

**Alınan Cevap (Özet):**
> Add crosshair commit mesajının kısa, net ve yeterli olduğu doğrulandı. Crosshair’in UI kapsamında değerlendirilmesi ve gereksiz karmaşıklıktan kaçınılması önerildi.

**Nasıl Kullandım:**
- [x] Direkt kullandım
- [ ] Adapte ettim
- [ ] Reddettim

**Açıklama:**
> Direkt kullanacaktım fakat kullanamadım. Commit etmeme izin vermedi. Ben de onu es geçtim.

---

# GENEL DEĞERLENDİRME VE METODOLOJİ

Bu proje geliştirilirken LLM'ler, bir "kod üretim aracı"ndan ziyade, **teknik bir danışman ve Pair Programmer** olarak konumlandırılmıştır.

### AI Kullanım Felsefem
Proje boyunca yapay zeka şu prensiplerce kullanılmıştır:

1. Oyunun temel mimarisine (Interface-based interaction, ScriptableObject veri yapısı) ben karar verdim; AI ise bu kararları uygulayacak kalıp kodların yazılmasını hızlandırdı.

2.  Üretilen hiçbir kod, satır satır okunmadan ve mantığı anlaşılmadan projeye dahil edilmemiştir. Örneğin; `InventoryUI` sisteminde AI'ın önerdiği "her karede listeyi yenileme" mantığı performans açısından sorgulanmış, proje ölçeği küçük olduğu için kabul edilmiştir ancak not düşülmüştür.

3.  **Hata Ayıklama ve Öğrenme**
    Karşılaşılan Unity'e özgü sorunlarda (örn: Character Controller'ın yokuş aşağı inerken havada kalması) AI bir dokümantasyon kaynağı olarak kullanılmış, sunulan çözümün fiziksel temeli (Vektör matematiği) anlaşıldıktan sonra uygulanmıştır.
