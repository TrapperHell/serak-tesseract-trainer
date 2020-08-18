using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SerakTesseractTrainer
{
    public partial class MainForm : Form
    {
        TTProject project = null;

        TessMain ts = new TessMain();
        NewFontInteface fnt;

        public MainForm()
        {
            InitializeComponent();

            this.Text = Configuration.ApplicationName;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            TesseractConfigHelper.LoadConfigurationFromFile();

            txtisolang.Text = Configuration.IsoLang;
        }

        #region MenuStrip Items

        #region File

        private void tsmiNewProject_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "TrapperHell's Tesseract Trainer Project (*.ttp)| *.ttp";
                sfd.DefaultExt = "ttp";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    project = TTProject.CreateNewProject(sfd.FileName);

                    if (project != null && !String.IsNullOrEmpty(project.Location))
                        this.Text = String.Format("{0} - {1} ({2})", project.Name, Configuration.ApplicationName, project.Location);

                    ClearUI();
                }
            }
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        #endregion

        #region Tools

        private void tsmiTesseractOptions_Click(object sender, EventArgs e)
        {
            new TesseractConfig().ShowDialog();
        }

        private void tsmiOCRMode_Click(object sender, EventArgs e)
        {
            tbcMain.SelectedIndex = 2;
        }

        #endregion

        #region Help

        private void tsmiViewHelp_Click(object sender, EventArgs e)
        {
            // TODO
        }

        private void tsmiAbout_Click(object sender, EventArgs e)
        {
            new AboutBox().ShowDialog();
        }

        #endregion

        #endregion

        private void ClearUI()
        {
            lbImages.Items.Clear();
        }

        #region Train Tesseract

        private void btnAddTrainImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofdImages = new OpenFileDialog())
            {
                ofdImages.Filter = "Images|*.bmp;*.gif;*.jpeg;*.jpg;*.png;*.tif;*.tiff";
                ofdImages.Multiselect = true;

                if (ofdImages.ShowDialog() == DialogResult.OK)
                {
                    lbImages.Items.Clear();

                    string[] imagePaths = ofdImages.FileNames;

                    foreach (string imagePath in imagePaths)
                    {
                        project.CopyImageFile(imagePath);

                        lbImages.Items.Add(Path.GetFileName(imagePath));
                    }
                }
            }
        }

        #endregion

        private void openPorjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string projectName = ts.openProject();

            if (String.IsNullOrEmpty(projectName))
                this.Text = String.Format("{0} - {1}", Configuration.ApplicationName, projectName);

            lbImages.Items.Clear();
            foreach (var item in TessMain.images)
            {
                lbImages.Items.Add(item.ToString());
            }

            Thread t = new Thread(Loaddictionaries);
            t.Priority = ThreadPriority.Lowest;
            t.Start();
        }
        #region Loading file Thread
        private void Loaddictionaries(object obj)
        {
            if (File.Exists(TessMain.projectFolder + @"\word-list"))
            {
                txtDictionary.Lines = File.ReadAllLines(TessMain.projectFolder + @"\word-list");
                txtDictionary.Enabled = true;
                btnSaveDictionary.Enabled = false;
            }
            if (File.Exists(TessMain.projectFolder + @"\freq-dawg"))
            {
                txtfreqwods.Lines = File.ReadAllLines(TessMain.projectFolder + @"\freq-dawg");
                txtfreqwods.Enabled = true;
                btncreateNew.Enabled = false;
            }
            if (File.Exists(TessMain.projectFolder + @"\unambig-dawg"))
            {
                txtunicharambig.Lines = File.ReadAllLines(TessMain.projectFolder + @"\unambig-dawg");
                txtunicharambig.Enabled = true;
                btnsaveunichar.Enabled = false;
            }
        }
        #endregion
        private void defineFontProperties(object sender, EventArgs e)
        {
            //fontwind = new Fonts();
            //fontwind.Show();
            fnt = new NewFontInteface();
            fnt.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ts.TrainTesseract();
            MessageBox.Show("Training Completed", "Training Hopefully Completed Succesfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }



        private void btnSave_Click(object sender, EventArgs e)
        {
            ts.savefreqwords(txtfreqwods.Lines);
            MessageBox.Show("Saved Succesfully", "Operation Succesful", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btncreateNew_Click(object sender, EventArgs e)
        {
            ts.createNewFreqWordList(txtfreqwods.Lines);
            btnBrowse.Enabled = false;
            txtfreqwods.Enabled = true;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "Text Files(*.txt)|*.txt";
            if (file.ShowDialog() == DialogResult.OK)
            {
                txtfreqwods.Lines = File.ReadAllLines(file.FileName, Encoding.UTF8);
                ts.browsefreqwords(file.FileName);
                txtfreqwods.Enabled = true;
            }

        }

        private void browseDictionary(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "Text Files(*.txt)|*.txt";
            if (file.ShowDialog() == DialogResult.OK)
            {
                txtDictionary.Lines = File.ReadAllLines(file.FileName, Encoding.UTF8);
                ts.browseDictionary(file.FileName);
                txtDictionary.Enabled = true;
            }

        }

        private void btnCreateNewDiction_Click(object sender, EventArgs e)
        {
            ts.createNewDictionary(txtDictionary.Lines);
            btnbrouseDictionary.Enabled = false;
            txtDictionary.Enabled = true;
        }

        private void btnSaveDictionary_Click(object sender, EventArgs e)
        {
            ts.savewordlist(txtDictionary.Lines);
            MessageBox.Show("Saved Succesfully", "Operation Succesful", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnbrwseunichar_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "Text Files(*.txt)|*.txt";
            if (file.ShowDialog() == DialogResult.OK)
            {
                txtunicharambig.Lines = File.ReadAllLines(file.FileName, Encoding.UTF8);
                ts.browseUnicharAmbig(file.FileName);
                txtunicharambig.Enabled = true;
            }

        }

        private void btncreateunichar_Click(object sender, EventArgs e)
        {
            ts.createNewUnicharambig(txtunicharambig.Lines);
            btnbrwseunichar.Enabled = false;
            txtunicharambig.Enabled = true;
        }

        private void btnsaveunichar_Click(object sender, EventArgs e)
        {
            ts.saveUnicharAmbig(txtunicharambig.Lines);
            MessageBox.Show("Saved Succesfully", "Operation Succesful", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void combineTessdata(object sender, EventArgs e)
        {
            ts.combineTessDatamethod();
        }

        private void browseforRecognition(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "Image Files(*.jpg,*.png,*.tiff,*.tif,*.bmp)|*.jpg;*.png;*.tiff;*.tif;*.bmp";
            if (file.ShowDialog() == DialogResult.OK)
            {
                txtLocation.Text = file.FileName;
            }
        }

        private void recognize(object sender, EventArgs e)
        {
            if (txtLocation.Text != null && txtisolang.Text != null)
            {
                string[] words = ts.recognizeimage(txtLocation.Text, txtisolang.Text);
                txtRecognizedWord.Lines = words;
                if (File.Exists(txtLocation.Text.Substring(0, txtLocation.Text.LastIndexOf('\\')) + @"\output.txt"))
                {
                    //trash temporary file;
                    File.Delete(txtLocation.Text.Substring(0, txtLocation.Text.LastIndexOf('\\')) + @"\output.txt");
                }
                btnBrowseToComp.Enabled = true;
                lblPercent.Enabled = true;
                progPercent.Enabled = true;
            }

        }

        private void button8_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "Text File(*.txt)|*.txt";
            if (file.ShowDialog() == DialogResult.OK)
            {
                bool fileloadedSuccesfully = ts.BrowseRatingWord(file.FileName, txtRecognizedWord.Lines);
                if (fileloadedSuccesfully)
                {
                    btnRate.Enabled = true;
                }
            }
        }
        float score;
        float prog;
        private void btnRate_Click(object sender, EventArgs e)
        {
            prog = 0.0f;
            score = ts.returnScore();
            timer1.Enabled = true;
            timer1.Start();
            timer1.Tick += timer1_Tick;     //A little bit of animation wont kill us !!! lol
        }
        void timer1_Tick(object sender, EventArgs e)
        {
            prog = (float)Math.Round(prog += 0.3f, 2);
            progPercent.Value = (int)prog;
            lblPercent.Text = prog + "%";
            if (prog >= score)
            {
                timer1.Stop();
                lblPercent.Text = score + "%";
                progPercent.Value = (int)score;
            }
        }

        private void removeNode(object sender, EventArgs e)
        {
            DialogResult rs = MessageBox.Show("Are You sure You Want To Remove This Item?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (rs == DialogResult.Yes)
            {
                ts.removeItem(lbImages.SelectedIndex);
                lbImages.Items.Clear();
                foreach (string item in TessMain.images)
                {
                    lbImages.Items.Add(item);
                }
            }
        }
    }
}