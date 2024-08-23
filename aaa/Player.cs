public class Player
{ 
    public int Id;
    public Faction nombre;
    public List<Card> Deck;
    public List<Card> Hand;
    public LeaderCard leader;
    public int TotalPoint =0;
    public bool CanPlay = false;
    public ZonasdelTablero zonasdelplayer;
    public EstadoDeJuego estadoDeJuego;
    public bool turnoPasado = false;
    
    private static Player _griegos;
    private static Player _nordicos;

    public static Player Griegos => _griegos ??= CrearJugador(Faction.Greek_Gods);
    public static Player Nordicos => _nordicos ??= CrearJugador(Faction.Nordic_Gods);

    public Player(Faction a)
    {
        this.nombre = a;
        Hand = new List<Card>(10);
        Deck = new List<Card>(30);
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