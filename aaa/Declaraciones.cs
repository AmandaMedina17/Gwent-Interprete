public abstract class Declaracion : claseMadre
{
    public new interface IVisitor<T>
    {
        T VisitarForDecl(For Decl);
        T VisitarBloqueDecl(Bloque Decl);
        T VisitarExpresionDecl(Expression Decl);
        T VisitarFuncionDecl(Funcion Decl);
        T VisitarReturnDecl(Return Decl);
        T VisitarVarDecl(Var Decl);
        T VisitarWhileDecl(While Decl);
        T VisitarIncYDec(IncYDec Decl);
    }



    public class IncYDec : Declaracion
    {
        public Token var;
        public Token operador;
        public Expresion valor;
        
        public IncYDec(Token var, Token operador, Expresion valor)
        {
            this.var = var;
            this.operador = operador;
            this.valor = valor;
        }
        public override T Aceptar<T>(IVisitor<T> visitante)
        {
            throw new NotImplementedException();
        }

        public override bool Semantica()
        {
            if(!(valor is null) && !valor.Semantica())
            {
                return false;
            }
           
            return true;
        }

        internal override void Aceptar(Interprete interprete)
        {
            throw new NotImplementedException();
        }
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

        public override bool Semantica()
        {
            bool noHayErrores = true;
            foreach(var var in declaraciones)
            {
                try{
                    if(!var.Semantica()) 
                    {
                        noHayErrores = false;
                        throw new Exception("Declaracion incorrecta");
                    }
                }
                catch{

                }
            }

            return noHayErrores;
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

        public override bool Semantica()
        {
            bool noHayErrores = true; 
            if(ExpressionValue.Semantica()) 
            {
                noHayErrores = false;
                throw new Exception("La expresion tiene errores");
            }
            return noHayErrores;
        }
    }
    public class For : Declaracion
    {
        public Token var;
        public Expresion Colection;
        public Bloque body;
        public For(Token var, Expresion Colection, Bloque body)
        {
            this.var = var;
            this.body = body;
            this.Colection = Colection;
        }
        public override T Aceptar<T>(IVisitor<T> visitante)
        {
            return visitante.VisitarForDecl(this);        
        }

        public override bool Semantica()
        {
            //Me falta comprobar colection
            bool noHayErrores = true;
            noHayErrores = body.Semantica();

            return noHayErrores;
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

        public override bool Semantica()
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

        public override bool Semantica()
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

        public override bool Semantica()
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

        public override bool Semantica()
        {
            bool hayError = true;

            hayError = Cuerpo.Semantica();

            

            return hayError;
        }
    }


    public abstract T Aceptar<T>(IVisitor<T> visitante);

     /*public class If : Declaracion
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

        public override bool Semantica()
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

        public override bool Semantica()
        {
            throw new NotImplementedException();
        }
    }
*/
}
