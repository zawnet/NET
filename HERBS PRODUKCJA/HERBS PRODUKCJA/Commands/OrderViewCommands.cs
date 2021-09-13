/*
 * Created by SharpDevelop.
 * User: Sylwek
 * Date: 2015-04-24
 * Time: 23:14
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows.Input;
using HERBS_PRODUKCJA.ViewModel;

namespace HERBS_PRODUKCJA.Commands
{
    public class RemoveGroupCommand : ICommand
    {
        #region ICommand implementation

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this._viewModel.RemoveGroup();
        }

        #endregion

        private ProdukcjaLaczenieViewModel _viewModel;

        public RemoveGroupCommand(ProdukcjaLaczenieViewModel viewModel)
        {
            this._viewModel = viewModel;
        }
    }

    public class GroupByCustomerCommand : ICommand
    {
        #region ICommand implementation

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this._viewModel.GroupByCustomer();
        }

        #endregion

        private ProdukcjaLaczenieViewModel _viewModel;

        public GroupByCustomerCommand(ProdukcjaLaczenieViewModel viewModel)
        {
            this._viewModel = viewModel;
        }
    }


    public class GroupByYearMonthCommand : ICommand
    {
        #region ICommand implementation

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this._viewModel.GroupByYearMonth();
        }

        #endregion

        private ProdukcjaLaczenieViewModel _viewModel;

        public GroupByYearMonthCommand(ProdukcjaLaczenieViewModel viewModel)
        {
            this._viewModel = viewModel;
        }
    }

}
