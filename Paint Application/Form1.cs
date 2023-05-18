using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{   public enum operation
    {
        pen,
        eraser,
        fill,
        line,
        rectangle,
        ellipse , 
        circle,
        text

    }
    public partial class Form1 : Form
    {
        Bitmap bitmap;
        Graphics graghic;
        Point P1, P2;

        Color penColor = Color.Black;
        int penSize = 1;
        Pen m_pen;
        
        Color EraserColor = Color.White;
        int EraserSize = 15;
        Pen m_eraser;

        operation clk;

        int frstX, frstY;
        int midX, midY;
        int lstX, lstY;

        ColorDialog colorDialog;
        FontDialog fontDialog;

        Color selectedColor;
        Color selectedErasingColor;

        
        Font txtFont;

        bool isDrawing = false;

        DashStyle PenStyle;


        public Form1()
        {
            InitializeComponent();
            
            this.Width = 950;
            this.Height = 650;

            bitmap=new Bitmap(picture.Width, picture.Height);
            graghic = Graphics.FromImage(bitmap);
            graghic.Clear(Color.White);
            picture.Image= bitmap;

            m_pen = new Pen(penColor, penSize);
            m_eraser= new Pen(EraserColor,EraserSize);

            colorDialog=new ColorDialog();
            fontDialog=new FontDialog();

            selectedColor = penColor;
            currentColor.BackColor = penColor;

            selectedErasingColor = EraserColor;
            currentErasingColor.BackColor = EraserColor;


            clk = operation.pen;

            txtFont = new Font("Times New Roman", 10, FontStyle.Bold);

            toolStripStatusLabel1.Text = "Shape : line ";
            toolStripStatusLabel3.Text = "size=1 ";

            PenStyle = DashStyle.Solid;
            m_pen.DashStyle = PenStyle;

        }


        private void picture_MouseDown(object sender, MouseEventArgs e)
        {
            isDrawing= true;

            P1 = e.Location;
            
            frstX = e.X;
            frstY = e.Y;

            if (clk == operation.text)
            {
                textBox1.Visible = true;
                this.ActiveControl = textBox1;

                graghic.DrawString(textBox1.Text, txtFont, new SolidBrush(selectedColor), frstX, frstY);
                textBox1.Text = "";

            }


        }


        private void picture_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {   
                switch (clk)
                {
                    case operation.pen:         
                        P2 = e.Location;
                        graghic.DrawLine(m_pen, P2, P1);
                        P1 = P2;
                        break;
                    case operation.eraser:         
                        P2 = e.Location;
                        graghic.DrawLine(m_eraser, P2, P1);
                        P1 = P2;
                        break;
                }
            }
            picture.Refresh();

            lstX = e.X;
            lstY = e.Y;

            midX = e.X - frstX;
            midY = e.Y - frstY;

            toolStripStatusLabel2.Text =  selectedColor.ToString();


        }


        private void picture_MouseUp(object sender, MouseEventArgs e)
        {
            isDrawing= false;

            midX = lstX - frstX;
            midY = lstY - frstY;
            switch (clk)
            {
                case operation.line:
                    graghic.DrawLine(m_pen,frstX,frstY,lstX,lstY);
                    toolStripStatusLabel1.Text = "shape : line";
                    break;
                case operation.rectangle:
                    graghic.DrawRectangle(m_pen,frstX,frstY,midX,midY);
                    toolStripStatusLabel1.Text = "shape : rectangle";
                    break;
                case operation.ellipse:
                    graghic.DrawEllipse(m_pen, frstX, frstY, midX, midY);
                    toolStripStatusLabel1.Text = "shape : Ellipse";

                    break;
                case operation.circle:
                    graghic.DrawEllipse(m_pen,frstX,frstY,midY,midY);
                    toolStripStatusLabel1.Text = "shape : Circle";
                    break;
                case operation.text:
                    picture.Cursor=Cursors.Default;
                    break;
                   
            }
          

        }

        private void picture_Paint(object sender, PaintEventArgs e)
        {
            Graphics grphc = e.Graphics;
            if (isDrawing)
            {
                switch (clk)
                {
                    case operation.line:
                        grphc.DrawLine(m_pen, frstX, frstY, lstX, lstY); break;
                    case operation.rectangle:
                        grphc.DrawRectangle(m_pen, frstX, frstY, midX, midY); break;
                    case operation.ellipse:
                        grphc.DrawEllipse(m_pen, frstX, frstY, midX, midY); break;
                    case operation.circle:
                        grphc.DrawEllipse(m_pen, frstX, frstY, midY, midY);break;
                    case operation.text:

                        break;

                }
               

            }
        }

        private void pen_Click(object sender, EventArgs e)
        {
            clk = operation.pen;
            m_pen = new Pen(selectedColor, 1);

           
        }

        private void drawingColor_Click(object sender, EventArgs e)
        {
            colorDialog.ShowDialog();
            selectedColor = colorDialog.Color;
            currentColor.BackColor= selectedColor;
            m_pen.Color = selectedColor;

        }

        private void erasingColor_Click(object sender, EventArgs e)
        {
            colorDialog.ShowDialog();
            selectedErasingColor = colorDialog.Color;
            currentErasingColor.BackColor = selectedErasingColor;
            m_eraser.Color = selectedErasingColor;
        }

        private void clear_Click(object sender, EventArgs e)
        {
            graghic.Clear(Color.White);
            picture.Image = bitmap;
            clk = operation.pen;
        }

        static Point map_Point_picbx_form(PictureBox pb,Point p)
        {
            float xRatio = 1f * pb.Image.Width / pb.Width;

            float yRatio = 1f* pb.Image.Height / pb.Height;

            return new Point((int)(p.X*xRatio),(int)(p.Y*yRatio));
        }

        private void colorPicker_MouseClick(object sender, MouseEventArgs e)
        {
            Point p = map_Point_picbx_form(colorPicker, e.Location);

            selectedColor = ((Bitmap)colorPicker.Image).GetPixel(p.X, p.Y);
            currentColor.BackColor = selectedColor;
            m_pen.Color = selectedColor; 
        }

        private void save_Click(object sender, EventArgs e)
        {
            SaveFileDialog savedFile= new SaveFileDialog();
            if (savedFile.ShowDialog() == DialogResult.OK)
            {
                Bitmap btmp = bitmap.Clone(new Rectangle(0, 0, picture.Width, picture.Height), bitmap.PixelFormat);
                btmp.Save(savedFile.FileName+".iti");
            }
        }
        private void fillPixel(Stack<Point> pt,Bitmap btmp , int x , int y , Color clrOld,Color clrNew)
        {
            Color currClr = btmp.GetPixel(x, y);
            if (currClr == clrOld)
            {
                pt.Push(new Point(x, y));
                btmp.SetPixel(x, y, clrNew);
            }
        }
        private void fillpixels(Bitmap btmp,int x,int y , Color clr)
        {
            Color oldclr=btmp.GetPixel(x, y);

            Stack<Point> pixel=new Stack<Point>();
            pixel.Push(new Point(x, y));    

            btmp.SetPixel(x, y, clr);

            if(oldclr==clr) { return; }

            while(pixel.Count > 0)
            {
                Point p = pixel.Pop();
                if(p.X>0 && p.Y>0 && p.X<btmp.Width-1 &&p.Y<btmp.Height-1) 
                {
                    fillPixel(pixel, btmp, p.X - 1, p.Y, oldclr, clr);
                    fillPixel(pixel, btmp, p.X , p.Y - 1, oldclr, clr);
                    fillPixel(pixel, btmp, p.X + 1, p.Y, oldclr, clr);
                    fillPixel(pixel, btmp, p.X , p.Y + 1, oldclr, clr);
                }
            }

        }

        private void fill_Click(object sender, EventArgs e)
        {
            clk = operation.fill;
        }

        private void picture_MouseClick(object sender, MouseEventArgs e)
        {
            if (clk==operation.fill)
            {
                Point pt = map_Point_picbx_form(picture, e.Location);
                fillpixels(bitmap, pt.X, pt.Y, selectedColor);
            }
        }

        private void circle_Click(object sender, EventArgs e)
        {
            clk = operation.circle;
        }

        private void brush_Click(object sender, EventArgs e)
        {
            clk = operation.pen;
            m_pen = new Pen(selectedColor,15);
            toolStripStatusLabel3.Text = "size=15";


        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {                
            m_pen =new Pen(selectedColor, float.Parse(comboBox1.SelectedItem.ToString()));
            toolStripStatusLabel3.Text = "Size=" + comboBox1.SelectedItem.ToString();
        }

        private void fntSizebx_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_pen = new Pen(selectedColor, float.Parse(fntSizebx.SelectedItem.ToString()));
            toolStripStatusLabel3.Text = "Size="+ fntSizebx.SelectedItem.ToString();


        }

        private void text_Click(object sender, EventArgs e)
        {
            clk = operation.text;
            picture.Cursor = Cursors.IBeam;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            clk = operation.pen;
            this.ActiveControl = picture;
            textBox1.Visible = false;

            

        }

        private void picture_Click(object sender, EventArgs e)
        {
            
        }

        private void txtStyleBtn_Click(object sender, EventArgs e)
        {
            fontDialog.ShowDialog();
            txtFont= fontDialog.Font;
            
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void styleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dialog dlg_Box = new Dialog();
            dlg_Box.color = selectedColor;
           DialogResult result= dlg_Box.ShowDialog();
            if (result == DialogResult.OK)
            {
                selectedColor = dlg_Box.color;
                currentColor.BackColor = selectedColor;
                m_pen.Color = selectedColor;
                

               if (dlg_Box.radio1 == true){
                    m_pen = new Pen(selectedColor, 1);
                        toolStripStatusLabel3.Text = "Size=1";
                        
                }
                else if (dlg_Box.radio2 == true) { m_pen = new Pen(selectedColor, 2);
                    toolStripStatusLabel3.Text = "Size=2";
                }
                else if (dlg_Box.radio3 == true) { m_pen = new Pen(selectedColor, 3);
                    toolStripStatusLabel3.Text = "Size=3";
                }
                else if (dlg_Box.radio4 == true) { m_pen = new Pen(selectedColor, 4);
                    toolStripStatusLabel3.Text = "Size=4";
                }
                else if (dlg_Box.radio5 == true) { m_pen = new Pen(selectedColor, 5);
                    toolStripStatusLabel3.Text = "Size=5";
                }
            }

        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            clk = operation.circle;

        }

        private void toolStripLabel2_Click(object sender, EventArgs e)
        {
            clk = operation.line;

        }

        private void toolStripLabel3_Click(object sender, EventArgs e)
        {
            clk = operation.rectangle;

        }

        private void toolStripLabel4_Click(object sender, EventArgs e)
        {
            clk = operation.pen;
        }

        private void toolStripLabel5_Click(object sender, EventArgs e)
        {
            m_pen = new Pen(selectedColor, 1);
            toolStripStatusLabel3.Text = "Size=1";

        }

        private void toolStripLabel6_Click(object sender, EventArgs e)
        {
            m_pen = new Pen(selectedColor, 2);
            toolStripStatusLabel3.Text = "Size=2";


        }

        private void toolStripLabel7_Click(object sender, EventArgs e)
        {
            m_pen = new Pen(selectedColor, 3);
            toolStripStatusLabel3.Text = "Size=3";

        }

        private void toolStripLabel8_Click(object sender, EventArgs e)
        {
            m_pen = new Pen(selectedColor, 4);
            toolStripStatusLabel3.Text = "Size=4";

        }

        private void toolStripLabel9_Click(object sender, EventArgs e)
        {
            m_pen = new Pen(selectedColor, 5);
            toolStripStatusLabel3.Text = "Size=5";


        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void solidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PenStyle = DashStyle.Solid;
            m_pen.DashStyle = PenStyle;
        }

        private void dotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PenStyle = DashStyle.Dot;
            m_pen.DashStyle = PenStyle;
        }

        private void dashToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PenStyle = DashStyle.Dash;
            m_pen.DashStyle = PenStyle;
        }

        private void eraser_Click(object sender, EventArgs e)
        {
            clk = operation.eraser;
        }

        private void line_Click(object sender, EventArgs e)
        {
            clk = operation.line;
        }
        private void rect_Click(object sender, EventArgs e)
        {
            clk = operation.rectangle;
        }
        private void ellipse_Click(object sender, EventArgs e)
        {
            clk = operation.ellipse;
        }


       

            
    }
}
