public class Entorno
{
    private static Entorno encerrado;
    private Entorno parent;
    public Dictionary<string, Expresion> Valores = new Dictionary<string, Expresion>();


    public static Entorno Encerrado
        {
            get
            {
                if (encerrado == null)
                {
                    encerrado = new Entorno();
                    encerrado.parent = null;
                    encerrado.Valores.Add("context", Context.context);
                }

                return encerrado;
            }
        }    
    public Entorno()
    {
        this.parent = null;
    }

    public Entorno(Entorno parent)
    {
        this.parent = parent;
    }

    public object Get(Token nombre) 
    {
        if (Valores.ContainsKey(nombre.Valor)) {
        return Valores[nombre.Valor];
        }

        if (parent != null) return parent.Get(nombre);
        throw new RuntimeError(nombre, "Undefined variable '" + nombre.Valor + "'.");
    }

    public void asignar(Token nombre, Expresion valor) 
    {
        if (Valores.ContainsKey(nombre.Valor)) {
        Valores.Add(nombre.Valor, valor);
        return;
        }

        if (parent != null) 
        {
            parent.asignar(nombre, valor);
            return;
        }
        
        throw new RuntimeError(nombre, "Undefined variable '" + nombre.Valor + "'.");
    }

    public void define(string nombre, Expresion valor)
    {
        if (Valores.ContainsKey(nombre))
        {
            // Si la variable ya está definida, actualiza su valor
            Valores[nombre] = valor;
        }
        else
        {
            // Si no está definida, agrega la nueva variable
            Valores.Add(nombre, valor);
        }
    }

    public void Set(Token var, Expresion valor)
        {
            if (var.Valor == "context") throw new Exception("Context es una keyword");

            if (Valores.ContainsKey(var.Valor))
            {
                if (valor != null)
                {
                    Valores[var.Valor] = valor;
                }
                else
                {
                    throw new Exception("Variable declarada ya anteriormente");
                }

            }
            else if (parent.Valores.ContainsKey(var.Valor)) parent.Valores[var.Valor]= valor;
            else Valores.Add(var.Valor, valor);
        }

    public Expresion this[string name]
        {
            get
            {
                try
                {
                    if (Valores.ContainsKey(name)) return Valores[name];
                    else if (parent.Valores.ContainsKey(name)) return parent.Valores[name];
                    else throw new KeyNotFoundException();
                }
                catch (KeyNotFoundException)
                {
                    throw new Exception("Variable no declarada");
                }
            }
            private set
            {
                try
                {
                    if (Valores.ContainsKey(name)) Valores[name] = value;
                    else if (parent.Valores.ContainsKey(name)) parent.Valores[name] = value;
                    else throw new KeyNotFoundException();
                }
                catch (KeyNotFoundException)
                {
                    throw new Exception("Variable no declarada");
                }
            }
        }
}