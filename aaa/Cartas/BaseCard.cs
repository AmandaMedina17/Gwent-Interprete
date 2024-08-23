public class BaseCard
{
    public string Name;
    public int InitialPower;
    private int _power;
    public int Power {
        get => _power;
        set => _power =this is UnitCard ? value : _power; }
    public Effect Effect;
    public string Faction;
    public TipoDeCarta TipoDeCarta;
    public List<Zonas> destinations = new List<Zonas>();

    public BaseCard(string Name, int InitialPower, string Faction, TipoDeCarta TipoDeCarta, List<Zonas> destinations, Effect Effect = null)
    {
        this.Name = Name;
        this.InitialPower = InitialPower;
        this.Faction = Faction;
        this.TipoDeCarta = TipoDeCarta;
        this.Effect = Effect;
        this.destinations = destinations;
    }

}