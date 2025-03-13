using ChoreChamp.API.Domain;

namespace ChoreChamp.API.Tests.UnitTests.Features.Chores
{
    public static class ChoreTestDataFactory
    {
        public static List<Chore> CreateChores(int count)
        {
            var chores = new List<Chore>();
            for (int i = 1; i <= count; i++)
            {
                chores.Add(
                    new Chore
                    {
                        Id = i,
                        Name = $"Chore {i}",
                        Description = $"Description {i}",
                    }
                );
            }
            return chores;
        }

        public static List<Chore> CreateEmptyChores()
        {
            return new List<Chore>();
        }
    }
}
