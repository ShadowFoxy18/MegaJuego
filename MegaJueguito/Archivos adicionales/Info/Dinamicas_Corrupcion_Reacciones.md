# Dinámicas de Corrupción con elementos y reacciones

> Documento de análisis del sistema actual. Separa las reglas indicadas por el usuario, las reglas registradas en el catálogo y los riesgos o propuestas que todavía requieren decisión. No fija valores de balance pendientes.

## 1. Reglas de partida

### Confirmado en conversación

- Corrupción puede actuar sobre los elementos y los estados de reacción que ya tenga el usuario afectado.
- El Éter amplifica los estados de reacción activos. La magnitud y el atributo concreto que amplifica cada estado aún no se han fijado.
- Corrupción tiene un máximo de 4 cargas. Mientras estén activas, cada golpe recibido añade daño fijo según las cargas multiplicado por X % de Maestría. Las cargas se pierden al terminar 2 turnos del portador.
- Al aplicar Corrupción a un usuario con Sobrecarga, el efecto de Sobrecarga se limpia y se resuelven las dos interacciones de Éter que corresponden a sus elementos: Agua produce Prisión Etérea y Electricidad produce Descarga Etérea.
- Viento y Naturaleza no tienen reacción directa. Si Corrupción se aplica a un usuario con Vórtice y Enredado, ambos estados reciben la influencia de Éter por separado: Vórtice pasa a Vórtice Etéreo y Enredado a Enredo Corrupto; pueden coexistir.
- Algunas reacciones son estados autónomos que actúan periódicamente y no son eliminados por otros estados. Granizo Etéreo es el ejemplo aclarado por el usuario.

### Registrado en el catálogo de diseño

- Agua + Éter: Prisión Etérea; hasta 3 cargas de Agua, explosión, daño y retraso, con Mojado en adyacentes.
- Hielo + Éter: Granizo Etéreo; efecto periódico independiente, ralentiza, daña y aplica Helado.
- Fuego + Éter: Fuego Kármico; daño de Fuego durante 3 turnos, no limpiable.
- Electricidad + Éter: Descarga Etérea; daño periódico y cada golpe aplica Electricidad.
- Enredado + Éter/Corrupción: Enredo Corrupto; reduce Ataque y Defensa, y expone a Fuego y Naturaleza.
- Vórtice + Éter/Corrupción: Vórtice Etéreo y la secuencia de Huracanes.
- Lumen + Corrupción: se propuso daño según los turnos de Purificación; falta confirmar unidad, consumo y prioridad.

## 2. Principio para resolver interacciones

Corrupción no debe borrar automáticamente toda reacción activa. El resultado depende de la naturaleza del efecto:

1. **Reacción de usuario que se transforma expresamente:** Sobrecarga es la excepción acordada. Corrupción la termina y deriva Agua a Prisión Etérea y Electricidad a Descarga Etérea.
2. **Estado periódico autónomo:** Granizo Etéreo conserva su reloj y sigue actuando aunque el usuario reciba otro estado. Fuego Kármico también se ha descrito como no limpiable.
3. **Reacción activa que puede amplificarse:** Corrupción añade el modificador de Éter según la regla que se defina; no vuelve a crear la reacción ni acumula su modificador cada vez que se procesa el mismo evento.
4. **Aplicación elemental disponible:** si una marca sigue activa y no ha sido consumida, puede participar en una reacción de Éter. La ficha debe indicar cuándo se consume, se conserva o se convierte.

En una acción con varias marcas, conviene tomar una instantánea de estados antes del golpe. Así, el orden de lectura no decide accidentalmente si el mismo par de elementos dispara una o dos reacciones.

## 3. Matriz de Corrupción por elemento

