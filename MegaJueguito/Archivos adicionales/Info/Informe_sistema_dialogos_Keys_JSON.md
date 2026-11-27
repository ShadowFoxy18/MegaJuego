# Sistema de diálogo y localización

**Proyecto:** MegaJuego (Unity)  
**Estado:** la base de localización está implementada; la gestión de conversaciones sigue pendiente.  
**Última revisión:** 2026-11-27  
**Propósito:** servir como referencia del diseño y del trabajo futuro en `ChatBehavior`.

## 1. Estado actual del proyecto

### Implementado

- `Assets/Script/Localization/LocalizationManager.cs` carga catálogos JSON asignados como `TextAsset`, resuelve una key para el idioma activo, usa el idioma predeterminado como respaldo y muestra `[key]` cuando no encuentra una entrada.
- `Assets/Script/Localization/LocalizedText.cs` conecta una key con un componente `UnityEngine.UI.Text` y refresca el texto cuando cambia el idioma.
- `Assets/Localization/es.json` contiene el catálogo español inicial, actualmente sin entradas.
- `Assets/Script/Logic/Interact/InteractionSensor2D.cs` detecta la entrada, permanencia y salida del actor, y expone UnityEvents para conectarlos desde el Inspector.
- `Assets/Player.inputactions` incluye `Interact`, con controles de teclado E y botón izquierdo del ratón.
- Existe la carpeta reservada `Assets/Script/Logic/Interact/ChatBehavior/` para los futuros scripts de conversación; todavía no contiene scripts.

### Aún no implementado

- No se encontró un controlador que abra, avance y cierre conversaciones.
- No hay modelo ni cargador de conversaciones JSON, ni vista de diálogo con hablante, retrato, texto y opciones.
- `EventSystem.cs` inicia combate; no gestiona conversaciones.
- La acción `Interact` existe, pero todavía no está conectada a un flujo de diálogo.

La localización y el diálogo son sistemas relacionados, pero distintos: `LocalizationManager` devuelve texto para una key; el futuro sistema de conversación decidirá qué key mostrar y cuándo.

## 2. Estructura prevista

Los scripts propios del diálogo irán en:

~~~text
Assets/Script/Logic/Interact/ChatBehavior/
~~~

La localización general permanece en `Assets/Script/Localization/`. Los catálogos de idioma se guardan en `Assets/Localization/`. Así, el diálogo puede reutilizar la localización sin duplicar el cargador de textos.

Responsabilidades sugeridas para los scripts, aún sin crear:

| Responsabilidad | Trabajo |
|---|---|
| `ChatInteractable` | Asociar el NPC con un ID de conversación y solicitar su inicio. |
| `DialogueController` | Mantener la conversación activa, mostrar el nodo actual, avanzar y cerrar. |
| `ConversationData` | Representar IDs, nodo inicial, hablantes, líneas, transiciones y opciones cargadas del JSON. |
| `ConversationLoader` | Cargar y validar los datos de conversación. |
| `DialogueView` | Actualizar panel, nombre, retrato, texto y opciones; no decidir la lógica narrativa. |

Estos son nombres descriptivos propuestos, no scripts existentes ni nombres definitivos. Para el primer prototipo se puede mantener el número de scripts pequeño y separar responsabilidades cuando haga falta.

## 3. Flujo esperado

~~~text
InteractionSensor2D detecta al jugador
  → el jugador pulsa Interact
  → se resuelve el NPC objetivo y su conversationId
  → DialogueController solicita esa conversación
  → ConversationLoader entrega los datos y valida sus referencias
  → el nodo actual proporciona speakerId, portraitSide y textKey
  → LocalizationManager resuelve textKey en el idioma activo
  → DialogueView muestra el resultado
  → Interact avanza al siguiente nodo o selecciona una opción
  → al terminar, se cierra el panel y se devuelve el control
~~~

El sensor puede indicar quién está disponible, pero no debería leer archivos ni administrar el avance narrativo. El diálogo debe decidir cómo resolver dos NPC dentro del radio, qué ocurre al alejarse y si se bloquea el movimiento.

## 4. Identificadores y formato de contenido

### Selector de conversación

La convención propuesta durante el diseño usa una letra para el contexto y tres dígitos para el apartado narrativo:

- `P001`: pueblo, apartado 001.
- `D001`: dungeon, apartado 001.
- `G001`: conversación genérica, apartado 001.

Este código debe identificar qué conversación o bloque narrativo cargar. No es una key de idioma ni el texto que se presenta al jugador.

### Líneas y hablantes

Cada nodo necesita un ID estable, un hablante, el lado del retrato (`I` o `D`), una key de texto y su transición. El formato exacto que combina la letra del hablante, `I`/`D` y tres dígitos sigue pendiente de fijar. También hay que precisar si esos dígitos identifican una línea, una secuencia o los límites de un bloque. Un conteo total de líneas por sí solo no indica dónde empieza ni dónde termina la conversación.

