namespace Match3
{
    public class Match3Events {
        #region outputs
        public delegate void MemberSpawned(Match3Member bubble, int NewX, int NewY);
        public delegate void MemberPositionUpdate(ushort Id, int NewX, int NewY);
        public delegate void MemberDestroyed(ushort Id);
        public delegate void ReadyForVisualiation();
        public delegate void ScoreEvent(int Score);

        public MemberSpawned OnMemberSpawned;
        public MemberPositionUpdate OnMemberPositionUpdate;
        public MemberDestroyed OnMemberDestroyed;
        public ReadyForVisualiation OnReadyForVisualization;
        public ScoreEvent OnGameScoreUpdate;
        public ScoreEvent OnGameFinished;
        #endregion

        #region inputs
        public delegate void InteractMember (int X, int Y);
        public InteractMember OnInteractMember;
        #endregion
    }
}

