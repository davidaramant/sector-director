# Renderer Independent

## Data Model Related

- [x] Load UDMF from disk
- [x] Make test maps
- [x] Create DataModelGenerator project as replacement for T4 templates
- [x] Replace Piglet with ~~Superpower~~ ~~Hime~~ a custom lexer/parser
- [x] Simplify DataModelGenerator
- [ ] Load binary Doom maps

## Game Related

- [x] Move player
- [x] Rotate player
- [x] Player horizontal clipping (simple)
- [ ] Redo player movement to project a desired location and prevent invalid moves
- [x] A working way to detect if the player is in a sector
- [ ] Menu system instead of opaque keyboard commands

## Misc

- [x] Text rendering (MonoGame)
- [x] Take another pass at input system (enum for discrete input?)
- [x] Benchmarks...
- [x] Convert projects to .NET Standard
- [ ] Upgrade to Core 3 when it's released and MonoGame support Core
- [x] Fix the Azure pipeline file once every is Core.  Mixing Framework and Core is a mess
- [ ] Deal with problematic Clipper dependency (not netstandard compliant)
- [ ] Use a cool BMF instead of boring Consolas font (`MonoGameExtended`)

# 2D Renderer

- [x] Render player position
- [x] Render player direction
- [x] Zoom in/out
- [x] Move view around
- [x] Show front side of linedef
- [x] Rotate view around player
- [x] Improve Wu line rendering
- [x] Don't draw lines that can't show up on screen
- [x] PSX Fire effect
- [ ] Ripple effect
- [x] Replace clunky math with real linear algebra stuff
- [x] Real clipping algorithm for lines

# 3D Renderer

- [ ] Render a sector
- [ ] Render linked sectors (portal)
- [ ] Non-Euclidean horizontal portals
- [ ] Lighting
- [ ] Texture mapping
- [ ] Transparent walls
- [ ] Vertical portals
- [ ] Sprites
