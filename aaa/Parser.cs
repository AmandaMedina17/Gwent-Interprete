using System.Linq.Expressions;
using System.Text.Json;
using System.Linq;

class Parser
{
    public class ParseError : Exception{}  //Para analizar errores durante el analisis
    public List<Token> tokens;  // Una lista que almacena los tokens generados por el escáner
    public int Current = 0;  //Un índice que apunta al token actual durante el análisis

    public Parser(List<Token> tokens)
    {
        this.tokens = tokens;
    }

    
    public List<Declaracion> parse() //Este método analiza la lista de tokens y genera una lista de declaraciones.
    {
        List<Declaracion> declaracions = new List<Declaracion>();             
        while (!esFinal()) 
        {
            declaracions.Add(declara());
        }        
        
        return declaracions;
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
        if (coincide(TokenType.False)) return new Expresion.ExpresionLiteral(false); 
        if (coincide(TokenType.True)) return new Expresion.ExpresionLiteral(true); 
        if (coincide(TokenType.Null)) return new Expresion.ExpresionLiteral(null);
        
        if (coincide(TokenType.Número, TokenType.Cadena)) 
        {
            return new Expresion.ExpresionLiteral(anterior().Literal);
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
        if (coincide(TokenType.Print)) return ImprimirDeclaracion();
        if (coincide(TokenType.While)) return whileDeclaracion();
        if (coincide(TokenType.Llave_abierta)) return new Declaracion.Bloque(bloque());

        return ExpresionDeclaracion();
    }

    public Declaracion ImprimirDeclaracion()
    {
        Expresion value = expresion();
        consume(TokenType.Punto_y_coma, "Se esperaba ;");
        return new Declaracion.Print(value);
    }

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
            // if (coincide(TokenType.Action)) return funcion("funcion");
            if (verifica(TokenType.Identificador)) return varDeclaracion();

            return declaracion();
        } 
        catch (ParseError)
        {
        sincronizar();
        return null;
        }
    }   

    public Declaracion varDeclaracion() 
    {
        Token nombre = consume(TokenType.Identificador, "Expect variable name.");

        Expresion inicializador = null;
        if (coincide(TokenType.Igual)) {
        inicializador = expresion();
        }

        consume(TokenType.Punto_y_coma, "Se esperaba ; despues de la declaracion de variable");
        return new Declaracion.Var(nombre, inicializador);
  }

    
    public List<Declaracion> bloque() 
    {
        List<Declaracion> declaraciones = new List<Declaracion>();

        while (!verifica(TokenType.Llave_cerrada) && !esFinal()) {
        declaraciones.Add(declara());
        }

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
        consume(TokenType.Parentesis_abierto, "Se esperaba ( despues de for");

        Declaracion inicializador;
        if (coincide(TokenType.Punto_y_coma)) 
        {
            inicializador = null;
        } 
        else if (coincide(TokenType.Var)) 
        {
            inicializador = varDeclaracion();
        } 
        else 
        {
            inicializador = ExpresionDeclaracion();
        }

        Expresion condicion = null;
        if (!verifica(TokenType.Punto_y_coma)) {
        condicion = expresion();
        }
        consume(TokenType.Punto_y_coma, "Se esperaba ; despues de condicion de bucle");

        Expresion incremento = null;
        if (!verifica(TokenType.Parentesis_cerrado)) {
        incremento = expresion();
        }
        consume(TokenType.Parentesis_cerrado, "Se esperaba ) despues de las clausulas for");

        Declaracion cuerpo = declaracion();

        if (incremento != null) 
        {
            cuerpo = new Declaracion.Bloque(new List<Declaracion>{cuerpo, new Declaracion.Expression(incremento)});
        }

        if (condicion == null) condicion = new Expresion.ExpresionLiteral(true);
        cuerpo = new Declaracion.While(condicion, cuerpo);

        if (inicializador != null) 
        {
            cuerpo = new Declaracion.Bloque(new List<Declaracion>{inicializador, cuerpo});
        }

        return cuerpo;
        
    }
}