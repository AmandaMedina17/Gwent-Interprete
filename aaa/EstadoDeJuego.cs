public class EstadoDeJuego
{
    public Player player;
    public Player enemy;
    public Tablero tablero;
    public Card card;

    public EstadoDeJuego(Player player, Player enemy)
    {
        this.player = player;
        this.enemy = enemy;
    }

}