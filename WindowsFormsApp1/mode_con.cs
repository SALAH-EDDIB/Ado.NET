using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string cnxSting = ConfigurationManager.ConnectionStrings["CnxString"].ConnectionString;
        int position = -1, personId;

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
             position = dataGridView1.CurrentRow.Index;
            personId = int.Parse(dataGridView1.Rows[position].Cells[0].Value.ToString());

            textBox1.Text = dataGridView1.Rows[position].Cells[1].Value.ToString();
            textBox2.Text = dataGridView1.Rows[position].Cells[2].Value.ToString();




        }

        private void button1_Click(object sender, EventArgs e)
        {

            SqlConnection cnx = new SqlConnection();
            cnx.ConnectionString = cnxSting ;
            string query = "select * from Person ;";
            SqlCommand command = new SqlCommand(query , cnx);
            cnx.Open();
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                dataGridView1.Rows.Clear();
                while (reader.Read())
                {
                    dataGridView1.Rows.Add(reader[0], reader[1], reader[2]);
                }


            } else
                MessageBox.Show("la table person est vide");

            cnx.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

            
            if (position != -1)
                return;

            using (SqlConnection cnx = new SqlConnection(cnxSting))
            {

                string query = "insert into Person values ('" + textBox1.Text.Trim() + "', '" + textBox2.Text.Trim() + "'  )";
                SqlCommand cmd = new SqlCommand(query , cnx);

                if (cnx.State == ConnectionState.Open)
                    cnx.Close();
                cnx.Open();

               int rowNum =  cmd.ExecuteNonQuery();
                button1_Click(sender, e);
                MessageBox.Show(rowNum + "Ligne effectée");

            }



        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button1_Click(sender, e);
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;


        }

        private void button5_Click(object sender, EventArgs e)
        {
            position = -1;
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
        }

        private void button4_Click(object sender, EventArgs e)
        {

            if (position == -1)
                return;

            using (SqlConnection cnx = new SqlConnection(cnxSting))
            {
                string query = "update Person set Firstname = @p1 , Lastname = @p2 where Id = @p3 ";
                SqlCommand cmd = new SqlCommand(query, cnx);
                cmd.Parameters.AddWithValue("@p1", textBox1.Text.Trim());
                cmd.Parameters.AddWithValue("@p2", textBox2.Text.Trim());
                cmd.Parameters.AddWithValue("@p3", personId);
                if (cnx.State == ConnectionState.Open)
                    cnx.Close();
                cnx.Open();
                int rowNum = cmd.ExecuteNonQuery();
                button1_Click(sender, e);
                MessageBox.Show(rowNum + "Ligne effectée");
                cnx.Close();
            }


        }

        private void button6_Click(object sender, EventArgs e)
        {

            using (SqlConnection cnx = new SqlConnection(cnxSting)) {
                
            string query = "select * from Person where Id = @p ";
            SqlCommand cmd = new SqlCommand(query, cnx);
            cmd.Parameters.AddWithValue("@p", int.Parse(textBox3.Text.Trim()));
            cnx.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                    reader.Read();
                MessageBox.Show("firstname : " + reader[1] + "\nlastname  : " + reader[2]);


            }
            else
                MessageBox.Show("la person n'existe pas ");
            cnx.Close();

        }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            if(position == -1)
            {
                MessageBox.Show("select a Person");
                return;
            }

            using (SqlConnection cnx = new SqlConnection(cnxSting))
            {
                string query = "delete from Person where id = " + personId;
                SqlCommand cmd = new SqlCommand(query, cnx);
                if (cnx.State == ConnectionState.Open)
                    cnx.Close();
                cnx.Open();
                int rowNum = cmd.ExecuteNonQuery();
                button1_Click(sender, e);
                MessageBox.Show(rowNum + "Ligne effectée");
                position = -1;
            }

        }
    }
}
