namespace Match3
{
    public static class Random
    {
        private static System.Random randomizer;
        public static int Range(int min, int max)
        {
            if (randomizer == null)
                randomizer = new System.Random();

            return randomizer.Next(min, max);
        }
    }
}
