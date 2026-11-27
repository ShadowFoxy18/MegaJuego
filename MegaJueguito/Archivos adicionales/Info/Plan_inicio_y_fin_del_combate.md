# Plan futuro: inicio y final del combate

**Proyecto:** MegaJuego (Unity)  
**Estado:** diseño y tareas pendientes; este documento no implementa cambios en los scripts ni en las escenas.  
**Objetivo:** entrar a una zona de combate y volver a la dungeon conservando la escena y el estado que ya estaban cargados.

## 1. Decisión de diseño propuesta

Cargar `CombatZone` de forma **aditiva**. La escena de la dungeon permanece cargada en memoria mientras dura el combate. Al terminar, se descarga `CombatZone` y se reactiva la dungeon. Así se evita volver a cargar la dungeon y se conserva su estado en memoria.

Unity permite cargar escenas aditivamente mediante `SceneManager.LoadSceneAsync` con `LoadSceneMode.Additive`. También permite elegir la escena activa y descargar una escena por separado. La escena que se carga durante el juego debe incluirse en Build Settings.

Referencias de Unity:

- [SceneManager.LoadScene](https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.LoadScene.html)
- [SceneManager](https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.html)

## 2. Flujo esperado

~~~text
Dungeon activa
  -> se inicia un encuentro
  -> guardar los datos de regreso y preparar la sesión de combate
  -> desactivar temporalmente la lógica de exploración
  -> cargar CombatZone aditivamente
  -> activar CombatZone e iniciar el combate
  -> resolver victoria, derrota o huida
  -> guardar el resultado del combate
  -> descargar CombatZone
  -> reactivar Dungeon y devolver el control al jugador
~~~

La dungeon sigue cargada, pero durante el combate debe quedar inactiva en lo que afecte a la partida: movimiento, IA, interacciones, cámara de exploración y sistemas que no deban avanzar. Al volver, se reanudan desde el estado que tenían.

## 3. Información que debe conservar la sesión

Antes de cargar `CombatZone`, el sistema de transición debe conservar o entregar a la sesión de combate:

- Escena de origen y punto de regreso del jugador.
- Grupo activo y estado necesario de sus personajes.
- Enemigos y oleadas del encuentro.
- Identificador o referencia del encuentro para resolverlo una sola vez.
- Estado final: victoria, derrota o huida.
- Cambios que deban persistir al regresar, como vida, consumibles usados o recompensas.

Se recomienda transportar datos e identificadores, no referencias a objetos visuales que pertenezcan a una escena. La sesión debe tener un único responsable de confirmar el resultado y evitar aplicarlo dos veces.

## 4. Fases de implementación, de menor a mayor

### Fase 0 — Preparar la escena de combate

- Revisar `CombatZone` y decidir qué objetos de prueba se mantienen o se sustituyen por la arena definitiva.
- Añadir `CombatZone` a Build Settings.
- Acordar qué controladores de la dungeon se pausan mientras el combate esté abierto.

**Criterio de cierre:** la escena puede cargarse desde una partida y se sabe qué objetos pertenecen a cada escena.

### Fase 1 — Probar carga aditiva y regreso

- Crear una transición de prueba desde la dungeon a `CombatZone`.
- Cargar la escena asíncronamente en modo aditivo.
- Desactivar la entrada de exploración y establecer `CombatZone` como escena activa cuando termine de cargarse.
- Añadir una salida de prueba que descargue `CombatZone`, reactive la dungeon y restaure el control.

**Criterio de cierre:** se entra y se sale de la zona de combate sin recargar la dungeon ni perder su estado visible.

### Fase 2 — Evitar sistemas duplicados

Revisar objetos que pueden aparecer en ambas escenas o existir como servicios globales:

- `EventSystem` y EventSystem de UI.
- Cámaras y `AudioListener`.
- Controladores del jugador y sistemas de entrada.
- Managers singleton, música, iluminación y UI persistente.

Definir por cada sistema si vive en dungeon, combate o una escena común persistente. No se deben dejar dos instancias procesando la misma entrada o manejando el mismo servicio.

**Criterio de cierre:** durante la prueba hay una sola cámara de juego, un solo sistema de UI/entrada y no aparecen managers duplicados.

### Fase 3 — Entregar datos e iniciar el encuentro

- Conectar el disparador real del encuentro con el sistema de transición.
- Preparar una sesión con grupo y enemigos antes de entrar a `CombatZone`.
- Inicializar `CombatDirector` después de cargar la escena y validar que los datos necesarios existan.
- Bloquear inicios repetidos mientras una transición o combate esté activo.