| Estado del usuario al aplicar Corrupción | Resultado/interacción | Qué permanece o cambia | Riesgo principal |
|---|---|---|---|
| **Agua / Mojado** | **Prisión Etérea** | Sustituye Mojado en el portador mientras dura; admite hasta 3 cargas de Agua y luego explota. | La explosión aplica Mojado a adyacentes, que pueden iniciar reacciones nuevas. Limitar la cadena a una resolución por objetivo e impacto. |
| **Agua + Electricidad en Sobrecarga** | **Prisión Etérea + Descarga Etérea** | Sobrecarga se limpia. Agua deriva a Prisión Etérea y Electricidad a Descarga Etérea en el mismo evento. | Descarga reaplica Electricidad periódicamente; Prisión debe impedir Mojado en el portador para que no rehaga Sobrecarga en cada tick. |
| **Hielo / Helado** | **Granizo Etéreo** | Crear un efecto periódico independiente del usuario. Continúa hasta su condición propia de fin; los estados del objetivo no lo rompen. | Si cada tick vuelve a aplicar Helado y hay Agua, puede volver a activar Congelado repetidamente. Separar el daño periódico de la aplicación de Helado, o limitar esta última. |
| **Fuego / marca de Fuego** | **Fuego Kármico** | Daño de Fuego durante 3 turnos; el catálogo indica que no se limpia. | Si cada tick cuenta como nueva aplicación de Fuego, podría detonar Vaporizado, Plasma u otras reacciones en cada turno. Definir si los ticks solo dañan o también aplican marca. |
| **Electricidad / marca de Electricidad** | **Descarga Etérea** | Daño periódico; cada golpe aplica Electricidad. | Cada aplicación puede combinarse con Agua, Hielo, Fuego o Naturaleza. Procesar como máximo una reacción hija por tick y objetivo. |
| **Naturaleza / Enredado** | **Enredo Corrupto** | Reduce Ataque y Defensa y crea vulnerabilidad a Fuego y Naturaleza. | La vulnerabilidad puede aumentar daño muchas veces; re-aplicar Naturaleza no debe volver a acumular reducción o vulnerabilidad sin límite. |
| **Enredo Corrupto + Agua** | Extensión de Enredo Corrupto a todos los enemigos | La propagación se resuelve como un único efecto de área. | La copia no debe disparar otra propagación global; marcar como descendiente del evento original. |
| **Enredo Corrupto + Hielo** | Aumenta el daño de Pétalos Escarchados | Interacción amplificadora de Escarcha. | Escarcha tiene 3 pétalos: no sumar una amplificación nueva por cada impacto o por cada copia del estado. |
| **Enredo Corrupto + Electricidad** | Parálisis, retraso AV y aumento de probabilidad crítica del atacante | Modifica al afectado y al atacante. | Re-aplicar Electricidad por golpes múltiples puede acumular retraso y crítico indefinidamente. Limitarlo por objetivo y activación de Enredo Corrupto. |
| **Viento / Vórtice** | **Vórtice Etéreo** | Corrupción aplicada a un usuario con Vórtice lo transforma. Si también tiene Enredado, este se transforma por separado en Enredo Corrupto. Viento y Naturaleza no reaccionan directamente. | La misma transformación no debe reiniciarse cada vez que se refresque el estado. Los Huracanes derivados del Etéreo son normales y no generan más Huracanes. |
| **Lumen / Purificación** | Daño ligado al tiempo en Purificación (valor propuesto: 2,5 × turnos) | Corrupción interactúa con Purificación; el catálogo todavía no aclara si la consume. | Si la duración se amplifica antes de calcular el daño, el resultado puede crecer; decidir el orden y si el estado se consume. |
| **Físico** | No hay una reacción Físico + Corrupción registrada | Quebrahielo sigue siendo la interacción del golpe Físico contundente con Congelado. | No inventar una reacción. Corrupción puede coexistir con efectos físicos salvo que una regla expresa los transforme. |
| **Éter / cargas etéreas** | Cargas hasta el máximo de 4; el catálogo propone que la aplicación siguiente active Corrupción | Daño aditivo por golpe recibido: daño fijo según cargas × X % de Maestría. | El daño adicional no debe volver a contar como un golpe que activa su propio daño; aclarar si la quinta aplicación consume/resetear cargas. |

## 4. Interacciones con reacciones que ya están activas

### Sobrecarga

Regla explícita: Corrupción limpia Sobrecarga y produce Prisión Etérea y Descarga Etérea a partir de Agua y Electricidad. La resolución debe ocurrir una sola vez por aplicación de Corrupción. Descarga puede mantener su periodicidad; su Electricidad no reactiva inmediatamente Sobrecarga mientras Prisión Etérea impida Mojado en el portador.

### Granizo Etéreo y otros efectos periódicos

Granizo Etéreo vive como efecto de reacción periódico, separado del estado que tenga el objetivo. Aplicar Corrupción puede amplificar su efecto si sigue activo, pero no lo elimina ni reinicia su reloj. Debe definirse si la amplificación afecta daño, ralentización, objetivos o duración; cambiar varios a la vez multiplica el riesgo de exceso.

Fuego Kármico y Descarga Etérea también son efectos temporales con golpes periódicos. El tick de daño debe ser distinto del evento que aplica una marca. En Descarga Etérea, si el tick aplica Electricidad, esta marca solo puede iniciar la reacción siguiente una vez para ese tick.

