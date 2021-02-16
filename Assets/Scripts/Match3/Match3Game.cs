using System;
using System.Collections.Generic;

namespace Match3
{
    public class Match3Game : IDisposable
    {
        public Match3Events GameEvents;

        private Match3Grid map;
        private int userScore;

        /// <summary>
        /// Creates a match 3 game.
        /// </summary>
        /// <param name="gridSizeX">x size of the grid</param>
        /// <param name="gridSizeY">y size of the grid</param>
        /// <param name="avatars">array of the avatars. match3 members will be randomized from this</param>
        public Match3Game (int gridSizeX, int gridSizeY, string [] avatars)
        {
            #region define
            map = new Match3Grid(new Vector (gridSizeX, gridSizeY), avatars);
            GameEvents = new Match3Events();
            #endregion

            GameEvents.OnInteractMember += InteractMember;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }


        public void InteractMember (int X, int Y) {
            map.RemoveFromPosition(new Vector(X, Y));

            List<ushort> destroyed;
            List<ushort> moveds;
            List<Vector> newPositions;

            map.CheckMap(out destroyed, out moveds, out newPositions);

            int dCount = destroyed.Count;
            int mCount = moveds.Count;

            for (int i = 0; i < dCount; i++) {
                GameEvents.OnMemberDestroyed (destroyed[i]);
            }

            for (int i = 0; i < mCount; i++) {
                GameEvents.OnMemberPositionUpdate (moveds[i], newPositions[i].X, newPositions[i].Y);
            }
        }

        private void AddScore(int multiplier) {
            userScore += (int)Math.Pow(2, multiplier);
            GameEvents.OnGameScoreUpdate?.Invoke(userScore);
        }
    }

}
