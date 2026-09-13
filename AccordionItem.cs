using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AccordionItem : MonoBehaviour
{
    [SerializeField] private Button headerButton;
    [SerializeField] private Text headerText;
    [SerializeField] private RectTransform contentPanel;
    [SerializeField] private LayoutElement layoutElement;
    [SerializeField] private Image headerBackground;
    
    [Space]
    [SerializeField] private float expandedHeight = 300f;
    [SerializeField] private float animationDuration = 0.3f;
    [SerializeField] private Color expandedHeaderColor = Color.white;
    [SerializeField] private Color collapsedHeaderColor = Color.gray;
    
    private bool isExpanded = false;
    private Coroutine animationCoroutine;
    private AccordionController accordionController;

    private void OnEnable()
    {
        if (headerButton != null)
        {
            headerButton.onClick.AddListener(OnHeaderClicked);
        }
    }

    private void OnDisable()
    {
        if (headerButton != null)
        {
            headerButton.onClick.RemoveListener(OnHeaderClicked);
        }
    }

    private void Start()
    {
        accordionController = GetComponentInParent<AccordionController>();
        
        if (layoutElement == null)
        {
            layoutElement = contentPanel.GetComponent<LayoutElement>();
            if (layoutElement == null)
            {
                layoutElement = contentPanel.gameObject.AddComponent<LayoutElement>();
            }
        }

        // İlk durumda kapalı olarak ayarla
        layoutElement.preferredHeight = 0;
        if (headerBackground != null)
        {
            headerBackground.color = collapsedHeaderColor;
        }
    }

    private void OnHeaderClicked()
    {
        if (accordionController != null)
        {
            accordionController.OnItemClicked(this);
        }
        else
        {
            ToggleAccordion();
        }
    }

    public void ToggleAccordion()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }

        isExpanded = !isExpanded;
        animationCoroutine = StartCoroutine(AnimateHeight(isExpanded ? expandedHeight : 0));
        UpdateHeaderColor();
    }

    public void SetExpanded(bool expanded)
    {
        if (expanded != isExpanded)
        {
            ToggleAccordion();
        }
    }

    private IEnumerator AnimateHeight(float targetHeight)
    {
        float elapsedTime = 0f;
        float startHeight = layoutElement.preferredHeight;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / animationDuration;
            layoutElement.preferredHeight = Mathf.Lerp(startHeight, targetHeight, progress);
            yield return null;
        }

        layoutElement.preferredHeight = targetHeight;
    }

    private void UpdateHeaderColor()
    {
        if (headerBackground != null)
        {
            headerBackground.color = isExpanded ? expandedHeaderColor : collapsedHeaderColor;
        }
    }

    public bool IsExpanded => isExpanded;

    public void SetHeader(string text)
    {
        if (headerText != null)
        {
            headerText.text = text;
        }
    }

    public void SetContent(string text)
    {
        Text contentText = contentPanel.GetComponentInChildren<Text>();
        if (contentText != null)
        {
            contentText.text = text;
        }
    }
}