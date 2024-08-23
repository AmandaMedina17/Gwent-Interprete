public class Tablero
{
    public static Tablero tablero = new Tablero();
    public Dictionary<string, List<Card>> Zonas;
    private int _ronda;
    public int Ronda { get => _ronda; private set => _ronda = value; }
    public bool TurnoAcabado;  
    public bool CambioDeRonda;
    public bool GriegosJugando;
    public List<Card> Climate = new List<Card>(3);


    public bool TurnoAcabo(Player player)
    {
        if(!TurnoAcabado) return false;

        TurnoAcabado = false;
        if (GriegosJugando) GriegosJugando=false;
        else GriegosJugando = true;
        return true;
    }

    public bool RondaAcabo(Player player)
    {
        if (TurnoAcabado) return false;
        TurnoAcabado = true;

        return true;
    }

    public Player JugadordelMomento()
    {
        if(GriegosJugando) return Player.Griegos;
        else return Player.Nordicos;
    }

    public Player EnemigodelMomento()
    {
        if(GriegosJugando) return Player.Nordicos;
        else return Player.Griegos;
    }
}