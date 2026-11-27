# Dungeon generation with Wave Function Collapse

We are rebuilding the generator with the **simple tiled model**: each grid cell becomes an empty cell or one room prefab. Neighboring room pieces must agree on their edge connectors.

## Step 1: room-piece catalog

`WfcRoomCatalog` stores room pieces in the Inspector. Each piece has a prefab, the sides with doors, whether it may be the start, and a weight. Pieces keep their authored orientation; a new orientation requires a separate prefab entry.

The empty cell is represented by `WfcRoomOption.Empty`: it has no prefab and four walls.

For the current prefabs, the initial catalog values will be:

- `BasicExample`: door to the north; allowed as the start.
- `BasicExample 2`: doors east and west.

## Planned stages

1. Define catalog pieces and their fixed door orientations.
2. Initialize every grid cell with the valid options; restrict the chosen start cell to start-capable variants.
3. Implement WFC observation and adjacency propagation.
4. Check reachability from the start, retry contradictions or disconnected results, then instantiate the selected prefabs.

The earlier `DungeonGenerator` and `DungeonGrid` scripts belong to the previous prototype; the WFC solver does not use them.
