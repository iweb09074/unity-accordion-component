# Unity Accordion Component

Unity 2021.3 LTS için Canvas tabanlı Accordion (genişletme/daraltma) component sistemi.

## 📋 İçindekiler

- [Özellikler](#özellikler)
- [Kurulum](#kurulum)
- [Kullanım](#kullanım)
- [Dosya Yapısı](#dosya-yapısı)
- [Inspector Ayarları](#inspector-ayarları)
- [Kod Örnekleri](#kod-örnekleri)
- [Lisans](#lisans)

## ✨ Özellikler

- ✅ Smooth animasyonlu genişletme/daraltma efekti
- ✅ Tekli veya çoklu açık item desteği
- ✅ Header renk değişimi (açık/kapalı durumlar)
- ✅ Dinamik item yönetimi
- ✅ Kolay entegrasyon
- ✅ Canvas UI ile tam uyumlu
- ✅ Özelleştirilebilir animasyon süresi ve yükseklik

## 🚀 Kurulum

### Adım 1: Dosyaları Projeye Ekle

1. `AccordionItem.cs` dosyasını Unity projesine kopyala
2. `AccordionController.cs` dosyasını Unity projesine kopyala

**Önerilen dizin yapısı:**
```
Assets/
├── Scripts/
│   ├── UI/
│   │   ├── AccordionItem.cs
│   │   └── AccordionController.cs
```

### Adım 2: Canvas Yapısını Oluştur

Aşağıdaki hiyerarşiyi Unity Scene'inde oluştur:

```
Canvas
└── AccordionPanel (Panel)
    ├── AccordionItem_1 (Panel)
    │   ├── Header (Button)
    │   │   └── Text
    │   └── Content (Panel)
    │       └── Text
    ├── AccordionItem_2 (Panel)
    │   ├── Header (Button)
    │   │   └── Text
    │   └── Content (Panel)
    │       └── Text
    └── AccordionItem_3 (Panel)
        ├── Header (Button)
        │   └── Text
        └── Content (Panel)
            └── Text
```

### Adım 3: Component Ayarları

#### AccordionPanel (Ana Panel)

1. **RectTransform** ayarları:
   - Layout Group ekle: **Vertical Layout Group**
   - Ayarlar:
     - Child Force Expand: Height ✓
     - Child Control Size: Height ✓
     - Spacing: 10

#### Her AccordionItem için

1. **Layout Element** ekle
   - Preferred Height: 70 (header için)
   - Layout Priority: 1

2. **AccordionItem** script'ini ekle ve Inspector'da ayarla:
   - **Header Button**: Header Button'ını assign et
   - **Header Text**: Header Text'i assign et
   - **Content Panel**: Content Panel'i assign et
   - **Layout Element**: Layout Element'i assign et
   - **Header Background**: Header Image'ı assign et (isteğe bağlı)
   - **Expanded Height**: 300
   - **Animation Duration**: 0.3
   - **Expanded Header Color**: Beyaz veya istediğin renk
   - **Collapsed Header Color**: Gri veya istediğin renk

#### AccordionPanel

1. **AccordionController** script'ini ekle
2. Inspector'da ayarla:
   - **Allow Multiple Open**: False (sadece bir item açık)
   - **Allow All Closed**: True (tüm itemler kapalı olabilir)

## 📖 Kullanım

### Basit Kullanım

```csharp
// AccordionController'ı bul
AccordionController accordion = GetComponent<AccordionController>();

// Tüm itemleri aç
accordion.ExpandAll();

// Tüm itemleri kapat
accordion.CollapseAll();

// Belirli bir item'i aç/kapat
accordion.SetItemExpanded(0, true);

// Item sayısını öğren
int itemCount = accordion.GetAccordionItemCount();
```

### Dinamik Item Oluşturma

```csharp
public class AccordionManager : MonoBehaviour
{
    [SerializeField] private Transform accordionContainer;
    [SerializeField] private AccordionItem accordionItemPrefab;
    [SerializeField] private AccordionController controller;

    public void CreateAccordionItems(List<string> headers, List<string> contents)
    {
        for (int i = 0; i < headers.Count; i++)
        {
            AccordionItem newItem = Instantiate(accordionItemPrefab, accordionContainer);
            newItem.SetHeader(headers[i]);
            newItem.SetContent(contents[i]);
            controller.AddAccordionItem(newItem);
        }
    }
}
```

### Event Dinleme (İsteğe Bağlı)

AccordionItem'e event ekleme:

```csharp
// AccordionItem.cs içinde değiştir
public class AccordionItem : MonoBehaviour
{
    public UnityEvent onExpanded = new UnityEvent();
    public UnityEvent onCollapsed = new UnityEvent();

    public void ToggleAccordion()
    {
        // ... existing code ...
        
        if (isExpanded)
            onExpanded.Invoke();
        else
            onCollapsed.Invoke();
    }
}
```

## 📁 Dosya Yapısı

### AccordionItem.cs

**Sorumluluğu:** Tek bir accordion item'inin davranışını yönetir

**Ana Methodlar:**
- `ToggleAccordion()` - Item'i aç/kapat
- `SetExpanded(bool expanded)` - Durumu ayarla
- `SetHeader(string text)` - Header metnini ayarla
- `SetContent(string text)` - İçerik metnini ayarla

**Properties:**
- `IsExpanded` - Açık/Kapalı durumunu döndür

### AccordionController.cs

**Sorumluluğu:** Tüm accordion item'lerini yönetir ve aralarındaki mantığı kontrol eder

**Ana Methodlar:**
- `OnItemClicked(AccordionItem)` - Item tıklandığında çağrılır
- `ExpandAll()` - Tüm item'leri aç
- `CollapseAll()` - Tüm item'leri kapat
- `SetItemExpanded(int index, bool expanded)` - Belirli item'i ayarla
- `AddAccordionItem(AccordionItem)` - Yeni item ekle
- `RemoveAccordionItem(AccordionItem)` - Item sil

## ⚙️ Inspector Ayarları

### AccordionItem Script

| Ayar | Tür | Açıklama |
|------|-----|----------|
| Header Button | Button | Başlık button'u |
| Header Text | Text | Başlık metni |
| Content Panel | RectTransform | İçerik paneli |
| Layout Element | LayoutElement | Yükseklik animasyonu için |
| Header Background | Image | Başlık arka planı |
| Expanded Height | Float | Açık durumda yükseklik (piksel) |
| Animation Duration | Float | Animasyon süresi (saniye) |
| Expanded Header Color | Color | Açık durumda başlık rengi |
| Collapsed Header Color | Color | Kapalı durumda başlık rengi |

### AccordionController Script

| Ayar | Tür | Açıklama |
|------|-----|----------|
| Accordion Items | List | Accordion item'lerinin listesi |
| Allow Multiple Open | Bool | Birden fazla item aynı anda açılabilir mi? |
| Allow All Closed | Bool | Tüm item'ler kapal�� olabilir mi? |

## 💻 Kod Örnekleri

### Örnek 1: FAQ Sistemi

```csharp
using UnityEngine;
using UnityEngine.UI;

public class FAQSystem : MonoBehaviour
{
    [SerializeField] private Transform accordionContainer;
    [SerializeField] private AccordionItem itemPrefab;
    [SerializeField] private AccordionController controller;

    private void Start()
    {
        CreateFAQ();
    }

    private void CreateFAQ()
    {
        var faqs = new[]
        {
            new { question = "Accordion nedir?", answer = "Accordion, başlığa tıklandığında içeriği gösteren/gizleyen UI bileşenidir." },
            new { question = "Nasıl kullanılır?", answer = "Script'leri projeye ekleyip Canvas'te hiyerarşi oluşturup component'leri assign edin." },
            new { question = "Animasyon hızı değişebilir mi?", answer = "Evet! Inspector'da Animation Duration değerini ayarlayabilirsiniz." }
        };

        foreach (var faq in faqs)
        {
            AccordionItem newItem = Instantiate(itemPrefab, accordionContainer);
            newItem.SetHeader(faq.question);
            newItem.SetContent(faq.answer);
            controller.AddAccordionItem(newItem);
        }
    }
}
```

### Örnek 2: Ayarlar Paneli

```csharp
using UnityEngine;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField] private AccordionController accordion;
    [SerializeField] private Button expandAllButton;
    [SerializeField] private Button collapseAllButton;

    private void Start()
    {
        expandAllButton.onClick.AddListener(() => accordion.ExpandAll());
        collapseAllButton.onClick.AddListener(() => accordion.CollapseAll());
    }

    public void ExpandSection(int index)
    {
        accordion.SetItemExpanded(index, true);
    }

    public void CollapseSection(int index)
    {
        accordion.SetItemExpanded(index, false);
    }
}
```

### Örnek 3: Database'den Veri Yükleme

```csharp
using UnityEngine;
using System.Collections.Generic;

public class AccordionDataLoader : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private AccordionItem prefab;
    [SerializeField] private AccordionController controller;

    public void LoadFromDatabase(List<AccordionData> dataList)
    {
        foreach (var data in dataList)
        {
            AccordionItem newItem = Instantiate(prefab, container);
            newItem.SetHeader(data.title);
            newItem.SetContent(data.description);
            controller.AddAccordionItem(newItem);
        }
    }
}

[System.Serializable]
public class AccordionData
{
    public string title;
    public string description;
}
```

## 🎨 Stil Özelleştirmesi

### Renk Şeması Değiştirme

```csharp
// AccordionItem inspector'da veya kod üzerinden
expandedHeaderColor = new Color(0.2f, 0.8f, 0.2f); // Yeşil
collapsedHeaderColor = new Color(0.8f, 0.8f, 0.8f); // Açık gri
```

### Animasyon Hızı Değiştirme

```csharp
// Hızlı: 0.1 - 0.2
// Normal: 0.3 - 0.5
// Yavaş: 0.6 - 1.0
animationDuration = 0.5f;
```

## 🐛 Sık Sorulan Problemler

### Problem 1: İçerik görünmüyor

**Çözüm:** Content Panel'in LayoutElement bileşeni olduğundan emin ol. Ayrıca Content Text'in LayoutElement'de "Preferred Height" ayarı olup olmadığını kontrol et.

### Problem 2: Animasyon keskin görünüyor

**Çözüm:** `animationDuration` değerini artır (örn: 0.5 veya 0.7)

### Problem 3: Birden fazla item açılıyor

**Çözüm:** AccordionController'da `Allow Multiple Open`'ı False olarak ayarla

### Problem 4: Item'ler üst üste çıkıyor

**Çözüm:** AccordionPanel'e Layout Group (Vertical Layout Group) ekle ve ayarlarını kontrol et

## 📝 Lisans

Bu proje özgürce kullanılabilir. MIT Lisansı altındadır.

## 👨‍💻 Geliştirici

Accordion Component - Unity 2021.3 LTS

---

**Sorularınız mı var?** GitHub Issues alanında soru sorabilirsiniz!