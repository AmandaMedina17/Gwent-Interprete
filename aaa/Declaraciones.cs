public abstract class Declaracion : claseMadre
{
    
    // public new interface IVisitor<T>
    // {
    //     T VisitarForDecl(For Decl);
    //     T VisitarBloqueDecl(Bloque Decl);
    //     T VisitarExpresionDecl(Expression Decl);
    //     T VisitarFuncionDecl(Funcion Decl);
    //     T VisitarReturnDecl(Return Decl);
    //     T VisitarVarDecl(Var Decl);
    //     T VisitarWhileDecl(While Decl);
    //     T VisitarIncYDec(IncYDec Decl);
    // }

    public class IncYDec : Declaracion
    {
        public Token var;
        public Token operador;
        public Expresion valor;
        public Entorno entorno;
        
        public IncYDec(Token var, Entorno entorno = null,Token operador = null, Expresion valor= null)
        {
            this.var = var;
            this.operador = operador;
            this.valor = valor;
            this.Ejecutar();
            this.entorno = entorno == null ? Entorno.Encerrado : entorno;
        }

        public override void Ejecutar()
        {
            try
            {
                switch (operador.Tipo)
                {
                    case TokenType.Igual:
                        entorno.Set(var, valor);
                        break;
                    case TokenType.Aumentar:
                        entorno.Set(var, new Expresion.ExpresionBinaria(entorno[var.Valor], valor, new Token(TokenType.Más, "+", null, 0)));
                        break;
                    case TokenType.Disminuir:
                        entorno.Set(var, new Expresion.ExpresionBinaria(entorno[var.Valor], valor,new Token(TokenType.Menos, "-", null, 0)));
                        break;
                    case TokenType.Mas_mas:
                        entorno.Set(var, new Expresion.ExpresionBinaria(entorno[var.Valor], new Expresion.ExpresionLiteral(new Token(TokenType.Número, "1", null, 0)), new Token(TokenType.Más, "+", null, 0)));
                        break;
                    case TokenType.Menos_menos:
                        entorno.Set(var, new Expresion.ExpresionBinaria(entorno[var.Valor], new Expresion.ExpresionLiteral(new Token(TokenType.Número, "1", null, 0)), new Token(TokenType.Menos, "-", null, 0)));
                        break;
                    default:
                        throw new Exception("Declaracion invalida");
                }
            }
            catch (NullReferenceException)
            {
               System.Console.WriteLine("Se accedio al valor");
            }
            
        }

        public Tipo type => valor is null? entorno[var.Valor].type() : valor.type();

        // public override T Aceptar<T>(IVisitor<T> visitante)
        // {
        //     throw new NotImplementedException();
        // }

        public override bool Semantica()
        {        
            if(!(valor is null) && !valor.Semantica())
            {
                return false;
            }
           
            return true;
        }

        public Expresion Evaluar()
        {
            Ejecutar();
            return (Expresion)entorno.Valores[var.Valor];
        }

       
    }
    public class Bloque : Declaracion
    {
        public List<claseMadre> declaraciones { get; }

        public Bloque(List<claseMadre> declaraciones)
        {
            this.declaraciones = declaraciones;
        }

        // public override T Aceptar<T>(IVisitor<T> visitante)
        // {
        //     return visitante.VisitarBloqueDecl(this);
        // }

        

        public override bool Semantica()
        {
            bool noHayErrores = true;
            foreach(var var in declaraciones)
            {
                try{
                    if(!var.Semantica()) 
                    {
                        noHayErrores = false;
                        throw new Exception("Invalid declaration contained in the block.");
                    }
                }
                catch{

                }
            }

            return noHayErrores;
        }

        public override void Ejecutar()
        {
            foreach(var objecto in declaraciones)
            {
                objecto.Ejecutar();
            }
        }
    }

    public class Expression : Declaracion
    {
        public Expresion ExpressionValue { get; }

        public Expression(Expresion expression)
        {
            ExpressionValue = expression;
        }

        // public override T Aceptar<T>(IVisitor<T> visitante)
        // {
        //     return visitante.VisitarExpresionDecl(this);
        // }

        public override bool Semantica()
        {
            bool noHayErrores = true; 
            if(!ExpressionValue.Semantica()) 
            {
                noHayErrores = false;
                throw new Exception("The expression is invalid.");
            }
            return noHayErrores;
        }

        public override void Ejecutar()
        {
            ExpressionValue.Ejecutar();
        }
    }
    public class For : Declaracion
    {
        Entorno entorno;
        public Token var;
        public Expresion Colection;
        public Bloque body;
        public For(Token var, Expresion Colection, Bloque body)
        {
            this.var = var;
            this.body = body;
            this.Colection = Colection;
        }

        public override void Ejecutar()
        {
            IEnumerator<object> colection;
            try
            {
                colection = ((IEnumerable<object>)Colection.Ejecutar()).GetEnumerator();
            }
            catch
            {
                throw new Exception("error");
            }

            if (entorno.Contains(var.Valor)) throw new Exception("Variable already defined previously.");

            while (colection.MoveNext())
            {
                entorno.Get(var);
                if(body is Bloque) body.Ejecutar();
            }

        }

        // public override T Aceptar<T>(IVisitor<T> visitante)
        // {
        //     return visitante.VisitarForDecl(this);        
        // }

        public override bool Semantica()
        {
            //Me falta comprobar colection
            bool noHayErrores = true;
            noHayErrores = body.Semantica();

            return noHayErrores;
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

        // public override T Aceptar<T>(IVisitor<T> visitante)
        // {
        //     return visitante.VisitarFuncionDecl(this);
        // }



        public override bool Semantica()
        {
            throw new NotImplementedException();
        }

        public override void Ejecutar()
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

        // public override T Aceptar<T>(IVisitor<T> visitante)
        // {
        //     return visitante.VisitarReturnDecl(this);
        // }

        public override bool Semantica()
        {
            throw new NotImplementedException();
        }

        public override void Ejecutar()
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

        // public override T Aceptar<T>(IVisitor<T> visitante)
        // {
        //     return visitante.VisitarVarDecl(this);
        // }


        public override bool Semantica()
        {
            

            if(!(Inicializador is null) && !Inicializador.Semantica())
            {
                return false;
            }
           
            return true;
        }

        public override void Ejecutar()
        {
            try{
                entorno.Set(Nombre, Inicializador);
            }
            catch
            {
                throw new Exception("valor mal");
            }
            // object valor = null;
            // if (Inicializador != null) 
            // {
            //     valor = Inicializador.Ejecutar();
            // }

            // entorno.define(Nombre.Valor, (Expresion)valor);
        }

        public Token Nombre { get; }
        public Expresion Inicializador { get; }
        Entorno entorno;
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

        // public override T Aceptar<T>(IVisitor<T> visitante)
        // {
        //     return visitante.VisitarWhileDecl(this);
        // }


        public override bool Semantica()
        {
            bool noHayErrores = true;

            if(!(Condicion.type() is Tipo.Bool) || !Condicion.Semantica())
            {
                System.Console.WriteLine("Invalid while condition.");
                return false;
            }
            noHayErrores = Cuerpo.Semantica();

            

            return noHayErrores;
        }

        public override void Ejecutar()
        {
            while (esVerdad(Condicion.Ejecutar())) 
        {
            Cuerpo.Ejecutar();
        }
        }

         private bool esVerdad(object obj) //Determina si un objeto se evalúa como verdadero
        { 
            if (obj == null) return false;
            if (obj is bool boleanValue) return (bool)obj; 
            return true;
        }
    }

    public class ActivacionEfecto : Declaracion
    {
        Expresion Nombre;
        List<(Token, Expresion)> parametros;
        Expresion selector;
        Effect efecto;
        
        public ActivacionEfecto(Expresion Nombre, List<(Token, Expresion)> parametros, Expresion selector)
        {
            this.Nombre = Nombre;
            this.parametros = parametros;
            this.selector = selector;
        }
        public override void Ejecutar()
        {
            efecto.Ejecutar();
        }

        public override bool Semantica()
        {
            if(!Nombre.Semantica()) return false;
            if (Nombre.type() != Tipo.Cadena) 
            {
                System.Console.WriteLine("Nombre del efecto invalido");
                return false;
            }
            if (!selector.Semantica()) return false;
            if (!(selector is null) && selector.type() != Tipo.Lista)
            {
                System.Console.WriteLine("Selector invalido");
                return false;
            }

            foreach (var item in parametros)
            {
                if(!item.Item2.Semantica())
                {
                    System.Console.WriteLine(item.Item1.Valor + "invalido");
                    return false;
                }
            
            }

                try
                {
                    efecto = Effect.efectosGuardados[(string)Nombre.Ejecutar()];
                    efecto.TargetsAndParametros(parametros, selector); 
                }
                catch (KeyNotFoundException)
                {
                    return false;
                }

            return true;
        }
    }


    // public abstract T Aceptar<T>(IVisitor<T> visitante);

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
