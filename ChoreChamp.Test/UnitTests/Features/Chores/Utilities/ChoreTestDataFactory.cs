using ChoreChamp.API.Domain;

namespace ChoreChamp.Test.UnitTests.Features.Chores.Utilities
{
    public static class ChoreTestDataFactory
    {
        public static IEnumerable<Chore> CreateChores(int count)
        {
            for (int i = 1; i <= count; i++)
            {
                yield return new Chore
                {
                    Id = i,
                    Name = $"Chore {i}",
                    Description = $"Description {i}",
                };
            }
        }

        public static IEnumerable<Chore> CreateEmptyChores()
        {
            return Enumerable.Empty<Chore>();
        }
    }
}
