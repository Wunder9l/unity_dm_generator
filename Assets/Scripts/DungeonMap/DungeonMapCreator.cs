using DungeonMap.Renderers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

namespace DungeonMap {
    public class DungeonMapCreator : MonoBehaviour {
        [SerializeField] private float roadLength = 3;
        [SerializeField] private float roomSize = 3;
        [SerializeField] private GameObject objectWithRenderer = null;
        private IDungeonMapRenderer Renderer => objectWithRenderer.GetComponent<IDungeonMapRenderer>();

        private void Awake() {
            if(objectWithRenderer ==null)
                objectWithRenderer = gameObject;
        }
        public void CreateDungeonMap(int roomsNumber, int maxLengthX, int maxLengthY) {
            var generator = new DungeonMapGenerator(roomsNumber, maxLengthX, maxLengthY);
            RenderMap(generator.Map);
        }

        public void TestCreateMap() => CreateDungeonMap(14, 5, 4);

        public void RenderMap(MapGrid map) {
            Assert.IsTrue(roomSize * roadLength != 0);
            Renderer.RenderDungeonMap(map, roomSize, roadLength);
        }
    }
}