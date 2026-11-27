# Ficha de elementos — diseño y actualizaciones

> Documento complementario de `Reacciones_elementales_MegaJuego.md`. Describe cómo se organiza la ficha de consulta de elementos y qué ajustes de diseño se han acordado en la conversación. Es una especificación de interfaz; no afirma que todos estos componentes estén implementados en el proyecto Unity.

## 1. Objetivo de la ficha

La ficha permite consultar las reglas elementales sin tener que recorrer una tabla enorme. Su contenido cambia según se seleccione:

- **Un elemento:** muestra su identidad, su aplicación/anomalía propia y, si el usuario lo solicita, un resumen de cómo interactúa con los demás.
- **Dos elementos:** muestra la reacción correspondiente, si existe, y resume sus efectos.
- **El mismo elemento dos veces:** muestra la aplicación de ese elemento. Ejemplo: **Agua + Agua → Moja**.

La vista también debe ayudar a distinguir elemento, aplicación elemental, anomalía y reacción. No debe asumir que un ataque aplica su elemento solo por causar daño de ese tipo.

## 2. Estructura de consulta

### Vista de selección

1. Mostrar los elementos como botones o fichas seleccionables.
2. Permitir seleccionar uno o dos elementos.
3. Resaltar los elementos seleccionados con su color de identidad.
4. Actualizar la ficha de información al cambiar la selección.
5. Si la combinación no produce una reacción, indicarlo claramente; no mostrar una reacción inventada.

### Vista de un solo elemento

La ficha muestra:

- Nombre y color del elemento.
- Rama/función: Soporte, Amplificador, Defensivo, Ofensivo o Físico aparte.
- Aplicación o anomalía propia, si está definida.
- Duración base conocida.
- Resumen breve de su mecánica individual.

Se añade un botón conmutador, por ejemplo **“Ver interacciones”**. Al activarlo, la ficha incorpora un resumen de las interacciones de ese elemento con los demás. Al desactivarlo, vuelve a mostrar solo la información propia del elemento.

Este resumen es especialmente útil para **Éter, Viento y Lumen**, porque sus mecánicas individuales y condiciones previas afectan a la lectura de sus combinaciones.

### Vista de dos elementos

La ficha prioriza la información útil para combate:

1. Nombre de la reacción o de la aplicación resultante.
2. Tipo(s) de comportamiento: Aditiva, Multiplicativa y/o Transformativa; además, indicador Limpiadora cuando corresponda.
3. Condición de activación: marcas, estados, cargas u orden necesario.
4. Efecto principal y objetivos afectados.
5. Duración, cargas, AV y multiplicador, solo cuando estén definidos.
6. Nota de versión cuando el dato sea provisional.

Si no hay una reacción entre los elementos, la ficha informa **“Sin reacción directa”**. Si pueden coexistir o interactúan a través de una condición previa, se especifica en lugar de tratarlos como una reacción inmediata.

## 3. Jerarquía visual de la tarjeta

Una composición recomendada para cada ficha:

- **Cabecera:** nombre de elemento o reacción, con el color correspondiente.
- **Etiquetas pequeñas:** aplicación, anomalía, tipo de reacción y versión/provisionalidad.
- **Descripción corta:** una frase que resuma la regla principal.
- **Efectos:** lista de daño, control, limpieza, cambios de estadística, aplicación y AV.
- **Condiciones:** requisitos previos y orden de activación.
- **Duración y números:** en un bloque separado para no confundirlos con la descripción narrativa.
- **Resumen expandible:** solo en la vista de un elemento, para consultar sus interacciones con los demás.

Se recomienda conservar un fondo claro y neutro, como en las capturas compartidas, y usar los colores en el título, los indicadores y las etiquetas. Así se mantiene legible el texto y el color no carga toda la tarjeta.

## 4. Colores acordados para guiar el diseño

Los colores se inspiraron en las capturas y se propusieron en tonos pastel para la interfaz. Los hexadecimales son referencias provisionales, no una paleta final aprobada.

| Elemento/estado | Color guía | Hex provisional |
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

### Reglas de aplicación del color

- Las reacciones deben conservar el vínculo visual con sus elementos de origen.
- Las reacciones de dos elementos pueden mostrar ambos colores en sus etiquetas o iconos.
- Los estados con identidad propia —por ejemplo, Corrupción y Purificación— pueden usar un color propio.
- Los Huracanes usan el color del elemento infundido en versión pastel. Así, un Huracán de Fuego se reconoce como Fuego sin dejar de distinguirse del color intenso del elemento.
- No se asignan colores finales a cada reacción hasta cerrar la paleta completa.

## 5. Actualizaciones incorporadas a la ficha

### A. Selección y resumen de interacciones

- La herramienta de consulta parte de una ventana con los elementos seleccionables.
- Se puede elegir una pareja para ver su reacción.
- En la vista de un solo elemento se incorpora un botón para desplegar u ocultar el resumen de interacciones con los demás.
- Al elegir el mismo elemento dos veces, se muestra la aplicación propia en vez de una reacción entre elementos distintos. El ejemplo definido es **Agua + Agua → Moja**.
- Éter, Viento y Lumen deben mostrar primero su mecánica individual; después, mediante el resumen, sus interacciones.

### B. Claridad de nombres y conceptos

La ficha separa términos que antes podían confundirse:

