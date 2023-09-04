using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Genetic_Algorithm
{
    public partial class Form1 : Form
    {
        private int field_size = 5;
        public int population_size = 64;
        public int[] gen_size;
        public int generation_number = 0; // номер поколения
        public int generation_attempt = 0; // количество сгенерированых поколений, нужно на случай если вся популяция непригодна.
        public int[] start_point = new int[2] { 0, 0 };
        public int[] end_point = new int[2] { 4, 4 };
        int endCondition;
        public Cell[,] field = new Cell[10, 10];
        int generationLimit = 500;
        Hero bestIndivid;
        Thread thread;

        public Form1()
        {
            InitializeComponent();
            this.label1.Hide();
            this.label2.Hide();
            this.label3.Hide();
            this.label4.Hide();
            this.label5.Hide();
            this.label6.Hide();
            this.label7.Hide();
            this.label8.Hide();
            label9.Text = "";
            label10.Text = "";
            this.numericUpDown1.Hide();
            this.numericUpDown2.Hide();
            this.numericUpDown3.Hide();
            this.numericUpDown4.Hide();
            this.numericUpDown5.Hide();
            this.numericUpDown6.Hide();
            this.numericUpDown1.Value = population_size;
            this.numericUpDown2.Value = field_size;
            this.numericUpDown3.Value = 0;
            this.numericUpDown4.Value = 0;
            this.numericUpDown5.Value = field_size-1;
            this.numericUpDown6.Value = field_size-1;
            this.button1.Hide();
            this.Input_Error_population.Hide();
            this.Input_Error_field.Hide();
            this.Input_Error_Startpoint.Hide();
            this.Input_Error_Endpoint.Hide();
            createField();
            

        }

        void createField() {
            var h = this.splitContainer1.Panel2.Height;
            int size = (h-50) / field_size;
            this.field = new Cell[field_size, field_size];
            for (int i = 0; i < field_size; i++) {
                for (int j = 0; j < field_size; j++) {
                    Cell cell = new Cell();
                    cell.Size = new Size(size, size);
                    cell.Location = new Point(j * size, 50 + i * size);
                    cell.BackColor = Color.Transparent;
                    field[i,j] = cell;

                    this.splitContainer1.Panel2.Controls.Add(cell);

                }
            }
            try {
                field[end_point[0], end_point[1]].setEnd();
                field[start_point[0], start_point[1]].setStart();
            }
            catch (NullReferenceException) {
                field[0, 0].setStart();
                field[field_size-1, field_size-1].setEnd();
                end_point[0] = field_size - 1;
                end_point[1] = field_size - 1;
                this.numericUpDown5.Value = field_size;
                this.numericUpDown6.Value = field_size;
            }
        }

        
        void updateField(int new_size) {
            for (int i =0; i < field_size;i++) {
                for (int j = 0;j < field_size;j++) {
                    this.splitContainer1.Panel2.Controls.Remove(field[i, j]);
                }
            }
            this.field_size = new_size;
            createField();
        }

        void setUpField()
        {
            

        }

        void setCells(int number_cells)
        {

        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if ((thread != null) && thread.IsAlive)
                thread.Abort();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void StartButton_Click(object sender, EventArgs e) //алгоритм работает в отдельном потоке от формы
        {
            thread = new Thread(delegate () {
                GeneticAlgorithm();
                ShowFoundedWay();
            });
            thread.Start();
        }

        void GeneticAlgorithm() {
            if (checkSettings()) {
                this.label9.Show();
                this.label10.Show();
                this.label9.Text = "Поиск подходящего гена...";
                this.label10.Text = $"Популяция номер {1}";
                int[,] field_projection = makeFieldprojection(); // создать матрицу на основе графического поля

                GeneticClass geneticClass = new GeneticClass(field_size, end_point, population_size);
                var population = geneticClass.generatePopulation(population_size);
                int attemptNumber = 1; // номер попытки генерации
                int generationNumber = 0; // номер популяции
                for (int i = 0; i < population_size; i++) { // 1 этап - вычисление приспособленности первого поколения
                    population[i].Move(field_projection, start_point[0], start_point[1]);
                    population[i].fitness = geneticClass.Fitfunction(population[i]);
                }
                var to_break = false;

                while (!to_break) {
                    generationNumber++;
                    population = geneticClass.selection(population);
                    population = geneticClass.crossingover(population);
                    geneticClass.mutation(population);

                    for (int i = 0; i < population_size; i++) { // считаем приспособленность потомков этап - вычисление приспособленности
                        population[i].Move(field_projection, start_point[0], start_point[1]);
                        population[i].fitness = geneticClass.Fitfunction(population[i]);
                    }

                    for (int i = 0; i < population_size; i++) { // проверка на окончание
                        if (population[i].fitness == endCondition) {
                            // здесь результат алгоритма
                            bestIndivid = population[i];
                            to_break = true;
                            break;

                        }
                    }

                    if (generationNumber == generationLimit) {
                        population = geneticClass.generatePopulation(population_size);
                        attemptNumber++;
                        generationNumber = 0;
                        this.label10.Text = $"Популяция номер {attemptNumber}";
                    }

                }
                this.label9.Text = "код найден";
            }
            else
                MessageBox.Show("Ошибка в заданных значениях");
        }

        int[,] makeFieldprojection() {
            int[,] field = new int[field_size,field_size];
            for (int i = 0; i < field_size; i++) {
                for (int j = 0; j < field_size; j++) {
                    switch ((int)this.field[i,j].state){
                        case -1:
                            field[i, j] = -1;
                            break;
                        case 0:
                            field[i, j] = 0; break;
                        case 1:
                            field[i, j] = 1;
                            break;
                        case 2:
                            field[i, j] = 0;
                            break;
                    }
                }
            }
            return field;
        }


        void ShowFoundedWay() {
            int x = start_point[0];
            int y = start_point[1];
            Color tmp_color;
            foreach (int i in bestIndivid.getGen()) {
                switch (i) {
                    case 0:
                        //tmp_color = field[x, y].BackColor;
                        field[x,y].BackColor = Color.Yellow;
                        Thread.Sleep(550);
                        //field[x,y].BackColor = tmp_color;
                        break;
                    case 1:
                        x--;
                       // tmp_color = field[x, y].BackColor;
                        field[x, y].BackColor = Color.Yellow;
                        Thread.Sleep(1000);
                        //field[x, y].BackColor = tmp_color;
                        break;
                    case 2:
                        y++;
                        //tmp_color = field[x, y].BackColor;
                        field[x, y].BackColor = Color.Yellow;
                        Thread.Sleep(1000);
                        //field[x, y].BackColor = tmp_color;
                        break;
                    case 3:
                        x++;
                       // tmp_color = field[x, y].BackColor;
                        field[x, y].BackColor = Color.Yellow;
                        Thread.Sleep(1000);
                       // field[x, y].BackColor = tmp_color;
                        break;
                    case 4:
                        y--;
                       // tmp_color = field[x, y].BackColor;
                        field[x, y].BackColor = Color.Yellow;
                        Thread.Sleep(1000);
                       // field[x, y].BackColor = tmp_color;
                        break;
                }
            }
            x = start_point[0];
            y = start_point[1];
            foreach (int i in bestIndivid.getGen()) {
                switch (i) {
                    case 1:
                        x--;
                        field[x, y].BackColor = Color.Transparent;
                        break;
                    case 2:
                        y++;
                        field[x, y].BackColor = Color.Transparent;
                        break;
                    case 3:
                        x++;
                        field[x, y].BackColor = Color.Transparent;
                        break;
                    case 4:
                        y--;
                        field[x, y].BackColor = Color.Transparent;
                        break;
                }
            }
            field[start_point[0], start_point[1]].setStart();
            field[end_point[0], end_point[1]].setEnd();

        }



        private void setObstacles_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void SetEndButton_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e) { // остановка
            if (thread.IsAlive)
                thread.Abort();
            foreach (Cell cell in field) 
                if (cell.state == Cell.States.END) 
                    cell.BackColor = Color.Red;
                else if(cell.state == Cell.States.Start)
                    cell.BackColor = Color.Green;
                else if(cell.state != Cell.States.OBSTACLE)
                    cell.BackColor = Color.Transparent;

            label9.Text = "";
            label10.Text = "";
                
            
        }

        private void Settings_Click(object sender, EventArgs e) {
            // поле изменения размера популяции, не забыть про ошибку размера популяции
            // поле изменения размера поля, сразу перерисовать и сделать ошибку о размере
            // Сделать 2 поля ввода начала и конца, не забыть про ошибки
            // сделать чекбокс для возможности установки препятствий, согласовать с Cell
            if (label1.Visible ==  false) {
                this.label1.Show();
                this.label2.Show();
                this.label3.Show();
                this.label4.Show();
                this.label5.Show();
                this.label6.Show();
                this.label7.Show();
                this.label8.Show();
                this.numericUpDown1.Show();
                this.numericUpDown2.Show();
                this.numericUpDown3.Show();
                this.numericUpDown4.Show();
                this.numericUpDown5.Show();
                this.numericUpDown6.Show();
                this.button1.Show();
            }
            else {
                this.label1.Hide();
                this.label2.Hide();
                this.label3.Hide();
                this.label4.Hide();
                this.label5.Hide();
                this.label6.Hide();
                this.label7.Hide();
                this.label8.Hide();
                this.numericUpDown1.Hide();
                this.numericUpDown2.Hide();
                this.numericUpDown3.Hide();
                this.numericUpDown4.Hide();
                this.numericUpDown5.Hide();
                this.numericUpDown6.Hide();
                this.button1.Hide();

            }

        }

        private void label1_Click(object sender, EventArgs e) {

        }

        private void label3_Click(object sender, EventArgs e) {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e) {

        }

        private void label2_Click(object sender, EventArgs e) {

        }

        private void button1_Click_2(object sender, EventArgs e) { // принять настройки
            // популяция
            if (this.numericUpDown1.Value < 6) {
                this.Input_Error_population.Show();
            }
            else {
                population_size = (int)this.numericUpDown1.Value;
                this.Input_Error_population.Hide();
            }
            // поле
            if (this.numericUpDown2.Value <= 3) {
                this.Input_Error_field.Show();
            }
            else {
                int new_field_size = (int)this.numericUpDown2.Value;
                updateField(new_field_size);
                this.Input_Error_field.Hide();
            }
            // Стартпоинты
            if ((this.numericUpDown3.Value > field_size - 1) || (this.numericUpDown3.Value < 0) || (this.numericUpDown4.Value > field_size - 1) || (this.numericUpDown4.Value < 0) ||
                this.numericUpDown3.Value == this.numericUpDown5.Value || this.numericUpDown4.Value == this.numericUpDown6.Value) {
                this.Input_Error_Startpoint.Show();
            }
            else {
                this.SetStart((int)this.numericUpDown3.Value, (int)this.numericUpDown4.Value);
                start_point[0] = (int)this.numericUpDown3.Value;
                start_point[1] = (int)this.numericUpDown4.Value;
                this.Input_Error_Startpoint.Hide();
            }
            // Эндпоинты
            if ((this.numericUpDown5.Value > field_size - 1) || (this.numericUpDown5.Value < 0) || (this.numericUpDown6.Value > field_size - 1) || (this.numericUpDown6.Value < 0) ||
                this.numericUpDown3.Value == this.numericUpDown5.Value || this.numericUpDown4.Value == this.numericUpDown6.Value) {
                this.Input_Error_Endpoint.Show();
            }
            else {
                this.SetEnd((int)this.numericUpDown5.Value, (int)this.numericUpDown6.Value);
                end_point[0] = (int)this.numericUpDown5.Value;
                end_point[1] = (int)this.numericUpDown6.Value;
                this.Input_Error_Endpoint.Hide();
            }
        }

        private void SetEnd(int new_end0,int new_end1) {
            field[end_point[0], end_point[1]].setFree();
            field[new_end0, new_end1].setEnd();
        }

        private void SetStart(int new_start0, int new_start1) {
            field[start_point[0], start_point[1]].setFree();
            field[new_start0, new_start1].setStart();
        }

        private bool checkSettings() { // если все условия соблюдены алгоритм начнется
            if (this.numericUpDown1.Value > 6)
                if (this.numericUpDown2.Value > 3)
                    if ((start_point[0] <= field_size - 1) && (start_point[0] >= 0) && (start_point[1] <= field_size - 1) && (start_point[0] >= 0))
                        if ((end_point[0] <= field_size - 1) && (end_point[0] >= 0) && (end_point[1] <= field_size - 1) && (end_point[1] >= 0))
                            if ((end_point[0] != start_point[0]) || (end_point[1] != start_point[1]))
                                return true;
            return false;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e) {
            MessageBox.Show("Визуализация работы \"Генетического Алгоритмa\". Задача алгоритма из случайного набора кодов, по которым строится путь, найти подходящий, используя генетический алгоритм и показать его.\n"+
                            "Алгоритм создает популяцию ботов, каждый имеент свой ген(код по которому он проходит путь), изначально ген создается рандомно, затем, путем скрещивания и отбора находится наиболее оптимальный\n" +
                            "Start - запуск алгоритма\n" +
                            "Остановить - остановка работы алгоритма\n" +
                            "Настройки - открывают панель с настройками\n"+
                            "Размер популяции должен быть не менее 6\n"+
                            "Размер поля квадаратный, указывается число которое задает размер поля n*n\n"+
                            "Координаты начала - точка откуда алгоритм начинает искать путь до точки назначения\n"+
                            "При нажатии на поле можно установить препятствия"); // в идеале сделать страницу справки, но я не знаю как это сделать
        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e) {

        }

        private void label9_Click(object sender, EventArgs e) {

        }

        private void label9_Click_1(object sender, EventArgs e) {

        }

        private void button3_Click_2(object sender, EventArgs e) {
            
        }
    }
}
