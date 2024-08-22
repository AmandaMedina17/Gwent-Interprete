public class Card : claseMadre
{
    private Expresion.ExpresionLiteral name;
    private Expresion.ExpresionLiteral type;
    private Expresion.ExpresionLiteral faction;
    private Expresion.ExpresionLiteral power;
    private List<Expresion> range;


    public Card(Expresion.ExpresionLiteral name, Expresion.ExpresionLiteral type, Expresion.ExpresionLiteral faction, Expresion.ExpresionLiteral power, List<Expresion> range) 
    {
        this.name = name;
        this.type = type;
        this.faction = faction;
        this.power = power;
        this.range = range;
    }
 
    public bool SemanticaIncorrecta()
    {
        bool hayError = false;
        if(!(name.Type is Tipo.Cadena)) hayError = true;
        if(!(type.Type is Tipo.Cadena)) hayError = true;
        if(!(faction.Type is Tipo.Cadena)) hayError = true;
        if(!(power.Type is Tipo.Numero)) hayError = true;

        return hayError;
    }

    public override bool Semantica()
    {
        throw new NotImplementedException();
    }

    public override void Ejecutar()
    {
        throw new NotImplementedException();
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