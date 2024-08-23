public class Card : claseMadre
{
    private Expresion.ExpresionLiteral name;
    private Expresion.ExpresionLiteral type;
    private Expresion.ExpresionLiteral faction;
    private Expresion.ExpresionLiteral power;
    private List<Expresion> range;
    OnActivation onActivation;

    public static List<BaseCard> cartas = new List<BaseCard>();



    public Card(Expresion.ExpresionLiteral name, Expresion.ExpresionLiteral type, Expresion.ExpresionLiteral faction, Expresion.ExpresionLiteral power, List<Expresion> range, OnActivation onActivation) 
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
        if(!(name.Type is Tipo.Cadena)) return false;
        if(!(type.Type is Tipo.Cadena)) return false;
        if(!(faction.Type is Tipo.Cadena)) return false;
        if(!(power.Type is Tipo.Numero)) return false;

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
                    cartas.Add(new UnitCard(name, power, faction, TipoDeCarta.Unit, zonas));
                    break;
                case "Plata":
                    cartas.Add(new UnitCard(name, power, faction, TipoDeCarta.Unit, zonas));
                    break;
                case "Weather":
                    cartas.Add(new ClimateCard(name, power, faction, TipoDeCarta.Climate, zonas));
                    break;
                case "Bonus":
                    cartas.Add(new IncreaseCard(name, power, faction, TipoDeCarta.Increase, zonas));
                    break;
                case "Bait":
                    cartas.Add(new BaitCard(name, power, faction, TipoDeCarta.Bait, zonas));
                    break;
                case "Clear":
                    cartas.Add(new ClearanceCard(name, power, faction, TipoDeCarta.Clearance, zonas));
                    break;
                case "Leader":
                    cartas.Add(new LeaderCard(name, power, faction, TipoDeCarta.Leader, zonas));
                    break;
                default:
                    throw new Exception("Invalid card type.");
            }

            if (!(onActivation is null));
        
    }
}

public enum Type
{
    Gold, Silver, Clima, Increase, Leader, 
}
public enum Range
{
    Melee, Ranged, Siege, 
}


public enum Faction
{
    Nordic_Gods, Greek_Gods,
}

public enum TipoDeCarta
{
    Unit, Increase, Climate, Clearance, Leader, Bait,
}