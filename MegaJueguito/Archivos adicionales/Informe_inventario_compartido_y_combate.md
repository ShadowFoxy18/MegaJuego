# Informe de diseño — Inventario compartido y de combate

**Proyecto:** MegaJuego (Unity)  
**Estado:** propuesta de diseño; no se ha implementado el sistema de inventario.  
**Objetivo:** administrar los objetos del grupo fuera del combate y preparar recursos para cada personaje durante el combate.

## 1. Propuesta principal

Mantener **un inventario compartido como fuente principal de objetos**. El inventario de combate de cada personaje sería una **asignación temporal** tomada del inventario compartido, no una segunda copia independiente.

Esto conserva la comodidad de compartir recursos y añade una decisión táctica: elegir antes del combate qué objetos prepara cada personaje.

## 2. Flujo propuesto

### Fuera del combate
- El grupo consulta los objetos y cantidades que posee.
- El jugador equipa objetos y prepara consumibles para cada personaje.
- La preparación se puede cambiar antes de entrar al combate.

### Al comenzar el combate
- Se crea una preparación por personaje desde el inventario compartido.
- Los objetos asignados quedan reservados para ese personaje.
- El límite de espacios o unidades se decidirá y podrá balancearse después.

### Durante el combate
- Cada personaje puede usar los objetos que tiene asignados.
- Al usar uno, disminuye su cantidad preparada.
- Si usar un objeto consume una acción o turno, se resolverá según las reglas de combate. El proyecto ya tiene `TurnTypes.UseItem` como tipo, pero no se encontró el inventario implementado.
- Para la primera versión, cada personaje usa sus objetos preparados y no accede directamente al almacén común durante su turno.

### Al terminar el combate
- Los objetos usados se descuentan del inventario compartido.
- Los objetos preparados pero no usados vuelven a estar disponibles.
- La actualización se aplica una sola vez para no duplicar o perder objetos.

## 3. Por qué usar una asignación temporal

Una reserva de combate separada y sin conexión con el inventario compartido puede provocar duplicados, pérdidas o diferencias entre lo que muestra la interfaz y lo guardado. La asignación temporal mantiene una sola fuente de verdad:

~~~text
Inventario compartido
  → preparación por personaje
  → consumos durante el combate
  → reconciliación al terminar
  → inventario compartido actualizado
~~~

Si más adelante se permite recuperar objetos del almacén durante el combate, se puede añadir como una acción explícita con un coste. No hace falta incluirlo en la primera versión.

## 4. Ventaja estratégica

El jugador podrá preparar recursos según la función del personaje y el combate esperado: dar recuperación a un personaje frágil, reservar recursos ofensivos, repartir objetos escasos y decidir qué conservar para después.

Para que la preparación tenga peso, se puede limitar cuántos objetos lleva cada personaje. El número de espacios o unidades queda pendiente; no se fija aquí ningún valor.

## 5. Datos y responsabilidades

| Parte | Responsabilidad |
|---|---|
| **Definición del objeto** | ID, nombre, icono, descripción, rareza, acumulación y, cuando se defina, tipo/efecto. |
| **Inventario compartido** | Cantidad poseída de cada ID de objeto. |
| **Preparación de combate** | IDs y cantidades asignados temporalmente a cada personaje. |
| **Estado de combate** | Cantidades preparadas que quedan tras los usos. |
| **Reconciliación** | Descuenta consumos y devuelve lo no usado al inventario común. |

Nombres orientativos para futuros scripts:

- `Inventory`: inventario compartido.
- `CombatLoadout`: objetos preparados por personaje.
- `CombatInventory`: acceso a las cantidades asignadas durante el combate.
- `InventoryService`: valida operaciones para añadir, retirar, reservar y devolver objetos.
- `ItemData`: definición del objeto, ya existente en el proyecto.

Son nombres sugeridos, no clases ya implementadas ni definitivas.

## 6. Situación actual del proyecto

En los scripts revisados se encontró:

- `ItemData` tiene `itemId`, nombre, icono, descripción, rareza y `isStackable`.
- `ItemDatabase` mantiene el catálogo y busca objetos por ID.
- `RarityConfig` permite asociar colores a rarezas.
- `TurnTypes` contiene `UseItem`.

No se encontró un inventario, preparación individual de combate, monedero ni lógica de consumo. La rareza y la base de datos describen objetos; no indican cuáles posee cada personaje o el grupo.

## 7. Rareza, acumulación y equipamiento

- La rareza clasifica y presenta objetos; no debería imponer automáticamente su cantidad máxima, precio o utilidad táctica.
- `isStackable` puede definir si varias unidades comparten una entrada.
- Equipo y consumibles pueden tener reglas distintas: el equipo ocupa ranuras de equipo; los consumibles se cuentan y preparan.
- Objetos de misión, equipados o no vendibles necesitarán restricciones propias al implementar comercio y guardado.
- La capacidad máxima, si existe, debe definirse como regla del inventario y no deducirse solo de la rareza.

## 8. Validaciones importantes

- No permitir cantidades negativas.
- No preparar más unidades de las que existen en el inventario compartido.
- No asignar unidades duplicadas a distintos personajes.
- Evitar reconciliar el mismo combate dos veces.
- Definir qué ocurre al huir, perder o abandonar el combate antes de confirmar consumos.
- Devolver al inventario los objetos preparados que no se usaron.
- Guardar IDs y cantidades, no copias de objetos ni referencias temporales de escena.

## 9. Decisiones pendientes

| Tema | Estado |
|---|---|
| Inventario compartido por el grupo | Propuesta preferida; falta confirmación final. |
| Preparar objetos por personaje antes del combate | Propuesta preferida; falta definir interfaz y momento. |
| Límite de ranuras/unidades por personaje | Pendiente de balance. |
| Si el uso consume una acción/turno | Pendiente; encajarlo con `TurnTypes.UseItem`. |
| Acceso al almacén durante el combate | Recomendación inicial: no, salvo una acción explícita con coste. |
| Qué ocurre al huir, perder o abandonar | Pendiente. |
| Reglas de equipo frente a consumibles | Pendiente. |
| Persistencia entre sesiones | Pendiente de elegir sistema de guardado. |

## 10. Orden recomendado de implementación

1. Definir si inventario y preparación pertenecen al grupo o a cada personaje.
2. Crear inventario compartido con IDs y cantidades.
3. Añadir una pantalla simple para preparar cada personaje.
4. Crear el estado temporal individual de combate.
5. Conectar el uso de consumibles con `UseItem`.
6. Reconciliar consumos y devolver sobrantes al terminar.
7. Añadir límites, restricciones y guardado tras validar el flujo básico.

La primera versión debería completar un ciclo sencillo con unos pocos objetos de prueba antes de añadir límites o más categorías.

## 11. Relación con otros sistemas

- **Comercio:** compras y ventas modifican el inventario compartido; la tienda no edita directamente una preparación activa.
- **Diálogos:** una opción puede abrir tienda o inventario, pero el diálogo no administra cantidades.
- **Rarezas:** clasifican y colorean objetos, no sustituyen las reglas de posesión.
- **Combate por turnos:** el inventario de combate valida quién puede usar cada recurso y cuántas unidades quedan.


