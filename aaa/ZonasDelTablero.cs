public class ZonasdelTablero
{
    public List<BaseCard> MeleeZone = new List<BaseCard>();
    public List<BaseCard> RangedZone = new List<BaseCard>();
    public List<BaseCard> SiegeZone = new List<BaseCard>();
    public List<BaseCard> Cementerio = new List<BaseCard>();
    public List<BaseCard> IncreaseMeleeZone = new List<BaseCard>();
    public List<BaseCard> IncreaseRangedZone = new List<BaseCard>();
    public List<BaseCard> IncreaseSiegeZone = new List<BaseCard>();
    public List<List<BaseCard>> listaDeLasZonas;
    public Player player;

    public ZonasdelTablero(Player player)
    {
        listaDeLasZonas = new List<List<BaseCard>> {MeleeZone, RangedZone, SiegeZone, IncreaseMeleeZone, IncreaseRangedZone, IncreaseSiegeZone};
        this.player = player;
    }

     public void MandarAlCementerio(BaseCard card = null, List<BaseCard> list = null)
    {
        // Caso 1: Se proporciona una lista completa para enviar al cementerio
        if (card == null && list != null)
        {
            for (int i = 0; i < list.Count; i++)
            {
                MandarAlCementerio(list[i], list);
            }
            return;
        }

        // Caso 2: Se proporciona una carta pero no una lista específica
        if (card != null && list == null)
        {
            if (IncreaseMeleeZone.Contains(card))
                MandarAlCementerio(card, IncreaseMeleeZone);
            else if (IncreaseMeleeZone.Contains(card))
                MandarAlCementerio(card, IncreaseMeleeZone);
            else if (IncreaseMeleeZone.Contains(card))
                MandarAlCementerio(card, IncreaseMeleeZone);
            else if (Tablero.tablero.Climate.Contains(card))
                MandarAlCementerio(card, Tablero.tablero.Climate);
            else
                foreach (var zone in listaDeLasZonas)
                    if (zone.Contains(card))
                    {
                        MandarAlCementerio(card, zone);
                        return;
                    }
            return;
        }

        // Caso 3: Se proporcionan tanto la carta como la lista
        if (card != null && list != null)
        {
           Cementerio.Add(card);
        }
    }

}

public enum Zonas
{
    Melee, Range, Siege,
}