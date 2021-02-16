namespace Match3
{
    public class Match3Member
    {
        /// <summary>
        /// Unique id in the game.
        /// </summary>
        public readonly ushort Id;

        /// <summary>
        /// Face of the member. Used for matching and visualization.
        /// </summary>
        public string Avatar;

        public Match3Member(ushort Id, string Avatar)
        {
            this.Id = Id;
            this.Avatar = Avatar;
        }
    }
}
