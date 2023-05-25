using System;

public class PoissonGenerator
{
    private Random _random;

    public PoissonGenerator(int seed)
    {
        _random = new Random(seed);
    }

    public int Generate(double lambda)
    {
        double L = Math.Exp(-lambda);
        double p = 1.0;
        int k = 0;

        do
        {
            k++;
            double u = _random.NextDouble();
            p *= u;
        } while (p > L);

        return k - 1;
    }
}

public class Request  
{
    public double app_time;
    public double sen_time;
    public Request(double a)
    {
        this.app_time = a;
    }
}


public class Program
{
  
    public static void Main()
    {
        var rnd = new Random();
        var generator = new PoissonGenerator(123);
        Console.WriteLine("Введите кол-во окон для синхронной системы:");
        int N = int.Parse(Console.ReadLine());
        Console.WriteLine("lambda\t\tavg_request\t\tavg_time");
        for( double lambda=0.001; lambda<1; lambda += 0.001)
        {
            double time_sum = 0;
            int numRequest = 0;
            double serv_req = 0;

            List<Request> queue = new List<Request>();
            
            int[] winArr = new int[N];
            for (int i = 0; i < N; i++)
            {
                winArr[i] = generator.Generate(Math.Round(lambda, 3));
            }
            for (int i = 0; i < N; i++)
            {
                for (int k = 0; k < winArr[i]; k++)
                {
                    queue.Add(new Request(rnd.NextDouble() + i));
                }
                numRequest += queue.Count;
                if (queue.Count != 0 && i != 0)
                {
                    time_sum += (i + 2) - queue[0].app_time;
                    queue.RemoveAt(0);
                    serv_req++;
                }
            }

            double avg_req = Math.Round( numRequest / (double)N, 3);
            double avg_time = Math.Round( time_sum / serv_req, 3);
            double TheoreticalReg = (double)(lambda * (2 - lambda)) / (2 * (1 - lambda));
            double TheoreticalAvgTime = (double)(( TheoreticalReg / lambda));
        
         
            Console.WriteLine($"{Math.Round(lambda,3)}\t\t{avg_req}\t\t\t{avg_time}\t\t{TheoreticalReg}\t\t{TheoreticalAvgTime}");
        }
        Console.ReadKey();
    }
}