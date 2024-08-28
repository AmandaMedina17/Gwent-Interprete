using System.Linq.Expressions;
using System.Reflection;

public abstract class Expresion
{
    public abstract object Ejecutar();
    public abstract Tipo type();
    public abstract bool Semantica();

    public class AsignarExpresion : Expresion
    {      
        public Token nombre;
        public Expresion valor;

        public AsignarExpresion(Token nombre, Expresion valor)
        {
            this.nombre = nombre;
            this.valor = valor;
        }

        public override object Ejecutar()
        {
            object expresionValor = valor.Ejecutar();
            Entorno.Encerrado.asignar(nombre, valor);
            return valor;    
        }

        public override bool Semantica()
        {
            if(!(valor is null) && !valor.Semantica())
            {
                return false;
            }
           
            return true;
        }

        public override Tipo type()
        {
            return valor.type();
        }
    }

    public class ExpresionBinaria : Expresion
    {
        static List<string> operacionesDisponibles = new List<string> { "+", "-", "*", "/", "<", ">", "<=", ">=", "==", "!=", "@", "@@"};
        public Expresion izquierda;
        public Expresion derecha;
        public Token operador;

        public ExpresionBinaria(Expresion izquierda, Expresion derecha, Token operador)
        {
            this.izquierda = izquierda;
            this.derecha = derecha;
            this.operador = operador;
        }
        public override object Ejecutar()
        {
            object expresionIzquierda = izquierda.Ejecutar();
            object expresionDerecha = derecha.Ejecutar(); 

            switch (operador.Tipo) 
            {
                case TokenType.Más:
                    if (expresionIzquierda is double && expresionDerecha is double) 
                    {
                        return (double)expresionIzquierda + (double)expresionDerecha;
                    } 
                    else throw new Exception("The operands must be numbers.");
                case TokenType.Menos:
                    comprobarNumero(operador, expresionIzquierda, expresionDerecha);
                    return (double)expresionIzquierda - (double)expresionDerecha;
                case TokenType.Asterizco:
                    comprobarNumero(operador, expresionIzquierda, expresionDerecha);
                    return (double)expresionIzquierda * (double)expresionDerecha;
                case TokenType.Slach:
                    comprobarNumero(operador, expresionIzquierda, expresionDerecha);
                    return (double)expresionIzquierda / (double)expresionDerecha;
                case TokenType.Mayor:
                    comprobarNumero(operador, expresionIzquierda, expresionDerecha);
                    return (double)expresionIzquierda > (double)expresionDerecha;
                case TokenType.Mayor_igual:
                    comprobarNumero(operador, expresionIzquierda, expresionDerecha);
                    return (double)expresionIzquierda >= (double)expresionDerecha;
                case TokenType.Menor:
                    comprobarNumero(operador, expresionIzquierda, expresionDerecha);
                    return (double)expresionIzquierda < (double)expresionDerecha;
                case TokenType.Menor_igual:
                    comprobarNumero(operador, expresionIzquierda, expresionDerecha);
                    return (double)expresionIzquierda <= (double)expresionDerecha;
                case TokenType.Concatenacion:
                    return expresionIzquierda.ToString() + expresionDerecha.ToString();
                case TokenType.Concatenacion_Espaciado:
                    return expresionIzquierda.ToString() + " " + expresionDerecha.ToString();
                case TokenType.Bang_igual: 
                    return !esIgual(expresionIzquierda, expresionDerecha);
                case TokenType.Igual_igual: 
                    return esIgual(expresionIzquierda, expresionDerecha);
                default: return null;
                
            }

        }

        public override bool Semantica()
        {
            bool noHayErrores = true;

            if(!operacionesDisponibles.Contains(operador.Valor))
            {
                noHayErrores = false;
                System.Console.WriteLine("Invalid binary expression operator.");
            }
            if((!(izquierda.type() is Tipo.Numero) && !(derecha.type() is Tipo.Numero)) || (!(izquierda.type() is Tipo.Cadena) && !(derecha.type() is Tipo.Cadena)) || (!(izquierda.type() is Tipo.Bool) && !(derecha.type() is Tipo.Bool)))
            {
                noHayErrores = false;
                System.Console.WriteLine("Expresion derecha e izquierda no coincidentes");
            }

            return noHayErrores;
        }

        public override Tipo type()
        {
            if((izquierda.type() is Tipo.Numero) && (derecha.type() is Tipo.Numero)) 
            {
                if(operador.Tipo == TokenType.Más || operador.Tipo == TokenType.Menos || operador.Tipo == TokenType.Slach || operador.Tipo == TokenType.Asterizco)return Tipo.Numero;
                else return Tipo.Bool;
            }
            else if((izquierda.type() is Tipo.Cadena) && (derecha.type() is Tipo.Cadena)) return Tipo.Cadena;
            else if((izquierda.type() is Tipo.Bool) && (derecha.type() is Tipo.Bool)) return Tipo.Bool;
            else throw new Exception("Expresion binaria mal");
        }

