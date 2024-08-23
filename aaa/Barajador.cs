public class Barajador
{
    private static Random rng = new Random();

    public static void BarajarCartas<T>(List<T> lista)
    {
        int n = lista.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T valor = lista[k];
            lista[k] = lista[n];
            lista[n] = valor;
        }
    }
}