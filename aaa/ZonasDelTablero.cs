public class ZonasdelTablero
{
    public List<Card> MeleeZone = new List<Card>();
    public List<Card> RangedZone = new List<Card>();
    public List<Card> SiegeZone = new List<Card>();
    public List<Card> Cementerio = new List<Card>();
    public List<Card> IncreaseMeleeZone = new List<Card>();
    public List<Card> IncreaseRangedZone = new List<Card>();
    public List<Card> IncreaseSiegeZone = new List<Card>();
    

}

public enum Zonas
{
    Melee, Range, Siege,
}