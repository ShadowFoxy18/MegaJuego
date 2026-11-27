# Informe total — MegaJuego (Aetherion: The Ethereal Tower)

> Informe de revisión del GDD y de los archivos adicionales, usando como criterio la skill de diseño de Masahiro Sakurai. Fecha de la revisión: 27-11-2026. El GDD todavía no está acabado y los archivos adicionales aún no se han integrado en él.

## 0. Alcance y límites de esta revisión

**Revisado:**

- `GDD - MegaJuego.docx` (la copia de la carpeta de archivos adicionales es idéntica al GDD subido antes).
- Informes `.md`: Reacciones elementales, Dinámicas de Corrupción, Ficha de elementos, Modelo de acciones y colas de combate, Plan de inicio y fin del combate, Inventario compartido y de combate, Economía y comercio, Diálogos por keys y JSON.
- Excel: `Progresion_DPS_MegaJuego.xlsx` y `Formula de daño.xlsx`.

**No revisado en detalle:** los artefactos HTML (configurador de multiplicadores, reacciones elementales, modelo de colas, progresión de AV) y el GIF de referencias. Según los informes, contienen los mismos datos que los `.md`.

**Advertencias:**

- La skill de Sakurai es un resumen hecho con IA a partir de subtítulos automáticos, así que puede contener errores. Los números `#NNN` indican el vídeo de origen.
- Los cálculos propios de este informe (marcados como tales) usan solo los datos de los archivos y no se han verificado en Unity.
- Las ideas de la sección 7 son propuestas para discutir, no decisiones.

---

## 1. El juego en resumen

RPG roguelike por turnos para PC (+16). Un grupo de tres aventureros escala una torre de energía corrupta, el Éter, a través de tres zonas (bosque, llanura helada y castillo). Cada piso incluye zona de inicio, mercader ambulante, zona de descanso y escalera.

| Personaje | Raza | Rol | Rasgo narrativo |
|---|---|---|---|
| Kael (guerrero) | Tabaxi | Daño directo; Físico | Busca venganza por su esposa; su hija espera su regreso |
| Sylvaine (maga) | Elfa | Daño elemental | Busca a una amiga perdida en una torre |
| Cassian (bardo) | Humano | Apoyo y debilitación; Éter | Quiere preservar las canciones de su reino destruido |

**Sistema de combate:** orden por AV (Valor de Acción), sistema elemental con reacciones, energía como recurso, y cinco acciones previstas (Atacar, Aguantar, Investigar, Consumir objeto, Huir).

---

## 2. Qué documenta cada archivo

### 2.1 GDD (docx)
Visión general, mecánicas, personajes, enemigos, NPC (tabla vacía), fórmula de daño y elementos. Está desactualizado respecto a los anexos: habla de nueve elementos, Torbellino y duraciones antiguas. El índice está vacío.

### 2.2 Reacciones elementales (`Reacciones_elementales_MegaJuego.md`)
- **Vocabulario:** elemento, aplicación (marca), anomalía, reacción y AV. Un ataque aplica su marca solo si la acción lo declara.
- **Duraciones base:** 2 turnos para Agua, Hielo, Fuego, Electricidad y Naturaleza; 3 turnos para Éter y Lumen.
- **Ramas:** Soporte (Éter, Lumen), Amplificadores (Viento, Naturaleza), Defensivos (Agua, Hielo), Ofensivos (Fuego, Electricidad) y Físico aparte.
- **Comportamientos de reacción:** Aditiva, Multiplicativa, Transformativa y Limpiadora (combinable con las demás).
- **Multiplicadores de golpe:** Vaporizado ×1,5; Derretido ×2,5; Plasma ×1,8; Quebrahielo ×1,3.
- **Catálogo (unas 20 reacciones):** Congelado, Sobrecarga, Florecimiento, Prisión Etérea, Agua Bendita (Santa Cura), Superconductor, Escarcha, Granizo Etéreo, Calcinado, Fuego Kármico, Fuego Fatuo, Intensificado, Descarga Etérea, Enredo Corrupto y sus variantes, y la secuencia Vórtice → Vórtice Etéreo → Huracanes.
- **Éter:** hasta 4 cargas de Corrupción Etérea; la quinta aplicación activa Corrupción durante 3 turnos.
- **Lumen:** aplica Purificación (3 turnos), que bloquea otros elementos y reacciones salvo Agua y Fuego.
- **Fórmulas:** daño normal (base × resistencia × elemental), curva de maestría, daño propio de reacción, y topes por personaje (ver 2.6).
- **Notas para Unity:** resolución por fases, cola de eventos determinista, datos editables con `ScriptableObject`.

