
public class BaitCard : BaseCard
{
    public BaitCard(string Name, int InitialPower, Faction Faction, TipoDeCarta TipoDeCarta, List<Zonas> destinations, EffectDelegate Effect = null) : base(Name, InitialPower, Faction, TipoDeCarta, destinations, Effect)
    {
    }

      public override bool Effect(EstadoDeJuego estadoDeJuego)
    {
        try
        {
            return Effect(estadoDeJuego.lista, estadoDeJuego.lista.IndexOf(estadoDeJuego.card)) & EffectDelegate.Invoke(estadoDeJuego);
        }
        catch (System.NullReferenceException)
        {
            return false;
        }
    }

    public bool Effect(List<BaseCard> list, int index)
    {
        BaseCard card = list[index];
        if (card is BaitCard) return false;
        list[index] = this;
        Context.context.Jugadores[Faction].Hand[Context.context.Jugadores[Faction].Hand.IndexOf(this)] = card;
        if (card is UnitCard unit) 
        {
            unit.Power = unit.InitialPower;
        }
    
        return true;
    }
}