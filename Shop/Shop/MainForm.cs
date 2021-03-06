﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Shop
{
    public partial class MainForm : Form
    {
        MyNewCollection shop;
        Journal j;
        bool Admin_in = false;
        public MainForm()
        {
            InitializeComponent();
            shop = new MyNewCollection();
            j = new Journal();
            shop.CollectionCountChanged += new MyNewCollection.CollectionHandler(j.CollectionCountChanged);
            shop.CollectionReferenceChanged += new MyNewCollection.CollectionHandler(j.CollectionReferenceChanged);
            shop.Name = "Магазин № 1";
        }

        void VisibleMainForm()
        {
            MainField.Visible = true;
            menuStrip1.Visible = true;
            InSystem.Visible = false;
            textBoxLogin.Clear();
            textBoxPass.Clear();
            textBoxLogin.Visible = false;
            textBoxPass.Visible = false;
            labelLogin.Visible = false;
            labelPass.Visible = false;
            labelErorIn.Text = "";
            labelErorIn.Visible = false;

        }
        void NonVisibleMainForm()
        {
            MainField.Visible = false;
            menuStrip1.Visible = false;
            InSystem.Visible = true;
            textBoxLogin.Visible = true;
            textBoxPass.Visible = true;
            labelLogin.Visible = true;
            labelPass.Visible = true;
            labelErorIn.Visible = true;
        }
        void WriteLogIn()
        {

            string s = "";
            DateTime date = DateTime.Now;
            if (Admin_in == true)
            {
                s = String.Format("Вошёл:Admin Время:{0}", date);
            }
            else
            {
                s = String.Format("Вошёл:User Время:{0}", date);
            }
            using (var writer = new StreamWriter("Log_in_out.txt", true))
            {
                writer.WriteLine(s);
            }
        }
        void WriteLogOut()
        {
            string s = "";
            DateTime date = DateTime.Now;
            if (Admin_in == true)
            {
                s = String.Format("Вышел:Admin Время:{0}", date);
            }
            else
            {
                s = String.Format("Вышел:User Время:{0}", date);
            }
            using (var writer = new StreamWriter("Log_in_out.txt", true))
            {
                //Добавляем к старому содержимому файла
                writer.WriteLine(s);
                writer.WriteLine("**********************************");
            }
        }
        private void OpenFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                MainField.Items.Clear();
                string[] Shop_file = File.ReadAllLines("Shop.txt");
                shop.Shop.Clear();
                for (int i = 0; i < Shop_file.Length; i++)
                {
                    string[] s = Shop_file[i].Split();
                    Product obj = null;
                    if (s[10] == "Коллории:")
                    {
                        if (s[12] == "Свежее")
                        {
                            obj = new Food_product(s[1], s[3], double.Parse(s[5]), int.Parse(s[7]), int.Parse(s[9]), double.Parse(s[11]), true);
                        }
                        else
                        {
                            obj = new Food_product(s[1], s[3], double.Parse(s[5]), int.Parse(s[7]), int.Parse(s[9]), double.Parse(s[11]), false);
                        }
                    }
                    else
                    {
                        if (s[10] == "Год_публикации:")
                        {
                            obj = new Library_Product(s[1], s[3], double.Parse(s[5]), int.Parse(s[7]), double.Parse(s[9]), int.Parse(s[11]));
                        }
                        else
                        {
                            obj = new Industrial_product(s[1], s[3], double.Parse(s[5]), int.Parse(s[7]), double.Parse(s[9]), double.Parse(s[11]));
                        }
                    }
                    shop.Shop.Add(i, obj);
                }
                foreach (KeyValuePair<int, Product> p1 in shop.Shop)
                {
                    MainField.Items.Add(p1.Value.ToString());
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Файл со всей информацией удалён");
            }
        }

        private void SaveFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] s = new string[shop.Length];
            int i = 0;
            foreach (KeyValuePair<int, Product> p in shop.Shop)
            {
                s[i] = p.Value.ToString();
                i++;
            }
            File.WriteAllLines("Shop.txt", s);
        }

        private void FileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainField.Items.Clear();
            foreach (KeyValuePair<int, Product> p in shop.Shop)
            {
                MainField.Items.Add(p.Value.ToString());
            }
        }

        private void LogDisplayToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MainField.Items.Clear();
            foreach (JournalEntry p in j.list)
            {
                MainField.Items.Add(p.ToString());
            }
        }

        private void LogInFileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string[] s = new string[j.list.Count];
            int i = 0;
            foreach (JournalEntry p in j.list)
            {
                s[i] = p.ToString();
                i++;
            }
            File.WriteAllLines("Journal.txt", s);
        }

        private void SellProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MainField.SelectedItem != null)
            {
                string s = MainField.SelectedItem.ToString();
                int i = 0;
                foreach (KeyValuePair<int, Product> p in shop.Shop)
                {
                    if (s == p.Value.ToString())
                    {
                        i = p.Key;
                        break;
                    }
                }
                shop.Sale_Event(i, s);
                MainField.Items.Clear();
                foreach (KeyValuePair<int, Product> p in shop.Shop)
                {
                    MainField.Items.Add(p.Value.ToString());
                }
            }
            else
            {
                MessageBox.Show("Необходимо выделить товар");
            }
        }

        private void DeleteProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MainField.SelectedItem != null)
            {
                string s = MainField.SelectedItem.ToString();
                int i = 0;
                foreach (KeyValuePair<int, Product> p in shop.Shop)
                {
                    if (s == p.Value.ToString())
                    {
                        i = p.Key;
                        break;
                    }
                }
                shop.Delete_Event(i, s);
                MainField.Items.Clear();
                foreach (KeyValuePair<int, Product> p in shop.Shop)
                {
                    MainField.Items.Add(p.Value.ToString());
                }
            }
            else
            {
                MessageBox.Show("Необходимо выделить товар");
            }
        }

        private void ClearMainFieldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainField.Items.Clear();
            j.list.Clear();
            File.Delete("Journal.txt");
            File.Delete("Shop.txt");
            shop.Shop.Clear();
        }

        private void CountOfFoodProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int food = (from elem in shop.Shop where (elem.Value is Food_product) select elem).Count();
            if (food != 0)
                MessageBox.Show("Продовольственных товаров: " + food.ToString(), "Количество продовольственных товаров");
            else
            {
                MessageBox.Show("Продовольственных товаров нет");
            }
        }

        private void CountIndustrialProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int food = (from elem in shop.Shop where (elem.Value is Industrial_product) select elem).Count();
            if (food != 0)
                MessageBox.Show("Промышленных товаров: " + food.ToString(), "Количество промышленных товаров");
            else
            {
                MessageBox.Show("Промышленных товаров нет");
            }
        }

        private void CountBooksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int food = (from elem in shop.Shop where (elem.Value is Library_Product) select elem).Count();
            if (food != 0)
                MessageBox.Show("Товаров для библеотеки: " + food.ToString(), "Количество товаров для библеотеки");
            else
            {
                MessageBox.Show("Товаров для библиотеки нет");
            }
        }

        private void OutputOfFoodProductToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var food = from Food in shop.Shop where Food.Value is Food_product select Food;
            if (food.Count() != 0)
            {
                MainField.Items.Clear();
                foreach (var f in food)
                    MainField.Items.Add(f.Value.ToString());
            }
            else
            {
                MessageBox.Show("Продовольственных товаров нет");
            }
        }

        private void OutputIndustrialProductToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var food = from Food in shop.Shop where Food.Value is Industrial_product select Food;
            if (food.Count() != 0)
            {
                MainField.Items.Clear();
                foreach (var f in food)
                    MainField.Items.Add(f.Value.ToString());
            }
            else
            {
                MessageBox.Show("Промышленных товаров нет");
            }
        }

        private void OutputBooksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var food = from Food in shop.Shop where Food.Value is Library_Product select Food;
            if (food.Count() != 0)
            {
                MainField.Items.Clear();
                foreach (var f in food)
                    MainField.Items.Add(f.Value.ToString());
            }
            else
            {
                MessageBox.Show("Товаров для библиотеки нет");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void AddProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            shop.Add_Event();
            MainField.Items.Clear();
            foreach (KeyValuePair<int, Product> p in shop.Shop)
            {
                MainField.Items.Add(p.Value.ToString());
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void InSystem_Click(object sender, EventArgs e)
        {
            string path = @"Pass.txt";
            string[] lines = new string[4];
            int i = 0;
            string line;
            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
            {

                while ((line = sr.ReadLine()) != null)
                {
                    lines[i] = line;
                    i++;
                }
            }
            if (textBoxLogin.Text == lines[0] & Auth.GetHash(textBoxPass.Text) == lines[1])
            {
                VisibleMainForm();
                Admin_in = true;
                WriteLogIn();
            }
            else
            {
                if (textBoxLogin.Text == lines[2] & Auth.GetHash(textBoxPass.Text) == lines[3])
                {
                    VisibleMainForm();
                    WriteLogIn();
                }
                else
                {
                    textBoxLogin.Clear();
                    textBoxPass.Clear();
                    labelErorIn.Text = "Логин или пароль введены неверно!!!\n            Повторите попытку!!!";
                }
            }
        }

        private void OutSystemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WriteLogOut();
            NonVisibleMainForm();
        }
    }
}
