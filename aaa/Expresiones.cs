using System.Linq.Expressions;

public abstract class Expresion
{
    public interface IVisitante<T>
    {
        T visitarAsignacionExpresion(AsignarExpresion obj);
        T visitarExpresionBinaria(ExpresionBinaria obj);
        T visitarLlamarExpresion(LlamarExpresion obj);
        T visitarGetExpresion(GetExpresion obj);
        T visitarExpresionAgrupacion(ExpresionAgrupacion obj);
        T visitarExpresionLiteral(ExpresionLiteral obj);
        T visitarExpresionLogica(ExpresionLogica obj);
        T visitarSetExpresion(SetExpresion obj);
        T visitarSuperExpresion(SuperExpresion obj);
        T visitarExpresionUnaria(ExpresionUnaria obj);
        T visitarExpresionVariable(ExpresionVariable obj);

    }

    public abstract T Aceptar<T>(IVisitante<T> visitante);

    public class AsignarExpresion : Expresion
    {
        public Token nombre;
        public Expresion valor;

        public AsignarExpresion(Token nombre, Expresion valor)
        {
            this.nombre = nombre;
            this.valor = valor;
        }

        public override T Aceptar<T>(IVisitante<T> visitante)
        {
            return visitante.visitarAsignacionExpresion(this);
        }
    }

    public class ExpresionBinaria : Expresion
    {
        public Expresion izquierda;
        public Expresion derecha;
        public Token operador;

        public ExpresionBinaria(Expresion izquierda, Expresion derecha, Token operador)
        {
            this.izquierda = izquierda;
            this.derecha = derecha;
            this.operador = operador;
        }
        
        public override T Aceptar<T>(IVisitante<T> visitante)
        {
            return visitante.visitarExpresionBinaria(this);
        }
    }

    public class LlamarExpresion : Expresion
    {
        public Expresion nombre;
        public Token parentesis;
        public List<Expresion> argumentos;

        public LlamarExpresion(Expresion nombre, Token parentesis, List<Expresion> argumentos)
        {
            this.nombre = nombre;
            this.argumentos = argumentos;
            this.parentesis = parentesis;
        }

        public override T Aceptar<T>(IVisitante<T> visitante)
        {
            return visitante.visitarLlamarExpresion(this);
        }
    }

    //Obtener expresion
    public class GetExpresion : Expresion
    {
        public Expresion objeto;
        public Token name;

        public GetExpresion(Expresion objeto, Token name)
        {
            this.name = name;
            this.objeto = objeto;
        }

        public override T Aceptar<T>(IVisitante<T> visitante)
        {
            return visitante.visitarGetExpresion(this);
        }
    }

    //Expresion de agrupacion
    public class ExpresionAgrupacion : Expresion
    {
        public Expresion expresion;

        public ExpresionAgrupacion(Expresion expresion)
        {
            this.expresion = expresion;
        }

        public override T Aceptar<T>(IVisitante<T> visitante)
        {
            return visitante.visitarExpresionAgrupacion(this);
        }
    }

    //Expresion Literal
    public class ExpresionLiteral : Expresion
    {
        public object valor;

        public ExpresionLiteral(object valor)
        {
            this.valor = valor;
        
        }

        public override T Aceptar<T>(IVisitante<T> visitante)
        {
            return visitante.visitarExpresionLiteral(this);
        }
    }

    //Expresion Logica
    public class ExpresionLogica : Expresion
    {
        public Expresion izquierda;
        public Expresion derecha;
        public Token operador;

        public ExpresionLogica(Expresion izquierda, Token operador, Expresion derecha)
        {
            this.izquierda = izquierda;
            this.derecha = derecha;
            this.operador = operador;
        }

        public override T Aceptar<T>(IVisitante<T> visitante)
        {
            return visitante.visitarExpresionLogica(this);
        }
    }

    //Fijar Expresion
    public class SetExpresion : Expresion
    {
        public Expresion objeto;
        public Token name;
        public Expresion valor;

        public SetExpresion(Expresion objeto, Token name, Expresion valor)
        {
            this.name = name;
            this.objeto = objeto;
            this.valor = valor;
        }

        public override T Aceptar<T>(IVisitante<T> visitante)
        {
            return visitante.visitarSetExpresion(this);
        }
    }

    //Super Expresion
    public class SuperExpresion : Expresion
    {
        public Token palabraReservada;
        public Token metodo;

        public SuperExpresion(Token palabraReservada, Token metodo)
        {
            this.palabraReservada = palabraReservada;
            this.metodo = metodo;
        }

        public override T Aceptar<T>(IVisitante<T> visitante)
        {
            return visitante.visitarSuperExpresion(this);
        }
    }

    //Esta Expresion
    public class ExpresionUnaria : Expresion
    {
        public Token operador;
        public Expresion derecha;

        public ExpresionUnaria(Token operador, Expresion derecha)
        {
            this.operador = operador;
            this.derecha = derecha;
        }

        public override T Aceptar<T>(IVisitante<T> visitante)
        {
            return visitante.visitarExpresionUnaria(this);
        }
    }

    //Expresion Variable
    public class ExpresionVariable : Expresion
    {
        public Token nombre;

        public ExpresionVariable(Token nombre)
        {
            this.nombre = nombre;
        }

        public override T Aceptar<T>(IVisitante<T> visitante)
        {
            return visitante.visitarExpresionVariable(this);
        }
    }


}



