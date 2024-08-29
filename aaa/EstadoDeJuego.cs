public class EstadoDeJuego
{
    public Player player;
    public Player enemy;
    public Tablero tablero;
    public BaseCard card;
    public List<BaseCard> lista;


    public EstadoDeJuego(Player player, Player enemy)
    {
        this.player = player;
        this.enemy = enemy;
    }

    public EstadoDeJuego UpdatePlayerInstance(List<BaseCard> position, BaseCard card)
    {
        lista = position;
        this.card = card;

        return this;
    }

}