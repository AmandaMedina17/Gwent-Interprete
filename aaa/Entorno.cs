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
    public Entorno(Entorno parent=null)
    {
        this.parent = parent ?? this.parent;
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
            else if (ContenidoParent(var.Valor)) Modificar(var.Valor, valor);
            else this.Valores.Add(var.Valor, valor);
        }

    public Expresion this[string name]
        {
            get
            {
                try
                {
                    if (Valores.ContainsKey(name)) return Valores[name];
                    else if (ContenidoParent(name)) return CogerDeParent(name);
                    else throw new Exception("KeyNotFoundException");
                }
                catch
                {
                    throw new Exception("Variable no declarada");
                }
            }
            private set
            {
                try
                {
                    if (Valores.ContainsKey(name)) Valores[name] = value;
                    else if (ContenidoParent(name)) Modificar(name, value);
                    else throw new Exception("KeyNotFoundException");
                }
                catch
                {
                    throw new Exception("Variable no declarada");
                }
            }
        }

    Expresion CogerDeParent(string var)
    {
        if (parent.Valores.ContainsKey(var)) return parent.Valores[var];
        else return parent.CogerDeParent(var);
    }

    public void Modificar(string var, Expresion valor)
    {
        if (parent.Valores.ContainsKey(var)) parent.Valores[var] = valor;
        else parent.Modificar(var, valor);
    }

    public bool ContenidoParent(string var)
    {
        if(parent == null) return false;
        return parent.Contains(var);
    }

    public bool Contains(string var) => Valores.ContainsKey(var) || ContenidoParent(var);
}