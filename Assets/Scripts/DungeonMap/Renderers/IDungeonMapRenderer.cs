namespace DungeonMap.Renderers {
    interface IDungeonMapRenderer {
        void RenderDungeonMap(MapGrid map, float roomSize, float roadLength);
    }
}