        private void comprobarNumero(Token operador, object izquierda, object derecha) //Verifica que un operando sea un número
        {
            if (izquierda is double && derecha is double) return;
            throw new RuntimeError(operador, "El operando debe ser un número.");
        }

        private bool esIgual(object a, object b) 
        {
            if (a == null && b == null) return true;
            if (a == null) return false;

            return a.Equals(b);
        }
    }

    public class Metodo : Expresion
    {
        public Expresion callee;
        public Token caller;
        public List<Expresion> argumentos;

        public Metodo(Expresion callee, Token caller, List<Expresion> argumentos = null)
        {
            this.callee = callee;
            this.argumentos = argumentos;
            this.caller = caller;
        }

        public override object Ejecutar()
        {
            object callee = this.callee.Ejecutar();

            Type type;
            if (callee is List<BaseCard>) type = typeof(List<BaseCard>);
            else if (callee is Card) type = typeof(Card);
            else if (callee is string) type = typeof(string);
            else if (callee is double) type = typeof(double);
            else type = typeof(object);

            MethodInfo method = null;

            try
            {
                method = type.GetMethod(caller.Valor);
            }
            catch (AmbiguousMatchException)
            {
                method = type.GetMethod(caller.Valor, new Type[0]);
            }

            if (method != null)
            {
                try
                {
                    object[] argumentosArray = argumentos.ToArray();
                    return method.Invoke(callee, argumentosArray);
                }
                catch (IndexOutOfRangeException)
                {
                    throw new Exception("Argumentos invalidos");
                }
            }
            else throw new Exception("Metodo no encontrado");
        }

    

        public override bool Semantica()
        {
            return true;
        }

        public override Tipo type()
        {
            throw new NotImplementedException();
        }
    }

    //Obtener expresion
    public class Propiedad : Expresion
    {
        public Expresion objeto;
        public Token name;

        public Propiedad(Expresion objeto, Token name)
        {
            this.name = name;
            this.objeto = objeto;
        }

        public override object Ejecutar()
        {
            object callee = objeto.Ejecutar();

            Type type;
            if (callee is List<BaseCard>) type = typeof(List<BaseCard>);
            else if (callee is BaseCard) type = typeof(BaseCard);
            else if (callee is string) type = typeof(string);
            else if (callee is double) type = typeof(double);
            else type = typeof(object);

            if (type.GetProperty(name.Valor) != null)
            {
                return type.GetProperty(name.Valor).GetValue(callee);
            }
            else throw new Exception("Propiedad no encontrada");
        }
        

        public override bool Semantica()
        {
            return true;
        }

        public override Tipo type()
        {
            throw new NotImplementedException();
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

        public override object Ejecutar()
        {
            return expresion.Ejecutar();
        }

        public override bool Semantica()
        {
            return true;
        }

        public override Tipo type()
        {
            return expresion.type();
        }
    }

    //Expresion Literal
    public class ExpresionLiteral : Expresion
    {
        public Token valor;
        public Tipo Type;

        public ExpresionLiteral(Token valor)
        {
            this.valor = valor;
            Type = type();
        
        }

        public override bool Semantica()
        {
            return true;
        }

        public override Tipo type()
        {
            switch(valor.Tipo)
            {
                case TokenType.Número: return Tipo.Numero;
                case TokenType.Cadena: return Tipo.Cadena;
                case TokenType.True: return Tipo.Bool;
                case TokenType.False: return Tipo.Bool;
                default: return Tipo.Object;
            }
        }

        public override object Ejecutar()
        {
           switch(valor.Tipo)
            {
                case TokenType.Número: return Convert.ToDouble(valor);
                case TokenType.Cadena: return valor.Valor.Substring(1, valor.Valor.Length - 2);
                case TokenType.True : return true;
                case TokenType.False: return false;
                default: throw new Exception("Invalid literal expression value");
            }
        }
    }

    //Expresion Logica
    public class ExpresionLogica : Expresion
    {
        static List<string> operacionesDisponibles = new List<string> {"||", "&&"};
        public Expresion izquierda;
        public Expresion derecha;
        public Token operador;

        public ExpresionLogica(Expresion izquierda, Token operador, Expresion derecha)
        {
            this.izquierda = izquierda;
            this.derecha = derecha;
            this.operador = operador;
        }

        public override object Ejecutar()
        {
            object expresionIzquierda = izquierda.Ejecutar();
            object expresionDerecha = derecha.Ejecutar(); 

            switch(operador.Tipo)
            {
                case TokenType.And:
                    if(expresionIzquierda is Boolean && expresionDerecha is Boolean)
                    {
                        return (bool)expresionIzquierda && (bool)expresionDerecha;
                    }
                    else throw new Exception("Operands must be boolean.");
                case TokenType.Or:
                    if(expresionIzquierda is Boolean && expresionDerecha is Boolean)
                    {
                        return (bool)expresionIzquierda || (bool)expresionDerecha;
                    }
                    else throw new Exception("Operands must be boolean.");
                default: return null;
            }
        }

