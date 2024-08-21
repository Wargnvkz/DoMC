using DoMCLib.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace DoMCInterface
{
    public partial class ShowFrameForm : Form
    {
        //WClasses.DrawNAxis.Draws DrawMain;
        //WClasses.DrawNAxis.Draws DrawSpectrum;

        public short[,] Image;
        public Bitmap bmp;
        ToolTip tt = new ToolTip();

        public ShowFrameForm()
        {
            InitializeComponent();
        }

        public new void Show()
        {
            //Image = DoMC.Tools.ImageTools.BaseFilter(Image,87);
            /*Image = DoMC.Tools.ImageTools.BaseFilter(Image, 93, 0.5);
            Image = DoMC.Tools.ImageTools.BaseFilter(Image, 41, 0.5);
            Image = DoMC.Tools.ImageTools.BaseFilter(Image, 19, 0.5);
            Image = DoMC.Tools.ImageTools.BaseFilter(Image, 20, 0.5);
            Image = DoMC.Tools.ImageTools.BaseFilter(Image, 44, 0.5);
            Image = DoMC.Tools.ImageTools.BaseFilter(Image, 22, 0.5);*/
            bmp = DrawImg(Image);
            var dev = ImageTools.CalculateDeviationFull(new short[][,] { Image }, new Rectangle(0, 0, 511, 511));
            lblDevAvgText.Text = dev.TotalAverage.ToString();
            lblDevDevText.Text = dev.TotalDeviation.ToString();
            lblPercentText.Text = (dev.TotalDeviation / dev.TotalAverage * 100).ToString("F3") + "%";
            base.Show();
        }
        public new void ShowDialog()
        {
            bmp = DrawImg(Image);
            base.ShowDialog();
        }

        public Bitmap DrawImg(short[,] Image)
        {
            if (Image == null) return null;
            Bitmap bmp = new Bitmap(512, 512);

            var lineimg = Image.Cast<short>();
            var max = lineimg.Max(l => l);
            var min = lineimg.Min(l => (short)l);
            if (max == min) if (min > 0) min = (short)(min - 1); else { max = (short)(max + 1); }

            if (min > 0) min = 0;
            max = (short)Math.Max(max, -min);
            min = max;
            for (int x = 0; x < 512; x++)
            {
                for (int y = 0; y < 512; y++)
                {
                    var v = Image[y, x];
                    var pv = v * 255 / max;
                    var nv = (-v) * 255 / min;
                    byte r = 0;
                    byte g = 0;
                    byte b = 0;
                    if (v >= 0)
                    {
                        r = (byte)pv;
                        g = r;
                        b = r;
                    }
                    else
                    {
                        r = 0;
                        g = 0;
                        b = (byte)((-v) * 255 / min);
                    }
                    //var sb = new SolidBrush(Color.FromArgb(r, g, b));
                    bmp.SetPixel(x, y, Color.FromArgb(r, g, b));
                    //g.FillRectangle(sb, x, y, 1, 1);
                }
            }
            return bmp;
        }

        private void DrawGraphs()
        {
            if (cbFullMax.Checked)
            {
                GraphsMax();
            }
            else
            {
                Graphs();
            }
        }

        private void DrawGraphLine(short[] line)
        {
            {
                chMain.ChartAreas.Clear();
                var ca = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
                ca.AxisX.Minimum = 0;
                ca.AxisX.Interval = 32;
                ca.AxisX.Maximum = 512;
                chMain.ChartAreas.Add(ca);
                chMain.Series.Clear();
                var ns = new System.Windows.Forms.DataVisualization.Charting.Series();
                ns.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                ns.Points.Clear();
                for (int i = 0; i < 512; i++)
                    ns.Points.AddXY(i, line[i]);
                chMain.Series.Add(ns);
            }
            {
                chFreq.ChartAreas.Clear();
                var ca = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
                ca.AxisX.Minimum = 0;
                ca.AxisX.Interval = 16;
                ca.AxisX.Maximum = 256;
                chFreq.ChartAreas.Add(ca);
                var freq = DoMCLib.Tools.ImageTools.Furje(line);
                chFreq.Series.Clear();
                var ns = new System.Windows.Forms.DataVisualization.Charting.Series();
                ns.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                ns.Points.Clear();
                for (int i = 5; i < 256; i++)
                    ns.Points.AddXY(i, freq[i].Magnitude);
                chFreq.Series.Add(ns);
                chFreq.Update();
            }
        }

        private void Graphs()
        {
            //GraphInit(maxY, minY, g);
            if (Image != null)
            {
                int frame = (int)numFrame.Value;
                short[] line = new short[512];
                if (cbVertical.Checked)
                {
                    for (int x = 0; x < 512; x++) line[x] = Image[x, frame];
                }
                else
                {
                    for (int x = 0; x < 512; x++) line[x] = Image[frame, x];
                }
                DrawGraphLine(line);
            }

            //DrawNew();
        }

        private void GraphsMax()
        {
            //GraphInit(maxY, minY, g);
            if (Image != null)
            {
                short[] line = new short[512];
                for (int y = 0; y < 512; y++) for (int x = 0; x < 512; x++) if (line[x] < Image[y, x]) line[x] = Image[y, x]; ;

                DrawGraphLine(line);
            }

            //DrawNew();
        }
        private void SpectrumReDraw(double maxY, double minY, Graphics g)
        {
            //PrepareDrawSpectrumData(g);
            //DrawSpectrum.Flash(g);

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
        }


        private void pbImg_Paint(object sender, PaintEventArgs e)
        {
            if (bmp == null) return;
            e.Graphics.DrawImage(bmp, 0, 0);
            var lineN = (int)numFrame.Value;
            if (cbVertical.Checked)
            {
                e.Graphics.DrawLine(new Pen(Color.Red), lineN, 0, lineN, 511);

            }
            else
            {
                e.Graphics.DrawLine(new Pen(Color.Red), 0, lineN, 511, lineN);
            }

        }

        private void numFrame_ValueChanged(object sender, EventArgs e)
        {
            //pbMain.Invalidate();
            // pbSpectrum.Invalidate();
            DrawGraphs();
            pbImg.Invalidate();
        }

        private void cbVertical_CheckedChanged(object sender, EventArgs e)
        {
            //pbMain.Invalidate();
            //pbSpectrum.Invalidate();
            DrawGraphs();
            pbImg.Invalidate();
        }

        private void pbMain_Resize(object sender, EventArgs e)
        {
            //pbMain.Invalidate();
        }

        private void pbImg_MouseUp(object sender, MouseEventArgs e)
        {
            if (cbVertical.Checked)
            {
                numFrame.Value = e.X;
            }
            else
            {
                numFrame.Value = e.Y;
            }
        }

        private void pbSpectrum_Paint(object sender, PaintEventArgs e)
        {
            if (Image == null) return;
            var lineimg = Image.Cast<short>();
            var max = lineimg.Max(l => (short)l);
            var min = lineimg.Min(l => (short)l);
            if (max == min) min = (short)(min - 1);
            SpectrumReDraw(max, min, e.Graphics);

        }

        private void pbSpectrum_Resize(object sender, EventArgs e)
        {
            //pbSpectrum.Invalidate();
        }

        private void ShowFrameForm_Load(object sender, EventArgs e)
        {
            DrawGraphs();
        }

        Point? prevPosition = null;
        ToolTip tooltip = new ToolTip();
        private void chMain_MouseMove(object sender, MouseEventArgs e)
        {
            var chart = sender as System.Windows.Forms.DataVisualization.Charting.Chart;
            if (chart == null) return;
            var pos = e.Location;
            if (prevPosition.HasValue && pos == prevPosition.Value)
                return;
            tooltip.RemoveAll();
            prevPosition = pos;
            var results = chart.HitTest(pos.X, pos.Y, false,
                                            System.Windows.Forms.DataVisualization.Charting.ChartElementType.DataPoint);
            foreach (var result in results)
            {
                if (result.ChartElementType == System.Windows.Forms.DataVisualization.Charting.ChartElementType.DataPoint)
                {
                    var prop = result.Object as System.Windows.Forms.DataVisualization.Charting.DataPoint;
                    if (prop != null)
                    {
                        var pointXPixel = result.ChartArea.AxisX.ValueToPixelPosition(prop.XValue);
                        var pointYPixel = result.ChartArea.AxisY.ValueToPixelPosition(prop.YValues[0]);

                        // check if the cursor is really close to the point (2 pixels around the point)
                        if (Math.Abs(pos.X - pointXPixel) < 2 &&
                            Math.Abs(pos.Y - pointYPixel) < 2)
                        {
                            tooltip.Show("X=" + prop.XValue + ", Y=" + prop.YValues[0], chart,
                                            pos.X, pos.Y - 15);
                        }
                    }
                }
            }
        }


        private void cbFullMax_CheckedChanged(object sender, EventArgs e)
        {
            if (cbFullMax.Checked)
            {
                numFrame.Enabled = false;
                cbVertical.Enabled = false;
            }
            else
            {
                numFrame.Enabled = true;
                cbVertical.Enabled = true;

            }
            DrawGraphs();
        }

        private void cmsiSaveFile_Click(object sender, EventArgs e)
        {
            var sd = new SaveFileDialog();
            if (sd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (var file = new FileStream(sd.FileName, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        var arr = ImageTools.ImageToArray(Image);

                        file.Write(arr, 0, arr.Length);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void cmsiSaveCompressedFile_Click(object sender, EventArgs e)
        {
            var sd = new SaveFileDialog();
            if (sd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (var file = new FileStream(sd.FileName, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        var arr = ImageTools.ImageToArray(Image);
                        var compressed = ImageTools.Compress(arr);
                        var str = ImageTools.ToBase64(compressed);
                        var tosave = Encoding.ASCII.GetBytes(str);
                        file.Write(tosave, 0, tosave.Length);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }


}
