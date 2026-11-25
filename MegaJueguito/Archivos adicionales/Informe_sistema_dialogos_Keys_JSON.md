# Informe de diseño incremental — Sistema de diálogos

**Proyecto:** MegaJuego (Unity)  
**Estado:** planificación inicial; no se ha implementado todavía un sistema de diálogos.  
**Objetivo:** construirlo poco a poco, validando cada parte antes de añadir la siguiente.

## 1. Idea general

El sistema debe permitir que el jugador inicie y avance conversaciones con personajes. El contenido narrativo se almacenará fuera de la lógica del juego, en archivos JSON, y las líneas se identificarán mediante **keys** estables.

En este informe, **key** significa identificador de contenido (por ejemplo, `dialogo.boticaria.saludo`), no una tecla del teclado. La tecla de interacción es una entrada aparte. El proyecto ya tiene una acción `Interact` asociada a E en `Assets/Player.inputactions`, pero queda por comprobar cómo la escena/controlador actual la consume y conectarla al futuro sistema.

La separación prevista es:

1. **Entrada:** detectar cuándo el jugador quiere hablar o avanzar.
2. **Conversación:** decidir qué nodo o línea se muestra a continuación.
3. **Datos:** leer nodos y opciones desde JSON.
4. **Textos:** resolver las keys al idioma activo.
5. **Interfaz:** mostrar nombre del hablante, texto y opciones.

## 2. Principios de diseño

- El texto no debe quedar escrito directamente dentro de los scripts de UI o interacción.
- Las keys son identificadores estables: cambiar una frase no debería exigir cambiar su key.
- El JSON contiene estructura y referencias; un catálogo de idioma contiene los textos visibles.
- La interacción del jugador se gestiona por Input System, no con consultas sueltas a `Input.GetKey` dentro del diálogo.
- El sistema debe poder comenzar con una conversación lineal y admitir opciones después.
- Los datos se validan antes de usarse: IDs duplicados o referencias inexistentes deben detectarse y dar mensajes útiles.
- Guardado, condiciones de historia, retratos, voces y edición visual quedan fuera de la primera versión salvo que se decida lo contrario.

## 3. Apartados y etapas de trabajo

### Etapa 0 — Cerrar alcance y flujo

Definir cómo se comporta una conversación antes de programarla:

- Qué objetos/personajes pueden iniciar diálogo.
- Si se inicia al pulsar E dentro de un área o al interactuar con un NPC concreto.
- Qué botón avanza las líneas y cuál selecciona una respuesta.
- Qué ocurre si el jugador se aleja, abre un menú o recibe otra entrada durante el diálogo.
- Qué acción cierra la conversación.
- Si el tiempo/jugador se bloquea mientras se conversa.
- Orden de prioridad cuando dos NPC pueden interactuarse a la vez.

**Entregable:** reglas cortas de interacción y cierre.

### Etapa 1 — Prototipo lineal en pantalla

Crear una conversación mínima que permita:

- Abrir el panel de diálogo.
- Mostrar hablante y texto.
- Avanzar de nodo con la entrada definida.
- Cerrar el panel al llegar al final.
- Evitar que una pulsación de inicio avance también la primera línea accidentalmente.

En esta etapa, el contenido puede ser de prueba. No se necesitan elecciones ni localización completa.

**Criterio de finalización:** un NPC inicia una conversación lineal, se muestran todas sus líneas en orden y el panel se cierra correctamente.

### Etapa 2 — Modelo de datos JSON

Representar cada conversación con un ID, un nodo inicial y una lista de nodos. Cada nodo tiene un ID propio, un hablante, una key de texto y una transición opcional.

Ejemplo conceptual:

~~~json
{
  "conversationId": "boticaria.intro",
  "startNodeId": "saludo",
  "nodes": [
    {
      "id": "saludo",
      "speakerKey": "npc.boticaria.nombre",
      "textKey": "dialogo.boticaria.saludo",
      "nextNodeId": "pregunta",
      "choices": []
    },
    {
      "id": "pregunta",
      "speakerKey": "npc.boticaria.nombre",
      "textKey": "dialogo.boticaria.pregunta",
      "nextNodeId": "",
      "choices": []
    }
  ]
}
~~~

