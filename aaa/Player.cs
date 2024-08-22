public class Player
{ 
    public Faction nombre;
    public List<Card> Deck;
    public List<Card> Hand;
    //public Leader leader;
    public int TotalPoint =0;
    public bool CanPlay = false;
    public ZonasdelTablero zonasdelplayer;
    public Context contexto;
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

    }

    public static Player CrearJugador(Faction faction)
    {
        Player player = new Player(faction);
        Player enemy = faction == Faction.Nordic_Gods ? Griegos:Nordicos;
        player.contexto = new Context(player, enemy);
        player.zonasdelplayer = new ZonasdelTablero();
        
        return player;
    }
}