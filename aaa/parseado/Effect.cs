using System.Security.Cryptography.X509Certificates;

public class Effect: claseMadre
{
    public static Dictionary<string, Effect> efectosGuardados;
    private Expresion.ExpresionLiteral name;
    public Declaracion Action;
    private Dictionary<string, Tipo> Params;
    Entorno entorno;

    public Effect(Expresion.ExpresionLiteral name, Declaracion Action, List<(Token, Token)> Params)
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

    public override bool Semantica()
    {
        bool noHayErrores = true;

        if (!(name.Type is Tipo.Cadena)) 
        {
            noHayErrores = false;
            System.Console.WriteLine("Tipo de nombre invalido");
        }
        else{
            string nombre = Convert.ToString(name.Evaluar());

            if(!efectosGuardados.ContainsKey(nombre)) efectosGuardados.Add(nombre, this);
            else throw new Exception("efecto ya guardado anteriormente");
        }

        if(!Action.Semantica())
        {
            noHayErrores = false;
            System.Console.WriteLine("Cuerpo de efecto invalido");
        }
        return noHayErrores;
    }

    internal override void Aceptar(Interprete interprete)
    {
        throw new NotImplementedException();
    }


}