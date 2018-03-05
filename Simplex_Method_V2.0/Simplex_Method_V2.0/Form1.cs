using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Simplex_Method_V2._0
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int n, m;

        public void UpdateDataGrid()
        {
            dataGridView1.Columns.Clear();

            for (int i = 0; i < numericUpDown1.Value; i++)
            {
                dataGridView1.Columns.Add("", "X" + (i + 1).ToString());
                if (i == Convert.ToInt16(numericUpDown1.Value - 1))
                {
                    dataGridView1.Columns.Add("", "<=>");
                    dataGridView1.Columns.Add("", "B");
                }
            }

            for (int j = 0; j < numericUpDown2.Value; j++)
            {
                dataGridView1.Rows.Add();
            }

            m = Convert.ToInt16(numericUpDown1.Value);
            n = Convert.ToInt16(numericUpDown2.Value);

        }


        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            UpdateDataGrid();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            UpdateDataGrid();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double[,] data = new double[n + 2, m * m + 2];
            double[,] newdata = new double[n + 2, m * m + 2];
            int[] sol = new int[n];

            double freelement, tmp;
            int mainrow = 0, maincol = 1, i, j = 0, g, countcols, M, end = 0;
            bool fl = false, w = false, v = false;
            countcols = n + m;
            M = m + n + 2;
            ////////////////////////////Построение массива с входными данными//////////////////////////////
            for (i = 0; i <= n; i++)
            {
                data[i, 0] = Convert.ToDouble(dataGridView1[m + 1, i].Value);
                for (j = 0; j < m; j++)
                {
                    data[i, j + 1] = Convert.ToDouble(dataGridView1[j, i].Value);
                }
            }

            for (i = 0; i <= n; i++)
                for (j = m + 1; j <= m + n; j++)
                {
                    if ((j - (m + 1)) == i)
                    {
                        char s = Convert.ToChar(dataGridView1[m, i].Value);
                        if (s == '<') data[i, j] = 1;
                        else if (s == '0') data[i, j] = 0;
                        else
                        {

                            if (s != '=')
                            {
                                countcols++;
                                data[i, j] = -1;
                            }
                            data[i, countcols] = 1;
                            w = true;
                            for (g = 0; g < m + n; g++)
                            {
                                data[n + 1, g] -= data[i, g];
                            }
                        }
                    }
                    else data[i, j] = 0;
                }


            ////////////CHECK//////////////
            for (i = 0; i <= n + 1; i++)
            {
                for (j = 0; j <= countcols; j++)
                {
                    richTextBox1.Text = richTextBox1.Text + Convert.ToString(data[i, j]) + "  ";
                }
                richTextBox1.Text = richTextBox1.Text + "\n";
            }
            richTextBox1.Text = richTextBox1.Text + "\n";
            //////////////////////////////

            //////////////////////////////////////////////////////////////////////////////////////////////

            ////////////////////////////////////////////-------START SIMPLEX-------///////////////////////////////////////////////////

            
            //----------///////////////////Начало работы вычисления исскуственной целевой ф-ции///////////////////////--------------//

            while (w)
            {
                w = false;
                ///////////////////////Определение главного столбца///////////////////////////
                tmp = data[n + 1, 1];

                for (j = 1; j <= m + n; j++)
                {
                    if (data[n + 1, j] < tmp)
                    {
                        tmp = data[n + 1, j];
                        maincol = j;
                    }
                }
                /////////////////////////////////////////////////////////////////////////////

                tmp = 11011;
                for (i = 0; i < n; i++)
                {
                    if (data[i, maincol] > 0)
                    {
                        data[i, countcols + 1] = data[i, 0] / data[i, maincol];
                        if (tmp == 11011)
                        {
                            tmp = data[i, countcols + 1];
                            mainrow = i;
                        }
                    }     //////////min = B[i]/A[i];
                    else
                    {
                        data[i, countcols + 1] = -1;
                        end++;
                    }
                    //if (data[i, countcols + 1] >= 0)
                    //{
                    //    tmp = data[i, countcols + 1];
                    //    mainrow = i;
                    //}
                }

                if (end == n)
                {
                    MessageBox.Show("Solving not found");
                    break;
                }


                ///////////////////////Определение главной строки////////////////////////////

                for (i = 0; i < n; i++)
                {
                    if ((data[i, countcols + 1] < tmp) && (data[i, countcols + 1] >= 0))
                    {
                        tmp = data[i, countcols + 1];
                        mainrow = i;
                    }
                }

                freelement = data[mainrow, maincol];
                MessageBox.Show(Convert.ToString(freelement));
                sol[mainrow] = maincol;
                /////////////////////////////////////////////////////////////////////////////

                //////////////Пересчёт нового плана методом прямоугольника///////////////////
                for (i = 0; i <= n + 1; i++)
                {
                    if (i == mainrow)
                        for (g = 0; g <= countcols; g++)
                        {
                            newdata[i, g] = data[i, g] / freelement;
                        }
                    else
                        for (j = 0; j <= countcols; j++)
                        {
                            if (j == maincol) newdata[i, j] = 0;
                            else
                            {
                                newdata[i, j] = Math.Round(data[i, j] - (data[mainrow, j] * data[i, maincol]) / freelement, 2, MidpointRounding.ToEven);
                            }

                            if (j == maincol && i == mainrow) newdata[i, j] = 1;
                        }
                }
                /////////////////////////////////////////////////////////////////////////////

                /////////////////////////////////PRINT//////////////////////////////////////
                for (i = 0; i <= n + 1; i++)
                {
                    for (j = 0; j <= countcols; j++)
                    {
                        richTextBox1.Text = richTextBox1.Text + Convert.ToString(newdata[i, j]) + "  ";
                    }
                    richTextBox1.Text = richTextBox1.Text + "\n";
                }
                richTextBox1.Text = richTextBox1.Text + "\n";
                ////////////////////////////////////////////////////////////////////////////

                //////////////////Копирование новых даных поверх старых/////////////////////
                for (i = 0; i <= n + 1; i++)
                {
                    for (j = 0; j < countcols; j++)
                    {
                        data[i, j] = newdata[i, j];
                    }
                }
                ////////////////////////////////////////////////////////////////////////////

                ///////////////////////////Проверка на зевершение///////////////////////////
                for (j = 0; j < m + n; j++)
                {
                    if (newdata[n + 1, j] != 0) w = true;
                }
                ////////////////////////////////////////////////////////////////////////////
            }
            //---------//////////////////////////////////////////////////////////////////////////////////////////----------//

            ////--------////////////////////////////Начало работы симплекс метода////////////////////////////////---------////
            ////
            countcols = m + n;
            ///////////////////////////Проверка на зaвершение///////////////////////////
            for (j = 1; j < m + n; j++)
            {
                if (newdata[n, j] >= 0) fl = true;
            }
            ////////////////////////////////////////////////////////////////////////////



            while (fl)
            {
                fl = false;
                ///////////////////////Определение главного столбца///////////////////////////
                tmp = data[n, 1];
                maincol = 1;

                if (radioButton1.Checked)
                {
                    for (j = 1; j <= m + n; j++)
                    {
                        if (data[n, j] > tmp)
                        {
                            tmp = data[n, j];
                            maincol = j;
                        }
                    }
                }
                else
                {
                    for (j = 1; j <= m + n; j++)
                    {
                        if (data[n, j] < tmp)
                        {
                            tmp = data[n, j];
                            maincol = j;
                        }
                    }
                }
                /////////////////////////////////////////////////////////////////////////////

                end = 0;
                tmp = 11011;
                for (i = 0; i < n; i++)
                {
                    if (data[i, maincol] > 0)
                    {
                        data[i, countcols + 1] = data[i, 0] / data[i, maincol];
                        if (tmp == 11011)
                        {
                            tmp = data[i, countcols + 1];
                            mainrow = i;
                        }
                    }     //////////min = B[i]/A[i];
                    else
                    {
                        data[i, countcols + 1] = -1;
                        end++;
                    }
                    //if (data[i, countcols + 1] >= 0)
                    //{
                    //    tmp = data[i, countcols + 1];
                    //    mainrow = i;
                    //}
                }

                if (end == n)
                {
                    MessageBox.Show("Solving not found");
                    break;
                }

                ///////////////////////Определение главной строки////////////////////////////


                for (i = 0; i < n; i++)
                {
                    if ((data[i, countcols + 1] <= tmp) && (data[i, countcols + 1] >= 0))
                    {
                        tmp = data[i, countcols + 1];
                        mainrow = i;
                    }
                }

                freelement = data[mainrow, maincol];
                sol[mainrow] = maincol;
                /////////////////////////////////////////////////////////////////////////////

                //////////////Пересчёт нового плана методом прямоугольника///////////////////
                for (i = 0; i <= n; i++)
                {
                    if (i == mainrow)
                        for (g = 0; g <= countcols; g++)
                        {
                            newdata[i, g] = data[i, g] / freelement;
                        }
                    else
                        for (j = 0; j <= countcols; j++)
                        {
                            if (j == maincol) newdata[i, j] = 0;
                            else
                            {
                                newdata[i, j] = Math.Round(data[i, j] - (data[mainrow, j] * data[i, maincol]) / freelement , 2, MidpointRounding.ToEven);
                            }

                            if (j == maincol && i == mainrow) newdata[i, j] = 1;
                        }
                }
                /////////////////////////////////////////////////////////////////////////////

                /////////////////////////////////PRINT//////////////////////////////////////
                for (i = 0; i <= n; i++)
                {
                    for (j = 0; j < countcols; j++)
                    {
                        richTextBox1.Text = richTextBox1.Text + Convert.ToString(Math.Round(newdata[i, j],2)) + "  ";
                    }
                    richTextBox1.Text = richTextBox1.Text + "\n";
                }
                richTextBox1.Text = richTextBox1.Text + "\n";
                ////////////////////////////////////////////////////////////////////////////

                //////////////////Копирование новых даных поверх старых/////////////////////
                for (i = 0; i <= n; i++)
                {
                    for (j = 0; j < countcols; j++)
                    {
                        data[i, j] = newdata[i, j];
                    }
                }
                ////////////////////////////////////////////////////////////////////////////

                ///////////////////////////Проверка на зевершение///////////////////////////
                if (radioButton1.Checked)
                {
                    for (j = 1; j < m + n; j++)
                    {
                        if (newdata[n, j] > 0) fl = true;
                    }
                }
                else
                {
                    for (j = 1; j < m + n; j++)
                    {
                        if (newdata[n, j] < 0) fl = true;
                    }
                }
                ////////////////////////////////////////////////////////////////////////////
                
            }

            MessageBox.Show("Result - " + Convert.ToString(newdata[n, 0] * (-1)));
            richTextBox1.Text = richTextBox1.Text + "\nResult - " + Convert.ToString(newdata[n, 0] * (-1));

            for (i = 0; i < n; i++)
            {
                if (sol[i] <= n && sol[i] != 0) richTextBox1.Text = richTextBox1.Text + "\nX" + Convert.ToString(sol[i]) + " = " + Convert.ToString(newdata[i, 0]);
                else v = true;
            }

            if (v) richTextBox1.Text = richTextBox1.Text + " Others variable = 0";
        }

    }
}
