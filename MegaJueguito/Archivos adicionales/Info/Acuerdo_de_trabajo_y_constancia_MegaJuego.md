# Acuerdo de trabajo y constancia — MegaJuego

**Fecha:** 27 de noviembre de 2026  
**Proyecto:** Aetherion: The Ethereal Tower / MegaJuego

## 1. Fuente oficial del proyecto

- El **GDD** y los **artefactos del proyecto** son la fuente oficial vigente del juego.
- Los archivos `.md` organizados por secciones son una versión anterior, creada antes de trasladar ese contenido al GDD. Pueden consultarse como referencia histórica o para localizar información, pero no prevalecen sobre el GDD y los artefactos actuales.
- No actualizaré el GDD hasta que el usuario lo pida expresamente.
- Al recibir esa petición, solo pasaré al GDD decisiones aceptadas y datos que correspondan; diferenciaré lo acordado de lo provisional y lo pendiente de verificar.

## 2. Cómo documentaremos el trabajo

- Toda respuesta sobre MegaJuego que contenga análisis, una decisión, una propuesta, una solución o una pregunta de diseño quedará registrada en uno o varios archivos `.md` dentro de `Archivos adicionales/Info`.
- Reutilizaré y actualizaré el MD temático apropiado cuando el contenido encaje; crearé otro cuando sea un tema independiente o el cambio merezca un registro separado.
- Cada registro indicará el estado del contenido: **Acordado**, **Propuesta**, **Pendiente** o **Por comprobar**. Una idea de análisis no se convertirá en decisión del equipo sin aceptación explícita.
- Las observaciones hechas solo leyendo código o documentos se describirán como revisión estática. No las presentaré como verificadas en Unity, jugadas o probadas si no se han ejecutado.
- En documentos de diseño se buscará conectar **mecánicas → dinámicas → experiencia del jugador (MDA)**: qué acciones tiene, qué decisiones producen y qué sensaciones o motivaciones persigue.

## 3. Uso de la skill Sakurai

La skill `sakurai` está instalada para orientar tareas de diseño, revisión y desarrollo de videojuegos. Se aplicará cuando ayude a MegaJuego, especialmente para revisar riesgo y recompensa, claridad de reglas, ritmo, recompensas, tutorial, rescate, legibilidad y foco en la experiencia del jugador. Sus recomendaciones se adaptarán al género, al alcance y a las decisiones del equipo; no se tratarán como reglas oficiales ni como decisiones ya aprobadas.

## 4. Constancia de lo realizado

1. Se instaló la skill `sakurai` desde el archivo proporcionado en `Archivos adicionales/Informes/sakurai-skill-main.zip`. La skill es una recopilación fan-made resumida mediante IA; no es material oficial de Masahiro Sakurai.
2. Se leyó el informe `Informe_total_MegaJuego_v1_(27_11_2026).md` y se tuvo en cuenta la imagen con el esquema MDA y preguntas de progresión por sesión.
3. Se hizo una revisión estática de scripts principales de combate, incluyendo `TurnQueue`, `CombatDirector`, `DamageManager`, `Character`, `SkillData` y tipos de turno. No se ejecutó Unity ni se hizo una prueba de juego.
4. Se creó `Preguntas_y_puntos_pendientes_MegaJuego.md`, con elementos que ya tienen base, preguntas abiertas, comprobaciones técnicas y un orden de trabajo sugerido.
5. Se concluyó que el GDD y los artefactos actuales son la fuente oficial, y que los MD anteriores son una versión histórica previa al traslado de contenido. Esta regla de trabajo queda acordada por el usuario.

## 5. Observaciones técnicas registradas, pendientes de verificación en ejecución

- `TurnQueue` ya contiene una base de orden por AV.
- `CombatDirector.ProcessTurn()` obtiene la decisión de un agente y la registra, pero en el código revisado todavía no aplica los efectos de la acción.
- `ActionMoment` y `SkillData` estaban vacíos en la revisión estática.
- En `DamageManager`, el término `(nivel del atacante + 20)` aparece tanto en numerador como en denominador del factor de defensa y se cancela algebraicamente.
- El factor elemental está definido como método, pero no participa en `CalculateDamage()` en el código revisado.
- La convención entre resistencia neutral, su valor inicial y `1 - resistencia` necesita aclaración para asegurar que no anule daño por defecto.

Estas observaciones describen el estado de los archivos leídos en esta revisión. Deben comprobarse tras cambios y no sustituyen una prueba en Unity.

## 6. Próximo punto de trabajo

El archivo `Preguntas_y_puntos_pendientes_MegaJuego.md` queda como lista de conversación y planificación. Un buen siguiente paso es responder una pregunta cada vez, registrar la respuesta aquí o en el MD temático correspondiente y, cuando el usuario lo pida, consolidar las decisiones aceptadas en el GDD.
