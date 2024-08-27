
using System.Diagnostics.Tracing;

public class UnitCard : BaseCard
{
    public Worth worth;
    public UnitCard(string Name, int InitialPower, Faction Faction, TipoDeCarta TipoDeCarta, List<Zonas> destinations, EffectDelegate Effect = null) : base(Name, InitialPower, Faction, TipoDeCarta, destinations, Effect)
    {
    }
}