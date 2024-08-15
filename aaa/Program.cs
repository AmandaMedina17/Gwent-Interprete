/*public class Program
{
    public static void Main(string[] args)
    {
        Expresion expresion = new Expresion.ExpresionBinaria(new Expresion.ExpresionUnaria(new Token(TokenType.Menos, "-", null, 1),new Expresion.ExpresionLiteral(123)),
            new Expresion.ExpresionAgrupacion( new Expresion.ExpresionLiteral(45.67)),
            new Token(TokenType.Asterizco, "*", null, 1));

        Console.WriteLine(new AstPrinter().Print(expresion));
    }
}

/*using System;  // <>

namespace Ejercicios{
    class Program
    {
        static void Main(string[] args)
        {
            Cuenta c1 = new Cuenta("Amanda", 100);

            Console.WriteLine(c1.Saldo);

    
        }
        
    }

    public class Cuenta
    {
        public string Titular{ get; private set; }
        public int Saldo{ get; private set; }

        public Cuenta(string titular, int saldo)
        {
            Titular = titular;
            Saldo = saldo;
        }

        public void Deposita(int cantidad)
        {
            if(cantidad <= 0)
                throw new Exception("Cantidad a depositar debe ser mayor que 0");
            Saldo += cantidad;
        }

        public void Extrae(int cantidad)
        {
            if(Saldo < cantidad)
                throw new Exception("No hay suficiente saldo");
            else if(cantidad == 0);
                throw new Exception("Cantidad a extraer debe ser mayor que 0");
            Saldo -= cantidad;
        }
    }


}*/


//----------------------------------------------------------------------------------------------


/*namespace EjercicioVehiculos
{
    public class Program
    {
        static void Main(string[] args)
        {
            Avion CubanaDeAviacion = new Avion();
            Coche Ferrari = new Coche();

            CubanaDeAviacion.arrancarMotor();
            Ferrari.pararMotor();

            CubanaDeAviacion.Conducir();
            Ferrari.Conducir();

            Vehiculo miVehiculo = new Coche();
            miVehiculo.Conducir();

        }
    }

    public class Vehiculo
    {
        public void arrancarMotor()
        {
            System.Console.WriteLine("brrrrmmmmm");
        }

        public void pararMotor()
        {
            System.Console.WriteLine("...");
        }
    
        public virtual void Conducir()
        {
            System.Console.WriteLine("Se conduce");
        }
    }

    public class Avion:Vehiculo
    {
        public override void Conducir()
        {
            System.Console.WriteLine("Se conduce por un piloto");
        }
    }

    public class Coche:Vehiculo
    {
        public override void Conducir()
        {
            System.Console.WriteLine("Se counduce por un chofer");
        }
    }
}*/


//-------------------------------------------------------------------------------------------------------------------

/*namespace Figuras
{
    public class Program
    {
        static void Main(string[] args)
        {
            /*Circulo ci1 = new Circulo(5);
            System.Console.WriteLine(ci1.areaFigura());
            System.Console.WriteLine(ci1.perimetroFigura());

            Rectangulo r1 = new Rectangulo(2, 3);
            System.Console.WriteLine(r1.areaFigura() + " cm cuadrados");
            System.Console.WriteLine(r1.perimetroFigura() + " cm"); 

            Triangulo t1 = new Triangulo(3, 2, 3, 3);
            System.Console.WriteLine(t1.areaFigura() + " cm cuadrados");
            System.Console.WriteLine(t1.perimetroFigura() + " cm"); 

            Circulo ci2 = new Circulo(4);
            Circulo ci3 = ci1;

            System.Console.WriteLine(ci1.Equals(ci2));
            System.Console.WriteLine(ci1.Equals(ci3));

            System.Console.WriteLine(ci3);

            string circunferencia = Figura.Circulos.ToString();
            System.Console.WriteLine(circunferencia);

            Figura tresLados = Figura.Triangulos;
            System.Console.WriteLine((int)tresLados);

            AlmacenarFiguras<Circulo> Circulitos = new AlmacenarFiguras<Circulo>(3);

            Circulitos.Anadir(new Circulo(2));
            Circulitos.Anadir(new Circulo(3));
            Circulitos.Anadir(new Circulo(4));

            System.Console.WriteLine(Circulitos.getAlmacen(2).radio);*/

/*List<int> numeros = new List<int>();
numeros.Add(1);
numeros.Add(2);
numeros.Add(3);

for (int i = 0; i < numeros.Count; i++)
{
    System.Console.Write("|" + numeros[i] + "|  ");
}*/

