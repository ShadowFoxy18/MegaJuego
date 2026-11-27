# Informe de diseño incremental — Economía, moneda e intercambios

**Proyecto:** MegaJuego (Unity)  
**Estado:** propuesta inicial; no se ha implementado una economía ni un sistema de comercio en los scripts revisados.  
**Objetivo:** permitir que los personajes compren, vendan e intercambien objetos con el herrero y otros comerciantes usando una moneda común.

## 1. Qué existe actualmente en el proyecto

La base de objetos ya tiene:

- `ItemData`, como `ScriptableObject` con ID, nombre, icono, descripción, rareza y si el objeto se puede acumular.
- `ItemDatabase`, que mantiene una lista de objetos y permite buscarlos por ID.
- `DungeonRoomRole.Merchant`, como rol previsto para una sala de mazmorra.

En los scripts revisados no encontré todavía:

- Inventario del jugador ni de los personajes.
- Monedero o contador de moneda.
- Precios de compra/venta.
- Catálogo/stock de comerciantes.
- Registro de transacciones o sistema para intercambiar objetos.

Por eso, el comercio debe construirse sobre la base de objetos existente, pero aún falta definir cómo posee y guarda objetos el grupo.

## 2. Principios de diseño

1. **Una moneda común al principio.** Añadir monedas secundarias solo si una mecánica real las necesita.
2. **Separar dinero de objetos.** La moneda vive en el monedero; los objetos viven en el inventario. Así no se puede vender accidentalmente el dinero como si fuera un objeto común.
3. **Un sistema de transacciones compartido.** Herrero, mercader y otros servicios pueden usar el mismo proceso de compra/intercambio y cambiar su catálogo o reglas.
4. **El precio pertenece a la oferta**, no al objeto base. El mismo objeto podría tener precios diferentes en comerciantes distintos.
5. **Una operación se completa entera o no se aplica.** Validar primero saldo, objetos requeridos, espacio y stock; luego aplicar los cambios juntos.
6. **Prototipo pequeño.** Empezar con una compra sencilla. Añadir venta, trueque y servicios del herrero en etapas.
7. **Guardar IDs estables.** Para partidas guardadas, registrar IDs de objetos y cantidades, no referencias a objetos temporales de escena.

## 3. Conceptos

| Concepto | Responsabilidad |
|---|---|
| **Moneda** | Recurso numérico común del grupo. Su nombre y aspecto siguen pendientes. |
| **Monedero** | Guarda el saldo actual y valida si el grupo puede pagar. |
| **Inventario** | Guarda las cantidades de objetos poseídos. Aún hay que elegir si es individual o compartido por el grupo. |
| **Oferta** | Define qué vende un comerciante, por cuánto, con qué stock y bajo qué condiciones. |
| **Receta/trueque** | Define objetos y cantidades que se entregan para recibir otros objetos. |
| **Servicio del herrero** | Acción como mejorar, reparar, crear o modificar equipo; sus costes se especifican por servicio. |
| **Transacción** | Operación atómica que descuenta recursos y entrega objetos o aplica un servicio. |

## 4. Etapas de desarrollo

### Etapa 0 — Decisiones de reglas

Acordar antes de programar:

- Nombre, icono y presentación de la moneda.
- Si el monedero/inventario es individual o compartido por todo el equipo.
- Si los comerciantes compran objetos al jugador.
- Si los precios son fijos o dependen del comerciante, región o progreso.
- Si los objetos únicos pueden venderse.
- Qué pasa si el inventario está lleno.
- Si el stock es ilimitado o se agota y se repone.
- Qué servicios concretos ofrece el herrero.

**Recomendación inicial:** un monedero y un inventario compartidos por el grupo, precios por oferta, sin inflación ni regateo para la primera versión.

### Etapa 1 — Monedero e inventario

Crear operaciones básicas:

- Consultar saldo.
- Sumar o quitar moneda con validación.
- Consultar cantidad de un objeto.
- Añadir o retirar objetos.
- Comprobar capacidad y límites de acumulación.

El inventario debe respetar `ItemData.isStackable`. Falta definir capacidad máxima, espacios, peso o ausencia de límite.

**Criterio de finalización:** un objeto de prueba se añade y retira; el saldo no baja de cero.

### Etapa 2 — Compra mínima

Un catálogo de prueba ofrece un objeto por un precio fijo. El flujo de compra:

1. El jugador selecciona una oferta.
2. La interfaz muestra objeto, cantidad, precio y saldo disponible.
3. El sistema comprueba saldo, stock e inventario.
4. Si todo es válido, cobra y entrega el objeto.
5. Si falla una validación, no modifica ni el saldo ni el inventario.

**Criterio de finalización:** la compra se aplica correctamente o falla sin cambios parciales.

### Etapa 3 — Herrero como comerciante

El herrero puede empezar vendiendo armas/equipo del catálogo. Después se pueden añadir servicios separados:

- **Mejora:** consume moneda y materiales, aplica un cambio a un equipo.
- **Fabricación:** consume moneda/materiales y crea un objeto.
- **Reparación:** solo si el juego implementa durabilidad.
- **Desmontaje/venta de materiales:** solo si existe una necesidad clara.

No conviene mezclar estos efectos con la compra simple: todos pueden compartir validación/cobro, pero cada servicio define su resultado.

### Etapa 4 — Venta al comerciante

Permitir vender objetos que el jugador posee:

- Mostrar cantidad y precio de venta.
- Evitar vender objetos equipados, de misión o marcados como no vendibles.
- Devolver moneda solo tras retirar efectivamente los objetos.
- Definir relación entre compra y venta. Como punto inicial, el precio de venta podría ser inferior al de compra, pero los valores aún no están definidos.

