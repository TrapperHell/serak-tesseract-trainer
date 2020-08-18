using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using SerakTesseractTrainer.Models;

namespace SerakTesseractTrainer
{
    public partial class BoxForm : Form
    {
        public BoxForm()
        {
            InitializeComponent();

            lbChars.DisplayMember = "Character";
            pbImage.ImageLocation = @"C:\Test.jpg";
        }

        private void pbImage_Layout(object sender, LayoutEventArgs e)
        {
            this.picBoxRelativeImgRect = (Rectangle)imageRectangleProperty.GetValue(pbImage, null);
        }

        Point? startPoint = null, endPoint = null;
        PropertyInfo imageRectangleProperty = typeof(PictureBox).GetProperty("ImageRectangle", BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Instance);
        Pen existentPen = new Pen(Brushes.Red, 2), drawPen = new Pen(Brushes.Black);

        private void pbImage_MouseDown(object sender, MouseEventArgs e)
        {
            Rectangle imgOffset = PicBoxRelativeImgRect;
            this.startPoint = new Point(e.Location.X - imgOffset.X, e.Location.Y - imgOffset.Y);
        }

        private void pbImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.startPoint.HasValue)
            {
                Rectangle imgOffset = PicBoxRelativeImgRect;
                this.endPoint = new Point(e.Location.X - imgOffset.X, e.Location.Y - imgOffset.Y);

                pbImage.Refresh();
            }
        }

        private void pbImage_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.startPoint.HasValue && this.endPoint.HasValue)
            {
                CharHolder ch = (lbChars.SelectedItem as CharHolder);
                ch.DrawRect = GetRectanglePosition(this.startPoint.Value, this.endPoint.Value);
                ch.ImageRectangle = PicBoxRelativeImgRect;

                this.startPoint = null;
                this.endPoint = null;

                pbImage.Refresh();
            }
        }

        private void pbImage_Paint(object sender, PaintEventArgs e)
        {
            foreach (var c in lbChars.Items)
            {
                CharHolder ch = c as CharHolder;
                if (ch.DrawRect.IsEmpty)
                    continue;

                Rectangle rect = ch.GetRelativePosition(PicBoxRelativeImgRect);
                Rectangle imgOffset = PicBoxRelativeImgRect;
                rect.X += imgOffset.X;
                rect.Y += imgOffset.Y;

                e.Graphics.DrawRectangle(this.existentPen, rect);
            }

            if (this.startPoint.HasValue && this.endPoint.HasValue)
            {
                Rectangle rect = GetRectanglePosition(this.startPoint.Value, this.endPoint.Value);
                Rectangle imgOffset = PicBoxRelativeImgRect;
                rect.X += imgOffset.X;
                rect.Y += imgOffset.Y;

                e.Graphics.DrawRectangle(this.drawPen, rect);
            }
        }

        private static Rectangle GetRectanglePosition(Point p1, Point p2)
        {
            Point smallestPoint = new Point(Math.Min(p1.X, p2.X), Math.Min(p1.Y, p2.Y));
            Point largestPoint = new Point(Math.Max(p1.X, p2.X), Math.Max(p1.Y, p2.Y));

            return new Rectangle(smallestPoint, new Size(largestPoint.X - smallestPoint.X, largestPoint.Y - smallestPoint.Y));
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            lbChars.Items.Add(new CharHolder());
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            lbChars.Items.Remove(lbChars.SelectedItem);

            pbImage.Refresh();
        }

        Rectangle picBoxRelativeImgRect;

        public Rectangle PicBoxRelativeImgRect
        {
            get { return picBoxRelativeImgRect; }
            set { picBoxRelativeImgRect = value; }
        }

        private void pbImage_LoadCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            this.picBoxRelativeImgRect = (Rectangle)imageRectangleProperty.GetValue(pbImage, null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CharHolder ch = lbChars.SelectedItem as CharHolder;

            Rectangle rect = ch.GetRelativePosition(new Rectangle(0, 0, pbImage.Image.Width, pbImage.Image.Height));
            //Rectangle imgOffset = PicBoxRelativeImgRect;
            //rect.X += imgOffset.X;
            //rect.Y += imgOffset.Y;

            MessageBox.Show(rect.ToString());
        }
    }
}