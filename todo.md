# Renderer Independent

## Data Model Related

- [x] Load UDMF from disk
- [x] Make test maps
- [x] Create DataModelGenerator project as replacement for T4 templates
- [x] Replace Piglet with ~~Superpower~~ Hime
- [x] Simplify DataModelGenerator

## Game Related

- [x] Move player
- [x] Rotate player
- [x] Player horizontal clipping (simple)
- [ ] Player vertical clipping
- [ ] Player horizontal clipping (properly taking into account radius)
- [ ] Sub-sector division
- [ ] Determine if a sector is convex

## Misc

- [x] Text rendering (MonoGame)
- [x] Take another pass at input system (enum for discrete input?)
- [x] Benchmarks...
- [ ] Convert projects to .NET Standard

# 2D Renderer

- [x] Render player position
- [x] Render player direction
- [x] Zoom in/out
- [x] Move view around
- [x] Show front side of linedef
- [x] Rotate view around player
- [x] Improve Wu line rendering
- [x] Don't draw lines that can't show up on screen
- [ ] Wu circle rendering
- [x] PSX Fire effect
- [ ] Ripple effect
- [ ] Replace clunky math with real linear algebra stuff
- [ ] Real clipping algorithm for lines

# 3D Renderer

- [ ] Render a sector
- [ ] Render linked sectors (portal)
- [ ] Non-Euclidean horizontal portals
- [ ] Lighting
- [ ] Texture mapping
- [ ] Transparent walls
- [ ] Vertical portals
- [ ] Sprites