using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using BMICalc.Services;

namespace BMICalc.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    // ── Backing fields ───────────────────────────────────────────────────────
    private bool   _isMetric     = true;
    private string _heightCm     = string.Empty;
    private string _weightKg     = string.Empty;
    private string _heightFt     = string.Empty;
    private string _heightIn     = string.Empty;
    private string _weightLbs    = string.Empty;
    private double _bmi;
    private string _category     = string.Empty;
    private string _categoryColor = "#F5B800";
    private bool   _hasResult;
    private string _errorMessage  = string.Empty;
    private bool   _hasError;

    // ── Properties ──────────────────────────────────────────────────────────

    public bool IsMetric
    {
        get => _isMetric;
        set
        {
            if (Set(ref _isMetric, value))
            {
                OnPropertyChanged(nameof(IsImperial));
                OnPropertyChanged(nameof(MetricButtonAlpha));
                OnPropertyChanged(nameof(ImperialButtonAlpha));
                // Clear result on unit switch
                HasResult = false;
                HasError  = false;
            }
        }
    }

    public bool IsImperial => !_isMetric;

    public double MetricButtonAlpha   => _isMetric ? 1.0 : 0.4;
    public double ImperialButtonAlpha => _isMetric ? 0.4 : 1.0;

    // Metric inputs
    public string HeightCm  { get => _heightCm;  set => Set(ref _heightCm,  value); }
    public string WeightKg  { get => _weightKg;  set => Set(ref _weightKg,  value); }

    // Imperial inputs
    public string HeightFt  { get => _heightFt;  set => Set(ref _heightFt,  value); }
    public string HeightIn  { get => _heightIn;  set => Set(ref _heightIn,  value); }
    public string WeightLbs { get => _weightLbs; set => Set(ref _weightLbs, value); }

    // Results
    public double Bmi           { get => _bmi;           private set => Set(ref _bmi,           value); }
    public string BmiDisplay    => _bmi.ToString("F1");
    public string Category      { get => _category;       private set => Set(ref _category,       value); }
    public string CategoryColor { get => _categoryColor;  private set => Set(ref _categoryColor,  value); }
    public bool   HasResult     { get => _hasResult;      private set => Set(ref _hasResult,      value); }

    // Error
    public string ErrorMessage  { get => _errorMessage;   private set => Set(ref _errorMessage,   value); }
    public bool   HasError      { get => _hasError;        private set => Set(ref _hasError,       value); }

    // ── Commands ─────────────────────────────────────────────────────────────
    public ICommand CalculateCommand   { get; }
    public ICommand SwitchMetricCommand   { get; }
    public ICommand SwitchImperialCommand { get; }

    public MainViewModel()
    {
        CalculateCommand      = new Command(OnCalculate);
        SwitchMetricCommand   = new Command(() => IsMetric = true);
        SwitchImperialCommand = new Command(() => IsMetric = false);
    }

    private void OnCalculate()
    {
        HasError  = false;
        HasResult = false;

        double weightKg;
        double heightM;

        if (_isMetric)
        {
            if (!double.TryParse(_weightKg, out double wKg) || wKg <= 0 ||
                !double.TryParse(_heightCm, out double hCm) || hCm <= 0)
            {
                ShowError("ENTER VALID HEIGHT + WEIGHT");
                return;
            }
            weightKg = wKg;
            heightM  = hCm / 100.0;
        }
        else
        {
            if (!double.TryParse(_weightLbs, out double wLbs) || wLbs <= 0 ||
                !double.TryParse(_heightFt,  out double hFt)  || hFt  <  0 ||
                !double.TryParse(_heightIn,  out double hIn)  || hIn  <  0)
            {
                ShowError("ENTER VALID HEIGHT + WEIGHT");
                return;
            }
            weightKg = BmiCalculator.LbsToKg(wLbs);
            heightM  = BmiCalculator.InchesToM(BmiCalculator.FeetInchesToInches(hFt, hIn));
        }

        if (heightM <= 0)
        {
            ShowError("HEIGHT CANNOT BE ZERO");
            return;
        }

        var bmi = BmiCalculator.Calculate(weightKg, heightM);
        Bmi           = bmi;
        OnPropertyChanged(nameof(BmiDisplay));
        Category      = BmiCalculator.GetLabel(bmi);
        CategoryColor = BmiCalculator.GetColorHex(bmi);
        HasResult = true;
    }

    private void ShowError(string message)
    {
        ErrorMessage = message;
        HasError     = true;
    }

    // ── INotifyPropertyChanged ───────────────────────────────────────────────
    public event PropertyChangedEventHandler? PropertyChanged;

    protected bool Set<T>(ref T field, T value, [CallerMemberName] string? name = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(name);
        return true;
    }

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
