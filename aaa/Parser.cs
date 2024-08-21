using System.Linq.Expressions;
using System.Text.Json;
using System.Linq;
using System.Runtime.CompilerServices;

class Parser
{
    public class ParseError : Exception{}  //Para analizar errores durante el analisis
    public List<Token> tokens;  // Una lista que almacena los tokens generados por el escáner
    public int Current = 0;  //Un índice que apunta al token actual durante el análisis
    Token True = new Token(TokenType.True, "true", true, 0);

    public Parser(List<Token> tokens)
    {
        this.tokens = tokens;
    }

    
    public List<claseMadre> parse() //Este método analiza la lista de tokens y genera una lista cartas.
    {
        List<claseMadre> cartasyefectos = new List<claseMadre>();  

        while (!esFinal()) 
        {
            cartasyefectos.Add(declararCartaOEfecto());
        }        
        
        return cartasyefectos;
    }
    

    //expresión → asignación
    public Expresion expresion() 
    {
        return asignacion();
    }

    //asignación → Asignación del IDENTIFICADOR "="| or ;
    public Expresion asignacion() 
    {
        Expresion expr = or();

        if (coincide(TokenType.Igual)) 
        {
            Token igual = anterior();
            Expresion valor = asignacion();

            if (expr is Expresion.ExpresionVariable) {
                Token nombre = ((Expresion.ExpresionVariable)expr).nombre;
                return new Expresion.AsignarExpresion(nombre, valor);
            }

            error(igual, "Invalid assignment target."); 
        }

        return expr;
    }

    //or → and ("o" and ) 
    public Expresion or() 
    {
        Expresion expr = and();

        while (coincide(TokenType.Or)) 
        {
            Token operador = anterior();
            Expresion derecha = and();
            expr = new Expresion.ExpresionLogica(expr, operador, derecha);
        }

        return expr;
    }

    //and → igualdad ("y" igualdad )
    public Expresion and() 
    {
        Expresion expr = igualdad();

        while (coincide(TokenType.And)) 
        {
            Token operador = anterior();
            Expresion derecha = igualdad();
            expr = new Expresion.ExpresionLogica(expr, operador, derecha);
        }

        return expr;
    }

    //igualdad → comparación ( ( "!=" | "==" ) comparación )*
    public Expresion igualdad()
    {
        Expresion expr = comparacion();
        while (coincide(TokenType.Bang_igual, TokenType.Igual_igual)) 
        { 
            Token operador = anterior();
            Expresion derecha = comparacion();
            expr = new Expresion.ExpresionBinaria(expr, derecha, operador);

        }
        return expr;
    }

    public bool coincide(params TokenType[] tipos) //Verifica si el token actual coincide con alguno de los tipos especificados. Si coincide, avanza al siguiente token y devuelve true
    { 
        foreach (TokenType tipo in tipos) {
            if (verifica(tipo)) 
            { 
                avanza(); 
                return true;

            }
        }
        return false;
    }

    public bool verifica(TokenType tipo) //Verifica si el token actual es del tipo especificado, sin avanzar al siguiente token. 
    { 
        if (esFinal()) return false;
        return mira().Tipo == tipo;

    }

    public Token avanza() // Avanza al siguiente token y devuelve el token actual
    {
        if (!esFinal()) Current++; 
        return anterior();

    }

    public bool esFinal() //Verifica si se ha llegado al final de los tokens
    { 
        return mira().Tipo == TokenType.Fin;
    }
    public Token mira() // Devuelve el token actual sin avanzar
    {
        return tokens[Current];
    }
    public Token anterior() // Devuelve el token anterior al actual
    {
        return tokens[Current - 1];
    }

    //comparación → término ( ( ">" | ">=" | "<" | "<=" ) término )*
    public Expresion comparacion()
    {
        Expresion expr = termino();

        while (coincide(TokenType.Mayor, TokenType.Mayor_igual, TokenType.Menor, TokenType.Menor_igual)) 
        {
            Token operador = anterior(); 
            Expresion derecha = termino();
            expr = new Expresion.ExpresionBinaria(expr, derecha, operador);

        }
        return expr;
    }

