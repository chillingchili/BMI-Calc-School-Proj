namespace BMICalc.Services;

public static class BmiCalculator
{
    /// <summary>
    /// Computes BMI from weight in kg and height in metres.
    /// </summary>
    public static double Calculate(double weightKg, double heightM)
    {
        if (heightM <= 0) throw new ArgumentOutOfRangeException(nameof(heightM));
        return weightKg / (heightM * heightM);
    }

    /// <summary>
    /// Returns the WHO category and a colour hex string for a given BMI.
    /// </summary>
    public static (string Label, string ColorHex) GetCategory(double bmi) => bmi switch
    {
        < 18.5 => ("UNDERWEIGHT",   "#5BC8FF"),   // cool blue
        < 25.0 => ("NORMAL",        "#F5B800"),   // amber
        < 30.0 => ("OVERWEIGHT",    "#FF8C42"),   // orange
        < 35.0 => ("OBESE CLASS I", "#FF4C4C"),   // red
        _      => ("OBESE CLASS II+","#C0392B")   // deep red
    };

    /// <summary>Gets only the category label.</summary>
    public static string GetLabel(double bmi) => GetCategory(bmi).Label;

    /// <summary>Gets only the category colour hex.</summary>
    public static string GetColorHex(double bmi) => GetCategory(bmi).ColorHex;

    // ── Unit conversion helpers ──────────────────────────────────────────────

    /// <summary>Converts pounds to kilograms.</summary>
    public static double LbsToKg(double lbs) => lbs * 0.45359237;

    /// <summary>Converts total inches to metres.</summary>
    public static double InchesToM(double inches) => inches * 0.0254;

    /// <summary>Converts feet + remaining inches to total inches.</summary>
    public static double FeetInchesToInches(double feet, double inches) => feet * 12 + inches;
}
