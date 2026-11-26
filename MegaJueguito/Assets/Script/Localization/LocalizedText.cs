using UnityEngine;
using UnityEngine.UI;

/// <summary>Shows the localized value of a key in a Unity UI Text component.</summary>
[RequireComponent(typeof(Text))]
public class LocalizedText : MonoBehaviour
{
    [SerializeField] private string localizationKey;
    [SerializeField] private Text targetText;

    private void Awake()
    {
        if (targetText == null)
            targetText = GetComponent<Text>();
    }

    private void OnEnable()
    {
        LocalizationManager.LanguageChanged += RefreshText;
        RefreshText();
    }

    private void Start()
    {
        // Start runs after every scene object's Awake, including the LocalizationManager.
        RefreshText();
    }

    private void OnDisable()
    {
        LocalizationManager.LanguageChanged -= RefreshText;
    }

    public void SetKey(string key)
    {
        localizationKey = key;
        RefreshText();
    }

    public void RefreshText()
    {
        if (targetText == null || LocalizationManager.Instance == null)
            return;

        targetText.text = LocalizationManager.Instance.GetText(localizationKey);
    }
}
