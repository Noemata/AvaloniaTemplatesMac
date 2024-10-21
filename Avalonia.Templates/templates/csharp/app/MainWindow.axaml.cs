using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AvaloniaAppTemplate;

public partial class MainWindow : Window, INotifyPropertyChanged
{
    int _count = 0;

    static string _osInfo = RuntimeInformation.OSDescription;
    public string OSInfo => _osInfo;

    string _message = "Hello world!";
    public string Message
    {
        get => _message;
        set => Set(ref _message, value);
    }

    public ICommand DoIt => new RelayCommand(It);

    public MainWindow() => InitializeComponent();

    public void OnButtonClicked(object sender, RoutedEventArgs args) => Message = $"Hi number {++_count}!";

    public void It() => Message = "It says hello.";

    #region PropertyChanged
    public new event PropertyChangedEventHandler? PropertyChanged;

    private void Set<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(storage, value)) return;
        storage = value;
        OnPropertyChanged(propertyName);
    }

    protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(storage, value)) return false;
        storage = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    #endregion
}

#region RelayCommand
public class RelayCommand : ICommand
{
    private readonly Func<bool>? _canExecute;
    private readonly Action _execute;
    public event EventHandler? CanExecuteChanged;

    public RelayCommand(Action execute) : this(execute, null) { }

    public RelayCommand(Action execute, Func<bool>? canExecute)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public bool CanExecute(object? parameter) => _canExecute == null || _canExecute();
    public void Execute(object? parameter) => _execute();
    public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}

public class RelayCommand<T> : ICommand
{
    private readonly Func<bool>? _canExecute;
    private readonly Action<T> _execute;
    public event EventHandler? CanExecuteChanged;

    public RelayCommand(Action<T> execute)
        : this(execute, null)
    {
    }

    public RelayCommand(Action<T> execute, Func<bool>? canExecute)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public bool CanExecute(object? parameter) => _canExecute == null || _canExecute();
    public void Execute(object? parameter) => _execute((T)parameter!);
    public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}
#endregion