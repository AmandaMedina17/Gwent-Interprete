using System.Security.Cryptography.X509Certificates;

public class Effect: claseMadre
{
    private Expresion name;
    public Declaracion Action;
    private Dictionary<string, Tipo> Params;

    public Effect(Expresion name, Declaracion Action, List<(Token, Token)> Params)
    {
        this.name = name;
        this.Action = Action;
        this.Params = new Dictionary<string, Tipo>();

        foreach(var dupla in Params)
        {
            string type = dupla.Item2.Valor;
            Tipo tipo = Tipo.Bool;

            switch (type)
            {
                case "String": tipo = Tipo.Cadena; break;
                case "Number": tipo = Tipo.Numero; break;
                case "Bool": tipo = Tipo.Bool; break;
                default: throw new Exception("Tipo invalido");
            }

            this.Params.Add(dupla.Item1.Valor, tipo);
        }
    }

    internal override void Aceptar(Interprete interprete)
    {
        throw new NotImplementedException();
    }
}