**Criterio de cierre:** un encuentro real puede abrir la escena con los participantes correctos y registrar cada unidad una sola vez.

### Fase 4 — Completar el ciclo de combate

- Mantener la cola de turnos por AV existente.
- Añadir elección de acciones para personajes del jugador; la IA enemiga ya puede proponer acciones.
- Hacer que el director ejecute las acciones, en lugar de limitarse a registrarlas.
- Incorporar aplicación de daño y comunicación de unidades derrotadas.

**Criterio de cierre:** un encuentro sencillo puede avanzar de un turno al siguiente y terminar al derrotar a un equipo.

### Fase 5 — Resultado y retorno

- Detectar victoria cuando se hayan derrotado los enemigos de todas las oleadas.
- Detectar derrota cuando el grupo protagonista ya no pueda continuar.
- Añadir huida cuando esté conectada como acción válida.
- Emitir y guardar el resultado una sola vez.
- Descargar `CombatZone`, reactivar la dungeon y aplicar cambios acordados al grupo/encuentro.

**Criterio de cierre:** victoria, derrota y huida (si se implementa) devuelven al jugador correctamente y no duplican recompensas ni consumos.

### Fase 6 — Presentación visual de la zona

- Colocar combatientes y asignar `CombatUnit.VisualRepresentation`.
- Añadir interfaz de orden de turnos, acciones, objetivos y resultado.
- Añadir animaciones, cámara de combate y transiciones visuales.
- Asegurar que la interfaz solo representa el estado de combate; no debe convertirse en la fuente de verdad de las reglas.

**Criterio de cierre:** el combate se puede jugar y entender visualmente desde `CombatZone`.

### Fase 7 — Conectar sistemas avanzados

- Integrar la fórmula de daño definitiva y estadísticas.
- Añadir aplicaciones, anomalías, reacciones y colas de eventos.
- Conectar objetos asignados al inventario de combate y los consumibles.
- Resolver guardado, recompensas, encuentros persistentes y balance.

**Criterio de cierre:** los sistemas completos se resuelven dentro del ciclo de turnos sin saltarse el flujo de transición ni duplicar efectos.

## 5. Situación encontrada en el proyecto

En la revisión del proyecto se observó:

- `EventSystem.StartCombat()` instancia un prefab de combate en la escena actual y le pasa grupo y oleadas; no realiza la transición a `CombatZone`.
- `CombatZone` existe como escena de prueba, pero no está incluida actualmente en Build Settings.
- `CombatDirector` registra unidades y solicita decisiones, pero todavía no ejecuta las acciones ni gestiona el resultado completo.
- `TurnQueue` ya calcula el orden normal mediante AV.
- La IA enemiga puede proponer una acción; falta una interfaz equivalente para decidir la acción del jugador.
- La vida del personaje necesita una interfaz de combate para aplicar daño y notificar derrotas.
- La asignación visual de cada unidad y el ciclo de retorno a la dungeon están pendientes.

Por eso conviene validar la carga/regreso en una prueba pequeña y luego completar el ciclo lógico antes de conectar reacciones, inventario y animaciones.

## 6. Cuidados técnicos para la carga aditiva

- La carga aditiva conserva la dungeon en memoria; no pausa automáticamente sus scripts. Hay que detener explícitamente los sistemas que no deben avanzar.
- Al cargar `CombatZone`, decidir cuál escena es la activa para que los objetos creados durante el combate pertenezcan a la escena correcta.
- Controlar cámaras, `AudioListener`, UI y servicios globales para que las dos escenas no creen instancias duplicadas.
- Descargar únicamente `CombatZone` al terminar; conservar la dungeon y restaurar su estado.
- Validar una sola transición a la vez y manejar fallos de carga o datos de encuentro incompletos.
- Evitar que se aplique dos veces el cierre del combate si llegan eventos repetidos de victoria/derrota.
- Probar repetidamente entrada, salida y un segundo combate para detectar objetos persistentes o estado residual.

## 7. Orden recomendado de trabajo

1. Validar carga aditiva y retorno con una salida de prueba.
2. Pausar y reanudar correctamente los sistemas de dungeon.
3. Pasar los datos de encuentro a `CombatZone`.
4. Completar selección y ejecución de acciones, vida y derrotas.
5. Implementar resultados y aplicar cambios al regresar.
6. Añadir interfaz, arte, animaciones y sistemas elementales.

Este orden mantiene separada la transición entre escenas de las reglas del combate y permite localizar fallos por etapa.
