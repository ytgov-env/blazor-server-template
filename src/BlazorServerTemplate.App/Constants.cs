namespace BlazorServerTemplate.App;

public static class Constants
{
    public static class CascadingValues
    {
        public const string Example = "Example";
    }

    public static class FormatStrings
    {
        public const string StandardDateFormat = Shared.Constants.FormatStrings.StandardDateFormat;
        public const string StandardDateFormatWithoutYear = Shared
            .Constants
            .FormatStrings
            .StandardDateFormatWithoutYear;
    }

    public static class Routes
    {
        public const string HomePage = "/";

        public static string GetHomePageLink() => HomePage;
    }
}
