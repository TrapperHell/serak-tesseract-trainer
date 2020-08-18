using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace SerakTesseractTrainer
{
    public class TTProject
    {
        public TTProject(string projectPath, XmlDocument document)
        {
            if (document == null || String.IsNullOrWhiteSpace(projectPath) || !File.Exists(projectPath))
                throw new ArgumentException();

            this.Document = document;
            this.Location = projectPath;
        }

        public string Location { get; }

        public XmlDocument Document { get; }

        public string Name
        {
            get
            {
                return Path.GetFileNameWithoutExtension(Location);
            }
        }



        public static TTProject CreateNewProject(string fileName)
        {
            if (String.IsNullOrWhiteSpace(fileName))
                return null;

            XmlDocument xmlDoc = CreateProjectXml();
            xmlDoc.Save(fileName);

            CreateProjectFileStructure(Path.GetDirectoryName(fileName));

            return new TTProject(fileName, xmlDoc);
        }

        private static XmlDocument CreateProjectXml()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null));

            // Insert XML Elements
            XmlElement rootNode = xmlDoc.CreateElement("TrapTesseractTrainerProject");
            xmlDoc.AppendChild(rootNode);
            rootNode.AppendChild(xmlDoc.CreateElement("Images"));
            rootNode.AppendChild(xmlDoc.CreateElement("BoxFiles"));
            rootNode.AppendChild(xmlDoc.CreateElement("TessData"));

            return xmlDoc;
        }

        private static void CreateProjectFileStructure(string projectPath)
        {
            projectPath = Path.Combine(projectPath, "TrainData");

            if (!Directory.Exists(projectPath))
                Directory.CreateDirectory(projectPath);

            File.WriteAllText(Path.Combine(projectPath, "font_properties"), string.Empty);
        }

        public void CopyImageFile(string imagePath)
        {
            XmlElement element;
            XmlText xmltext;

            //Copy Image If it Does Not Exist in The project folder
            string newPath = Path.Combine(Location, Path.GetFileName(imagePath));
            if (!File.Exists(newPath))
            {
                File.Copy(imagePath, newPath);
                element = Document.CreateElement("Images");
                xmltext = Document.CreateTextNode(Path.GetFileName(imagePath));
                element.AppendChild(xmltext);
                Document.SelectSingleNode("Images").AppendChild(element);
                Document.Save(Location);
            }
            else
            {
                MessageBox.Show("ImageFile Already Exist", "File Exist", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            string boxPath = Path.Combine(Path.GetDirectoryName(imagePath), Path.GetFileNameWithoutExtension(imagePath), ".box");
            if (File.Exists(boxPath))
            {
                string newBoxPath = Path.Combine(Location, Path.GetFileNameWithoutExtension(newPath), ".box");
                if (!File.Exists(newBoxPath))
                {
                    File.Copy(boxPath, newBoxPath);
                    element = Document.CreateElement("BoxFiles");
                    xmltext = Document.CreateTextNode(Path.GetFileName(boxPath));
                    element.AppendChild(xmltext);
                    Document.SelectSingleNode("BoxFiles").AppendChild(element);
                    Document.Save(Location);
                }
                else
                {
                    MessageBox.Show("Box Files Already Exist");
                }
            }
            else
            {
                BoxCreator cr = new BoxCreator(imagePath);
                cr.Show();
            }
        }
    }
}
