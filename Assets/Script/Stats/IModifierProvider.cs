using System.Collections;

namespace RPG.Stats
{
    public interface IModifierProvider
    {
        IEnumerable GetAdditiveModifiers(Stat stat);
        IEnumerable GetPercentageModifiers(Stat stat);
    }
}