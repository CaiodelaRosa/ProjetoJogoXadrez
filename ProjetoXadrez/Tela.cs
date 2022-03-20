using System;
using Tabuleiro_;

namespace ProjetoXadrez
{
    internal class Tela
    {
        public static void ImprimirTabuleiro(Tabuleiro Tab)
        {
            for (int i = 0; i < Tab.Linhas; i++)
            {
                for (int j = 0; j < Tab.Colunas; j++)
                {
                    if (Tab.Peca(i, j) == null)
                    {
                        Console.Write($"- ");
                    }
                    else
                    {
                        Console.WriteLine($"{Tab.Peca(i, j)} ");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
