using System;
using System.Collections.Generic;

namespace Match3
{
    /// <summary>
    /// Match3 game manager class.
    /// Creates grid, sends outputs, and receive the game input.
    /// </summary>
    public class Match3Game : IDisposable {
        public readonly Match3Events GameEvents;
        private readonly Match3Grid map;

        private int userScore;

        /// <summary>
        /// Creates a match 3 game.
        /// </summary>
        /// <param name="minMatch">how many match required in a row to MATCH3? 3? :D</param>
        /// <param name="gridSizeX">x size of the grid</param>
        /// <param name="gridSizeY">y size of the grid</param>
        /// <param name="avatars">array of the avatars. match3 members will be randomized from this</param>
        /// <param name="members">spawned members. id, avatar, x position, y position</param>
        public Match3Game (ushort minMatch, int gridSizeX, int gridSizeY, string [] avatars, Action<ushort, string, int, int> members) {
            map = new Match3Grid(minMatch, new Vector (gridSizeX, gridSizeY), avatars);
            GameEvents = new Match3Events();

            GameEvents.OnInteractMember += InteractMember;

            for (int y = 0, sizeY = map.Size.Y; y < sizeY; y++) {
                for (int x = 0, sizeX = map.Size.X; x < sizeX; x++) {
                    if (map.Grid[y][x] != null) {
                        members?.Invoke(map.Grid[y][x].Id, map.Grid[y][x].Avatar, x, y);
                    }
                }
            }
        }

        public void Dispose() {
            GC.SuppressFinalize(this);
        }

        public void InteractMember (int X, int Y) {
            var position = new Vector(X, Y);
            ushort id = map.RemoveFromPosition(position);

            GameEvents.OnMemberDestroyed?.Invoke(id);

            Vector[] drops = new Vector[map.Size.Y];
            int dropCount = map.DropFromTop(position, drops);
            for (int i=0; i<dropCount; i++) {
                GameEvents.OnMemberPositionUpdate?.Invoke(map.GetFromPosition(drops[i]).Id, drops[i].X, drops[i].Y);
            }

            int total = 0;
            map.CheckMap(
                    (ushort _id) => {
                        GameEvents.OnMemberDestroyed(_id);
                    }, (ushort _id, Vector _position) => {
                        GameEvents.OnMemberPositionUpdate(_id, _position.X, _position.Y);
                        total++;
                    });

            AddScore(total);

            GameEvents.OnReadyForVisualization?.Invoke();
        }

        private void AddScore(int multiplier) {
            userScore += (int)Math.Pow(1.2, multiplier);
            GameEvents.OnGameScoreUpdate?.Invoke(userScore);
        }
    }
}