        public override bool Semantica()
        {
            bool noHayErrores = true;

            if(!operacionesDisponibles.Contains(operador.Valor))
            {
                System.Console.WriteLine("Invalid logical expression operator.");
                noHayErrores = false;
            }
            if(!(izquierda.type() == Tipo.Bool) && !(derecha.type() == Tipo.Bool)) noHayErrores = false;
            return noHayErrores;
        }

        public override Tipo type()
        {
            if((izquierda.type() is Tipo.Bool) && (derecha.type() is Tipo.Bool)) return Tipo.Bool;
            else throw new Exception("The left and right expression are not both of type bool.");
        }
    }

    public class Indexador : Expresion
    {
        Expresion Collection;
        Expresion indice;

        public Indexador(Expresion Collection, Expresion indice)
        {
            this.Collection = Collection;
            this.indice = indice;
        }
        public override object Ejecutar()
        {
            
            List<BaseCard> collection = (List<BaseCard>)Collection.Ejecutar();
            int intIndice = (int)indice.Ejecutar();
            return collection[intIndice];

        }

        public override bool Semantica()
        {
            throw new NotImplementedException();
        }

        public override Tipo type()
        {
            throw new NotImplementedException();
        }
    }
    public class Objeto : Expresion
    {
        public object valor;

        public Objeto(object valor)
        {
            this.valor = valor;
        }
      
        public override object Ejecutar()
        {
            return valor;
        }

        public override bool Semantica()
        {
            throw new NotImplementedException();
        }

        public override Tipo type()
        {
            if(valor is int || valor is double) return Tipo.Numero;
            if(valor is string) return Tipo.Cadena;
            if(valor is Card) return Tipo.Card;
            else return Tipo.Object;
        }
    }


    //Expresion Unaria
    public class ExpresionUnaria : Expresion
    {
        static List<string> operadoresDisponibles = new() { "-", "!"};
        public Token operador;
        public Expresion derecha;

        public ExpresionUnaria(Token operador, Expresion derecha)
        {
            this.operador = operador;
            this.derecha = derecha;
        }

        public override object Ejecutar()
        {
            object expresion = derecha.Ejecutar();
            switch(operador.Tipo)
            {
                case TokenType.Bang: return !esVerdad(derecha);
                case TokenType.Menos: return -(double)expresion;
                default: throw new Exception("Invalid unary expression operator.");
            }
        }

        public override bool Semantica()
        {
            bool noHayErrores = true;

            if(!(derecha.type() is Tipo.Bool) && !(derecha.type() is Tipo.Numero))
            {
                System.Console.WriteLine("Expresion derecha de tipo no esperado");
                noHayErrores = false;
            }

            if(!operadoresDisponibles.Contains(operador.Valor)) 
            {
                System.Console.WriteLine("Operador unario invalido");
                noHayErrores = false;
            }

            return noHayErrores;

        }

        public override Tipo type()
        {
            return derecha.type();
        }

        private bool esVerdad(object obj) //Determina si un objeto se evalúa como verdadero
        { 
            if (obj == null) return false;
            if (obj is bool boleanValue) return (bool)obj; 
            return true;
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

        public override object Ejecutar()
        {
            return Entorno.Encerrado.Get(nombre);    
        }

        public override bool Semantica()
        {
            return true;
        }

        public override Tipo type()
        {
            if(Entorno.Encerrado.Valores[nombre.Valor] is string) return Tipo.Cadena;
            if(Entorno.Encerrado.Valores[nombre.Valor] is double) return Tipo.Numero;
            if(Entorno.Encerrado.Valores[nombre.Valor] is bool) return Tipo.Bool;
            throw new Exception("tipo invalido");

        }
    }

    public class Predicate : Expresion
    {
        public Token var;
        public Expresion condicion;
        Entorno entorno;

        public Predicate(Token var, Expresion condicion, Entorno entorno)
        {
            this.var = var;
            this.condicion = condicion;
            this.entorno = entorno;
        }
        public override object Ejecutar()
        {
            return new Predicate<Card>(EvaluarCarta);
        }

        private bool EvaluarCarta(Card card)
        {
            entorno.asignar(var, new Objeto(card));
            return (bool)condicion.Ejecutar();
        }

        public override bool Semantica()
        {
            if(!(condicion.type() is Tipo.Bool)) return false;
            return true;
        }

        public override Tipo type()
        {
            return Tipo.Predicate;
        }
    }

    public class DeclVar : Expresion
    {
        public Declaracion.IncYDec valor;

        public DeclVar(Declaracion.IncYDec valor)
        {
            this.valor = valor;
        }

        public override object Ejecutar()
        {
            return valor.Evaluar().Ejecutar();
        }

        public override bool Semantica()
        {
            throw new NotImplementedException();
        }

        public override Tipo type()
        {
            throw new NotImplementedException();
        }
    }
}





