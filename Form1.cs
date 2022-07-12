using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace UIVaja2
{
    using Player = GameTable.Players;

    public partial class Form1 : Form
    {

        GameTable tictactoe;
        int depth; 
        // Globalne spremenljivke
        bool turn = true;
        int plusInf = int.MaxValue; // Infinity+ za Max
        int minusInf = int.MinValue; // Infinity- za Min
        public List<Button> buttonList;
        Button selectedButton;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            tictactoe = new GameTable();
            buttonList = new List<Button>();
            #region Buttons
            buttonList.Add(button1); 
            buttonList.Add(button2);
            buttonList.Add(button3);
            buttonList.Add(button4);
            buttonList.Add(button5);
            buttonList.Add(button6);
            buttonList.Add(button7);
            buttonList.Add(button8);
            buttonList.Add(button9);
            #endregion
            depthBox.Text = "1";
            depth = Convert.ToInt32(depthBox.Text);

        }
        private void StartRound(int column, int row)
        {
            // Pred pričetkom preveri ali je igre konec
            if (tictactoe.Done())
            {
                checkTheWinner();
            }
            // Če ni, naredi potezo, kliči algoritem minimax na trenutni igralni mizi
            else
            {
                tictactoe.MakeMove(column * 3 + row); // Nastavi premik igralca iz izbranega buttona
                MinMaxAI minimax = new MinMaxAI(depth);
                minimax.MinMax(tictactoe, Player.o, 0, minusInf, plusInf); // Klic minimax
                moveOnBoard(tictactoe); // Premik na igralni mizi
                if (tictactoe.Done()) // Še enkrat preverimo ali je kdo zmagal, oziroma ali je 
                    // presegel dovoljeno število devetih premikov. Potrebno je v vsaki vejitvi preveriti drugače je treba dvakrat pritisniti po dejanskem zakljucku igre
                {
                    checkTheWinner();
                }
            }
        }
        private void UserMove(object sender, MouseEventArgs e)
        {
            selectedButton = (Button)sender;
            // Pridobi številko gumba in nastavi pozicije za klic funkcije StartRound()
            int column = (Int32.Parse(selectedButton.Name.Substring(selectedButton.Name.Length - 1)) - 1) / 3;
            int row = (Int32.Parse(selectedButton.Name.Substring(selectedButton.Name.Length - 1)) - 1) % 3;
            // Nastavimo ob vsakem kliku uporabnika na X
            // disableamo button, da uporabnik ne more dvakrat pritisniti na isto mesto in se izognemo 
            // neželenim efektom
            selectedButton.Text = "X";
            selectedButton.Enabled = false;
            selectedButton.BackColor = Color.Yellow;
            StartRound(column, row);
        }

      
        private void ResetButton(object sender, EventArgs e)
        {
            depth = Convert.ToInt32(depthBox.Text);
            ReinitializeBoard();
        }
        private void moveOnBoard(GameTable board)
        {
            // Pridobi poteze premika računalniškega igralca in jih postavi na gumbe
            List<int> aiMoveList = board.AIMoves();
            for (int i = 0; i < aiMoveList.Count; i++)
            {
                int position = aiMoveList[i];
                buttonList[position].Text = "O";
                buttonList[position].BackColor = Color.Orange;
                buttonList[position].Enabled = false;
            }
        }
        private void checkTheWinner()
        {
            // Preveri če je zmagovalec oziroma ali je že bilo 9 premikov in disable-a buttone
            if (tictactoe.WinCheck() == Player.x)
            {
                turn = false;
                if (!turn)
                {
                    foreach (Button btn in buttonList)
                    {
                        btn.Enabled = false;
                    }
                    MessageBox.Show("You win!");

                }

            }
            else if (tictactoe.WinCheck() == Player.o)
            {
                foreach (Button btn in buttonList)
                {
                    btn.Enabled = false;
                }
                MessageBox.Show("You lost!");                

            }
            else if (tictactoe.numOfMoves() == 9 && tictactoe.WinCheck() == Player.none)
            {
                turn = false;
                if (!turn)
                {
                    foreach (Button btn in buttonList)
                    {
                        btn.Enabled = false;
                    }
                    MessageBox.Show("Draw!");
                }
            }
        }

        private void ReinitializeBoard()
        {
            // Enabla buttone in resetira igralno mizo
            foreach (Button btn in buttonList)
            {
                btn.Enabled = true;
                btn.Enabled = true;
                btn.BackColor = Color.White;
                btn.Text = "";
            }
            tictactoe.ReinitializeGame();

        }
        private void button1_Click(object sender, EventArgs e)
        {

        }

    }
}
