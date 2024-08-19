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

    public Player(Faction a)
    {
        this.nombre = a;
        Hand = new List<Card>(10);
        Deck = new List<Card>(30);

    }

}