### 2.3 Dinámicas de Corrupción (`Dinamicas_Corrupcion_Reacciones.md`)
- Cómo interactúa Corrupción con cada estado.
- **Regla acordada:** Sobrecarga + Corrupción se limpia y produce Prisión Etérea y Descarga Etérea.
- **Principios:** instantánea de estados antes del golpe, una reacción por evento, objetivo y estado, y amplificación calculada desde la base, no de forma recursiva.
- **Riesgos de ciclos:**
  - Granizo Etéreo → Helado → Congelado
  - Descarga Etérea → Electricidad → nuevas reacciones
  - Fuego Fatuo con varios ataques
  - Multiplicadores acumulados (Vaporizado × Derretido = ×3,75 hipotético; con Plasma, ×6,75)
- **Huracanes:** profundidad fija de tres niveles, sin bucle recursivo.

### 2.4 Ficha de elementos (`Ficha_elementos_MegaJuego.md`)
Especificación de interfaz de consulta de elementos y reacciones: selección de uno o dos elementos, botón "Ver interacciones", estados de versión (Acordado, Provisional, Pendiente, V1 histórica), paleta provisional de colores y modelo de datos para Unity.

### 2.5 Acciones y colas de combate (`Modelo_Acciones_y_Colas_de_Combate.md`)
- **Referencia:** Honkai: Star Rail.
- **Tipos de acción:** normal (cola por AV), adicional sin prioridad (FIFO) y adicional con prioridad (interrumpe y luego reanuda).
- **Unidad de tiempo:** 1 turno de referencia = 100 AV.
- **Acciones por 100 AV:**

| Personaje | Inicio | Medio | Final |
|---|---:|---:|---:|
| Maga | 2 | 4,5 | 6 |
| Bardo | 3 | 4 | 5,5 |
| Guerrero | 1 | 3,5 | 5 |

- Equivalencia con `TurnQueue`: `acciones por 100 AV = SPD / 100`.
- Siete decisiones abiertas (prioridad de contraataques, cancelación de adicionales, desempate de AV, etc.).

### 2.6 Progresión de daño (Excel)
- **Objetivo de daño por acción:** 1.000 / 5.000 / 15.000 / 40.000 (Inicio, Temprana, Media, Final). Según la hoja, es el mismo para los tres personajes.
- **Topes finales por personaje:**

| Rol | BaseDamage ref. | Maestría | Tasa de Anomalía | Bono elemental | Bono crítico |
|---|---:|---:|---:|---:|---:|
| Maga | 450 | 1000 | 120% | 50% | 250% |
| Bardo | 750 | 500 | 90% | 75% | 175% |
| Guerrero | 1250 | 250 | 45% | 20% | 300% |

- **Multiplicador elemental final (hoja):** Maga 3,44; Bardo 1,82; Guerrero 0,59.
- Los stats escalan linealmente con la fase (25%, 50%, 75%, 100%).
- El modelo no incluye reacciones, daño aditivo ni buffs de equipo.
- `Formula de daño.xlsx`: hoja de pruebas de la fórmula y de multiplicador de nivel. Su segunda hoja contiene una matriz de reacciones antigua (V1), casi toda con el mismo texto copiado.

### 2.7 Plan de inicio y fin del combate
- Cargar `CombatZone` de forma aditiva y mantener la dungeon en memoria, pausando sus sistemas.
- Siete fases, de menor a mayor, cada una con criterio de cierre.
- **Estado del proyecto según el informe:** `CombatDirector` aún no ejecuta acciones; `TurnQueue` ya ordena por AV; `CombatZone` no está en Build Settings; falta la interfaz de decisión del jugador.

### 2.8 Inventario compartido y de combate
- Inventario compartido como fuente única.
- El jugador prepara por personaje una asignación temporal antes del combate.
- Al terminar, se reconcilian consumos y se devuelve lo no usado.
- Pendientes: ranuras por personaje, coste en turnos de usar un objeto, qué pasa al huir o perder.

