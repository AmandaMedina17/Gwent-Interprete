using System.Text;

public class Inicio {

    private static Interprete interprete = new Interprete();
    public static bool hadError = false;
    static bool hadRuntimeError = false;
    public static void Main(String[] args) 
    { 
        if (args.Length > 1) 
        {
            System.Console.WriteLine("Uso mi nuevo interprete"); 
            Environment.Exit(64);
        } 
        else if (args.Length == 1) 
        { 
            runFile(args[0]);
        } else 
        { 
            RunPrompt();
        
        }
    }

    // Método para ejecutar el contenido de un archivo
    private static void runFile(string path)
    { 
        byte[] bytes = File.ReadAllBytes(path); //Para leer los bytes de un archivo
        string content = Encoding.Default.GetString(bytes);  //Convertir lo bytes en una cadena
        Run(content);

        if (hadError) Environment.Exit(65);
        if (hadRuntimeError) Environment.Exit(70);
    }

    private static void RunPrompt()
    {
        using (var reader = new StreamReader(Console.OpenStandardInput()))
        {
            
                Console.Write("> ");
                string linea = "x = 5 > 4; y = 8 * 2; z = 30 - 28; Print ( x ); Print ( y @@ z );";
            
                Run(linea);
                hadError = false;
            
        }
    }

    private static void Run(string fuente)
    {
        Escaner escaner = new Escaner(fuente);
        List<Token> tokens = escaner.escanearTokens();
        Parser parser = new Parser(tokens);
        List<Declaracion> declaraciones = parser.parse();
        if (hadError) return;
        interprete.interpretar(declaraciones);

    }

    public static void Error(int linea, string mensaje)
    {
        Report(linea, "", mensaje);
    }

    private static void Report(int linea, string donde, string mensaje)
    {
        Console.Error.WriteLine($"[línea {linea}] Error{donde}: {mensaje}");
        hadError = true;
    }

    public static void error(Token token, string message) {
    if (token.Tipo == TokenType.Fin) {
      Report(token.linea, " al final", message);
    } else {
      Report(token.linea, " en '" + token.Valor + "'", message);
    }
  }

  public static void runtimeError(RuntimeError error) 
  {
    Console.Error.WriteLine(error.Message + $"\n[line {error.token.linea}]");
    hadRuntimeError = true;
  }
}
