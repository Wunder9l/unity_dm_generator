using UnityEngine;

namespace DungeonMap.Renderers.Room {
    class DMRoomRenderer : MonoBehaviour, IDMRoomRenderer {
        [SerializeField] private GameObject RoomPrefab;
        public void Render(RectTransform parent, Vector2 position, Vector2 sizes) {
            var room = GameObject.Instantiate(RoomPrefab, parent);
            var rectTransform = room.GetComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.zero;
            rectTransform.pivot = Vector2.zero;
            rectTransform.anchoredPosition = position;
            rectTransform.sizeDelta = sizes;
        }
    }
}
