using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AccordionExampleScene : MonoBehaviour
{
    [SerializeField] private Transform accordionContainer;
    [SerializeField] private AccordionItem accordionItemPrefab;
    [SerializeField] private AccordionController accordionController;
    [SerializeField] private Button expandAllButton;
    [SerializeField] private Button collapseAllButton;

    private List<AccordionData> exampleData;

    private void Start()
    {
        InitializeExampleData();
        CreateAccordionItems();
        SetupButtons();
    }

    /// <summary>
    /// Örnek veri listesini başlat
    /// </summary>
    private void InitializeExampleData()
    {
        exampleData = new List<AccordionData>
        {
            new AccordionData
            {
                title = "📱 Mobil Uygulama Mimarisi",
                content = "Mobil uygulamalar genellikle Model-View-Controller (MVC) veya Model-View-ViewModel (MVVM) " +
                         "mimarisini takip ederler. Bu yaklaşımlar kodun bakımını ve test edilebilirliğini kolaylaştırır. " +
                         "Unity'de de benzer mimariler uygulanabilir ve oyun veya uygulama geliştirmesi sırasında önemli rol oynarlar."
            },
            new AccordionData
            {
                title = "🎮 Unity'de UI Tasarımı",
                content = "Unity'nin Canvas sistemi, 2D UI elemanlarını oluşturmak için güçlü bir araçtır. " +
                         "RectTransform, Layout Groups ve Animation sistemini kullanarak interaktif ve dinamik " +
                         "arayüzler tasarlayabilirsiniz. Accordion Component, UI'nızı daha kompakt ve kullanıcı dostu hale getirir."
            },
            new AccordionData
            {
                title = "⚡ Performans Optimizasyonu",
                content = "Accordion Component'i kullanırken performans önemlidir. Object pooling, " +
                         "coroutine yönetimi ve canvas render call optimizasyonu gibi teknikler uygulanabilir. " +
                         "Özellikle çok sayıda accordion item'i olan listelerde bu optimizasyonlar kritiktir."
            },
            new AccordionData
            {
                title = "🎨 Stil ve Animasyon",
                content = "Accordion component'inin görünümünü özelleştirmek için renkler, fontlar ve animasyon " +
                         "sürelerini ayarlayabilirsiniz. Smooth animasyonlar kullanıcı deneyimini iyileştirir. " +
                         "Farklı durumlar (expanded, collapsed, hover) için farklı stil tanımlamaları yapabilirsiniz."
            },
            new AccordionData
            {
                title = "🔧 Teknik İpuçları",
                content = "1. LayoutElement'i her zaman kullanın\n" +
                         "2. Animasyon sürelerini tutarlı tutun\n" +
                         "3. Header ve Content panel'lerini doğru hiyerarşide oluşturun\n" +
                         "4. AccordionController'ı parent panel'de tutun\n" +
                         "5. Dinamik item oluşturma sırasında prefab'dan instantiate edin"
            }
        };
    }

    /// <summary>
    /// Accordion item'lerini oluştur
    /// </summary>
    private void CreateAccordionItems()
    {
        if (accordionItemPrefab == null)
        {
            Debug.LogError("AccordionItem prefab'ı assign edilmemiş!");
            return;
        }

        foreach (var data in exampleData)
        {
            // Prefab'dan yeni instance oluştur
            AccordionItem newItem = Instantiate(accordionItemPrefab, accordionContainer);
            
            // Başlık ve içeriği ayarla
            newItem.SetHeader(data.title);
            newItem.SetContent(data.content);
            
            // Controller'a ekle
            if (accordionController != null)
            {
                accordionController.AddAccordionItem(newItem);
            }

            Debug.Log($"Accordion item oluşturuldu: {data.title}");
        }
    }

    /// <summary>
    /// Butonları ayarla
    /// </summary>
    private void SetupButtons()
    {
        if (expandAllButton != null)
        {
            expandAllButton.onClick.AddListener(OnExpandAllClicked);
        }
        else
        {
            Debug.LogWarning("Expand All Button'u assign edilmemiş!");
        }

        if (collapseAllButton != null)
        {
            collapseAllButton.onClick.AddListener(OnCollapseAllClicked);
        }
        else
        {
            Debug.LogWarning("Collapse All Button'u assign edilmemiş!");
        }
    }

    /// <summary>
    /// Tüm accordion item'lerini aç
    /// </summary>
    private void OnExpandAllClicked()
    {
        if (accordionController != null)
        {
            accordionController.ExpandAll();
            Debug.Log("Tüm accordion item'leri açıldı");
        }
    }

    /// <summary>
    /// Tüm accordion item'lerini kapat
    /// </summary>
    private void OnCollapseAllClicked()
    {
        if (accordionController != null)
        {
            accordionController.CollapseAll();
            Debug.Log("Tüm accordion item'leri kapatıldı");
        }
    }

    /// <summary>
    /// Belirli bir item'i aç
    /// </summary>
    public void ExpandItem(int index)
    {
        if (accordionController != null)
        {
            accordionController.SetItemExpanded(index, true);
        }
    }

    /// <summary>
    /// Belirli bir item'i kapat
    /// </summary>
    public void CollapseItem(int index)
    {
        if (accordionController != null)
        {
            accordionController.SetItemExpanded(index, false);
        }
    }

    /// <summary>
    /// Accordion Controller'ı al
    /// </summary>
    public AccordionController GetAccordionController()
    {
        return accordionController;
    }

    /// <summary>
    /// Accordion item sayısını al
    /// </summary>
    public int GetItemCount()
    {
        return accordionController != null ? accordionController.GetAccordionItemCount() : 0;
    }
}

/// <summary>
/// Accordion data modeli
/// </summary>
[System.Serializable]
public class AccordionData
{
    public string title;
    public string content;
}
