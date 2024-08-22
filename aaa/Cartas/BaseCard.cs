public class BaseCard
{
    public string Name;
    public int InitialPower;
    private int _power;
    public int Power {
        get => _power;
        set => _power =this is UnitCard ? value : _power; }
    public Effect Effect;
    public Faction Faction;
    public TipoDeCarta TipoDeCarta;
    public Zonas[] destinations = new Zonas[3];

    public BaseCard(string Name, int InitialPower, Faction Faction, TipoDeCarta TipoDeCarta, Effect Effect, Zonas[] destinations)
    {
        this.Name = Name;
        this.InitialPower = InitialPower;
        this.Faction = Faction;
        this.TipoDeCarta = TipoDeCarta;
        this.Effect = Effect;
        this.destinations = destinations;
    }

}