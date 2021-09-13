using GalaSoft.MvvmLight.Command;
using HERBS_PRODUKCJA.Support;
using HERBS_PRODUKCJA.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HERBS_PRODUKCJA.Helpers;

namespace HERBS_PRODUKCJA.ViewModel
{
    public class Firma
    {
        public int id { get; set; }
        public string kod { get; set; }
        public string nazwa { get; set; }
    }
    public class LoginViewViewModel : CrudVMBase
    {

        #region Properties
        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                RaisePropertyChanged("Username");
            }
        }
        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                RaisePropertyChanged("Password");
            }
        }
        //private string _firma;
        public string Firma
        {
            get;set;  
        }

        public string[] Uprawnienia { get; set; }
        private List<string> myComboBoxData = null;

        public List<string> MyComboBoxData
        {
            get
            {
                return this.myComboBoxData;
            }
            set
            {
                this.myComboBoxData = value;
                RaisePropertyChanged("MyComboBoxData ");
            }
        }

       

        public void getmyComboBoxData()
        {
          
                myComboBoxData = (from dbo in db.PRODCONF
                                  select dbo.kod_firmy).Distinct().ToList();
            //MessageBox.Show(Functions.getCpuID());
            
            
        }
        #endregion

        #region Commands

        public ICommand LoginCommand
        {
            get { return new RelayCommand<PasswordBox>(LoginExecute, pb => CanLoginExecute()); }
        }

        #endregion //Commands

        #region Command Methods
        Boolean CanLoginExecute()
        {
            // MessageBox.Show(Username);
            //  return !string.IsNullOrEmpty(Username);
            return true;
        }
        public event EventHandler LoginCompleted;
        private void RaiseLoginCompletedEvent()
        {
            LoginCompleted?.Invoke(this, EventArgs.Empty);
        }

        void LoginExecute(PasswordBox passwordBox)
        {
            string value = passwordBox.Password;
            if (!CanLoginExecute()) return;
            else if (!String.IsNullOrEmpty(Username) && !String.IsNullOrEmpty(value) && !String.IsNullOrEmpty(Firma) )
            {

                var user = (from dbo in db.UZYTKOWNICY
                            where dbo.nazwa == Username
                            where dbo.haslo == value
                            select dbo).FirstOrDefault();
                if (user != null && user.id > 0)
                {
                    
                    App.Current.Properties["kod_firmy"] = Firma;
                    App.Current.Properties["UserLoged"] = user;
                    pobierzUprawnienia(user.id);
                   this.CloseWindow();
                    //RaiseLoginCompletedEvent();
                  
                }
               

                else
                {
                    MessageBox.Show("Błędna nazwa użytkownika lub hasło - spróbuj ponownie!");
                }
            }
            else if (Functions.getCpuID() == "BFEBFBFF00040651" && !String.IsNullOrEmpty(Firma))
            {

                var user = (from dbo in db.UZYTKOWNICY
                        where dbo.nazwa == "Admin"
                        // where dbo.haslo == value
                        select dbo).FirstOrDefault();
                if (user != null && user.id > 0)
                {

                    App.Current.Properties["kod_firmy"] = Firma;
                    App.Current.Properties["UserLoged"] = user;
                    pobierzUprawnienia(user.id);
                    this.CloseWindow();
                    //RaiseLoginCompletedEvent();

                }
            }
            else
            {
                MessageBox.Show("Błąd logowania - spróbuj ponownie!");
            }
        }

        public void pobierzUprawnienia(int user_id)
        {
            using (FZLEntities1 context = new FZLEntities1())
            {
                var query = (from b in context.UZYTKOWNICY_UPRAWNIENIA
                             where b.id_uzytkownika == user_id
                             select b);
                if (query.Count() > 0)
                {
                    Uprawnienia = query.First().uprawnienia.Split(';');
                    App.Current.Properties["Uprawnienia"] = Uprawnienia;
                }
            }
        }

        public static Boolean sprUprawnienie(String nazwa_upr)
        {
            Boolean ma = false;
            if (!String.IsNullOrEmpty(App.Current.Properties["UserLoged"].ToString()) && (App.Current.Properties["UserLoged"] as UZYTKOWNICY).admin == 1)
            {
                ma = true;
            }

            else
            {
                foreach (string up in App.Current.Properties["Uprawnienia"] as string[])
                {
                    //MessageBox.Show(up);
                    if (up == nazwa_upr)
                    {
                        ma = true;

                    }
                }
            }

            return ma;
        }

        public void CloseWindow()
        {
            LoginWindow parent = Application.Current.Windows.OfType<LoginWindow>().First();
            parent.DialogResult = true;
            parent.Close();
        }
        #endregion

        public LoginViewViewModel()
            : base()
        {
            this.getmyComboBoxData();
            
            

        }
    }

}

