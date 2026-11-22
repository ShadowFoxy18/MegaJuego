# Elementos y reacciones — MegaJuego

> Documento de diseño vivo. Resume lo definido en la conversación hasta ahora. Los nombres, duraciones o efectos sin valores cerrados se marcan como provisionales o pendientes; no se presentan como balance final.

## 1. Vocabulario del sistema

| Término | Significado |
|---|---|
| **Elemento** | Base temática y mecánica del sistema: Agua, Hielo, Fuego, Electricidad, Naturaleza, Viento, Éter y Lumen. **Físico** existe aparte de estas ramas. |
| **Aplicación elemental** | Marca temporal que un ataque puede dejar en un objetivo para habilitar interacciones posteriores. Que un ataque haga daño de un elemento **no implica automáticamente** que aplique su marca: cada acción debe declarar si aplica elemento. |
| **Anomalía elemental** | Estado o consecuencia producida por una aplicación o interacción elemental. Puede añadir reglas como cargas, duración, daño, control o cambios de estadísticas. |
| **Reacción elemental** | Interacción entre elementos, aplicaciones y estados. Puede cambiar el daño, añadir daño, transformar estados o limpiar efectos. |
| **AV** | Valor de acción usado para ordenar turnos. Un retraso suma AV al valor actual del afectado; retrasa su próxima acción, no se la elimina. |

### Duración de marcas y estados

Como regla de partida, las aplicaciones de Agua, Hielo, Fuego, Electricidad y Naturaleza duran **2 turnos**; las de Éter y Lumen, **3 turnos**. Son duraciones base: una reacción puede sustituirlas o usar una duración propia. Los turnos se cuentan para el personaje afectado y se actualizan al llegar su turno, según las reglas particulares del estado.

### Ramas de diseño

| Rama | Elementos | Función |
|---|---|---|
| **Soporte** | Éter y Lumen | Éter causa daño al cargarse y tiene interacciones versátiles. Lumen es el estado purificado del Éter y resulta especialmente dañino para seres etéreos. |
| **Amplificadores** | Viento y Naturaleza | Mejoran o extienden interacciones ofensivas y defensivas. |
| **Defensivos** | Agua y Hielo | Priorizan prevención, control y limitación del enemigo; su daño directo no es su función principal. |
| **Ofensivos** | Fuego y Electricidad | Fuentes principales de daño dentro de las reacciones. |
| **Aparte de las ramas** | Físico | Tiene acciones y reglas propias; la acción contundente interactúa con Congelado. |

## 2. Fórmulas de daño

Las fórmulas siguientes transcriben las capturas compartidas por el usuario. Son la **fórmula de diseño**; conviene mantenerla diferenciada de lo que esté implementado actualmente en Unity hasta verificar cada término en el código.

### Daño normal

~~~text
Damage = BaseDamage × ResistanceMultiplier × ElementalMultiplier

BaseDamage = (baseEscalator × AtqMultiplier × MultiplierDamage) + AdditiveDamage

ResistanceMultiplier = Defense × Vulnerability × ElementalResistances

Defense =
  (AtqLevel + 20)
  / ((DefLevel + 20) × (0.1 + DefenseDef − DefReduction) × (AtqLevel + 20))

Vulnerability = 1 − DefMitigation + AtqVulnerability

ElementalResistances = 1 − ElementalResDef + AtqPENRes

ElementalMultiplier =
  MaestryMultiplier × (1 + BonoElemental) × (1 + AdditionalBonus)

MaestryMultiplier =
  ((Maestry × 2) / (Maestry + 1200)) × (1 + TasaAnomalia)

BonoElemental =
  (((1 + Maestry) × (1 + ElementalBonus) × 0.6)
  / ((1 + Maestry) + 1250))
  × ((1 + ElementalBonus) × 1.2)
~~~

**Notas de lectura**