La estructura está abierta a revisión. Conviene empezar con campos simples serializables. `JsonUtility` de Unity no admite diccionarios normales de forma directa, por lo que se recomienda guardar datos como listas de objetos con campos `id` y `value`, y construir un índice de búsqueda en memoria.

**Criterio de finalización:** una conversación de prueba se carga desde un `TextAsset` JSON y se reproduce sin texto duplicado en los scripts.

### Etapa 3 — Keys y catálogo de idioma

Separar los textos visibles en uno o más archivos de idioma. Ejemplo:

~~~json
{
  "language": "es",
  "entries": [
    {
      "key": "npc.boticaria.nombre",
      "value": "Boticaria"
    },
    {
      "key": "dialogo.boticaria.saludo",
      "value": "¿Necesitas algo para el viaje?"
    }
  ]
}
~~~

Convención recomendada para keys:

- Usar nombres descriptivos y estables.
- Separar categorías con puntos: `ui.confirmar`, `npc.boticaria.nombre`, `dialogo.boticaria.saludo`.
- Evitar incluir idioma en la key; el idioma se elige en el catálogo.
- No reutilizar una misma key para frases distintas si el contexto cambia su significado.

Hay que decidir más adelante qué hacer cuando una key no existe: mostrar la propia key, usar un idioma de respaldo o registrar un error visible en desarrollo.

**Criterio de finalización:** cambiar el catálogo activo permite cambiar idioma sin modificar el archivo de conversación.

### Etapa 4 — Opciones y ramificaciones

Añadir opciones al nodo. Cada respuesta mostrada al jugador apunta al ID del siguiente nodo. La conversación puede terminar con una transición vacía o un indicador explícito de final.

Ejemplo conceptual:

~~~json
{
  "id": "ofrecer_ayuda",
  "speakerKey": "npc.boticaria.nombre",
  "textKey": "dialogo.boticaria.ofrecer_ayuda",
  "nextNodeId": "",
  "choices": [
    {
      "textKey": "dialogo.boticaria.opcion.comprar",
      "nextNodeId": "hablar_de_compra"
    },
    {
      "textKey": "dialogo.boticaria.opcion.despedirse",
      "nextNodeId": "despedida"
    }
  ]
}
~~~

Las condiciones de opciones (misiones, objetos, flags) se aplazan hasta que exista un caso de juego concreto que las necesite.

**Criterio de finalización:** las opciones aparecen en pantalla y llevan al nodo indicado, sin depender de su posición en el archivo.

### Etapa 5 — Conexión con NPC e interacción

Vincular un personaje interactuable con el ID de conversación. El sensor/input solo solicita abrir ese ID; el gestor de diálogo se encarga de cargarlo y mostrarlo.

Propuesta de responsabilidades:

- **Interactable/NPC:** indica qué conversación iniciar.
- **Input de jugador:** comunica la acción de interactuar/avanzar.
- **DialogueManager:** mantiene la conversación activa y mueve el cursor entre nodos.
- **DialogueData:** representa conversación, nodo y opciones.
- **TextCatalog:** resuelve keys para el idioma actual.
- **DialogueView:** pinta hablante, línea y opciones; no decide reglas narrativas.

Estos son nombres de referencia, no nombres ya existentes ni definitivos.

### Etapa 6 — Validación y herramientas de contenido

Añadir validaciones para:

- Conversaciones sin nodo inicial.
- IDs de conversación o nodo duplicados.
- `nextNodeId` y opciones que apuntan a nodos inexistentes.
- Keys sin traducción.
- Ciclos accidentales o conversaciones sin salida.
- Archivos JSON con formato incorrecto.

