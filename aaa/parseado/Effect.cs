public class Effect: claseMadre
{
    public static Dictionary<string, Effect> efectosGuardados;
    private Expresion.ExpresionLiteral name;
    public Declaracion Action;
    private Dictionary<string, Tipo> Params;
    private Token context;
    private Token targets;
    private bool parametrosAndTargets = false;
    Entorno entorno;

    public Effect(Expresion.ExpresionLiteral name, Declaracion Action, List<(Token, Token)> Params, Token Targets, Token Context)
    {
        this.name = name;
        this.Action = Action;
        this.Params = new Dictionary<string, Tipo>();
        this.targets = Targets is null ? new Token(TokenType.Identificador, "targets", null, 0) : Targets;
        this.context = Context is null ? new Token(TokenType.Identificador, "context", null, 0) : Context;


        foreach(var dupla in Params)
        {
            string type = dupla.Item2.Valor;
            Tipo tipo = Tipo.Bool;

            switch (type)
            {
                case "String": tipo = Tipo.Cadena; break;
                case "Number": tipo = Tipo.Numero; break;
                case "Bool": tipo = Tipo.Bool; break;
                default: throw new Exception("Tipo invalido");
            }

            this.Params.Add(dupla.Item1.Valor, tipo);
        }
    }

    public override bool Semantica()
    {
        bool noHayErrores = true;

        if (!(name.Type is Tipo.Cadena)) 
        {
            noHayErrores = false;
            System.Console.WriteLine("Invalid effect name type.");
        }
        else{
            string nombre = Convert.ToString(name.Ejecutar());

            if(!efectosGuardados.ContainsKey(nombre)) efectosGuardados.Add(nombre, this);
            else throw new Exception("Previously saved effect.");
        }

        if(!Action.Semantica())
        {
            noHayErrores = false;
            System.Console.WriteLine("Invalid effect body.");
        }
        return noHayErrores;
    }

   

    public override void Ejecutar()
    {
        if(!parametrosAndTargets) throw new Exception("Trying to run " + name + " effect whitout parameters");
        Action.Ejecutar();
    }

    public void TargetsAndParametros(List<(Token, Expresion)> parametros, Expresion targets)
    {
        if(!(this.context.Valor != "context")) entorno.asignar(this.context, Context.context);
        entorno.asignar(this.targets, targets);

        foreach (var par in parametros)
        {
            try
            {
                if (Params[par.Item1.Valor] is Tipo.Object) System.Console.WriteLine("Asegurar tipo");
                else if (!(par.Item2.type() is Tipo.Object || par.Item2.type() == Params[par.Item1.Valor])) System.Console.WriteLine("Invalid expression received in param");;
            }
            catch (KeyNotFoundException)
            {
                System.Console.WriteLine("Invalid param received");
            }

            entorno.asignar(par.Item1, par.Item2);
        }
    
        parametrosAndTargets = true;
    }
}


public static class WarehouseOfEffects
{
    public static Dictionary<string, EffectDelegate> efectos = new Dictionary<string, EffectDelegate>();

    public static void AgregarEfecto(string name, EffectDelegate effect)
    {
        efectos.Add(name, effect);
    }

public static EffectDelegate ObtenerEfecto(string name)
    {
        try
        {
            return efectos[name];
        }
        catch (System.ArgumentOutOfRangeException)
        {
            return efectos["Empty"];
        }
    }

    //Efectos
    public static void EmptyEffect(EstadoDeJuego estadoDeJuego)
    {
        
    }
    public static void DrawCard(EstadoDeJuego estadoDeJuego)
    {
        if (!(estadoDeJuego.player.Deck.Count == 0)) estadoDeJuego.player.Hand.Add(estadoDeJuego.player.Deck[0]);
        else System.Console.WriteLine("There are no more cards in the deck.");
    }

