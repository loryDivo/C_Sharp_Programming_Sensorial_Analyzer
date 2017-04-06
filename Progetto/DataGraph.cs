using Progetto;
using System;
using System.Drawing;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using ZedGraph;

namespace Progetto
{
    public class DataGraph : Form
    {
        private System.ComponentModel.IContainer components;
        private ZedGraph.ZedGraphControl zedGraphControlAccS0;
        private ZedGraph.ZedGraphControl zedGraphControlAccS1;
        private ZedGraph.ZedGraphControl zedGraphControlAccS2;
        private ZedGraph.ZedGraphControl zedGraphControlAccS3;
        private ZedGraph.ZedGraphControl zedGraphControlAccS4;
        private ZedGraph.ZedGraphControl zedGraphControlGirS0;
        private ZedGraph.ZedGraphControl zedGraphControlGirS1;
        private ZedGraph.ZedGraphControl zedGraphControlGirS2;
        private ZedGraph.ZedGraphControl zedGraphControlGirS3;
        private ZedGraph.ZedGraphControl zedGraphControlGirS4;
        private ZedGraph.ZedGraphControl zedGraphControlThetaS0;
        /*private ZedGraph.ZedGraphControl zedGraphControlThetaS1;
        private ZedGraph.ZedGraphControl zedGraphControlThetaS2;
        private ZedGraph.ZedGraphControl zedGraphControlThetaS3;
        private ZedGraph.ZedGraphControl zedGraphControlThetaS4;*/
        private ZedGraph.ZedGraphControl zedGraphControlStandLaySit;
        private ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private VisualThread visualThread;

        public DataGraph(VisualThread visualThread)
        {
            this.visualThread = visualThread;
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            zedGraphControlAccS0_Load(this, new EventArgs());
            zedGraphControlAccS1_Load(this, new EventArgs());
            zedGraphControlAccS2_Load(this, new EventArgs());
            zedGraphControlAccS3_Load(this, new EventArgs());
            zedGraphControlAccS4_Load(this, new EventArgs());
            zedGraphControlGirS0_Load(this, new EventArgs());
            zedGraphControlGirS1_Load(this, new EventArgs());
            zedGraphControlGirS2_Load(this, new EventArgs());
            zedGraphControlGirS3_Load(this, new EventArgs());
            zedGraphControlGirS4_Load(this, new EventArgs());
            zedGraphControlThetaS0_Load(this, new EventArgs());
            /*zedGraphControlThetaS1_Load(this, new EventArgs());
            zedGraphControlThetaS2_Load(this, new EventArgs());
            zedGraphControlThetaS3_Load(this, new EventArgs());
            zedGraphControlThetaS4_Load(this, new EventArgs());*/
            zedGraphControlStandLaySit_Load(this, new EventArgs());
            setGraph(Sensor.Sensor1, DataInfo.Acc);
            this.FormClosing += DataGraph_Closing;
        }

        public void UpdateAllZedGraph()
        {
            zedGraphControlAccS0.AxisChange();
            zedGraphControlAccS1.AxisChange();
            zedGraphControlAccS2.AxisChange();
            zedGraphControlAccS3.AxisChange();
            zedGraphControlAccS4.AxisChange();
            zedGraphControlGirS0.AxisChange();
            zedGraphControlGirS1.AxisChange();
            zedGraphControlGirS2.AxisChange();
            zedGraphControlGirS3.AxisChange();
            zedGraphControlGirS4.AxisChange();
            zedGraphControlThetaS0.AxisChange();
            /*zedGraphControlThetaS1.AxisChange();
            zedGraphControlThetaS2.AxisChange();
            zedGraphControlThetaS3.AxisChange();
            zedGraphControlThetaS4.AxisChange();*/
            zedGraphControlStandLaySit.AxisChange();
            zedGraphControlAccS0.Invalidate();
            zedGraphControlAccS1.Invalidate();
            zedGraphControlAccS2.Invalidate();
            zedGraphControlAccS3.Invalidate();
            zedGraphControlAccS4.Invalidate();
            zedGraphControlGirS0.Invalidate();
            zedGraphControlGirS1.Invalidate();
            zedGraphControlGirS2.Invalidate();
            zedGraphControlGirS3.Invalidate();
            zedGraphControlGirS4.Invalidate();
            zedGraphControlThetaS0.Invalidate();
            /*zedGraphControlThetaS1.Invalidate();
            zedGraphControlThetaS2.Invalidate();
            zedGraphControlThetaS3.Invalidate();
            zedGraphControlThetaS4.Invalidate();*/
            zedGraphControlStandLaySit.Invalidate();
            zedGraphControlAccS0.Refresh();
            zedGraphControlAccS1.Refresh();
            zedGraphControlAccS2.Refresh();
            zedGraphControlAccS3.Refresh();
            zedGraphControlAccS4.Refresh();
            zedGraphControlGirS0.Refresh();
            zedGraphControlGirS1.Refresh();
            zedGraphControlGirS2.Refresh();
            zedGraphControlGirS3.Refresh();
            zedGraphControlGirS4.Refresh();
            zedGraphControlThetaS0.Refresh();
            /*zedGraphControlThetaS1.Refresh();
            zedGraphControlThetaS2.Refresh();
            zedGraphControlThetaS3.Refresh();
            zedGraphControlThetaS4.Refresh();*/
            zedGraphControlStandLaySit.Refresh();
        }