    //término → factor ( ("-" | "+" ) factor )*
    public Expresion termino() 
    { 
        Expresion expr = factor();
        while (coincide(TokenType.Menos, TokenType.Más, TokenType.Concatenacion, TokenType.Concatenacion_Espaciado)) 
        { 
            Token operador = anterior(); 
            Expresion derecha = factor();
            expr = new Expresion.ExpresionBinaria(expr, derecha, operador);
        }
        return expr;
    }

    //factor → unario ( ( "/" | "*" ) unario )* 
    public Expresion factor() 
    { 
        Expresion expr = unario();
        while (coincide(TokenType.Slach, TokenType.Asterizco)) 
        { 
            Token operador = anterior(); 
            Expresion derecha = unario();
            expr = new Expresion.ExpresionBinaria(expr, derecha, operador);
        }
        return expr;
    }

    //unario → ( "!" | "-" ) unario| primaria
    public Expresion unario() 
    {
        if (coincide(TokenType.Bang, TokenType.Menos)) 
        { 
            Token operador = anterior(); 
            Expresion derecha = unario();
            return new Expresion.ExpresionUnaria(operador, derecha);
        
        }
        return call();    
    }

    public Expresion call() 
    {
        Expresion expr = primario();

        while (true) 
        { 
        if (coincide(TokenType.Parentesis_abierto)) 
        {
            expr = terminarLlamada(expr);
        } 
        else 
        {
            break;
        }
        }

        return expr;
    }

    public Expresion terminarLlamada(Expresion expr) 
    {
        List<Expresion> argumentos = new List<Expresion>();
        if (!verifica(TokenType.Parentesis_cerrado)) 
        {
            do {
                argumentos.Add(expresion());
                if (argumentos.Count >= 255) 
                {
                    error(mira(), "No se puede obtener mas de 255 argumentos");
                }
                } while (coincide(TokenType.Coma));
        }

        Token paren = consume(TokenType.Parentesis_cerrado, "Se esperaba ) despues de arguemntos");

        return new Expresion.LlamarExpresion(expr, paren, argumentos);
    }

    //primario → NUMBER | STRING | "true" | "false" | "null"| "(" expresión ")"
    public Expresion primario() 
    {
        if (coincide(TokenType.False)) return new Expresion.ExpresionLiteral(anterior(), Tipo.Bool); 
        if (coincide(TokenType.True)) return new Expresion.ExpresionLiteral(anterior(), Tipo.Bool); 
        if (coincide(TokenType.Null)) return new Expresion.ExpresionLiteral(null, Tipo.nil);
        
        if (coincide(TokenType.Número)) 
        {
            return new Expresion.ExpresionLiteral(anterior(), Tipo.Numero);
        }

        if(coincide(TokenType.Cadena))
        {
            return new Expresion.ExpresionLiteral(anterior(), Tipo.Cadena);

        }

        if (coincide(TokenType.Identificador)) 
        {
            return new Expresion.ExpresionVariable(anterior());
        }
        
        if (coincide(TokenType.Parentesis_abierto)) 
        { 
            Expresion expr = expresion();
            consume(TokenType.Parentesis_cerrado, "Expect ')' after expression."); 
            return new Expresion.ExpresionAgrupacion(expr);
        }

        throw error(mira(), "Espera expresion");
    }

    public Token consume(TokenType tipo, string mensaje) // Asegura que el token actual es del tipo esperado y avanza al siguiente.
    { 
        if (verifica(tipo)) return avanza();

        throw error(mira(), mensaje);
    }

    public ParseError error(Token token, string mensaje) //Maneja errores de análisis.
    { 
        Inicio.error(token, mensaje);
        return new ParseError();
    }

    public void sincronizar() //Recupera el análisis después de un error
    {
        avanza();

        while (!esFinal()) {
        if (anterior().Tipo == TokenType.Punto_y_coma) return;

        switch (mira().Tipo) 
        {
            case TokenType.Class:
            case TokenType.Fun:
            case TokenType.Var:
            case TokenType.For:
            case TokenType.If:
            case TokenType.While:
            case TokenType.Return:
            return;
        }

        avanza();
        }
    }

