public class Entorno
{
    private Entorno encerrando;
    public Dictionary<string, object> Valores = new Dictionary<string, object>();

    public Entorno()
    {
        this.encerrando = null;
    }

    public Entorno(Entorno encerrando)
    {
        this.encerrando = encerrando;
    }

    public object Get(Token nombre) 
    {
        if (Valores.ContainsKey(nombre.Valor)) {
        return Valores[nombre.Valor];
        }

        if (encerrando != null) return encerrando.Get(nombre);
        throw new RuntimeError(nombre, "Undefined variable '" + nombre.Valor + "'.");
    }

    public void asignar(Token nombre, object valor) 
    {
        if (Valores.ContainsKey(nombre.Valor)) {
        Valores.Add(nombre.Valor, valor);
        return;
        }

        if (encerrando != null) 
        {
            encerrando.asignar(nombre, valor);
            return;
        }
        
        throw new RuntimeError(nombre, "Undefined variable '" + nombre.Valor + "'.");
    }

    public void define(string nombre, object valor)
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
}