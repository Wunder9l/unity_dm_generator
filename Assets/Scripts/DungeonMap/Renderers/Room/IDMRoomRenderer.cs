using UnityEngine;

namespace DungeonMap.Renderers.Room {
    interface IDMRoomRenderer {
        void Render(RectTransform parent, Vector2 position, Vector2 sizes);
    }
}