### Etapa 5 — Trueque e intercambio de objetos

Permitir intercambiar una o varias entradas por una o varias salidas. Ejemplo conceptual: entregar 3 minerales y moneda para recibir un material refinado.

Cada receta/oferta debe indicar:

- Objetos y cantidades de entrada.
- Cantidad de moneda requerida, si aplica.
- Objetos y cantidades de salida.
- Requisitos opcionales.
- Si se puede repetir y cuántas veces.
- Comerciante que la ofrece.

La transacción comprueba todo antes de consumir nada y entrega todos los resultados a la vez.

### Etapa 6 — Guardado y persistencia

Guardar como mínimo:

- Saldo de moneda.
- Inventario por ID de objeto y cantidad.
- Stock restante de ofertas limitadas, si se implementa.
- Cambios de equipo y mejoras realizadas, si el herrero los ofrece.

El catálogo base y las recetas pueden permanecer como datos del juego; el guardado registra solo el estado que cambió durante la partida.

## 5. Flujo de compra y trueque

~~~text
Jugador abre el comercio
  → mostrar catálogo/ofertas
  → jugador elige oferta o receta
  → mostrar coste, requisitos y resultado
  → validar saldo, objetos, capacidad, stock y requisitos
  → si algo falla: mostrar motivo y no cambiar datos
  → si todo es válido: cobrar/retirar entradas y entregar resultados
  → refrescar monedero, inventario e interfaz
~~~

La validación y aplicación deberían centralizarse en un servicio de transacciones. La UI no debe descontar moneda por su cuenta ni llamar por separado a “quitar dinero” y “dar objeto” sin proteger la operación conjunta.

## 6. Modelo de datos recomendado

Propuesta conceptual, no clases ya existentes:

- **Wallet:** saldo de moneda y funciones validadas para añadir/gastar.
- **Inventory:** cantidades poseídas, espacio/capacidad y operaciones de añadir/retirar.
- **ShopDefinition:** ID de comerciante y lista de ofertas.
- **ShopOffer:** ID de oferta, referencia a `ItemData`, precio, stock y límites.
- **BarterRecipe:** entradas (objetos/cantidades y moneda) y salidas.
- **BlacksmithService:** acción de herrería y coste/resultados específicos.
- **TransactionService:** valida y aplica compra, venta, trueque o servicio.
- **ShopView:** muestra catálogo y resultado; no posee la lógica de economía.

Se puede reutilizar `ItemDatabase.GetById` para resolver objetos en datos guardados, aunque los IDs deben ser únicos y validados.

## 7. Persistencia y validaciones

- Validar IDs duplicados en `ItemDatabase` y en ofertas.
- No permitir saldo negativo ni cantidades de objetos inferiores a cero.
- Comprobar la capacidad antes de cobrar.
- Si una operación falla, restaurar o no modificar todos los recursos.
- Revisar los objetos nulos o IDs inexistentes al cargar partidas.
- Limitar cantidades y precios a valores válidos.
- Si se guardan precios en JSON, usar IDs y cantidades enteras; evitar valores flotantes para la moneda.
- Diferenciar error de saldo, inventario lleno, falta de materiales, stock agotado y requisito incumplido para dar mensajes claros.

## 8. Interfaz inicial

Una ventana de comerciante puede tener:

- Nombre y tipo de comerciante.
- Monedero actual visible.
- Lista de ofertas.
- Icono, nombre, descripción y cantidad del objeto.
- Precio/recursos requeridos.
- Botón de compra o intercambio.
- Confirmación opcional para operaciones de alto coste.
- Mensaje claro si la operación no se puede realizar.

El mismo panel puede servir a varios comerciantes si la identidad y catálogo vienen de datos. El herrero puede tener una pestaña de servicios más adelante.

## 9. Decisiones pendientes

| Tema | Estado |
|---|---|
| Nombre y representación de la moneda | Pendiente |
| Monedero/inventario individual o compartido | Pendiente; se recomienda compartido para empezar |
| Capacidad del inventario | Pendiente |
| Compra y venta entre personajes o desde un almacén de grupo | Pendiente |
| Precios y economía de progresión | Pendiente |
| Si comerciantes tienen stock y reposición | Pendiente |
| Servicios concretos del herrero | Pendiente |
| Trueques y materiales que participan | Pendiente |
| Objetos no vendibles/equipados/de misión | Pendiente |
| Guardado de economía e inventario | Pendiente de elegir sistema de guardado |
| Relación con diálogos | Puede abrirse el comercio desde una interacción/dialogue option, pero mantener comercio independiente del sistema de diálogos |

## 10. Orden recomendado

1. Elegir nombre de moneda y decidir si inventario/monedero es de grupo.
2. Definir capacidad y reglas mínimas del inventario.
3. Implementar una compra pequeña y atómica con un objeto de prueba.
4. Crear el catálogo del herrero usando ofertas con precio propio.
5. Añadir venta al comerciante.
6. Añadir recetas de trueque.
7. Añadir servicios de herrería y persistencia.
8. Conectar la apertura de la tienda a una interacción o diálogo.

Este orden permite tener primero una compra segura y comprobable. El trueque, las mejoras y el stock pueden añadirse cuando las reglas del juego los necesiten.

## 11. Relación con otros sistemas en planificación

- **Diálogos por keys y JSON:** pueden mostrar una opción como “Hablar”, “Comerciar” o “Mejorar equipo”. El sistema de diálogo solo inicia la acción; el sistema de comercio administra precios, inventario y transacciones.
- **Teletransporte:** la simplificación visual del fundido se mantiene como pendiente separado.
- **Objetos actuales:** `ItemData` y `ItemDatabase` son una base reutilizable; todavía no reemplazan el inventario ni el monedero que faltan.


