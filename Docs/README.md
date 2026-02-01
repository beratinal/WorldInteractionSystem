# World Interaction System - Berat İnal

Bu proje, Ludu Arts staj başvurusu kapsamında geliştirilmiş; genişletilebilir, modüler ve SOLID prensiplerine uygun bir Unity etkileşim(Interaction) sistemidir.

## Kurulum
- **Unity Versiyonu:** 6000.2.14f1
- **Kurulum Adımları:**
  1. Projeyi bilgisayarınıza klonlayın veya indirin.
  2. Unity Hub üzerinden projeyi `Add` diyerek ekleyin.
  3. Proje açıldıktan sonra `Project` panelinden `Assets/Scenes/SampleScene` sahnesini açın.
  4. Editördeki **Play** butonuna basarak başlatın.

## Nasıl Test Edilir
**Test Sahnesi:** `SampleScene` (Ana oyun döngüsünün olduğu sahne)

**Kontroller:**
- **W, A, S, D:** Hareket
- **Space:** Zıplama
- **Mouse:** Kamera Yönü
- **E (Bas Çek):** Hızlı Etkileşim (Kapı açma, Şalter indirme, Eşya toplama)
- **E (Basılı Tut):** Süreli Etkileşim (Sandık açma)
- **TAB:** Envanteri Aç/Kapat

### Test Senaryoları

1. **Door Test:**
   - Kapıya yaklaşın, "Open Door" mesajını görün
   - E'ye basın, kapı açılsın
   - Tekrar basın, kapı kapansın

2. **Key + Locked Door Test:**
   - Kilitli kapıya yaklaşın, "Locked - Key Required" mesajını görün
   - Anahtarı bulun ve toplayın
   - Kilitli kapıya geri dönün, şimdi açılabilir olmalı

3. **Switch Test:**
   - Switch'e yaklaşın ve aktive edin
   - Kapının tetiklendiğini görün

4. **Chest Test:**
   - Sandığa yaklaşın
   - E'ye basılı tutun, progress bar dolsun
   - Sandık açılsın ve içindeki item alınsın

## Mimari Kararlar

### 1. Interface Tabanlı Etkileşim (`IInteractable`)
Tag kontrolü (`if (tag == "Door")`) yerine `IInteractable` arayüzü kullanıldı.
- **Neden:** Modülerlik ve düşük bağımlılık(Loose Coupling). Yeni bir etkileşimli nesne eklemek için Player kodunu değiştirmeye gerek yoktur.
- **Trade-off:** Her etkileşimli obje için ayrı script veya türetilmiş sınıf yazılması gerekir ancak projenin büyümesi açısından bu kabul edilebilir bir maliyettir.

### 2. ScriptableObject ile Veri Yönetimi
Eşya verileri (Ad, İkon, Prefab) ScriptableObject'ler üzerinde tutuldu.
- **Neden:** Data-Driven tasarım. Kod yazmadan yeni anahtarlar veya eşyalar üretilebilir. Memory yönetimi açısından daha verimlidir.

### 3. Raycast & Caching
`InteractionDetector` scripti her karede Ray atar ancak `TryGetComponent` maliyetini düşürmek için son algılanan objeyi önbellekte (cache) tutar.

### Kullanılan Design Patterns

| Pattern | Kullanım Yeri | Neden |
|---------|---------------|-------|
| **Strategy** | `IInteractable` Interface | Kapı, Sandık ve Şalter gibi farklı nesnelerin aynı "Interact" komutuna farklı tepkiler vermesini sağlamak ve `if-else` karmaşasından kaçınmak için. |
| **Observer** | `UnityEvents` (Switch System) | Şalterin (Subject), tetikleyeceği objeden (Observer) habersiz olması; yani kod bağımlılığı olmadan Inspector üzerinden ışık veya kapı bağlanabilmesi için. |
| **Flyweight** (veya Data Object) | `ScriptableObjects` (Item Data) | Her eşya için ayrı bir sınıf oluşturmak yerine, ortak verileri (İsim, İkon, Prefab) tek bir veri dosyasında tutup bellek yönetimini optimize etmek için. |
| **Singleton** | `InventoryManager` | Envanter durumuna ve UI güncellemelerine oyunun her yerinden tek bir referansla ulaşabilmek için. |

## Ludu Arts Standartlarına Uyum

- **Klasör Yapısı:** Proje kök dizini temiz tutuldu. Tüm geliştirme `Assets/InteractionSystem` altında; `Scripts`, `Prefabs`, `Materials` gibi alt klasörlere ayrıldı. `_Dev` gibi geçici klasörler temizlendi.
- **İsimlendirme (Naming Convention):**
  - Prefablar: `P_` öneki (Örn: `P_Chest_Base`)
  - Private Serialized Fields: `m_` öneki (Örn: `m_InteractionRange`)
  - Scriptler: PascalCase ve Namespace kullanımı (`InteractionSystem.Runtime...`)
- **Kod Kalitesi:** `[SerializeField]`, `[Header]`, `[Tooltip]` nitelikleri kullanılarak Inspector okunabilirliği artırıldı.

## Tamamlanan Özellikler

### Zorunlu (Must Have)

- [x] Core Interaction System
  - [x] IInteractable interface
  - [x] InteractionDetector
  - [x] Range kontrolü

- [x] Interaction Types
  - [x] Instant
  - [x] Hold
  - [x] Toggle

- [x] Interactable Objects
  - [x] Door (locked/unlocked)
  - [x] Key Pickup
  - [x] Switch/Lever
  - [x] Chest/Container

- [x] UI Feedback
  - [x] Interaction prompt
  - [x] Dynamic text
  - [x] Hold progress bar
  - [x] Cannot interact feedback

- [x] Simple Inventory
  - [x] Key toplama
  - [x] UI listesi

### Bonus (Nice to Have)

- [X] Animation entegrasyonu
- [X] Sound effects
- [X] Multiple keys / color-coded
- [X] Interaction highlight
- [ ] Save/Load states
- [X] Chained interactions

## Bilinen Limitasyonlar

- **Save/Load Sistemi:** Proje isterleri arasında "Bonus" olarak yer alan kayıt sistemi, test edilebilirliği karmaşıklaştırmamak ve proje kapsamını yönetilebilir tutmak adına (PROMPTS.md dosyasında belirtildiği üzere) bu aşamada **uygulanmamıştır**.
- **Animasyon:** Kapı ve sandık animasyonları için Animator Controller yerine, kontrolü daha kesin olan Coroutine tabanlı kod (Procedural Animation) tercih edilmiştir.

## Ekstra Özellikler

- **Gelişmiş Görsel Geri Bildirim (Highlight):** Etkileşime geçilebilir nesneler, üzerine bakıldığında parlaklık artışı ile efekt verir.
- **UnityEvent Entegrasyonu:** Şalter (Switch) sistemi, kod bağımlılığı olmadan Inspector üzerinden herhangi bir objeyi (Işık, Kapı, Particle) tetikleyebilecek esneklikte tasarlandı.
- **Dinamik Crosshair & UI:** Oyuncu bir nesneye baktığında ilgili etkileşim metni (Örnek: "Press E to Open") dinamik olarak güncellenir.
- **Fizik İyileştirmeleri:** Karakter kontrolcüsünde yokuş aşağı inerken havada kalma sorunu fizik tabanlı yerçekimi yamasıyla çözüldü.

## İletişim

|-------|-------|
| Ad Soyad | Berat İnal |
| E-posta | brtinal0@gmail.com |
| LinkedIn | https://www.linkedin.com/in/berat-inal-2b6bb2218/ |
| GitHub | github.com/beratinal |
