using System.Reflection.Metadata;

public class Context : Expresion
{
    public Tablero board;
    public Dictionary<Faction, Player> Jugadores = new Dictionary<Faction, Player> {{Faction.Greek_Gods, Player.Griegos}, {Faction.Nordic_Gods, Player.Nordicos}};

    public Context()
    {
        board = new Tablero();
    }

    static Context _context;
    public static Context context // Asegura que solo exista una instancia de Context en todo el juego.
    {
        get
        {
            if (_context is null) _context = new Context();
            return _context;
        }
    }

    //Propiedades del Context//

    public int TriggerPlayer => board.JugadordelMomento().Id;  //Devuelve el Id del jugador que desencadeno el efecto. 

    public MetodosListas Board()  //Devulve una lista con todas las cartas del tablero
    {
        List<BaseCard> lista = new List<BaseCard>();
        foreach (var jugador in Jugadores.Values)
        {
            foreach (var item in jugador.zonasdelplayer.listaDeLasZonas)
            {
                lista.AddRange(item);
            }
        }
        foreach (var item in Tablero.tablero.Climate)
        {
            lista.Add(item);
        }
        return new MetodosListas(lista);
    }

    public MetodosListas HandOfPlayer(Player player)
    {
        return new MetodosListas(player.Hand);
    }

    public MetodosListas FieldOfPlayer(Player player)
    {
        List<BaseCard> lista = new List<BaseCard>();
        foreach (var item in player.zonasdelplayer.listaDeLasZonas)
        {
            lista.AddRange(item);
        }
        return new MetodosListas(lista);
    }

    public MetodosListas GraveyardOfPlayer(Player player)
    {
        return new MetodosListas(player.zonasdelplayer.Cementerio);
    }

    public MetodosListas DeckOfPlayer(Player player)
    {
        return new MetodosListas(player.Deck);
    }

    public MetodosListas Hand()
    {
        return HandOfPlayer(board.JugadordelMomento());
    }

    public MetodosListas OtherHand()
    {
        return HandOfPlayer(board.EnemigodelMomento());
    }

    public MetodosListas Deck()
    {
        return DeckOfPlayer(board.JugadordelMomento());
    }

    public MetodosListas OtherDeck()
    {
        return DeckOfPlayer(board.EnemigodelMomento());
    }
    
    public MetodosListas Field()
    {
        return FieldOfPlayer(board.JugadordelMomento());
    }

    public MetodosListas OtherField()
    {
        return FieldOfPlayer(board.EnemigodelMomento());
    }
    
    public MetodosListas Graveyard()
    {
        return GraveyardOfPlayer(board.JugadordelMomento());
    }

     public MetodosListas OtherGraveyard()
    {
        return GraveyardOfPlayer(board.EnemigodelMomento());
    }

    public override object Ejecutar()
    {
        throw new NotImplementedException();
    }

    public override Tipo type()
    {
        throw new NotImplementedException();
    }

    public override bool Semantica()
    {
        throw new NotImplementedException();
    }
}

