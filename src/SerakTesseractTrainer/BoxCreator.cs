using System;
using System.Windows.Forms;

namespace SerakTesseractTrainer
{
    public partial class BoxCreator : Form
    {
        string projectFolder = TessMain.projectFolder;
        string filename;
        public BoxCreator(string file)
        {
            InitializeComponent();
            this.filename = file.Substring(file.LastIndexOf('\\') + 1); ;
        }

        private void createnewbox(object sender, EventArgs e)
        {
            TesseractExecutor.cmdExcute(Configuration.TesseractName, " " + filename + " " + filename.Substring(0, filename.LastIndexOf('.')) + " batch.nochop makebox", projectFolder);
        }

        private void bootstrapnewchar(object sender, EventArgs e)
        {
            TesseractExecutor.cmdExcute(Configuration.TesseractName, " " + filename + " " + filename.Substring(0, filename.LastIndexOf('.')) + " -l " + Configuration.IsoLang + " batch.nochop makebox", projectFolder);
        }
    }
}