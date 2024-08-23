public class Selector : Expresion
{
    public Expresion Source;
    public Expresion Single;
    public Expresion Predicate;
    public Expresion Parent;

    public Selector(Expresion Source, Expresion Single, Expresion Predicate, Expresion Parent)
    {
        this.Source = Source;
        this.Single = Single;
        this.Predicate = Predicate;
        this.Parent = Parent;
    }

    public override object Ejecutar()
    {
        List<Card> lista;
        switch ((string)Source.Ejecutar())
        {
            case "board":
                lista = Context.context.Board();
                break;
            case "deck":
                lista = Context.context.Deck();
                break;
            case "otherDeck":
                lista = Context.context.OtherDeck();
                break;
            case "hand":
                lista = Context.context.Hand();
                break;
            case "otherHand":
                lista = Context.context.OtherHand();
                break;
            case "field":
                lista = Context.context.Field();
                break;
            case "otherField":
                lista = Context.context.OtherField();
                break;
            case "parent":
                lista = (List<Card>)Parent.Ejecutar();
                break;
            default:
                throw new Exception("Invalid source");
        }
        return lista;
    }

    public override bool Semantica()
    {
        if(Source.type() != Tipo.Cadena) 
        {
            System.Console.WriteLine("Selector Source is invalid.");
            return false;
        }
        else if (!Source.Semantica()) return false;
        else if(Single.type() != Tipo.Bool)
        {
            System.Console.WriteLine("Selector Single is invalid.");
            return false;
        }
        else if (!Single.Semantica()) return false;
        else if (Predicate.type() != Tipo.Predicate)
        {
            System.Console.WriteLine("Selector Predicate is invalid");
            return false;
        }
        else if(!Predicate.Semantica()) return false;
        return true;
    
    }

    public override Tipo type()
    {
        return Tipo.Lista;
    }
}