        /** Setta uno ZedGraph standard, utile per quasi tutti i nostri grafici. */
        private void standardZedGraph_Load(ZedGraphControl zedGraph, Color color, string name, string yUnit, PointPairList points)
        {
            zedGraph.GraphPane.CurveList.Clear();
            // GraphPane object holds one or more Curve objects (or plots)
            GraphPane myPane = zedGraph.GraphPane;
            myPane.Title.Text = "Value of " + name;
            myPane.XAxis.Title.Text = "Time (unit)";
            myPane.YAxis.Title.Text = "Value(" + yUnit + ")";
            myPane.XAxis.Scale.MaxAuto = true;                  //modifica larghezza asse X
            LineItem MyCurve = myPane.AddCurve(name, points, color, SymbolType.None);
            MyCurve.Line.Width = 0.5F;
            zedGraph.AxisChange();
            zedGraph.Refresh();
            zedGraph.Invalidate();
        }

        private void zedGraphControlAccS0_Load(object sender, EventArgs e)
        {
            standardZedGraph_Load(zedGraphControlAccS0, Color.Red, "Accelerometer", "module", visualThread.pointPlAccSensor1);
        }

        private void zedGraphControlAccS1_Load(object sender, EventArgs e)
        {
            standardZedGraph_Load(zedGraphControlAccS1, Color.Red, "Accelerometer", "module", visualThread.pointPlAccSensor2);
        }

        private void zedGraphControlAccS2_Load(object sender, EventArgs e)
        {
            standardZedGraph_Load(zedGraphControlAccS2, Color.Red, "Accelerometer", "module", visualThread.pointPlAccSensor3);
        }

        private void zedGraphControlAccS3_Load(object sender, EventArgs e)
        {
            standardZedGraph_Load(zedGraphControlAccS3, Color.Red, "Accelerometer", "module", visualThread.pointPlAccSensor4);
        }

        private void zedGraphControlAccS4_Load(object sender, EventArgs e)
        {
            standardZedGraph_Load(zedGraphControlAccS4, Color.Red, "Accelerometer", "module", visualThread.pointPlAccSensor5);
        }

        private void zedGraphControlGirS0_Load(object sender, EventArgs e)
        {
            standardZedGraph_Load(zedGraphControlGirS0, Color.Blue, "Giroscope", "module", visualThread.pointPlGirSensor1);
        }

        private void zedGraphControlGirS1_Load(object sender, EventArgs e)
        {
            standardZedGraph_Load(zedGraphControlGirS1, Color.Blue, "Giroscope", "module", visualThread.pointPlGirSensor2);
        }

        private void zedGraphControlGirS2_Load(object sender, EventArgs e)
        {
            standardZedGraph_Load(zedGraphControlGirS2, Color.Blue, "Giroscope", "module", visualThread.pointPlGirSensor3);
        }

        private void zedGraphControlGirS3_Load(object sender, EventArgs e)
        {
            standardZedGraph_Load(zedGraphControlGirS3, Color.Blue, "Giroscope", "module", visualThread.pointPlGirSensor4);
        }

        private void zedGraphControlGirS4_Load(object sender, EventArgs e)
        {
            standardZedGraph_Load(zedGraphControlGirS4, Color.Blue, "Giroscope", "module", visualThread.pointPlGirSensor5);
        }

        private void zedGraphControlThetaS0_Load(object sender, EventArgs e)
        {
            standardZedGraph_Load(zedGraphControlThetaS0, Color.Green, "Theta", "degrees", visualThread.pointPlThetaSensor1);
        }

        /*private void zedGraphControlThetaS1_Load(object sender, EventArgs e)
        {
            standardZedGraph_Load(zedGraphControlThetaS1, Color.Green, "Theta", "degrees", visualThread.pointPlThetaSensor2);
        }

        private void zedGraphControlThetaS2_Load(object sender, EventArgs e)
        {
            standardZedGraph_Load(zedGraphControlThetaS2, Color.Green, "Theta", "degrees", visualThread.pointPlThetaSensor3);
        }

        private void zedGraphControlThetaS3_Load(object sender, EventArgs e)
        {
            standardZedGraph_Load(zedGraphControlThetaS3, Color.Green, "Theta", "degrees", visualThread.pointPlThetaSensor4);
        }

        private void zedGraphControlThetaS4_Load(object sender, EventArgs e)
        {
            standardZedGraph_Load(zedGraphControlThetaS4, Color.Green, "Theta", "degrees", visualThread.pointPlThetaSensor5);
        }*/

