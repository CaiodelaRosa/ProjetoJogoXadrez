using System;
using Tabuleiro_;
using Xadrez;

namespace ProjetoXadrez
{
    class Program
    {
        static void Main(string[] args)
        {
            PosicaoXadrez pos = new PosicaoXadrez('e', 1);

            Console.WriteLine(pos.ToPosicao());
            Console.WriteLine(pos);
        }
    }
}