    public static void EliminateMostPowerfullCard(EstadoDeJuego estadoDeJuego)
    {
        BaseCard mostPowerfulCard = null;
        int maxPower = int.MinValue;

        if(estadoDeJuego.enemy.zonasdelplayer.MeleeZone.Count > 0)
        {
            FindMostPowerfulCardInListMelee();
        }
        else if(estadoDeJuego.enemy.zonasdelplayer.RangedZone.Count >0)
        {
            FindMostPowerfulCardInListRange();
        }
        
        else if(estadoDeJuego.enemy.zonasdelplayer.SiegeZone.Count > 0)
        {
            FindMostPowerfulCardInListSiege();
        }
      
        void FindMostPowerfulCardInListMelee()
        {
            for (int i = 0; i < estadoDeJuego.enemy.zonasdelplayer.MeleeZone.Count; i++)
            {
           
                if(estadoDeJuego.enemy.zonasdelplayer.MeleeZone[i].Power  > maxPower)
                {
                    mostPowerfulCard = estadoDeJuego.enemy.zonasdelplayer.MeleeZone[i];
                    maxPower = estadoDeJuego.enemy.zonasdelplayer.MeleeZone[i].Power;
                }
            }
        }
 
        void FindMostPowerfulCardInListRange()
        {
            for (int i = 0; i < estadoDeJuego.enemy.zonasdelplayer.RangedZone.Count; i++)
            {
          
                if(estadoDeJuego.enemy.zonasdelplayer.RangedZone[i].Power > maxPower)
                {
                    mostPowerfulCard = estadoDeJuego.enemy.zonasdelplayer.RangedZone[i];
                    maxPower = estadoDeJuego.enemy.zonasdelplayer.RangedZone[i].Power;
                }
            }
        }

        void FindMostPowerfulCardInListSiege()
        {
            for (int i = 0; i < estadoDeJuego.enemy.zonasdelplayer.SiegeZone.Count; i++)
            {
            
                if(estadoDeJuego.enemy.zonasdelplayer.SiegeZone[i].Power > maxPower)
                {
                    mostPowerfulCard = estadoDeJuego.enemy.zonasdelplayer.SiegeZone[i];
                    maxPower = estadoDeJuego.enemy.zonasdelplayer.SiegeZone[i].Power;
                }
            }    
        }
        if(mostPowerfulCard != null)
        {
            if(estadoDeJuego.enemy.zonasdelplayer.MeleeZone.Contains(mostPowerfulCard))
            {
                estadoDeJuego.enemy.zonasdelplayer.MeleeZone.Remove(mostPowerfulCard);
            }
            else if(estadoDeJuego.enemy.zonasdelplayer.RangedZone.Contains(mostPowerfulCard))
            {
                estadoDeJuego.enemy.zonasdelplayer.RangedZone.Remove(mostPowerfulCard);
            }
            else if(estadoDeJuego.enemy.zonasdelplayer.SiegeZone.Contains(mostPowerfulCard))
            {
                estadoDeJuego.enemy.zonasdelplayer.SiegeZone.Remove(mostPowerfulCard);
            }
        }
    }

     public static void CalculateAverage(EstadoDeJuego estadoDeJuego)
    {
        List<UnitCard> list = new List<UnitCard>();
        double Total = 0;

        for (int i = 0; i < estadoDeJuego.player.zonasdelplayer.listaDeLasZonas.Count; i++)
        {
            Average(estadoDeJuego.player.zonasdelplayer.listaDeLasZonas[i], list, ref Total);
            Average(estadoDeJuego.enemy.zonasdelplayer.listaDeLasZonas[i], list, ref Total);
        }

        double average = Total / list.Count;

        foreach (UnitCard item in list)
        {
            if (item.worth == Worth.Silver) item.Power = (int)average;
        }
    }

    static void Average(List<BaseCard> listToCheck, List<UnitCard> listToStore, ref double power)
    {
        foreach (BaseCard item in listToCheck)
        {
            if (item is UnitCard unit)
            {
                listToStore.Add(unit);
                power += unit.Power;
            }
        }
    }

    public static void MultiplyPower(EstadoDeJuego estadoDeJuego)
    {
        int repetitions = 0; 
        List<UnitCard> list = new List<UnitCard>();

        for (int i = 0; i < estadoDeJuego.player.zonasdelplayer.listaDeLasZonas.Count; i++)
            FindMatchingCards(estadoDeJuego.player.zonasdelplayer.listaDeLasZonas[i], (UnitCard)estadoDeJuego.card, list, ref repetitions);

        foreach (UnitCard item in list)
        {
            item.Power = repetitions * item.InitialPower;
        }
    }

    static void FindMatchingCards(List<BaseCard> list, UnitCard card, List<UnitCard> listToSave, ref int repetitions)
    {
        foreach (BaseCard item in list)
        {
            if (card.Equals(item))
            {
                repetitions++;
                listToSave.Add((UnitCard)item);
            }
        }
    }


