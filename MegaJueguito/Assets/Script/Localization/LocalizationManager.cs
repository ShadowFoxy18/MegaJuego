using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LocalizationEntry
{
    public string key;
    public string value;
}

[Serializable]
public class LocalizationFile
{
    public string languageCode;
    public List<LocalizationEntry> entries = new List<LocalizationEntry>();
}

/// <summary>Loads language JSON files and returns text by stable key.</summary>
public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance { get; private set; }
    public static event Action LanguageChanged;

    [Header("Catalogs")]
    [SerializeField] private List<TextAsset> languageFiles = new List<TextAsset>();
    [SerializeField] private string defaultLanguage = "es";

    private readonly Dictionary<string, Dictionary<string, string>> catalogs =
        new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);
    private readonly HashSet<string> missingKeyWarnings = new HashSet<string>();
    private string currentLanguage;

    public string CurrentLanguage { get { return currentLanguage; } }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadCatalogs();
        SelectInitialLanguage();
    }

    /// <summary>Returns the translation for a key, or [key] if it cannot be found.</summary>
    public string GetText(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            return string.Empty;

        if (TryGetText(currentLanguage, key, out string value))
            return value;

        if (!string.Equals(currentLanguage, defaultLanguage, StringComparison.OrdinalIgnoreCase)
            && TryGetText(defaultLanguage, key, out value))
            return value;

        string warningId = currentLanguage + ":" + key;
        if (missingKeyWarnings.Add(warningId))
            Debug.LogWarning("No existe la key de localización '" + key + "' para el idioma '" + currentLanguage + "'.", this);

        return "[" + key + "]";
    }

    /// <summary>Changes the active language. Returns false if that catalog was not loaded.</summary>
    public bool SetLanguage(string languageCode)
    {
        if (string.IsNullOrWhiteSpace(languageCode) || !catalogs.ContainsKey(languageCode))
        {
            Debug.LogWarning("No se cargó ningún catálogo para el idioma '" + languageCode + "'.", this);
            return false;
        }

        if (string.Equals(currentLanguage, languageCode, StringComparison.OrdinalIgnoreCase))
            return true;

        currentLanguage = languageCode;
        missingKeyWarnings.Clear();
        LanguageChanged?.Invoke();
        return true;
    }

    /// <summary>Void wrapper for UnityEvents and UI Buttons.</summary>
    public void ChangeLanguage(string languageCode)
    {
        SetLanguage(languageCode);
    }

    public string[] GetAvailableLanguages()
    {
        string[] languages = new string[catalogs.Count];
        catalogs.Keys.CopyTo(languages, 0);
        return languages;
    }

    private void LoadCatalogs()
    {
        foreach (TextAsset languageFile in languageFiles)
        {
            if (languageFile == null)
                continue;

            LocalizationFile file;
            try
            {
                file = JsonUtility.FromJson<LocalizationFile>(languageFile.text);
            }
            catch (Exception exception)
            {
                Debug.LogError("No se pudo leer el catálogo '" + languageFile.name + "': " + exception.Message, languageFile);
                continue;
            }

            if (file == null || string.IsNullOrWhiteSpace(file.languageCode))
            {
                Debug.LogError("El catálogo '" + languageFile.name + "' necesita un languageCode.", languageFile);
                continue;
            }

            if (catalogs.ContainsKey(file.languageCode))
            {
                Debug.LogError("Hay más de un catálogo para el idioma '" + file.languageCode + "'.", languageFile);
                continue;
            }

            Dictionary<string, string> entries = new Dictionary<string, string>(StringComparer.Ordinal);
            if (file.entries != null)
            {
                foreach (LocalizationEntry entry in file.entries)
                {
                    if (entry == null || string.IsNullOrWhiteSpace(entry.key))
                    {
                        Debug.LogWarning("El catálogo '" + languageFile.name + "' contiene una entrada sin key.", languageFile);
                        continue;
                    }

                    if (entries.ContainsKey(entry.key))
                    {
                        Debug.LogError("La key '" + entry.key + "' está duplicada en '" + languageFile.name + "'.", languageFile);
                        continue;
                    }

                    entries.Add(entry.key, entry.value ?? string.Empty);
                }
            }

            catalogs.Add(file.languageCode, entries);
        }
    }

    private void SelectInitialLanguage()
    {
        if (catalogs.ContainsKey(defaultLanguage))
        {
            currentLanguage = defaultLanguage;
            return;
        }

        foreach (string languageCode in catalogs.Keys)
        {
            currentLanguage = languageCode;
            Debug.LogWarning("No existe el idioma predeterminado '" + defaultLanguage + "'; se usará '" + currentLanguage + "'.", this);
            return;
        }

        currentLanguage = defaultLanguage;
        Debug.LogWarning("LocalizationManager no tiene catálogos asignados.", this);
    }

    private bool TryGetText(string languageCode, string key, out string value)
    {
        value = null;
        return !string.IsNullOrEmpty(languageCode)
            && catalogs.TryGetValue(languageCode, out Dictionary<string, string> languageCatalog)
            && languageCatalog.TryGetValue(key, out value);
    }
}
