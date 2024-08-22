using System.Dynamic;
using System.Text.RegularExpressions;
using System.Collections;

/*La clase Escaner es responsable de leer una cadena de texto y dividirla en tokens
que representan los componentes del lenguaje. Utiliza un enfoque basado en una máquina
de estados, donde cada carácter se procesa y se convierte en un token específico. 
La clase también maneja errores de sintaxis, como caracteres inesperados y cadenas sin terminar.*/
public class Escaner
{
    public string fuente{ get; set; }  //propiedad que almacena la cadena de texto que se va a escanear.
    public List<Token> Tokens = new List<Token>();  //Una lista que almacenará los tokens generados
    public int start = 0;  //Inicio del token actual
    public int current = 0;  //Posicion actual
    public int linea = 1;  //contador que lleva el seguimiento del número de línea actual en la fuente.

    public Escaner(string fuente) 
    {
        this.fuente = fuente;
        palabrasReservadas();
    }

    private bool isAtEnd() //Este método verifica si se ha llegado al final de la cadena de entrada.

    {
        return current >= fuente.Length;
    }

    public List<Token> escanearTokens()   //Este método escanea la cadena de entrada y genera tokens.
    {
        while (!isAtEnd()) 
        {
            start = current;
            Tokenescaneado();
        }
            
        Tokens.Add(new Token(TokenType.Fin, "", null, linea));
        return Tokens;
        
    }

    private void Tokenescaneado() //Este método se encarga de analizar el carácter actual y determinar qué tipo de token debe generarse.
    {
       char c = advance();
    switch (c) 
    {
        // Un caracter
        case '(': addToken(TokenType.Parentesis_abierto); break;
        case ')': addToken(TokenType.Parentesis_cerrado); break;
        case '{': addToken(TokenType.Llave_abierta); break;
        case '}': addToken(TokenType.Llave_cerrada); break;
        case '[': addToken(TokenType.Corchete_Abierto); break;
        case ']': addToken(TokenType.Corchete_Cerrado); break;
        case ',': addToken(TokenType.Coma); break;
        case '.': addToken(TokenType.Punto); break;
        case ';': addToken(TokenType.Punto_y_coma); break;
        case '*': addToken(TokenType.Asterizco); break;
        case ':': addToken(TokenType.Doble_punto); break;

        // Dos caracteres
        case '!': addToken(match('=') ? TokenType.Bang_igual : TokenType.Bang); break;
        case '<': addToken(match('=') ? TokenType.Menor_igual : TokenType.Menor); break;
        case '>': addToken(match('=') ? TokenType.Mayor_igual : TokenType.Mayor); break;
        case '&': addToken(match('&') ? TokenType.And : throw new ArgumentException(linea + " caracter inesperado")); break;
        case '|': addToken(match('|') ? TokenType.Or : throw new ArgumentException(linea + " caracter inesperado")); break;
        case '@': addToken(match('@') ? TokenType.Concatenacion_Espaciado : TokenType.Concatenacion); break;
        case '=': if(match('=')) addToken(TokenType.Igual_igual);
                    else if(match('>')) addToken(TokenType.Lambda);
                    else addToken(TokenType.Igual); break;
        case '+': if(match('+')) addToken(TokenType.Mas_mas);
                    else if(match('=')) addToken(TokenType.Aumentar);
                    else addToken(TokenType.Más); break;
        case '-': if(match('-')) addToken(TokenType.Menos_menos);
                    else if(match('=')) addToken(TokenType.Disminuir);
                    else addToken(TokenType.Menos); break;

        case '/':
            if (match('/')) 
            {
                // Un comentario va hasta el final de la línea.
                while (peek() != '\n' && !isAtEnd()) advance();
            } 
            else 
            {
                addToken(TokenType.Slach);
            }
            break;

        // Nuevas líneas y espacios en blanco
        case ' ':
        case '\r':
        case '\t':
            break;
        case '\n':
            linea++;
            break;

        // Literales de cadena
        case '"': String(); break;

        // Literales numéricos
        default:
            if (isDigit(c)) 
            {
                número();
            } 
            else if (isAlpha(c)) 
            {
                identificador();
            }
            else 
            {
                throw new ArgumentException(linea + " Caracter inesperado");
            }
            break;
            
        }
    }
    private bool isAlpha(char c)  //Saber si el caracter es una letra
    {
        return (c >= 'a' && c <= 'z') ||
        (c >= 'A' && c <= 'Z') ||
        c == '_';
    }

