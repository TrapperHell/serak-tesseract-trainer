using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace SerakTesseractTrainer
{
    class TessMain
    {
        public static string projectFolder;
        private XmlDocument ProjXML = new XmlDocument();
        private XmlElement xmlimages;
        private XmlElement xmlboxFiles;
        public static ArrayList images = new ArrayList();

        public string openProject()
        {
            string projectName = null;

            OpenFileDialog openpro = new OpenFileDialog();
            openpro.Filter = "Tesseract ser Project(*.ser)|*.ser";
            if (openpro.ShowDialog() == DialogResult.OK)
            {
                projectName = openpro.FileName;

                string projectFile = openpro.FileName;
                string projectPath = projectFile.Substring(0, projectFile.LastIndexOf('\\'));
                projectFolder = projectPath + "\\TrainData";
                //Fonts.font_properties=File.ReadAllLines(projectFolder + @"\font_properties");
                ProjXML.Load(openpro.FileName);
                XmlNode tessimages = ProjXML.SelectSingleNode("TesseractProject/TessImages");
                XmlNode tessbox = ProjXML.SelectSingleNode("TesseractProject/TessBoxFiles");
                xmlimages = (XmlElement)tessimages;
                xmlboxFiles = (XmlElement)tessbox;
                XmlNodeList tempimagenode = tessimages.SelectNodes("Images");
                foreach (XmlNode item in tempimagenode)
                {
                    images.Add(item.InnerText);
                }
                //TODO:load dictionary if exists
            }

            return projectName;
        }
        public void setFontProperties()
        {
            string[] fonts;
            try
            {
                fonts = File.ReadAllLines(projectFolder + "\\" + @"font_properties");
            }
            catch (IOException)
            {
                MessageBox.Show("File not found");
            }
        }
        #region TesseractCMD Executor
        public void TrainTesseract()
        {
            foreach (var item in images)
            {
                if (File.Exists(projectFolder + '\\' + item.ToString().Substring(0, item.ToString().LastIndexOf('.')) + ".box"))
                {
                    TesseractExecutor.cmdExcute(Configuration.TesseractName, " " + item + " " + item.ToString().Substring(0, item.ToString().LastIndexOf('.')) + " nobatch box.train", projectFolder);
                }
                else
                {
                    MessageBox.Show("Box File is Missing", "Error Cannot continue excution");
                    return;
                }
            }
            string[] files = Directory.GetFiles(projectFolder);
            //Compute the Character Set  
            string boxfilesingleline = null;
            string trfilessigleline = null;
            foreach (string item in files)
            {
                if (item.EndsWith(".box"))
                {
                    boxfilesingleline += " " + item.Substring(item.LastIndexOf('\\') + 1) + " ";
                }
                if (item.EndsWith(".tr"))
                {
                    trfilessigleline += " " + item.Substring(item.LastIndexOf('\\') + 1) + " ";
                }
            }
            TesseractExecutor.cmdExcute("unicharset_extractor.exe", boxfilesingleline, projectFolder);
            //Clustering
            TesseractExecutor.cmdExcute("shapeclustering.exe", " -F font_properties -U unicharset " + trfilessigleline, projectFolder);
            TesseractExecutor.cmdExcute("mftraining.exe", " -F font_properties -U unicharset -O " + Configuration.IsoLang + ".unicharset " + trfilessigleline, projectFolder);
            TesseractExecutor.cmdExcute("cntraining.exe", trfilessigleline, projectFolder);
        }
        #endregion
        #region DawgfilecreationandMAnipulation

        public void savefreqwords(string[] freqlisttext)
        {
            File.WriteAllLines(projectFolder + @"\freq-dawg", freqlisttext, Encoding.UTF8);
        }

        public void createNewFreqWordList(string[] freqlisttext)
        {
            if (!File.Exists(projectFolder + @"\freq-dawg"))
            {
                File.WriteAllLines(projectFolder + @"\freq-dawg", freqlisttext, Encoding.UTF8);
            }
            else
            {
                DialogResult rs = MessageBox.Show("File Already Exist Do you Want To Replace It?", "File Exist", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
                if (rs == DialogResult.Yes)
                {
                    File.WriteAllLines(projectFolder + @"\freq-dawg", freqlisttext, Encoding.UTF8);
                }
            }
        }

        public void browsefreqwords(string freqlisttext)
        {
            if (!File.Exists(projectFolder + @"\freq-dawg"))
            {
                File.Copy(freqlisttext, projectFolder + @"\freq-dawg");
            }
            else
            {
                DialogResult rs = MessageBox.Show("File Already Exist Do you Want To Replace It?", "File Exist", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
                if (rs == DialogResult.Yes)
                {
                    File.Copy(freqlisttext, projectFolder + @"\freq-dawg", true);
                }
            }
        }

        public void browseDictionary(string wordlisttext)
        {
            if (!File.Exists(projectFolder + @"\word-list"))
            {
                File.Copy(wordlisttext, projectFolder + @"\word-list");
            }
            else
            {
                DialogResult rs = MessageBox.Show("File Already Exist Do you Want To Replace It?", "File Exist", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
                if (rs == DialogResult.Yes)
                {
                    File.Copy(wordlisttext, projectFolder + @"\word-list", true);
                }
            }
        }

        public void createNewDictionary(string[] rt)
        {
            if (!File.Exists(projectFolder + @"\word-list"))
            {
                File.WriteAllLines(projectFolder + @"\word-list", rt, Encoding.UTF8);
            }
            else
            {
                DialogResult rs = MessageBox.Show("File Already Exist Do you Want To Replace It?", "File Exist", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
                if (rs == DialogResult.Yes)
                {
                    File.WriteAllLines(projectFolder + @"\word-list", rt, Encoding.UTF8);
                }
            }
        }

        public void savewordlist(string[] wordlisttxt)
        {
            File.WriteAllLines(projectFolder + @"\word-list", wordlisttxt, Encoding.UTF8);
        }

        public void browseUnicharAmbig(string unichartext)
        {
            if (!File.Exists(projectFolder + @"\unambig-dawg"))
            {
                File.Copy(unichartext, projectFolder + @"\unambig-dawg");
            }
            else
            {
                DialogResult rs = MessageBox.Show("File Already Exist Do you Want To Replace It?", "File Exist", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
                if (rs == DialogResult.Yes)
                {
                    File.Copy(unichartext, projectFolder + @"\unambig-dawg", true);
                }
            }
        }

        internal void createNewUnicharambig(string[] unichartext)
        {
            if (!File.Exists(projectFolder + @"\unambig-dawg"))
            {
                File.WriteAllLines(projectFolder + @"\unambig-dawg", unichartext, Encoding.UTF8);
            }
            else
            {
                DialogResult rs = MessageBox.Show("File Already Exist Do you Want To Replace It?", "File Exist", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
                if (rs == DialogResult.Yes)
                {
                    File.WriteAllLines(projectFolder + @"\unambig-dawg", unichartext, Encoding.UTF8);
                }
            }
        }

        internal void saveUnicharAmbig(string[] unichartext)
        {
            File.WriteAllLines(projectFolder + @"\unambig-dawg", unichartext, Encoding.UTF8);
        }
        #endregion

        public void combineTessDatamethod()
        {
            if (!Directory.Exists(projectFolder + @"\Tessdata"))
            {
                Directory.CreateDirectory(projectFolder + @"\Tessdata");
            }
            if (File.Exists(projectFolder + @"\word-list"))
            {
                TesseractExecutor.cmdExcute("wordlist2dawg.exe", " word-list " + Configuration.IsoLang + ".word-dawg " + Configuration.IsoLang + ".unicharset ", projectFolder);
            }
            if (File.Exists(projectFolder + @"\unambig-dawg"))
            {
                TesseractExecutor.cmdExcute("wordlist2dawg.exe", " unambig-dawg " + Configuration.IsoLang + ".unicharambigs " + Configuration.IsoLang + ".unicharset ", projectFolder);
            }
            if (File.Exists(projectFolder + @"\freq-dawg"))
            {
                TesseractExecutor.cmdExcute("wordlist2dawg.exe", " freq-dawg " + Configuration.IsoLang + ".freq-dawg " + Configuration.IsoLang + ".unicharset ", projectFolder);
            }
            try
            {
                File.Copy(projectFolder + @"\inttemp", projectFolder + '\\' + Configuration.IsoLang + @".inttemp", true);
                File.Copy(projectFolder + @"\shapetable", projectFolder + '\\' + Configuration.IsoLang + @".shapetable", true);
                File.Copy(projectFolder + @"\normproto", projectFolder + '\\' + Configuration.IsoLang + @".normproto", true);
                File.Copy(projectFolder + @"\pffmtable", projectFolder + '\\' + Configuration.IsoLang + @".pffmtable", true);
                TesseractExecutor.cmdExcute("combine_tessdata.exe", " " + Configuration.IsoLang + ".", projectFolder);
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                File.Copy(projectFolder + '\\' + Configuration.IsoLang + ".traineddata", projectFolder + @"\Tessdata\" + Configuration.IsoLang + @".traineddata", true);
                MessageBox.Show("Creation of Tessdata is Succesfull", "Completed Succesfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        public string[] recognizeimage(string imagepath, string lang)
        {
            TesseractExecutor.cmdExcute(Configuration.TesseractName, " \"" + imagepath + "\" \"" + imagepath.Substring(0, imagepath.LastIndexOf('\\')) + "\\output\" -l " + lang, imagepath.Substring(0, imagepath.LastIndexOf('\\')));
            return (File.ReadAllLines(imagepath.Substring(0, imagepath.LastIndexOf('\\')) + "\\output.txt", Encoding.UTF8));
        }
        List<string> originalwordlist = new List<string>();
        List<string> recognizedwordlist = new List<string>();
        int totalwords;
        public bool BrowseRatingWord(string file, string[] text)
        {
            string[] originaltxt = File.ReadAllLines(file);
            foreach (string item in originaltxt)
            {
                foreach (string subitem in item.Split(' '))
                {
                    originalwordlist.Add(subitem);
                }
            }
            foreach (string item in text)
            {
                foreach (string substring in item.Split(' '))
                {
                    recognizedwordlist.Add(substring);
                }
            }
            if (originaltxt.Count<string>() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public float returnScore()
        {
            int correctwords = 0;
            IEnumerable<string> distrecognized = recognizedwordlist.Distinct<string>();
            IEnumerable<string> distoriginal = originalwordlist.Distinct<string>();
            totalwords = distoriginal.Count<string>(); //total number of words
            foreach (string item1 in distoriginal)
            {
                foreach (string item2 in distrecognized)
                {
                    if (item2 == item1)
                    {
                        correctwords++;
                    }
                }
            }
            float tempscore;    //Percent Must not Exced 100%
            if (correctwords <= totalwords)
            {
                tempscore = (float)correctwords / (float)totalwords * 100;
            }
            else
            {
                tempscore = (float)totalwords / (float)correctwords * 100;
            }
            double score = Math.Round(tempscore, 2);
            return (float)score;
        }

        public void removeItem(int p)
        {
            //TODO:
            string projectFile = null;

            XmlNode tempnode1 = ProjXML.SelectSingleNode("TesseractProject/TessImages");
            XmlNode tempnodebox = ProjXML.SelectSingleNode("TesseractProject/TessBoxFiles");
            XmlNodeList nodes = tempnode1.SelectNodes("Images");
            XmlNodeList nodesbox = tempnodebox.SelectNodes("BoxFiles");
            try
            {
                if (File.Exists(projectFolder + "\\" + images[p].ToString()))
                {
                    File.Delete(projectFolder + "\\" + images[p].ToString());
                    images.RemoveAt(p);
                    nodes[p].ParentNode.RemoveChild(nodes[p]);
                }
                if (File.Exists(projectFolder + "\\" + images[p].ToString().Substring(0, images[p].ToString().LastIndexOf('.')) + ".box"))
                {
                    File.Delete(projectFolder + "\\" + images[p].ToString().Substring(0, images[p].ToString().LastIndexOf('.')) + ".box");
                    nodesbox[p].ParentNode.RemoveChild(nodesbox[p]);
                }
                ProjXML.Save(projectFile);
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Error Has Occurred Make sure You Have Selected An item", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}
