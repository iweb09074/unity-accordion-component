# Unity Accordion Component - İleri Seviye Kullanım Kılavuzu

Bu kılavuz, Accordion Component'ini gelişmiş senaryolarda nasıl kullanacağınızı gösterir.

## 📚 İçindekiler

- [Dinamik Veri Yükleme](#dinamik-veri-yükleme)
- [Event Sistemi](#event-sistemi)
- [Animasyon Özelleştirmesi](#animasyon-özelleştirmesi)
- [ScrollView ile Entegrasyon](#scrollview-ile-entegrasyon)
- [API İntegrasyonu](#api-integrasyonu)
- [Performans Optimizasyonu](#performans-optimizasyonu)
- [Mobil Optimizasyonu](#mobil-optimizasyonu)
- [Tema Yönetimi](#tema-yönetimi)

## 🔄 Dinamik Veri Yükleme

### Senaryo 1: JSON Dosyasından Veri Yükleme

```csharp
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class JSONAccordionLoader : MonoBehaviour
{
    [SerializeField] private TextAsset jsonFile;
    [SerializeField] private Transform container;
    [SerializeField] private AccordionItem prefab;
    [SerializeField] private AccordionController controller;

    [System.Serializable]
    public class AccordionDataList
    {
        public List<AccordionDataItem> items;
    }

    [System.Serializable]
    public class AccordionDataItem
    {
        public string title;
        public string content;
        public string category;
    }

    private void Start()
    {
        LoadFromJSON();
    }

    private void LoadFromJSON()
    {
        if (jsonFile == null)
        {
            Debug.LogError("JSON dosyası assign edilmemiş!");
            return;
        }

        AccordionDataList dataList = JsonUtility.FromJson<AccordionDataList>(jsonFile.text);

        foreach (var item in dataList.items)
        {
            CreateAccordionItem(item);
        }
    }

    private void CreateAccordionItem(AccordionDataItem data)
    {
        AccordionItem newItem = Instantiate(prefab, container);
        newItem.SetHeader(data.title);
        newItem.SetContent($"<b>{data.category}</b>\n{data.content}");
        controller.AddAccordionItem(newItem);
    }
}
```

**JSON Örneği:**
```json
{
  "items": [
    {
      "title": "Soru 1",
      "content": "Bu bir cevaptır",
      "category": "FAQ"
    },
    {
      "title": "Soru 2",
      "content": "Bu başka bir cevaptır",
      "category": "FAQ"
    }
  ]
}
```

### Senaryo 2: Database'den Veri Yükleme

```csharp
using UnityEngine;
using System.Collections.Generic;

public class DatabaseAccordionLoader : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private AccordionItem prefab;
    [SerializeField] private AccordionController controller;
    private DatabaseManager dbManager;

    private void Start()
    {
        dbManager = GetComponent<DatabaseManager>();
        LoadFromDatabase();
    }

    private void LoadFromDatabase()
    {
        // SQLite, Firebase, vb. database'den veri çek
        List<AccordionRecord> records = dbManager.GetAllRecords("accordion_table");

        foreach (var record in records)
        {
            CreateAccordionItem(record);
        }
    }

    private void CreateAccordionItem(AccordionRecord record)
    {
        AccordionItem newItem = Instantiate(prefab, container);
        newItem.SetHeader(record.Title);
        newItem.SetContent(record.Description);
        controller.AddAccordionItem(newItem);
    }
}

[System.Serializable]
public class AccordionRecord
{
    public int Id;
    public string Title;
    public string Description;
    public string Category;
    public System.DateTime CreatedDate;
}
```

## 📡 Event Sistemi

### Senaryo 3: Event-Based Accordion

```csharp
using UnityEngine;
using UnityEngine.Events;

public class EventBasedAccordion : MonoBehaviour
{
    public class AccordionExpandedEvent : UnityEvent<int> { }
    public class AccordionCollapsedEvent : UnityEvent<int> { }

    public static AccordionExpandedEvent onAccordionExpanded = new AccordionExpandedEvent();
    public static AccordionCollapsedEvent onAccordionCollapsed = new AccordionCollapsedEvent();

    private AccordionController controller;
    private int itemIndex = 0;

    private void Start()
    {
        controller = GetComponent<AccordionController>();
        SetupEventListeners();
    }

    private void SetupEventListeners()
    {
        // Event listener ekle
        onAccordionExpanded.AddListener(OnItemExpanded);
        onAccordionCollapsed.AddListener(OnItemCollapsed);
    }

    public void NotifyItemExpanded(int index)
    {
        onAccordionExpanded.Invoke(index);
    }

    public void NotifyItemCollapsed(int index)
    {
        onAccordionCollapsed.Invoke(index);
    }

    private void OnItemExpanded(int index)
    {
        Debug.Log($"Item {index} açıldı");
        // Analytics gönder
        SendAnalytics("accordion_expanded", index);
    }

    private void OnItemCollapsed(int index)
    {
        Debug.Log($"Item {index} kapatıldı");
        // Analytics gönder
        SendAnalytics("accordion_collapsed", index);
    }

    private void SendAnalytics(string action, int itemIndex)
    {
        // Google Analytics, Firebase Analytics vb.
        Debug.Log($"[Analytics] {action} - Item {itemIndex}");
    }

    private void OnDestroy()
    {
        onAccordionExpanded.RemoveListener(OnItemExpanded);
        onAccordionCollapsed.RemoveListener(OnItemCollapsed);
    }
}
```

### Senaryo 4: Custom Callback Sistemi

```csharp
using UnityEngine;
using System;

public class CallbackAccordionItem : AccordionItem
{
    public Action<bool> OnStateChanged;
    
    private bool previousState;

    public void Initialize(Action<bool> stateChangeCallback)
    {
        OnStateChanged = stateChangeCallback;
    }

    // AccordionItem'in ToggleAccordion methodunu override et
    public void ToggleAccordionWithCallback()
    {
        ToggleAccordion();
        
        if (IsExpanded != previousState)
        {
            OnStateChanged?.Invoke(IsExpanded);
            previousState = IsExpanded;
        }
    }
}
```

## 🎨 Animasyon Özelleştirmesi

### Senaryo 5: Özel Animasyon Efektleri

```csharp
using UnityEngine;
using System.Collections;

public class AdvancedAccordionAnimation : MonoBehaviour
{
    [SerializeField] private RectTransform contentPanel;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private AnimationCurve easingCurve = AnimationCurve.EaseInOutCubic();

    public IEnumerator AnimateExpand(float targetHeight)
    {
        float startHeight = contentPanel.rect.height;
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = easingCurve.Evaluate(elapsedTime / animationDuration);

            // Yükseklik animasyonu
            RectTransform rect = contentPanel.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, Mathf.Lerp(startHeight, targetHeight, progress));

            // Opacity animasyonu
            if (canvasGroup != null)
            {
                canvasGroup.alpha = Mathf.Lerp(0, 1, progress);
            }

            // Scale animasyonu
            contentPanel.localScale = Vector3.Lerp(
                new Vector3(1, 0, 1),
                Vector3.one,
                progress
            );

            yield return null;
        }

        contentPanel.sizeDelta = new Vector2(contentPanel.sizeDelta.x, targetHeight);
        if (canvasGroup != null)
            canvasGroup.alpha = 1;
        contentPanel.localScale = Vector3.one;
    }

    public IEnumerator AnimateCollapse()
    {
        float startHeight = contentPanel.rect.height;
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = easingCurve.Evaluate(elapsedTime / animationDuration);

            RectTransform rect = contentPanel.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, Mathf.Lerp(startHeight, 0, progress));

            if (canvasGroup != null)
            {
                canvasGroup.alpha = Mathf.Lerp(1, 0, progress);
            }

            contentPanel.localScale = Vector3.Lerp(
                Vector3.one,
                new Vector3(1, 0, 1),
                progress
            );

            yield return null;
        }

        contentPanel.sizeDelta = new Vector2(contentPanel.sizeDelta.x, 0);
        if (canvasGroup != null)
            canvasGroup.alpha = 0;
        contentPanel.localScale = new Vector3(1, 0, 1);
    }
}
```

## 📜 ScrollView ile Entegrasyon

### Senaryo 6: Sonsuz Scroll Accordion

```csharp
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InfiniteScrollAccordion : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform scrollContent;
    [SerializeField] private AccordionItem prefab;
    [SerializeField] private AccordionController controller;
    [SerializeField] private int itemsPerPage = 10;

    private List<AccordionData> allData;
    private int currentPage = 0;
    private bool isLoading = false;

    private void Start()
    {
        scrollRect.onValueChanged.AddListener(OnScrollChanged);
    }

    private void OnScrollChanged(Vector2 scrollPosition)
    {
        // Sayfanın sonuna yaklaşıldığında
        if (scrollPosition.y < 0.2f && !isLoading)
        {
            LoadMoreItems();
        }
    }

    private void LoadMoreItems()
    {
        isLoading = true;
        int startIndex = currentPage * itemsPerPage;
        int endIndex = Mathf.Min(startIndex + itemsPerPage, allData.Count);

        for (int i = startIndex; i < endIndex; i++)
        {
            CreateAccordionItem(allData[i]);
        }

        currentPage++;
        isLoading = false;
    }

    private void CreateAccordionItem(AccordionData data)
    {
        AccordionItem newItem = Instantiate(prefab, scrollContent);
        newItem.SetHeader(data.title);
        newItem.SetContent(data.content);
        controller.AddAccordionItem(newItem);
    }
}
```

## 🌐 API İntegrasyonu

### Senaryo 7: REST API'den Veri Çekme

```csharp
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class APIAccordionLoader : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private AccordionItem prefab;
    [SerializeField] private AccordionController controller;
    [SerializeField] private string apiUrl = "https://api.example.com/accordion";

    private void Start()
    {
        StartCoroutine(LoadFromAPI());
    }

    private IEnumerator LoadFromAPI()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(apiUrl))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"API Hatası: {www.error}");
                yield break;
            }

            string jsonData = www.downloadHandler.text;
            APIResponse response = JsonUtility.FromJson<APIResponse>(jsonData);

            foreach (var item in response.items)
            {
                CreateAccordionItem(item);
            }
        }
    }

    private void CreateAccordionItem(APIItem item)
    {
        AccordionItem newItem = Instantiate(prefab, container);
        newItem.SetHeader(item.title);
        newItem.SetContent(item.description);
        controller.AddAccordionItem(newItem);
    }

    [System.Serializable]
    public class APIResponse
    {
        public List<APIItem> items;
    }

    [System.Serializable]
    public class APIItem
    {
        public string title;
        public string description;
        public string id;
    }
}
```

## ⚡ Performans Optimizasyonu

### Senaryo 8: Object Pooling

```csharp
using UnityEngine;
using System.Collections.Generic;

public class AccordionItemPool : MonoBehaviour
{
    [SerializeField] private AccordionItem prefab;
    [SerializeField] private int poolSize = 20;

    private Queue<AccordionItem> availableItems = new Queue<AccordionItem>();
    private HashSet<AccordionItem> activeItems = new HashSet<AccordionItem>();

    private void Awake()
    {
        // Pool oluştur
        for (int i = 0; i < poolSize; i++)
        {
            AccordionItem item = Instantiate(prefab);
            item.gameObject.SetActive(false);
            availableItems.Enqueue(item);
        }
    }

    public AccordionItem GetItem()
    {
        AccordionItem item;

        if (availableItems.Count > 0)
        {
            item = availableItems.Dequeue();
        }
        else
        {
            item = Instantiate(prefab);
        }

        item.gameObject.SetActive(true);
        activeItems.Add(item);
        return item;
    }

    public void ReturnItem(AccordionItem item)
    {
        item.gameObject.SetActive(false);
        activeItems.Remove(item);
        availableItems.Enqueue(item);
    }

    public void ReturnAllItems()
    {
        foreach (var item in activeItems)
        {
            item.gameObject.SetActive(false);
            availableItems.Enqueue(item);
        }
        activeItems.Clear();
    }
}
```

### Senaryo 9: Lazy Loading

```csharp
using UnityEngine;
using System.Collections;

public class LazyLoadAccordion : MonoBehaviour
{
    [SerializeField] private AccordionItem prefab;
    [SerializeField] private Transform container;

    public void CreateAccordionItemWithLazyLoad(string title, string contentUrl)
    {
        AccordionItem newItem = Instantiate(prefab, container);
        newItem.SetHeader(title);
        newItem.SetContent("Yükleniyor...");

        // İçeriği geç yükle
        StartCoroutine(LoadContentAsync(newItem, contentUrl));
    }

    private IEnumerator LoadContentAsync(AccordionItem item, string contentUrl)
    {
        // Simülasyon: gerçek uygulamada API çağrısı yapılabilir
        yield return new WaitForSeconds(1f);
        
        // İçeriği güncelle
        item.SetContent("Yüklenen içerik burada");
    }
}
```

## 📱 Mobil Optimizasyonu

### Senaryo 10: Touch Input ve Swipe Desteği

```csharp
using UnityEngine;
using UnityEngine.EventSystems;

public class MobileAccordionOptimization : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Vector2 touchStartPos;
    private Vector2 touchEndPos;
    private AccordionItem associatedItem;

    private void Start()
    {
        associatedItem = GetComponent<AccordionItem>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        touchStartPos = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        touchEndPos = eventData.position;
        
        // Swipe algıla
        float swipeDistance = Vector2.Distance(touchStartPos, touchEndPos);
        
        // Swipe değilse item'i aç/kapat
        if (swipeDistance < 50f)
        {
            associatedItem.ToggleAccordion();
        }
    }
}

// Double-tap desteği
public class DoubleTapAccordion : MonoBehaviour
{
    private float lastTapTime = 0f;
    private float doubleTapThreshold = 0.3f;
    private AccordionItem item;

    private void Start()
    {
        item = GetComponent<AccordionItem>();
    }

    public void OnTap()
    {
        float timeSinceLastTap = Time.time - lastTapTime;

        if (timeSinceLastTap < doubleTapThreshold)
        {
            // Double-tap algılandı
            item.SetExpanded(true);
        }
        else
        {
            // Single-tap
            item.ToggleAccordion();
        }

        lastTapTime = Time.time;
    }
}
```

## 🎨 Tema Yönetimi

### Senaryo 11: Dinamik Tema Sistemi

```csharp
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class AccordionTheme
{
    public Color expandedHeaderColor;
    public Color collapsedHeaderColor;
    public Color textColor;
    public float animationDuration;
    public string themeName;
}

public class AccordionThemeManager : MonoBehaviour
{
    [SerializeField] private List<AccordionTheme> themes = new List<AccordionTheme>();
    [SerializeField] private AccordionItem[] accordionItems;
    private int currentThemeIndex = 0;

    private void Start()
    {
        ApplyTheme(0);
    }

    public void ApplyTheme(int themeIndex)
    {
        if (themeIndex >= themes.Count)
        {
            Debug.LogWarning("Tema bulunamadı!");
            return;
        }

        AccordionTheme theme = themes[themeIndex];
        currentThemeIndex = themeIndex;

        foreach (var item in accordionItems)
        {
            // Tema ayarlarını uygula (custom property eklenmesi gerekir)
            // item.ApplyTheme(theme);
        }

        Debug.Log($"Tema '{theme.themeName}' uygulandı");
    }

    public void SwitchToNextTheme()
    {
        currentThemeIndex = (currentThemeIndex + 1) % themes.Count;
        ApplyTheme(currentThemeIndex);
    }
}
```

### Senaryo 12: Light/Dark Mode

```csharp
using UnityEngine;
using UnityEngine.UI;

public class AccordionLightDarkMode : MonoBehaviour
{
    private bool isDarkMode = false;
    private AccordionItem[] accordionItems;

    [SerializeField] private Color lightModeHeaderColor = Color.white;
    [SerializeField] private Color darkModeHeaderColor = Color.black;
    [SerializeField] private Color lightModeTextColor = Color.black;
    [SerializeField] private Color darkModeTextColor = Color.white;

    private void Start()
    {
        accordionItems = GetComponentsInChildren<AccordionItem>();
    }

    public void ToggleLightDarkMode()
    {
        isDarkMode = !isDarkMode;
        ApplyColorMode();
    }

    private void ApplyColorMode()
    {
        Color headerColor = isDarkMode ? darkModeHeaderColor : lightModeHeaderColor;
        Color textColor = isDarkMode ? darkModeTextColor : lightModeTextColor;

        foreach (var item in accordionItems)
        {
            // Renkler uygulanır
            Image headerImage = item.GetComponentInChildren<Image>();
            if (headerImage != null)
            {
                headerImage.color = headerColor;
            }

            Text headerText = item.GetComponentInChildren<Text>();
            if (headerText != null)
            {
                headerText.color = textColor;
            }
        }

        Debug.Log(isDarkMode ? "Dark Mode Açık" : "Light Mode Açık");
    }
}
```

## 🔗 Tüm Senaryo Kombinasyonu

Tüm bu özellikleri birleştirerek kompleks bir sistem oluşturabilirsiniz:

```csharp
using UnityEngine;

public class ComplexAccordionSystem : MonoBehaviour
{
    [SerializeField] private APIAccordionLoader apiLoader;
    [SerializeField] private AccordionThemeManager themeManager;
    [SerializeField] private EventBasedAccordion eventSystem;
    [SerializeField] private InfiniteScrollAccordion infiniteScroll;

    private void Start()
    {
        // API'den veri yükle
        // apiLoader.LoadData();

        // Tema uygula
        themeManager.ApplyTheme(0);

        // Event'leri dinle
        EventBasedAccordion.onAccordionExpanded.AddListener(OnAccordionExpanded);
    }

    private void OnAccordionExpanded(int index)
    {
        Debug.Log($"Accordion {index} açıldı - Analytics gönder");
    }
}
```

---

**Bu ileri seviye örneklerle, Accordion Component'ini profesyonel uygulamalarda kullanabilirsiniz!** 🚀