        /** Questa ZedGraph è particolare per gli elementi che deve contenere. */
        private void zedGraphControlStandLaySit_Load(object sender, EventArgs e)
        {
            standardZedGraph_Load(zedGraphControlStandLaySit, Color.Orange, "Stand Lay Sit", "", visualThread.pointPlStandLaySit);
            GraphPane graphPane = zedGraphControlStandLaySit.GraphPane;

            LineObj threshHoldLine = new LineObj(
            Color.Blue,
            0,
            Recognizer.LAY_THRESHOLD,
            1,
            Recognizer.LAY_THRESHOLD);
            threshHoldLine.Location.CoordinateFrame = CoordType.XChartFractionYScale;
            threshHoldLine.IsClippedToChartRect = true;
            threshHoldLine.Line.Style = System.Drawing.Drawing2D.DashStyle.Dash;
            graphPane.GraphObjList.Add(threshHoldLine);

            LineObj threshHoldLine2 = new LineObj(
            Color.Blue,
            0,
            Recognizer.LAY_SIT_THRESHOLD,
            1,
            Recognizer.LAY_SIT_THRESHOLD);
            threshHoldLine2.Location.CoordinateFrame = CoordType.XChartFractionYScale;
            threshHoldLine2.IsClippedToChartRect = true;
            threshHoldLine2.Line.Style = System.Drawing.Drawing2D.DashStyle.Dash;
            graphPane.GraphObjList.Add(threshHoldLine2);

            LineObj threshHoldLine3 = new LineObj(
            Color.Blue,
            0,
            Recognizer.SIT_THRESHOLD,
            1,
            Recognizer.SIT_THRESHOLD);
            threshHoldLine3.Location.CoordinateFrame = CoordType.XChartFractionYScale;
            threshHoldLine3.IsClippedToChartRect = true;
            threshHoldLine3.Line.Style = System.Drawing.Drawing2D.DashStyle.Dash;
            graphPane.GraphObjList.Add(threshHoldLine3);

            TextObj text1 = new TextObj("Lay", -0.03D, Recognizer.LAY_THRESHOLD + 0.3D, CoordType.XChartFractionYScale, AlignH.Right, AlignV.Top);
            text1.ZOrder = ZOrder.D_BehindAxis;
            graphPane.GraphObjList.Add(text1);

            TextObj text2 = new TextObj("Lay/Sit", -0.03D, Recognizer.LAY_SIT_THRESHOLD + 0.3D, CoordType.XChartFractionYScale, AlignH.Right, AlignV.Top);
            text2.ZOrder = ZOrder.D_BehindAxis;
            graphPane.GraphObjList.Add(text2);

            TextObj text3 = new TextObj("Sit", -0.03D, Recognizer.SIT_THRESHOLD + 0.3D, CoordType.XChartFractionYScale, AlignH.Right, AlignV.Top);
            text3.ZOrder = ZOrder.D_BehindAxis;
            graphPane.GraphObjList.Add(text3);

            graphPane.YAxis.Scale.Min = 0D;
            graphPane.YAxis.Scale.Max = 12D;
            graphPane.YAxis.Title.Text = "       ";

            zedGraphControlStandLaySit.AxisChange();
            zedGraphControlStandLaySit.Refresh();
            zedGraphControlStandLaySit.Invalidate();
        }

        private void DataGraph_Load(object sender, EventArgs e)
        {
        }

