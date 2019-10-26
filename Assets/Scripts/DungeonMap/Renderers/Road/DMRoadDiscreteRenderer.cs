using DungeonMap.Renderers.Road;
using System.Linq;
using UnityEngine;

namespace DungeonMap.Renderers {
    enum RoadDirection {
        ToRight, ToUp
    }
    class DMRoadDiscreteRenderer : MonoBehaviour, IDMRoadRenderer {
        [SerializeField] private GameObject RoadPrefab;

        public void Render(RectTransform parent, Vector2 position, Vector2 sizes) {
            var (unitsNumber, unitLength) = CalculateUnits(sizes);
            RoadDirection direction = sizes.x > sizes.y ? RoadDirection.ToRight : RoadDirection.ToUp;
            Vector2 unitSize = Vector2.one * unitLength;
            foreach (var i in Enumerable.Range(0, unitsNumber)) {
                var road = GameObject.Instantiate(RoadPrefab, parent);
                var rectTransform = road.GetComponent<RectTransform>();
                rectTransform.anchorMin = Vector2.zero;
                rectTransform.anchorMax = Vector2.zero;
                rectTransform.pivot = Vector2.zero;
                rectTransform.anchoredPosition = position;
                rectTransform.sizeDelta = unitSize;
                position += unitLength * (direction == RoadDirection.ToRight ? Vector2.right : Vector2.up);
            }
        }

        private (int, float) CalculateUnits(Vector2 sizes) {
            float length = Mathf.Max(sizes.x, sizes.y) / Mathf.Min(sizes.x, sizes.y);
            if (length % 1 > 0.5f) {
                return (Mathf.CeilToInt(length), Mathf.Max(sizes.x, sizes.y) / Mathf.Ceil(length));
            } else {
                return (Mathf.FloorToInt(length), Mathf.Max(sizes.x, sizes.y) / Mathf.Floor(length));
            }
        }
    }
}
