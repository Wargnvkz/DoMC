using DoMCLib.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DoMCLib.Forms
{
    public partial class ShowPreformImages : Form
    {
        short[,] OriginalImage;
        short[,] OriginalStandardImage;
        short[,] ImageToDraw;
        short[,] StandardImageToDraw;

        //short[,] DiffImage;

        Bitmap TestBmpReadImage;
        Bitmap TestBmpDiffImage;
        Bitmap TestBmpStandardImage;

        private Rectangle MainRect = new Rectangle(0, 0, 511, 511);
        Rectangle FullRect = new Rectangle(0, 0, 511, 511);

        bool invertColors = false;
        bool ShowCheckArea = true;
        Rectangle CheckArea = new Rectangle(0, 0, 511, 511);

        Stopwatch timer = new Stopwatch();

        private DoMCLib.Classes.ImageProcessResult ImageProcessResult;
        private DoMCLib.Configuration.ImageProcessParameters ImageProcessParameters;

        DefectMethod CalculationMethod = DefectMethod.PlainDifference;

        public ShowPreformImages()
        {
            InitializeComponent();
            timer.Start();

            Prepare();
        }
        public void Prepare()
        {
            var screen = Screen.FromControl(this);

            var sheight = screen.WorkingArea.Height - SystemInformation.CaptionHeight;
            var indent = 10;
            var picWidth = 512;
            var picHeight = 512;
            var ChartHeight = 300;
            pbTestReadImage.Location = new Point(indent, pbTestReadImage.Location.Y);
            pbTestReadImage.Size = new Size(picWidth, picHeight);

            pbTestDifference.Location = new Point(picWidth + indent * 2, pbTestDifference.Location.Y);
            pbTestDifference.Size = new Size(picWidth, picHeight);

            pbTestStandard.Location = new Point(picWidth * 2 + indent * 3, pbTestStandard.Location.Y);
            pbTestStandard.Size = new Size(picWidth, picHeight);


            //var lblTestRead = new Label();
            //this.Controls.Add(lblTestRead);
            lblTestRead.AutoSize = true;
            lblTestRead.Location = new Point(pbTestReadImage.Location.X, pbTestReadImage.Location.Y - 25);
            lblTestRead.Text = "Прочитанное изображение:";

            // var lblTestStandard = new Label();
            //this.Controls.Add(lblTestStandard);
            lblTestStandard.AutoSize = true;
            lblTestStandard.Location = new Point(pbTestStandard.Location.X, pbTestStandard.Location.Y - 25);
            lblTestStandard.Text = "Эталон:";

            //var lblTestDifference = new Label();
            //this.Controls.Add(lblTestDifference);
            lblTestDifference.AutoSize = true;
            lblTestDifference.Location = new Point(pbTestDifference.Location.X, pbTestDifference.Location.Y - 25);
            lblTestDifference.Text = "Разница:";

            Application.DoEvents();
            //chTestReadLine.Location = new Point(pnlTestSockets.Location.X, pnlTestSockets.Location.Y + pnlTestSockets.Size.Height + 10);
            //chTestReadLine.Size = new Size(swidth - chTestReadLine.Location.X - 10, sheight - chTestReadLine.Location.Y - 20);

            chTestReadLine.Location = new Point(pbTestReadImage.Location.X, pbTestReadImage.Location.Y + pbTestReadImage.Size.Height + indent);
            //chTestReadLine.Size = new Size(picWidth, sheight - chTestReadLine.Location.Y - 20);
            chTestReadLine.Size = new Size(picWidth, ChartHeight);

            chTestDiff.Location = new Point(pbTestDifference.Location.X, pbTestReadImage.Location.Y + pbTestReadImage.Size.Height + indent);
            //chTestDiff.Size = new Size(picWidth, sheight - chTestReadLine.Location.Y - 20);
            chTestDiff.Size = new Size(picWidth, ChartHeight);

            chTestStandard.Location = new Point(pbTestStandard.Location.X, pbTestReadImage.Location.Y + pbTestReadImage.Size.Height + indent);
            //chTestStandard.Size = new Size(picWidth, sheight - chTestReadLine.Location.Y - 20);
            chTestStandard.Size = new Size(picWidth, ChartHeight);



        }
        public new void Show()
        {
            CalcImages();
            RedrawImages();
            base.Show();

        }
        public void RecalcAndRedrawImages()
        {
            CalcImages();
            RedrawImages();
        }

        private void CalcImages()
        {
            var start = timer.ElapsedTicks;
            bool isSocketGood = true;
            if (OriginalImage != null && OriginalStandardImage != null)
            //if (ImageToDraw != null && StandardImageToDraw != null)
            {
                ImageToDraw = OriginalImage.Clone() as short[,];
                StandardImageToDraw = OriginalStandardImage.Clone() as short[,];

                var ImageAvg = DoMCLib.Tools.ImageTools.Average(ImageToDraw, ImageProcessParameters.GetRectangle());
                lblAverage.Text = ImageAvg.ToString();
                switch (CalculationMethod)
                {
                    case DefectMethod.CheckBySettings:
                        {
                            ImageProcessResult = DoMCLib.Tools.ImageTools.CheckIfSocketGood(ImageToDraw, StandardImageToDraw, ImageProcessParameters);

                            isSocketGood = ImageProcessResult.IsSocketGood;
                            if (isSocketGood)
                            {
                                lblResultResult.BackColor = Color.Green;
                            }
                            else
                            {
                                lblResultResult.BackColor = Color.Red;

                            }
                            var ErrStr = ImageProcessResult.ErrorToString();
                            lblResultResult.Text = isSocketGood ? "✓" : ErrStr;// "X";
                        }
                        break;
                    case DefectMethod.PlainDifference:
                        {
                            ImageProcessResult = DoMCLib.Tools.ImageTools.CheckIfSocketGood(ImageToDraw, StandardImageToDraw, ImageProcessParameters);

                            isSocketGood = ImageProcessResult.IsSocketGood;
                            if (isSocketGood)
                            {
                                lblResultResult.BackColor = Color.Green;
                            }
                            else
                            {
                                lblResultResult.BackColor = Color.Red;

                            }
                            var ErrStr = ImageProcessResult.ErrorToString();
                            lblResultResult.Text = isSocketGood ? "✓" : ErrStr;// "X";

                            ImageProcessResult = new Classes.ImageProcessResult();
                            ImageProcessResult.ResultImage = DoMCLib.Tools.ImageTools.GetDifference(StandardImageToDraw, ImageToDraw);
                        }
                        break;
                    case DefectMethod.Normalize:
                        {
                            var stat = ImageTools.CalculateDeviationFull(new short[][,] { StandardImageToDraw });
                            var k = 25000d / stat.Max[0];
                            var std = ImageTools.Multiply(StandardImageToDraw, k);
                            var img = ImageTools.Multiply(ImageToDraw, k);
                            ImageProcessResult.ResultImage = DoMCLib.Tools.ImageTools.GetDifference(std, img);
                        }
                        break;
                    case DefectMethod.NormalizedDeviation:
                        {
                            var stat = ImageTools.CalculateDeviationFull(new short[][,] { StandardImageToDraw });
                            var k = 25000d / stat.Max[0];
                            var std = ImageTools.Multiply(StandardImageToDraw, k);
                            var img = ImageTools.Multiply(ImageToDraw, k);
                            var dif = DoMCLib.Tools.ImageTools.GetDifference(std, img);

                            ImageProcessResult.ResultImage = ImageTools.DeviationByLine(dif, 10);
                        }
                        break;
                    case DefectMethod.Gradient:
                        {
                            var stat = ImageTools.CalculateDeviationFull(new short[][,] { StandardImageToDraw });
                            var k = 25000d / stat.Max[0];
                            var std = ImageTools.Multiply(StandardImageToDraw, k);
                            var img = ImageTools.Multiply(ImageToDraw, k);
                            var dif = DoMCLib.Tools.ImageTools.GetDifference(std, img);
                            var grad = ImageTools.GradientAbs(dif);

                            ImageProcessResult.ResultImage = grad;
                        }
                        break;
                    case DefectMethod.VNormalize:
                        {
                            ImageToDraw = ImageTools.NormalizeByFibers(OriginalImage, ImageProcessParameters.GetRectangle(), 25000);
                            StandardImageToDraw = ImageTools.NormalizeByFibers(OriginalStandardImage, ImageProcessParameters.GetRectangle(), 25000);
                            var diff = DoMCLib.Tools.ImageTools.GetDifference(StandardImageToDraw, ImageToDraw);

                            ImageProcessResult.ResultImage = diff;
                        }
                        break;
                    case DefectMethod.VNormalizeDenoise:
                        {
                            var img = ImageTools.Denoise(OriginalImage);
                            img = ImageTools.NormalizeByFibers(img, ImageProcessParameters.GetRectangle(), 25000);
                            ImageToDraw = img;
                            var stdimg = ImageTools.Denoise(OriginalStandardImage);
                            stdimg = ImageTools.NormalizeByFibers(stdimg, ImageProcessParameters.GetRectangle(), 25000);
                            StandardImageToDraw = stdimg;
                            var diff = DoMCLib.Tools.ImageTools.GetDifference(StandardImageToDraw, ImageToDraw);
                            //ImageToDraw = ImageTools.NormalizeByFibers(img, ImageProcessParameters.GetRectangle(), 25000);
                            //StandardImageToDraw = ImageTools.NormalizeByFibers(OriginalStandardImage, ImageProcessParameters.GetRectangle(), 25000);
                            //var diff = DoMCLib.Tools.ImageTools.GetDifference(StandardImageToDraw, ImageToDraw);

                            ImageProcessResult.ResultImage = diff;
                        }
                        break;
                }

            }

            var stop = timer.ElapsedTicks;
            var seconds = (stop - start) * 1e-4;
            lblTimeDecision.Text = seconds.ToString("F2") + " мс";

            if (ImageToDraw != null) TestBmpReadImage = ImageTools.DrawImage(ImageToDraw, invertColors, CheckArea: ShowCheckArea ? CheckArea : (Rectangle?)null);
            if (StandardImageToDraw != null) TestBmpStandardImage = ImageTools.DrawImage(StandardImageToDraw, invertColors, CheckArea: ShowCheckArea ? CheckArea : (Rectangle?)null);
            if (isSocketGood)
            {
                if (ImageToDraw != null && StandardImageToDraw != null && ImageProcessResult.ResultImage != null) TestBmpDiffImage = ImageTools.DrawImage(ImageProcessResult.ResultImage, invert: invertColors, CheckArea: ShowCheckArea ? CheckArea : (Rectangle?)null);

            }
            else
            {
                if (ImageToDraw != null && StandardImageToDraw != null && ImageProcessResult.ResultImage != null) TestBmpDiffImage = ImageTools.DrawImage(ImageProcessResult.ResultImage, invertColors, BadPoint: ImageProcessResult.MaxDeviationPoint, CheckArea: ShowCheckArea ? CheckArea : (Rectangle?)null);
            }
            SetImages();
        }

        private void RedrawImages()
        {
            pbTestDifference.Invalidate();
            pbTestReadImage.Invalidate();
            pbTestStandard.Invalidate();
        }

        private void TestTabDrawGraphLine(int linen)
        {
            short[] ReadLine;
            short[] StandardLine;
            short[] DiffLine;
            if (cbFullMax.Checked)
            {
                ReadLine = LineFrom2D(ImageToDraw, linen);
                StandardLine = LineFrom2D(ImageToDraw, linen);
                DiffLine = LineFrom2D(ImageToDraw, linen);
                for (int y = 0; y < 512; y++) for (int x = 0; x < 512; x++) if (ReadLine[x] < ImageToDraw[y, x]) ReadLine[x] = ImageToDraw[y, x]; ;
                for (int y = 0; y < 512; y++) for (int x = 0; x < 512; x++) if (StandardLine[x] < StandardImageToDraw[y, x]) StandardLine[x] = StandardImageToDraw[y, x]; ;
                for (int y = 0; y < 512; y++) for (int x = 0; x < 512; x++) if (DiffLine[x] < ImageProcessResult.ResultImage[y, x]) DiffLine[x] = ImageProcessResult.ResultImage[y, x]; ;
            }
            else
            {
                if (cbFullAvg.Checked)
                {
                    ReadLine = LineFrom2D(ImageToDraw, linen);
                    StandardLine = LineFrom2D(ImageToDraw, linen);
                    DiffLine = LineFrom2D(ImageToDraw, linen);
                    for (int x = MainRect.Left; x <= MainRect.Right; x++)
                    {
                        if (x < 0 || x > 511) continue;
                        var sum = 0;
                        for (int y = MainRect.Top; y < MainRect.Bottom; y++)
                        {
                            if (y < 0 || y > 511) continue;
                            sum += ImageToDraw[y, x];
                        }
                        ReadLine[x] = (short)(sum / MainRect.Height);
                    }
                    for (int x = MainRect.Left; x <= MainRect.Right; x++)
                    {
                        if (x < 0 || x > 511) continue;
                        var sum = 0;
                        for (int y = MainRect.Top; y < MainRect.Bottom; y++)
                        {
                            if (y < 0 || y > 511) continue;
                            sum += StandardImageToDraw[y, x];
                        }
                        StandardLine[x] = (short)(sum / MainRect.Height);
                    }
                    for (int x = MainRect.Left; x <= MainRect.Right; x++)
                    {
                        if (x < 0 || x > 511) continue;
                        DiffLine[x] = (short)(ReadLine[x] - StandardLine[x]);
                    }
                    /*
                    for (int x = 0; x < 512; x++)
                    {
                        var sum = 0;
                        for (int y = 0; y < 512; y++)
                            sum += Image[y, x];
                        ReadLine[x] = (short)(sum / 512);
                    }
                    for (int x = 0; x < 512; x++)
                    {
                        var sum = 0;
                        for (int y = 0; y < 512; y++)
                            sum += StandardImage[y, x];
                        StandardLine[x] = (short)(sum / 512);
                    }
                    for (int x = 0; x < 512; x++)
                    {
                        DiffLine[x] = (short)(ReadLine[x] - StandardLine[x]);
                    }*/
                }
                else
                {
                    ReadLine = LineFrom2D(ImageToDraw, linen);
                    StandardLine = LineFrom2D(StandardImageToDraw, linen);
                    DiffLine = LineFrom2D(ImageProcessResult.ResultImage, linen);
                }
            }

            {
                #region Graph Read
                if (ReadLine != null)
                {
                    chTestReadLine.ChartAreas.Clear();
                    var ca = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
                    ca.AxisX.Minimum = 0;
                    ca.AxisX.Interval = 32;
                    ca.AxisX.Maximum = 512;
                    chTestReadLine.ChartAreas.Add(ca);
                    chTestReadLine.Series.Clear();


                    var ns = new System.Windows.Forms.DataVisualization.Charting.Series();
                    ns.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    ns.BorderWidth = 2;
                    ns.Color = Color.OrangeRed;
                    ns.Points.Clear();
                    for (int i = 0; i < 512; i++)
                        ns.Points.AddXY(i, ReadLine[i]);
                    chTestReadLine.Series.Add(ns);
                }
                #endregion


                #region Standard
                if (StandardLine != null)
                {
                    chTestStandard.ChartAreas.Clear();
                    var ca = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
                    ca.AxisX.Minimum = 0;
                    ca.AxisX.Interval = 32;
                    ca.AxisX.Maximum = 512;
                    chTestStandard.ChartAreas.Add(ca);
                    chTestStandard.Series.Clear();


                    var ns = new System.Windows.Forms.DataVisualization.Charting.Series();
                    ns.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    ns.BorderWidth = 2;
                    ns.Color = Color.OrangeRed;
                    ns.Points.Clear();
                    for (int i = 0; i < 512; i++)
                        ns.Points.AddXY(i, StandardLine[i]);
                    chTestStandard.Series.Add(ns);
                }
                #endregion

                #region Difference
                if (DiffLine != null)
                {

                    chTestDiff.ChartAreas.Clear();
                    var ca = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
                    ca.AxisX.Minimum = 0;
                    ca.AxisX.Interval = 32;
                    ca.AxisX.Maximum = 512;
                    chTestDiff.ChartAreas.Add(ca);
                    chTestDiff.Series.Clear();


                    var ns = new System.Windows.Forms.DataVisualization.Charting.Series();
                    ns.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    ns.BorderWidth = 2;
                    ns.Color = Color.OrangeRed;
                    ns.Points.Clear();
                    for (int i = 0; i < 512; i++)
                        ns.Points.AddXY(i, DiffLine[i]);
                    chTestDiff.Series.Add(ns);
                }
                #endregion
            }

        }

        private short[] LineFrom2D(short[,] Image, int frame)
        {
            if (Image == null) return null;
            short[] line = new short[512];
            if (cbVertical.Checked)
            {
                for (int x = 0; x < 512; x++) line[x] = Image[x, frame];
            }
            else
            {
                for (int x = 0; x < 512; x++) line[x] = Image[frame, x];
            }
            return line;
        }

        private void cmsiLoadStandard_Click(object sender, EventArgs e)
        {
            var sd = new OpenFileDialog();
            if (sd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (var file = new FileStream(sd.FileName, FileMode.Open, FileAccess.Read))
                    {
                        var filearr = new byte[file.Length];
                        file.Read(filearr, 0, filearr.Length);
                        var imgarr = ImageTools.ArrayToImage(filearr);
                        SetStandardImage(imgarr);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void SetStandardImage(short[,] img)
        {
            //StandardImageToDraw = img;
            OriginalStandardImage = img;
        }
        public void SetImage(short[,] img)
        {
            OriginalImage = img;
            //ImageToDraw = img;
        }

        public void SetImageProcessParameters(DoMCLib.Configuration.ImageProcessParameters ipp)
        {
            ImageProcessParameters = ipp.Clone();
            CheckArea = ImageProcessParameters.GetRectangle();
            if (ImageProcessParameters.Decisions[0].Operations.Count == 0)
            {
                ImageProcessParameters.Decisions[0].Operations.Add(new Classes.Configuration.CCD.DecisionOperation() { OperationType = Classes.Configuration.CCD.DecisionOperationType.Difference, Parameter = 0 });
                ImageProcessParameters.Decisions[0].Operations.Add(new Classes.Configuration.CCD.DecisionOperation() { OperationType = Classes.Configuration.CCD.DecisionOperationType.Dispersion, Parameter = 10 });
                ImageProcessParameters.Decisions[0].Result = Classes.Configuration.CCD.DecisionActionResult.Defect;
                ImageProcessParameters.Decisions[0].DecisionAction = Classes.MakeDecisionAction.Max;
                ImageProcessParameters.Decisions[0].ParameterCompareGoodIfLess = 1500;
            }
            if (ImageProcessParameters.Decisions[1].Operations.Count == 0)
            {
                ImageProcessParameters.Decisions[1].Operations.Add(new Classes.Configuration.CCD.DecisionOperation() { OperationType = Classes.Configuration.CCD.DecisionOperationType.Difference, Parameter = 0 });
                ImageProcessParameters.Decisions[1].Result = Classes.Configuration.CCD.DecisionActionResult.Color;
                ImageProcessParameters.Decisions[1].DecisionAction = Classes.MakeDecisionAction.Average;
                ImageProcessParameters.Decisions[1].ParameterCompareGoodIfLess = 1500;
            }
        }

        private void cmsiLoadImage_Click(object sender, EventArgs e)
        {
            var sd = new OpenFileDialog();
            if (sd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (var file = new FileStream(sd.FileName, FileMode.Open, FileAccess.Read))
                    {
                        var filearr = new byte[file.Length];
                        file.Read(filearr, 0, filearr.Length);
                        var imgarr = ImageTools.ArrayToImage(filearr);
                        SetImage(imgarr);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void cmsiLoadCompressedStandard_Click(object sender, EventArgs e)
        {
            var sd = new OpenFileDialog();
            if (sd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (var file = new FileStream(sd.FileName, FileMode.Open, FileAccess.Read))
                    {
                        var filearr = new byte[file.Length];
                        file.Read(filearr, 0, filearr.Length);
                        var base64str = Encoding.ASCII.GetString(filearr);
                        var compressed = ImageTools.FromBase64(base64str);
                        var bytearr = ImageTools.Decompress(compressed);
                        var imgarr = ImageTools.ArrayToImage(bytearr);
                        StandardImageToDraw = imgarr;
                        CalcImages();
                        RedrawImages();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void cmsiLoadCompressedImage_Click(object sender, EventArgs e)
        {
            var sd = new OpenFileDialog();
            if (sd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (var file = new FileStream(sd.FileName, FileMode.Open, FileAccess.Read))
                    {
                        var filearr = new byte[file.Length];
                        file.Read(filearr, 0, filearr.Length);
                        var base64str = Encoding.ASCII.GetString(filearr);
                        var compressed = ImageTools.FromBase64(base64str);
                        var bytearr = ImageTools.Decompress(compressed);
                        var imgarr = ImageTools.ArrayToImage(bytearr);
                        ImageToDraw = imgarr;
                        CalcImages();
                        RedrawImages();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void SetImages()
        {
            if (TestBmpReadImage != null)
                pbTestReadImage.Image = TestBmpReadImage;

            if (TestBmpDiffImage != null)
                pbTestDifference.Image = TestBmpDiffImage;

            if (TestBmpStandardImage != null)
                pbTestStandard.Image = TestBmpStandardImage;
        }

        private void pbTestReadImage_Paint(object sender, PaintEventArgs e)
        {
            //if (TestBmpReadImage == null) return;
            //pbTestReadImage.Image = TestBmpReadImage;
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

        private void pbTestDifference_Paint(object sender, PaintEventArgs e)
        {
            //if (TestBmpDiffImage == null) return;
            //pbTestDifference.Image = TestBmpDiffImage;
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

        private void pbTestStandard_Paint(object sender, PaintEventArgs e)
        {
            //if (TestBmpStandardImage == null) return;
            //pbTestStandard.Image = TestBmpStandardImage;
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
            TestTabDrawGraphLine((int)numFrame.Value);
            RedrawImages();
        }

        private void cbVertical_CheckedChanged(object sender, EventArgs e)
        {
            RedrawImages();
        }

        private void cbFullMax_CheckedChanged(object sender, EventArgs e)
        {
            if (cbFullMax.Checked || cbFullAvg.Checked)
            {
                numFrame.Enabled = false;
                cbVertical.Enabled = false;
            }
            else
            {
                numFrame.Enabled = true;
                cbVertical.Enabled = true;

            }
            numFrame.Value = 0;
            RedrawImages();
            TestTabDrawGraphLine(0);
        }

        private void pbTestReadImage_MouseUp(object sender, MouseEventArgs e)
        {
            if (cbVertical.Checked)
            {
                if (e.X >= 0 && e.X < 512)
                    numFrame.Value = e.X;
            }
            else
            {
                if (e.Y >= 0 && e.Y < 512)
                    numFrame.Value = e.Y;
            }
        }

        private void pbTestDifference_MouseUp(object sender, MouseEventArgs e)
        {
            if (cbVertical.Checked)
            {
                if (e.X >= 0 && e.X < 512)
                    numFrame.Value = e.X;
            }
            else
            {
                if (e.Y >= 0 && e.Y < 512)
                    numFrame.Value = e.Y;
            }
        }

        private void pbTestStandard_MouseUp(object sender, MouseEventArgs e)
        {
            if (cbVertical.Checked)
            {
                if (e.X >= 0 && e.X < 512)
                    numFrame.Value = e.X;
            }
            else
            {
                if (e.Y >= 0 && e.Y < 512)
                    numFrame.Value = e.Y;
            }
        }





        private void CheckPreformAlgorithmsForm_Resize(object sender, EventArgs e)
        {
            Prepare();
        }

        private void cbInvertColors_CheckedChanged(object sender, EventArgs e)
        {
            invertColors = cbInvertColors.Checked;
            CalcImages();
            RedrawImages();
        }

        private void tsmiSaveImage_Click(object sender, EventArgs e)
        {
            var sd = new SaveFileDialog();
            sd.Filter = "Binary(*.bin)|*.bin|CSV(*csv)|*.csv";
            if (sd.ShowDialog() == DialogResult.OK)
            {
                switch (sd.FilterIndex)
                {
                    case 1:
                        if (sender == tsmiSaveImage)
                        {
                            SaveImageBin(ImageToDraw, sd.FileName);
                        }
                        else
                        {
                            if (sender == tsmiSaveStandardImage)
                            {
                                SaveImageBin(StandardImageToDraw, sd.FileName);
                            }
                            else
                            {
                                SaveImageBin(ImageProcessResult.ResultImage, sd.FileName);
                            }
                        }
                        break;
                    case 2:
                        if (sender == tsmiSaveImage)
                        {
                            SaveImageCSV(ImageToDraw, sd.FileName);
                        }
                        else
                        {
                            if (sender == tsmiSaveStandardImage)
                            {
                                SaveImageCSV(StandardImageToDraw, sd.FileName);
                            }
                            else
                            {
                                SaveImageCSV(ImageProcessResult.ResultImage, sd.FileName);
                            }
                        }
                        break;
                }
            }
        }
        private void SaveImageBin(short[,] img, string filename)
        {
            var bytes = ImageTools.ImageToArray(img);
            using (var f = File.OpenWrite(filename))
            {
                f.Write(bytes, 0, bytes.Length);
            }
        }
        private void SaveImageCSV(short[,] img, string filename)
        {
            using (var sw = new StreamWriter(filename))
            {
                for (int y = 0; y < 512; y++)
                {
                    for (int x = 0; x < 512; x++)
                    {
                        sw.Write(img[y, x] + ";");
                    }
                    sw.WriteLine();
                }
            }
        }

        private void спеднеквадратическоеОтклонениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CalculationMethod = DefectMethod.CheckBySettings;
            RecalcAndRedrawImages();
        }

        private enum DefectMethod
        {
            CheckBySettings,
            PlainDifference,
            Normalize,
            NormalizedDeviation,
            Gradient,
            VNormalize,
            VNormalizeDenoise
        }


        private void tsmiPlainDifference_Click(object sender, EventArgs e)
        {
            CalculationMethod = DefectMethod.PlainDifference;
            RecalcAndRedrawImages();
        }

        private void cbShowCheckSquare_CheckedChanged(object sender, EventArgs e)
        {
            ShowCheckArea = cbShowCheckArea.Checked;
            CalcImages();
            RedrawImages();

        }

        private void tsmiNormalize_Click(object sender, EventArgs e)
        {
            CalculationMethod = DefectMethod.Normalize;
            RecalcAndRedrawImages();
        }

        private void tsmiDeviationOfNormilized_Click(object sender, EventArgs e)
        {

            CalculationMethod = DefectMethod.NormalizedDeviation;
            RecalcAndRedrawImages();
        }

        private void tsmiGradient_Click(object sender, EventArgs e)
        {
            CalculationMethod = DefectMethod.Gradient;
            RecalcAndRedrawImages();
        }

        private void tsmiVNormalize_Click(object sender, EventArgs e)
        {
            CalculationMethod = DefectMethod.VNormalize;
            RecalcAndRedrawImages();
        }

        private void tsmiVNormalizeDenoise_Click(object sender, EventArgs e)
        {
            CalculationMethod = DefectMethod.VNormalizeDenoise;
            RecalcAndRedrawImages();
        }
    }
    public class Gauss
    {

        public static double[] Solve(double[][] m)
        {
            var N = m.Length;
            var k = new double[N];
            for (int i = 0; i < m.Length; i++)
            {
                if (m[i].Length != N + 1) return k;
            }

            double[][] M = new double[N][];
            for (int i = 0; i < m.Length; i++)
            {
                M[i] = new double[N + 1];
                //bool ToZero = false;
                //if (m[i].Any(e => e > 1e-3)) ToZero = true;
                M[i] = RoundLine(m[i]);
                /*for (int j = 0; j < N + 1; j++)
                {
                    M[i][j] = Math.Round(m[i][j],8);
                }*/
            }


            for (int j = 0; j < N; j++)
            {
                var mainLine = M[j];
                var mainind = IndexOfFirstNonZeroElenment(mainLine);
                if (mainind == -1) throw new ArithmeticException("Есть повторы строк. Однозначно решить невозможно");
                var mainV = mainLine[mainind];
                var reducedMainLine = RoundLine(LineMultiplyBy(mainLine, 1 / mainV));
                M[j] = reducedMainLine;
                for (int i = j + 1; i < N; i++)
                {
                    var curLine = M[i];
                    var curind = IndexOfFirstNonZeroElenment(curLine);
                    if (curind == -1) throw new ArithmeticException("Есть повторы строк. Однозначно решить невозможно");
                    if (curind > j) continue;
                    var curV = curLine[curind];
                    var reducedCurLine = RoundLine(LineMultiplyBy(curLine, 1 / curV));
                    var newCurLine = RoundLine(SubstractLines(reducedCurLine, reducedMainLine));
                    M[i] = newCurLine;
                }
            }
            M = SortByFirstElementPosition(M);
            for (int j = N - 1; j >= 0; j--)
            {
                var mainLine = M[j];
                var mainind = IndexOfLastNonZeroElenment(mainLine);
                if (mainind == -1) throw new ArithmeticException("Есть повторы строк. Однозначно решить невозможно");
                //var mainV = mainLine[mainind];
                //var reducedMainLine = LineMultiplyBy(mainLine, 1 / mainV);
                //M[j] = reducedMainLine;
                for (int i = j - 1; i >= 0; i--)
                {
                    var curLine = M[i];
                    var curind = IndexOfLastNonZeroElenment(curLine);
                    if (curind == -1) throw new ArithmeticException("Есть повторы строк. Однозначно решить невозможно");
                    if (curind < j) continue;
                    var curV = curLine[curind];
                    var multipliedMainLine = RoundLine(LineMultiplyBy(mainLine, curV));
                    var newCurLine = RoundLine(SubstractLines(curLine, multipliedMainLine));

                    M[i] = newCurLine;
                }
            }
            return M.Select(line => line[line.Length - 1]).ToArray();
        }

        private static int IndexOfFirstNonZeroElenment(double[] line)
        {
            var v = Array.FindIndex(line, 0, line.Length - 1, e => e != 0);
            return v;
        }
        private static int IndexOfLastNonZeroElenment(double[] line)
        {
            var v = Array.FindLastIndex(line, line.Length - 2, line.Length - 1, e => e != 0);
            return v;
        }

        private static double[] LineMultiplyBy(double[] line, double v)
        {
            double[] result = new double[line.Length];
            for (var i = 0; i < line.Length; i++)
            {
                result[i] = line[i] * v;
            }
            return result;
        }
        private static double[][] SortByFirstElementPosition(double[][] M)
        {
            return M.OrderBy(line => IndexOfFirstNonZeroElenment(line)).ToArray();
        }
        private static double[] SubstractLines(double[] minuendLine, double[] subtrahendLine)
        {
            double[] line = new double[subtrahendLine.Length];
            for (var i = 0; i < line.Length; i++)
            {
                line[i] = minuendLine[i] - subtrahendLine[i];
            }
            return line;
        }

        private static double[] RoundLine(double[] line)
        {
            var newline = new double[line.Length];
            for (int i = 0; i < line.Length; i++)
            {
                newline[i] = Math.Round(line[i], 10);
            }
            return newline;
        }

    }


}
