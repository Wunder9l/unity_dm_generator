using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

namespace DungeonMap.Renderers {
    class DungeonMapTilemapRenderer : MonoBehaviour, IDungeonMapRenderer {
        private int roomSize;
        private int roadLength;
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private TileBase roadTile;
        [SerializeField] private TileBase roomTile;

        public delegate void OnMapUpdatedCallback(Tilemap tilemap);
        public event OnMapUpdatedCallback OnMapUpdated;

        public void RenderDungeonMap(MapGrid map, float roomSize, float roadLength) {
            Assert.IsNotNull(tilemap);
            Assert.IsNotNull(roadTile);
            Assert.IsNotNull(roomTile);

            this.roomSize = Convert.ToInt32(roomSize);
            this.roadLength = Convert.ToInt32(roadLength);
            tilemap.ClearAllTiles();
            foreach (var level in map.Levels) {
                RenderLevel(level);
            }
            OnMapUpdated?.Invoke(tilemap);

            Debug.Log($"Tilemap size is {tilemap.size.x} x {tilemap.size.y}, with cell size {tilemap.cellSize.x}x{tilemap.cellSize.y}");
        }

        internal void RenderLevel(List<Node> nodes) {
            nodes.ForEach(node => {
                var position = GetRoomPosition(node);
                tilemap.SetTile(position, roomTile);
                if (node.UpNeighbour != null) {
                    foreach (int y in Enumerable.Range(node.Y * (roadLength + roomSize) + roomSize, roadLength))
                        tilemap.SetTile(new Vector3Int(position.x, y, 0), roadTile);
                }
                if (node.RightNeighbour != null) {
                    foreach (int x in Enumerable.Range(node.X * (roadLength + roomSize) + roomSize, roadLength))
                        tilemap.SetTile(new Vector3Int(x, position.y, 0), roadTile);
                }
            });
        }

        private Vector3Int GetRoomPosition(Node node) {
            return new Vector3Int(
                node.X * (roadLength + roomSize) + roomSize / 2,
                node.Y * (roadLength + roomSize) + roomSize / 2,
                0);
        }
    }
}
