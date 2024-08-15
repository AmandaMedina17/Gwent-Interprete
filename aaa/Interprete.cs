using System.Linq.Expressions;

public class Interprete : Expresion.IVisitante<Object>, Declaracion.IVisitor<object>
{
    private Entorno entorno = new Entorno();  // Es una estructura que mantiene las variables y sus valores actuales. Actúa como un contexto de ejecución para las expresiones y declaraciones
    
    //Expresion de Asignacion
    public object visitarAsignacionExpresion(Expresion.AsignarExpresion obj)
    {
        object valor = evaluar(obj.valor);
        entorno.asignar(obj.nombre, valor);
        return valor;        
    }

    // Expresion de Agrupacion
    public object visitarExpresionAgrupacion(Expresion.ExpresionAgrupacion obj)
    {
        return evaluar(obj.expresion);
    }

    // Expresion Binaria
    public object visitarExpresionBinaria(Expresion.ExpresionBinaria obj)
    {
        object izquierda = evaluar(obj.izquierda);
        object derecha = evaluar(obj.derecha); 

        switch (obj.operador.Tipo) 
        {
            case TokenType.Mayor:
                comprobarNumero(obj.operador, izquierda, derecha);
                return (double)izquierda > (double)derecha;
            case TokenType.Mayor_igual:
                comprobarNumero(obj.operador, izquierda, derecha);
                return (double)izquierda >= (double)derecha;
            case TokenType.Menor:
                comprobarNumero(obj.operador, izquierda, derecha);
                return (double)izquierda < (double)derecha;
            case TokenType.Menor_igual:
                comprobarNumero(obj.operador, izquierda, derecha);
                return (double)izquierda <= (double)derecha;
            case TokenType.Concatenacion:
                return izquierda.ToString() + derecha.ToString();
            case TokenType.Concatenacion_Espaciado:
                return izquierda.ToString() + " " + derecha.ToString();
            case TokenType.Menos:
                comprobarNumero(obj.operador, izquierda, derecha);
                return (double)izquierda - (double)derecha;
            case TokenType.Bang_igual: 
                return !esIgual(izquierda, derecha);
            case TokenType.Igual_igual: 
                return esIgual(izquierda, derecha);
            case TokenType.Más:
                if (izquierda is double && derecha is double) 
                {
                    return (double)izquierda + (double)derecha;
                } 
                throw new RuntimeError(obj.operador, "Los operandos deben ser dos números o dos cadenas.");
            case TokenType.Slach:
                comprobarNumero(obj.operador, izquierda, derecha);
                return (double)izquierda / (double)derecha;
            case TokenType.Asterizco:
                comprobarNumero(obj.operador, izquierda, derecha);
                return (double)izquierda * (double)derecha;
        }

        return null;        
    }

    // Expresion Literal
    public object visitarExpresionLiteral(Expresion.ExpresionLiteral obj)
    {
        return obj.valor;
    }

    // Expresion Logica
    public object visitarExpresionLogica(Expresion.ExpresionLogica obj)
    {
        object izquierda = evaluar(obj.izquierda);

    if (obj.operador.Tipo == TokenType.Or) {
      if (esVerdad(izquierda)) return izquierda;
    } else {
      if (!esVerdad(izquierda)) return izquierda;
    }

    return evaluar(obj.derecha);
    }

    // Expresion Unaria
    public object visitarExpresionUnaria(Expresion.ExpresionUnaria obj)
    {
        object derecha = evaluar(obj.derecha);
        switch (obj.operador.Tipo) 
        { 
            case TokenType.Bang:
            return !esVerdad(derecha);        
            case TokenType.Menos:
            return -(double)derecha;    
        }
        return null;    
    }

    // Expresion Variable
    public object visitarExpresionVariable(Expresion.ExpresionVariable obj)
    {
        return entorno.Get(obj.nombre);    
    }

    public object visitarGetExpresion(Expresion.GetExpresion obj)
    {
        throw new NotImplementedException();
    }

    public object visitarLlamarExpresion(Expresion.LlamarExpresion obj)
    {
        
        object destinatario = evaluar(obj.nombre);

        List<object> argumentos = new List<object>();
        foreach (Expresion argumento in obj.argumentos) { 
        argumentos.Add(evaluar(argumento));
        }
        if (!(destinatario is IInvocable)) {
        throw new RuntimeError(obj.parentesis,"Solo puede llamar a funciones y clases");
        }