- Los porcentajes se pasan como fracciones: 120% = 1,2.
- `baseEscalator` y `AtqMultiplier` pertenecen al ataque concreto; no hay que tratarlos automáticamente como atributos globales del personaje.
- `AdditionalBonus` aparece en la fórmula recibida. Se excluye de los cálculos de balance que acordamos excluirlo.
- Los cálculos neutrales toman `ResistanceMultiplier = 1` y no añaden modificaciones externas de defensa, resistencia, penetración o vulnerabilidad.
- Maestría es una estadística general. Tasa de Anomalía es una estadística nueva relacionada con el daño de reacciones; su uso en otras mecánicas, como la aplicación de marcas, se decidirá por sistema.

### Topes usados en el balance por personaje

Estos valores son los máximos acordados para las comparaciones actuales, no una curva obligatoria ni estadísticas iniciales:

| Rol | BaseDamage de referencia | Maestría máx. | Tasa de Anomalía máx. | ElementalBonus máx. | Bono de crítico máx. | Prob. crítica objetivo |
|---|---:|---:|---:|---:|---:|---:|
| Maga | 450 | 1000 | 120% | 50% | 250% | Cerca del 100% |
| Bardo | 750 | 500 | 90% | 75% | 175% | Cerca del 100% |
| Guerrero | 1250 | 250 | 45% | 20% | 300% | Cerca del 100% |

Los bonos de crítico son el valor adicional sobre el golpe normal. La fórmula de daño de la captura no incluye el promedio de críticos; para las comparaciones anteriores se usó:

```text
Daño esperado = Daño sin crítico × (1 + ProbabilidadCrítica × BonoCrítico)
```

La comparación presupone que BonoCrítico es un bono extra (por ejemplo, 250% = 2,5 adicional) y que el golpe crítico aplica ese bono encima del daño normal. Los valores de BaseDamage son puntos de partida por rol; el `baseEscalator` requerido puede cambiar según el ataque de referencia y no debe confundirse con esos valores.
### Daño propio de reacción

La captura de elementos presenta esta fórmula:

~~~text
ReactionDamage =
  ReactionMultiplier × ElementBonus × LevelMultiplier
  × (1 + (ReactionNumerator × Maestry)
       / (ReactionDenominator + Maestry))
~~~

Cada reacción que inflija daño puede tener sus propios valores editables en Unity:

| Parámetro | Uso |
|---|---|
| `ReactionMultiplier` | Escala base de esa reacción. |
| `ReactionNumerator` | Controla la contribución de Maestría. |
| `ReactionDenominator` | Define la curva con la que crece la contribución de Maestría. |

Los tres valores son específicos de cada reacción. No se han fijado todavía para todas. **Tasa de Anomalía** también deberá incorporarse a las reacciones según el diseño que se cierre; no se debe añadir a la fórmula sin decidir cómo, ni aplicar el escalado de reacción dos veces.

## 3. Clasificación funcional de las reacciones

En el catálogo se documentan cuatro comportamientos. Una reacción puede tener varios a la vez; **Limpiadora** puede acompañar a cualquiera de los otros tres.

| Comportamiento | Qué hace |
|---|---|
| **Aditiva** | Añade un valor al daño base o un bono aditivo a los ataques indicados. |
| **Multiplicativa** | Modifica el daño del golpe relevante que activa, detona o rompe la reacción. El golpe normal no debería recibir el multiplicador por adelantado. |
| **Transformativa** | Cambia el estado del afectado o crea una condición con efectos propios, inmediatos o durante varios turnos. |
| **Limpiadora** | Elimina las aplicaciones, anomalías o problemas de estado especificados. Se registra como comportamiento adicional porque puede combinarse con las otras categorías. |

Los multiplicadores de golpe no son lo mismo que `ReactionMultiplier` de la fórmula de daño propio de reacción. Hay que decidir por efecto si modifica un golpe ya calculado o si genera un componente nuevo, para evitar duplicar daño.

## 4. Catálogo de interacciones acordadas

### Agua

