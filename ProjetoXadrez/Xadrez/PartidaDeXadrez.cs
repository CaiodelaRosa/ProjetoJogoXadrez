using System;
using System.Collections.Generic;
using System.Text;
using Tabuleiro_;

namespace Xadrez
{
    class PartidaDeXadrez
    {
        public Tabuleiro Tab { get; private set; }
        public int Turno { get; private set; }
        public Cor JogadorAtual { get; private set; }
        public bool Terminada { get; private set; }
        private HashSet<Peca> Pecas;
        private HashSet<Peca> Capturadas;
        public bool Xeque { get; private set; }
        public Peca VulneravelEnPassant { get; private set; }

        public PartidaDeXadrez()
        {
            Tab = new Tabuleiro(8, 8);
            Turno = 1;
            JogadorAtual = Cor.Branca;
            Terminada = false;
            Xeque = false;
            VulneravelEnPassant = null;
            Pecas = new HashSet<Peca>();
            Capturadas = new HashSet<Peca>();
            ColocarPecas();
        }

        public Peca ExecutaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = Tab.RetirarPeca(origem);
            p.IncrementarQtdMovimentos();
            Peca pecaCapturada = Tab.RetirarPeca(destino);
            Tab.ColocarPeca(p, destino);
            if (pecaCapturada != null)
            {
                Capturadas.Add(pecaCapturada);
            }

            // Jogada especial roque pequeno
            if (p is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna + 1);
                Peca T = Tab.RetirarPeca(origemT);
                T.IncrementarQtdMovimentos();
                Tab.ColocarPeca(T, destinoT);
            }

