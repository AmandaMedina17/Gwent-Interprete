using System.Text;

 
public class AstPrinter : Expresion.IVisitante<string>
{
    
    public string Print(Expresion expr)
    {
        return expr.Aceptar(this);
    }

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
        builder.Append(expr.Aceptar(this));
    }
        builder.Append(")");

        return builder.ToString();
    }


    string Expresion.IVisitante<string>.visitarAsignacionExpresion(Expresion.AsignarExpresion obj)
    {
        throw new NotImplementedException();
    }

    

    string Expresion.IVisitante<string>.visitarLlamarExpresion(Expresion.LlamarExpresion obj)
    {
        throw new NotImplementedException();
    }

    string Expresion.IVisitante<string>.visitarGetExpresion(Expresion.GetExpresion obj)
    {
        throw new NotImplementedException();
    }

       string Expresion.IVisitante<string>.visitarExpresionLogica(Expresion.ExpresionLogica obj)
    {
        throw new NotImplementedException();
    }

    string Expresion.IVisitante<string>.visitarSetExpresion(Expresion.SetExpresion obj)
    {
        throw new NotImplementedException();
    }

    string Expresion.IVisitante<string>.visitarSuperExpresion(Expresion.SuperExpresion obj)
    {
        throw new NotImplementedException();
    }

    string Expresion.IVisitante<string>.visitarExpresionVariable(Expresion.ExpresionVariable obj)
    {
        throw new NotImplementedException();
    }

   
}