| Interacción | Resultado y efecto conocido |
|---|---|
| **Agua consigo misma** | La aplicación se denomina **Moja**; en documentos anteriores también aparece como Mojado. Es una marca de Agua, no una reacción de daño. |
| **Agua + Hielo** | **Congelado**. Requiere que el objetivo tenga Agua/Mojado. Retrasa su siguiente acción mediante AV. Cuando le llega el turno, pierde Congelado y queda con **Helado** (aplicación de Hielo). |
| **Agua + Fuego** | **Vaporizado**, Multiplicativa **×1,5** al golpe detonador. Limpia las dos aplicaciones. |
| **Agua + Electricidad** | **Sobrecarga**. Mientras dura, el enemigo queda afectado por ambas aplicaciones y se retrasa su siguiente acción mediante AV. La duración final de la reacción y la magnitud del retraso quedan pendientes. |
| **Agua + Naturaleza** | **Florecimiento**. El objetivo conserva ambas aplicaciones; a sus usuarios adyacentes se les aplica Enredado. |
| **Agua + Éter** | **Pompa**. Mientras está en Pompa, el objetivo no tiene Mojado. Puede recibir hasta 3 cargas de Agua. Al explotar, recibe daño considerable y se retrasa su acción; el mismo efecto alcanza a sus adyacentes y aplica Mojado a esos usuarios. Falta fijar valores y duración. |
| **Agua + Lumen** | **Agua Bendita**. Limpia problemas de estado y aplicaciones elementales, y cura según parte de la Maestría del aplicador. Las cargas y la curación se llaman **Santa Cura**. Contra un usuario etéreo limpia sus elementos y le causa daño según Maestría durante los turnos activos. |

### Hielo

| Interacción | Resultado y efecto conocido |
|---|---|
| **Hielo consigo mismo** | Aplicación **Helado**. |
| **Hielo + Fuego** | **Derretido**, Multiplicativa **×2,5** al golpe que detona la reacción. |
| **Hielo + Electricidad** | **Superconductor**. Aplica Electricidad a los adyacentes, aumenta el daño Físico y Eléctrico que reciben los afectados y retrasa su siguiente acción mediante AV. La frase posterior “reduce la PEN a Res Física y Eléctrica” necesita traducirse a la estadística exacta del juego, porque puede describir una reducción de Resistencias o de Penetración a Resistencias, que no producen el mismo resultado. |
| **Hielo + Naturaleza** | **Escarcha**. Cuando el usuario afectado actúa, aparece un Pétalo Escarchado; cada pétalo causa daño de Hielo al aparecer. Al llegar a 3 cargas, se produce una explosión que daña al objetivo y a sus adyacentes. |
| **Hielo + Éter** | **Granizo Etéreo**. Afecta a todos los usuarios del campo, reduce su Velocidad, les causa algo de daño durante varios turnos y aplica Helado. Falta precisar duración, daño y fuerza de la ralentización. |

### Fuego

| Interacción | Resultado y efecto conocido |
|---|---|
| **Fuego consigo mismo** | Marca de Fuego; el golpe no aplica la marca automáticamente salvo que la acción lo indique. El nombre final de la marca no está fijado. |
| **Fuego + Electricidad** | **Plasma**, Multiplicativa **×1,8** al golpe detonador. Afecta al objetivo y a sus adyacentes, y limpia ambas aplicaciones. |
| **Fuego + Naturaleza** | **Calcinado**. Retrasa la siguiente acción. El daño durante el tiempo transcurrido hasta su turno se calcula a partir de `AV pasado × X% de Maestría`; falta definir X y la conversión a daño. |
| **Fuego + Éter** | **Fuego Kármico**. Llama azul inextinguible que inflige mucho daño de Fuego durante **3 turnos**. No se puede limpiar: termina cuando se agota su duración. |
| **Fuego + Lumen** | **Fuego Fatuo**. Llama azul clara que cura problemas de estado y potencia los movimientos de Fuego hacia otros. Quien intente golpear al afectado recibe aplicación de Fuego y una carga etérea. Si se activa sobre un ser etéreo, se consume automáticamente y causa daño masivo repartido entre los usuarios presentes del equipo. Falta definir el reparto y los valores. |

### Electricidad