La edición inicial puede hacerse en JSON con un editor de texto. Si el contenido crece, se puede valorar una herramienta de editor Unity para validar y previsualizar conversaciones.

## 4. Flujo esperado en runtime

~~~text
Jugador pulsa Interact
  → resolver NPC objetivo
  → pedir conversación por conversationId
  → cargar/obtener ConversationData
  → saltar al startNodeId
  → resolver speakerKey y textKey en el catálogo del idioma
  → mostrar DialogueView
  → al avanzar, seguir nextNodeId
  → si hay choices, presentar opciones y seguir el nodo elegido
  → al terminar, cerrar la vista y devolver el control
~~~

Conviene que el gestor mantenga un único diálogo activo inicialmente. El comportamiento ante otra petición de conversación durante una activa se debe fijar en la Etapa 0.

## 5. Decisiones pendientes

| Tema | Estado |
|---|---|
| Keys significan IDs de texto/contenido | Interpretación asumida para este informe; confirmar si el término se usa con otro significado. |
| Entrada E para iniciar/avanzar | Existe la acción `Interact` en `Player.inputactions`; falta definir/confirmar el flujo exacto en las escenas. |
| Una conversación activa a la vez | Recomendación inicial, pendiente de acordar. |
| JSON separado por conversación o catálogo global | Pendiente; se puede empezar con archivos pequeños y organizar según crezca el contenido. |
| Dónde alojar JSON en Unity | Pendiente: `Resources`, carga desde archivo u otro sistema. Debe elegirse antes de escribir el cargador. |
| Idiomas iniciales | Pendiente. |
| Cierre al alejarse o abrir menú | Pendiente. |
| Pausa de movimiento durante diálogo | Pendiente. |
| Retratos, animaciones y voces | Fuera del prototipo lineal inicial. |
| Condiciones/flags de historia | Aplazadas hasta tener un caso concreto. |

## 6. Riesgos y decisiones técnicas

- No usar el texto visible como ID: editar la frase rompería referencias.
- No basar los saltos en el orden de los nodos en el JSON: usar IDs explícitos.
- Evitar cargar archivos por el sistema de rutas del ordenador; el juego debe poder localizar el contenido en builds. La estrategia de carga elegida debe ser compatible con Unity y el empaquetado final.
- No crear un sistema de localización complejo antes de necesitar varios idiomas; aun así, usar keys desde el principio facilita añadirlos.
- Si se usa `JsonUtility`, mantener el modelo en clases serializables sencillas y listas; no depender de diccionarios o polimorfismo complejo.
- El JSON no debe permitir referencias silenciosamente vacías. Validar datos al cargar en desarrollo.
- Las líneas y opciones deben resolverse por key y tener un mensaje de respaldo para ayudar a encontrar contenido faltante.

## 7. Orden recomendado para avanzar

1. Acordar el flujo de inicio, avance y cierre.
2. Diseñar el JSON mínimo para una conversación lineal.
3. Elegir cómo se cargan los JSON dentro de Unity.
4. Implementar el cargador y validar un archivo de ejemplo.
5. Crear la vista y reproducir la conversación.
6. Conectar una acción del Input System.
7. Añadir opciones/ramas.
8. Añadir catálogo de idioma.
9. Conectar NPCs y después valorar condiciones de historia.

Este orden mantiene el primer resultado pequeño y jugable: primero se comprueba que el diálogo se reproduce; luego se generaliza el contenido y se amplían sus opciones.

## 8. Registro de decisiones y cambios

| Fecha | Decisión / avance | Estado |
|---|---|---|
| 2026-11-25 | Se propone crear diálogos mediante IDs/keys y datos JSON. El proyecto tiene una acción `Interact` en `Player.inputactions`, pero no se localizó un sistema de diálogos implementado en los scripts revisados. | Punto de partida |
| 2026-11-25 | Se deja pendiente simplificar la creación del Canvas del fundido del teletransporte; no forma parte del alcance del sistema de diálogos. | Pendiente separado |


