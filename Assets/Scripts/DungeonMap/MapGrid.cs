using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DungeonMap {
    public class MapGrid {
        private List<List<Node>> nodesByLevels;
        private float transitionProbability = 0.5f;
        public List<List<Node>> Levels { get => nodesByLevels; }
        public Vector2Int Sizes => new Vector2Int(CountX, CountY);
        public int CountX {
            get {
                if (nodesByLevels?.Count > 0) {
                    return nodesByLevels.Count(level => level.Count > 0);
                }
                return 0;
            }
        }
        public int MinY {
            get {
                if (nodesByLevels?.Count > 0) {
                    return nodesByLevels.Min(oneLevelNodes => (oneLevelNodes.Count > 0) ? oneLevelNodes.Min(node => node.Y) : int.MaxValue);
                }
                return 0;
            }
        }
        public int CountY {
            get {
                if (nodesByLevels?.Count > 0) {
                    return nodesByLevels.Max(oneLevelNodes => (oneLevelNodes.Count > 0) ? oneLevelNodes.Max(node => node.Y + 1) : int.MinValue);
                }
                return 0;
            }
        }

        public MapGrid(int x) {
            nodesByLevels = new List<List<Node>>();
            for (int i = 0; i < x; ++i) {
                nodesByLevels.Add(new List<Node>());
            }
        }

        public float[] GetLevelsWeights() => nodesByLevels.Select((val, index) => LevelWeight(val, index)).ToArray();

        private float LevelWeight(List<Node> level, int index) {
            if (level.Count > 0) {
                return 1f / (level.Count + 1);
            } else {
                if ((index > 0 && nodesByLevels[index - 1].Count > 0) ||
                    (index < nodesByLevels.Count - 1 && nodesByLevels[index + 1].Count > 0)) {
                    return 1f;
                }
            }
            return 0f;
        }

        internal void SetStartNode(Node startNode) {
            nodesByLevels[startNode.X].Add(startNode);
        }

        internal void InsertNode(int indexOfLevel, int y) {
            var newNode = new Node(indexOfLevel, y);
            List<Node> possibleNeighbours = new List<Node>();
            Action<Node> addTransitionMaybe = delegate (Node node) {
                possibleNeighbours.Add(node);
                if (TransitionChance()) {
                    node.MakeTransitionTo(newNode);
                }
            };

            // transitions for nodes in same level
            nodesByLevels[indexOfLevel].ForEach(node => {
                if (node.Y == y - 1 || node.Y == y + 1) {
                    addTransitionMaybe(node);
                }
            });
            // transitions for nodes on the left
            if (indexOfLevel > 0) {
                nodesByLevels[indexOfLevel - 1].ForEach(node => {
                    if (node.Y == y) {
                        addTransitionMaybe(node);
                    }
                });
            }
            // transitions for nodes on the right
            if (indexOfLevel < nodesByLevels.Count - 1) {
                nodesByLevels[indexOfLevel + 1].ForEach(node => {
                    if (node.Y == y) {
                        addTransitionMaybe(node);
                    }
                });
            }

            if (newNode.TransitionsCount == 0) {
                var selectedNode = possibleNeighbours[UnityEngine.Random.Range(0, possibleNeighbours.Count - 1)];
                newNode.MakeTransitionTo(selectedNode);
            }

            nodesByLevels[indexOfLevel].Add(newNode);
        }

        private bool TransitionChance() => UnityEngine.Random.Range(0f, 1f) <= transitionProbability;

        internal List<int> GetPossibleNodePosition(int indexOfLevel) {
            var positions = new List<int>();
            nodesByLevels[indexOfLevel].ForEach(node => {
                positions.Add(node.Y - 1);
                positions.Add(node.Y + 1);
            });
            if (indexOfLevel > 0) {
                nodesByLevels[indexOfLevel - 1].ForEach(node => positions.Add(node.Y));
            }
            if (indexOfLevel < nodesByLevels.Count - 1) {
                nodesByLevels[indexOfLevel + 1].ForEach(node => positions.Add(node.Y));
            }
            return positions.Distinct().ToList();
        }

        internal void Rearange() {
            var minY = MinY;
            nodesByLevels.ForEach(oneLevelNodes => oneLevelNodes.ForEach(node => node.RearangeByY(minY)));
        }
    }
}