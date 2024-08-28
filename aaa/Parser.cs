using System.Linq.Expressions;
using System.Text.Json;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;

class Parser
{
    public class ParseError : Exception{}  //Para analizar errores durante el analisis
    public List<Token> tokens;  // Una lista que almacena los tokens generados por el escáner
    public int Current = 0;  //Un índice que apunta al token actual durante el análisis
    Token True = new Token(TokenType.True, "true", true, 0);
    Stack<Entorno> entornos;


    public Parser(List<Token> tokens)
    {
        this.tokens = tokens;
        entornos = new Stack<Entorno>();
        entornos.Push(Entorno.Encerrado);
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
    public bool esKeyword(Token token)
    {
        if(Escaner.keywords.ContainsValue(token.Tipo))
        {
            avanza();
            return true;
        }
        return false;
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
        return primario();    
    }

    //primario → NUMBER | STRING | "true" | "false" | "null"| "(" expresión ")"
    public Expresion primario() 
    {
        if (coincide(TokenType.False)) return new Expresion.ExpresionLiteral(anterior()); 
        if (coincide(TokenType.True)) return new Expresion.ExpresionLiteral(anterior()); 
        if (coincide(TokenType.Null)) return new Expresion.ExpresionLiteral(null);
        
        if (coincide(TokenType.Número)) 
        {
            return new Expresion.ExpresionLiteral(anterior());
        }

        if(coincide(TokenType.Cadena))
        {
            return new Expresion.ExpresionLiteral(anterior());

        }

        

        Expresion expr = null;

            if (coincide(TokenType.Parentesis_abierto))
            {
               expr = expresion();
                if (mira().Tipo != TokenType.Parentesis_cerrado && !(expr is Expresion.Predicate)) throw new Exception("error");
            }
            else
            {
                if (coincide (TokenType.Fin, TokenType.Punto_y_coma, TokenType.Coma, TokenType.Parentesis_cerrado)) error(mira(), "Token inesperado");

                if (coincide(TokenType.Identificador))
                {
                    Token variable = anterior();

                    if (coincide(TokenType.Mas_mas, TokenType.Menos_menos))
                    {
                        expr = new Expresion.DeclVar(new Declaracion.IncYDec(variable, entornos.Peek(), anterior()));
                    }
                    if (coincide(TokenType.Corchete_Abierto))
                    {
                        expr = new Expresion.Indexador(new Expresion.ExpresionLiteral(anterior()), expresion());

                        if (!coincide(TokenType.Corchete_Cerrado)) error(mira(), "Indexador invalido");
                    }
                    else if (coincide(TokenType.Parentesis_cerrado) && coincide(TokenType.Lambda)) return Predicate(variable);

                    else expr = new Expresion.DeclVar(new Declaracion.IncYDec(variable, entornos.Peek()));

                }
                else { expr = new Expresion.ExpresionLiteral(anterior());}

                while (coincide(TokenType.Punto))
                {
                    Token caller = null;
                    if (esKeyword(mira()) || coincide(TokenType.Identificador)) caller = anterior();
                    else error(mira(), "Se esperaba un valor");

                    if (coincide(TokenType.Parentesis_abierto))
                    {
                        if (!coincide(TokenType.Parentesis_cerrado))
                        {
                            List<Expresion> arguments = new List<Expresion>();

                            do{ arguments.Add(expresion()); 
                            Current--;
                            }while (coincide(TokenType.Coma));

                            if (!coincide(TokenType.Parentesis_cerrado)) error(mira(), "Se esperaba parentesis cerrado");

                            expr = new Expresion.Metodo(expr, caller, arguments);
                        }
                        else expr = new Expresion.Metodo(expr, caller);
                    }
                    else expr = new Expresion.Propiedad(expr, caller);
                }
                

            }
        return expr;

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
        if (coincide(TokenType.While)) return whileDeclaracion();
        

        return ExpresionDeclaracion();
    }

    public Declaracion ExpresionDeclaracion()
    {   
        Token variable = anterior();
        if(coincide(TokenType.Punto_y_coma)) return new Declaracion.IncYDec(variable, entornos.Peek());

        else if(verifica(TokenType.Igual)) return varDeclaracion(variable);
        else if (coincide(TokenType.Aumentar, TokenType.Disminuir)) return new Declaracion.IncYDec(variable, entornos.Peek(), anterior(), expresion());
        
        else 
        {
            Expresion exp = expresion();
            return new Declaracion.Expression(exp);
        }
    }
    
    
  
    public Declaracion declara() 
    {
        
        try 
        {

            Declaracion declaration = null;

            while(!verifica(TokenType.Punto_y_coma))
            {

                if (coincide(TokenType.Identificador))
                {
                    declaration = ExpresionDeclaracion();
                }
                else declaration = declaracion();
            }
            

            if(!coincide(TokenType.Punto_y_coma)) error(mira(), "Se esperaba ';");
                
            return declaration;
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
        return new Declaracion.Var(nombre, inicializador);
    }

    
    public List<claseMadre> bloque() 
    {
        List<claseMadre> declaraciones = new List<claseMadre>();

        if (entornos.Count > 1) entornos.Push(new Entorno(entornos.Peek()));
            do
            {
                try
                {
                    if (verifica(TokenType.Fin))  throw new Exception("Bloque no terminado");

                    else if (coincide(TokenType.While)) declaraciones.Add(whileDeclaracion());

                    else if (coincide(TokenType.For)) declaraciones.Add(forDeclaracion());

                    else declaraciones.Add(declara());
                }
                catch
                {
                    sincronizar();
                }

            } while (!coincide(TokenType.Llave_cerrada));

            coincide(TokenType.Punto_y_coma);

            if (entornos.Count > 1) entornos.Pop();

            return declaraciones;
        // do
        // {
        //     declaraciones.Add(declara());
        // }
        // while (!coincide(TokenType.Llave_cerrada));

        // coincide(TokenType.Punto_y_coma);

        // consume(TokenType.Llave_cerrada, "Se esperaba } despues del bloque");
        // return declaraciones;
    }

    public Declaracion whileDeclaracion() 
    {
        consume(TokenType.Parentesis_abierto, "Se esperaba ( despues de while");
        Expresion condicion = expresion();
        Current -= 1;
        consume(TokenType.Parentesis_cerrado, "Se esperaba ) despues de la condicion");
        Declaracion cuerpo = null;
        if (coincide(TokenType.Llave_abierta)) cuerpo = new Declaracion.Bloque(bloque());
        else cuerpo = declara();
        return new Declaracion.While(condicion, cuerpo);
    }

    public Declaracion forDeclaracion() 
    {
        Token Variable = null;
        if(coincide(TokenType.Identificador)) Variable = anterior();
        else error(mira(), "Variable no declarada");

        Expresion Colection = null;
        if(coincide(TokenType.In))
        {        
            Colection = expresion();
        }
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
        OnActivation onActivation = null;

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
                    if(!coincide(TokenType.Doble_punto)) error(mira(), "Se esperaban dos puntos");
                    
                    if(coincide(TokenType.Corchete_Abierto))
                    {
                        do{
                            range.Add(expresion());
                            if(!coincide(TokenType.Coma) && !verifica(TokenType.Corchete_Cerrado)) error(mira(), "Se esperaba una coma");
                        }while(!coincide(TokenType.Corchete_Cerrado));
                    }
                    else range.Add(expresion());
                    if(!coincide(TokenType.Coma)) error(mira(), "Se esperaba una coma");
                }
                else if (coincide(TokenType.OnActivation))
                    {
                        if (!coincide(TokenType.Doble_punto)) error(mira(), "Se esperaba el cuerpo del OnActivation");
                        
                        Dictionary<Declaracion.ActivacionEfecto, Declaracion.ActivacionEfecto> effects = new Dictionary<Declaracion.ActivacionEfecto, Declaracion.ActivacionEfecto>();

                        
                        if (coincide(TokenType.Corchete_Abierto))
                        {
                            do
                            {
                                var effectPair = AsignarEfecto();
                                effects.Add(effectPair.Key, effectPair.Value);
                                if (!coincide(TokenType.Coma) && !verifica(TokenType.Corchete_Cerrado))  error(mira(), "Estructura invalida");

                            } while (!coincide(TokenType.Llave_cerrada));
                        }
                        else 
                        {
                            var effectPair = AsignarEfecto();
                            effects.Add(effectPair.Key, effectPair.Value);
                        }

                        onActivation = new OnActivation(effects);
                        if (!coincide(TokenType.Coma)) error(mira(), "Se esperaba una coma");
                    }

                    else throw new Exception("Declaracion invalida de carta");
                }
            catch{System.Console.WriteLine("no es una carta");}
        }while(!coincide(TokenType.Llave_cerrada));