    private bool isDigit(char c)  //Saber si el caracter es un numero
    {

        // Convierte el carácter a su valor numérico
        double numericValue = char.GetNumericValue(c);

        if(numericValue==-1) return false;
        else return true;

        // Verifica si el valor numérico es un dígito (0-9)
        //return numericValue >= 0 && numericValue <= 9;


    }

    private bool isAlphaNumeric(char c)  //Saber si es numero o letra
    {
        return isAlpha(c) || isDigit(c);
    }

    private void identificador()  //Este método analiza los identificadores
    {
        while (isAlphaNumeric(peek())) advance();
        string text = fuente.Substring(start, current-start);
         if (!keywords.TryGetValue(text, out TokenType type))
        {
            // Si no se encuentra, se trata como un identificador
            type = TokenType.Identificador;
        }

        addToken(type);
    }


    private void número() // Este método analiza los números en la fuente, incluyendo números decimales.
    {
         while (isDigit(peek())) advance();

        // Asegúrate de que la subcadena extraída sea un número válido
        string numberString = fuente.Substring(start, current - start);
        try
        {
            addToken(TokenType.Número, double.Parse(numberString));
        }
        catch (FormatException)
        {
            throw new ArgumentException(linea + " Número no válido: " + numberString);
        }
    }

    private char peekNext()  //Este metodo mira el caracter siguiente
    {
        if (current + 1 >= fuente.Length) return '\0';
        return fuente[current + 1];
    }
    

    private void String()  // Este método analiza las cadenas de texto delimitadas por comillas
    {
        while (peek() != '"' && !isAtEnd()) 
        {
            if (peek() == '\n') linea++;
            advance();
            }
            if (isAtEnd()) {
            throw new ArgumentException(linea + "Cadena sin terminar.");
        }

        advance();

        string value = fuente.Substring(start + 1, current - start - 1);
        addToken(TokenType.Cadena, value);    
    }

    private char peek()  //Este metodo mira el caracter en current
    {
        if (isAtEnd()) return '\0';
        return fuente[current];
    }

    public char advance() // Avanza a la siguiente posición en la cadena de entrada
    {
        current++;
        return fuente[current - 1];
    }
    
    private void addToken(TokenType type) 
    {
        addToken(type, null);
    }
    
    private void addToken(TokenType type, object literal) //Agrega un nuevo token a la lista de tokens
    {
        string text = fuente.Substring(start, current - start);
        Tokens.Add(new Token(type, text, literal, linea));
    }

    private bool match(char expected) //Metodo para ver si el caracter coincide con el esperado
    {
        if (isAtEnd()) return false;
        if (fuente[current] != expected) return false;

        current++;
        return true;
    }
        
    private static Dictionary<string, TokenType> keywords;
   
    private void palabrasReservadas()
    {
        keywords = new Dictionary<string, TokenType>
        {
            { "false", TokenType.False },
            { "for", TokenType.For },
            { "else", TokenType.Else },
            { "return", TokenType.Return },
            { "true", TokenType.True },
            { "while", TokenType.While },
            { "effect", TokenType.Effect },
            { "Name", TokenType.Name },
            { "Params", TokenType.Params },
            { "Action", TokenType.Action },
            { "card", TokenType.Card },
            { "Type", TokenType.Type },
            { "Faction", TokenType.Faction },
            { "Power", TokenType.Power },
            { "Range", TokenType.Range },
            { "OnActivation", TokenType.OnActivation },
            { "Efect", TokenType.Effect_card },
            { "Selector", TokenType.Selector },
            { "Source", TokenType.Source },
            { "Single", TokenType.Single },
            { "Predicate", TokenType.Predicate },
            { "PostAction", TokenType.PosAction },
            { "Print", TokenType.Print },
            { "in", TokenType.In }
        };




    }
}