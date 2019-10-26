using UnityEngine;

namespace DungeonMap.Renderers.Road {
    interface IDMRoadRenderer {
        void Render(RectTransform parent, Vector2 position, Vector2 sizes);
    }
}