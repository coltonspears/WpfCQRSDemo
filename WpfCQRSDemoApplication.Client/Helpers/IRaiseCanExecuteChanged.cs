using System.Windows.Input;

namespace WpfCQRSDemo.Helpers
{
    public interface IRaiseCanExecuteChanged : ICommand
    {
        void RaiseCanExecuteChanged();
    }

    public static class CommandExtensions
    {
        public static void RaiseCanExecuteChanged(this ICommand command)
        {
            if (command is IRaiseCanExecuteChanged raisable)
            {
                raisable.RaiseCanExecuteChanged();
            }
        }
    }
}
