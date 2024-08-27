public class Player
{ 
    public int Id;
    public Faction nombre;
    public List<BaseCard> Deck;
    public List<BaseCard> Hand;
    public LeaderCard leader;
    public int TotalPoint =0;
    public bool CanPlay = false;
    public ZonasdelTablero zonasdelplayer;
    public EstadoDeJuego estadoDeJuego;
    public bool turnoPasado = false;
    
    private static Player _griegos;
    private static Player _nordicos;

    public static Player Griegos
    {
        get 
        {
            if (_griegos == null)
            {
                _griegos = new Player(Faction.Greek_Gods);
                _griegos.estadoDeJuego = new EstadoDeJuego(_griegos, Nordicos);
                _griegos.zonasdelplayer = new ZonasdelTablero(_griegos);
                
            }
            return _griegos;
        }
    }

    public static Player Nordicos
    {
        get 
        {
            if (_nordicos == null)
            {
                _nordicos = new Player(Faction.Nordic_Gods);
                _nordicos.estadoDeJuego = new EstadoDeJuego(_nordicos, Griegos);
                _nordicos.zonasdelplayer = new ZonasdelTablero(_nordicos);
            }
            return _nordicos;
        }
    }


    public Player(Faction a)
    {
        this.nombre = a;
        Hand = new List<BaseCard>(10);
        Deck = new List<BaseCard>(30);
        this.Id = (int)nombre;

    }

    public static Player CrearJugador(Faction faction)
    {
        Player player = new Player(faction);
        Player enemy = faction == Faction.Nordic_Gods ? Griegos:Nordicos;
        player.estadoDeJuego = new EstadoDeJuego(player, enemy);
        player.zonasdelplayer = new ZonasdelTablero(player);
        
        return player;
    }
}