    public static void RemoveLeastPowerfulCard(EstadoDeJuego estadoDeJuego)
    {
        BaseCard leastPowerfulCard = null;
        int minPower = int.MaxValue;

        if(estadoDeJuego.enemy.zonasdelplayer.MeleeZone.Count > 0)
        {
            FindleastPowerfulCardInListMelee();
        }
        else if(estadoDeJuego.enemy.zonasdelplayer.RangedZone.Count >0)
        {
            FindleastPowerfulCardInListRange();
        }
        
        else if(estadoDeJuego.enemy.zonasdelplayer.SiegeZone.Count > 0)
        {
            FindleastPowerfulCardInListSiege();
        }
      
        void FindleastPowerfulCardInListMelee()
        {
            for (int i = 0; i < estadoDeJuego.enemy.zonasdelplayer.MeleeZone.Count; i++)
            {
           
                if(estadoDeJuego.enemy.zonasdelplayer.MeleeZone[i].Power  < minPower)
                {
                    leastPowerfulCard = estadoDeJuego.enemy.zonasdelplayer.MeleeZone[i];
                    minPower = estadoDeJuego.enemy.zonasdelplayer.MeleeZone[i].Power;
                }
            }
        }
 
        void FindleastPowerfulCardInListRange()
        {
            for (int i = 0; i < estadoDeJuego.enemy.zonasdelplayer.RangedZone.Count; i++)
            {
          
                if(estadoDeJuego.enemy.zonasdelplayer.RangedZone[i].Power < minPower)
                {
                    leastPowerfulCard = estadoDeJuego.enemy.zonasdelplayer.RangedZone[i];
                    minPower = estadoDeJuego.enemy.zonasdelplayer.RangedZone[i].Power;
                }
            }
        }

        void FindleastPowerfulCardInListSiege()
        {
            for (int i = 0; i < estadoDeJuego.enemy.zonasdelplayer.SiegeZone.Count; i++)
            {
            
                if(estadoDeJuego.enemy.zonasdelplayer.SiegeZone[i].Power < minPower)
                {
                    leastPowerfulCard = estadoDeJuego.enemy.zonasdelplayer.SiegeZone[i];
                    minPower = estadoDeJuego.enemy.zonasdelplayer.SiegeZone[i].Power;
                }
            }    
        }
        if(leastPowerfulCard != null)
        {
            if(estadoDeJuego.enemy.zonasdelplayer.MeleeZone.Contains(leastPowerfulCard))
            {
                estadoDeJuego.enemy.zonasdelplayer.MeleeZone.Remove(leastPowerfulCard);
            }
            else if(estadoDeJuego.enemy.zonasdelplayer.RangedZone.Contains(leastPowerfulCard))
            {
                estadoDeJuego.enemy.zonasdelplayer.RangedZone.Remove(leastPowerfulCard);
            }
            else if(estadoDeJuego.enemy.zonasdelplayer.SiegeZone.Contains(leastPowerfulCard))
            {
                estadoDeJuego.enemy.zonasdelplayer.SiegeZone.Remove(leastPowerfulCard);
            }
        }
    }

    public static void ClearRow(EstadoDeJuego estadoDeJuego)
    {
        List<List<BaseCard>> rows = new List<List<BaseCard>> {estadoDeJuego.enemy.zonasdelplayer.MeleeZone, estadoDeJuego.enemy.zonasdelplayer.RangedZone, estadoDeJuego.enemy.zonasdelplayer.SiegeZone};

        List<BaseCard> smallesNonEmptyRow = null;
        int minCount = int.MaxValue;

        for (int i = 0; i < rows.Count; i++)
        {
            if(rows[i].Count > 0 && rows[i].Count < minCount)
            {
                smallesNonEmptyRow = rows[i];
                minCount = rows[i].Count;
            }
        }
        if(smallesNonEmptyRow != null)
        { 
            smallesNonEmptyRow.Clear();
        }
    }

    public static void AddIncrease(EstadoDeJuego estadoDeJuego)
    {
        if(estadoDeJuego.card.destinations[0] == Zonas.Melee) AddPowerMelee();
        if(estadoDeJuego.card.destinations[0] == Zonas.Range) AddPowerRange();
        if(estadoDeJuego.card.destinations[0] == Zonas.Siege) AddPowerSiege();
        
        void AddPowerMelee()
        {
            if(estadoDeJuego.player.zonasdelplayer.MeleeZone.Count > 0)
            {
                for (int i = 0; i < estadoDeJuego.player.zonasdelplayer.MeleeZone.Count; i++)
                {
                estadoDeJuego.player.zonasdelplayer.MeleeZone[i].Power += 1;
                };
            }
        } 
        
        void AddPowerRange()
        {
            if(estadoDeJuego.player.zonasdelplayer.RangedZone.Count > 0)
            {
                for (int i = 0; i < estadoDeJuego.player.zonasdelplayer.RangedZone.Count; i++)
                {
                estadoDeJuego.player.zonasdelplayer.RangedZone[i].Power += 1;
                }
            }
        } 
        
        void AddPowerSiege()
        {
            if(estadoDeJuego.player.zonasdelplayer.SiegeZone.Count > 0)
            {
                for (int i = 0; i < estadoDeJuego.player.zonasdelplayer.SiegeZone.Count; i++)
                {
                estadoDeJuego.player.zonasdelplayer.SiegeZone[i].Power += 1;
                }
            }
        } 
    }
}

public delegate void EffectDelegate(EstadoDeJuego estadoDeJuego);