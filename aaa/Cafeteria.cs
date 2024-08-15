/*namespace Weboo.Examen
{
    class Program
    {
        static void Main(string[] args)
        {
            //          TABLA CON DEMORA EN PLATO POR COCINERO
            //              _________________________________________
            //   __________|_cocinero_#0_|_cocinero_#1_|_cocinero_#2_|
            //  | plato #0 |  1 minuto   |  3 minutos  |  5 minutos  |
            //  | plato #1 |  2 minutos  |  4 minutos  |  1 minuto   |
            //  | plato #2 |  3 minutos  |  5 minutos  |  2 minutos  |
            //  | plato #3 |  4 minutos  |  1 minuto   |  3 minutos  |
            //  | plato #4 |  5 minutos  |  2 minutos  |  4 minutos  |
            //
            int[,] demoraEnPlatoPorCocinero = { { 1, 3, 5 },
                                                { 2, 4, 1 },
                                                { 3, 5, 2 },
                                                { 4, 1, 3 },
                                                { 5, 2, 4 } };


            // Se crea una cafetería con 5 platos y 3 cocineros
            ICafeteria cafeteria = new Cafeteria(demoraEnPlatoPorCocinero);
            IPedido pedido = null;

            Console.WriteLine($"Número de Platos: {cafeteria.NumeroDePlatos}");
            // ~> Número de Platos: 5
            Console.WriteLine($"Número de Cocineros: {cafeteria.NumeroDeCocineros}");
            // ~> Número de Cocineros: 3
            Console.WriteLine();

            InspeccionarCafeteria(cafeteria);



            static void InspeccionarCafeteria(ICafeteria cafeteria)
            {
                Console.WriteLine("=====================================================================");
                Console.WriteLine("Inspeccionando ...");
                Console.WriteLine("=====================================================================");

                Console.WriteLine(">> Pedidos en espera:");
                foreach(IPedido pedido in cafeteria.PedidosEnEspera()) {
                    Console.WriteLine(PedidoToString(pedido));
                }
                Console.WriteLine();

                Console.WriteLine(">> Pedidos por cocinero:");
                foreach(var info in Enumerate(cafeteria.PedidosPorCocinero())) {
                    int i = info.Item1;
                    IPedido pedido = info.Item2;
                    Console.WriteLine($"Cocinero: {i} --> {{ {PedidoToString(pedido)} }}");
                }    
            }
            Console.WriteLine();

            Console.WriteLine(">> Tiempo restante por cocinero:");
            foreach(var info in Enumerate(cafeteria.TiempoRestantePorCocinero())) {
                int i = info.Item1;
                int tiempo = info.Item2;
                Console.WriteLine($"Cocinero: {i} --> Tiempo: {tiempo}");
            }
            Console.WriteLine("=====================================================================");
            Console.WriteLine();
        }
        static IEnumerable<Tuple<int, T>> Enumerate<T>(IEnumerable<T> sequence)
        {
            return sequence.Select((x, i) => new Tuple<int, T>(i, x));
        }

        static string PedidoToString(IPedido pedido)
        {
            return pedido != null ? $"Cliente: { pedido.Cliente}, Plato: { pedido.Plato}, Tiempo en espera: {pedido.TiempoEnEspera}" : "Sin pedidos";
        }
    }

        
    
    public interface IPedido
    {
        string Cliente { get; }
        int Plato { get; }
        int TiempoEnEspera { get; }
    }

    public interface ICafeteria
    {
        int NumeroDePlatos { get; }
        int NumeroDeCocineros { get; }
        void LlegaCliente(string cliente, int plato);
        IPedido SalePedido();
        IEnumerable<IPedido> PedidosEnEspera();
        IEnumerable<IPedido> PedidosPorCocinero();
        IEnumerable<int> TiempoRestantePorCocinero();
    }
    public class Pedido : IPedido
    {
        public string Cliente { get; private set; }
        public int Plato { get; private set; }
        public int TiempoEnEspera { get; private set; }

        public Pedido(string cliente, int plato, int tiempoEnEspera)
        {
            Cliente = cliente;
            Plato = plato;
            TiempoEnEspera = tiempoEnEspera;
        }
    }
    public class Cafeteria : ICafeteria
    {
        List<IPedido> pedidosEspera = new List<IPedido>();

        int[,] demoraEnPlatoPorCocinero;
        public Cafeteria(int[,] demoraEnPlatoPorCocinero)
        {
            this.demoraEnPlatoPorCocinero = demoraEnPlatoPorCocinero;
        }

        public int NumeroDePlatos { get { return demoraEnPlatoPorCocinero.GetLength(0); } }
        public int NumeroDeCocineros { get { return demoraEnPlatoPorCocinero.GetLength(1); } }

        //public Queue<Dictionary<string, int>> dicPedidos = new Queue<Dictionary<string, int>>();
        public void LlegaCliente(string cliente, int plato)
        {
            IPedido pedido = new Pedido(cliente, plato, 0);
        }
        public IPedido SalePedido()
        {
            throw new NotImplementedException();
        }
        public IEnumerable<IPedido> PedidosEnEspera()
        {
            throw new NotImplementedException();
            
        }
        public IEnumerable<IPedido> PedidosPorCocinero()
        {
            throw new NotImplementedException();
        }
        public IEnumerable<int> TiempoRestantePorCocinero()
        {
            throw new NotImplementedException();
        }
    }
    
}
*/