using Additional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ustupka
{
    class Program
    {
        
        static void Main(string[] args)
        {
            int A = 2;//кол-во целевых функций
            int N = 3;//кол-во ограничений
            int M = 2;//кол-во переменных
            //todo: автоизменение знака
            double[,] targetFunctions = { { 0, -1, -3},
                                          { 0, -40,-10} };
            // Максимизация или минимизация
            bool[] directions = { true, true };
            
            // Ограничения
            double[,] restrictions = new double[,]{ {90, 2, 1},
                                                      {60, 1, 1},
                                                      {50,  0, 1}};
            // Знаки ограничений
            // <= true
            // >= false
            bool[] signs = { true, true, false };
            
            // Дополнительные ограничения
            double[,] additionalRestrictions = new double[N-1,M+1];
            // Уступки
            double[] assigments = { 16 };
            // итоговые значения функций
            double[] criteriaValues = new double[A];
            double[] result = new double[M];
            double[,] table_result;
            //Ввод данных
            if (true)
            {
                Console.Write("Введите количество целевых функций: ");
                A = Int32.Parse(Console.ReadLine());
                Console.Write("Введите количество Ограничений: ");
                N = Int32.Parse(Console.ReadLine());
                Console.Write("Введите количество переменных: ");
                M = Int32.Parse(Console.ReadLine());

                // Вводим коофициенты целевых функций
                for (int i = 0; i < A; i++)
                {
                    Console.WriteLine("F[" + i + "]= ");
                    targetFunctions[i, 0] = 0;
                    for (int j = 1; j < M+1; j++)
                    {
                        targetFunctions[i, j] = -Int32.Parse(Console.ReadLine());
                    }
                    Console.WriteLine("Max? ");
                    directions[i] = Int32.Parse(Console.ReadLine()) ==1?true:false;
                }
                // Вводим коофициенты ограничений
                for (int i = 0; i < N; i++)
                {
                    Console.WriteLine("Ограничение "+i+" ");
                    Console.WriteLine("Знак ограничения " + i + " ");
                    signs[i] = Int32.Parse(Console.ReadLine()) == 1 ? true : false;
                    Console.WriteLine("Коэфициенты ограничения " + i + " ");
                    for (int j = 0; j < M+1; j++)
                    {
                        if(signs[i])
                            restrictions[i, j] = Int32.Parse(Console.ReadLine());
                        else
                            restrictions[i, j] = -Int32.Parse(Console.ReadLine());

                    }
                }
                // Вводим уступки
                for (int j = 0; j < A-1; j++)
                {
                    Console.WriteLine("Уступка " + j + " ");
                    assigments[j] = Int32.Parse(Console.ReadLine());
                }
            }
            for (int i = 0; i < A; i++)
            { 
                // Заполняем таблицу для прохождения симплекс метода
                double[,] table = new double[N+i+1,M+1];
                Array.Copy(restrictions, table, N*(M+1));
                Array.Copy(additionalRestrictions, 0, table, N * (M + 1), i*M+1);
                Array.Copy(targetFunctions, i*(M+1), table, N * (M + 1)+i*(M+1), M+1);
                // Находим оптимум
                Simplex S = new Simplex(table,directions[i]);
                table_result = S.Calculate(result);
                double crt = -targetFunctions[i, 0];
                // Новое ограничение
                for (int k = 0; k < M; k++)
                {
                    crt -= targetFunctions[i, k+1]*result[k];
                }
                if (i != criteriaValues.Length - 1)
                {
                    crt -= assigments[i];
                    criteriaValues[i] = crt;
                    double[,] tmp = new double[1,4];
                    Array.Copy(targetFunctions, tmp, 4);
                    tmp[0, 0] = -crt;
                    Array.Copy(tmp, 0, additionalRestrictions, i*4, 4);
                }
                else
                {
                    criteriaValues[i] = crt;
                }
            
            }
           

            Console.WriteLine("Решение:");
            for(int i = 0; i < criteriaValues.Length; i++)
            {
                Console.WriteLine("f"+i+" = " + criteriaValues[i]);
            }
            Console.ReadLine();
        }
    }
}
