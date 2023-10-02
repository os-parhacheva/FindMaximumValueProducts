using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Traineeship2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.Columns.Add("index", "index");
            dataGridView1.Columns.Add("volume", "volume");
            dataGridView1.Columns.Add("cost", "cost");
            dataGridView1.Columns[0].ReadOnly = true;
            textBox3.ReadOnly = true;

        }
        private void button2_Click(object sender, EventArgs e)
        {
            try 
            {
                dataGridView1.Rows.Clear();
                int M = Convert.ToInt32(textBox2.Text);
                for(int i=0; i < M; i++)
                {
                    dataGridView1.Rows.Add(i,0,0);
                }
            }
            catch
            {
                MessageBox.Show("Данные введены некорректно", "Error", MessageBoxButtons.OK,
                                                                       MessageBoxIcon.Exclamation,
                                                                       MessageBoxDefaultButton.Button1);
            }           
        }      
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                textBox3.Clear();
                if (string.IsNullOrEmpty(textBox1.Text) || Convert.ToInt32(textBox1.Text) == 0) throw new NullReferenceException("Не указан объем контейнера");

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (Convert.ToInt32(row.Cells[1].Value) == 0) 
                        throw new NullReferenceException("Указано нулевое значение объема товара");
                }
            
                List<int> selectedProducts = FindMaximumValueProducts();
                if (selectedProducts.Count > 0) textBox3.Text = string.Join("; \r\n", selectedProducts);
                else throw new NullReferenceException("Подходящих товаров нет");
            }
            catch(Exception ex) 
            {               
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK,
                                                     MessageBoxIcon.Exclamation,
                                                     MessageBoxDefaultButton.Button1);
            }
        }
        private List<int> FindMaximumValueProducts()
        {
            int M = Convert.ToInt32(textBox2.Text);  // Количество товаров
            int N = Convert.ToInt32(textBox1.Text);  // Объем контейнера

            // Список товаров в формате (объем, стоимость)
            List<Tuple<int, int>> products = new List<Tuple<int, int>>();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                int volume = Convert.ToInt32(row.Cells[1].Value);
                int cost = Convert.ToInt32(row.Cells[2].Value);
                products.Add(Tuple.Create(volume, cost));
            }
            
            // Создаем список пар (отношение стоимости к объему, индекс товара)
            var valuePerVolume = products.Select((product, index) => new { ValuePerVolume = (double)product.Item2 / product.Item1, Index = index })
                                      .OrderByDescending(product => product.ValuePerVolume)
                                      .ToList();

            int currentVolume = 0;  // Текущий объем в контейнере
            List<int> selectedProducts = new List<int>();  // Выбранные товары

            foreach (var product in valuePerVolume)
            {
                int productIndex = product.Index;
                int volume = products[productIndex].Item1;

                if (currentVolume + volume <= N)
                {
                    selectedProducts.Add(productIndex);
                    currentVolume += volume;
                }
            }
            selectedProducts.Sort();
            return selectedProducts;
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number= e.KeyChar;
            if (!Char.IsDigit(number) && (number != (char)Keys.Back) && (number != (char)Keys.Delete) && (number != '.'))
            {
                e.Handled = true;
            }
        }
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && (number != (char)Keys.Back) && (number != (char)Keys.Delete)&& (number != '.'))
            {
                e.Handled = true;
            }
        }
        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dataGridView1.CurrentCell.ColumnIndex !=0) //Desired Column
            {
                e.Control.KeyPress += new KeyPressEventHandler(dataGridView1_KeyPress);
            }
        }
        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && (number != (char)Keys.Back) && (number != (char)Keys.Delete) && (number != '.'))
            {
                e.Handled = true;
            }
        }
    }
}
