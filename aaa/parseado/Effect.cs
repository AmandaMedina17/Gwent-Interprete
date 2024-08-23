using System.Security.Cryptography.X509Certificates;

public class Effect: claseMadre
{
    public static Dictionary<string, Effect> efectosGuardados;
    private Expresion.ExpresionLiteral name;
    public Declaracion Action;
    private Dictionary<string, Tipo> Params;
    private Token context;
    private Token targets;
    private bool parametrosAndTargets = false;
    Entorno entorno;

    public Effect(Expresion.ExpresionLiteral name, Declaracion Action, List<(Token, Token)> Params, Token Targets, Token Context)
    {
        this.name = name;
        this.Action = Action;
        this.Params = new Dictionary<string, Tipo>();
        this.targets = Targets is null ? new Token(TokenType.Identificador, "targets", null, 0) : Targets;
        this.context = Context is null ? new Token(TokenType.Identificador, "context", null, 0) : Context;


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
            System.Console.WriteLine("Invalid effect name type.");
        }
        else{
            string nombre = Convert.ToString(name.Ejecutar());

            if(!efectosGuardados.ContainsKey(nombre)) efectosGuardados.Add(nombre, this);
            else throw new Exception("Previously saved effect.");
        }

        if(!Action.Semantica())
        {
            noHayErrores = false;
            System.Console.WriteLine("Invalid effect body.");
        }
        return noHayErrores;
    }

   

    public override void Ejecutar()
    {
        if(!parametrosAndTargets) throw new Exception("Trying to run " + name + " effect whitout parameters");
        Action.Ejecutar();
    }

    public void TargetsAndParametros(List<(Token, Expresion)> parametros, Expresion targets)
    {
        if(!(this.context.Valor != "context")) entorno.asignar(this.context, Context.context);
        entorno.asignar(this.targets, targets);

        foreach (var par in parametros)
        {
            try
            {
                if (Params[par.Item1.Valor] is Tipo.Object) System.Console.WriteLine("Asegurar tipo");
                else if (!(par.Item2.type() is Tipo.Object || par.Item2.type() == Params[par.Item1.Valor])) System.Console.WriteLine("Invalid expression received in param");;
            }
            catch (KeyNotFoundException)
            {
                System.Console.WriteLine("Invalid param received");
            }

            entorno.asignar(par.Item1, par.Item2);
        }
    
        parametrosAndTargets = true;
    }
}