    public Declaracion declaracion() //Analiza diferentes tipos de declaraciones
    {
        if (coincide(TokenType.For)) return forDeclaracion();
        // if (coincide(TokenType.Print)) return ImprimirDeclaracion();
        if (coincide(TokenType.While)) return whileDeclaracion();
        

        return ExpresionDeclaracion();
    }

    // public Declaracion ImprimirDeclaracion()
    // {
    //     Expresion value = expresion();
    //     consume(TokenType.Punto_y_coma, "Se esperaba ;");
    //     return new Declaracion.Print(value);
    // }

    public Declaracion ExpresionDeclaracion()
    {
        Expresion expr = expresion();
        consume(TokenType.Punto_y_coma, "Se esperaba ;");
        return new Declaracion.Expression(expr);
    }
    
    
  
    public Declaracion declara() 
    {
        try 
        {
            if (verifica(TokenType.Identificador)) 
            {
                Token nombre = consume(TokenType.Identificador, "Expect variable name.");

                if(verifica(TokenType.Igual))
                {
                    return varDeclaracion(nombre);
                }
                else 
                {
                    Token operador = avanza();
                    Expresion valor = expresion();
                    return new Declaracion.IncYDec(nombre, operador, valor);
                }
            }

            return declaracion();
        } 
        catch (ParseError)
        {
        sincronizar();
        return null;
        }
    }   

    public Declaracion varDeclaracion(Token nombre) 
    {
        

        Expresion inicializador = null;
        if (coincide(TokenType.Igual)) {
        inicializador = expresion();
        }

        consume(TokenType.Punto_y_coma, "Se esperaba ; despues de la declaracion de variable");
        return new Declaracion.Var(nombre, inicializador);
  }

    
    public List<claseMadre> bloque() 
    {
        List<claseMadre> declaraciones = new List<claseMadre>();

        do
        {
            declaraciones.Add(declara());
        }
        while (!verifica(TokenType.Llave_cerrada) && !esFinal());

        coincide(TokenType.Punto_y_coma);

        consume(TokenType.Llave_cerrada, "Se esperaba } despues del bloque");
        return declaraciones;
    }

    public Declaracion whileDeclaracion() 
    {
        consume(TokenType.Parentesis_abierto, "Se esperaba ( despues de while");
        Expresion condicion = expresion();
        consume(TokenType.Parentesis_cerrado, "Se esperaba ) despues de la condicion");
        Declaracion cuerpo = declaracion();

        return new Declaracion.While(condicion, cuerpo);
    }

    public Declaracion forDeclaracion() 
    {
        Token Variable = null;
        if(coincide(TokenType.Identificador)) Variable = anterior();
        else error(mira(), "Variable no declarada");

        if(!coincide(TokenType.In)) error(mira(), "Se esperaba la palabra reservada 'in'");

        Expresion Colection = expresion();
        Declaracion.Bloque body = null;

        if(coincide(TokenType.Llave_abierta)) body = new Declaracion.Bloque(bloque());
        
        return new Declaracion.For(Variable, Colection, body);
        
    }

    public claseMadre declararCartaOEfecto()
    {
        if (coincide(TokenType.Card))
        {
            if(coincide(TokenType.Llave_abierta)) return cardDeclaracion();
        }

        if (coincide(TokenType.Effect))
        {
            if(coincide(TokenType.Llave_abierta)) return effectDeclaracion();
        }
        return null;

    }

