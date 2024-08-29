
public class ClimateCard : BaseCard
{
    public ClimateCard(string Name, int InitialPower, Faction Faction, TipoDeCarta TipoDeCarta, List<Zonas> destinations, EffectDelegate Effect = null) : base(Name, InitialPower, Faction, TipoDeCarta, destinations, Effect)
    {
    }

    public override bool Effect(EstadoDeJuego estadoDeJuego) 
    {
        ClimateEffect(Zonas.Melee); 
        ClimateEffect(Zonas.Range); 
        ClimateEffect(Zonas.Siege); 
        return true;
    }

    private void ClimateEffect(Zonas range)
    {
        if (this.destinations.Contains(range))
        {
            List<BaseCard> _Griegos;
            List<BaseCard> _Nordicos;

            if (range == Zonas.Melee)
            {
                _Griegos = Player.Griegos.zonasdelplayer.MeleeZone;
                _Nordicos = Player.Nordicos.zonasdelplayer.MeleeZone;
            }
            else if (range == Zonas.Range)
            {
                _Griegos = Player.Griegos.zonasdelplayer.RangedZone;
                _Nordicos = Player.Nordicos.zonasdelplayer.RangedZone;
            }
            else 
            {
                _Griegos = Player.Griegos.zonasdelplayer.SiegeZone;
                _Nordicos = Player.Nordicos.zonasdelplayer.SiegeZone;
            }
            
            

            for (int i = 0; i < Math.Min(_Griegos.Count, _Nordicos.Count); i++) 
            {
                if (_Nordicos[i] is UnitCard nordicard && nordicard.worth == Worth.Silver)
                {
                    nordicard.Power -= nordicard.Power < InitialPower ? nordicard.Power : InitialPower;
                }
                if (_Griegos[i] is UnitCard greekcard && greekcard.worth == Worth.Silver)
                {
                    greekcard.Power -= greekcard.Power < InitialPower ? greekcard.Power : InitialPower;
                }
            }
        }
    }
}