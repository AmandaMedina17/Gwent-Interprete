
using System.Diagnostics.Tracing;

public class UnitCard : BaseCard
{
    public Worth worth;
    public UnitCard(string Name, int InitialPower, string Faction, TipoDeCarta TipoDeCarta, List<Zonas> destinations, Effect Effect = null) : base(Name, InitialPower, Faction, TipoDeCarta, destinations, Effect)
    {
    }
}