public class Player
{ 
    public int Id;
    public Faction nombre;
    public List<BaseCard> Deck;
    public List<BaseCard> Hand;
    public LeaderCard leader;
    public int TotalPoint =0;
    public bool CanPlay = false;
    public ZonasdelTablero zonasdelplayer;
    public EstadoDeJuego estadoDeJuego;
    public bool turnoPasado = false;
    public Dictionary<Zonas, List<BaseCard>> ZonaConLista { get; private set; }
    
    private static Player _griegos;
    private static Player _nordicos;

    public static Player Griegos
    {
        get 
        {
            if (_griegos == null)
            {
                _griegos = new Player(Faction.Greek_Gods);
                _griegos.estadoDeJuego = new EstadoDeJuego(_griegos, Nordicos);
                _griegos.zonasdelplayer = new ZonasdelTablero(_griegos);
                _griegos.ZonaConLista = new Dictionary<Zonas, List<BaseCard>>
                {
                    {Zonas.Melee,  _griegos.zonasdelplayer.MeleeZone},
                    {Zonas.Range,  _griegos.zonasdelplayer.RangedZone},
                    {Zonas.Siege, _griegos.zonasdelplayer.SiegeZone}
                };
            }
            return _griegos;
        }
    }

    public static Player Nordicos
    {
        get 
        {
            if (_nordicos == null)
            {
                _nordicos = new Player(Faction.Nordic_Gods);
                _nordicos.estadoDeJuego = new EstadoDeJuego(_nordicos, Griegos);
                _nordicos.zonasdelplayer = new ZonasdelTablero(_nordicos);
                _nordicos.ZonaConLista = new Dictionary<Zonas, List<BaseCard>>
                {
                    {Zonas.Melee,  _nordicos.zonasdelplayer.MeleeZone},
                    {Zonas.Range,  _nordicos.zonasdelplayer.RangedZone},
                    {Zonas.Siege, _nordicos.zonasdelplayer.SiegeZone}
                };
            }
            return _nordicos;
        }
    }


    public Player(Faction a)
    {
        this.nombre = a;
        Hand = new List<BaseCard>(10);
        Deck = new List<BaseCard>(30);
        this.Id = (int)nombre;
        

    }

    private void InitializeZonaConLista()
    {
        
    }

    public bool SeJugoCarta(int originPosition, int targetPosition, Zonas range, out bool noEfecto)
    {
        noEfecto = false;

        if(estadoDeJuego is null) estadoDeJuego = new EstadoDeJuego(this, Tablero.tablero.EnemigodelMomento());

        if (!(this.Hand[originPosition] is BaseCard card) || card is BaitCard || Tablero.tablero.RondaAcabada) 
        {
            return false;
        }

        if (card is ClimateCard climate)
        {
            Tablero.tablero.Climate[targetPosition] = climate;
        }
        else if (!this.zonasdelplayer.TryAdd(card, this.ZonaConLista[range], targetPosition))
        {
            return false;
        }

        try
        {
            noEfecto = !card.Effect(estadoDeJuego.UpdatePlayerInstance(this.ZonaConLista[range], card));
        }
        catch
        {
            WarehouseOfEffects.CogerEfecto(card.Name)(estadoDeJuego.UpdatePlayerInstance(this.ZonaConLista[range], card));
            noEfecto = false;
        }
        Tablero.tablero.TurnoAcabado = true;
        Tablero.tablero.CalculateTotalDamage();
        return true;
    }

    // public static Player CrearJugador(Faction faction)
    // {
    //     Player player = new Player(faction);
    //     Player enemy = faction == Faction.Nordic_Gods ? Griegos:Nordicos;
    //     player.estadoDeJuego = new EstadoDeJuego(player, enemy);
    //     player.zonasdelplayer = new ZonasdelTablero(player);
        
    //     return player;
    // }
}