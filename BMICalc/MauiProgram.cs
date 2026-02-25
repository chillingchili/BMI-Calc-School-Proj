using Microsoft.Extensions.Logging;

namespace BMICalc;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("BigShoulders-Black.ttf",      "BigShoulders-Black");
				fonts.AddFont("ChakraPetch-Regular.ttf",     "ChakraPetch-Regular");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
