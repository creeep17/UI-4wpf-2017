using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace UI_4wpf_2017
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            comboBox1.ItemsSource = NamedBrush.AllNamedBrushes();
            comboBox1.SelectedValuePath = "Brush";
            comboBox1.SelectedIndex = 0;
            comboBox1.Focus();

        }
        class NamedBrush
        {
            static Dictionary<string, SolidColorBrush> colorNames =
            new Dictionary<string, SolidColorBrush>(141);
            string name;
            static NamedBrush()
            {
                foreach (System.Reflection.PropertyInfo pi in
                typeof(Colors).GetProperties())
                    colorNames[pi.Name] = new
                    SolidColorBrush((Color)pi.GetValue(null, null));
            }
            NamedBrush(string n)
            {
                name = n;
            }
            public SolidColorBrush Brush
            {
                get { return colorNames[name]; }
            }
            public string Name
            {
                get { return name; }
            }
            public override string ToString()
            {
                return name;
            }
            public static IEnumerable<NamedBrush> AllNamedBrushes()
            {
                return colorNames.Select(e => new NamedBrush(e.Key));
            }
            public static Brush GetBrush(string name)
            {
                return colorNames.ContainsKey(name ?? "") ?
                colorNames[name] : null;
            }

        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            rect1.Fill = (Brush)comboBox1.SelectedValue;
        }

        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            rect2.Fill = NamedBrush.GetBrush((string)listBox1.SelectedItem);
        }

        private void label1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            listBox1.SelectedIndex = listBox1.Items.Add(comboBox1.Text);
        }

        private void label2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int i = listBox1.SelectedIndex;
            if (i == -1)
            {
                Console.Beep();
                return;
            }
            listBox1.Items.RemoveAt(i);
            if (i == listBox1.Items.Count)
                i--;
            listBox1.SelectedIndex = i;
        }

        private void label3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int i = listBox1.SelectedIndex;
            if (i == -1)
                label1_MouseDown(null, null);
            else
            {
                listBox1.Items.Insert(i, comboBox1.Text);
                listBox1.SelectedIndex = i;
            }
        }

        private void label4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int i = listBox1.SelectedIndex;
            if (i <= 0)
            {
                Console.Beep();
                return;
            }
            var x = listBox1.Items[i];
            listBox1.Items[i] = listBox1.Items[i - 1];
            listBox1.Items[i - 1] = x;
            listBox1.SelectedIndex = i - 1;
        }

        private void label5_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int i = listBox1.SelectedIndex;
            if (i == -1 || i == listBox1.Items.Count - 1)
            {
                Console.Beep();
                return;
            }
            var x = listBox1.Items[i];
            listBox1.Items[i] = listBox1.Items[i + 1];
            listBox1.Items[i + 1] = x;
            listBox1.SelectedIndex = i + 1;
        }

        private void label6_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (listBox1.Items.Count == 0)
            {
                Console.Beep();
                return;
            }
            File.WriteAllLines("LISTBOXES.dat",
            listBox1.Items.Cast<string>());
        }

        private void label7_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!File.Exists("LISTBOXES.dat"))
            {
                Console.Beep();
                return;
            }
            listBox1.Items.Clear();
            foreach (var e1 in File.ReadLines("LISTBOXES.dat"))
                listBox1.Items.Add(e1);
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
        }
    }
}
