# Preguntas y puntos pendientes — Aetherion: The Ethereal Tower

**Estado:** documento de trabajo. Las preguntas de abajo no cambian decisiones ya acordadas ni convierten las propuestas en requisitos. “Por comprobar” significa que hace falta verlo en el juego o contrastarlo con datos; no implica automáticamente que esté mal.

## 1. Lo que ya tiene una base sólida

- **El trío tiene motivos narrativos propios.** Kael, Sylvaine y Cassian quieren subir a la torre por razones distintas. ¿Cómo influye cada motivo en lo que el personaje puede hacer o en las decisiones de la partida?
- **El mundo y los elementos comparten una idea central.** El Éter, la Corrupción y Lumen ayudan a que el sistema elemental pertenezca al mundo de Aetherion. ¿Qué parte de esa historia descubrirá el jugador mientras juega?
- **La documentación identifica estados de decisión.** Mantener las etiquetas *Acordado*, *Provisional*, *Pendiente* e *Histórico* ayuda a no confundir una propuesta con una regla final.
- **El modelo de combate ya contempla orden por AV y agentes de turno.** La cola y la interfaz `ITurnAgent` son una base para ordenar combatientes y separar quién decide una acción.
- **Las reglas de reacción buscan evitar bucles.** La instantánea de estados, el límite de una reacción por evento y la profundidad fija de Huracanes son decisiones preventivas valiosas. ¿Se mantienen igual en la implementación?
- **El inventario compartido puede generar decisiones antes del combate.** Preparar objetos para cada personaje tiene potencial táctico si el coste y las restricciones son claros.
- **Hay infraestructura reutilizable.** La localización y la generación de mazmorras son piezas útiles, siempre que apoyen el combate y el recorrido que se valide primero.

Estos puntos se consideran una base de diseño, no una confirmación de que ya hayan sido probados con jugadores.

## 2. Experiencia principal y bucle de partida

- ¿Cómo describimos el gancho del juego en una sola frase que explique qué decisión o sensación lo diferencia de otros RPG por turnos?
- Usando MDA, ¿qué experiencias queremos priorizar: fantasía, narrativa, reto, descubrimiento u otras? ¿Cuáles son las dos o tres principales?
- ¿Qué decisión interesante debe repetirse en cada combate y en cada piso?
- ¿Cuál es el bucle completo: explorar → combatir → recibir una recompensa → prepararse → elegir cómo avanzar?
- ¿Cuántos pisos y combates hay en una carrera? ¿Qué puede variar entre carreras y qué contenido está diseñado a mano?
- ¿Qué ocurre al morir? ¿Qué se pierde, qué se conserva y qué permite volver a empezar con ganas?
- ¿Puede el jugador retirarse o regresar? Si cada piso tiene descanso y mercader, ¿qué riesgo queda al continuar?
- ¿Qué se gana al superar un combate y qué decisiones permite tomar esa recompensa?
- ¿Cómo termina una zona y qué razón da al jugador para querer entrar en la siguiente?

## 3. Combate: acciones, AV y recursos

- ¿Qué significa exactamente cada acción para el jugador: **Atacar**, **Aguantar**, **Investigar**, **Consumir objeto** y **Huir**?
- ¿Aguantar reduce cuánto daño, durante cuánto tiempo y cuánta energía recupera? ¿En qué situación resulta mejor que atacar?
- ¿Investigar consume un turno o AV? ¿Revela debilidades, estados o la reacción que causaría una combinación? ¿Qué gana el jugador al usarla?
- ¿Qué tipos de energía hay, cómo se obtienen, en qué se gastan y cómo se recuperan?
- ¿Qué unidad de tiempo usa el juego? ¿Cómo se traducen velocidad, AV y las acciones adicionales en turnos comprensibles?
- ¿Cómo se ordenan acciones con el mismo AV? ¿Qué pasa si una acción adicional interrumpe a otra, si el objetivo muere o si el personaje queda incapacitado?
- ¿Cuánto debería durar un combate normal y cuántas decisiones debería tomar el jugador antes de resolverlo?
- ¿Qué información ve el jugador antes de confirmar una acción: objetivo, daño estimado, gasto de energía, estado aplicado y posible reacción?

## 4. Elementos, reacciones y Corrupción

- ¿Qué dos o tres reacciones son imprescindibles para el primer prototipo? ¿Qué aporta cada una que las otras no puedan hacer?
- ¿Cómo se muestran las marcas, las duraciones y las combinaciones posibles sin exigir que el jugador memorice una tabla extensa?
- ¿En qué momento del turno disminuye la duración de un estado: antes o después de actuar, y cómo afecta a una reacción que se activa en ese mismo golpe?
- Si un impacto puede activar varias reacciones, ¿cuál se resuelve primero y por qué?
- ¿Qué valores de reacción están acordados y cuáles siguen provisionales: multiplicadores, daño propio, duración, alcance y límites?
- ¿Cómo se aplica Corrupción: cargas, umbral, daño, duración, renovación y limpieza? ¿Qué riesgo introduce para quien la usa y qué recompensa ofrece?
- ¿Qué atributo reduce Corrupción: resistencia elemental, penetración u otro? ¿La misma regla se utiliza en el documento, la hoja de cálculo y Unity?
- ¿Qué sucede con Lumen y Purificación cuando se aplican otros elementos? ¿Cómo se comunica el bloqueo al jugador?
- ¿Qué significa Físico dentro del sistema: una marca, un tipo de daño sin marca o una mecánica propia?

## 5. Identidad del trío, progresión y experiencia por sesión

