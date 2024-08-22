public class Tablero
{
    public Dictionary<string, List<Card>> Zonas;
    private int _ronda;
    public int Ronda { get => _ronda; private set => _ronda = value; }
    public bool TurnoAcabado;  
    public bool CambioDeRonda;
    public bool EnemyJugando;


    public bool TurnoAcabo(Player player)
    {
        if(!TurnoAcabado) return false;

        TurnoAcabado = false;
        if (EnemyJugando) EnemyJugando=false;
        else EnemyJugando = true;
        return true;
    }

    public bool RondaAcabo(Player player)
    {
        if (TurnoAcabado) return false;
        TurnoAcabado = true;

        return true;
    }
}