        public void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.zedGraphControlAccS0 = new ZedGraph.ZedGraphControl();
            this.zedGraphControlAccS1 = new ZedGraph.ZedGraphControl();
            this.zedGraphControlAccS2 = new ZedGraph.ZedGraphControl();
            this.zedGraphControlAccS3 = new ZedGraph.ZedGraphControl();
            this.zedGraphControlAccS4 = new ZedGraph.ZedGraphControl();
            this.zedGraphControlGirS0 = new ZedGraph.ZedGraphControl();
            this.zedGraphControlGirS1 = new ZedGraph.ZedGraphControl();
            this.zedGraphControlGirS2 = new ZedGraph.ZedGraphControl();
            this.zedGraphControlGirS3 = new ZedGraph.ZedGraphControl();
            this.zedGraphControlGirS4 = new ZedGraph.ZedGraphControl();
            this.zedGraphControlThetaS0 = new ZedGraph.ZedGraphControl();
            /*this.zedGraphControlThetaS1 = new ZedGraph.ZedGraphControl();
            this.zedGraphControlThetaS2 = new ZedGraph.ZedGraphControl();
            this.zedGraphControlThetaS3 = new ZedGraph.ZedGraphControl();
            this.zedGraphControlThetaS4 = new ZedGraph.ZedGraphControl();*/
            this.zedGraphControlStandLaySit = new ZedGraph.ZedGraphControl();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "S0 Accelerometer",
            "S1 Accelerometer",
            "S2 Accelerometer",
            "S3 Accelerometer",
            "S4 Accelerometer",
            "S0 Giroscope",
            "S1 Giroscope",
            "S2 Giroscope",
            "S3 Giroscope",
            "S4 Giroscope",
            "S0 Theta",
            "Stand Sit Lay"
                /*"S1 Theta",
                "S2 Theta",
                "S3 Theta",
                "S4 Theta"*/
            });
            this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox1.Location = new System.Drawing.Point(101, 17);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(174, 21);
            this.comboBox1.TabIndex = 2;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Current Graph:";
            // 
            // zedGraphControlAccS0
            // 
            this.zedGraphControlAccS0.Location = new System.Drawing.Point(34, 44);
            this.zedGraphControlAccS0.Name = "zedGraphControlAccS0";
            this.zedGraphControlAccS0.ScrollGrace = 0D;
            this.zedGraphControlAccS0.ScrollMaxX = 0D;
            this.zedGraphControlAccS0.ScrollMaxY = 0D;
            this.zedGraphControlAccS0.ScrollMaxY2 = 0D;
            this.zedGraphControlAccS0.ScrollMinX = 0D;
            this.zedGraphControlAccS0.ScrollMinY = 0D;
            this.zedGraphControlAccS0.ScrollMinY2 = 0D;
            this.zedGraphControlAccS0.Size = new System.Drawing.Size(666, 302);
            this.zedGraphControlAccS0.TabIndex = 4;
            //this.zedGraphControlAccS0.Load += new System.EventHandler(this.zedGraphControlAccS0_Load_1);
            // 
            // zedGraphControlAccS1
            // 
            this.zedGraphControlAccS1.Location = new System.Drawing.Point(34, 44);
            this.zedGraphControlAccS1.Name = "zedGraphControlAccS1";
            this.zedGraphControlAccS1.ScrollGrace = 0D;
            this.zedGraphControlAccS1.ScrollMaxX = 0D;
            this.zedGraphControlAccS1.ScrollMaxY = 0D;
            this.zedGraphControlAccS1.ScrollMaxY2 = 0D;
            this.zedGraphControlAccS1.ScrollMinX = 0D;
            this.zedGraphControlAccS1.ScrollMinY = 0D;
            this.zedGraphControlAccS1.ScrollMinY2 = 0D;
            this.zedGraphControlAccS1.Size = new System.Drawing.Size(666, 302);
            this.zedGraphControlAccS1.TabIndex = 5;
            // 
            // zedGraphControlAccS2
            // 
            this.zedGraphControlAccS2.Location = new System.Drawing.Point(34, 44);
            this.zedGraphControlAccS2.Name = "zedGraphControlAccS2";
            this.zedGraphControlAccS2.ScrollGrace = 0D;
            this.zedGraphControlAccS2.ScrollMaxX = 0D;
            this.zedGraphControlAccS2.ScrollMaxY = 0D;
            this.zedGraphControlAccS2.ScrollMaxY2 = 0D;
            this.zedGraphControlAccS2.ScrollMinX = 0D;
            this.zedGraphControlAccS2.ScrollMinY = 0D;
            this.zedGraphControlAccS2.ScrollMinY2 = 0D;
            this.zedGraphControlAccS2.Size = new System.Drawing.Size(666, 302);
            this.zedGraphControlAccS2.TabIndex = 6;
            // 
            // zedGraphControlAccS3
            // 
            this.zedGraphControlAccS3.Location = new System.Drawing.Point(34, 44);
            this.zedGraphControlAccS3.Name = "zedGraphControlAccS3";
            this.zedGraphControlAccS3.ScrollGrace = 0D;
            this.zedGraphControlAccS3.ScrollMaxX = 0D;
            this.zedGraphControlAccS3.ScrollMaxY = 0D;
            this.zedGraphControlAccS3.ScrollMaxY2 = 0D;
            this.zedGraphControlAccS3.ScrollMinX = 0D;
            this.zedGraphControlAccS3.ScrollMinY = 0D;
            this.zedGraphControlAccS3.ScrollMinY2 = 0D;
            this.zedGraphControlAccS3.Size = new System.Drawing.Size(666, 302);
            this.zedGraphControlAccS3.TabIndex = 7;
            // 
            // zedGraphControlAccS4
            // 
            this.zedGraphControlAccS4.Location = new System.Drawing.Point(34, 44);
            this.zedGraphControlAccS4.Name = "zedGraphControlAccS4";
            this.zedGraphControlAccS4.ScrollGrace = 0D;
            this.zedGraphControlAccS4.ScrollMaxX = 0D;
            this.zedGraphControlAccS4.ScrollMaxY = 0D;
            this.zedGraphControlAccS4.ScrollMaxY2 = 0D;
            this.zedGraphControlAccS4.ScrollMinX = 0D;
            this.zedGraphControlAccS4.ScrollMinY = 0D;
            this.zedGraphControlAccS4.ScrollMinY2 = 0D;
            this.zedGraphControlAccS4.Size = new System.Drawing.Size(666, 302);
            this.zedGraphControlAccS4.TabIndex = 8;
            // 
            // zedGraphControlGirS0
            // 
            this.zedGraphControlGirS0.Location = new System.Drawing.Point(34, 44);
            this.zedGraphControlGirS0.Name = "zedGraphControlGirS0";
            this.zedGraphControlGirS0.ScrollGrace = 0D;
            this.zedGraphControlGirS0.ScrollMaxX = 0D;
            this.zedGraphControlGirS0.ScrollMaxY = 0D;
            this.zedGraphControlGirS0.ScrollMaxY2 = 0D;
            this.zedGraphControlGirS0.ScrollMinX = 0D;
            this.zedGraphControlGirS0.ScrollMinY = 0D;
            this.zedGraphControlGirS0.ScrollMinY2 = 0D;
            this.zedGraphControlGirS0.Size = new System.Drawing.Size(666, 302);
            this.zedGraphControlGirS0.TabIndex = 9;
            // 
            // zedGraphControlGirS1
            // 
            this.zedGraphControlGirS1.Location = new System.Drawing.Point(34, 44);
            this.zedGraphControlGirS1.Name = "zedGraphControlGirS1";
            this.zedGraphControlGirS1.ScrollGrace = 0D;
            this.zedGraphControlGirS1.ScrollMaxX = 0D;
            this.zedGraphControlGirS1.ScrollMaxY = 0D;
            this.zedGraphControlGirS1.ScrollMaxY2 = 0D;
            this.zedGraphControlGirS1.ScrollMinX = 0D;
            this.zedGraphControlGirS1.ScrollMinY = 0D;
            this.zedGraphControlGirS1.ScrollMinY2 = 0D;
            this.zedGraphControlGirS1.Size = new System.Drawing.Size(666, 302);
            this.zedGraphControlGirS1.TabIndex = 10;
            // 
            // zedGraphControlGirS2
            // 
            this.zedGraphControlGirS2.Location = new System.Drawing.Point(34, 44);
            this.zedGraphControlGirS2.Name = "zedGraphControlGirS2";
            this.zedGraphControlGirS2.ScrollGrace = 0D;
            this.zedGraphControlGirS2.ScrollMaxX = 0D;
            this.zedGraphControlGirS2.ScrollMaxY = 0D;
            this.zedGraphControlGirS2.ScrollMaxY2 = 0D;
            this.zedGraphControlGirS2.ScrollMinX = 0D;
            this.zedGraphControlGirS2.ScrollMinY = 0D;
            this.zedGraphControlGirS2.ScrollMinY2 = 0D;
            this.zedGraphControlGirS2.Size = new System.Drawing.Size(666, 302);
            this.zedGraphControlGirS2.TabIndex = 11;
            // 
            // zedGraphControlGirS3
            // 
            this.zedGraphControlGirS3.Location = new System.Drawing.Point(34, 44);
            this.zedGraphControlGirS3.Name = "zedGraphControlGirS3";
            this.zedGraphControlGirS3.ScrollGrace = 0D;
            this.zedGraphControlGirS3.ScrollMaxX = 0D;
            this.zedGraphControlGirS3.ScrollMaxY = 0D;
            this.zedGraphControlGirS3.ScrollMaxY2 = 0D;
            this.zedGraphControlGirS3.ScrollMinX = 0D;
            this.zedGraphControlGirS3.ScrollMinY = 0D;
            this.zedGraphControlGirS3.ScrollMinY2 = 0D;
            this.zedGraphControlGirS3.Size = new System.Drawing.Size(666, 302);
            this.zedGraphControlGirS3.TabIndex = 12;
            // 
            // zedGraphControlGirS4
            // 
            this.zedGraphControlGirS4.Location = new System.Drawing.Point(34, 44);
            this.zedGraphControlGirS4.Name = "zedGraphControlGirS4";
            this.zedGraphControlGirS4.ScrollGrace = 0D;
            this.zedGraphControlGirS4.ScrollMaxX = 0D;
            this.zedGraphControlGirS4.ScrollMaxY = 0D;
            this.zedGraphControlGirS4.ScrollMaxY2 = 0D;
            this.zedGraphControlGirS4.ScrollMinX = 0D;
            this.zedGraphControlGirS4.ScrollMinY = 0D;
            this.zedGraphControlGirS4.ScrollMinY2 = 0D;
            this.zedGraphControlGirS4.Size = new System.Drawing.Size(666, 302);
            this.zedGraphControlGirS4.TabIndex = 13;
            // 
            // zedGraphControlThetaS0
            // 
            this.zedGraphControlThetaS0.Location = new System.Drawing.Point(34, 44);
            this.zedGraphControlThetaS0.Name = "zedGraphControlThetaS0";
            this.zedGraphControlThetaS0.ScrollGrace = 0D;
            this.zedGraphControlThetaS0.ScrollMaxX = 0D;
            this.zedGraphControlThetaS0.ScrollMaxY = 0D;
            this.zedGraphControlThetaS0.ScrollMaxY2 = 0D;
            this.zedGraphControlThetaS0.ScrollMinX = 0D;
            this.zedGraphControlThetaS0.ScrollMinY = 0D;
            this.zedGraphControlThetaS0.ScrollMinY2 = 0D;
            this.zedGraphControlThetaS0.Size = new System.Drawing.Size(666, 302);
            this.zedGraphControlThetaS0.TabIndex = 14;
            /*// 
            // zedGraphControlThetaS1
            // 
            this.zedGraphControlThetaS1.Location = new System.Drawing.Point(34, 44);
            this.zedGraphControlThetaS1.Name = "zedGraphControlThetaS1";
            this.zedGraphControlThetaS1.ScrollGrace = 0D;
            this.zedGraphControlThetaS1.ScrollMaxX = 0D;
            this.zedGraphControlThetaS1.ScrollMaxY = 0D;
            this.zedGraphControlThetaS1.ScrollMaxY2 = 0D;
            this.zedGraphControlThetaS1.ScrollMinX = 0D;
            this.zedGraphControlThetaS1.ScrollMinY = 0D;
            this.zedGraphControlThetaS1.ScrollMinY2 = 0D;
            this.zedGraphControlThetaS1.Size = new System.Drawing.Size(666, 302);
            this.zedGraphControlThetaS1.TabIndex = 15;
            // 
            // zedGraphControlThetaS2
            // 
            this.zedGraphControlThetaS2.Location = new System.Drawing.Point(34, 44);
            this.zedGraphControlThetaS2.Name = "zedGraphControlThetaS2";
            this.zedGraphControlThetaS2.ScrollGrace = 0D;
            this.zedGraphControlThetaS2.ScrollMaxX = 0D;
            this.zedGraphControlThetaS2.ScrollMaxY = 0D;
            this.zedGraphControlThetaS2.ScrollMaxY2 = 0D;
            this.zedGraphControlThetaS2.ScrollMinX = 0D;
            this.zedGraphControlThetaS2.ScrollMinY = 0D;
            this.zedGraphControlThetaS2.ScrollMinY2 = 0D;
            this.zedGraphControlThetaS2.Size = new System.Drawing.Size(666, 302);
            this.zedGraphControlThetaS2.TabIndex = 16;
            // 
            // zedGraphControlThetaS3
            // 
            this.zedGraphControlThetaS3.Location = new System.Drawing.Point(34, 44);
            this.zedGraphControlThetaS3.Name = "zedGraphControlThetaS3";
            this.zedGraphControlThetaS3.ScrollGrace = 0D;
            this.zedGraphControlThetaS3.ScrollMaxX = 0D;
            this.zedGraphControlThetaS3.ScrollMaxY = 0D;
            this.zedGraphControlThetaS3.ScrollMaxY2 = 0D;
            this.zedGraphControlThetaS3.ScrollMinX = 0D;
            this.zedGraphControlThetaS3.ScrollMinY = 0D;
            this.zedGraphControlThetaS3.ScrollMinY2 = 0D;
            this.zedGraphControlThetaS3.Size = new System.Drawing.Size(666, 302);
            this.zedGraphControlThetaS3.TabIndex = 17;
            // 
            // zedGraphControlThetaS4
            // 
            this.zedGraphControlThetaS4.Location = new System.Drawing.Point(34, 44);
            this.zedGraphControlThetaS4.Name = "zedGraphControlThetaS4";
            this.zedGraphControlThetaS4.ScrollGrace = 0D;
            this.zedGraphControlThetaS4.ScrollMaxX = 0D;
            this.zedGraphControlThetaS4.ScrollMaxY = 0D;
            this.zedGraphControlThetaS4.ScrollMaxY2 = 0D;
            this.zedGraphControlThetaS4.ScrollMinX = 0D;
            this.zedGraphControlThetaS4.ScrollMinY = 0D;
            this.zedGraphControlThetaS4.ScrollMinY2 = 0D;
            this.zedGraphControlThetaS4.Size = new System.Drawing.Size(666, 302);
            this.zedGraphControlThetaS4.TabIndex = 18;*/
            // 
            // zedGraphControlStandLaySit
            // 
            this.zedGraphControlStandLaySit.Location = new System.Drawing.Point(34, 44);
            this.zedGraphControlStandLaySit.Name = "zedGraphControlStandLaySit";
            this.zedGraphControlStandLaySit.ScrollGrace = 0D;
            this.zedGraphControlStandLaySit.ScrollMaxX = 0D;
            this.zedGraphControlStandLaySit.ScrollMaxY = 0D;
            this.zedGraphControlStandLaySit.ScrollMaxY2 = 0D;
            this.zedGraphControlStandLaySit.ScrollMinX = 0D;
            this.zedGraphControlStandLaySit.ScrollMinY = 0D;
            this.zedGraphControlStandLaySit.ScrollMinY2 = 0D;
            this.zedGraphControlStandLaySit.Size = new System.Drawing.Size(666, 302);
            this.zedGraphControlStandLaySit.TabIndex = 15;
            // 
            // DataGraph
            // 
            this.ClientSize = new System.Drawing.Size(738, 370);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.zedGraphControlAccS0);
            this.Controls.Add(this.zedGraphControlAccS1);
            this.Controls.Add(this.zedGraphControlAccS2);
            this.Controls.Add(this.zedGraphControlAccS3);
            this.Controls.Add(this.zedGraphControlAccS4);
            this.Controls.Add(this.zedGraphControlGirS0);
            this.Controls.Add(this.zedGraphControlGirS1);
            this.Controls.Add(this.zedGraphControlGirS2);
            this.Controls.Add(this.zedGraphControlGirS3);
            this.Controls.Add(this.zedGraphControlGirS4);
            this.Controls.Add(this.zedGraphControlThetaS0);
            /*this.Controls.Add(this.zedGraphControlThetaS1);
            this.Controls.Add(this.zedGraphControlThetaS2);
            this.Controls.Add(this.zedGraphControlThetaS3);
            this.Controls.Add(this.zedGraphControlThetaS4);*/
            this.Controls.Add(this.zedGraphControlStandLaySit);
            this.Name = "DataGraph";
            this.Text = "Xbus Master 2.0";
            this.Load += new System.EventHandler(this.DataGraph_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void initializeZedGraph(ZedGraphControl zedGraphControl, Action<object, EventArgs> zedGraphControl_Load)
        {
            zedGraphControl.Location = new System.Drawing.Point(22, 49);
            zedGraphControl.Name = "noName";
            zedGraphControl.ScrollGrace = 0D;
            zedGraphControl.ScrollMaxX = 0D;
            zedGraphControl.ScrollMaxY = 0D;
            zedGraphControl.ScrollMaxY2 = 0D;
            zedGraphControl.ScrollMinX = 0D;
            zedGraphControl.ScrollMinY = 0D;
            zedGraphControl.ScrollMinY2 = 0D;
            zedGraphControl.Size = new System.Drawing.Size(466, 209);
            zedGraphControl.TabIndex = 0;
            zedGraphControl.Load += new System.EventHandler(zedGraphControl_Load);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            int index = comboBox.SelectedIndex;

            if (index == 0)
            {
                setGraph(Sensor.Sensor1, DataInfo.Acc);
            }
            else if (index == 1)
            {
                setGraph(Sensor.Sensor2, DataInfo.Acc);
            }
            else if (index == 2)
            {
                setGraph(Sensor.Sensor3, DataInfo.Acc);
            }
            else if (index == 3)
            {
                setGraph(Sensor.Sensor4, DataInfo.Acc);
            }
            else if (index == 4)
            {
                setGraph(Sensor.Sensor5, DataInfo.Acc);
            }
            else if (index == 5)
            {
                setGraph(Sensor.Sensor1, DataInfo.Gir);
            }
            else if (index == 6)
            {
                setGraph(Sensor.Sensor2, DataInfo.Gir);
            }
            else if (index == 7)
            {
                setGraph(Sensor.Sensor3, DataInfo.Gir);
            }
            else if (index == 8)
            {
                setGraph(Sensor.Sensor4, DataInfo.Gir);
            }
            else if (index == 9)
            {
                setGraph(Sensor.Sensor5, DataInfo.Gir);
            }
            else if (index == 10)
            {
                setGraph(Sensor.Sensor1, DataInfo.Magn);
                /*} else if (index == 11) {
                    setGraph(Sensor.Sensor2, DataInfo.Magn);
                } else if (index == 12) {
                    setGraph(Sensor.Sensor3, DataInfo.Magn);
                } else if (index == 13) {
                    setGraph(Sensor.Sensor4, DataInfo.Magn);
                } else if (index == 14) {
                    setGraph(Sensor.Sensor5, DataInfo.Magn);*/
            }
            else if (index == 11)
            {
                setGraph(Sensor.none, DataInfo.StandLaySit);
            }
            else
            {
                setGraph(Sensor.none, DataInfo.none);
            }
        }

        public void setGraph(Sensor sensor, DataInfo dataInfo)
        {
            zedGraphControlAccS0.Visible = false;
            zedGraphControlAccS1.Visible = false;
            zedGraphControlAccS2.Visible = false;
            zedGraphControlAccS3.Visible = false;
            zedGraphControlAccS4.Visible = false;
            zedGraphControlGirS0.Visible = false;
            zedGraphControlGirS1.Visible = false;
            zedGraphControlGirS2.Visible = false;
            zedGraphControlGirS3.Visible = false;
            zedGraphControlGirS4.Visible = false;
            zedGraphControlThetaS0.Visible = false;
            /*zedGraphControlThetaS1.Visible = false;
            zedGraphControlThetaS2.Visible = false;
            zedGraphControlThetaS3.Visible = false;
            zedGraphControlThetaS4.Visible = false;*/
            zedGraphControlStandLaySit.Visible = false;

            switch (sensor)
            {
                case Sensor.Sensor1:
                    {
                        switch (dataInfo)
                        {
                            case DataInfo.Acc:
                                {
                                    zedGraphControlAccS0.Visible = true;
                                    break;
                                }
                            case DataInfo.Gir:
                                {
                                    zedGraphControlGirS0.Visible = true;
                                    break;
                                }
                            case DataInfo.Magn:
                                {
                                    zedGraphControlThetaS0.Visible = true;
                                    break;
                                }
                        }
                        break;
                    }
                case Sensor.Sensor2:
                    {
                        switch (dataInfo)
                        {
                            case DataInfo.Acc:
                                {
                                    zedGraphControlAccS1.Visible = true;
                                    break;
                                }
                            case DataInfo.Gir:
                                {
                                    zedGraphControlGirS1.Visible = true;
                                    break;
                                }
                                /*case DataInfo.Magn:
                                    {
                                        zedGraphControlThetaS1.Visible = true;
                                        break;
                                    }*/
                        }
                        break;
                    }
                case Sensor.Sensor3:
                    {
                        switch (dataInfo)
                        {
                            case DataInfo.Acc:
                                {
                                    zedGraphControlAccS2.Visible = true;
                                    break;
                                }
                            case DataInfo.Gir:
                                {
                                    zedGraphControlGirS2.Visible = true;
                                    break;
                                }
                                /*case DataInfo.Magn:
                                    {
                                        zedGraphControlThetaS2.Visible = true;
                                        break;
                                    }*/
                        }
                        break;
                    }
                case Sensor.Sensor4:
                    {
                        switch (dataInfo)
                        {
                            case DataInfo.Acc:
                                {
                                    zedGraphControlAccS3.Visible = true;
                                    break;
                                }
                            case DataInfo.Gir:
                                {
                                    zedGraphControlGirS3.Visible = true;
                                    break;
                                }
                                /*case DataInfo.Magn:
                                    {
                                        zedGraphControlThetaS3.Visible = true;
                                        break;
                                    }*/
                        }
                        break;
                    }
                case Sensor.Sensor5:
                    {
                        switch (dataInfo)
                        {
                            case DataInfo.Acc:
                                {
                                    zedGraphControlAccS4.Visible = true;
                                    break;
                                }
                            case DataInfo.Gir:
                                {
                                    zedGraphControlGirS4.Visible = true;
                                    break;
                                }
                                /*case DataInfo.Magn:
                                    {
                                        zedGraphControlThetaS4.Visible = true;
                                        break;
                                    }*/
                        }
                        break;
                    }
                case Sensor.none:
                    {
                        if (dataInfo == DataInfo.StandLaySit)
                            this.zedGraphControlStandLaySit.Visible = true;
                        break;
                    }
            }
        }

        private void DataGraph_Closing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult != DialogResult.Cancel)
            {
                switch (MessageBox.Show(this, "Vuoi veramente uscire dal programma?", "Chiusura programma", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    case DialogResult.No:
                        e.Cancel = true;
                        break;
                    default:
                        Program.CloseApplication();
                        break;
                }
            }
        }
    }
}