        IInvocable funcion = (IInvocable)destinatario;
        if (argumentos.Count != funcion.aridad()) {
        throw new RuntimeError(obj.parentesis, "Expected " + funcion.aridad() + " arguments but got " + argumentos.Count + ".");
    }

        return funcion.call(this, argumentos);
  
    }

    public object visitarSetExpresion(Expresion.SetExpresion obj)
    {
        throw new NotImplementedException();
    }

    public object visitarSuperExpresion(Expresion.SuperExpresion obj)
    {
        throw new NotImplementedException();
    }

    public void interpretar(List<Declaracion> declaraciones) 
    { 
        try 
        {
            foreach (Declaracion declaracion in declaraciones)
             {
                Ejecutar(declaracion); 
            }       
        } 
        catch (RuntimeError error) 
        {
            Inicio.runtimeError(error);
        }
    }

    private object evaluar(Expresion obj) //Llama al método Aceptar de una expresión, que a su vez llama al método visitante correspondiente en el intérprete
    { 
        return obj.Aceptar(this);
    }

    private bool esVerdad(object obj) //Determina si un objeto se evalúa como verdadero
    { 
        if (obj == null) return false;
        if (obj is bool boleanValue) return (bool)obj; 
        return true;
    }

    private bool esIgual(object a, object b) 
    {
        if (a == null && b == null) return true;
        if (a == null) return false;

        return a.Equals(b);
    }

    private void comprobarNumero(Token operador, object operando) //Verifica que un operando sea un número
    {
        if (operando is double) return;
        throw new RuntimeError(operador, "El operando debe ser un número.");
    }

    private void comprobarNumero(Token operador, object izquierda, object derecha) //Verifica que un operando sea un número
    {
        if (izquierda is double && derecha is double) return;
        throw new RuntimeError(operador, "El operando debe ser un número.");
    }

    private string encadenar(object obj) 
    { 
        if (obj == null) return "null";
        if (obj is double) 
        { 
            string texto = obj.ToString(); 
            if (texto.EndsWith(".0")) 
            {
                texto = texto.Substring(0, texto.Length - 2);
            }
            return texto;

        }
        return obj.ToString();
    }

    private void Ejecutar(Declaracion decl)
    {
        decl.Aceptar(this);
    }
    public object VisitarBloqueDecl(Declaracion.Bloque Decl)
    {
        ejecutarBloque(Decl.declaraciones, new Entorno(entorno));
        return null;
    }

    private void ejecutarBloque(List<Declaracion> declaraciones, Entorno entorno) 
    {
        Entorno anterior = this.entorno;
        try 
        {
            this.entorno = entorno;

            foreach (Declaracion declaracion in declaraciones) 
            {
                Ejecutar(declaracion);
            }
        } 
        finally 
        {
            this.entorno = anterior;
        }

    }

    public object VisitarClassDecl(Declaracion.Class Decl)
    {
        throw new NotImplementedException();
    }

    public object VisitarExpresionDecl(Declaracion.Expression Decl)
    {
        evaluar(Decl.ExpressionValue);
        return null;

    }

    public object VisitarFuncionDecl(Declaracion.Funcion Decl)
    {
        throw new NotImplementedException();
    }

    public object VisitarIfDecl(Declaracion.If Decl)
    {
        if (esVerdad(evaluar(Decl.Condicion))) 
        {
            Ejecutar(Decl.Ejecucion);
        } else if (Decl.Else != null) 
        {
            Ejecutar(Decl.Else);
        }
        return null;

    }

    public object VisitarPrintDecl(Declaracion.Print Decl)
    {
        object valor = evaluar(Decl.Expresion);
        Console.WriteLine(encadenar(valor));
        return null;

    }

    public object VisitarReturnDecl(Declaracion.Return Decl)
    {
        throw new NotImplementedException();
    }

    public object VisitarVarDecl(Declaracion.Var Decl)
    {
        object valor = null;
        if (Decl.Inicializador != null) 
        {
            valor = evaluar(Decl.Inicializador);
        }

        entorno.define(Decl.Nombre.Valor, valor);
        return null;

    }

    public object VisitarWhileDecl(Declaracion.While Decl)
    {
        while (esVerdad(evaluar(Decl.Condicion))) 
        {
            Ejecutar(Decl.Cuerpo);
        }
        return null;
    }

    
}