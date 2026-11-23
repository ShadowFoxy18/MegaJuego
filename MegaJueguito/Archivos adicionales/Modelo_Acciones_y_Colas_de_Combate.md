# Modelo de acciones y colas de combate

> Propuesta de arquitectura basada en la regla indicada por el usuario. Describe el orden de resolución; no modifica los scripts ni decide los efectos pendientes de las reacciones.

## 1. Referencia: Honkai: Star Rail

La referencia útil es separar el orden normal por **AV** de las acciones que se ejecutan fuera de ese orden. En Star Rail, la acción normal se ordena por Valor de Acción; adelantar una acción modifica su posición en ese orden, mientras que un turno extra o un ataque de seguimiento son mecanismos adicionales y no equivalen simplemente a reducir el AV.

Para MegaJuego, la adaptación propuesta conserva esa separación y añade la distinción propia del proyecto: acciones adicionales **sin prioridad** y **con prioridad**. No se pretende copiar el comportamiento interno exacto de Star Rail.

Referencias de consulta:

- [HoYoWiki oficial: ejemplo de habilidad de enemigo que concede un turno extra inmediato](https://wiki.hoyolab.com/pc/hsr/entry/4842?lang=en-us).
- [Guía de AV y acciones de Star Rail en HoYoLAB](https://www.hoyolab.com/article/19822028). Es una guía comunitaria alojada en HoYoLAB, no documentación oficial del motor.

## 2. Clases de acción

### Acción normal

- Pertenece a un combatiente.
- Espera en la cola principal ordenada por AV.
- Cuando llega su turno, el jugador o la IA elige una acción normal y esta se resuelve.
- Al terminar, se programa la siguiente acción normal de ese combatiente según su AV y Velocidad.

### Acción adicional sin prioridad

- La crea una acción, reacción o efecto periódico.
- Se añade al final de una cola secundaria FIFO, respetando el orden de creación.
- No interrumpe la acción que está resolviéndose.
- La cola secundaria se procesa antes de elegir al siguiente combatiente de la cola normal.
- Si una adicional sin prioridad genera otra adicional sin prioridad, la nueva se añade al final. No salta a las que ya estaban esperando.

### Acción adicional con prioridad

- Se resuelve inmediatamente cuando se genera, interrumpiendo el flujo que estaba activo.
- Al terminar, se retoma la acción suspendida en el punto donde quedó.
- Si varias prioridades nacen del mismo evento, se resuelven en orden determinista de creación, salvo que una regla de prioridad indique otro orden.
- Si una acción adicional sin prioridad genera una acción con prioridad, esta se resuelve antes de continuar con la siguiente entrada FIFO.

## 3. Orden de resolución

1. La cola principal selecciona al combatiente con el AV más bajo.
2. Se resuelve su acción normal hasta completar el impacto o punto de interrupción.
3. Las acciones adicionales con prioridad generadas por ese evento se resuelven inmediatamente.
4. Se reanuda y completa la acción normal; se atienden las prioridades que genere.
5. Se vacía la cola FIFO de acciones adicionales sin prioridad. Cada una puede generar prioridades inmediatas.
6. Cuando ambas colas adicionales quedan vacías, se selecciona la siguiente acción normal por AV.

Esquema:

~~~text
Cola normal por AV
  -> acción normal
       -> adicional con prioridad (inmediata; suspende y luego reanuda)
       -> adicionales sin prioridad (FIFO, antes del siguiente AV)
  -> siguiente acción normal por AV
~~~

## 4. Ejemplos del sistema

### Huracán Etéreo

- La acción que lo provoca resuelve el impacto/reacción correspondiente.
- Si esa reacción crea un Huracán elemental normal derivado, se registra como una acción adicional según la prioridad que se decida para el Huracán.
- Esa adicional contiene sus 3 instancias. Los Huracanes normales derivados no crean más instancias de Huracán.
- Hace falta decidir si esas instancias son tres impactos de una sola acción adicional o tres entradas separadas. Se recomienda una sola acción con tres impactos para conservar juntas la fuente, el objetivo, el daño y los límites de replicación.

### Granizo Etéreo

- Granizo mantiene su propio reloj periódico y es un efecto autónomo; ningún estado aplicado al usuario lo rompe.
- Cuando llega su tick, crea una resolución de efecto periódico. Si el tick debe esperar al flujo de combate, puede entrar en la cola adicional sin prioridad.
- El reloj/efecto de Granizo no se convierte en el turno normal del usuario afectado ni reinicia el AV del usuario.
- Falta definir si su reloj avanza por AV global, por turnos de combatientes o por otra unidad. Esa decisión controla cuándo se genera el tick, no si un estado puede cancelar Granizo.

### Reacción multiplicativa

Vaporizado modifica el impacto que la detona; no necesita crear otra acción adicional por ese solo hecho. Si una reacción crea un golpe separado, ese golpe sí se modela como acción adicional.

## 5. Invariantes recomendadas

- Una acción adicional no modifica ni reinicia el AV normal de su fuente, salvo una regla explícita.
- Una acción adicional no reduce duraciones expresadas en turnos normales, salvo que su ficha lo indique. Esto evita que una cadena de extras haga expirar todos los estados antes del siguiente actor.
- La prioridad se define como orden de ejecución; no implica por sí sola daño mayor, más impactos ni una categoría de reacción nueva.
- Cada acción adicional guarda origen, objetivo, causa, elemento, prioridad, impactos restantes y evento padre.
- Las acciones de área especifican si cada objetivo produce su propia reacción y si esa reacción puede crear adicionales.
- Los efectos autónomos tienen un ciclo de vida y reloj separados del estado del usuario.
- Para impedir una cascada no intencionada, cada acción derivada conserva el evento padre y declara qué tipos de nuevas acciones puede generar. Esto respeta la regla ya establecida de que Huracanes normales derivados no generan Huracanes nuevos.

## 6. Decisiones necesarias antes de implementarlo

1. ¿La cola de adicionales sin prioridad se vacía completamente antes del siguiente AV? Este documento adopta esa regla como recomendación.
2. ¿Las prioridades se resuelven en FIFO de prioridad o una prioridad nueva interrumpe también a otra prioridad ya activa? Recomendación: terminar el impacto atómico actual y luego procesar la nueva prioridad antes de reanudar la acción suspendida.
3. ¿Qué prioridad tienen ataques de seguimiento, contraataques, explosiones, ticks y Huracanes derivados?
4. ¿Una adicional ya creada se cancela si su fuente u objetivo cae antes de resolverse? Los efectos autónomos, como Granizo Etéreo, no deben cancelarse por perder un usuario de origen.
5. ¿Los impactos múltiples son una entrada de cola con varios impactos o varias entradas independientes?
6. ¿Qué fases cuentan como un turno para la duración de estados y la reducción de cargas: solo acciones normales, también adicionales, o relojes propios?
7. ¿Cómo desempata la cola normal a dos combatientes con el mismo AV?

## 7. Encaje con los scripts existentes

El código actual ya calcula una cola principal por AV en `TurnQueue`, pero `CombatDirector.ProcessTurn` solo pide y registra la decisión. Aún no ejecuta acciones, no existe una cola adicional y no hay resolutor de reacciones. Conviene definir las clases y reglas de arriba antes de conectar daño y estados para que las acciones adicionales no queden mezcladas con el AV normal.

La cola principal deberá producir una acción normal; un resolutor ejecutará sus fases y drenará la cola de prioridad y la cola FIFO en los puntos definidos. Los efectos periódicos tendrán un programador propio que, al llegar su tick, generará una resolución autónoma o una entrada secundaria, según la regla de prioridad del efecto.
