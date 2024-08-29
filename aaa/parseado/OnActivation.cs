public class OnActivation : Declaracion
{
    public List<(ActivacionEfecto, ActivacionEfecto)> efectos;

    public OnActivation(List<(ActivacionEfecto, ActivacionEfecto)> efectos)
    {
        this.efectos = efectos;
    }

    public override void Ejecutar()
    {
        foreach (var item in efectos)
        { try
            {
                item.Item1.Ejecutar();
            }
        catch(Exception)
            {
                System.Console.WriteLine("Effecto invalido");
            }

        try
            {
                item.Item2.Ejecutar();
            }
        catch (Exception )
            {
                System.Console.WriteLine("Effecto invalido");
            }
        }
    }

    public override bool Semantica()
    {
        foreach (var item in efectos)
        {
            if(!item.Item1.Semantica())
            {
                System.Console.WriteLine("Efecto invalido");
                return false;
            }
            if(item.Item2.Semantica())
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

