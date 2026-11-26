# Localización

Base de traducción por keys para el juego. Usa `UnityEngine.UI.Text` (el componente `Text` clásico de Unity UI).

## Preparación en Unity

1. Crea un GameObject vacío en la escena inicial y añade `LocalizationManager`.
2. En `Language Files`, añade `Assets/Localization/es.json` y establece `Default Language` en `es`.
3. En cada `Text` del Canvas que deba localizarse, añade `LocalizedText` y escribe su key.
4. Para cambiar idioma desde un botón o script, llama `ChangeLanguage("es")` al `LocalizationManager` (o `SetLanguage` desde código).

El gestor persiste entre escenas. Añade más archivos JSON a `Language Files` cuando se creen nuevos idiomas. Cada archivo usa este formato:

```json
{
  "languageCode": "es",
  "entries": [
    { "key": "dialogo.P001.N010", "value": "Texto traducido" }
  ]
}
```

Las keys distinguen mayúsculas y minúsculas y deben ser únicas dentro del catálogo. Si falta una traducción en el idioma activo, se usa la del idioma predeterminado. Si también falta allí, se muestra `[key]` y se registra una advertencia una vez.

Esta base resuelve y presenta textos. El gestor de diálogos decidirá más adelante qué key solicitar y cuándo avanzar la conversación.

