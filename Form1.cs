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
        public Form1()
        {
            InitializeComponent();
            GameState = 0;
            MenuMade = true;
            Menu();


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
        }
        
        private void PressQuit(object Sender, EventArgs e)
        {
            Application.Exit();
        }
    }

}
