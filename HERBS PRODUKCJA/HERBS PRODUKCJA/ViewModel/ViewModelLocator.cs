/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocatorTemplate xmlns:vm="clr-namespace:HERBS_PRODUKCJA.ViewModel"
                                   x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using HERBS_PRODUKCJA.Model;
using HERBS_PRODUKCJA.Helpers;

namespace HERBS_PRODUKCJA.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<IDataService, Design.DesignDataService>();
            }
            else
            {
                SimpleIoc.Default.Register<IDataService, DataService>();
            }
            // SimpleIoc.Default.Register<IDialogService, DialogService>();

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<ProdukcjaViewModel>();
            SimpleIoc.Default.Register<ProdukcjaDetaleViewModel>();
            SimpleIoc.Default.Register<ProdukcjaLaczenieViewModel>();
            SimpleIoc.Default.Register<ProdukcjaMaszynaParamMonitViewModel>();
            SimpleIoc.Default.Register<ProdukcjaMagazynViewModel>();
            SimpleIoc.Default.Register<ProdukcjaMagazynDokumentViewModel>();
            SimpleIoc.Default.Register<ProdukcjaTowaryViewModel>();
            // SimpleIoc.Default.Register<LoginViewViewModel>();
        }

        /// <summary>
        /// Gets the Main property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
        public ProdukcjaViewModel Produkcja
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ProdukcjaViewModel>();
            }
        }
        public ProdukcjaDetaleViewModel ProdukcjaDetale
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ProdukcjaDetaleViewModel>();
            }
        }
        public ProdukcjaLaczenieViewModel ProdukcjaLaczenie
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ProdukcjaLaczenieViewModel>();
            }
        }
        public ProdukcjaMagazynViewModel ProdukcjaMagazyn
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ProdukcjaMagazynViewModel>();
            }
        }

        public ProdukcjaMagazynDokumentViewModel ProdukcjaMagazynDokument
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ProdukcjaMagazynDokumentViewModel>();
            }
        }
        public ProdukcjaTowaryViewModel ProdukcjaTowary
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ProdukcjaTowaryViewModel>();
            }
        }
        /*
        public LoginViewViewModel Login
        {
            get
            {
                return ServiceLocator.Current.GetInstance<LoginViewViewModel>();
            }
        }

    */

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
        }
    }
}