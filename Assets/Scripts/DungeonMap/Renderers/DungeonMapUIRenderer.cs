using DungeonMap.Renderers.Road;
using DungeonMap.Renderers.Room;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace DungeonMap.Renderers {
    class DungeonMapUIRenderer : MonoBehaviour, IDungeonMapRenderer {
        [SerializeField] private RectTransform panel;

        private float roomSize;
        private float roadLength;
        private float unitSize;
        private Vector2 startOffset;
        private IDMRoomRenderer roomRenderer;
        private IDMRoadRenderer roadRenderer;

        private void Awake() {
            if (panel == null) {
                panel = GetComponent<RectTransform>();
            }
        }

        public void RenderDungeonMap(MapGrid map, float roomSize, float roadLength) {
            Assert.IsNotNull(panel);

            this.roomSize = roomSize;
            this.roadLength = roadLength;
            roomRenderer = GetComponent<IDMRoomRenderer>();
            roadRenderer = GetComponent<IDMRoadRenderer>();
            CalculateUnitSizeAndStartOffset(map.Sizes);

            Clear();
            foreach (var level in map.Levels) {
                RenderLevel(level);
            }
            //OnMapUpdated?.Invoke(tilemap);
        }

        private void CalculateUnitSizeAndStartOffset(Vector2Int sizes) {
            var xCells = roomSize * sizes.x + roadLength * Mathf.Max(sizes.x - 1, 0);
            var yCells = roomSize * sizes.y + roadLength * Mathf.Max(sizes.y - 1, 0);
            unitSize = Mathf.Min(panel.rect.width / Mathf.Max(xCells, 1), panel.rect.height / Mathf.Max(yCells, 1));
            startOffset = new Vector2((panel.rect.width - xCells * unitSize) / 2, (panel.rect.height - yCells * unitSize) / 2);
        }

        private void Clear() {
            foreach (Transform child in transform) {
                GameObject.Destroy(child.gameObject);
            }
        }

        private void RenderLevel(List<Node> nodes) {
            var roomSizes = Vector2.one * (roomSize * unitSize);
            nodes.ForEach(node => {
                var position = GetRoomPosition(node);
                roomRenderer.Render(panel, position, roomSizes);
                if (node.UpNeighbour != null) {
                    roadRenderer.Render(panel,
                        position + new Vector2((roomSize - 1) * unitSize / 2, roomSize * unitSize),
                        new Vector2(unitSize, roadLength * unitSize));
                }
                if (node.RightNeighbour != null) {
                    roadRenderer.Render(panel,
                        position + new Vector2(roomSize * unitSize, (roomSize - 1) * unitSize / 2),
                        new Vector2(roadLength * unitSize, unitSize));
                }
            });
        }

        private Vector2 GetRoomPosition(Node node) {
            return new Vector2(
                node.X * unitSize * (roadLength + roomSize) + roomSize / 2,
                node.Y * unitSize * (roadLength + roomSize) + roomSize / 2
            ) + startOffset;
        }
    }
}