            // Jogada especial roque grande
            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca T = Tab.RetirarPeca(origemT);
                T.IncrementarQtdMovimentos();
                Tab.ColocarPeca(T, destinoT);
            }

            // Jogada especial en passant
            if (p is Peao)
            {
                if (origem.Coluna != destino.Coluna && pecaCapturada == null)
                {
                    Posicao posP;
                    if (p.Cor == Cor.Branca)
                    {
                        posP = new Posicao(destino.Linha + 1, destino.Coluna);
                    }
                    else
                    {
                        posP = new Posicao(destino.Linha - 1, destino.Coluna);
                    }
                    pecaCapturada = Tab.RetirarPeca(posP);
                    Capturadas.Add(pecaCapturada);
                }
            }

            return pecaCapturada;
        }

        public void DesfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada)
        {
            Peca p = Tab.RetirarPeca(destino);
            p.DecrementarQtdMovimentos();
            if (pecaCapturada != null)
            {
                Tab.ColocarPeca(pecaCapturada, destino);
                Capturadas.Remove(pecaCapturada);
            }
            Tab.ColocarPeca(p, origem);

            // Jogada especial roque pequeno
            if (p is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna + 1);
                Peca T = Tab.RetirarPeca(destinoT);
                T.IncrementarQtdMovimentos();
                Tab.ColocarPeca(T, origemT);
            }

            // Jogada especial roque grande
            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca T = Tab.RetirarPeca(destinoT);
                T.IncrementarQtdMovimentos();
                Tab.ColocarPeca(T, origemT);
            }

            // Jogada especial en passant
            if (p is Peao)
            {
                if (origem.Coluna != destino.Coluna && pecaCapturada == VulneravelEnPassant)
                {
                    Peca peao = Tab.RetirarPeca(destino);
                    Posicao posP;
                    if (p.Cor == Cor.Branca)
                    {
                        posP = new Posicao(3, destino.Coluna);
                    }
                    else
                    {
                        posP = new Posicao(4, destino.Coluna);
                    }
                    Tab.ColocarPeca(p, posP);
                }
            }

            /*if (p is Peao)
            {
                if (destino.Linha == 7 || destino.Linha == 0)
                {
                    Peca peao = Tab.RetirarPeca(destino);
                    Tab.ColocarPeca(p, origem);
                }
            }*/
        }

        public void RealizaJogada(Posicao origem, Posicao destino)
        {
            Peca pecaCapturada = ExecutaMovimento(origem, destino);

            if (EmXeque(JogadorAtual))
            {
                DesfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException("Você não pode se colocar em xeque!");
            }

            //Jogada especial promocao
            Peca promo = Tab.Peca(destino);
            if (promo is Peao)
            {
                if ((promo.Cor == Cor.Branca && destino.Linha == 0) || (promo.Cor == Cor.Preta && destino.Linha == 7))
                {
                    Console.WriteLine("Promoção de peão!!!");
                    Console.Write("Selecione uma peça (D/T/C/B): ");
                    char pp = char.Parse(Console.ReadLine());
                    if (pp == 'D' || pp == 'd')
                    {
                        promo = Tab.RetirarPeca(destino);
                        Pecas.Remove(promo);
                        Peca dama = new Dama(Tab, promo.Cor);
                        Tab.ColocarPeca(dama, destino);
                    }
                    else if (pp == 'T' || pp == 't')
                    {
                        promo = Tab.RetirarPeca(destino);
                        Pecas.Remove(promo);
                        Peca torre = new Torre(Tab, promo.Cor);
                        Tab.ColocarPeca(torre, destino);
                    }
                    else if (pp == 'B' || pp == 'b')
                    {
                        promo = Tab.RetirarPeca(destino);
                        Pecas.Remove(promo);
                        Peca bispo = new Bispo(Tab, promo.Cor);
                        Tab.ColocarPeca(bispo, destino);
                    }
                    else if (pp == 'C' || pp == 'c')
                    {
                        promo = Tab.RetirarPeca(destino);
                        Pecas.Remove(promo);
                        Peca cavalo = new Cavalo(Tab, promo.Cor);
                        Tab.ColocarPeca(cavalo, destino);
                    }
                    else
                    {
                        DesfazMovimento(origem, destino, pecaCapturada);
                        throw new TabuleiroException("Opção inválida!");
                    }
                }
            }

            // Testa adversario em xeque
            if (EmXeque(Adversaria(JogadorAtual)))
            {
                Xeque = true;
            }
            else
            {
                Xeque = false;
            }

            if (TesteXequemate(Adversaria(JogadorAtual)))
            {
                Terminada = true;
            }
            else
            {
                Turno++;
                MudaJogador();
            }

            Peca p = Tab.Peca(destino);

            // Jogada especial en passant
            if (p is Peao && (destino.Linha == origem.Linha + 2 || destino.Linha == origem.Linha - 2))
            {
                VulneravelEnPassant = p;
            }
            else
            {
                VulneravelEnPassant = null;
            }
        }

        public void ValidarPosicaoDeOrigem(Posicao pos)
        {
            if (Tab.Peca(pos) == null)
            {
                throw new TabuleiroException("Não existe peça na posição de origem escolhida!");
            }
            if (JogadorAtual != Tab.Peca(pos).Cor)
            {
                throw new TabuleiroException("A peça de origem escolhida não é sua!");
            }
            if (!Tab.Peca(pos).ExisteMovimentosPossiveis())
            {
                throw new TabuleiroException("Não há movimentos possíveis para a peça de origem escolhida!");
            }
        }

        public void ValidarPosicaoDeDestino(Posicao origem, Posicao destino)
        {
            if (!Tab.Peca(origem).MovimentoPossivel(destino))
            {
                throw new TabuleiroException("Posição de destino inválida!");
            }
        }
        void MudaJogador()
        {
            if (JogadorAtual == Cor.Branca)
            {
                JogadorAtual = Cor.Preta;
            }
            else
            {
                JogadorAtual = Cor.Branca;
            }
        }

        public HashSet<Peca> PecasCapturadas(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in Capturadas)
            {
                if (x.Cor == cor)
                {
                    aux.Add(x);
                }
            }
            return aux;
        }

        public HashSet<Peca> PecasEmJogo(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in Pecas)
            {
                if (x.Cor == cor)
                {
                    aux.Add(x);
                }
            }
            aux.ExceptWith(PecasCapturadas(cor));
            return aux;
        }

        private Cor Adversaria(Cor cor)
        {
            if (cor == Cor.Branca)
            {
                return Cor.Preta;
            }
            else
            {
                return Cor.Branca;
            }
        }

        private Peca Rei(Cor cor)
        {
            foreach (Peca x in PecasEmJogo(cor))
            {
                if (x is Rei)
                {
                    return x;
                }
            }
            return null;
        }

        public bool EmXeque(Cor cor)
        {
            Peca R = Rei(cor);
            if (R == null)
            {
                throw new TabuleiroException($"Não tem rei da cor {cor} no tabuleiro!");
            }

            foreach (Peca x in PecasEmJogo(Adversaria(cor)))
            {
                bool[,] mat = x.MovimentosPossiveis();
                if (mat[R.Posicao.Linha, R.Posicao.Coluna])
                {
                    return true;
                }
            }
            return false;
        }

        public bool TesteXequemate(Cor cor)
        {
            if (!EmXeque(cor))
            {
                return false;
            }
            foreach (Peca x in PecasEmJogo(cor))
            {
                bool[,] mat = x.MovimentosPossiveis();
                for (int i = 0; i < Tab.Linhas; i++)
                {
                    for (int j = 0; j < Tab.Colunas; j++)
                    {
                        if (mat[i, j])
                        {
                            Posicao origem = x.Posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaCapturada = ExecutaMovimento(origem, destino);
                            bool testeXeque = EmXeque(cor);
                            DesfazMovimento(origem, destino, pecaCapturada);

                            if (!testeXeque)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public void ColocarNovaPeca(char coluna, int linha, Peca peca)
        {
            Tab.ColocarPeca(peca, new PosicaoXadrez(coluna, linha).ToPosicao());
            Pecas.Add(peca);
        }

        private void ColocarPecas()
        {
            ColocarNovaPeca('a', 1, new Torre(Tab, Cor.Branca));
            ColocarNovaPeca('b', 1, new Cavalo(Tab, Cor.Branca));
            ColocarNovaPeca('c', 1, new Bispo(Tab, Cor.Branca));
            ColocarNovaPeca('d', 1, new Dama(Tab, Cor.Branca));
            ColocarNovaPeca('e', 1, new Rei(Tab, Cor.Branca, this));
            ColocarNovaPeca('f', 1, new Bispo(Tab, Cor.Branca));
            ColocarNovaPeca('g', 1, new Cavalo(Tab, Cor.Branca));
            ColocarNovaPeca('h', 1, new Torre(Tab, Cor.Branca));
            ColocarNovaPeca('a', 2, new Peao(Tab, Cor.Branca, this));
            ColocarNovaPeca('b', 2, new Peao(Tab, Cor.Branca, this));
            ColocarNovaPeca('c', 2, new Peao(Tab, Cor.Branca, this));
            ColocarNovaPeca('d', 2, new Peao(Tab, Cor.Branca, this));
            ColocarNovaPeca('e', 2, new Peao(Tab, Cor.Branca, this));
            ColocarNovaPeca('f', 2, new Peao(Tab, Cor.Branca, this));
            ColocarNovaPeca('g', 2, new Peao(Tab, Cor.Branca, this));
            ColocarNovaPeca('h', 2, new Peao(Tab, Cor.Branca, this));

            ColocarNovaPeca('a', 8, new Torre(Tab, Cor.Preta));
            ColocarNovaPeca('b', 8, new Cavalo(Tab, Cor.Preta));
            ColocarNovaPeca('c', 8, new Bispo(Tab, Cor.Preta));
            ColocarNovaPeca('e', 8, new Rei(Tab, Cor.Preta, this));
            ColocarNovaPeca('d', 8, new Dama(Tab, Cor.Preta));
            ColocarNovaPeca('f', 8, new Bispo(Tab, Cor.Preta));
            ColocarNovaPeca('g', 8, new Cavalo(Tab, Cor.Preta));
            ColocarNovaPeca('h', 8, new Torre(Tab, Cor.Preta));
            ColocarNovaPeca('a', 7, new Peao(Tab, Cor.Preta, this));
            ColocarNovaPeca('b', 7, new Peao(Tab, Cor.Preta, this));
            ColocarNovaPeca('c', 7, new Peao(Tab, Cor.Preta, this));
            ColocarNovaPeca('d', 7, new Peao(Tab, Cor.Preta, this));
            ColocarNovaPeca('e', 7, new Peao(Tab, Cor.Preta, this));
            ColocarNovaPeca('f', 7, new Peao(Tab, Cor.Preta, this));
            ColocarNovaPeca('g', 7, new Peao(Tab, Cor.Preta, this));
            ColocarNovaPeca('h', 7, new Peao(Tab, Cor.Preta, this));
        }
    }
}