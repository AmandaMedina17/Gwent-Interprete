public class BaseCard
{
    public string Name;
    public int InitialPower;
    private int _power;
    public int Power {
        get => _power;
        set => _power =this is UnitCard ? value : _power; }
    public EffectDelegate EffectDelegate;
    public Faction Faction;
    public TipoDeCarta TipoDeCarta;
    public List<Zonas> destinations = new List<Zonas>();
    public List<BaseCard> PlaceRightNow;

    public BaseCard(string Name, int InitialPower, Faction Faction, TipoDeCarta TipoDeCarta, List<Zonas> destinations, EffectDelegate effectDelegate = null)
    {
        SelectEffect(effectDelegate);
        this.Name = Name;
        this.InitialPower = InitialPower;
        this.Faction = Faction;
        this.TipoDeCarta = TipoDeCarta;
        this.destinations = destinations;
        

    }
    public void Place(List<BaseCard> PlaceRightNow) => this.PlaceRightNow = PlaceRightNow is null ? Context.context.Jugadores[Faction].Hand : PlaceRightNow;

    public Player Owner { get => Context.context.Jugadores[Faction]; set => Owner = value; }
    public void SelectEffect(EffectDelegate effectDelegate)
    {
        if(effectDelegate == null) EffectDelegate = WarehouseOfEffects.EmptyEffect;
        else EffectDelegate = effectDelegate;
    }

    public virtual void Effect(EstadoDeJuego estadoDeJuego)
    {
        try
        {
            EffectDelegate.Invoke(estadoDeJuego);

        }
        catch (System.NullReferenceException)
        {
          
        }
    }

}

public enum Worth
{
    Golden, Silver
}