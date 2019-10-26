
using System;
using System.Collections.Generic;

namespace DungeonMap {
    struct Position {
        public int X;
        public int Y;

        public Position(int x, int y) {
            X = x;
            Y = y;
        }
    }
    public class Node {
        private Position position;
        private List<Node> transitions = new List<Node>();
        public int X => position.X;
        public int Y => position.Y;
        public int TransitionsCount => transitions.Count;

        public Node UpNeighbour => transitions.Find(node => node.Y == Y + 1);
        public Node RightNeighbour => transitions.Find(node => node.X == X + 1);

        public List<Node> Neighbours => transitions;

        public Node(int x, int y) {
            position = new Position(x, y);

        }
        public void MakeTransitionTo(Node other) {
            transitions.Add(other);
            other.transitions.Add(this);
        }

        internal void RearangeByY(int minY) {
            position.Y -= minY;
        }
    }
}
