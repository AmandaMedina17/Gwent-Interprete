
public class IncreaseCard : BaseCard
{
    public double Incremento;
    public IncreaseCard(string Name, int InitialPower, Faction Faction, TipoDeCarta TipoDeCarta, List<Zonas> destinations, double Incremento = 0, EffectDelegate Effect = null) : base(Name, InitialPower, Faction, TipoDeCarta, destinations, Effect)
    {
        this.Incremento = Incremento == 0? 1: Incremento;
    }


}
