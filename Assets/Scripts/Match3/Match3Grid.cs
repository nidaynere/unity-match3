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

            Grid = new Match3Member[Size.Y][];
            for (int y = 0; y < Size.Y; y++) {
                Grid[y] = new Match3Member[sizeX];

                for (int x=0; x<sizeX; x++) {
                    Grid[y][x] = new Match3Member(idCounter, avatars[Random.Range(0, avatarsLength)]);
                    idCounter++;
                }
            }

            var destroyed = new List<ushort>();
            var moveds = new List<ushort>();
            var newPositions = new List<Vector>();
            // Dear reviewer, Sorry for this :((

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

            Vector[] drops = new Vector[sizeY];
            Vector[] matches = new Vector[sizeX];

            for (int y = sizeY - 1; y >= 0; y--) {
                while (true) {

                    int matchCount = GetMatchesAtRow(y, ref matches);

                    if (matchCount == 0) {
                        break;
                    }

                    for (int i=0; i<matchCount; i++) {
                        var member = GetFromPosition(matches[i]);

                        if (member != null) {
                            destroyed.Add(GetFromPosition(matches[i]).Id);
                        }

                        RemoveFromPosition(matches[i]);
                        
                        int count = DropFromTop(matches[i], drops);

                        for (int d = 0; d < count; d++) {
                            moveds.Add (GetFromPosition(drops[d]).Id);
                            newPositions.Add(drops[d]);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Drops vectors to the given position.
        /// </summary>
        /// <param name="position">Positions to be dropped to</param>
        /// <param name="updated">Dropped positions. Put an array with Y length of the map</param>
        /// <returns>Positions count</returns>
        public int DropFromTop (Vector position, Vector[] updated)
        {
            int x = position.X;
            int y = position.Y;
            int c = 0;

            for (int b = y; b > 0; b--)
            {
                if (Grid[b - 1][x] != null)
                {
                    updated[c] = new Vector(x, b);

                    Grid[b][x] = Grid[b-1][x];
                    Grid[b - 1][x] = null;

                    c++;
                }
            }

            return c;
        }

        public ushort RemoveFromPosition(Vector position) {
            if (position.Y < 0 || position.Y >= Size.Y) {
                return ushort.MinValue;
            }

            if (position.X < 0 || position.X >= Size.X) {
                return ushort.MinValue;
            }

            if (Grid[position.Y][position.X] == null) {
                return ushort.MinValue;
            }

            var id = Grid[position.Y][position.X].Id;
            Grid[position.Y][position.X] = null;
            return id;
        }

        public Match3Member GetFromPosition (Vector position) {
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
                bool matchOnThisPoint = Grid[rowIndex][x] != null && Grid[rowIndex][x + 1] != null && Grid[rowIndex][x].Avatar.Equals(Grid[rowIndex][x + 1].Avatar);
                if (matchOnThisPoint) {
                    match++;
                }

                if (!matchOnThisPoint || x == xLength-2) {
                    if (match >= minMatch) {
                        for (int i = 0; i < match; i++) {
                            matches[counter++] = new Vector (matchStartPoint+i, rowIndex);
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
