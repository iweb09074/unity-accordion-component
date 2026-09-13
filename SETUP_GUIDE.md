# Unity Accordion Component - Scene Setup Kılavuzu

Bu kılavuz, Unity 2021.3 LTS'de Accordion Component'ini adım adım nasıl kuracağınızı gösterir.

## 📋 Ön Koşullar

- Unity 2021.3 LTS veya üzeri
- Temel Canvas bilgisi
- AccordionItem.cs ve AccordionController.cs dosyaları projenizde

## 🎯 Adım 1: Dosyaları İçe Aktarma

1. `AccordionItem.cs` dosyasını `Assets/Scripts/UI/` dizinine kopyala
2. `AccordionController.cs` dosyasını `Assets/Scripts/UI/` dizinine kopyala
3. `AccordionExampleScene.cs` dosyasını `Assets/Scripts/UI/` dizinine kopyala (opsiyonel)

```
Assets/
├── Scripts/
│   └── UI/
│       ├── AccordionItem.cs
│       ├── AccordionController.cs
│       └── AccordionExampleScene.cs
```

## 🎨 Adım 2: Canvas ve Panel Oluşturma

### 2.1 Canvas Oluştur
1. Hierarchy'de sağ tıkla → UI → Canvas
2. Canvas ismini "MainCanvas" olarak değiştir

### 2.2 Ana Panel Oluştur
1. Canvas'e sağ tıkla → UI → Panel
2. İsmini "AccordionPanel" olarak değiştir
3. RectTransform ayarları:
   - Anchor: Center
   - Pos X: 0, Pos Y: 0
   - Width: 400, Height: 800
   - Scale: 1, 1, 1

### 2.3 ScrollView Ekle (Opsiyonel - Çok sayıda item için)

Eğer çok sayıda accordion item'i ekleyecekseniz:

1. AccordionPanel'e sağ tıkla → UI → Scroll View
2. Scroll View'i konfigüre et:
   - Content Layout Group'a Vertical Layout Group ekle
   - Preferred Height ayarını kaldır
   - Child Force Expand: Height ✓
   - Child Control Size: Height ✓

## 🔄 Adım 3: Accordion Item Prefab Oluşturma

### 3.1 Item Panel Oluştur
1. AccordionPanel'e sağ tıkla → UI → Panel
2. İsmini "AccordionItem" olarak değiştir
3. RectTransform:
   - Preferred Height: 70 (Layout Element ekleyip ayarla)
   - Layout Priority: 1

### 3.2 Header Button Oluştur
1. AccordionItem'e sağ tıkla → UI → Button
2. İsmini "Header" olarak değiştir
3. RectTransform:
   - Left: 0, Right: 0
   - Top: 0, Height: 60

### 3.3 Header Text Ekle
1. Header Button'a sağ tıkla → Text
2. İsmini "HeaderText" olarak değiştir
3. Text ayarları:
   - Content: "Item Başlığı"
   - Font: Arial veya istediğin font
   - Font Size: 18
   - Best Fit: ✓

### 3.4 Content Panel Oluştur
1. AccordionItem'e sağ tıkla → UI → Panel
2. İsmini "ContentPanel" olarak değiştir
3. RectTransform:
   - Top: 60, Bottom: 0
   - Layout Element: Preferred Height 0 (başlangıçta)

### 3.5 Content Text Ekle
1. ContentPanel'e sağ tıkla → Text
2. İsmini "ContentText" olarak değiştir
3. Text ayarları:
   - Content: "İçerik buraya gelecek"
   - Font: Arial
   - Font Size: 14
   - Alignment: Upper Left
   - Text wrapping: ✓

## ⚙️ Adım 4: Component'leri Ayarlama

### 4.1 AccordionItem Component'ini Ekle
1. AccordionItem GameObject'ini seç
2. Inspector → Add Component → AccordionItem

### 4.2 AccordionItem Component'ini Konfigüre Et

Inspector'da aşağıdaki alanları doldur:

| Alan | Değer |
|------|-------|
| Header Button | Header Button'ını drag-drop et |
| Header Text | HeaderText'i drag-drop et |
| Content Panel | ContentPanel'i drag-drop et |
| Layout Element | Layout Element'i drag-drop et (ContentPanel'den) |
| Header Background | Header Button'ın Image component'i |
| Expanded Height | 300 |
| Animation Duration | 0.3 |
| Expanded Header Color | Beyaz (1, 1, 1, 1) |
| Collapsed Header Color | Gri (0.8, 0.8, 0.8, 1) |

### 4.3 Prefab Oluştur
1. AccordionItem GameObject'ini seç
2. Assets/Prefabs/ klasörüne sürükle
3. "Prefab/AccordionItem.prefab" olarak kaydet

## 📌 Adım 5: AccordionController Kurulumu

### 5.1 Controller Component'ini Ekle
1. AccordionPanel'i seç
2. Inspector → Add Component → AccordionController

### 5.2 AccordionController'ı Konfigüre Et

