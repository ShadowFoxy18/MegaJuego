# Generación de mazmorras — desarrollo por pasos

## Paso 1: modelo de cuadrícula

`DungeonGrid` guarda qué celdas están ocupadas, ofrece posiciones vecinas libres y calcula distancias con BFS. Cada celda guarda un `DungeonCell` con tres datos: `PrefabKey`, `PrefabUseNumber` y `RoomNumber`. `00` significa vacío; `I1,1` identifica el primer uso del prefab de inicio en la sala 1; `C1,2` identifica el primer uso del prefab básico en la sala 2; `C2,3` identifica el segundo uso del prefab básico en la sala 3. La key `I` se reserva para inicio y `C` para sala básica.

Las distancias se calculan aparte con `GetDistancesFrom`; no forman parte de esta nomenclatura.

## Paso 2: cantidad y expansión

`DungeonGenerator` elige una cantidad aleatoria entre `minRooms` y `maxRooms`, contando todas las salas del mapa. Empieza en el centro y añade cada sala en una celda vacía vecina a las salas existentes. De este modo, la distribución queda conectada.

En el Inspector, `DungeonGenerator` tiene un campo de prefab para `I` y otro para `C`. Aún no se instancian; por ahora solo se genera la cuadrícula con sus keys y contadores. Para ver esa cuadrícula, activa `Generate On Start` y pulsa Play. En la Console aparecerá la cantidad generada y valores como `I1,1`, `C1,2` o `00` si la celda está vacía.

## Reglas acordadas para pasos posteriores

- Cada mazmorra tendrá una sala de inicio, una de jefe, una de guardado y una de mercader.
- El mínimo y máximo del Inspector contarán todas esas salas.
- El jefe estará a un mínimo de 3 conexiones del inicio.
- Guardado y mercader estarán cada uno a un mínimo de 3 conexiones del inicio y 5 del jefe.
- El catálogo aceptará una cantidad todavía indefinida de prefabs y variantes.

Estas reglas y la selección/instanciación de prefabs todavía no están implementadas. Por ahora, la cuadrícula usa las dos keys acordadas; la lista de referencias a los prefabs en el Inspector se conectará en el siguiente paso.
