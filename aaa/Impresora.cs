using System.Text;

 
public class AstPrinter
{
    

    public string visitarExpresionBinaria(Expresion.ExpresionBinaria expr) 
    { 
        return PonerEntreParentesis(expr.operador.Valor, new[] { expr.izquierda, expr.derecha });    
    }

    public string visitarExpresionAgrupacion(Expresion.ExpresionAgrupacion expr) { 
    return PonerEntreParentesis("grupo", new[] { expr.expresion });
    }

    public string visitarExpresionLiteral(Expresion.ExpresionLiteral expr) { 
    if (expr.valor == null) return "nil";
    return expr.valor.ToString();
    }

    public string visitarExpresionUnaria(Expresion.ExpresionUnaria expr) {
    return PonerEntreParentesis(expr.operador.Valor, new[] { expr.derecha });
    }


     private string PonerEntreParentesis(string name, IEnumerable<Expresion> expresiones) 
     {
        StringBuilder builder = new StringBuilder();

        builder.Append("(").Append(name);
        foreach(Expresion expr in expresiones) {
        builder.Append(" ");
        
    }
        builder.Append(")");

        return builder.ToString();
    }


}