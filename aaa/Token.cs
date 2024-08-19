using System.Reflection;
using System.Collections.Generic;

public class Token
{
    public string Valor{ get; private set; }
    public TokenType Tipo{ get; private set; }
    public object Literal{ get; private set; }
    public int linea{ get; private set; }
    
    public Token(TokenType Tipo, string Valor, Object Literal, int linea)
    {
        this.Valor = Valor;
        this.Tipo = Tipo;
        this.linea = linea;
        this.Literal = Literal;
    }

    public override string ToString()
    {
        return Valor + ": ";
    }
}

public enum TokenType {
// Fichas de un solo carácter.
    Parentesis_abierto, Parentesis_cerrado, Llave_abierta, Llave_cerrada, Corchete_Abierto, Corchete_Cerrado, 
    Coma, Punto, Menos, Más, Punto_y_coma, Barra_oblicua, Asterizco, Doble_punto,

    // Tokens de uno o doscaracteres. 
    Bang, Bang_igual, Igual, Igual_igual, Mayor, Mayor_igual, Menor, Menor_igual, Slach,
    Mas_mas, Concatenacion, Concatenacion_Espaciado, Lambda, 

    // Literales.
    Identificador, Cadena, Número,

    // Palabras clave.
    And, Class, Else, False, For, If, Or, Return, True, Var, While, Fun, Print,

    Effect, Name, Params, Amount, Action, Card, Type, Faction, Power, Range, OnActivation, Selector, 
    Source, Single, Predicate, PosAction, Effect_card, Targets, Context,

    Fin, Null,

}


public enum Source
{
    board, hand, otherHand, deck, otherDeck, field, otherField, parent
}

public enum Tipo
{
    Numero,
    Cadena,
    Bool,
    nil,

}
