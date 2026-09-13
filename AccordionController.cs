using UnityEngine;
using System.Collections.Generic;

public class AccordionController : MonoBehaviour
{
    [SerializeField] private List<AccordionItem> accordionItems = new List<AccordionItem>();
    [SerializeField] private bool allowMultipleOpen = false;
    [SerializeField] private bool allowAllClosed = true;

    private AccordionItem currentOpenItem;

    private void Start()
    {
        // Eğer accordion itemler atanmamışsa otomatik olarak bul
        if (accordionItems.Count == 0)
        {
            accordionItems.AddRange(GetComponentsInChildren<AccordionItem>());
        }
    }

    public void OnItemClicked(AccordionItem clickedItem)
    {
        if (allowMultipleOpen)
        {
            // Birden fazla item açık olabilir
            clickedItem.ToggleAccordion();
        }
        else
        {
            // Sadece bir item açık olabilir
            if (clickedItem.IsExpanded)
            {
                // Zaten açık olan item tıklandı, kapat
                clickedItem.ToggleAccordion();
                currentOpenItem = null;
            }
            else
            {
                // Yeni item açılıyor
                if (currentOpenItem != null && currentOpenItem != clickedItem)
                {
                    // Önceki açık item'i kapat
                    currentOpenItem.ToggleAccordion();
                }

                clickedItem.ToggleAccordion();
                currentOpenItem = clickedItem;
            }
        }
    }

    public void ExpandAll()
    {
        foreach (AccordionItem item in accordionItems)
        {
            if (!item.IsExpanded)
            {
                item.ToggleAccordion();
            }
        }
    }

    public void CollapseAll()
    {
        foreach (AccordionItem item in accordionItems)
        {
            if (item.IsExpanded)
            {
                item.ToggleAccordion();
            }
        }
        currentOpenItem = null;
    }

    public void SetItemExpanded(int index, bool expanded)
    {
        if (index >= 0 && index < accordionItems.Count)
        {
            AccordionItem item = accordionItems[index];
            if (item.IsExpanded != expanded)
            {
                item.ToggleAccordion();
            }
        }
    }

    public void AddAccordionItem(AccordionItem item)
    {
        if (!accordionItems.Contains(item))
        {
            accordionItems.Add(item);
        }
    }

    public void RemoveAccordionItem(AccordionItem item)
    {
        accordionItems.Remove(item);
        if (currentOpenItem == item)
        {
            currentOpenItem = null;
        }
    }

    public int GetAccordionItemCount()
    {
        return accordionItems.Count;
    }

    public AccordionItem GetAccordionItem(int index)
    {
        if (index >= 0 && index < accordionItems.Count)
        {
            return accordionItems[index];
        }
        return null;
    }
}