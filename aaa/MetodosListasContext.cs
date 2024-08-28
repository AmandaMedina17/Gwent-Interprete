using System.Collections;

public class MetodosListas : IList<BaseCard>
{
    List<BaseCard> lista;
    Tablero tablero;
    Player player;

    public MetodosListas()
    {
        lista = new List<BaseCard>();
    }

    public MetodosListas(List<BaseCard> baseCards, Player player=null)
    {
        lista = baseCards;
        if (player is null) tablero = Tablero.tablero;
        else this.player = player;
    }

    BaseCard IList<BaseCard>.this[int index] { get => lista[index]; set => lista[index] = value; }

    public int Count => lista.Count;

    public bool IsReadOnly => false;

    public MetodosListas Find(Predicate<BaseCard> predicate)
    {
        return  new MetodosListas(lista.FindAll(predicate));
    }

    public void Push(BaseCard card)
    {
        lista.Add(card);
    }

    public void SendBottom(BaseCard card)
    {
        lista.Insert(lista.Count, card);
    }

    public BaseCard Pop()
    {
        lista.Remove(lista[0]);
        return lista[0];
    }

     public void Remove(BaseCard card)
    {
        if (player is null)
        {
            (card.Faction == Faction.Greek_Gods ? Player.Griegos.zonasdelplayer : Player.Griegos.zonasdelplayer).MandarAlCementerio(card);
        }
        else player.zonasdelplayer.MandarAlCementerio(card);
        lista.Remove(card);

    }

    public void Shuffle()
    {
        Barajador.BarajarCartas(lista);
    }
    public void Add(BaseCard item)
    {
        throw new NotImplementedException();
    }

    public void Clear()
    {
        lista.Clear();
    }

    public bool Contains(BaseCard item)
    {
        return lista.Contains(item);
    }

    public void CopyTo(BaseCard[] array, int arrayIndex)
    {
        lista.CopyTo(array, arrayIndex);
    }

    public IEnumerator<BaseCard> GetEnumerator()
    {
        return lista.GetEnumerator();
    }

    public int IndexOf(BaseCard item)
    {
        return lista.IndexOf(item);
    }

    public void Insert(int index, BaseCard item)
    {
        lista.Insert(index, item);
    }

    bool ICollection<BaseCard>.Remove(BaseCard item)
    {
        if (this.Contains(item)) this.Remove(item);

        return this.Contains(item);
    }

    public void RemoveAt(int index)
    {
        if (player is null)
        {
            (lista[index].Faction == Faction.Greek_Gods ? Player.Griegos.zonasdelplayer : Player.Nordicos.zonasdelplayer).MandarAlCementerio(lista[index]);
        }
        else player.zonasdelplayer.MandarAlCementerio(lista[index]);

        lista.RemoveAt(index);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }
}