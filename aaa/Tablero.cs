public class Tablero
{
    public static Tablero tablero = new Tablero();
    public Dictionary<string, List<BaseCard>> Zonas;
    public int Ronda = 0;
    public bool TurnoAcabado;  
    public bool RondaAcabada;
    public bool GriegosJugando;
    public bool NuevaRonda = true;
    public List<BaseCard> Climate = new List<BaseCard>(3);
    public int RondasGanadasGriegos = 0;
    public int RondasGanadasNordicos = 0;


    public bool TurnoAcabo(Player player)
    {
        if(!TurnoAcabado) return false;

        TurnoAcabado = false;
        if (GriegosJugando) GriegosJugando = Player.Nordicos.turnoPasado;
        else GriegosJugando = Player.Griegos.turnoPasado;
        return true;
    }

    public bool RondaAcabo(Player player)
    {
        if (TurnoAcabado) return false;
        TurnoAcabado = true;
        if (GriegosJugando) Player.Griegos.turnoPasado = true;
        else Player.Nordicos.turnoPasado = true;   
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

    public void ComenzarRonda()
    {
        Barajador.BarajarCartas(Player.Griegos.Deck);
        Barajador.BarajarCartas(Player.Nordicos.Deck);

        if(NuevaRonda)
        {
            if(Ronda == 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    Player.Griegos.Hand.Add(Player.Griegos.Deck[i]);
                    Player.Griegos.Hand.Add(Player.Nordicos.Deck[i]);

                }
            }
            else 
            {
                for (int i = 0; i < 2; i++)
                {
                    Player.Griegos.Hand.Add(Player.Griegos.Deck[i]);
                    Player.Griegos.Hand.Add(Player.Nordicos.Deck[i]);

                }
            }
            Ronda ++;
            NuevaRonda = false;
        }
    }

    public string GanadorDeRonda()
    {
        string Ganador = null;
        if(Player.Griegos.turnoPasado && Player.Nordicos.turnoPasado)
        {
            if(Player.Griegos.TotalPoint < Player.Nordicos.TotalPoint)
            {
                RondasGanadasNordicos++;
                GriegosJugando = false;
                Ganador = Player.Nordicos.nombre.ToString();
            }
            else if(Player.Griegos.TotalPoint > Player.Nordicos.TotalPoint)
            {
                RondasGanadasGriegos++;
                GriegosJugando = true;
                Ganador = Player.Griegos.nombre.ToString();
            }
            else
            {
                if(!GriegosJugando) GriegosJugando = true;
                else GriegosJugando = false;
                Ganador = "Empate";
            }
            Player.Griegos.turnoPasado = false;
            Player.Nordicos.turnoPasado = false;
            NuevaRonda = true;
            Player.Griegos.TotalPoint = 0; 
            Player.Nordicos.TotalPoint = 0;

        }
        return Ganador;
    }

    public string GanadorDeLaPartida()
    {
        string Ganador = "";
        if (Ronda >= 2)
        {
            if(RondasGanadasGriegos >=2)
            {
                Ganador = Player.Griegos.nombre.ToString();
            }
            else if(RondasGanadasNordicos >=2)
            {
                Ganador = Player.Nordicos.nombre.ToString();
            }
        }
        return Ganador;
    }

   

}