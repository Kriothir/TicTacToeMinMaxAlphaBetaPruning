using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIVaja2
{
    using Player = GameTable.Players;

    class MinMaxAI
    {
        // Globalne spremenljivke
        private int depthLimit;
        Player Tic = Player.x;
        Player Toe = Player.o;
        Player secondPlayer;
        int startingIndex = -1;

        public MinMaxAI(int depth)
        {
            depthLimit = depth; // Nastavi globino
        }
        // https://en.wikipedia.org/wiki/Alpha%E2%80%93beta_pruning
        //https://stackabuse.com/minimax-and-alpha-beta-pruning-in-python/
        //https://iq.opengenus.org/alpha-beta-pruning-minimax-algorithm/
        public int MinMax(GameTable board, Player firstPlay, int depth, int alfa, int beta)
        {
            Player currentPlayer = board.getCurrentPlayer(); // Pridobi trenutnega igralca, nasprotnega pa nastavi
            if (firstPlay == Tic){
                secondPlayer = Toe;

            }
            
            secondPlayer = Tic;

            if (!(board.Done() || depth == depthLimit))
            {
                #region Max
                if (currentPlayer == firstPlay)  //Igralec X oziroma max na potezi
                {
                    int indexState = startingIndex;  // Začetni index je vedno -1, dokler ne najdemo primernega premika

                    List<int> states = board.freeCoordinate(); // Pridobi prosta mesta na gameboardu
                    int size = states.Count;

                    for (int i = 0; i < size; i++)
                    {
                        int newMove = states[i];
                        GameTable newBoard = board.CurrentBoard(); // Pridobi, trenutno stanje gameboarda in naredi novo potezo
                        newBoard.MakeMove(newMove);
                        int currVal = MinMax(newBoard, firstPlay, depth + 1, alfa, beta); // Rekurzivni klic
                        if (alfa >= beta) // Alfa beta izlocanje
                        {
                            break;
                        }
                        if (currVal > alfa) // Če je vrednost lista manjsa kot alfa, nastavi alfa na novo vrednost in 
                                            // kazalec(indexstate) postavimo na vrednost pozicije premika iz states liste
                        {
                            alfa = currVal;
                            indexState = newMove;
                        }

                    }
                    if (indexState != startingIndex)  // Če kazalec indexstate ni enako -1, pomeni da smo našli premik in naredimo premik
                    {
                        board.MakeMove(indexState);
                    }
                    return alfa; // Vrnemo alfa
                }
                #endregion
                #region Min
                else
                {
                    int indexState = startingIndex; // Na potezi min, oziroma O
                    List<int> states = board.freeCoordinate(); // Lista prostih mest
                    int size = states.Count; // Velikost liste

                    for (int i = 0; i < size; i++)
                    {
                        int newMove = states[i]; // Premik skozi prazna mesta
                        GameTable newBoard = board.CurrentBoard(); // Pridobimo trenutno stanje
                        newBoard.MakeMove(newMove); // Naredimo potezo

                        int currVal = MinMax(newBoard, firstPlay, depth + 1, alfa, beta);
                        // Rekurzivni klic minimax
                        if (beta <= alfa) // Če je alfa večji, prekini, (alfa beta izločanje)
                        {
                            break;
                        }
                        if (currVal < beta) // Isto kot pri max
                        {
                            beta = currVal;
                            indexState = newMove;
                        }

                    }
                    if (indexState != startingIndex)
                    {
                        board.MakeMove(indexState); // Naredimo premik, če je bil najden premik
                    }
                    return beta;
                }
                #endregion
            }
            else
            {
                return getValue(board, firstPlay); // Če je igre konec vrni rezultat (+100 ali -100)
            }
           
        }
        public int getValue(GameTable board, Player currentPlayer)
        {
            if(currentPlayer == Tic)
            {
                secondPlayer = Toe;
            }
            else
            {
                secondPlayer = Tic;
            }
            if (board.WinCheck() == currentPlayer && board.Done() )
            {
                return 100;
            }
            else if (board.WinCheck() == secondPlayer && board.Done())
            {
                return -100;
            }
            else
            {
                return board.HeuristicCalculatorForPlayer(currentPlayer);
            }
        }
    }
}
