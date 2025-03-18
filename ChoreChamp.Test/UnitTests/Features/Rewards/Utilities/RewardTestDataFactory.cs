using ChoreChamp.API.Domain;

namespace ChoreChamp.Test.UnitTests.Features.Rewards.Utilities;

class RewardTestDataFactory
{
    public static IEnumerable<Reward> CreateRewards(int count, bool isLastRewardUnavailable = false)
    {
        for (int i = 1; i <= count; i++)
        {
            var reward = new Reward($"Reward {i}", $"Description {i}", 10);

            if (i == count && isLastRewardUnavailable)
            {
                reward.UpdateAvailability();
            }

            yield return reward;
        }
    }
}
