﻿namespace Match3
{
    public class Match3Events {
        #region outputs
        public delegate void MemberPositionUpdate(ushort Id, int NewX, int NewY);
        public delegate void MemberDestroyed(ushort Id);
        public delegate void ReadyForVisualiation();
        public delegate void ScoreEvent(int Score);

        public MemberPositionUpdate OnMemberPositionUpdate;
        public MemberDestroyed OnMemberDestroyed;
        public ReadyForVisualiation OnReadyForVisualization;
        public ScoreEvent OnGameScoreUpdate;
        #endregion

        #region inputs
        public delegate void InteractMember (int X, int Y);
        public InteractMember OnInteractMember;
        #endregion
    }
}

