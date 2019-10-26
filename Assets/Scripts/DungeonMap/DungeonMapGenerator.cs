using System;
using System.Collections.Generic;
using System.Linq;

namespace DungeonMap {
    public class DungeonMapGenerator {
        public int TotalRooms;
        public int MaxLengthX;
        public int PreferableLengthY;
        private MapGrid map;

        public MapGrid Map { get => map; }

        public DungeonMapGenerator(int totalRooms, int maxLengthX, int preferableLengthY) {
            TotalRooms = totalRooms;
            MaxLengthX = maxLengthX;
            PreferableLengthY = preferableLengthY;
            map = new MapGrid(MaxLengthX);

            UnityEngine.Random.InitState(DateTime.Now.Millisecond);
            Node startNode = GenerateStartNode();
            map.SetStartNode(startNode);
            for (int i = 1; i < totalRooms; ++i) {
                GenerateNextNode();
            }
            map.Rearange();
        }

        private Node GenerateStartNode() {
            return new Node(0, UnityEngine.Random.Range(0, PreferableLengthY / 2));
        }

        private void GenerateNextNode() {
            var levelsWeights = map.GetLevelsWeights();
            var indexOfLevel = GetRandomWeightedIndex(levelsWeights);
            var possiblePositions = map.GetPossibleNodePosition(indexOfLevel);
            var y = ChoosePosition(possiblePositions);
            map.InsertNode(indexOfLevel, y);
        }

        private int ChoosePosition(List<int> variants) => variants[UnityEngine.Random.Range(0, variants.Count - 1)];

        public int GetRandomWeightedIndex(float[] weights) {
            float randomNumber = UnityEngine.Random.Range(0f, weights.Sum());
            float curSum = 0f;
            for (int i = 0; i < weights.Length; ++i) {
                curSum += weights[i];
                if (curSum >= randomNumber) {
                    return i;
                }
            }
            return 0;
        }

    }
}