### 2.9 Economía y comercio
- Moneda común, monedero separado del inventario, transacciones atómicas.
- Servicios del herrero (mejora, fabricación, reparación solo si hay durabilidad).
- Venta y trueque por etapas.
- Todo está sin precios, sin nombre de moneda y sin servicios definidos.

### 2.10 Diálogos por keys y JSON
- La localización (`LocalizationManager`, `LocalizedText`) está implementada, pero `es.json` está vacío.
- El gestor de conversaciones (`ChatBehavior`) está pendiente.
- Convención propuesta: P/D/G + tres dígitos para el contexto (pueblo, dungeon, genérico).
- El formato de hablante, lado del retrato y secuencia aún no está cerrado.

---

## 3. Lo que funciona bien

- **Personajes con motivación personal clara.** Kael, Sylvaine y Cassian tienen razones propias para subir la torre.
- **Mundo coherente.** Todos los elementos nacen del Éter; Lumen es el Éter purificado y queda como desbloqueo futuro del bardo, un buen hilo de progresión.
- **Documentación rigurosa y honesta.** Separa lo acordado, lo provisional y lo pendiente, y los propios informes detectan sus riesgos de bucles.
- **Arquitectura de combate bien pensada.** Las colas, la instantánea de estados y la regla de una reacción por evento evitan cadenas descontroladas.
- **Inventario con decisión táctica.** Preparar objetos por personaje crea riesgo y recompensa antes del combate (#006, #015).
- **Rendimientos decrecientes en la maestría.** La curva evita el escalado sin freno.

---

## 4. Preocupaciones principales (según los criterios de Sakurai)

### 4.1 Falta el gancho central y el bucle roguelike
- El GDD describe el género ("roguelike + turnos + gestión"), pero no dice qué hace único al juego.
- Sakurai propone resumir el juego en una frase de riesgo y recompensa (#006).
- **El bucle roguelike sigue sin definirse:**
  - qué pasa al morir y qué se conserva,
  - cómo se generan los pisos (el GDD menciona "4 terrenos aleatorios" por zona, sin aclarar),
  - si hay rutas con más riesgo y más botín,
  - qué recompensa hay tras cada combate.
- Con mercader y descanso en cada piso, la tensión de "seguir o volver" desaparece (#015).
- Una carrera repetida necesita ver siempre algo nuevo delante (#127). Hoy hay tres zonas y la última no tiene enemigos.
- Sakurai recomienda decidir los premios apenas existe el esqueleto del juego (#075).

### 4.2 Complejidad del sistema elemental
- Hay unas 20 reacciones para solo tres personajes jugables. Subir el "juego" baja la accesibilidad (#146).
- Algunas cadenas son difíciles de leer en pantalla: Huracán Etéreo → reacción → Huracanes derivados; Sobrecarga + Corrupción → Prisión Etérea + Descarga Etérea.
- Sakurai propone el criterio de poder explicarle cada regla al jugador una a una (#021).
- El sistema recuerda mucho a las reacciones de Genshin Impact. Conviene justificar para qué sirve cada pieza en un juego por turnos de tres personajes (#009).

### 4.3 Peso del guerrero en las cifras actuales
- Según el Excel, el objetivo de daño por acción es igual para los tres personajes.
- El guerrero actúa menos en todas las fases (1 → 3,5 → 5 acciones por turno de referencia, frente a 2 → 4,5 → 6 de la maga y 3 → 4 → 5,5 del bardo).
- Su multiplicador elemental final es 0,59, frente a 3,44 de la maga.
- Su única reacción documentada es Quebrahielo (×1,3), la más débil.
- Con esto, el rol de "daño base +++" no se refleja todavía en los números. Sakurai prefiere exagerar las diferencias antes que igualarlas (#245).
- **Contexto:** el elemento Físico aún tiene mecánicas por definir, así que parte del hueco es de documentación. Ver sección 7.
- **Observación sobre la tabla de roles del GDD:** cada clase suma 6 cruces (3+1+2, 1+2+3, 2+3+1), un reparto muy simétrico.

### 4.4 Ritmo y duración del combate
- Con 5-6 acciones por personaje cada 100 AV, el final tendría unas 16 acciones por turno de referencia, más las de los enemigos y las reacciones.
- Hacen falta animaciones saltables y combates cortos (#116).
- La energía sigue sin definirse (tipos, consumo, recuperación) aunque el GDD la presenta como mecánica central.
- Aguantar recupera energía y reduce daño, y podría convertirse en la acción por defecto.
- Investigar solo da información; Sakurai recomienda que el sistema empuje a explotar debilidades (#015). Una idea: que Investigar muestre qué reacción provocaría cada ataque.

### 4.5 Rescate, tutorial y onboarding
- No hay tutorial: con cinco acciones, varios tipos de energía y los elementos, el primer combate debe enseñar jugando.
- No hay ninguna medida de rescate si el jugador se atasca (#211) ni de reintento rápido (#116).
- Sakurai insiste en que el valor del juego se vea en los primeros minutos, no en el lore inicial.

### 4.6 Orden de trabajo
- Hay mucha infraestructura avanzada (localización implementada, diseño de comercio y de diálogos) antes de comprobar que un combate completo es divertido.
- Sakurai insiste en probar el núcleo cuanto antes (#083).
- Sugerencia: un combate jugable con el trío, el AV, tres acciones y dos o tres reacciones (por ejemplo, Vaporizado y Derretido), y construir el resto después.

### 4.7 Economía genérica
- Sin nombre de moneda, precios ni servicios concretos del herrero.
- Sakurai pide que gastar sea atractivo, no solo posible (#075, #201): ¿qué cosa concreta querrá comprar el jugador?

### 4.8 Enemigos y alcance
- Los enemigos se describen por apariencia y no por rasgo de combate.
- El castillo no tiene enemigos y falta el jefe final. No se aclara si Fenrir es jefe.
- Los slimes elementales son una buena herramienta de enseñanza.

---

## 5. Incoherencias y errores detectados

| # | Dónde | Problema |
|---|---|---|
| 1 | GDD | El nombre aparece como *Aetherion*, *The Ethereal Tower* y *Etheral*; "Ciudad del juego" es un marcador sin nombre. |
| 2 | GDD | Índice y tabla de NPC vacíos; mezcla de "Mago" y "Maga". |
| 3 | GDD | Está desactualizado respecto a los anexos: nueve elementos, Torbellino, duraciones antiguas, Derretido con valores copiados de Vaporizado. |
| 4 | GDD y Excel | El GDD cita la constante **12** en el bono elemental; el Excel y los informes usan **1,2**. |
| 5 | Informe de reacciones | La fórmula de `Defense` tal como está transcrita tiene `(AtqLevel + 20)` arriba y abajo, así que se cancela. Con los valores de `Formula de daño.xlsx` (niveles 75 y 100, defensa 0,6) el resultado 0,6129 sale de `95 / (95 + 100 × 0,6)`. Parece una transcripción distinta de la hoja; conviene verificarlo. |
| 6 | `Formula de daño.xlsx`, Hoja2 | Matriz de reacciones antigua casi toda con el mismo texto ("Quemado \| Transformativa…"). Conviene actualizarla o quitarla. |
| 7 | Excel y colas de combate | El DPS es un modelo de hoja de cálculo, no una medición. `SkillData` está vacío y los valores de `Character.GetAttackStats()` son provisionales. |
| 8 | GDD | Kael y Sylvaine son ambos descritos como "fríos", lo que diluye el contraste. |
| 9 | GDD | "Gestión de recursos" se anuncia como pilar, pero solo se describen la energía y las monedas. |

Otras incoherencias menores del informe de reacciones, ya reconocidas por el propio documento: no queda claro si Corrupción reduce Resistencia Elemental o el atributo `PEN a Res`, y la frase de Superconductor tiene la misma ambigüedad.

---

## 6. Decisiones pendientes (consolidadas)

### Núcleo del juego
- Frase de gancho del juego y qué lo diferencia.
- Estructura de la carrera: muerte, qué se conserva, generación de pisos, rutas de riesgo.
- Recompensas tras combate y progresión entre carreras.
- Energía: tipos, consumo, recuperación y relación con Aguantar.
- Qué hace cada una de las cinco acciones con el modelo de AV, y si Investigar revela reacciones.

### Combate y reacciones
- Valores de las reacciones sin cerrar: Sobrecarga, Prisión Etérea, Granizo Etéreo, Fuego Fatuo, Descarga Etérea, Calcinado, Intensificado, Superconductor, Escarcha, Huracanes.
- Fórmula de `ReactionDamage` y cómo entra Tasa de Anomalía.
- Cargas de Corrupción: daño fijo, X% de Maestría, renovación y quinta aplicación.
- Qué atributo reduce Corrupción.
- Reacción Naturaleza + Viento (existe, sin nombre ni mecánica).
- Lumen + Naturaleza y Lumen + Éter.
- Cómo se resuelven varias reacciones válidas en un mismo impacto.
- Reglas de las colas de acciones: siete decisiones abiertas (contraataques, cancelación, desempate, etc.).
- Unidad del reloj de Granizo Etéreo.

### Personajes y progresión
- Mecánicas del elemento Físico.
- Lo que aporta cada personaje al sistema de reacciones (la maga genera elementos; ¿y el guerrero?).
- Si las curvas lineales por fase son definitivas.

### Sistemas
- Inventario: ranuras por personaje, coste de usar objetos, qué pasa al huir o perder.
- Economía: nombre de la moneda, precios, servicios del herrero, objetos no vendibles, stock.
- Diálogos: significado de P/D/G, formato de hablante y secuencia, cierre y movimiento durante conversaciones.
- Guardado y persistencia.

### Documento
- Cuál es la fuente de la verdad entre el GDD y los anexos.
- Paleta final de colores y efectos audiovisuales por reacción.

---

## 7. El elemento Físico: ideas para discutir

Hoy solo está definido Quebrahielo (Físico contundente + Congelado, ×1,3). Físico no usa marcas ni reacciones, y el guerrero tiene el perfil opuesto al de la maga: la maestría más baja (250), pero el mayor bono crítico (300%) y el mayor daño base de referencia. Eso sugiere una identidad propia: **fiabilidad y golpes pesados, no cadenas de reacciones**.

Ideas, sin decisión tomada:

1. **Escalado propio.** Que Físico no dependa de Maestría, sino de crítico y de reducir la defensa del enemigo (`DefReduction` ya existe en la fórmula).
2. **Riesgo y recompensa en el golpe** (#191, #236). Un ataque pesado que retrase la siguiente acción a cambio de mucho más daño; encaja con su ritmo lento.
3. **Hacer útil Aguantar.** Que el guerrero acumule el daño recibido y lo libere en su siguiente golpe.
4. **Respuesta a Purificación.** Lumen bloquea elementos y reacciones salvo Agua y Fuego, y Físico no es una marca. Habría que decidir si los golpes físicos siguen funcionando contra un enemigo purificado.
5. **Generalizar Quebrahielo.** Que el guerrero sea el "rompedor" de varios estados, no solo de Congelado.

**Riesgo:** si todo su valor es daño directo, el combate puede quedarse en "pulsar atacar". Conviene que Físico tenga decisiones propias.

---

## 8. Próximos pasos recomendados

1. **Decidir la fuente de la verdad** y actualizar el GDD con los anexos: unificar el nombre, completar el índice y la tabla de NPC, y añadir una tabla de decisiones abiertas con responsable y fecha.
2. **Añadir un diagrama del bucle principal** (explorar → combatir → premio → ciudad) y cerrar qué pasa al morir.
3. **Cerrar la fórmula:** verificar `Defense`, la constante 1,2 y los topes del Excel. Quitar o actualizar la Hoja2.
4. **Definir las mecánicas de Físico** y el reparto de aportes del trío al sistema de reacciones.
5. **Construir un combate mínimo jugable:** trío, AV, tres acciones y dos o tres reacciones. Probar si es divertido antes de añadir economía y diálogos.
6. **Diseñar la enseñanza progresiva:** asociar cada zona a elementos concretos y mostrar la reacción prevista antes de elegir el ataque.
7. **Definir energía y rescate:** tipos de energía, animaciones saltables y una medida de ayuda si el jugador se atasca.
8. **Medir en Unity** el daño real y compararlo con el modelo de la hoja antes de dar los números por buenos.
