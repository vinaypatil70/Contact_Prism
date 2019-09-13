using PrismMVVMTestProject.Models;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using PrismMVVMTestProject.Enums;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using PrismMVVMTestProject.Properties;
using System.Linq;

namespace PrismMVVMTestProject.ViewModels
{
    class ContactViewModel : BindableBase
    {
        string filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Contacts.json";

        private Contact contact;
        public Contact Contact
        {
            get { return contact; }
            set { SetProperty(ref contact, value); }
        }

        private int tabSelectedIndex = 2;
        public int TabSelectedIndex
        {
            get { return tabSelectedIndex; }
            set { SetProperty(ref tabSelectedIndex, value); }
        }

        private ObservableCollection<Contact> contacts;
        public ObservableCollection<Contact> Contacts
        {
            get { return contacts; }
            set { SetProperty(ref contacts, value); }
        }

        private ObservableCollection<Country> countries;
        public ObservableCollection<Country> Countries
        {
            get { return countries; }
            set { SetProperty(ref countries, value); }
        }

        private Country selectedCountry;
        public Country SelectedCountry
        {
            get { return selectedCountry; }
            set
            {
                SetProperty(ref selectedCountry, value);
                PopulateRegions(value);
            }
        }

        private ObservableCollection<Region> regions;
        public ObservableCollection<Region> Regions
        {
            get { return regions; }
            set { SetProperty(ref regions, value); }
        }

        private Region selectedRegion;
        public Region SelectedRegion
        {
            get { return selectedRegion; }
            set
            {
                if (value != null)
                {
                    SetProperty(ref selectedRegion, value);
                    PopulateCities(value);
                }
            }
        }

        private ObservableCollection<City> cities;
        public ObservableCollection<City> Cities
        {
            get { return cities; }
            set { SetProperty(ref cities, value); }
        }

        private City selectedCity;
        public City SelectedCity
        {
            get { return selectedCity; }
            set
            {
                if (value != null)
                {
                    SetProperty(ref selectedCity, value);
                }
            }
        }

        public IList<MaritalStatus> MaritalStatus
        {
            get
            {
                return Enum.GetValues(typeof(MaritalStatus)).Cast<MaritalStatus>().ToList<MaritalStatus>();
            }
        }

        public RelayCommand cmdSaveContact { get; set; }
        public RelayCommand cmdReset { get; set; }
        public RelayCommand cmdBrowser { get; set; }
        public RelayCommand cmdDeleteAll { get; set; }

        public ContactViewModel()
        {
            this.cmdSaveContact = new RelayCommand(SaveContact);
            this.cmdReset = new RelayCommand(Reset);
            this.cmdBrowser = new RelayCommand(Browser);
            this.cmdDeleteAll = new RelayCommand(DeleteAll);
            Load();
        }

        private void Browser()
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                Contact.PhotoUri = op.FileName;
            }
        }

        private async void Load()
        {
            Contact = new Contact();
            Contacts = LoadContacts();
            Countries = await LoadCountries();
        }

        private ObservableCollection<Contact> LoadContacts()
        {
            ObservableCollection<Contact> contacts = new ObservableCollection<Contact>();
            if (System.IO.File.Exists(filePath))
            {
                string json = System.IO.File.ReadAllText(filePath);
                contacts = new ObservableCollection<Contact>(Newtonsoft.Json.JsonConvert.DeserializeObject<List<Contact>>(json));
            }
            return contacts;
        }

        private async Task<ObservableCollection<Country>> LoadCountries()
        {
            ObservableCollection<Country> countries = new ObservableCollection<Country>();
            HttpClient objClient = new HttpClient();
            HttpResponseMessage response = await objClient.GetAsync("http://battuta.medunes.net/api/country/all/?key=1e46f1476755626ad3bbe36742ccaa8d");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string result = await response.Content.ReadAsStringAsync();
                countries = new ObservableCollection<Country>(Newtonsoft.Json.JsonConvert.DeserializeObject<List<Country>>(result));
            }

            return countries;
        }

        private async void PopulateRegions(Country value)
        {
            HttpClient objClient = new HttpClient();
            HttpResponseMessage response = await objClient.GetAsync("http://battuta.medunes.net/api/region/" + value.code + "/all/?key=1e46f1476755626ad3bbe36742ccaa8d");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string result = await response.Content.ReadAsStringAsync();
                Regions = new ObservableCollection<Region>(Newtonsoft.Json.JsonConvert.DeserializeObject<List<Region>>(result));
            }
        }

        private async void PopulateCities(Region value)
        {
            HttpClient objClient = new HttpClient();
            HttpResponseMessage response = await objClient.GetAsync("http://battuta.medunes.net/api/city/" + value.country + "/search/?region=" + value.region + "&key=1e46f1476755626ad3bbe36742ccaa8d");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string result = await response.Content.ReadAsStringAsync();
                Cities = new ObservableCollection<City>(Newtonsoft.Json.JsonConvert.DeserializeObject<List<City>>(result));
            }
        }

        private void SaveContact()
        {
            if (ValidateContact())
            {
                Contacts.Add(Contact);
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(Contacts);
                System.IO.File.WriteAllText(filePath, json);
                MessageBoxResult result = MessageBox.Show(Resources.SaveMessage, Resources.Save, MessageBoxButton.OK, MessageBoxImage.Information);
                if (result == MessageBoxResult.OK)
                {
                    ShowContactDetails();
                    Reset();
                }
            }
            else
            {
                MessageBoxResult result = MessageBox.Show(Resources.ValidationErrorMessage, Resources.Validation, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private bool ValidateContact()
        {
            if (Contact != null)
            {
                if (!string.IsNullOrEmpty(Contact.FirstName) && !string.IsNullOrEmpty(Contact.MiddleName) && !string.IsNullOrEmpty(Contact.LastName) && !string.IsNullOrEmpty(Contact.PhoneNumber))
                {
                    return true;
                }
            }

            return false;
        }

        private void Reset()
        {
            Contact = new Contact();
        }

        private void ShowContactDetails()
        {
            TabSelectedIndex = 0;
        }

        private void DeleteAll()
        {
            MessageBoxResult result = MessageBox.Show(Resources.DeleteMessage, Resources.DeleteAll, MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (result == MessageBoxResult.OK)
            {
                System.IO.File.Delete(filePath);
                Contacts = LoadContacts();
            }

        }
    }
}