    public Card cardDeclaracion()
    {
        Expresion.ExpresionLiteral name = null;
        Expresion.ExpresionLiteral faction = null;
        Expresion.ExpresionLiteral type = null;
        Expresion.ExpresionLiteral power = null;
        List<Expresion> range = new List<Expresion>();

        do
        {
            try
            {
                if(coincide(TokenType.Name)) name = AsignarExpresion(name is null);  
                else if(coincide(TokenType.Faction)) faction = AsignarExpresion(faction is null);
                else if(coincide(TokenType.Type)) type = AsignarExpresion(type is null);
                else if(coincide(TokenType.Power)) power = AsignarExpresion(power is null);
                else if(coincide(TokenType.Range)) 
                {
                    if(!coincide(TokenType.Doble_punto)) new ParseError();
                    
                    if(coincide(TokenType.Corchete_Abierto))
                    {
                        do{
                        range.Add(expresion());
                        if(!coincide(TokenType.Coma)) new ParseError();
                        }while(!coincide(TokenType.Corchete_Cerrado));
                    }
                    else range.Add(expresion());
                    if(!coincide(TokenType.Coma)) new ParseError();
                }

            }
            catch{System.Console.WriteLine("no es una carta");}
        }while(!coincide(TokenType.Llave_cerrada));

        if(name == null) throw new Exception("Carta sin nombre");
        if(type == null) throw new Exception("Carta sin tipo");
        if(faction == null) throw new Exception("Carta sin faccion");
        if(power == null) throw new Exception("Carta sin poder");
        if(range.Count == 0) throw new Exception("Lista de range vacia");


        return new Card(name, type, faction, power, range);


    } 

    public Effect effectDeclaracion()
    { 
        Expresion.ExpresionLiteral name = null;
        Declaracion Action = null;
        Token targets;
        Token context;
        List<(Token, Token)> parametros = new List<(Token, Token)>();

        do{
            try{
                if(verifica(TokenType.Fin)) error(mira(), "Unfinished effect.");
                else if(coincide(TokenType.Name)) name = AsignarExpresion(name is null);

                else if(coincide(TokenType.Params)) 
                {
                    if(coincide(TokenType.Doble_punto))
                    {
                        if(coincide(TokenType.Llave_abierta))
                        {
                            parametros.Add(Parametros());
                        }
                        else error(mira(), "Parameters are expected");
                        if(!coincide(TokenType.Llave_cerrada)) error(mira(), "Se esperaba un llave cerrada");
                        if(!coincide(TokenType.Coma)) error(mira(), "Se esperaba una coma");
                    }
                    else error(mira(), "Two points expected");
                }

                else if(coincide(TokenType.Action))
                {
                    if(!(Action == null)) throw error(mira(), "Expresion ya asignada");

                    if(coincide(TokenType.Doble_punto))
                    {
                        if(coincide(TokenType.Parentesis_abierto))
                        {
                            if(coincide(TokenType.Targets)) targets = anterior();
                            if(!coincide(TokenType.Coma)) error(mira(), "A comma is expected");
                            if(coincide(TokenType.Context)) context = anterior();
                            if(!coincide(TokenType.Parentesis_cerrado)) error(mira(), "Closing parenthesis expected");
                        }
                        consume(TokenType.Lambda, "'=>' expected");
                        if(coincide(TokenType.Llave_abierta)) Action = new Declaracion.Bloque(bloque());
                        else Action = declara();
                        if(Action == null) error(mira(), "Body action is expected");
                    }
                    else error(mira(), "Two points expected");
                }
                else error(mira(), "An action was expected");
            }
            catch (ParseError)
            {
                sincronizar();
            }
        }while(!coincide(TokenType.Llave_cerrada));
        
        if(name == null) throw new Exception("Efecto sin nombre");
        if(Action == null) throw new Exception("Efecto sin action");

        return new Effect(name, Action, parametros);
    }

    public Expresion.ExpresionLiteral AsignarExpresion(bool estado)
    {
        if(!estado) throw error(mira(), "Expresion ya asiganda");

        Expresion.ExpresionLiteral Expr = null;

        if(coincide(TokenType.Doble_punto)) Expr = (Expresion.ExpresionLiteral?)primario();

        if(!coincide(TokenType.Coma)) error(mira(), "A comma is expected");
        return Expr;
    }

    public (Token, Token ) Parametros()
    {
        Token nombre = null;
        Token tipo = null;

        if(coincide(TokenType.Identificador)) nombre = anterior();
        else error(mira(), "Se esperaba un identificador.");

        if(!coincide(TokenType.Doble_punto)) error(mira(), "Se esperaban dos puntos.");

        if(coincide(TokenType.Identificador)) tipo = anterior();
        else error(mira(), "Se esperaba un identificador.");

        return (nombre, tipo);
    }


}