- **Elemento:** Agua, Hielo, Fuego, Electricidad, Naturaleza, Viento, Éter, Lumen; Físico queda aparte.
- **Aplicación elemental:** marca temporal que prepara interacciones futuras.
- **Anomalía elemental:** estado o consecuencia con reglas propias.
- **Reacción:** resultado de combinar elementos, marcas o condiciones.
- **Purificación:** estado aplicado por Lumen.
- **Agua Bendita:** reacción de Agua con Lumen.
- **Santa Cura:** cargas y curación asociadas a Agua Bendita.

Así, Purificación no se utiliza como sinónimo de Agua Bendita ni de Santa Cura.

### C. Colores y Huracanes

- Se propusieron colores por elemento guiados por las capturas.
- Para los Huracanes se prefirió usar los propios colores de los elementos en estilo pastel.
- Los colores deben reforzar la lectura, sin sustituir el texto de la reacción ni comunicar valores de daño que todavía no se han definido.

### D. Estados de versión

La ficha debe mostrar qué información está confirmada y cuál sigue abierta. Por ejemplo, puede usar etiquetas:

- **Acordado:** regla explícita definida por el usuario.
- **Provisional:** regla usada como referencia, todavía susceptible de cambios.
- **Pendiente:** falta nombre, duración, objetivo, fórmula o magnitud.
- **V1 histórica:** dato de una captura antigua que no reemplaza una regla posterior.

Esto es importante porque las fichas visuales iniciales eran V1 y algunas reacciones cambiaron desde entonces.

## 6. Contenido de referencia para la vista individual

| Elemento | Qué debe destacar la ficha |
|---|---|
| **Agua** | Aplicación llamada Moja/Mojado; control, marcas persistentes en algunas reacciones y sin asumir daño alto. |
| **Hielo** | Aplicación Helado; puede culminar en Congelado, Escarcha o interacciones de control. |
| **Fuego** | Marca de Fuego; es una fuente principal de daño en reacciones. |
| **Electricidad** | Marca de Electricidad; se combina con Fuego, Agua, Hielo, Naturaleza y Éter según sus reglas. |
| **Naturaleza** | Función amplificadora; Enredado puede evolucionar a Enredo Corrupto. Naturaleza + Viento tiene una reacción; su nombre, condición y efecto están pendientes de registrar. |
| **Viento** | Sin marca elemental previa, su anomalía genera Vórtice; luego puede pasar a Vórtice Etéreo y a Huracanes. |
| **Éter** | Soporte que genera cargas etéreas; la quinta aplicación tras cuatro cargas activa Corrupción. Puede coexistir con Naturaleza y Viento. |
| **Lumen** | Soporte y estado purificado del Éter; aplica Purificación. Su ficha debe separar sus interacciones con Agua/Fuego de las otras interacciones aún por especificar. |
| **Físico** | Rama aparte; destacar acciones contundentes y su interacción Quebrahielo con Congelado. |

## 7. Resumen de interacción: criterios de presentación

Cuando el botón **“Ver interacciones”** está activo:

- Mostrar solo los pares relevantes para el elemento seleccionado.
- Separar reacción inmediata de interacción condicionada por un estado o por el orden de aplicación.
- Indicar si la combinación consume, mantiene o aplica marcas.
- Mostrar objetivos afectados: objetivo principal, adyacentes o todos los usuarios del campo.
- Indicar explícitamente los retrasos de acción como cambios de AV, sin sugerir que se pierde el turno.
- Advertir si los números son provisionales.
- Para Naturaleza + Viento, mostrar que existe una reacción y marcar como pendientes su nombre, condición y efecto.
- Para Éter y Viento, mostrar la secuencia que transforma Vórtice en Vórtice Etéreo antes de crear Huracanes.
- Para Lumen, mantener separadas Agua Bendita, Fuego Fatuo y Purificación.

## 8. Modelo de datos recomendado para Unity

Esta estructura es una propuesta para mantener la ficha sincronizada con las reglas. No presupone nombres de clases ya existentes:

- **ElementDefinition:** ID, nombre localizado, color, icono, rama y resumen individual.
- **ApplicationDefinition:** nombre de marca, elemento, duración base y reglas de renovación.
- **ReactionDefinition:** pareja/condiciones, nombre, etiquetas de comportamiento, descripción, duración, efectos y valores numéricos.
- **InteractionRule:** requisito de orden, cargas, estado previo, coexistencia, limpieza y objetivos.
- **ElementCardView:** dibuja una definición sin contener reglas de combate.
- **ElementPicker:** controla selección de uno o dos elementos.
- **ReactionSummaryPanel:** muestra/oculta el resumen de interacciones para la vista individual.

La interfaz debería leer los mismos datos editables que usa el combate, en lugar de duplicar a mano los efectos. Para descripciones narrativas, conviene mantener texto separado de los parámetros mecánicos.

### Lógica de selección orientativa

~~~text
si hay 1 elemento:
  mostrar su ficha individual
  si el resumen está activado:
    mostrar sus interacciones conocidas

si hay 2 elementos iguales:
  mostrar la aplicación o mecánica propia del elemento

si hay 2 elementos distintos:
  buscar reacción directa y condiciones de activación
  si existe:
    mostrar la ficha de reacción
  si no existe:
    mostrar "Sin reacción directa" y coexistencia si aplica
~~~

## 9. Límites de esta ficha

La ficha es una ayuda de consulta y no reemplaza el resolutor de combate. No debe calcular resultados con valores ausentes ni convertir propuestas en reglas finales. Las fórmulas generales y el catálogo vigente están en `Reacciones_elementales_MegaJuego.md`.

Entre los datos que todavía no conviene presentar como definitivos están los multiplicadores de daño propio de cada reacción, las fórmulas completas de Corrupción, algunos tiempos de AV, ciertas interacciones de Lumen y los hex exactos de la paleta.

