using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SerakTesseractTrainer
{
    public partial class TesseractConfig : Form
    {
        public TesseractConfig()
        {
            InitializeComponent();

            this.Text = String.Format("{0} - Tesseract Configuration", Configuration.ApplicationName);
        }

        private void TesseractConfig_Load(object sender, EventArgs e)
        {
            txtTesseractPath.Text = Configuration.TesseractPath;
            txtIsoLang.Text = Configuration.IsoLang;
        }

        private void btnConfig_BrowseTesseractPath_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Tesseract Executables (*.exe)|*.exe";
                ofd.DefaultExt = Configuration.TesseractName;

                if (ofd.ShowDialog() == DialogResult.OK)
                    txtTesseractPath.Text = Path.GetDirectoryName(ofd.FileName);
            }
        }

        private void txtIsoLang_SetCaretPosition(object sender, EventArgs e)
        {
            txtIsoLang.SelectionStart = txtIsoLang.Text.Length;
            txtIsoLang.SelectionLength = 0;
        }

        private void btnConfig_Save_Click(object sender, EventArgs e)
        {
            string tessPath = txtTesseractPath.Text, isoLang = txtIsoLang.Text;
            TesseractConfigHelper.ValidateConfiguration(ref tessPath, ref isoLang);

            epMain.Clear();

            if (tessPath == null)
                epMain.SetError(txtTesseractPath, "Path is empty or invalid.");
            else if (isoLang == null)
                epMain.SetError(txtIsoLang, "Iso Language is empty, less than three letters or invalid.");
            else
            {
                string[] config = { tessPath, isoLang };
                TesseractConfigHelper.SaveConfigurationToFile(config);

                Configuration.TesseractPath = tessPath;
                Configuration.IsoLang = isoLang;

                this.Close();
            }
        }
    }

    public static class TesseractConfigHelper
    {
        /// <summary>
        /// Loads the application configuration from file.
        /// </summary>
        public static void LoadConfigurationFromFile()
        {
            string configPath = Path.Combine(Environment.CurrentDirectory, Configuration.ConfigFileName);

            if (File.Exists(configPath))
            {
                try
                {
                    string[] configuration = File.ReadAllLines(configPath);

                    if (configuration != null && configuration.Length >= 2)
                    {
                        string tessPath = configuration[0], isoLang = configuration[1];
                        ValidateConfiguration(ref tessPath, ref isoLang);

                        Configuration.TesseractPath = tessPath;
                        Configuration.IsoLang = isoLang;
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// Saves the application configuration to file.
        /// </summary>
        /// <param name="config">
        /// The application configuration to be saved.
        /// </param>
        public static void SaveConfigurationToFile(string[] config)
        {
            string configPath = Path.Combine(Environment.CurrentDirectory, Configuration.ConfigFileName);

            try
            {
                File.WriteAllLines(configPath, config);
            }
            catch { }
        }

        /// <summary>
        /// Validates the configuration. If any of the settings are invalid, these are
        /// set to null.
        /// </summary>
        public static void ValidateConfiguration(ref string tesseractPath, ref string isoLang)
        {
            if (String.IsNullOrWhiteSpace(tesseractPath) || !Directory.Exists(tesseractPath))
                tesseractPath = null;

            if (String.IsNullOrWhiteSpace(isoLang) || isoLang.Length != 3 || !isoLang.ToList().All(c => char.IsLetter(c)))
                isoLang = null;
        }
    }
}