- ¿Qué puede hacer cada personaje que ningún otro pueda sustituir fácilmente?
- ¿Cómo se expresa en juego la función de Kael como guerrero de daño directo si actúa menos que la maga y el bardo?
- ¿Qué decisión táctica propia tendrá Físico? ¿Golpe lento y pesado, romper estados, aprovechar Aguantar u otra opción? ¿Qué riesgo acompaña a su recompensa?
- ¿Los tres deberían alcanzar el mismo daño por acción o el mismo aporte total con patrones distintos? ¿Qué queremos que sea útil en cada rol?
- Para cada sesión o tramo, ¿qué mecánica aprende el jugador, qué contenido ve, qué logra completar y cuánto dura?
- ¿Qué progresión se obtiene entre combates y entre carreras? ¿Las recompensas fortalecen al personaje, amplían opciones o revelan contenido?
- ¿Cómo evita el juego que la dificultad sea una subida lineal sin descansos, descubrimientos o momentos de dominio?
- ¿Qué ayuda se ofrece si el jugador no entiende una reacción o pierde varias veces? ¿Cómo se puede reintentar rápido?
- ¿Qué debería descubrir un jugador nuevo durante los primeros minutos, antes de leer explicaciones largas?

## 6. Economía, inventario, historia y presentación

- ¿Cuál es la moneda y qué compra el jugador que le resulte útil o deseable?
- ¿Qué vende el mercader, qué servicios ofrece el herrero y cómo cambia su inventario entre pisos o carreras?
- ¿Cuántos objetos puede preparar cada personaje? ¿Usarlos consume una acción, AV o energía?
- ¿Qué pasa con los objetos preparados si el grupo huye, pierde o termina el combate?
- ¿Qué partes de la historia se cuentan durante la partida y cuáles pueden saltarse o consultarse después?
- ¿Cómo se enseñan estados y reacciones con señales visuales, animación, sonido y texto consistentes?
- ¿Qué formato final tendrán las claves de diálogo, hablantes, retratos, secuencias y movimiento durante conversaciones?
- ¿Qué localizaciones estarán listas para la primera versión y qué texto de prueba permite comprobar que el sistema está conectado?

## 7. Fórmulas y datos por comprobar

Antes de ajustar el balance, contrastar la misma acción en la fórmula escrita, la hoja de cálculo y el juego:

- ¿La constante del bono elemental es **1,2** o **12**? El GDD y los anexos no coinciden.
- ¿La fórmula de defensa pretende que el nivel del atacante aparezca en numerador y denominador? En el `DamageManager` actual esos términos se cancelan.
- ¿Qué representa el valor neutral de resistencia? En `Character` se inicializa en 1, mientras `DamageManager` calcula `1 - resistencia`; hay que confirmar que un valor por defecto no anule el daño.
- ¿El multiplicador elemental se aplica en el daño? `GetElementalFactor` existe en `DamageManager`, pero no participa en `CalculateDamage`.
- ¿Cómo se suman daño aditivo, vulnerabilidad, defensa, resistencias y daño de reacción? Asegurar que las mismas reglas se usan en GDD, Excel y código.
- ¿Los objetivos de daño por fase se han medido en Unity o son valores teóricos? ¿Incluyen reacciones, estados, mejoras de equipo y oleadas?
- ¿Las resistencias configuradas en los assets usan el mismo convenio que la fórmula?

## 8. Implementación y pruebas de experiencia

- ¿El turno del jugador ya puede elegir una acción y un objetivo desde una interfaz?
- ¿`CombatDirector` ejecuta la acción, aplica daño y estados, y avanza el combate, o todavía registra la decisión en consola?
- ¿Qué responsabilidad tendrá `ActionMoment` y qué datos necesita `SkillData` para resolver una habilidad?
- ¿Existe ya un agente de jugador además del agente básico de enemigos? ¿Cómo se devuelve el control a la interfaz tras una acción adicional?
- ¿Se puede completar un combate de principio a fin, incluida victoria, derrota, huida y recompensa?
- ¿La escena de combate y su flujo de entrada/salida están incluidos en la configuración de compilación prevista?
- ¿Cuánto tarda el primer combate y dónde duda un jugador que no conoce el proyecto?
- ¿Qué errores aparecen al repetir turnos, derrotar a todos los enemigos, retirar una unidad o empezar otra oleada?

**Observación del código revisado:** `CombatDirector.ProcessTurn()` obtiene una decisión del agente y la registra, pero todavía no resuelve sus efectos; `ActionMoment` y `SkillData` están vacíos. La cola de AV ya es una base concreta. Hay que volver a comprobar estos estados al integrar cambios posteriores.

## 9. GDD y organización del trabajo

- ¿Cuál archivo es la fuente de verdad: el GDD o los anexos? ¿Quién mantiene la versión consolidada?
- ¿Qué secciones del GDD siguen desactualizadas: nombre del juego, número de elementos, reacciones, duraciones, fórmula, índice y NPC?
- ¿Cada decisión pendiente tiene responsable, fecha y estado?
- ¿Qué debe resolverse antes de añadir más reacciones, economía o diálogos?
- ¿Qué criterio hará que el combate mínimo se considere suficientemente claro y entretenido para ampliarlo?

## 10. Orden de trabajo sugerido

1. Acordar el gancho, la experiencia MDA prioritaria y el bucle de una carrera.
2. Cerrar energía, acciones y consecuencias básicas de ganar, perder, huir y descansar.
3. Construir un combate mínimo: el trío, un enemigo, una interfaz funcional y dos o tres reacciones.
4. Verificar las fórmulas y sus valores neutrales en el código y en el juego.
5. Probar la sesión inicial con jugadores nuevos; registrar duración, confusiones y acciones elegidas.
6. Ajustar los roles y el ritmo con datos de juego.
7. Integrar gradualmente pisos, recompensas, más reacciones, economía y diálogos.
8. Consolidar los acuerdos y las pruebas en el GDD para que los anexos no diverjan.
