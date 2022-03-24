using System;
using Tabuleiro_;
using Xadrez;
using System.Collections.Generic;

namespace ProjetoXadrez
{
    class Tela
    {  
        public static void ImprimirPartida(PartidaDeXadrez partida)
        {
            ImprimirTabuleiro(partida.Tab);
            Console.WriteLine();
            ImprimirPecasCapturadas(partida);
            Console.WriteLine();
            Console.WriteLine($"Turno: {partida.Turno}");
            Console.WriteLine($"Aguardando jogada do jogador: {partida.JogadorAtual}");
           if (!partida.Terminada)
            {
                if (partida.Xeque)
                {
                    Console.WriteLine();
                    Console.Write("XEQUE!");
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("XEQUEMATE!!");
                Console.WriteLine();
                Console.WriteLine($"Vencedor: {partida.JogadorAtual}");
            }
        }

        public static void ImprimirPecasCapturadas(PartidaDeXadrez partida)
        {
            Console.WriteLine("Pecas capturadas: ");
            Console.Write("Brancas: ");
            ImprimirConjunto(partida.PecasCapturadas(Cor.Branca));
            Console.WriteLine();
            Console.Write("Pretas: ");
            ImprimirConjunto(partida.PecasCapturadas(Cor.Preta));
            Console.WriteLine();
        }

        public static void ImprimirConjunto(HashSet<Peca> conjunto)
        {

            int i = 0;
            Console.Write("[");
            foreach (Peca x in conjunto)
            {
                if (i == 0)
                {
                    Console.Write($"{x}");
                }
                else
                {
                    Console.Write($", {x}");
                }
                i++;
            }
            Console.Write("]");
                
        }

        public static void ImprimirTabuleiro(Tabuleiro tab)
        {
            for (int i = 0; i < tab.Linhas; i++)
            {
                ConsoleColor aux1 = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(8 - i + " ");
                Console.ForegroundColor = aux1;
                for (int j = 0; j < tab.Colunas; j++)
                {

                    ImprimirPeca(tab.Peca(i, j));


                }
                Console.WriteLine();
            }
            ConsoleColor aux = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("  A B C D E F G H");
            Console.ForegroundColor = aux;
        }
        public static void ImprimirTabuleiro(Tabuleiro tab, bool[,] movimentosPossiveis)
        {
            ConsoleColor fundoOriginal = Console.BackgroundColor;
            ConsoleColor fundoAlterado = ConsoleColor.DarkYellow;

            for (int i = 0; i < tab.Linhas; i++)
            {
                ConsoleColor aux1 = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(8 - i + " ");
                Console.ForegroundColor = aux1;
                for (int j = 0; j < tab.Colunas; j++)
                {
                    if (movimentosPossiveis[i, j])
                    {
                        Console.BackgroundColor = fundoAlterado ;
                    }
                    else
                    {
                        Console.BackgroundColor= fundoOriginal ;
                    }
                    ImprimirPeca(tab.Peca(i, j));
                    Console.BackgroundColor = fundoOriginal;

                }
                Console.WriteLine();
            }
            ConsoleColor aux = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("  A B C D E F G H");
            Console.ForegroundColor = aux;
            Console.BackgroundColor = fundoOriginal;
        }

        public static PosicaoXadrez LerPosicaoXadrez()
        {
            string s = Console.ReadLine();
            char coluna = s[0];
            int linha = int.Parse(s[1] + "");
            return new PosicaoXadrez(coluna, linha);
        }

        public static void ImprimirPeca(Peca peca)
        {
            if (peca == null)
            {
                Console.Write($"- ");
            }
            else
            {

                if (peca.Cor == Cor.Branca)
                {
                    Console.Write(peca);
                }
                else
                {
                    ConsoleColor aux = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.Write(peca);
                    Console.ForegroundColor = aux;
                }
                Console.Write(" ");
            }
        }
    }

}