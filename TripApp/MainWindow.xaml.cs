using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace TripApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string DataFileName = @"..\..\trips.txt";
        List<Trip> tripList = new List<Trip>();
        public MainWindow()
        {
            InitializeComponent();
            LoadDataFromFile();
        }
        private void SaveFile()
        {
            using (StreamWriter writer = new StreamWriter(DataFileName))
            {
                foreach (Trip t in tripList)
                {
                    writer.WriteLine(t.ToDataString());
                }
            }
        }
        private void BtnSaveSelected_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "trips files(*.trips)|*.trips|All files(*.*)|*.*";
            saveFileDialog.Title = "Save trips in file";
            if (saveFileDialog.ShowDialog() == true)
            {
                string allData = "";
                foreach (Trip trip in tripList)
                {
                    allData += trip.ToString() + "\n";
                }
                File.WriteAllText(saveFileDialog.FileName, allData);
            }
        }
        //below is done and is working
        private void BtnDeleteTrip_Click(object sender, RoutedEventArgs e)
        {
            if (LvTrips.SelectedIndex == -1)
            {
                MessageBox.Show("Please choose a trip to delete");
                return;
            }
            Trip tripToBeDeleted = (Trip)LvTrips.SelectedItem;
            if (tripToBeDeleted != null)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure to delete the trip?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    tripList.Remove(tripToBeDeleted);
                    ResetValue();
                }
            }
        }
        
        private void BtnUpdateTrip_Click(object sender, RoutedEventArgs e)
        {
            if (LvTrips.SelectedIndex == -1)
            {
                MessageBox.Show("Please choose a trip");
                return;
            }

            Trip tripToBeUpdated = (Trip)LvTrips.SelectedItem;
            tripToBeUpdated.Destination = txtDestination.Text;
            tripToBeUpdated.Name = txtName.Text;
            tripToBeUpdated.Passport = txtPassport.Text; 
            tripToBeUpdated.Departure = dpDeparture.Text;
            tripToBeUpdated.ReturnDate = dpReturn.Text;

            ResetValue();
        }

        private void BtnAddTrip_Click(object sender, RoutedEventArgs e)
        {
            string destination = txtDestination.Text;
            string name = txtName.Text;
            string passport = txtPassport.Text;

            if ((destination == ""))
            {
                MessageBox.Show("Please choose a destination");
                return;
            }
            if ((name == ""))
            {
                MessageBox.Show("Please enter the name");
                return;
            }
            if ((passport == ""))
            {
                MessageBox.Show("Please enter the passport");
                return;
            }
            DateTime? selectedDepartureDate = dpDeparture.SelectedDate;
            if (selectedDepartureDate == null)
            {
                MessageBox.Show("Please choose a departure date");
                return;
            }
            DateTime? selectedReturnDate = dpReturn.SelectedDate;
            if (selectedReturnDate == null)
            {
                MessageBox.Show("Please choose a return date");
                return;
            }


            Trip trip = new Trip(destination, name, passport, selectedDepartureDate.ToString(), selectedReturnDate.ToString());
            tripList.Add(trip);
            ResetValue();
        }
        private void ResetValue()
        {
            LvTrips.Items.Refresh();
            txtDestination.Clear();
            txtName.Clear();
            txtPassport.Clear();
            dpDeparture.Text = "";
            dpReturn.Text = "";
            //LvTrips.SelectedIndex = -1;
            BtnDeleteTrip.IsEnabled = false;
            BtnUpdateTrip.IsEnabled = false;
        }
        private void LoadDataFromFile()
        {
            using (StreamReader sr = new StreamReader(DataFileName))
            {
                string line = sr.ReadLine();
                while (line != null)
                {
                    string[] myData = line.Split(';');

                    Trip trip = new Trip(myData[0], myData[1], myData[2], myData[3], myData[4]);
                    tripList.Add(trip);
                    line = sr.ReadLine();
                }
                LvTrips.ItemsSource = tripList;

            }
        }

        private void LvTrips_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BtnDeleteTrip.IsEnabled = true;
            BtnUpdateTrip.IsEnabled = true;

            var selectedItem = LvTrips.SelectedItem;
            if (selectedItem is Trip)
            {
                Trip trip = (Trip)LvTrips.SelectedItem;
                txtDestination.Text = trip.Destination;
                txtName.Text = trip.Name;
                txtPassport.Text = trip.Passport;
                //dpDueDate.Text = task.DueDate;
                dpDeparture.SelectedDate = DateTime.Parse(trip.Departure);
                dpReturn.SelectedDate = DateTime.Parse(trip.ReturnDate);
                
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveFile();
        }

        
    }
}
