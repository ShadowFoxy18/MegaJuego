# GDD – The Ethereal Tower

## 1. Visión general

- **Género:** gestión de aventura, con combate por turnos y progresión roguelike.
- **Plataforma y público:** PC, mayores de 16 años.
- **Premisa:** un grupo de aventureros escala una torre de energía maligna ("Etérea") para salvar su ciudad.
- **Trasfondo:** hace mucho tiempo, unos pilares negros sacudieron el mundo, dominándolo y sucumbiendo ciudades, hasta que un grupo de héroes frenó su avance. En la actualidad solo quedan pocas civilizaciones que luchan contra la corrupción Etérea, entre ellas "Ciudad del juego" (nombre provisional), donde la historia de esos héroes casi se ha olvidado.

---

## 2. Mecánicas

### 2.1 Gestión de recursos

Se consiguen monedas y objetos en la torre; en la ciudad se canjean por equipamiento o recursos útiles para la aventura.

### 2.2 Combate por turnos

- El orden de turnos depende de la velocidad de cada personaje (jugador o enemigo), por un sistema de barra de acción tipo *Honkai Star Rail*: cada personaje avanza hacia su turno a un ritmo proporcional a su velocidad.
- **9 elementos:** Hielo, Físico, Fuego, Agua, Eléctrico, Naturaleza, Viento, Éter y Lumen. Generan reacciones entre ellos (ver sección 2.2.1).
- Al inicio del turno, el grupo recupera energía física y/o etérea (energía física: se gasta al atacar y se recupera con el tiempo; energía etérea: se gasta al lanzar hechizos, y sin recuperarla no se pueden seguir lanzando).
- **5 acciones por turno:** Atacar, Aguantar, Investigar, Consumir objeto, Huir.

| Acción | Descripción |
|---|---|
| **Atacar** | Cada personaje cuenta con **6 ranuras de ataque activas** en combate. Puede desbloquear más ataques de los que caben en esas 6 ranuras —obtenidos al equipar ciertas armas o al subir de nivel—, y el jugador elige libremente cuáles de sus ataques desbloqueados asignar a las 6 ranuras activas fuera de combate (inspirado en el sistema de *Expedition 33*: un catálogo amplio de movimientos aprendidos, de los que se selecciona un subconjunto activo). |
| **Aguantar** | Acción defensiva: reduce el daño recibido (~50%) y recupera algo de energía física/etérea. |
| **Investigar** | Revela información del enemigo (debilidades, puntos débiles); la cantidad de información depende del personaje que investiga. Puede que no consuma turno (pendiente de definir). |
| **Consumir objeto** | Permite usar objetos personales de cada personaje, distribuidos entre la party. |
| **Huir** | Si un personaje elige huir, pierde su turno; cuando les toca a los demás, también omiten su turno. Cuando todos los turnos de la party se han omitido, el grupo consigue escapar del combate. |

#### 2.2.1 Reacciones elementales (diseño preliminar)

El sistema de reacciones ya tiene un diseño preliminar extenso, con reacciones de un solo elemento y de combinación de dos, cada una con tipo (Transformativa / Multiplicativa / Aditiva / Limpiadora), duración en turnos, y en varios casos una fórmula propia. Ejemplos ya definidos:

- **Hielo:** Helado (reduce velocidad), Congelado (Hielo+Agua: aplica Helado+Mojado, pierde turno, vulnerable a físico/fuego), Derretido (Fuego+Hielo: multiplicador de daño), Quebracizo (Hielo+Físico: daño base extra, reduce defensa).
- **Agua:** Mojado, Vaporizado (Fuego+Agua: multiplicador de daño), Sobrecargado (Electricidad+Agua: potencia Electrocutado).
- **Fuego:** Quemado (daño en el tiempo según velocidad), Plasma (Fuego+Electricidad: daño en área a adyacentes).
- **Eléctrico:** Electrocutado (daño en el tiempo).
- **Naturaleza:** Enredo (reduce daño enemigo según maestría, vulnerable a Quemado), Intensificado (Naturaleza+Eléctrico o +Agua: potencia reacciones futuras).
- **Viento:** Torbellino (replica el elemento del enemigo 3 veces a él y adyacentes).
- **Éter / Lumen:** asociados a temas de corrupción y purificación — *Raíz Corrupta* y *Huracán Etéreo* (Éter+Viento: debuffs de ataque/defensa, no removibles con medios normales), *Fuego Fatuo* (Éter+Fuego: daño espiritual en el tiempo), *Corrupción* (vulnerabilidad acumulativa), *Agua Sanadora/Santa Cura* (Éter o Lumen+Agua: cura a aliados heridos con el tiempo), *Lumen/Purificación* (limpia Corrupción y varias reacciones negativas, inflige daño según turnos acumulados).

*(Diseño de reacciones en desarrollo; la cantidad final de reacciones y el comportamiento exacto de Éter/Lumen sigue abierto — ver preguntas pendientes.)*

### 2.3 Roguelike

- La torre es el escenario principal, donde el grupo avanza por pisos.
- Tres zonas (nombres provisionales): **The Forest** (bosque, inicial), **Frozen Plain** (nevada, intermedia), **The Castle** (medieval/castillo, final).
- Cada piso incluye: zona de inicio, mercader ambulante, zona de descanso, escalera de subida.
- Al entrar en una nueva zona se generan 4 terrenos de mazmorra de forma aleatoria, para variar la experiencia entre partidas.
- **Recurso de rescate:** la party posee un objeto de rescate (no se compra) que, si el grupo entero llega a 0, lo activa y evita la derrota total. Se recarga al volver a la ciudad antes de subir de nuevo a la torre — mecánica pendiente de desarrollo narrativo y de reglas (p. ej. qué penalización conlleva usarlo).

---

## 3. Personajes (grupo de 3)

| Personaje | Rol, raza y clase | Personalidad | Motivación |
|---|---|---|---|
| **Kael** | Guerrero, tabaxi (**No-Etéreo**), líder del grupo, armas cortas | Frío, distante, directo | Venganza por la muerte de su esposa aventurera. Su hija espera su regreso. |
| **Sylvaine ("Sylphie")** | Maga, élfica (**No-Etérea**), magia y ataques a distancia | Lógica, sensible, fría | Encontrar a su amiga íntima, desaparecida mientras exploraban una torre. |
| **Cassian** | Bardo, humano (**No-Etéreo**), apoyo y debilitación, rompe la cuarta pared, miedo escénico | Reservado, carismático, entrometido | Su reino fue destruido por las torres. Quiere preservar los cantares y contar la historia. |

**Regla de raza confirmada:** toda raza jugable es No-Etérea por diseño; toda raza enemiga/monstruo nace del Éter y es Etérea (ver sección 4).

**Nota sobre clases:** la clase "Bardo" sustituye a la clase originalmente llamada "Clérigo" en el diseño de sistema (mismas aptitudes: único tipo de personaje con acceso ofensivo a Éter y Lumen). El Clérigo como nombre de clase queda disponible para NPCs o enemigos si se necesita en el futuro.

---

## 4. Enemigos

Todas las razas enemigas son **Etéreas** (nacidas de la corrupción de los pilares), por la misma regla de raza indicada arriba.

- **Comunes:** slimes elementales (variante visual por elemento).
- **The Forest:** Grud (monstruo de bosque), Seta (seta con patas), Troll (rocas animadas), Capa Voladora (mini jefe).
- **Frozen Plain:** Lobos (se vuelven humanoides al enfadarse), Piñas de hielo (lanzan semillas), Yeti (mini jefe, lanza piñas de hielo), Fenrir (lobo grande — rol exacto, por confirmar).
- **The Castle:** sin enemigos definidos todavía.

---

## 5. NPC

| La Boticaria | El Herrero | *(pendiente)* |
|---|---|---|
| Rol narrativo y de juego sin definir | Rol narrativo y de juego sin definir | Sin contenido |