### Estados de Naturaleza y Vórtice

- Naturaleza + Viento no tiene reacción directa; Enredado y Vórtice pueden coexistir sin reaccionar entre sí.
- Si Corrupción se aplica cuando coexisten Enredado y Vórtice, los dos estados se transforman por separado en Enredo Corrupto y Vórtice Etéreo.
- Un Huracán infundido con otro elemento puede resolver interacciones independientes con las marcas de Naturaleza y Viento/Vórtice del objetivo.
- **Florecimiento (Agua + Naturaleza)** aplica Enredado a los usuarios adyacentes al objetivo. Esa extensión de Enredado pertenece a Florecimiento.
- **Enredo Corrupto + Agua** es otra interacción distinta: extiende Enredo Corrupto a todos los enemigos.
- La reacción **Viento + Naturaleza** existe, pero su nombre y mecánica siguen pendientes. No se debe asumir que comparte con Florecimiento la extensión de Enredado ni que propaga Naturaleza.

### Secuencia corregida de Huracanes

- El Huracán elemental normal reparte el elemento correspondiente en **3 instancias** entre los usuarios alcanzados y causa daño durante esos impactos. No es un aplicador persistente que mantenga aplicaciones en turnos posteriores.
- El Huracán elemental Etéreo reparte su elemento en **5 instancias**. Si una instancia alcanza a un usuario que ya tiene otro elemento, se resuelve la reacción entre ambos elementos y se crean **3 instancias de un Huracán normal del elemento que ya tenía ese usuario**.
- Si el usuario alcanzado tiene marcas de Naturaleza y Viento/Vórtice, el elemento del Huracán puede resolver una interacción con cada una por separado; esto no constituye una reacción directa Naturaleza + Viento.
- Esos Huracanes normales derivados no generan instancias de Huracán adicionales. Por tanto, esta secuencia tiene una profundidad fija: Huracán Etéreo, reacción en el usuario alcanzado y Huracán normal derivado. No existe el bucle recursivo descrito anteriormente.
- El número total de reacciones e impactos derivados aún depende de cuántas de las 5 instancias encuentran usuarios con otra marca elemental y de cómo se reparten las 3 instancias normales; eso puede aumentar el daño de una resolución, pero no prolongar indefinidamente la cadena.

### Reacciones de golpe y estados consumidos

Vaporizado (×1,5) y Plasma (×1,8) limpian aplicaciones; Quebrahielo (×1,3) rompe Congelado. Derretido es ×2,5, pero su regla de consumo debe especificarse. Agua Bendita limpia estados y aplicaciones. Estos consumos pueden cortar una cadena, siempre que se resuelvan de forma determinista.

## 5. Riesgos de ciclos y sobreexplotación

| Cadena potencial | Por qué puede crecer | Límite recomendado para análisis |
|---|---|---|
| **Huracán Etéreo → interacciones por marca → Huracán normal derivado** | Cada impacto puede encontrar marcas distintas en el mismo objetivo y resolver interacciones independientes con Naturaleza y Viento/Vórtice. | No es un bucle: los Huracanes normales derivados no generan más Huracanes. Falta fijar la prioridad si varias interacciones compiten por el mismo impacto o consumen una marca común. |
| **Florecimiento y Enredo Corrupto + Agua** | Florecimiento extiende Enredado a adyacentes; Enredo Corrupto + Agua extiende Enredo Corrupto a todos los enemigos. Son dos efectos distintos. | No encadenar copias de una misma propagación sin una regla expresa. Viento + Naturaleza no tiene propagación definida. |
| **Granizo Etéreo → Helado + Agua → Congelado → nuevo retraso** | El efecto periódico puede volver a congelar al mismo usuario cada turno. | Separar el tick de daño de la aplicación de Helado, o fijar un límite de activaciones de Congelado por Granizo y objetivo. El estado de Granizo no se elimina. |
| **Descarga Etérea → Electricidad periódica → otras reacciones → nueva aplicación** | Cada tick de Descarga aplica una marca que puede detonar otra reacción. | Máximo de una reacción hija por tick y objetivo; esa hija no vuelve a crear otro tick de Descarga en la misma resolución. |
| **Fuego Fatuo → ataque del enemigo → Fuego + carga de Éter** | Ataques múltiples o repetidos pueden aplicar varias cargas por una sola acción recibida. | Definir si se activa una vez por acción, por atacante o por impacto. No activar represalias por impactos hijos. |
| **Enredo Corrupto + Electricidad repetido** | La re-aplicación puede acumular parálisis, retraso AV y probabilidad crítica. | Una activación por aplicación válida; reemplazar/refrescar duración en vez de sumar potencia sin tope. |
| **Éter amplifica una reacción ya amplificada** | Si se modifica el valor modificado, cada aplicación produce crecimiento compuesto. | Guardar el valor base y calcular una sola amplificación desde él; no usar el resultado anterior como nueva base. |
| **Multiplicadores en un mismo impacto** | Una marca puede cumplir varias condiciones antes de que otra reacción la consuma. | Definir prioridad o permitir explícitamente co-reacciones. No multiplicar dos factores por orden accidental. |

