public abstract class Declaracion : claseMadre
{
    public new interface IVisitor<T>
    {
        T VisitarBloqueDecl(Bloque Decl);
        T VisitarClassDecl(Class Decl);
        T VisitarExpresionDecl(Expression Decl);
        T VisitarFuncionDecl(Funcion Decl);
        T VisitarIfDecl(If Decl);
        T VisitarPrintDecl(Print Decl);
        T VisitarReturnDecl(Return Decl);
        T VisitarVarDecl(Var Decl);
        T VisitarWhileDecl(While Decl);
    }


    public class Bloque : Declaracion
    {
        public List<claseMadre> declaraciones { get; }

        public Bloque(List<claseMadre> declaraciones)
        {
            this.declaraciones = declaraciones;
        }

        public override T Aceptar<T>(IVisitor<T> visitante)
        {
            return visitante.VisitarBloqueDecl(this);
        }

        internal override void Aceptar(Interprete interprete)
        {
            throw new NotImplementedException();
        }
    }

    public class Class : Declaracion
    {
        public Token Nombre { get; }
        public Expresion.ExpresionVariable Superclass { get; }
        public List<Declaracion.Funcion> Metodos { get; }
        public Class(Token Nombre, Expresion.ExpresionVariable Superclass, List<Declaracion.Funcion> Metodos)
        {
            this.Nombre = Nombre;
            this.Superclass = Superclass;
            this.Metodos = Metodos;
        }

        public override T Aceptar<T>(IVisitor<T> visitante)
        {
            return visitante.VisitarClassDecl(this);
        }

        internal override void Aceptar(Interprete interprete)
        {
            throw new NotImplementedException();
        }
    }

    public class Expression : Declaracion
    {
        public Expresion ExpressionValue { get; }

        public Expression(Expresion expression)
        {
            ExpressionValue = expression;
        }

        public override T Aceptar<T>(IVisitor<T> visitante)
        {
            return visitante.VisitarExpresionDecl(this);
        }

        internal override void Aceptar(Interprete interprete)
        {
            throw new NotImplementedException();
        }
    }

    public class Funcion : Declaracion
    {
        public Token Nombre { get; }
        public List<Token> Parametros { get; }
        public List<Declaracion> Cuerpo { get; }
        public Funcion(Token Nombre, List<Token> Parametros, List<Declaracion> Cuerpo)
        {
            this.Nombre = Nombre;
            this.Parametros = Parametros;
            this.Cuerpo = Cuerpo;
        }

        public override T Aceptar<T>(IVisitor<T> visitante)
        {
            return visitante.VisitarFuncionDecl(this);
        }

        internal override void Aceptar(Interprete interprete)
        {
            throw new NotImplementedException();
        }
    }

    public class If : Declaracion
    {
        public Expresion Condicion { get; }
        public Declaracion Ejecucion { get; }
        public Declaracion Else { get; }
        public If(Expresion Condicion, Declaracion Ejecucion, Declaracion Else)
        {
            this.Condicion = Condicion;
            this.Ejecucion = Ejecucion;
            this.Else = Else;
        }

        public override T Aceptar<T>(IVisitor<T> visitante)
        {
            return visitante.VisitarIfDecl(this);
        }

        internal override void Aceptar(Interprete interprete)
        {
            throw new NotImplementedException();
        }
    }

    public class Print : Declaracion
    {
        public Expresion Expresion { get; }

        public Print(Expresion Expresion)
        {
            this.Expresion = Expresion;
        }

        public override T Aceptar<T>(IVisitor<T> visitante)
        {
            return visitante.VisitarPrintDecl(this);
        }

        internal override void Aceptar(Interprete interprete)
        {
            throw new NotImplementedException();
        }
    }

    public class Return : Declaracion
    {
        public Token palabraReservada { get; }
        public Expresion Valor { get; }
        public Return(Token palabraReservada, Expresion Valor)
        {
            this.palabraReservada = palabraReservada;
            this.Valor = Valor;
        }

        public override T Aceptar<T>(IVisitor<T> visitante)
        {
            return visitante.VisitarReturnDecl(this);
        }

        internal override void Aceptar(Interprete interprete)
        {
            throw new NotImplementedException();
        }
    }

    public class Var : Declaracion
    {
        public Var(Token Nombre, Expresion Inicializador)
        {
            this.Nombre = Nombre;
            this.Inicializador = Inicializador;
        }

        public override T Aceptar<T>(IVisitor<T> visitante)
        {
            return visitante.VisitarVarDecl(this);
        }

        internal override void Aceptar(Interprete interprete)
        {
            throw new NotImplementedException();
        }

        public Token Nombre { get; }
        public Expresion Inicializador { get; }
    }

    public class While : Declaracion
    {
        public Expresion Condicion { get; }
        public Declaracion Cuerpo { get; }
        public While(Expresion Condicion, Declaracion Cuerpo)
        {
            this.Condicion = Condicion;
            this.Cuerpo = Cuerpo;
        }

        public override T Aceptar<T>(IVisitor<T> visitante)
        {
            return visitante.VisitarWhileDecl(this);
        }

        internal override void Aceptar(Interprete interprete)
        {
            throw new NotImplementedException();
        }
    }


    public abstract T Aceptar<T>(IVisitor<T> visitante);
}
