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

    public ZonasdelTablero(Player player)
    {
        listaDeLasZonas = new List<List<BaseCard>> {MeleeZone, RangedZone, SiegeZone, IncreaseMeleeZone, IncreaseRangedZone, IncreaseSiegeZone};
    }

}

public enum Zonas
{
    Melee, Range, Siege,
}