## 6. Cálculos que sí pueden acotarse

Multiplicadores registrados para golpes:

| Reacción | Factor |
|---|---:|
| Vaporizado | ×1,5 |
| Derretido | ×2,5 |
| Plasma | ×1,8 |
| Quebrahielo | ×1,3 |

Si un mismo golpe de Fuego pudiera detonar a la vez Vaporizado y Derretido, el resultado hipotético sería ×1,5 × ×2,5 = **×3,75**. Si también detonara Plasma, sería ×1,5 × ×2,5 × ×1,8 = **×6,75**. Estos no son resultados aprobados: las reacciones de limpieza y el orden de consumo podrían impedir que coexistan en un mismo impacto. El catálogo debe escoger una regla: prioridad de una reacción por golpe, o co-reacciones explícitas con factores compuestos.

El daño numérico completo no se puede cerrar aquí: faltan los valores de varias reacciones, la regla de amplificación de Éter y la frecuencia de impactos/ticks. Además, el objetivo de daño de progresión del Excel y el límite de 1000 mencionado en conversación siguen sin reconciliarse.

## 7. Relación con las colas de acciones

La arquitectura propuesta distingue acciones normales en la cola de AV, adicionales sin prioridad en una cola FIFO y adicionales con prioridad que interrumpen y se resuelven inmediatamente. Los detalles y las decisiones todavía abiertas están en [Modelo de acciones y colas de combate](Modelo_Acciones_y_Colas_de_Combate.md).

Esta distinción ayuda a analizar las reacciones sin confundir un efecto que modifica el golpe actual con otro golpe separado. Granizo Etéreo mantiene un reloj autónomo: al llegar su tick genera una resolución, pero la cola de acciones no convierte Granizo en un estado del usuario ni permite que otro estado lo elimine.

## 8. Reglas de resolución recomendadas

1. Cada golpe, tick, explosión o aplicación recibe un identificador de evento y conserva su evento padre.
2. Cada reacción se activa como máximo una vez por combinación de evento, objetivo y estado de origen, salvo las co-reacciones expresamente aprobadas.
3. Las marcas y estados se leen de una instantánea previa; después se consumen, conservan o transforman según la ficha de la reacción.
4. Los ticks periódicos se ejecutan aunque el usuario tenga otros estados. Solo se interrumpen por la condición propia de terminación de la reacción; Granizo Etéreo no se limpia mediante otro estado.
5. Las aplicaciones derivadas de un efecto en área pueden detonar reacciones en cada objetivo, pero no reinician la propagación del efecto raíz.
6. La amplificación de Éter se calcula desde la potencia base del estado, una vez por activación; no se acumula recursivamente.
7. Los modificadores AV y los bonos a Ataque, Defensa, crítico y daño necesitan límites, duración y regla de renovación antes de fijar números.

## 9. Decisiones pendientes

- Qué atributo aumenta Éter en cada estado activo: daño, potencia, duración, AV, cargas, objetivos o impactos.
- Si una aplicación de Corrupción amplifica estados que ya existían antes del evento o también los que acaba de crear.
- Si Granizo Etéreo aplica Helado al iniciar o en cada tick; su ciclo autónomo no se rompe por estados del usuario.
- Si los ticks de Fuego Kármico y Descarga Etérea vuelven a aplicar sus elementos y pueden iniciar reacciones.
- Cuándo se consumen/reinician las marcas en Derretido, Superconductor, Prisión Etérea, Intensificado y demás reacciones sin regla de consumo cerrada.
- Cómo se resuelven varias reacciones válidas para un mismo impacto.
- Reparto de las 5 instancias etéreas y de las 3 instancias del Huracán normal derivado cuando varios objetivos tienen marcas compatibles.
- Prioridad y consumo de marcas si un impacto de Huracán activa interacciones distintas en el mismo objetivo.
- Valores de Corrupción, Maestría X y reglas de la quinta aplicación tras cuatro cargas.
- Objetivo de daño por golpe y DPS temporal usado para el balance final.
