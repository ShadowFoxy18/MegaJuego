using UnityEngine;

public class GenerateIcon : MonoBehaviour
{
    [SerializeField]
    private GameObject iconGameObject;

    [SerializeField]
    bool showIconOnStart = false;

    private void Awake()
    {
        if (iconGameObject == null)
        {
            Debug.LogError("Asigna un GameObject al campo iconGameObject en el Inspector.", this);
            enabled = false;
        }

        if (!showIconOnStart)
        {
            HideIcon();
        }
    }

    public void ShowIcon()
    {
        iconGameObject.SetActive(true);
    }

    public void HideIcon()
    {
        iconGameObject.SetActive(false);
    }
}