| Interacción | Resultado y efecto conocido |
|---|---|
| **Electricidad consigo misma** | Marca de Electricidad; el golpe no garantiza su aplicación. Nombre final de la marca pendiente. |
| **Electricidad + Naturaleza** | **Intensificado**. El aplicador obtiene un buff de Velocidad y los siguientes ataques reciben un bono aditivo. No modifica directamente el `baseAV`; el cálculo de la iniciativa/AV debe considerar los buffs y debuffs de Velocidad activos. Falta definir duración y magnitudes. |
| **Electricidad + Éter** | Reacción provisionalmente llamada **Descarga Etérea**. Golpea al usuario afectado con daño Eléctrico durante varios turnos y cada golpe aplica Electricidad. Nombre y duración aún pueden cambiar. |

### Físico

| Interacción | Resultado y efecto conocido |
|---|---|
| **Acción Física contundente + Congelado** | **Quebrahielo**, Multiplicativa **×1,3** del daño original del golpe contundente que rompe Congelado. Afecta al golpe rompedor; no es una aplicación elemental. |
### Naturaleza, Viento, Éter y Lumen

| Interacción | Resultado y efecto conocido |
|---|---|
| **Naturaleza + Viento** | **No reaccionan entre sí**. Pueden coexistir en el mismo objetivo junto con Éter. La propuesta anterior “Esporas Errantes” queda descartada como reacción vigente. |
| **Enredado + Corrupción/Éter** | **Enredo Corrupto**. Reduce Ataque y Defensa y vuelve al afectado vulnerable a Fuego y Naturaleza. Esas vulnerabilidades no eliminan Enredo Corrupto. |
| **Enredo Corrupto + Hielo** | Aumenta el daño de los Pétalos Escarchados. Falta definir cuánto y cómo se acumula. |
| **Enredo Corrupto + Electricidad** | Paraliza al afectado, retrasa su siguiente acción mediante AV y aumenta la probabilidad crítica del atacante que provocó la reacción. |
| **Enredo Corrupto + Agua** | Aplica Enredo Corrupto a todos los enemigos. |
| **Lumen + Agua** | Agua Bendita / Santa Cura, según se describe en Agua. |
| **Lumen + Fuego** | Fuego Fatuo. |
| **Lumen + Naturaleza / Éter** | El usuario indicó que Lumen puede interactuar con estos elementos; los efectos concretos de esas dos combinaciones no están suficientemente especificados. Mantenerlas como combinaciones reconocidas, pendientes de ficha. |
| **Lumen aplicado** | Aplica **Purificación** durante 3 turnos. El usuario purificado no puede recibir otros elementos o reacciones salvo Agua y Fuego, con la interacción especial de Corrupción descrita más abajo. **Purificación**, **Agua Bendita** y **Santa Cura** son conceptos distintos. |
| **Purificación + Corrupción** | Si el purificado recibe Corrupción, recibe daño según el tiempo que llevaba Purificado: se propuso **2,5 × turnos en Purificación**. La unidad exacta y si consume Purificación necesitan confirmación. |

## 5. Viento: secuencia y huracanes

La secuencia acordada depende del orden. Viento no aplica directamente un Huracán:

1. Si el objetivo no tiene una marca elemental, una **Anomalía de Viento** le aplica **Vórtice**.
2. Si Vórtice recibe Corrupción, se transforma en **Vórtice Etéreo**.
3. Si Vórtice o Vórtice Etéreo recibe Agua, Hielo, Fuego o Electricidad, se crea el Huracán del elemento correspondiente.
4. Cada tipo de Huracán reduce la resistencia del elemento que representa y puede aplicar su elemento a quienes alcanza.

| Variante | Instancias | Alcance |
|---|---:|---|
| Huracán normal | 3 impactos | Cono: objetivo y usuarios adyacentes. Si hay menos blancos cercanos que impactos, puede rebotar para completar las instancias. |
| Huracán Etéreo | 5 impactos | Puede alcanzar a todos los usuarios del equipo objetivo, cuyo máximo previsto es 5. |
| Huracán Etéreo adicional | 2 impactos adicionales al 50% del daño normal | No se vuelve a replicar. |

Los Huracanes impregnan con su elemento a los usuarios alcanzados. Por eso pueden causar reacciones nuevas durante la misma acción. Se registra cada reacción por impacto y por objetivo. Ejemplo acordado: un Huracán Etéreo de Fuego que alcanza a un usuario con marca de Agua provoca Vaporizado en ese usuario —y el golpe de ese Huracán recibe el aumento correspondiente—; después crea un Huracán Etéreo de Agua. Las dos instancias adicionales al 50% nunca inician otra replicación.

