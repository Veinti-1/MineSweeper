using System;
using System.Drawing;
using System.Windows.Forms;

namespace MineSweeper
{
    public partial class Form1 : Form
    {
        static int n, Tminas;
        Random rnd = new Random();
        int[,] TableroVals;
        bool usarBandera = false;
        int Descubierto, segundos, Banderas, fontS;
        bool endCondition;
        DataGridViewCellStyle styleGrass = new DataGridViewCellStyle();
        DataGridViewCellStyle styleDig = new DataGridViewCellStyle();
        DataGridViewCellStyle styleFlag = new DataGridViewCellStyle();
        public Form1()
        {
            InitializeComponent();
            comboBox1.Text = "EASY";
        }
        private void Inicio(int fSize)
        {
            segundos = 0;
            Tablero.Rows.Clear();
            Tablero.RowCount = n;
            Tablero.ColumnCount = n;
            styleGrass.BackColor = Color.FromArgb(81, 219, 199);
            styleDig.BackColor = Color.FromArgb(147, 186, 180);
            styleFlag.BackColor = Color.Red;
            styleDig.Alignment = DataGridViewContentAlignment.MiddleCenter;
            styleFlag.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Tablero.Font = new Font("Arial", fSize, FontStyle.Bold);
            Tablero.ClearSelection();
            for (int i = 0; i < n; i++)
            {
                DataGridViewColumn columna = Tablero.Columns[i];
                DataGridViewRow fila = Tablero.Rows[i];
                columna.Width = (int)(Tablero.Width / n);
                fila.Height = (int)(Tablero.Height / n);
                for (int j = 0; j < n; j++)
                {
                    TableroVals[j, i] = 0;
                    Tablero[j, i].Style = styleGrass;
                }
            }
            Banderas = Tminas;
            label1.Text = "🏴 FLAGS: " + Banderas;
            Minas(Tminas);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            endCondition = false;
            timer1.Enabled = !endCondition;
            switch (comboBox1.Text)
            {
                case "EASY":
                    n = 9;
                    Tminas = 10;
                    fontS = 16;
                    break;
                case "MEDIUM":
                    n = 16;
                    Tminas = 40;
                    fontS = 12;
                    break;
                case "HARD":
                    n = 24;
                    Tminas = 99;
                    fontS = 7;
                    break;
            }
            label2.Visible = false;
            label2.Text = "YOU WIN";
            Descubierto = 0;
            TableroVals = new int[n, n];
            usarBandera = false;
            radioButton1.Checked = usarBandera;
            Inicio(fontS);
        }
        private bool PosValida(int x, int y)
        {
            return (x >= 0) && (x < n) && (y >= 0) && (y < n);
        }
        private void Minas(int cantidad)
        {
            int x, y;
            for (int i = 0; i < cantidad; i++)
            {
                x = rnd.Next(0, n);
                y = rnd.Next(0, n);
                if (TableroVals[x, y] < 8)
                {
                    TableroVals[x, y] = 9;
                    AsignarVal(x, y);
                }
                else
                    i--;
            }
            //WriteVals();
        }
        private void AsignarVal(int x, int y)
        {
            if (PosValida(x + 1, y))
                TableroVals[x + 1, y]++;
            if (PosValida(x - 1, y))
                TableroVals[x - 1, y]++;
            if (PosValida(x, y + 1))
                TableroVals[x, y + 1]++;
            if (PosValida(x, y - 1))
                TableroVals[x, y - 1]++;
            if (PosValida(x + 1, y + 1))
                TableroVals[x + 1, y + 1]++;
            if (PosValida(x - 1, y - 1))
                TableroVals[x - 1, y - 1]++;
            if (PosValida(x + 1, y - 1))
                TableroVals[x + 1, y - 1]++;
            if (PosValida(x - 1, y + 1))
                TableroVals[x - 1, y + 1]++;
        }
        private void Revelar(int x, int y)
        {
            if (TableroVals[x, y] > 8)
            {
                Tablero[x, y].Value = "💣";
                Tablero[x, y].Style = styleFlag;
                label2.Text = "YOU LOSE";
                label2.Visible = true;
                endCondition = true;
                timer1.Enabled = !endCondition;
            }
            else
            {
                if (TableroVals[x, y] > 0)
                {
                    Tablero[x, y].Value = TableroVals[x, y];
                    Tablero[x, y].Style = Colores(TableroVals[x, y]);
                    TableroVals[x, y] = -1;
                    Descubierto++;
                }
                else if (TableroVals[x, y] == 0)
                    RevelarRec(x, y);
            }
        }
        private void RevelarRec(int x, int y)
        {
            if (TableroVals[x, y] != -1)
            {
                if (TableroVals[x, y] > 0)
                {
                    Tablero[x, y].Value = TableroVals[x, y];
                    Tablero[x, y].Style = Colores(TableroVals[x, y]);
                    TableroVals[x, y] = -1;
                }
                else
                {
                    TableroVals[x, y] = -1;
                    Tablero[x, y].Value = " ";
                    Tablero[x, y].Style = styleDig;
                    if (PosValida(x + 1, y))
                        RevelarRec(x + 1, y);
                    if (PosValida(x - 1, y))
                        RevelarRec(x - 1, y);
                    if (PosValida(x, y + 1))
                        RevelarRec(x, y + 1);
                    if (PosValida(x, y - 1))
                        RevelarRec(x, y - 1);
                    if (PosValida(x + 1, y + 1))
                        RevelarRec(x + 1, y + 1);
                    if (PosValida(x - 1, y - 1))
                        RevelarRec(x - 1, y - 1);
                    if (PosValida(x + 1, y - 1))
                        RevelarRec(x + 1, y - 1);
                    if (PosValida(x - 1, y + 1))
                        RevelarRec(x - 1, y + 1);
                }
                Descubierto++;
            }
        }
        private void Marcar(int x, int y)
        {
            if (Tablero[x, y].Style == styleFlag)
            {
                Tablero[x, y].Style = styleGrass;
                Tablero[x, y].Value = " ";
                Banderas++;
            }
            else if (Tablero[x, y].Style != styleDig && Banderas > 0)
            {
                Tablero[x, y].Style = styleFlag;
                Tablero[x, y].Value = "🏴";
                Banderas--;
            }
            label1.Text = "🏴 FLAGS: " + Banderas;
        }
        private DataGridViewCellStyle Colores(int val)
        {
            switch (val)
            {
                case 1:
                    DataGridViewCellStyle style1 = new DataGridViewCellStyle();
                    style1.BackColor = styleDig.BackColor;
                    style1.ForeColor = Color.FromArgb(0, 60, 255);
                    style1.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    return style1;
                case 2:
                    DataGridViewCellStyle style2 = new DataGridViewCellStyle();
                    style2.BackColor = styleDig.BackColor;
                    style2.ForeColor = Color.FromArgb(0, 200, 30);
                    style2.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    return style2;
                case 3:
                    DataGridViewCellStyle style3 = new DataGridViewCellStyle();
                    style3.BackColor = styleDig.BackColor;
                    style3.ForeColor = Color.FromArgb(232, 14, 14);
                    style3.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    return style3;
                case 4:
                    DataGridViewCellStyle style4 = new DataGridViewCellStyle();
                    style4.BackColor = styleDig.BackColor;
                    style4.ForeColor = Color.FromArgb(31, 42, 145);
                    style4.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    return style4;
                case 5:
                    DataGridViewCellStyle style5 = new DataGridViewCellStyle();
                    style5.BackColor = styleDig.BackColor;
                    style5.ForeColor = Color.FromArgb(145, 31, 31);
                    style5.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    return style5;
                case 6:
                    DataGridViewCellStyle style6 = new DataGridViewCellStyle();
                    style6.BackColor = styleDig.BackColor;
                    style6.ForeColor = Color.FromArgb(33, 156, 143);
                    style6.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    return style6;
                case 7:
                    DataGridViewCellStyle style7 = new DataGridViewCellStyle();
                    style7.BackColor = styleDig.BackColor;
                    style7.ForeColor = Color.FromArgb(35, 37, 41);
                    style7.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    return style7;
                case 8:
                    DataGridViewCellStyle style8 = new DataGridViewCellStyle();
                    style8.BackColor = styleDig.BackColor;
                    style8.ForeColor = Color.FromArgb(108, 128, 125);
                    style8.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    return style8;
                default:
                    return styleDig;
            }
        }
        private void CheckWin()
        {
            if (Descubierto == ((int)Math.Pow(n, 2) - Tminas) && Banderas == 0)
            {
                label2.Visible = true;
                endCondition = true;
                timer1.Enabled = !endCondition;
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            label3.Text = "🕑 TIME: \n" + segundos;
            segundos++;
        }
        private void Tablero_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!endCondition)
            {
                if (usarBandera)
                    Marcar(e.ColumnIndex, e.RowIndex);
                else if (Tablero[e.ColumnIndex, e.RowIndex].Style.BackColor != Color.Red)
                    Revelar(e.ColumnIndex, e.RowIndex);
                CheckWin();
            }
            Tablero.ClearSelection();
        }
        private void radioButton1_Click(object sender, EventArgs e)
        {
            usarBandera = !usarBandera;
            radioButton1.Checked = usarBandera;
        }
        private void WriteVals()
        {
            // Debug Tool
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (TableroVals[j, i] > 8)
                    {
                        Tablero[j, i].Style = styleFlag;
                    }
                    if (TableroVals[j, i] == 1 )
                    {
                        Tablero[j, i].Style = styleDig;
                    }
                    Tablero[j, i].Value = TableroVals[j, i];
                }
                Console.WriteLine();
            }
        }
    }
}
