using System.Collections.Generic;

namespace Match3
{
    /// <summary>
    /// a grid that holds the whole game.
    /// </summary>
    public struct Match3Grid {
        private const ushort minMatch = 3;

        public Match3Member[][] Grid;
        public Vector Size;

        public Match3Grid(Vector GridSize, string [] avatars) {
            Size = GridSize;

            ushort idCounter = 0;

            int sizeX = GridSize.X;
            int avatarsLength = avatars.Length;
            // define grid.
            Grid = new Match3Member[Size.Y][];
            for (int y = 0; y < Size.Y; y++) {
                Grid[y] = new Match3Member[sizeX];

                for (int i=0; i<sizeX; i++) {
                    Grid[y][i] = new Match3Member(idCounter, avatars[Random.Range(0, avatarsLength)]);
                    idCounter++;
                }
            }

            var destroyed = new List<ushort>();
            var moveds = new List<ushort>();
            var newPositions = new List<Vector>();

            CheckMap(out destroyed, out moveds, out newPositions);
        }

        /// <summary>
        /// checks map and remove matches.
        /// </summary>
        public void CheckMap (out List<ushort> destroyed, out List<ushort> moveds, out List<Vector> newPositions) {
            int sizeY = Size.Y;
            int sizeX = Size.X;

            destroyed = new List<ushort>();
            moveds = new List<ushort>();
            newPositions = new List<Vector>();

            Vector[] matches = new Vector[sizeX];

            for (int y = sizeY - 1; y > 0; y--) {
                while (true) {
                    int matchCount = GetMatchesAtRow(y, ref matches);
                    if (matchCount == 0) {
                        break;
                    }

                    // clear all.
                    for (int i=0; i<matchCount; i++) {
                        var member = GetFromPosition(matches[i]);

                        if (member != null) {
                            destroyed.Add(GetFromPosition(matches[i]).Id);
                        }

                        RemoveFromPosition(matches[i]);

                        // bring from top.
                        for (int b = matches[i].Y; b > 0; b--) {
                            if (Grid[b - 1][matches[i].X] != null)
                            {
                                newPositions.Add(new Vector(matches[i].X, b));
                                moveds.Add(Grid[b][matches[i].X].Id);
                            }

                            Grid[b][matches[i].X] = Grid[b - 1][matches[i].X];
                        }
                    }
                }
            }
        }

        public bool RemoveFromPosition(Vector position)
        {
            if (position.Y < 0 || position.Y >= Size.Y)
            {
                return false;
            }

            if (position.X < 0 || position.X >= Size.X)
            {
                return false;
            }

            if (Grid[position.Y][position.X] == null)
            {
                return false;
            }

            Grid[position.Y][position.X] = null;

            return true;
        }

        private Match3Member GetFromPosition (Vector position) {
            if (position.Y < 0 || position.Y >= Size.Y) {
                return null;
            }

            if (position.X < 0 || position.X >= Size.X) {
                return null;
            }

            return Grid[position.Y][position.X];
        }

        /// <summary>
        /// Finds matches in the specific row.
        /// </summary>
        /// <param name="rowIndex">index of the row</param>
        /// <param name="matches">put an here array as length of X size of grid</param>
        /// <returns>matches amount</returns>
        private int GetMatchesAtRow (int rowIndex, ref Vector [] matches) {
            if (rowIndex < 0 || rowIndex >= Size.Y) {
                // out of map?
                return 0; 
            }

            int counter = 0;
            int xLength = Size.X;
            int matchStartPoint = 0;
            int match = 1;

            for (int x = 0; x < xLength - 1; x++) {
                if (Grid[rowIndex][x] != null && Grid[rowIndex][x].Id.Equals(Grid[rowIndex][x].Id)) {
                    match++;
                }
                else {// no match.
                    if (match >= minMatch) { // gather last match.
                        for (int i = matchStartPoint; i < match; i++) {
                            matches[counter++] = new Vector (rowIndex, i);
                        }
                    }

                    match = 1;
                    matchStartPoint = x + 1;
                }
            }

            return counter;
        }
    }
}