Vórtice se ha documentado con **2 turnos** y Vórtice Etéreo con **3** en el esquema actual. Conviene tratarlas como duraciones provisionales hasta que se confirmen expresamente.

## 6. Éter: cargas y Corrupción

- La Anomalía Etérea genera **cargas de Corrupción Etérea**.
- Cada carga aumenta el daño recibido por el afectado; el porcentaje crece según las cargas activas y la Maestría del aplicador. La fórmula y los porcentajes siguen abiertos.
- Las cargas tienen duración. Una nueva Anomalía Etérea renueva sus turnos.
- Cuando el objetivo ya tiene 4 cargas, la siguiente aplicación (la quinta) activa **Corrupción** durante 3 turnos.
- Corrupción reduce las resistencias elementales del afectado. Hay que cerrar si se refiere a bajar la resistencia elemental del objetivo o a modificar el atributo llamado `PEN a Res` en la fórmula, ya que son operaciones distintas.
- Éter, Viento y Naturaleza pueden coexistir. La interacción Éter+Viento que transforma Vórtice en Vórtice Etéreo no impide que Naturaleza siga presente.

## 7. Elementos, aplicaciones y estados nombrados

| Elemento/estado | Nombre usado | Notas |
|---|---|---|
| Agua | Moja / Mojado | Marca de Agua. La interfaz puede mostrar “Moja” al consultar Agua consigo misma. |
| Hielo | Helado | Aplicación de Hielo; también es lo que queda al terminar Congelado. |
| Fuego | Marca de Fuego | Marca elemental simple; nombre final pendiente. |
| Electricidad | Marca de Electricidad | Marca elemental simple; nombre final pendiente. |
| Naturaleza | Enredado | Estado/aplicación vinculado a control y a su evolución Enredo Corrupto. |
| Viento | Vórtice | Anomalía que inicia la secuencia de Huracanes. |
| Éter | Cargas de Corrupción Etérea / Corrupción | Las cargas preceden a Corrupción; no son sinónimos. |
| Lumen | Purificación | Estado de Lumen durante 3 turnos. No es Agua Bendita ni Santa Cura. |
| Agua + Lumen | Agua Bendita / Santa Cura | Agua Bendita es la reacción; Santa Cura nombra sus cargas y curación. |
| Físico contundente | Quebrahielo | Acción contundente que rompe Congelado. |

## 8. Paleta de elementos y reacciones

La paleta se guía por las capturas que compartió el usuario. Los colores de abajo son una **aproximación pastel para organizar la ficha y el juego**, no códigos finales aprobados. Huracanes usan el color del elemento correspondiente en su versión pastel.

| Elemento | Color guía | Hex provisional |
|---|---|---|
| Físico | Gris acero | `#AEB7C2` |
| Agua | Turquesa/cian | `#55DCE5` |
| Hielo | Azul lavanda | `#8797F7` |
| Fuego | Coral rojo | `#FF8585` |
| Electricidad | Amarillo cálido | `#FFD966` |
| Naturaleza | Verde claro | `#8DE49A` |
| Viento | Cian verdoso | `#79D8D2` |
| Éter | Violeta | `#963DD1` |
| Corrupción | Morado profundo | `#6E168F` |
| Lumen | Rosa/lila luminoso | `#EBA2FF` |

### Convención visual sugerida

- Mantener el color del elemento en las etiquetas y los iconos de sus aplicaciones.
- Las reacciones de dos elementos pueden usar ambos colores en el icono o una mezcla suave; no hace falta inventar un color nuevo para cada combinación.
- Estados como Corrupción o Purificación pueden tener un color propio, distinto de la marca elemental que los activó.
- Para Huracanes, usar siempre el pastel del elemento infundido, como pidió el usuario.
- Las fichas de información pueden conservar fondo neutro claro y usar color en el nombre/tipo, siguiendo las capturas.

## 9. Notas para programarlo en Unity