Separar estos datos evita que un cambio de idioma altere el flujo:

- `conversationId`: localiza la conversación, por ejemplo `P001`.
- `nodeId`: identifica una línea/nodo dentro de ella.
- `speakerId`: identifica quién habla.
- `portraitSide`: `I` o `D`, cuando la presentación use retratos.
- `textKey`: key que `LocalizationManager` resuelve.
- `nextNodeId` u opciones: define la continuación.

Ejemplo conceptual de datos de conversación:

~~~json
{
  "conversationId": "P001",
  "startNodeId": "N001",
  "nodes": [
    {
      "id": "N001",
      "speakerId": "boticaria",
      "portraitSide": "I",
      "textKey": "dialogo.P001.N001",
      "nextNodeId": "N002",
      "choices": []
    }
  ]
}
~~~

El ejemplo muestra la separación de responsabilidades; el esquema JSON final y la convención final de IDs todavía no están cerrados.

Los catálogos de idioma contienen los textos visibles, siguiendo el formato ya usado por `LocalizationManager`:

~~~json
{
  "languageCode": "es",
  "entries": [
    { "key": "dialogo.P001.N001", "value": "Texto de ejemplo" }
  ]
}
~~~

Las keys deben ser estables y únicas en el catálogo. No deben contener el texto traducido. `JsonUtility` no admite diccionarios normales de forma directa; para JSON se usarán listas serializables con IDs y valores, y el cargador podrá construir un índice en memoria.

## 5. Reglas iniciales recomendadas

Para el primer resultado jugable:

1. Un NPC abre una conversación lineal identificada por `conversationId`.
2. Una conversación está activa a la vez.
3. La acción `Interact` inicia, avanza y cierra según el estado del diálogo; una pulsación no debe iniciar y avanzar la primera línea en el mismo evento.
4. El nodo termina al no tener transición siguiente. Las opciones se añaden después de comprobar el flujo lineal.
5. Una key ausente muestra `[key]` y registra una advertencia, como hace la localización actual.
6. Cada referencia a nodo y cada key se valida al cargar el contenido.

## 6. Etapas de trabajo

### Etapa 1 — Cerrar el comportamiento

Definir el NPC objetivo cuando hay varios, inicio/avance/cierre, movimiento durante la conversación, cierre por distancia o menú y qué pasa ante una segunda petición.

### Etapa 2 — Prototipo lineal

Crear la vista mínima y reproducir una conversación de prueba. Conectar `Interact` al controlador y probar apertura, avance, final y cierre.

### Etapa 3 — Datos externos

Definir el formato JSON final, elegir cómo se empaquetan y cargan los datos de conversación en Unity, y validar IDs duplicados, nodos inexistentes y keys faltantes.

### Etapa 4 — Localización

Reutilizar `LocalizationManager`; añadir entradas a `Assets/Localization/es.json` y verificar el respaldo de idioma. Añadir otro catálogo cuando exista una traducción real.

### Etapa 5 — Opciones

Incluir opciones en nodos y transiciones por ID. Añadir condiciones de historia solo cuando una conversación concreta las necesite.

### Etapa 6 — Contenido y herramientas

Crear conversaciones reales, comprobar referencias entre nodos y considerar un editor de Unity si editar JSON manualmente se vuelve difícil.

## 7. Decisiones pendientes

| Tema | Estado |
|---|---|
| Significado exacto de P/D/G y alcance de los tres dígitos | Falta confirmar con ejemplos de conversaciones. |
| Formato de ID de hablante, lado I/D y secuencia de diálogo | Falta precisar; separar del conteo total de líneas. |
| NPC objetivo cuando hay varios interactuables | Pendiente. |
| Avanzar y elegir respuestas con la misma acción `Interact` u otras acciones | Pendiente. |
| Cierre al alejarse, pausar o abrir menú | Pendiente. |
| Pausar movimiento del jugador | Pendiente. |
| Ubicación y formato final de JSON de conversaciones | Pendiente; los textos de idioma sí tienen base en `Assets/Localization/`. |
| Retratos, voces, condiciones y banderas de historia | Para fases posteriores. |

## 8. Registro

| Fecha | Avance | Estado |
|---|---|---|
| 2026-11-25 | Se propusieron conversaciones por contexto/keys, JSON y responsabilidades separadas. | Diseño inicial. |
| 2026-11-27 | Se actualizó el estado tras encontrar `LocalizationManager`, `LocalizedText`, `Assets/Localization/es.json` y la carpeta vacía `ChatBehavior`. Se corrigió que aún no existe un gestor de conversaciones. | Localización implementada; diálogo pendiente. |
