using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;

namespace Matrix_pp
{
    class Program
    {
        static void Main(string[] args)
        {
            //var matrixA = GetMatrix("A");
           // var matrixB = GetMatrix("B");
            var matrixA = GetMatrixRandom("A");
            var matrixB = GetMatrixRandom("B");
            if (matrixA.GetLength(1) != matrixB.GetLength(0))
            {
                throw new Exception(
                    "Умножение не возможно! Количество столбцов первой матрицы не равно количеству строк второй матрицы.");
            }

           // Console.WriteLine("Матрица А * матрица В = ");
            var time1 = System.Diagnostics.Stopwatch.StartNew();
            var matrixC1 = Multiplication(matrixA, matrixB);
            time1.Stop();
            var matrixC2 = MultiplicationParal(matrixA, matrixB);
           // PrintMatrix(matrixC1);
            Console.WriteLine();
           // PrintMatrix(matrixC2);
        }


        static int[,] GetMatrix(string name)
        {
            Console.WriteLine($"Введите количество строк матрицы {name}: ");
            int n = int.Parse(Console.ReadLine());
            Console.WriteLine($"Введите количество столбцов матрицы {name}: ");
            int m = int.Parse(Console.ReadLine());
            Console.WriteLine($"Введите элементы матрицы {name}: ");
            int[,] matrix = new int[n, m];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                int[] raw = Console.ReadLine().Split().Select(p => int.Parse(p)).ToArray();
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrix[i, j] = raw[j];
                }
            }

            return matrix;
        }
        static int[,] GetMatrixRandom(string name)
        {
            Console.WriteLine($"Введите количество строк матрицы {name}: ");
            int n = int.Parse(Console.ReadLine());
            Console.WriteLine($"Введите количество столбцов матрицы {name}: ");
            int m = int.Parse(Console.ReadLine());
            Random r = new Random();
            int[,] matrix = new int[n, m];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrix[i, j] = r.Next(1,10);
                }
            }

            return matrix;
        }


        static void PrintMatrix(int[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(matrix[i, j] + " ");
                }

                Console.WriteLine();
            }
        }
        

        static int[,] Multiplication(int[,] matrixA, int[,] matrixB)
        {

            var matrixC = new int[matrixA.GetLength(0), matrixB.GetLength(1)];

            var time = System.Diagnostics.Stopwatch.StartNew();
            for (var i = 0; i < matrixA.GetLength(0); i++)
            {
                for (var j = 0; j < matrixB.GetLength(1); j++)
                {
                    matrixC[i, j] = 0;

                    for (var k = 0; k < matrixA.GetLength(1); k++)
                    {
                        matrixC[i, j] += matrixA[i, k] * matrixB[k, j];
                    }
                }
            }
            time.Stop();
            var elapsedMs = time.ElapsedMilliseconds;

            Console.WriteLine("Время вычисления в обычном режиме: " + elapsedMs);

            return matrixC;
        }

        static int[,] MultiplicationParal(int[,] matrixA, int[,] matrixB)
        {
            int n = matrixA.GetLength(0);
            int m = matrixA.GetLength(1);
            int k = matrixB.GetLength(1);
            int[,] matrix = new int[n, k];
            var numberOfThreads = Environment.ProcessorCount;
            var threads = new Thread[numberOfThreads];
            int partSize = n / numberOfThreads + 1;
            var time = System.Diagnostics.Stopwatch.StartNew();
            for (int i = 0; i < numberOfThreads; ++i)
            {
                var localI = i;
                threads[i] = new Thread(() =>
                {
                    for (int e = partSize * localI; e < partSize * (localI + 1) && e < n; ++e)
                    {
                        for (int j = 0; j < k; j++)
                        {
                            for (int r = 0; r < m; r++)
                            {
                                matrix[e, j] += matrixA[e, r] * matrixB[r, j];
                            }
                        }
                    }
                });
            }
            foreach (var thread in threads)
            {
                thread.Start();
            }
            foreach (var thread in threads)
            {
                thread.Join();
            }
            time.Stop();
            var elapsedMs = time.ElapsedMilliseconds;
            Console.WriteLine("Время вычисления в многопоточном режиме: " + elapsedMs);

            return matrix;

        }
    }
}
        
