using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GlobalTpObjs : MonoBehaviour
{
    public static GlobalTpObjs Instance { get; private set; }

    [Header("Canvas de transición")]
    [SerializeField] private Canvas fadeCanvas;
    [SerializeField] private bool activateCanvasOnStart;
    [SerializeField, Min(0f)] private float fadeDuration = 0.4f;

    private CanvasGroup fadeGroup;
    private Coroutine activeFade;

    public bool IsReady { get { return enabled && fadeCanvas != null && fadeGroup != null; } }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        if (transform.parent == null)
            DontDestroyOnLoad(gameObject);

        if (fadeCanvas == null)
        {
            Debug.LogError("Asigna el Canvas de transición en GlobalTpObjs.", this);
            enabled = false;
            return;
        }

        fadeGroup = fadeCanvas.GetComponent<CanvasGroup>();
        if (fadeGroup == null)
            fadeGroup = fadeCanvas.gameObject.AddComponent<CanvasGroup>();

        fadeGroup.alpha = 0f;
        fadeGroup.blocksRaycasts = false;
        fadeGroup.interactable = false;
        EnsureBlackPanel();

        if (fadeCanvas.transform.parent == null)
            DontDestroyOnLoad(fadeCanvas.gameObject);
    }

    private void Start()
    {
        SetCanvasActive(activateCanvasOnStart);
    }

    /// <summary>Oscurece la pantalla. Se puede llamar desde un UnityEvent.</summary>
    public void Oscurecer()
    {
        StartFade(1f);
    }

    /// <summary>Aclara la pantalla. Se puede llamar desde un UnityEvent.</summary>
    public void Aclarar()
    {
        StartFade(0f);
    }

    /// <summary>Oscurece y permite que otro script espere hasta que termine el fundido.</summary>
    public IEnumerator OscurecerYEsperar()
    {
        yield return FadeAndWait(1f);
    }

    /// <summary>Aclara y permite que otro script espere hasta que termine el fundido.</summary>
    public IEnumerator AclararYEsperar()
    {
        yield return FadeAndWait(0f);
    }

    /// <summary>Activa el Canvas si estaba apagado o lo apaga si estaba activo.</summary>
    public void SwitchCanvas()
    {
        if (fadeCanvas == null)
            return;

        bool isOnSameObject = fadeCanvas.gameObject == gameObject;
        bool isActive = isOnSameObject ? fadeCanvas.enabled : fadeCanvas.gameObject.activeSelf;
        SetCanvasActive(!isActive);
    }

    private void SetCanvasActive(bool isActive)
    {
        if (fadeCanvas == null)
            return;

        bool isOnSameObject = fadeCanvas.gameObject == gameObject;
        if (activeFade != null)
        {
            StopCoroutine(activeFade);
            activeFade = null;
        }

        if (isOnSameObject)
            fadeCanvas.enabled = isActive;
        else
            fadeCanvas.gameObject.SetActive(isActive);

        if (fadeGroup != null)
            fadeGroup.blocksRaycasts = isActive && fadeGroup.alpha > 0f;
    }

    private void StartFade(float targetAlpha)
    {
        if (!enabled || fadeGroup == null)
            return;

        if (activeFade != null)
            StopCoroutine(activeFade);

        activeFade = StartCoroutine(FadeTo(targetAlpha));
    }

    private IEnumerator FadeAndWait(float targetAlpha)
    {
        if (!enabled || fadeGroup == null)
            yield break;

        if (activeFade != null)
            StopCoroutine(activeFade);

        activeFade = StartCoroutine(FadeTo(targetAlpha));
        yield return activeFade;
        activeFade = null;
    }

    private IEnumerator FadeTo(float targetAlpha)
    {
        fadeCanvas.gameObject.SetActive(true);
        fadeCanvas.enabled = true;
        fadeGroup.blocksRaycasts = true;

        float startAlpha = fadeGroup.alpha;
        if (fadeDuration <= 0f)
        {
            fadeGroup.alpha = targetAlpha;
        }
        else
        {
            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                fadeGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeDuration);
                yield return null;
            }

            fadeGroup.alpha = targetAlpha;
        }

        fadeGroup.blocksRaycasts = targetAlpha > 0f;
    }

    private void EnsureBlackPanel()
    {
        if (fadeCanvas.GetComponentInChildren<Image>(true) != null)
            return;

        GameObject panel = new GameObject("Black Fade Panel", typeof(RectTransform), typeof(Image));
        panel.transform.SetParent(fadeCanvas.transform, false);

        RectTransform rect = panel.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        Image image = panel.GetComponent<Image>();
        image.color = Color.black;
        image.raycastTarget = false;
    }
}
