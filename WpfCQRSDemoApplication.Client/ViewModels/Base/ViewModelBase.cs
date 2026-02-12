using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using WpfCQRSDemo.Infrastructure.CQRS;
using WpfCQRSDemo.Infrastructure.Services;
using WpfCQRSDemoApplication.Client.Infrastructure.SignalR;
using WpfCQRSDemoApplication.Shared.Contracts.Commands;

namespace WpfCQRSDemo.ViewModels.Base
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        protected ILogger Logger { get; }
        protected IErrorService ErrorService { get; }
        protected IDialogService DialogService { get; }
        protected INavigationService NavigationService { get; }
        protected ICommandQueryDispatcher Dispatcher { get; }
        protected ISignalRService SignalR { get; }

        protected ViewModelBase(IInfrastructureServices infrastructure)
        {
            if (infrastructure == null) throw new ArgumentNullException("infrastructure");
            Logger = infrastructure.Logger;
            ErrorService = infrastructure.ErrorService;
            DialogService = infrastructure.DialogService;
            NavigationService = infrastructure.NavigationService;
            Dispatcher = infrastructure.Dispatcher;
            SignalR = infrastructure.SignalR;
        }

        public bool IsBusy
        {
            get => field;
            set => SetProperty(ref field, value);
        }

        public string BusyMessage
        {
            get => field;
            set => SetProperty(ref field, value);
        }

        protected async Task ExecuteCommandAsync(ICommand command, string errorMessage = null)
        {
            try
            {
                IsBusy = true;
                await Dispatcher.ExecuteAsync(command);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, errorMessage ?? "Error executing " + command.GetType().Name);
                ErrorService.HandleError(ex);
                await DialogService.ShowMessageAsync(errorMessage ?? "An error occurred");
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected async Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command, string errorMessage = null)
        {
            try
            {
                IsBusy = true;
                return await Dispatcher.ExecuteAsync(command);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, errorMessage ?? "Error executing " + command.GetType().Name);
                ErrorService.HandleError(ex);
                await DialogService.ShowMessageAsync(errorMessage ?? "An error occurred");
                return default(TResult);
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected async Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query, string errorMessage = null, string busyMessage = null)
        {
            try
            {
                IsBusy = true;
                BusyMessage = busyMessage;
                return await Dispatcher.QueryAsync(query);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, errorMessage ?? "Error executing " + query.GetType().Name);
                ErrorService.HandleError(ex);
                await DialogService.ShowMessageAsync(errorMessage ?? "An error occurred");
                return default(TResult);
            }
            finally
            {
                IsBusy = false;
                BusyMessage = null;
            }
        }

        public virtual Task InitializeAsync()
        {
            return Task.FromResult(true);
        }

        public virtual Task OnNavigatedToAsync(object parameter)
        {
            return Task.FromResult(true);
        }

        public virtual Task OnNavigatedFromAsync()
        {
            return Task.FromResult(true);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