        if(name == null) throw new Exception("Carta sin nombre");
        if(type == null) throw new Exception("Carta sin tipo");
        if(faction == null) throw new Exception("Carta sin faccion");
        if(power == null) throw new Exception("Carta sin poder");
        if(range.Count == 0) throw new Exception("Lista de range vacia");


        return new Card(name, type, faction, power, range, onActivation);


    } 

    KeyValuePair<Declaracion.ActivacionEfecto, Declaracion.ActivacionEfecto> AsignarEfecto()
{
    if (!coincide(TokenType.Llave_abierta)) error(mira(), "Se esperaba '{'");

    Expresion effectName = null;
    List<(Token, Expresion)> _params = new List<(Token, Expresion)>();
    Expresion selector = null;
    Expresion PostAction = null;
    List<(Token, Expresion)> _paramsPostAction = new List<(Token, Expresion)>();
    Expresion selectorPostAction = null;

    do
    {
         try
                {
                    if (verifica(TokenType.Fin)) error(mira(), "Declaracion no terminada");

                    else if (coincide(TokenType.Effect_card))
                    {
                        if (!coincide(TokenType.Doble_punto)) error(mira(), "Se esperaba ':'");

                        if (coincide(TokenType.Llave_abierta)) CuerpoDelEfecto(ref effectName, ref _params, TokenType.Name);
                        else effectName = expresion();

                        if (!coincide(TokenType.Coma)) error(mira(), "Se esperaba ','");
                    }

                    else if (coincide(TokenType.Selector))
                    {
                        if (!coincide(TokenType.Doble_punto)) error(mira(), "Se esperaba ':'");
                        if (!coincide(TokenType.Llave_abierta)) error(mira(), "Se esperaba '{'");
                        selector = Selector();
                    }

                    else if (coincide(TokenType.PostAction))
                    {
                        if (!coincide(TokenType.Doble_punto)) error(mira(), "Se esperaba ':'");

                        if (coincide(TokenType.Llave_abierta)) selectorPostAction = CuerpoDelEfecto(ref PostAction, ref _paramsPostAction, TokenType.Type, selector);
                        else effectName = expresion();

                        if (!coincide(TokenType.Coma)) error(mira(), "Se esperaba ','"); 
                    }

                    else error(mira(), "");
                }
                catch
                {
                    sincronizar();
                }
            } while (!coincide(TokenType.Llave_cerrada));

    if (effectName is null) throw new Exception("Nombre del efecto invalido");
    if (selector is null) throw new Exception("Selector invalido");
    if (!(PostAction is null)) 
    {
        if(selectorPostAction is null) throw new Exception("Selector del PostAction invalido");
    }

    Declaracion.ActivacionEfecto key = new Declaracion.ActivacionEfecto(effectName, _params, selector);
    Declaracion.ActivacionEfecto value = new Declaracion.ActivacionEfecto(PostAction, _paramsPostAction, selectorPostAction is null ? selector : selectorPostAction);

    return new KeyValuePair<Declaracion.ActivacionEfecto, Declaracion.ActivacionEfecto> (key, value);
}


     Expresion CuerpoDelEfecto(ref Expresion effectName, ref List<(Token, Expresion)> _params, TokenType name, Expresion parentSelector = null)
        {
            Expresion selector = null;
            do
            {
                try
                {
                    if (verifica(TokenType.Fin)) error(mira(), "");

                    else if (coincide(name)) effectName = AsignarExpresion(effectName is null);

                    else if (coincide(TokenType.Identificador)) _params.Add((anterior(), AsignarExpresion(true)));

                    else if (name is TokenType.Type && coincide(TokenType.Selector))
                    {
                        if (!coincide(TokenType.Doble_punto)) error(mira(), "");
                        if (!coincide(TokenType.Llave_abierta)) error(mira(), "");
                        selector = Selector(parentSelector);
                    }
                    else throw new Exception("Se esperaba un selector");
                }
                catch 
                {
                    sincronizar();
                }
            } while (!coincide(TokenType.Llave_cerrada));

            return selector;
        }

        Expresion Selector(Expresion parent = null)
        {
            Expresion source = null;
            Expresion single = null;
            Expresion predicate = null;

            do
            {
                try
                {
                    if (verifica(TokenType.Fin)) error(mira(), "");

                    else if (coincide(TokenType.Source)) source = AsignarExpresion(source is null);

                    else if (coincide(TokenType.Single)) single = AsignarExpresion(single is null);

                    else if (coincide(TokenType.Predicate)) predicate = AsignarExpresion(predicate is null);

                    else throw new Exception("Selector invalido");
                }
                catch 
                {
                    sincronizar();
                }
            } while (!coincide(TokenType.Llave_cerrada));

            if (source is null) throw new Exception("Source invalido");
            if (predicate is null) throw new Exception("Predicate invalido");

            return new Selector(source, predicate, single, parent);
        }

        Expresion Predicate(Token variable = null)
        {
            if(variable is null)
            {
                if (coincide(TokenType.Parentesis_abierto) && coincide(TokenType.Identificador))
                {
                    variable = anterior();
                    if (!coincide(TokenType.Parentesis_cerrado)) error(mira(), "Se esperaba un parentesis cerrado");
                }
                else error(mira(), "Invalid Predicate");

                if (!coincide(TokenType.Lambda)) error(mira(), "Invalid Predicate");
            }
            Expresion condition = expresion();
            return new Expresion.Predicate(variable, condition, entornos.Pop());
        }



    public Effect effectDeclaracion()
    { 
        Expresion.ExpresionLiteral name = null;
        Declaracion Action = null;
        Token targets = null;
        Token context = null;
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
                            if(coincide(TokenType.Identificador)) targets = anterior();
                            if(!coincide(TokenType.Coma)) error(mira(), "A comma is expected");
                            if(coincide(TokenType.Identificador)) context = anterior();
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

        return new Effect(name, Action, parametros, targets, context);
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