/*LinkedList<int> numbers = new LinkedList<int>();

numbers.AddLast(1);
numbers.AddLast(2);
numbers.AddLast(3);
numbers.AddLast(4);
numbers.AddLast(5);

foreach (int numerito in numbers)
{
    System.Console.WriteLine(numerito);
}

List<int> numeros = new List<int> {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};

List<int> numerosPares = numeros.FindAll(numero => numero % 2==0);

numerosPares.ForEach(numero => System.Console.Write(" |" + numero + "| "));


}

}

enum Figura { Triangulos, Circulos, Rectangulos, Cuadrado};

abstract class Figuras
{
public abstract double areaFigura();

public abstract double perimetroFigura();
}

class Circulo:Figuras
{
public double radio;

public Circulo(double radio)
{
this.radio = radio;
}

public override double areaFigura()
{
return Math.PI * Math.Pow(radio, 2);
}

public override double perimetroFigura()
{
return 2 * Math.PI * radio;
}

public override string ToString()
{
return "El circulo tiene un radio de " + 5 + " unidades.";
}
}

class Rectangulo:Figuras
{
public double ancho;
public double largo;

public Rectangulo(double ancho, double largo)
{
this.ancho = ancho;
this.largo = largo;
}

public override double areaFigura()
{
return largo * ancho;
}

public override double perimetroFigura()
{
return 2*largo + 2*ancho;
}
}

class Triangulo:Figuras
{
public double base1;
public double altura;
public double cateto1;
public double cateto2; 


public Triangulo(double base1, double altura, double cateto1, double cateto2)
{
this.base1 = base1;
this.altura = altura;
this.cateto1 = cateto1;
this.cateto2 = cateto2;

}

public override double areaFigura()
{
return (base1*altura)/2;
}

public override double perimetroFigura()
{
return base1+cateto1+cateto2;
}
}

class AlmacenarFiguras<T>
{
private int i =0;
private int n;
public T[] almacen;
public AlmacenarFiguras(int n)
{
this.n = n;
almacen = new T[n];
}

public void Anadir(T obj)
{
almacen[i] = obj;
i++;
}

public T getAlmacen(int j)
{
return almacen[j];
}
}
}*/


//--------------------------------------------------------------------------------------------------------------------------------


/*namespace interfaces
{
    public class Program
    {
        static void Main(string[] args)
        {
            Cuenta AnoNuevo = new Cuenta(1000, "Amanda");
            Cuenta SanValentin = new Cuenta(500, "Gilberto");
            Cuenta Aniversario = new Cuenta(2000, "Ana");
            Cuenta Cumpleanos = new Cuenta(400, "Agatha");

            Cuenta[] diasImportantes = new Cuenta[] {AnoNuevo, SanValentin, Aniversario, Cumpleanos};

            Ordenar(diasImportantes, new ComparadorTitularCuenta());

            for (int i = 0; i < diasImportantes.Length; i++)
            {
                System.Console.WriteLine(diasImportantes[i].Imprimir());

            }

            
        }

        public static void Ordenar<T>(T[] items, IComparer<T> comp)
        {
            for (int i = 0; i < items.Length -1; i++)
            {
                for (int j = i+1; j < items.Length; j++)
                {
                    if(comp.Compare(items[i], items[j])>0)
                    {
                        T temp = items[i];
                        items[i] = items[j];
                        items[j] = temp;
                    }
                }
            }

        }

        public static T[] CloneArray<T>(T[] items) where T: IClon<T>
        {
            T[] result = new T[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                if(items[i] != null)
                {
                    result[i] = items[i].Clon();
                }
                
            }
            return result;

        }

        
    }

    public interface IComparable<T>
    {
       int CompareTo(T obj); 
    }

    public interface IClon<T>
    {
        T Clon();
    }

    public interface IComparer<T>
    {
        int Compare(T x, T y);
    }

    class Cuenta
    {
        public int Saldo;
        public string Titular{ get; set; }

        public Cuenta(int Saldo, string Titular)
        {
            this.Saldo = Saldo;
            this.Titular = Titular;
        }

        public string Imprimir()
        {
            return Titular;
        }
    }

    class ComparadorTitularCuenta : IComparer<Cuenta>
    {
        public int Compare(Cuenta c1, Cuenta c2)
        {
            if(c1==null || c2==null)
                throw new Exception("no");
            return c1.Titular.CompareTo(c2.Titular);
        }
    }



    class Fecha : IComparable<Fecha>, IClon<Fecha>
    {
        public int Dia{ get; set;}
        public int Mes{ get; set;}
        public int Ano{ get; set;}

        public Fecha(int Dia, int Mes, int Ano)
        {
            this.Dia = Dia;
            this.Mes = Mes;
            this.Ano = Ano;
        }

        public int CompareTo(Fecha f)
        {
            if(Ano < f.Ano) return -1;
            else if(Ano > f.Ano) return 1;
            if(Mes < f.Mes) return -1;
            else if(Mes > f.Mes) return 1;
            if(Dia < f.Dia) return -1;
            else if(Dia > f.Dia) return 1;
            else return 0;
        }

        public Fecha Clon()
        {
            return new Fecha(Dia, Mes, Ano);
        }

        public string Impreso()
        {
            return Dia + "|" + Mes + "|" + Ano;
        }
    }
}


using System.Collections;

namespace interfaces
{
    public class Program
    {
        static void Main(string[] args)
        {
            Intervalo intervalo = new Intervalo(1, 10);
            foreach(int k in intervalo.Pares) System.Console.WriteLine(k);
        }
        
    }

   public class Intervalo : IEnumerable<int>
   {
        public int Inf{ get; set;}
        public int Sup{ get; set;}

        public Intervalo(int inf, int sup)
        {
            Inf = inf;
            Sup = sup;
        }
        public IEnumerator<int> GetEnumerator()
        {
            for (int i = Inf; i <= Sup; i++) yield return i;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerable<int> Pares
        {
            get
            {
                int par;
                if (Inf%2==0) par=Inf;
                else par = Inf +1;
                for (int i = par; i <= Sup; i+=2)
                    yield return i;
            }
            
        }

        public IEnumerable<int> Impares
        {
            get
            {
                int impar;
                if (Inf%2==0) impar=Inf+1;
                else impar = Inf;
                for (int i = impar; i <= Sup; i+=2)
                {
                    yield return i;
                } 
            }
            
        }
   }

}*/

