using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using PrismMVVMTestProject.Enums;

namespace PrismMVVMTestProject.Models
{
    public class Contact : BindableBase
    {
        private string firstName;
        public string FirstName
        {
            get { return firstName; }
            set
            {
                SetProperty(ref firstName, value);
                OnPropertyChanged("FullName");
            }
        }
        private string middleName;
        public string MiddleName
        {
            get { return middleName; }
            set
            {
                SetProperty(ref middleName, value);
                OnPropertyChanged("FullName");
            }
        }

        private string lastName;
        public string LastName
        {
            get { return lastName; }
            set
            {
                SetProperty(ref lastName, value);
                OnPropertyChanged("FullName");
            }
        }

        public string FullName { get { return string.Format("{0} {1} {2}", firstName, MiddleName, lastName); } }

        private Gender gender = Gender.Male;
        public Gender Gender
        {
            get { return gender; }
            set
            {
                SetProperty(ref gender, value);
            }
        }

        private string phoneNumber;
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set
            {
                SetProperty(ref phoneNumber, value);
            }
        }

        private string photoUri;
        public string PhotoUri
        {
            get { return photoUri; }
            set
            {
                SetProperty(ref photoUri, value);
            }
        }

        private DateTime dob = DateTime.Now;
        public DateTime DOB
        {
            get { return dob; }
            set
            {
                SetProperty(ref dob, value);
                CalculateAge();
            }
        }

        private string age;
        public string Age
        {
            get { return age; }
            set
            {
                SetProperty(ref age, value);
            }
        }

        private double height;
        public double Height
        {
            get { return Math.Round(height, 2); }
            set
            {
                SetProperty(ref height, value);
            }
        }

        private string currentAddress;
        public string CurrentAddress
        {
            get { return currentAddress; }
            set
            {
                SetProperty(ref currentAddress, value);
                CopyAddrress();
            }
        }

        private bool isBothAddressSame;
        public bool IsBothAddressSame
        {
            get { return isBothAddressSame; }
            set
            {
                SetProperty(ref isBothAddressSame, value);
                CopyAddrress();
            }
        }
       
        private string permanantAddress;
        public string PermanantAddress
        {
            get { return permanantAddress; }
            set
            {
                SetProperty(ref permanantAddress, value);
            }
        }

        private MaritalStatus maritalStatus = MaritalStatus.Single;
        public MaritalStatus MaritalStatus
        {
            get { return maritalStatus; }
            set
            {
                SetProperty(ref maritalStatus, value);
            }
        }

        private void CalculateAge()
        {
            int age = DateTime.Now.Year - DOB.Year;
            if (DateTime.Now.DayOfYear < DOB.DayOfYear)
                age = age - 1;

            Age = age.ToString();
        }

        private void CopyAddrress()
        {
            if (IsBothAddressSame)
            {
                this.PermanantAddress = this.CurrentAddress;
                this.OnPropertyChanged(() => this.PermanantAddress);
            }
        }

    }
}
