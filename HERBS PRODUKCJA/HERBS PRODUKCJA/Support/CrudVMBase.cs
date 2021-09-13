using GalaSoft.MvvmLight.Messaging;
using HERBS_PRODUKCJA.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using HERBS_PRODUKCJA;

namespace HERBS_PRODUKCJA.Support
{
    public class CrudVMBase : NotifyUIBase
    {
        protected FZLEntities1 db = new FZLEntities1();

        protected void HandleCommand(CommandMessage action)
        {
            if (isCurrentView)
            {
                switch (action.Command)
                {
                    case CommandType.Insert:
                        Insert();
                        break;
                    case CommandType.InsertDW:
                        InsertDW();
                        break;
                    case CommandType.InsertTW:
                        InsertTW();
                        break;
                    case CommandType.Edit:
                        EditCurrent();
                        break;
                    case CommandType.Delete:
                        DeleteCurrent();
                        break;
                    case CommandType.Commit:
                        CommitUpdates();
                        break;
                    case CommandType.Refresh:
                        RefreshData();
                        break;
                    default:
                        break;
                }
            }
        }
        private Visibility throbberVisible = Visibility.Visible;
        public Visibility ThrobberVisible
        {
            get { return throbberVisible; }
            set
            {
                throbberVisible = value;
                RaisePropertyChanged();
            }
        }
        private string errorMessage;

        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                errorMessage = value;
                RaisePropertyChanged();
            }
        }

        protected virtual void CommitUpdates()
        {
        }
        protected virtual void DeleteCurrent()
        {
        }
        protected virtual void EditCurrent()
        {
        }
        protected virtual void Insert()
        {
        }
        protected virtual void InsertDW()
        {
        }
        protected virtual void InsertTW()
        {
        }
        protected virtual void RefreshData()
        {
            GetData();
           // Messenger.Default.Send<UserMessage>(new UserMessage { Message = "Data Refreshed" });
        }
        protected virtual void GetData()
        {
        }
        protected CrudVMBase()
        {
            GetData();
            Messenger.Default.Register<CommandMessage>(this, (action) => HandleCommand(action));
            Messenger.Default.Register<NavigateMessage>(this, (action) => CurrentUserControl(action));
        }
        protected bool isCurrentView = false;
        private void CurrentUserControl(NavigateMessage nm)
        {
            if (this.GetType() == nm.ViewModelType)
            {
                isCurrentView = true;
            }
            else
            {
                isCurrentView = false;
            }
        }
    }
}
