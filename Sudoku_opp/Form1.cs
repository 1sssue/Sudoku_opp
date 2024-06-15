using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sudoku_oop
{
    public partial class Form1 : Form
    {
        const int n = 3; // ����� �����
        const int sizeButton = 50; // ����� ������
        public int[,] map = new int[n * n, n * n]; // ������� ���������� ����� ��� ���������� ����� ������
        public Button[,] buttons = new Button[n * n, n * n]; // ������� ���������� ����� ��� ������
        public Form1()
        {
            InitializeComponent();
            GenerateMap();
        }

        public void GenerateMap() // ����� ��� ��������� ����� ������
        {
            for (int i = 0; i < n * n; i++) // ���� ��� �����
            {
                for (int j = 0; j < n * n; j++) // ���� ��� ��������
                {
                    map[i, j] = (i * n + i / n + j) % (n * n) + 1; // �������� ����� ����������
                    buttons[i, j] = new Button(); // ������� ���� ������ ��� ����� ������
                }
            }

            Random r = new Random();
            for (int i = 0; i < 40; i++)
            {
                ShuffleMap(r.Next(0, 5)); // ���������� ����� ���������� �����
            }

            CreateMap();
            HideCells();
        }

        public void HideCells() // ����� ��� ������������ ������ ������
        {
            int N = 40;
            Random r = new Random();
            while (N > 0)
            {
                for (int i = 0; i < n * n; i++)
                {
                    for (int j = 0; j < n * n; j++)
                    {
                        if (!string.IsNullOrEmpty(buttons[i, j].Text))
                        {
                            int a = r.Next(0, 3);
                            buttons[i, j].Text = a == 0 ? "" : buttons[i, j].Text;
                            buttons[i, j].Enabled = a == 0 ? true : false;

                            if (a == 0)
                                N--;
                            if (N <= 0)
                                break;
                        }
                    }
                    if (N <= 0)
                        break;
                }
            }
        }

        public void ShuffleMap(int i) // ����� ��� ����������� �����
        {
            switch (i)
            {
                case 0:
                    MatrixTransposition();
                    break;
                case 1:
                    SwapRowsInBlock();
                    break;
                case 2:
                    SwapColumnsInBlock();
                    break;
                case 3:
                    SwapBlocksInRow();
                    break;
                case 4:
                    SwapBlocksInColumn();
                    break;
                default:
                    MatrixTransposition();
                    break;
            }
        }

        public void SwapBlocksInColumn() // ����� ��� ����� ����� � �������
        {
            Random r = new Random();
            var block1 = r.Next(0, n);
            var block2 = r.Next(0, n);
            while (block1 == block2)
                block2 = r.Next(0, n);
            block1 *= n;
            block2 *= n;
            for (int i = 0; i < n * n; i++)
            {
                var k = block2;
                for (int j = block1; j < block1 + n; j++)
                {
                    var temp = map[i, j];
                    map[i, j] = map[i, k];
                    map[i, k] = temp;
                    k++;
                }
            }
        }

        public void SwapBlocksInRow() // ����� ��� ����� ����� � �����
        {
            Random r = new Random();
            var block1 = r.Next(0, n);
            var block2 = r.Next(0, n);
            while (block1 == block2)
                block2 = r.Next(0, n);
            block1 *= n;
            block2 *= n;
            for (int i = 0; i < n * n; i++)
            {
                var k = block2;
                for (int j = block1; j < block1 + n; j++)
                {
                    var temp = map[j, i];
                    map[j, i] = map[k, i];
                    map[k, i] = temp;
                    k++;
                }
            }
        }

        public void SwapRowsInBlock()  // ����� ��� ����� ����� � �����.
        {
            Random r = new Random();
            var block = r.Next(0, n);
            var row1 = r.Next(0, n);
            var line1 = block * n + row1;
            var row2 = r.Next(0, n);
            while (row1 == row2)
                row2 = r.Next(0, n);
            var line2 = block * n + row2;
            for (int i = 0; i < n * n; i++)
            {
                var temp = map[line1, i];
                map[line1, i] = map[line2, i];
                map[line2, i] = temp;
            }
        }

        public void SwapColumnsInBlock() // ����� ��� ����� �������� � �����
        {
            Random r = new Random();
            var block = r.Next(0, n);
            var row1 = r.Next(0, n);
            var line1 = block * n + row1;
            var row2 = r.Next(0, n);
            while (row1 == row2)
                row2 = r.Next(0, n);
            var line2 = block * n + row2;
            for (int i = 0; i < n * n; i++)
            {
                var temp = map[i, line1];
                map[i, line1] = map[i, line2];
                map[i, line2] = temp;
            }
        }

        public void MatrixTransposition() // ����� ��� �������������� �������
        {
            int[,] tMap = new int[n * n, n * n];
            for (int i = 0; i < n * n; i++)
            {
                for (int j = 0; j < n * n; j++)
                {
                    tMap[i, j] = map[j, i];
                }
            }
            map = tMap;
        }

        public void CreateMap() // ����� ��� ��������� ����� ������
        {
            for (int i = 0; i < n * n; i++)
            {
                for (int j = 0; j < n * n; j++)
                {
                    Button button = new Button();
                    buttons[i, j] = button;
                    button.Size = new Size(sizeButton, sizeButton);
                    button.Text = map[i, j].ToString();
                    button.Click += OnCellPressed;
                    button.Location = new Point(j * sizeButton, i * sizeButton);
                    this.Controls.Add(button);
                }
            }
        }

        public void OnCellPressed(object? sender, EventArgs e) // �������� ��䳿 ���������� �� ������
        {
            Button? pressedButton = sender as Button;
            if (pressedButton != null)
            {
                string buttonText = pressedButton.Text;
                if (string.IsNullOrEmpty(buttonText))
                {
                    pressedButton.Text = "1";
                }
                else
                {
                    int num = int.Parse(buttonText);
                    num++;
                    if (num == 10)
                        num = 1;
                    pressedButton.Text = num.ToString();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e) // �������� ��䳿 ���������� �� ������ "���������"
        {
            for (int i = 0; i < n * n; i++)
            {
                for (int j = 0; j < n * n; j++)
                {
                    var btnText = buttons[i, j].Text;
                    if (btnText != map[i, j].ToString())
                    {
                        MessageBox.Show("�����������!");
                        return;
                    }
                }
            }
            MessageBox.Show("��������!");
            for (int i = 0; i < n * n; i++)
            {
                for (int j = 0; j < n * n; j++)
                {
                    this.Controls.Remove(buttons[i, j]);
                }
            }
            GenerateMap();
        }

        private void button2_Click(object sender, EventArgs e) // �������� ��䳿 ���������� �� ������ "���� ���"
        {
            for (int i = 0; i < n * n; i++)
            {
                for (int j = 0; j < n * n; j++)
                {
                    this.Controls.Remove(buttons[i, j]);
                }
            }
            GenerateMap();
        }

        private void AutoFillMap() // ����� ��� ������������� ���������� ����� ���
        {
            for (int i = 0; i < n * n; i++)
            {
                for (int j = 0; j < n * n; j++)
                {
                    buttons[i, j].Text = map[i, j].ToString();
                }
            }
        }

        private void button3_Click_1(object sender, EventArgs e) // �������� ��䳿 ���������� �� ������ "�����������"
        {
            AutoFillMap();
        }

        private void button4_Click(object sender, EventArgs e) // �������� ��䳿 ���������� �� ������ "������� (Wiki)"
        {
            string url = "https://uk.wikipedia.org/wiki/%D0%A1%D1%83%D0%B4%D0%BE%D0%BA%D1%83#:~:text=%D0%BF%D0%BE%20%D1%88%D0%B0%D1%85%D0%B0%D1%85.-,%D0%9F%D1%80%D0%B0%D0%B2%D0%B8%D0%BB%D0%B0%20%D0%BA%D0%BB%D0%B0%D1%81%D0%B8%D1%87%D0%BD%D0%BE%D0%B3%D0%BE%20%D1%81%D1%83%D0%B4%D0%BE%D0%BA%D1%83,-%5B%D1%80%D0%B5%D0%B4.";
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }
    }
}
