public class LeaderCard : BaseCard
{
    public LeaderCard(string Name, int InitialPower, Faction Faction, TipoDeCarta TipoDeCarta, List<Zonas> destinations, EffectDelegate effectDelegate = null) : base(Name, InitialPower, Faction, TipoDeCarta, destinations, effectDelegate)
    {
    }

    public override void Effect(EstadoDeJuego estadoDeJuego)
    {
        try
        {
            if(!(EffectDelegate is null)) EffectDelegate.Invoke(estadoDeJuego)  ;
                                      
        }
        catch(System.NullReferenceException)
        {
           
        }
    }
}