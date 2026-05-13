# Dungeon Game

Dungeon Game is a Unity 2D prototype centered on procedural dungeon generation. The project currently focuses on generating, rendering, and navigating random dungeon layouts at runtime, with combat and enemy behavior acting as a secondary gameplay layer.

## Procedural Dungeon Generation (Main Focus)

The dungeon system is built around a runtime pipeline that starts from the player's grid location and creates a connected layout of floors and walls.

- `DungeonGenerator` drives the full generation flow:
  - Clears previous node/tile data.
  - Runs walker-based floor creation.
  - Expands the layout with corridors.
  - Builds wall candidates around floor tiles.
  - Runs post-processing passes to reduce gaps and improve shape quality.
  - Enforces global connectivity by linking disconnected floor regions.
  - Spawns pathfinding nodes and caches them for A*.
  - Paints floor, wall, and connector tiles on both world and minimap tilemaps.
- `ProceduralAlgorithms` provides core generation primitives:
  - `RandomWalkAlgorithm` for organic room/floor growth.
  - `RandomCorridorAlgorithm` for directional corridor carving.
- `TileMapVisualizer` handles directional tile painting:
  - Selects floor/wall/corner/connector variants based on local tile direction context.
  - Paints both gameplay tilemaps and minimap tilemaps from the generated data.

![Dungeon Gameplay](./GIFs/dungeon_generation.gif)


## How It Works

Generation and navigation flow (high level):

1. Read player start as a grid position (`PlayerManager`).
2. Generate floor tiles via random walk iterations.
3. Carve additional corridors from generated positions.
4. Derive surrounding wall tiles from floor boundaries.
5. Run floor/wall post-processing and connectivity fixes.
6. Detect and place wall connector tiles.
7. Paint floor/wall/connectors into world + minimap tilemaps (`TileMapVisualizer`).
8. Spawn node objects across floor tiles and create neighbor links.
9. Cache node graph in `AStarManager` for enemy chase pathfinding.

## Current Features

- Runtime procedural dungeon generation with post-processing and connectivity repair.
- Tilemap-based world rendering plus minimap/world-map support.
- Custom node graph generation for A* path queries.
- Soldier player prototype with:
  - Movement.
  - Base attack.
  - Arrow attack (including hold/release charge behavior with camera zoom feedback).
  - Special attack visual feedback (area indicator + camera impulse shake).
- Enemy AI framework with state-machine patterns (idle, wander, chase, attack, hurt, death) and custom Armored Orc variants.

## Controls

- `W / A / S / D`: Move
- `F`: Create new dungeon at players location.
- `Left Mouse Button (LMB)`: Base attack
- `Right Mouse Button (RMB)`: Secondary attack (hold/release charge behavior)
- `Space`: Special attack
- `Tab`: Hold to open world map, release to return to minimap

## Tech Stack

- Engine: Unity `6000.3.9f1`
- Language: C#

## Getting Started

1. Clone this repository.
2. Open the project with Unity `6000.3.9f1`.
3. Open scene: `Assets/Scenes/SampleScene.unity`.
4. Press Play in the Unity Editor.
5. Use the controls above to test movement, combat prototypes, and map toggling.

## Legacy Project Notice

This project is now considered a legacy prototype.

- Active feature development is no longer a primary focus.
- The repository is kept mainly for reference, learning, and archival purposes.
- Future updates, if any, are expected to be limited to small fixes or maintenance.


