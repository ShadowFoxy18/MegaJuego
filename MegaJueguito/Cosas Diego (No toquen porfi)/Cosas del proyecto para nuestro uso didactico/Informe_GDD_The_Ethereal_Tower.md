# Informe del contenido: GDD "The Ethereal Tower"

## 1. Visión general

- **Género:** juego de gestión de aventura que mezcla roguelike, combate por turnos y gestión de recursos.
- **Plataforma y público:** PC, mayores de 16 años.
- **Premisa:** un grupo de aventureros escala una torre de energía maligna ("Etérea") para salvar su ciudad.
- **Trasfondo:** hace mucho tiempo unos pilares negros arrasaron el mundo y destruyeron ciudades hasta que un grupo de héroes frenó su avance. Hoy quedan pocas civilizaciones, entre ellas la "Ciudad del juego", donde la historia de esos héroes casi se ha olvidado.

---

## 2. Mecánicas

El documento divide el juego en tres bloques.

### 2.1 Gestión de recursos

Se consiguen monedas y objetos en la torre, y en la ciudad se canjean por equipamiento o recursos útiles para la aventura.

### 2.2 Combate por turnos

- El orden de turnos depende de la velocidad de cada personaje, sea jugador o enemigo.
- Hay **9 elementos**: Hielo, Físico, Fuego, Agua, Eléctrico, Naturaleza, Viento, Éter y Lumen. Pueden generar reacciones entre ellos, pero la cantidad de reacciones está en desarrollo.
- Al inicio del turno, el grupo recupera energía.
- Acciones disponibles (5): atacar, aguantar, investigar, consumir objeto y huir.

| Acción | Descripción |
|---|---|
| **Atacar** | Hasta 6 tipos de ataque que consumen energía. Se obtienen con armas o al subir de nivel. |
| **Aguantar** | Acción defensiva: reduce el daño recibido y recupera algo de energía. |
| **Investigar** | Revela información y debilidades del enemigo, más o menos según el personaje. |
| **Consumir objeto** | Usa los objetos que tiene cada personaje. |
| **Huir** | Se nombra, pero no se describe. |

### 2.3 Roguelike

- La torre es el escenario principal y donde ocurre la mayor parte de la trama. El grupo avanza por pisos.
- Hay tres zonas, con nombres pendientes de confirmar:
  - **The Forest:** zona inicial, temática de bosque y naturaleza.
  - **Frozen Plain:** segunda zona, temática nevada.
  - **The Castle:** zona final, temática medieval y de castillo.
- Cada piso incluye: zona de inicio, mercader ambulante, zona de descanso y escalera de subida.
- Al entrar en una nueva zona se generan **4 terrenos de mazmorra de forma aleatoria**, para que cada partida sea distinta.

---

## 3. Personajes (grupo de 3)

| Personaje | Rol y raza | Personalidad | Motivación |
|---|---|---|---|
| **Kael** | Guerrero tabaxi, líder del grupo, armas cortas | Frío, distante, directo | Venganza por la muerte de su esposa aventurera. Su hija espera su regreso. |
| **Sylvaine ("Sylphie")** | Maga élfica, magia y ataques a distancia | Lógica, sensible, fría | Encontrar a su amiga íntima, desaparecida mientras exploraban una torre. |
| **Cassian** | Bardo humano, apoyo, debilitación, rompe la cuarta pared, tiene miedo escénico | Reservado, carismático, entrometido | Su reino fue destruido por las torres. Quiere preservar los cantares y contar la historia. |

---

## 4. Enemigos

- **Comunes:** slimes elementales (con arte de variantes por elemento).
- **The Forest:**
  - Grud: monstruo del bosque.
  - Seta: seta con patas.
  - Troll: montón de rocas que cobran vida.
  - Capa Voladora: mini jefe.
- **Frozen Plain:**
  - Lobos: se vuelven humanoides si se enfadan.
  - Piñas de hielo: lanzan sus semillas.
  - Yeti: mini jefe, lanza las piñas de hielo.
  - Fenrir: un lobo grande.
- **The Castle:** sin enemigos definidos.
- Hay bocetos para la mayoría de los enemigos de bosque y nieve.

---

## 5. NPC

Tabla de tres columnas:

| La Boticaria | El Herrero | (vacía) |
|---|---|---|
| Ilustración de una chica con cuernos de ciervo y ropa verde | Ilustración de un personaje reptiliano rojo forjando | Sin contenido |

No hay texto descriptivo de ninguno.

---

## 6. Sistema de daño

**Fórmula general:**

```
Daño = DañoBase × MultiplicadorReducción × MultiplicadorElemental
```

### Daño base

```
(baseEcalator × AtqMultiplier × MultiplierDamage) + AditiveDamage
```

### Multiplicador de reducción

```
Defensa × Vulnerabilidad × ResistenciasElementales
```

- **Defensa:** depende del nivel del atacante y del defensor, con reducciones de defensa.
- **Vulnerabilidad:** `1 − DefMitigation + AtqVulnerability`
- **Resistencias elementales:** `1 − ElementalResDef + AtqPENRes`

### Multiplicador elemental

```
MaestryMultiplier × (BonoElemental + 1) × (1 + AdditionalBonus)
```

- **Multiplicador de maestría:** `((Maestry × 2) / (Maestry + 1200)) × (1 + Anomaly)`
- **Bono elemental:** fórmula con constantes 1250, 0,6 y 12.

### Tabla de roles

| | Daño base | Multiplicador de reducción | Multiplicador elemental |
|---|---|---|---|
| **Guerrero** | +++ | + | ++ |
| **Mago** | + | ++ | +++ |
| **Bardo** | ++ | +++ | + |

---

## 7. Material gráfico incluido

- Ilustración de portada con los tres protagonistas.
- Chibis y retratos de cada personaje.
- Imagen de ejemplo de mapa de mazmorra.
- Sprites de slimes por elemento.
- Bocetos de enemigos de bosque y nieve.
- Imágenes de los NPC.

---

# Dudas que deja el documento

## Combate

1. ¿Cuánta energía cuesta cada ataque, cuál es el máximo y cuánto se recupera por turno?
2. ¿Qué hace exactamente "Huir"? ¿Tiene probabilidad de fallo o coste?
3. ¿Cuáles son las reacciones elementales y qué hacen Éter y Lumen?
4. ¿Cómo funciona la música de Cassian en combate y qué efecto tiene su miedo escénico?
5. ¿Qué cantidad de información da "Investigar" según cada personaje?
6. ¿Los enemigos tienen debilidades fijas por elemento?

## Roguelike y progresión

7. ¿Qué significa "4 terrenos de la mazmorra"? ¿Salas, mapas o secciones por piso?
8. ¿Qué pasa cuando muere el grupo? ¿Qué se pierde y qué se conserva?
9. ¿Qué tipos de recursos hay y cómo es la economía de monedas, objetos y equipo?
10. ¿Cómo se sube de nivel y hay árbol de habilidades?
11. ¿Cuántos pisos tiene cada zona y qué ofrecen exactamente el mercader y la zona de descanso?
12. ¿Hay jefe final? ¿Cuál es el objetivo último de la torre?

## Contenido pendiente

13. ¿Qué enemigos hay en The Castle?
14. ¿Qué rol cumplen la Boticaria y el Herrero, y quién iría en la tercera columna?
15. ¿Qué es Fenrir: enemigo normal, mini jefe o jefe?
16. ¿Cuál es el nombre definitivo del juego y de la ciudad? Aparecen "Ethereal", "Etheral" y "Etérea", y "Ciudad del juego" es un placeholder.
17. ¿Cuáles serán los nombres finales de las zonas?

## Sistema de daño

18. ¿Qué significan `Anomaly`, `AtqPENRes`, `DefMitigation`, `DefenseDef` y `baseEcalator`?
19. ¿De dónde salen las constantes 1200, 1250, 0,6 y 12?
20. ¿Hay algún ejemplo numérico que valide la fórmula?
21. En la fracción de defensa, ¿`AtqLevel + 20` aparece dos veces a propósito?

## Generales

22. ¿Por qué el público es +16?
23. ¿Quién es el jugador dentro de la historia? No se indica si hay diálogos, decisiones o varios finales.
24. ¿Qué incluirá la primera versión jugable?
25. ¿Las imágenes de los NPC son propias o de referencia externa?