---

## 6. Sistema de daño

**Fórmula general:**
```
Daño = DañoBase × MultiplicadorReducción × MultiplicadorElemental
```

**Daño base:**
```
(baseEscalator × AtqMultiplier × MultiplierDamage) + DañoAditivo
```

**Multiplicador de reducción:**
```
Defensa × Vulnerabilidad × ResistenciasElementales
```
- Defensa: depende del nivel del atacante y del defensor, con reducciones de defensa.
- Vulnerabilidad: `1 − DefMitigation + AtqVulnerability`
- Resistencias elementales: `1 − ElementalResDef + AtqPENRes`

**Multiplicador elemental:**
```
MaestryMultiplier × (BonoElemental + 1) × (1 + DañoAdicional)
```
- Multiplicador de maestría: `((Maestry × 2) / (Maestry + 1200)) × (1 + Anomaly)`
- Bono elemental: fórmula con constantes 1250, 0.6 y 12 (origen de las constantes, pendiente).

**Tabla de roles:**

| | Daño base | Multiplicador de reducción | Multiplicador elemental |
|---|---|---|---|
| **Guerrero** | +++ | + | ++ |
| **Mago** | + | ++ | +++ |
| **Bardo** | ++ | +++ | + |

---

## 7. Respuestas a las preguntas pendientes (según lo hablado hasta ahora)

Numeración según la lista original de dudas.

**2. ¿Qué hace "Huir"?** Resuelto: el personaje que huye pierde su turno; cuando les toca a los demás, también omiten el suyo. Al omitirse los turnos de toda la party, el grupo escapa del combate. Queda abierto si tiene probabilidad de fallo o coste adicional.

**3. ¿Reacciones elementales, y qué hacen Éter/Lumen?** Parcialmente resuelto: hay un diseño preliminar extenso de reacciones (ver sección 2.2.1). Éter se asocia temáticamente a corrupción/debuffs persistentes; Lumen a purificación/curación. La lista final y el balance exacto siguen en desarrollo.

**8. ¿Qué pasa cuando muere el grupo?** Parcialmente resuelto: existe un objeto de rescate (no comprable) que evita la derrota total si el grupo llega a 0; se recarga al volver a la ciudad. Falta definir la penalización asociada y si hay un límite de usos por expedición.

**18. ¿Qué significan los términos de la fórmula?** Resuelto a nivel mecánico (pendiente de justificación narrativa/de diseño):
   - `baseEscalator`, `AtqMultiplier`, `MultiplierDamage`: provienen de la habilidad/ataque concreto usado en el turno (varían según qué ataque se elige, no son fijos del personaje).
   - `Anomaly`: estadística asociada a Raza y Clase que amplifica el multiplicador de maestría.
   - `AtqPENRes` (penetración): estadística del atacante que reduce el efecto de la resistencia elemental del defensor.
   - `DefMitigation`: estadística defensiva plana de Raza y Clase del defensor.
   - `DefenseDef`: el valor de Defensa del defensor (Raza × Clase).

**21. ¿`AtqLevel + 20` aparece dos veces a propósito?** Sin resolver — se implementó tal como aparece en la fórmula original, pero no se confirmó si es intencional o un error de tipeo del diseño base. Pendiente de revisión.

El resto de las preguntas de la lista original (1, 4, 5, 6, 7, 9–17, 19, 20, 22–25) siguen abiertas, sin definición adicional por ahora.

---

## 8. Preguntas nuevas que surgieron en esta revisión

- ¿Es una convención fija que **toda** raza jugable futura sea No-Etérea y **toda** raza enemiga sea Etérea, o puede haber excepciones más adelante?
- Los 6 ataques del sistema de combate, ¿se obtienen únicamente por arma equipada y nivel, o también por otras fuentes (objetos, eventos de historia)?
- El objeto de rescate: ¿tiene un límite de usos por expedición a la torre, o se puede usar una vez por combate sin restricción mientras no se haya "recargado"?
