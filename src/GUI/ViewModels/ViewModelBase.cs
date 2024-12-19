using System.ComponentModel;
using System.Runtime.CompilerServices;
using ReactiveUI;
namespace GUI.ViewModels;

public class ViewModelBase : ReactiveObject {
    // Add any common properties or methods for all ViewModels here
    private bool isBusy;
    private string errorMessage = string.Empty;

    public bool IsBusy {
        get => isBusy;
        protected set => this.RaiseAndSetIfChanged(ref isBusy, value);
    }

    public string ErrorMessage {
        get => errorMessage;
        protected set => this.RaiseAndSetIfChanged(ref errorMessage, value);
    }

    protected virtual void setError(string message)
        => ErrorMessage = message;

    protected virtual void clearError()
        => ErrorMessage = string.Empty;
}