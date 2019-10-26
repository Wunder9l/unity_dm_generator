using DungeonMap.Renderers;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DungeonMap {
    public class DMCamera : MonoBehaviour {
        private Camera camera;
        [SerializeField] DungeonMapTilemapRenderer tilemapRenderer;
        private void Start() {
            camera = GetComponent<Camera>();
        }
        private void OnEnable() {
            if (tilemapRenderer != null) {
                tilemapRenderer.OnMapUpdated += OnMapUpdated;
            }
        }
        private void OnDisable() {
            if (tilemapRenderer != null) {
                tilemapRenderer.OnMapUpdated += OnMapUpdated;
            }
        }
        void OnMapUpdated(Tilemap tilemap) {
            camera.orthographicSize = (float)(Math.Max(tilemap.size.x, tilemap.size.y) + 1)/2f;
            transform.localPosition = new Vector3((tilemap.size.x + 1) / 2f, (tilemap.size.y + 1) / 2f, -10f);
        }
    }
}