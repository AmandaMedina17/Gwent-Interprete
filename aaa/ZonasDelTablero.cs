public class ZonasdelTablero
{
    public List<BaseCard> MeleeZone = new List<BaseCard>();
    public List<BaseCard> RangedZone = new List<BaseCard>();
    public List<BaseCard> SiegeZone = new List<BaseCard>();
    public List<BaseCard> Cementerio = new List<BaseCard>();
    public List<bool> Clearances { get => Clearances; private set => Clearances = value; }
    public BaseCard[] Increase = new BaseCard[3];
    public List<BaseCard> IncreaseMeleeZone = new List<BaseCard>(1);
    public List<BaseCard> IncreaseRangedZone = new List<BaseCard>(1);
    public List<BaseCard> IncreaseSiegeZone = new List<BaseCard>(1);
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

    public bool TryAdd(BaseCard card, List<BaseCard> list, int index = 0)
    {
        var zonetype = GetZoneType(list);

        switch (card.TipoDeCarta)
        {
            case TipoDeCarta.Unit:
                return AddUnitCard(list, index, card);

            case TipoDeCarta.Clearance:
                return AddClearanceCard(list, index, zonetype, card);

            case TipoDeCarta.Increase:
                return AddIncreaseCard(zonetype, card);

            default:
                return false;
        }
    }

    private Zonas GetZoneType(List<BaseCard> list)
    {
        if (list == MeleeZone) return Zonas.Melee;
        if (list == RangedZone) return Zonas.Range;
        if (list == SiegeZone) return Zonas.Siege;
        throw new ArgumentException("Invalid zone list");
    }
    private bool AddUnitCard(List<BaseCard> list, int index, BaseCard card)
    {
        if (IsSlotEmpty(list, index))
        {
            list[index] = card;
            return true;
        }
        return false;
    }

    private bool AddClearanceCard(List<BaseCard> list, int index, Zonas zoneType, BaseCard card)
    {
        if (IsSlotEmpty(list, index))
        {
            list[index] = card;
            Clearances[(int)zoneType] = true;
            return true;
        }
        return false;
    }

    private List<BaseCard> GetIncreaseListForZone(Zonas zoneType)
    {
        switch (zoneType)
        {
            case Zonas.Melee:
                return IncreaseMeleeZone;
            case Zonas.Range:
                return IncreaseRangedZone;
            case Zonas.Siege:
                return IncreaseSiegeZone;
            default:
                throw new ArgumentException("Invalid zone type");
        }
    }

    private bool AddIncreaseCard(Zonas zoneType, BaseCard card)
    {
        List<BaseCard> IncreaseList = GetIncreaseListForZone(zoneType);
        if (IsSlotEmpty(IncreaseList, 0))
        {
            IncreaseList[0] = card;
            return true;
        }
        return false;
    }

    private bool IsSlotEmpty(List<BaseCard> list, int index)
    {
        return list[index] == null;
    }


}

public enum Zonas
{
    Melee, Range, Siege,
}