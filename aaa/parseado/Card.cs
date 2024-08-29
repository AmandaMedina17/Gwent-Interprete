public class Card : claseMadre
{
    private Expresion name;
    private Expresion type;
    private Expresion faction;
    private Expresion power;
    private List<Expresion> range;
    public Effect effect;
    OnActivation onActivation;

    public static List<BaseCard> cartas = new List<BaseCard>();



    public Card(Expresion name, Expresion type, Expresion faction, Expresion power, List<Expresion> range, OnActivation onActivation) 
    {
        this.name = name;
        this.type = type;
        this.faction = faction;
        this.power = power;
        this.range = range;
        this.onActivation = onActivation;
    }
 
    public override bool Semantica()
    {
        if(!(name.type() is Tipo.Cadena)) return false;
        if(!(type.type() is Tipo.Cadena)) return false;
        if(!(faction.type() is Tipo.Cadena)) return false;
        if(!(power.type() is Tipo.Numero)) return false;

        return true;
    }

   

    public override void Ejecutar()
    {
        string name = (string)this.name.Ejecutar();
        string faction = (string)this.faction.Ejecutar();
        Faction Faction = faction == "Greek_Gods"? Faction.Greek_Gods : faction == "Nordic_Gods"? Faction.Nordic_Gods : throw new Exception("Invalid Faction");
        List<Zonas> zonas = new List<Zonas>();
        int power = this.power is null? 0 : (int)this.power.Ejecutar();
            

            foreach (var item in range)
            {
                switch ((string)item.Ejecutar())
                {
                    case "Melee":
                        zonas.Add(Zonas.Melee);
                        break;
                    case "Ranged":
                        zonas.Add(Zonas.Range);
                        break;
                    case "Siege":
                        zonas.Add(Zonas.Siege);
                        break;
                    default:
                        throw new Exception("Invalid range");
                }
            }

            switch ((string)type.Ejecutar())
            {
                case "Oro":
                    cartas.Add(new UnitCard(name, power, Faction, TipoDeCarta.Unit, zonas));
                    break;
                case "Plata":
                    cartas.Add(new UnitCard(name, power, Faction, TipoDeCarta.Unit, zonas));
                    break;
                case "Weather":
                    cartas.Add(new ClimateCard(name, power, Faction, TipoDeCarta.Climate, zonas));
                    break;
                case "Bonus":
                    cartas.Add(new IncreaseCard(name, power, Faction, TipoDeCarta.Increase, zonas));
                    break;
                case "Bait":
                    cartas.Add(new BaitCard(name, power, Faction, TipoDeCarta.Bait, zonas));
                    break;
                case "Clear":
                    cartas.Add(new ClearanceCard(name, power, Faction, TipoDeCarta.Clearance, zonas));
                    break;
                case "Leader":
                    cartas.Add(new LeaderCard(name, power, Faction, TipoDeCarta.Leader, zonas));
                    break;
                default:
                    throw new Exception("Invalid card type.");
            } 

        if (!(onActivation is null)) cartas[cartas.Count - 1].SelectEffect((EstadoDeJuego estadoDeJuego) => {
            try
            {
                onActivation.Ejecutar();
                return true;
            }
            catch
            {
                return false;
            }
        
        });
    }

}

public enum TypeOfCard
{
    Gold, Silver, Clima, Increase, Leader, 
}

public enum Faction
{
    Nordic_Gods, Greek_Gods,
}

public enum TipoDeCarta
{
    Unit, Increase, Climate, Clearance, Leader, Bait,
}