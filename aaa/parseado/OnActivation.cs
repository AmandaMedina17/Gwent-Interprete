public class OnActivation : Declaracion
{
    public Dictionary<ActivacionEfecto, ActivacionEfecto> efectos;

    public OnActivation(Dictionary<ActivacionEfecto, ActivacionEfecto> efectos)
    {
        this.efectos = efectos;
    }

    public override void Ejecutar()
    {
        foreach (var item in efectos)
        {
            item.Key.Ejecutar(); 
            if(!(item.Value is null)) item.Value.Ejecutar();
        }
    }

    public override bool Semantica()
    {
        foreach (var item in efectos)
        {
            if(!item.Key.Semantica())
            {
                System.Console.WriteLine("Efecto invalido");
                return false;
            }
            if(item.Value.Semantica())
            {
                System.Console.WriteLine("Efecto invalido");
                return false;
            }
            
        }
        return true;
    }

    public void Ejecucion(EstadoDeJuego estadoDeJuego)
        {
            try
            {
                Ejecutar();
            
            }
            catch(Exception)
            {
             
            }
        }
}

