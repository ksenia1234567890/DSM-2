using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace лр2
// Необходимо реализовать программу, которая позволит производить   поиск по одномерному массиву

// Функционал должен удовлетворять следующим условиям:

// массив может быть введен с клавиатуры;
// сгенерирован случайно;
// считан с файла;
// пользователь может ввести число для поиска числа в массиве;
// программа может вывести минимальное или максимальное значение массива по запросу пользователя;
// результат поиска выводится как на экран, так и в файл с соответствующим текстом;
// пользователь может выбрать вариант поиска по массиву.


{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public int[] array = new int[10]; //массив, в котором производится поиск числа, минимального или максимального значения
        
        // Считывание массива из файла
        private void button1_Click(object sender, EventArgs e)
        {
            string way = "D:\\\\3 курс\\6 семестр\\рпм\\массив.txt";
            string line = File.ReadAllText(way);
            int y = 0;
            foreach (var i in line.Split('*'))
            {
                array[y] = int.Parse(i);
                y++;
            }
            string outcome = "";
            for (int i = 0; i < array.Length; i++)
            {
                outcome += Convert.ToString(array[i]) + " ";
            }
            textBox1.Text = $"{outcome}";
        }
        
        // Заполнение массива случайными числами
        private void button2_Click(object sender, EventArgs e)
        {
            string line = "";
            Random numeric = new Random();
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = numeric.Next(1, 100);
                line += Convert.ToString(array[i]) + " ";
            }

            textBox1.Text = $"{line}";
        }
        // Считывание массива из текстового поля
        private void textBox1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                string line = textBox2.Text;
                int j = 0;
                foreach (var i in line.Split(' '))
                {
                    array[j] = Convert.ToInt32(i);
                    j++;
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Неверный формат. Попробуйте ввести заново");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // Последовательный поиск. Поиск числа в массиве
            try
            {
                int number = int.Parse(textBox2.Text);
                int y = 0;

                for (int i = 0; i < array.Length; i++)
                {
                    if (number == array[i])
                    {
                        MessageBox.Show($"Введённое число присутствует в массиве. Позиция (0 не является первым элементом) - {i+1}");
                        y = 1;
                        FileStream fs = new FileStream("D:\\\\3 курс\\6 семестр\\рпм\\лр2.txt", FileMode.Append);
                        StreamWriter sw = new StreamWriter(fs);
                        sw.WriteLine($"Введённое число присутствует в массиве. Позиция (0 не является первым элементом) - {i+1}");
                        sw.Close();
                        fs.Close();
                        break;
                    }
                }
                if (y == 0)
                {
                    MessageBox.Show($"Число {number} отсутствует в массиве.");
                    FileStream fs = new FileStream("D:\\\\3 курс\\6 семестр\\рпм\\лр2.txt", FileMode.Append);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine($"Число {number} отсутствует в массиве.");
                    sw.Close();
                    fs.Close();
                }
            }
            catch(FormatException)
            {
                MessageBox.Show("Неверный формат. Попробуйте ввести заново");
            }
        }
        // Создание документа, где будут фиксироваться все действия с массивом
        private void Form1_Load(object sender, EventArgs e)
        {
            File.Create("D:\\\\3 курс\\6 семестр\\рпм\\лр2.txt");
        }

        // Сортировка пузырьком
        public int[] Sort(int[] copy)
        {
            int count = 0;
            for (int i = 1; ; i++)
            {
                if (i == copy.Length)
                {
                    i = 1;
                }
                int y = i - 1;
                if (copy[y] < copy[i])
                {
                    count++;
                }
                if (copy[y] > copy[i])
                {
                    int changer = copy[i];
                    copy[i] = copy[y];
                    copy[y] = changer;
                    count = 0;
                }
                if (count == array.Length)
                {
                    break;
                }
            }
            return copy;
        }

        // Бинарный поиск: пользователь нажал на кнопку "Бинарный поиск"
        // Сначала производится сортировка, после - бинарный поиск

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                // Сортировка пузырьком

                // копия массива
                int[] copy = new int[array.Length]; 
                array.CopyTo(copy, 0);
                // сортировка копии массива
                Sort(copy); 
                string copy_str = String.Join(" ", copy);
                MessageBox.Show($"Отсортированный массив\n{copy_str}");
                int number = int.Parse(textBox2.Text);
                // результат поиска
                int search = BinarySearch(copy, number); 

                // Если результат поиска равен -122, вывести сообщение и записать в файл "Заданное число не найдено"
                // Иначе вывести сообщение с числом и полученным результатом
                if (search == -122)
                {
                    textBox3.Text = $"Число {number}  не найдено";
                    FileStream fs = new FileStream("D:\\\\3 курс\\6 семестр\\рпм\\лр2.txt", FileMode.Append);
                    StreamWriter sw2 = new StreamWriter(fs);
                    sw2.WriteLine(textBox3.Text);
                    sw2.Close();
                    fs.Close();
                }
                else
                {
                    textBox3.Text = $"{search} = {number}. Заданное число найдено";
                    FileStream fs = new FileStream("D:\\\\3 курс\\6 семестр\\рпм\\лр2.txt", FileMode.Append);
                    StreamWriter sw2 = new StreamWriter(fs);
                    sw2.WriteLine(textBox3.Text);
                    sw2.Close();
                    fs.Close();
                }
            }
            catch(FormatException)
            {
                MessageBox.Show("Неверный формат. Попробуйте ввести заново");
            }

        }
        // Метод BinarySearch производит бинарный поиск

        public int BinarySearch(int[] copy, int number)
        {
            // Нижняя и верхняя границы
            int lower = 0;
            int upper = copy.Length - 1;
            
            // Цикл: пока нижняя граница меньше верхней или равна, цикл уменьшает или увеличивает границу
            while (lower <= upper)
            {
                // Середина массива
                var middle = (lower + upper) / 2;
                MessageBox.Show($"{middle}");

                if (number == copy[middle])
                {
                    return copy[middle];
                }

                if (number < copy[middle])
                {
                    // Верхняя граница
                    upper = middle - 1;
                }
                else
                {
                    // Нижняя граница
                    lower = middle + 1;
                }
            }
            // Число не найдено
            return -122;

        }
        // Вывод минимального значения
        private void button4_Click(object sender, EventArgs e)
        {
            //копия массива
            int[] copy = new int[array.Length]; 
            array.CopyTo(copy, 0);
            // сортировка копии массива
            Sort(copy); 
            textBox3.Text = $"Минимальное число в массиве: {copy[0]}";
            FileStream fs = new FileStream("D:\\\\3 курс\\6 семестр\\рпм\\лр2.txt", FileMode.Append);
            StreamWriter sw2 = new StreamWriter(fs);
            sw2.WriteLine(textBox3.Text);
            sw2.Close();
            fs.Close();
        }
        // Вывод максимального значения
        private void button5_Click(object sender, EventArgs e)
        {
            //копия массива
            int[] copy = new int[array.Length]; 
            array.CopyTo(copy, 0);
            // сортировка копии массива
            Sort(copy); 
            textBox3.Text = $"Максимальное число в массиве: {copy[copy.Length-1]}";
            FileStream fs = new FileStream("D:\\\\3 курс\\6 семестр\\рпм\\лр2.txt", FileMode.Append);
            StreamWriter sw2 = new StreamWriter(fs);
            sw2.WriteLine(textBox3.Text);
            sw2.Close();
            fs.Close();
        }
    }
}
