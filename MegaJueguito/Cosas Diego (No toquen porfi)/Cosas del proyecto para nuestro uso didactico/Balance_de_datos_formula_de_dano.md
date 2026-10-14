# The Ethereal Tower – Balance de datos: fórmula de daño y tabla de roles

> Documento de trabajo. Recoge lo acordado hasta ahora sobre el punto de balance de datos. Lo marcado como **Pendiente** no se da por cerrado hasta confirmarlo.

---

## 1. Fórmula de daño: los tres bloques

```
Daño = DañoBase × MultiplicadorReducción × MultiplicadorElemental
```

### 1.1 Daño Base
**Pregunta que responde:** ¿qué tan fuerte es el golpe en sí, antes de que llegue a nadie?

Depende solo del atacante y de la habilidad concreta que usa. No tiene en cuenta al defensor.

```
(baseEscalator × AtqMultiplier × MultiplierDamage) + DañoAditivo
```

| Término | Origen |
|---|---|
| `baseEscalator`, `AtqMultiplier`, `MultiplierDamage` | Salen del ataque concreto elegido en el turno. No son fijos del personaje. |
| `DañoAditivo` | Extra fijo. Puede venir del arma o de reacciones activas. |

### 1.2 Multiplicador de Reducción
**Pregunta que responde:** ¿cuánto del golpe se bloquea o se absorbe?

Es un cálculo sobre el defensor, con ajustes que aporta el atacante (por ejemplo, la penetración).

```
Defensa × Vulnerabilidad × ResistenciasElementales
```

| Sub-parte | Qué representa | Fórmula |
|---|---|---|
| **Defensa** | Reducción por niveles del atacante y del defensor. | Fracción con `AtqLevel + 20`, `DefLevel + 20`, `DefenseDef` y `DefReduction` |
| **Vulnerabilidad** | Debuffs o buffs que hacen recibir más o menos daño plano. | `1 − DefMitigation + AtqVulnerability` |
| **Resistencias elementales** | Específica del elemento del ataque. | `1 − ElementalResDef + AtqPENRes` |

**Lectura del resultado:**
- Menor que 1: reduce el daño.
- Mayor que 1: lo amplifica (defensor vulnerable).

### 1.3 Multiplicador Elemental
**Pregunta que responde:** ¿cuánto daño extra sale por el dominio del atacante sobre ese elemento?

No es "pegar fuerte", sino "pegar con fuego siendo bueno con el fuego".

```
MaestryMultiplier × (BonoElemental + 1) × (1 + DañoAdicional)
```

- **Multiplicador de maestría:** `((Maestry × 2) / (Maestry + 1200)) × (1 + Anomaly)`. Tiene rendimientos decrecientes por el `/(Maestry + 1200)`: cada punto extra de maestría rinde menos.
- **Bono elemental:** otra curva con sus propias constantes (1250, 0.6 y 12).

### 1.4 Términos definidos

| Término | Significado |
|---|---|
| `Anomaly` | Estadística de Raza y Clase que amplifica el multiplicador de maestría. |
| `AtqPENRes` | Penetración del atacante: reduce el efecto de la resistencia elemental del defensor. |
| `DefMitigation` | Estadística defensiva plana de Raza y Clase del defensor. |
| `DefenseDef` | Valor de Defensa del defensor (Raza × Clase). |

> **Nota sobre valores de la fracción de Defensa:** el `0,1` y la repetición de `AtqLevel + 20` **no son erratas**. Son valores utilizables dentro de la fórmula. Falta documentar en el GDD qué representa cada uno.

---

## 2. Tabla de roles

| | Daño Base | Multiplicador de Reducción | Multiplicador Elemental |
|---|---|---|---|
| **Guerrero** | +++ | + | ++ |
| **Mago** | + | ++ | +++ |
| **Bardo** | ++ | +++ | + |

### 2.1 Cómo se lee la tabla (aclaración clave)

**Los "+" expresan de qué parte de la fórmula sale el daño de cada clase.** No miden cuánto defiende cada clase ni cuánto daño hace en total: es un reparto de fuentes de daño.

| Clase | Fuente principal | Segunda fuente | Tercera fuente |
|---|---|---|---|
| **Guerrero** | Daño Base (+++) | Elemental (++) | Reducción (+) |
| **Mago** | Elemental (+++) | Reducción (++) | Daño Base (+) |
| **Bardo** | Reducción (+++) | Daño Base (++) | Elemental (+) |

### 2.2 Qué implica para cada clase

- **Guerrero:** su daño sale de la fuerza bruta del golpe. Escala con la habilidad usada y el arma (`baseEscalator`, `AtqMultiplier`, `MultiplierDamage`, `DañoAditivo`). Su `++` elemental encaja con el elemento Físico, que es uno de los 9 elementos y por tanto también usa Maestría.
- **Mago:** su daño sale del dominio del elemento (Maestría y Bono Elemental).
- **Bardo:** su aportación sale del bloque de Reducción. No golpea fuerte: hace que los golpes lleguen más fuertes bajando las defensas del enemigo (Vulnerabilidad, Resistencias elementales, penetración `AtqPENRes`). Esto es coherente con su rol de aplicar estados positivos y negativos.

### 2.3 Consecuencias para el Bardo

- Para que el bloque de Reducción **aumente** el daño, el Bardo debe empujarlo por encima de 1: más Vulnerabilidad y menos Resistencia en el enemigo.
- Tener acceso exclusivo a Éter y Lumen es distinto de escalar bien en daño elemental (`+` en Elemental). Su valor está en aplicar estados, que alimentan Vulnerabilidad, Daño Aditivo y las reacciones del grupo.
- Con esta lectura, la tabla ya expresa el rol del Bardo y **no hace falta un cuarto eje** (por ejemplo, "Potencia de estados").

---

## 3. Historial de correcciones en este punto

| Lectura inicial (incorrecta) | Lectura final (correcta) |
|---|---|
| La columna de Reducción medía cuánto defiende cada clase. | Los "+" miden de dónde sale el daño de cada clase. |
| `+++` en Reducción = "defiende mejor" (número más bajo). | `+++` en Reducción = más contribución de ese bloque al daño. |
| Contradicción aparente del Bardo (acceso a Éter/Lumen con `+` elemental). | No hay contradicción: acceso a un elemento no es lo mismo que escalar en él. |

---

## 4. Pendiente de cerrar

1. **Alcance del Bardo:** ¿su reducción de defensas del enemigo beneficia a **todo el grupo** o solo a los ataques del propio Bardo?
2. **Valor numérico de los "+":** asignar cifras a `+`, `++` y `+++` para poder probar la fórmula.
3. **Ejemplo numérico completo** que valide la fórmula, incluida la fracción de Defensa con sus valores `0,1` y `AtqLevel + 20`.
4. **Documentar en el GDD** qué representa cada valor de la fracción de Defensa.
5. **Redactar la tabla de roles** en el GDD explicando que los "+" indican la fuente del daño, para evitar lecturas erróneas.

---

## 5. Enlaces con otros puntos del índice

- **Punto 1 (rol del Bardo):** queda cubierto por la lectura de la tabla (sección 2.3), pendiente de la pregunta sobre alcance.
- **Punto 5 (fórmula de defensa):** parcialmente avanzado; falta el ejemplo numérico.
- **Punto 2 (elementos):** la sección de Elementos deberá aclarar el papel de Físico, Éter y Lumen en la Maestría.
