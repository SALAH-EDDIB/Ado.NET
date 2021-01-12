using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class mode_décon : Form
    {

        AdoNET Ado;
        int position = -1 , movieId;

        public mode_décon()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            Ado = new AdoNET();
            string query = "GetMoviesCat";
            Ado.Command.CommandText = query;
            Ado.Command.CommandType = CommandType.StoredProcedure;
            Ado.Command.Connection = Ado.Connection;
            Ado.Adapter.SelectCommand = Ado.Command;
            Ado.Adapter.Fill(Ado.Ds);
            Ado.DtCat = Ado.Ds.Tables[1];
            Ado.DtMovie = Ado.Ds.Tables[0];
            Ado.DtMovie.TableName = "Movie";
            dataGridView1.DataSource = Ado.DtMovie;
            comboBox1.DataSource = Ado.DtCat;
            comboBox1.DisplayMember = Ado.DtCat.Columns[1].ColumnName;
            comboBox1.ValueMember = Ado.DtCat.Columns[0].ColumnName;
            comboBox1.SelectedIndex = -1;



            

            










        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataRow movie = Ado.DtMovie.NewRow();
            movie[0] = (int.Parse(Ado.DtMovie.Rows[Ado.DtMovie.Rows.Count - 1][0].ToString()) + 1).ToString();
            movie[1] = textBox1.Text.Trim();
            movie[2] = textBox2.Text.Trim();
            movie[3] = comboBox1.SelectedValue.ToString();


            Ado.DtMovie.Rows.Add(movie);

            foreach (Control item in Controls)
            {
                if (item is TextBox)
                    item.Text = String.Empty;
                comboBox1.SelectedIndex = -1;

             }



        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                SqlCommandBuilder builder = new SqlCommandBuilder(Ado.Adapter);
                Ado.Adapter.Update(Ado.Ds , "Movie");
                MessageBox.Show("Sauvagarde");



            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);

            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (position == -1)
            {
                MessageBox.Show("select a movie");
                return;
            }
            

            foreach (DataRow row in Ado.DtMovie.Rows)
            {
                if (row[0].ToString().Equals(movieId.ToString())) {
                    row.Delete();
                    textBox1.Text = String.Empty;
                    textBox2.Text = String.Empty;
                    return;
                }

            }


          
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (position == -1)
            {
                MessageBox.Show("select a movie");
                return;
            }


            foreach (DataRow row in Ado.DtMovie.Rows)
            {
                if (row[0].ToString().Equals(movieId.ToString()))
                {
                    row[1] = textBox1.Text.Trim();
                    row[2] = textBox2.Text.Trim();
                    row[3] =  comboBox1.SelectedValue.ToString();
                }

            }

        }

        private void button5_Click(object sender, EventArgs e)
        {

                position = -1;
                textBox1.Text = string.Empty;
                textBox2.Text = string.Empty;
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            position = dataGridView1.CurrentRow.Index;
            movieId = int.Parse(dataGridView1.Rows[position].Cells[0].Value.ToString());
            textBox1.Text = dataGridView1.Rows[position].Cells[1].Value.ToString();
            textBox2.Text = dataGridView1.Rows[position].Cells[2].Value.ToString();
            comboBox1.SelectedValue = dataGridView1.Rows[position].Cells[3].Value.ToString();
        }
    }
}
