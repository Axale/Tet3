using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tet3
{
    public partial class Form1 : Form
    {
        bool MenuMade;
        int GameState;
        Timer falltimer;
        PictureBox Board;
        Tetramino CurrTet;
        LockedBoard LogicBoard;
        public Form1()
        {
            InitializeComponent();
            GameState = 0;
            MenuMade = true;
            Menu();
            

        }

        private void LatTrans(object Sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.Up:
                    UpPress();
                    break;
                case Keys.Left:
                    LeftPress();
                    break;
                case Keys.Right:
                    RightPress();
                    break;
            }
        }

        private void UpPress()
        {
            int[] tempvects = new int[8];
            int[] temprel = new int[8];
            for (int i = 1; i < 4; i++)
            {
                temprel[2 * i] = -1 * CurrTet.blocks[i].relvects[1];
                temprel[2 * i + 1] = 1 * CurrTet.blocks[i].relvects[0];

                tempvects[2 * i] = CurrTet.blocks[0].Location[0] + temprel[2 * i];
                tempvects[2 * i + 1] = CurrTet.blocks[0].Location[1] + temprel[2 * i + 1];
                if ((tempvects[2 * i] < 0 )|| (tempvects[2 * i] > 9)) return;
                if ((tempvects[2 * i + 1] > 19) || (tempvects[2 * i + 1] < 0)) return;
                if (LogicBoard.Rows[tempvects[2 * i + 1]].blockrow[tempvects[2 * i]] != null) return;
            }

            CurrTet.Rotate(tempvects, temprel);

        }

        private void RightPress()
        {

            for (int i = 0; i < 4; i++)
            {
                if (CurrTet.blocks[i].Location[0] == 9) return;
                if (LogicBoard.Rows[CurrTet.blocks[i].Location[1]].blockrow[CurrTet.blocks[i].Location[0] + 1] != null) return;
            }
            CurrTet.right();
        }

        private void LeftPress()
        {

            for (int i = 0; i < 4; i++)
            {
                if (CurrTet.blocks[i].Location[0] == 0) return;
                if (LogicBoard.Rows[CurrTet.blocks[i].Location[1]].blockrow[CurrTet.blocks[i].Location[0] - 1] != null) return;
            }
            CurrTet.left();
        }
        private void TickEvent(object Sender, EventArgs e)
        {
            if (CheckColl())
            {
                LockTet();
                inittet();
            }
            else CurrTet.down();
        }

        private void LockTet()
        {
            for(int i = 0;i < 4; i++)
            {
                LogicBoard.AddBlock(CurrTet.blocks[i]);
            }
            LogicBoard.ClearRows();
        }

        private bool CheckColl()
        {
            for(int i = 0;i < 4; i++)
            {
                if (CurrTet.blocks[i].Location[1] > 18) return true;
                if (LogicBoard.Rows[CurrTet.blocks[i].Location[1] + 1].blockrow[CurrTet.blocks[i].Location[0]] != null) return true;
            }
            return false;
        }

        private void Menu()
        {

            Button Startbutton = new Button();
            Startbutton.Size = new Size(200, 50);
            Startbutton.Location = new Point(300, 200);
            Startbutton.Text = "Start Game";

            Button Quitbutton = new Button();
            Quitbutton.Size = new Size(200, 50);
            Quitbutton.Location = new Point(300, 500);
            Quitbutton.Text = "Quit Game";

            this.Controls.Add(Startbutton);
            this.Controls.Add(Quitbutton);

            Startbutton.Click += PressStart;
            Quitbutton.Click += PressQuit;

            return;
        }

        private void PressStart(object Sender, EventArgs e)
        {
            this.Controls.Clear();
            MenuMade = false;
            Board = new PictureBox();
            Board.Location = new Point(200, 100);
            Board.Size = new Size(400, 800);
            Board.Image = new Bitmap("..\\..\\..\\textures\\board.png");
            Controls.Add(Board);
            
            LogicBoard = new LockedBoard();

            inittet();

            falltimer = new Timer();
            falltimer.Interval = 800;
            falltimer.Tick += TickEvent;

            KeyDown += LatTrans;

            falltimer.Start();
        }

        private void inittet()
        {
            CurrTet = new Tetramino();
            for (int i = 0; i < 4; i++) 
            {
                Controls.Add(CurrTet.blocks[i].BlockTexture);
                CurrTet.blocks[i].BlockTexture.BringToFront();
            }
        }
        
        private void PressQuit(object Sender, EventArgs e)
        {
            Application.Exit();
        }

    }

    public class LockedBoard
    {
        public List<Row> Rows;
        public List<int> RowsToClear;

        public LockedBoard()
        {
            Rows = new List<Row>(20);
            RowsToClear = new List<int>(0);
            for (int i = 0; i < 20; i++) Rows.Add(new Row());
        }

        public void AddBlock(Block block)
        {
            Rows[block.Location[1]].blockrow[block.Location[0]] = block;
            if(++Rows[block.Location[1]].count >= 10)
            {
                RowsToClear.Add(block.Location[1]);
            }

        }

        public void ClearRows()
        {
            int offset = 1;
            int LoopRow;
            if (RowsToClear.Count == 0) return;
            LoopRow = RowsToClear.Max();

            while(LoopRow - offset > 0)
            {
                if (RowsToClear.Contains(LoopRow - offset)) offset++;
                
            }
        }
        public class Row
        {
            public List<Block?> blockrow;
            public int count;

            public Row()
            {
                blockrow = new List<Block?>(10);
                count = 0;
                for(int i = 0; i < 10; i++)
                {
                    blockrow.Add(null);
                }
            }

        }
    }

    public class Block
    {
        public PictureBox BlockTexture;
        public int[] Location;
        public int[] relvects;
        public Block(int xoff, int yoff)
        {
            relvects = new int[2] {xoff, yoff};
            Location = new int[2] { 4 + xoff, 0 + yoff };
            BlockTexture = new PictureBox();
            BlockTexture.Location = new Point(Location[0] * 40 + 200, Location[1] * 40 + 100);
            BlockTexture.Size = new Size(40, 40);
            BlockTexture.Image = new Bitmap("..\\..\\..\\textures\\redtile.bmp");
            if (Location[1] < 0)
            {
                BlockTexture.Visible = false;
            }
            
        }

        public void fall()
        {
            Location[1]++;
            if (Location[1] == 0) BlockTexture.Visible = true;
            UpdateImage();
            return;
        }

        public void UpdateImage()
        {
            BlockTexture.Location = new Point(200 + Location[0] * 40, 100 + Location[1] * 40);
        }

        public void left()
        {
            Location[0]--;
            UpdateImage();
            return;
        }

        public void right()
        {
            Location[0]++;
            UpdateImage();
            return;
        }
    }

    public class Tetramino
    {
        public List<Block> blocks;
        private List<string> color;
        private int[] offset;

        public Tetramino()
        {
            this.blocks = new List<Block>(4);
            color = new List<string>(1);
            offset = new int[8] { 0, 0, -1, 0, 1, 0, 2, 0 };
            for (int i = 0; i < 4; i++)
            {
                blocks.Add(new Block(offset[2 * i], offset[2 * i + 1]));
            }
        }

        public void down()
        {
            for (int i = 0; i < 4; i++) blocks[i].fall();
        }

        public void left()
        {
            for (int i = 0; i < 4; i++) blocks[i].left();
        }
        
        public void right()
        {
            for(int i = 0; i < 4; i++) blocks[i].right();
        }
        
        public void Rotate(int[] tempvects, int[] temprel)
        {
            for(int j = 1; j < 4; j++)
            {
                blocks[j].relvects[0] = temprel[2 * j];
                blocks[j].relvects[1] = temprel[(2 * j) + 1];

                blocks[j].Location[0] = tempvects[2 * j];
                blocks[j].Location[1] = tempvects[2 * j + 1];
                blocks[j].UpdateImage();  
            }
        }
    }
}
