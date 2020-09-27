using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Comboflistviewandcomboboxandprogressbar
{

    public partial class MainWindow : Window
    {
        List<Items> items = new List<Items>();
        String itemtImage;
        String itemName;
        String itemType;
        double itemPrice;
        public MainWindow()
        {
            InitializeComponent();

        }


        private void Add_To_Menu(object sender, RoutedEventArgs e)
        {
            itemtImage = textImage.Text;
            itemName = textName.Text;
            itemType = textType.Text;
            itemPrice = Convert.ToDouble(textPrice.Text);
            try
            {
                Image img = new Image();
                img.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Resources/" + itemtImage));
                img.Width = 20;
                img.Height = 20;
                TextBlock txtName = new TextBlock();
                txtName.VerticalAlignment = VerticalAlignment.Center;
                txtName.Inlines.Add(itemName);

                StackPanel panel = new StackPanel();
                panel.Orientation = Orientation.Horizontal;
                panel.Children.Add(img);
                panel.Children.Add(txtName);
                comboBox.Items.Add(panel);
                status.Content = "Successfully Added Item to Menu";
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid Input!");
            }
        }

        private void Add_To_Basket(object sender, RoutedEventArgs e)
        {
            double sum = 0;
            if (comboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please Select an Item!");
                return;
            }
            else
            {
                items.Add(new Items() { Image = (AppDomain.CurrentDomain.BaseDirectory + "Resources/" + itemtImage), Name = itemName, Type = itemType, Price = itemPrice });
                listView.ItemsSource = null;
                listView.ItemsSource = items;

                foreach (Items item in items)
                {
                    sum += item.Price;
                }
                total.Content = "Total: " + sum.ToString();
                status.Content = "Successfully Added to Basket";
            }
        }

        private void Remove_Item(object sender, RoutedEventArgs e)
        {
            if (listView.SelectedIndex == -1)
            {
                MessageBox.Show("Please Select an Item to Remove");
            }
            else
            {
                Items selectedUser = (Items)listView.SelectedItem;
                listView.ItemsSource = null;
                items.Remove(selectedUser);
                listView.ItemsSource = items;
                status.Content = "Successfully Removed an Item";
                double sum = 0;
                foreach (Items item in items)
                {
                    sum += item.Price;
                }
                total.Content = sum.ToString();
            }
        }

        private void Pay(object sender, RoutedEventArgs e)
        {
            if (textMoney.Text == "")
            {
                MessageBox.Show("Please enter your money amount.");
                return;
            }
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            status.Content = "Processing...";
            worker.RunWorkerAsync();
        }
        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i <= 100; i++)
            {
                (sender as BackgroundWorker).ReportProgress(i);
                Thread.Sleep(10);
            }
        }
        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            double sum = 0;
            double change = 0;
            progressBar.Value = e.ProgressPercentage;

            if (progressBar.Value == 100)
            {
                int money = Convert.ToInt32(textMoney.Text);
                foreach (Items item in items)
                {
                    sum += item.Price;
                }

                change = money - sum;
                status.Content = "Successfully Saved Transaction";

                MessageBox.Show("Total: " + sum.ToString());
                MessageBox.Show("Money: " + money.ToString());
                MessageBox.Show("Change: " + change.ToString());
            }
        }
        public class Items
        {
            public string Image { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public double Price { get; set; }
        }
    }
}