| Alan | Değer |
|------|-------|
| Allow Multiple Open | False |
| Allow All Closed | True |

### 5.3 Layout Group Ekle
1. AccordionPanel'e Layout Group ekle (Vertical Layout Group)
2. Ayarlar:
   - Child Force Expand: Height ✓
   - Child Control Size: Height ✓
   - Spacing: 10
   - Padding: Left 10, Right 10, Top 10, Bottom 10

## 🎮 Adım 6: Example Scene'i Kurma

### 6.1 Manager GameObject Oluştur
1. Hierarchy'de sağ tıkla → Create Empty
2. İsmini "AccordionManager" olarak değiştir

### 6.2 Script Ekle
1. AccordionManager'ı seç
2. Add Component → AccordionExampleScene

### 6.3 Script Referanslarını Ayarla

Inspector'da aşağıdaki alanları doldur:

| Alan | Atama |
|------|--------|
| Accordion Container | AccordionPanel'i drag-drop et |
| Accordion Item Prefab | AccordionItem.prefab'ı drag-drop et |
| Accordion Controller | AccordionPanel'deki AccordionController'ı drag-drop et |
| Expand All Button | Expand All Button'ını drag-drop et |
| Collapse All Button | Collapse All Button'ını drag-drop et |

### 6.4 Control Button'ları Oluştur

1. Canvas'e sağ tıkla → UI → Button
   - İsmini "ExpandAllButton" olarak değiştir
   - Position: Solda, üstte
   - Text: "Tümünü Aç"

2. Canvas'e sağ tıkla → UI → Button
   - İsmini "CollapseAllButton" olarak değiştir
   - Position: Solda, üstte (aşağısında)
   - Text: "Tümünü Kapat"

## ✅ Adım 7: Test Etme

1. Play butonuna tıkla
2. Accordion header'larına tıkla
3. "Tümünü Aç" ve "Tümünü Kapat" butonlarını test et

**Beklenen Davranış:**
- Header'a tıklama content'i açmalı/kapatmalı
- Smooth animasyon olmalı
- Header rengi değişmeli (açık/kapalı)
- Sadece bir item aynı anda açık olmalı

## 🔍 Hiyerarşi Özeti

Tam kurulu sahne şu şekilde görünmelidir:

```
Canvas
├── MainPanel (Panel)
│   └── Scroll View (opsiyonel)
│       └── Content
│           ├── AccordionItem_1 (Prefab Instance)
│           │   ├── Header (Button)
│           │   │   └── HeaderText (Text)
│           │   └── ContentPanel (Panel)
│           │       └── ContentText (Text)
│           ├── AccordionItem_2
│           │   ├── Header
│           │   │   └── HeaderText
│           │   └── ContentPanel
│           │       └── ContentText
│           └── AccordionItem_3
│               ├── Header
│               │   └── HeaderText
│               └── ContentPanel
│                   └── ContentText
├── ExpandAllButton (Button)
│   └── Text
├── CollapseAllButton (Button)
│   └── Text
└── AccordionManager (Empty GameObject)
    └── AccordionExampleScene (Script)
```

## 🛠️ Hızlı Sorun Giderme

### Problem: İçerik gösterilmiyor
**Çözüm:** ContentPanel'e Layout Element ekle ve Preferred Height değerini kontrol et

### Problem: Animasyon çalışmıyor
**Çözüm:** AccordionItem script'inde LayoutElement doğru atanmış mı kontrol et

### Problem: Button'lar tepki vermiyor
**Çözüm:** AccordionController script'inin kurulduğundan emin ol

### Problem: Çoklu item'ler açılıyor
**Çözüm:** AccordionController'da `Allow Multiple Open`'ı False yap

### Problem: Item'ler üst üste çıkıyor
**Çözüm:** AccordionPanel'e Vertical Layout Group ekle

## 📝 İyileştirmeler

### 1. Daha Fazla Item Ekleme

Script üzerinden dinamik olarak:

```csharp
AccordionExampleScene manager = GetComponent<AccordionExampleScene>();
// Otomatik olarak example veriyi yükler
```

### 2. Özel Stil Uygulamak

```csharp
AccordionItem item = accordionController.GetAccordionItem(0);
// Item üzerine özel stiller ekle
```

### 3. Veriyı Database'den Yükleme

Aşağıdaki gibi bir sistem oluşturabilesiniz:

```csharp
public void LoadAccordionFromDatabase(string query)
{
    List<AccordionData> data = DatabaseManager.GetData(query);
    foreach(var item in data)
    {
        // Accordion item oluştur
    }
}
```

## 🎓 Sonraki Adımlar

1. Animasyon sürelerini ve renkleri özelleştir
2. Dinamik içerik yükleme ekle
3. Scroll View ile entegre et
4. Event'ler ekle (onExpanded, onCollapsed)
5. Mobil touch davranışı için optimize et

---

**Başarıyla kuruldunuz! Accordion Component'iniz hazır.** 🎉