1. **Separar conceptos:** elemento del ataque, marca aplicada, anomalía/reacción activa y efecto temporal son datos distintos.
2. **La aplicación debe ser explícita:** una acción informa si aplica una marca; el cálculo de daño elemental no la añade por defecto.
3. **Resolver por fases:** capturar estados anteriores al golpe, detectar reacciones, calcular daño, aplicar limpieza/estados/AV y luego procesar eventos secundarios.
4. **Registrar cada impacto y objetivo:** los Huracanes generan varias instancias; una reacción en un objetivo modifica ese impacto y no el daño de las otras instancias contra otros objetivos.
5. **Usar cola de eventos determinista:** cada evento derivado guarda su origen y evento padre. Limitar replicaciones; los dos Huracanes adicionales al 50% no se replican.
6. **Duraciones y cargas por entidad:** guardar aplicador, objetivo, turnos restantes, cargas, origen y reglas de renovación.
7. **AV y Velocidad:** el retraso suma al AV actual y no elimina acciones. Los buffs de Velocidad no deben editar directamente `baseAV`; el cálculo de la siguiente iniciativa debe consultar modificadores activos.
8. **Datos editables:** valores, duraciones, alcances, cargas, número de impactos y funciones deben configurarse desde datos de Unity (por ejemplo, `ScriptableObject`) para equilibrarlos sin rehacer el resolutor.
9. **Daño sin doble aplicación:** guardar por separado daño normal, bono multiplicativo al golpe detonador y daño propio de reacción. `ReactionMultiplier` de fórmula y multiplicador de golpe son campos diferentes.
10. **Pruebas de orden:** probar marca previa, reacciones encadenadas, limpieza, rebotes con 1–5 objetivos y turnos justo antes/después de expirar estados.

### Registro orientativo de evento

~~~text
CombatEvent
  sourceActorId
  targetActorId
  actionId
  incomingElement
  appliesElement
  stateSnapshotBefore[]
  reactionsTriggered[]
  damageComponents
  statesAppliedOrRemoved[]
  avChange
  parentEventId
  chainDepth
  canReplicate
~~~

Es un esquema conceptual para facilitar la implementación; los nombres de clases/campos pueden adaptarse al proyecto.

## 10. Pendiente de especificar

- Los tres valores de `ReactionDamage` (`ReactionMultiplier`, `ReactionNumerator`, `ReactionDenominator`) para cada reacción que cause daño.
- Cómo incorpora Tasa de Anomalía la fórmula de daño propio de reacción.
- Multiplicadores y componente de daño final de Vaporizado, Derretido, Plasma y Quebrahielo en relación con la fórmula común; los factores de golpe actuales son ×1,5, ×2,5, ×1,8 y ×1,3 respectivamente.
- Valores de las cargas etéreas, duración/renovación exacta y fórmula de Corrupción.
- Qué atributo reduce Corrupción: Resistencia Elemental o `PEN a Res`.
- Duración/magnitud de retrasos de Sobrecarga, Congelado, Superconductor, Calcinado y Enredo Corrupto.
- Reglas detalladas de las interacciones Lumen+Naturaleza y Lumen+Éter, manteniendo clara la protección de Purificación.
- Valores de daño, objetivos y duración de Pompa, Granizo Etéreo, Escarcha, Fuego Kármico, Fuego Fatuo, Descarga Etérea y Huracanes.
- Si las marcas base de Fuego y Electricidad reciben nombres propios.
- Los hex definitivos de la paleta y el efecto audiovisual por reacción.

## 11. Decisiones anteriores que ya no son vigentes

- La infografía antigua que presentaba Aditiva, Multiplicativa, Transformativa y Limpiadora como cuatro tipos exclusivos no es la fuente de las definiciones actuales. Se conservan cuatro comportamientos para catalogar; Limpiadora se puede combinar con otro tipo.
- La propuesta “Esporas Errantes” para Naturaleza + Viento queda descartada: la regla más reciente dice que Naturaleza y Viento no reaccionan y pueden coexistir.
- Los nombres de fichas V1 como Bloom, Quemado, Torbellino, Raíz Corrupta o la variante previa de Fuego Fatuo no sustituyen automáticamente las reglas nuevas de este documento.


