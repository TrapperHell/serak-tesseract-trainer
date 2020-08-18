namespace SerakTesseractTrainer
{
    public static class Configuration
    {
        internal const string ApplicationName = "TrapperHell's Tesseract 3.x Trainer";
        internal const string TesseractName = "tesseract.exe";
        internal const string ConfigFileName = "Config.cfg";

        internal static string TesseractPath { get; set; }
        internal static string IsoLang { get; set; }
    }
}