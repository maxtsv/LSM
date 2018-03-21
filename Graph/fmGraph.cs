using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Graph
{
    public partial class fmGraph : Form
    {
        int scale = 40;
        float dash = 6;
        const int N = 5;                // points fixed amount
        const double STEP = 0.01;       // step on interpolation
        const int BEGIN = 0;            // drawing range begin 
        const int END = 6;              // drawing range end 
        Point O;                        
        PointF[] Points = new PointF[N];
        int MovingPointIndex = -1;      // represents state of input: -1 when not dragging point
        int ManualPointIndex = -1;      // represents state of input: -1 when not inputting point



        public PointF PixToGrid(PointF p)
        {
            p.X = (p.X - O.X) / scale;
            p.Y = (O.Y - p.Y) / scale;
            return p;
        }

        public PointF GridToPix(PointF p)
        {
            p.X = O.X + p.X * scale;
            p.Y = O.Y - p.Y * scale;
            return p;
        }

        public fmGraph()
        {
           InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g;
            g = e.Graphics;
            DrawGrid(g);
            DrawPoints(g, Points);


            if (ManualPointIndex == -1)
            {
                DrawLagrangePol(g, Points);
                DrawLSM(g, Points);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            SetCenter();
            this.Invalidate();
        }

        protected void SetCenter()
        {
            try
            {
                O.X = fmGraph.ActiveForm.Width / 2;
                O.Y = fmGraph.ActiveForm.Height / 2;
                //scale = fmGraph.ActiveForm.Height / 20;
            }
            catch (Exception)  // form is minimised
            {
                O.X = 0;
                O.Y = 0;
            }
        }

        protected void DrawGrid(Graphics g)  
        {
            Pen pen = new Pen(Color.Black);
            pen.Width = 1;

            Font font = new Font("Arial", 13, FontStyle.Bold);
            SolidBrush brush = new SolidBrush(Color.Black);

            g.DrawString("X", font, brush, new PointF(O.X * 2 - scale, O.Y));
            g.DrawString("0", font, brush, O);
            g.DrawString("Y", font, brush, new PointF(O.X, scale));

            g.DrawLine(pen, O.X, 0, O.X, O.Y * 2);
            g.DrawLine(pen, 0, O.Y, O.X * 2, O.Y);

            int M = Math.Max(O.X / scale, O.Y / scale);
            int i = -M;
            while (i < M)
            {
                g.DrawLine(pen, GridToPix(new PointF(i, -dash / scale)), GridToPix(new PointF(i, dash / scale)));
                g.DrawLine(pen, GridToPix(new PointF(-dash / scale, -i)), GridToPix(new PointF(dash / scale, -i)));
                if ((i > 0) && (i <= 5))
                    g.DrawString(i.ToString(), font, brush, GridToPix(new PointF(i, 0)));
                i += 1;
            }
        }

        protected void DrawPoints(Graphics g, PointF[] Ps)
        {
            Pen pen = new Pen(Color.Red);
            pen.Width = dash;

            int k = 1;                    // Koeff for increasing moving point

            for (int c = 0; c < Ps.Length; c++)
            {
                pen.Color = ((c == MovingPointIndex)||(c == ManualPointIndex)) ? (Color.Blue) : (Color.Red);
                k = (c == MovingPointIndex) ? 2 : 1;

                g.DrawEllipse(pen, O.X + scale * Ps[c].X - dash * k / 2, O.Y - scale * Ps[c].Y - dash * k / 2, dash * k, dash * k);
                // g.DrawEllipse(pen, 
                //               new RectangleF(GridToPix(new PointF(Ps[c].X - dash * k / (2 * scale) , 
                //                                                   Ps[c].Y + dash * k / (2 * scale))), 
                //                              new SizeF(dash * k, dash * k)));             
            }
        }

        protected void DrawLagrangePol(Graphics g, PointF[] Ps)
        {
            try
            {
                Pen pen = new Pen(Color.Orange);
                pen.Width = 2;
                Font font = new Font("Arial", 11, FontStyle.Bold);
                SolidBrush brush = new SolidBrush(pen.Color);
                g.DrawString("——————————  Lagrange Polynomial", font, brush, new PointF(0, O.Y*2 - 32*dash));

                double x = BEGIN;
                PointF P0 = new PointF((float)x, (float)LagrangePolynom(x, Ps));
                PointF P1 = P0;

                while (x <= END)
                {
                    P1 = new PointF((float)x, (float)LagrangePolynom(x, Ps));
                    g.DrawLine(pen, GridToPix(P0), GridToPix(P1));
                    P0 = P1;
                    x += STEP;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        static double LagrangePolynom(double x, PointF[] Ps)
        {
            double Polynom = 0;
            int size = Ps.Length;
            for (int i = 0; i < size; i++)
            {
                double Base = 1;
                for (int j = 0; j < size; j++)
                {
                    if (j != i)
                    {
                        Base *= (x - Ps[j].X) / (Ps[i].X - Ps[j].X);
                    }
                }
                Polynom += Base * Ps[i].Y;
            }
            return Polynom;
        }

        protected void DrawLSM(Graphics g, PointF[] Ps)
        {
            try
            {
                if (Ps.Length == 0)
                    return;
                Pen pen = new Pen(Color.Green);
                pen.Width = 2;
                Font font = new Font("Arial", 11, FontStyle.Bold);
                SolidBrush brush = new SolidBrush(pen.Color); 
                int n = 4;  // degree of 3

                double[] K = GetPolynomKoeff(Ps, n);               

                string txt = "——————————  LSM Polynomial degree of " + (n - 1).ToString()+":\n";
               // for (int i = K.Length-1; i >=0; i--)
               //     txt += (new String(' ', 39))+"K" + i.ToString() + " = " + K[i].ToString() + "\n";
                g.DrawString(txt, font, brush, new PointF(0, O.Y * 2 - 28 * dash));

                double x = BEGIN;
                PointF P0, P1;
                P0 = new PointF(0, (float)K[0]);

                while (x <= END)
                {
                    double Fx = 0;
                    for (int i = 0; i < n; i++)
                        Fx += Math.Pow(x, i) * K[i];

                    P1 = new PointF((float)x, (float)Fx);
                    g.DrawLine(pen, GridToPix(P0), GridToPix(P1));
                    P0 = P1;

                    x += STEP;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }



        private double[] GetPolynomKoeff(PointF[] Ps, int n)
        {
            //Cramer's method
            double baseDet = CalcDeterminant(PointsToMarix(Ps, -1),n);
            double[] K = new double[n];
            for (int k = 0; k < n; k++)
            {
                double Det = CalcDeterminant(PointsToMarix(Ps, k),n);
                K[k] = Math.Round(Det / baseDet, 5);               
            }
            return K;
        }

        private double[,] PointsToMarix(PointF[] P, int K)
        {
            //Creating matrixes for Cramer's method:
            //K is number of matrix's column, which replacing with system's left side
            int n = P.Length;
            double[,] M = new double[n, n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if ((i == 0) && (j == 0))
                        M[0, 0] = n;

                    if ((K > -1) && (j == K))
                        M[i, K] = P.Sum(x => x.Y * Math.Pow((double)x.X, i));
                    else
                        M[i, j] = P.Sum(x => Math.Pow((double)x.X, (i + j)));
                }
            }
            return M;
        }

        public static double CalcDeterminant(double[,] M, int n)
        {
            // calc detrminant by multiplying first row's element 
            // by determinant of the matrix which is not in element's row\col
            double d = 0;
            int k, i, j, subi, subj;
            double[,] subM = new double[n, n];
            if (n == 2)
            {
                return ((M[0, 0] * M[1, 1]) - (M[1, 0] * M[0, 1]));
            }
            else
            {
                for (k = 0; k < n; k++)
                {
                    subi = 0;
                    for (i = 1; i < n; i++)
                    {
                        subj = 0;
                        for (j = 0; j < n; j++)
                        {
                            if (j == k)                            
                                continue;
                                                        
                            subM[subi, subj] = M[i, j];
                            subj++;
                        }
                        subi++;
                    }
                    d += Math.Pow(-1, k) * M[0, k] * CalcDeterminant(subM, n - 1);
                }
            }
            return d;
        }

        protected void CreateTable(PointF[] P)
        {
            //creating table and grid
            DataTable table = new DataTable();
            table.Columns.Add("X", typeof(float));
            table.Columns.Add("Y", typeof(float));
            for (int i = 0; i < P.Length; i++)
            {
                table.Rows.Add();
                table.Rows[i]["X"] = Math.Round(P[i].X, 2);
                table.Rows[i]["Y"] = Math.Round(P[i].Y, 2);
            }
            grPoints.DataSource = table;
            grPoints.Columns["X"].ReadOnly = true;
            grPoints.Columns["X"].Width = 40;
            grPoints.Columns["Y"].Width = 40;
        }

        protected void RefreshTable(PointF[] P)
        { 
            for (int i = 0; i < P.Length; i++)
            {             
               grPoints.Rows[i].Cells["X"].Value = Math.Round(P[i].X,2);
               grPoints.Rows[i].Cells["Y"].Value = Math.Round(P[i].Y,2);
            }            
        }

        protected void RefreshPoints(PointF[] Ps)
        {
            for (int c = 0; c < Ps.Length; c++)
            {
                Ps[c].X = (float)grPoints.Rows[c].Cells["X"].Value;
                Ps[c].Y = (float)grPoints.Rows[c].Cells["Y"].Value;
            }
        }

        private void fmGraph_MouseDown(object sender, MouseEventArgs e)
        {
            if (ManualPointIndex > -1)
                MovingPointIndex = ManualPointIndex;
             //searhing for the points around mouse_down position            
            int i = 0;
            float d = 0.5F;
            PointF mPos = PixToGrid(new PointF(e.X, e.Y));

            while (i < Points.Length)
            {
                if (((Points[i].X <= mPos.X + d) & (Points[i].X >= mPos.X - d)) &
                    ((Points[i].Y <= mPos.Y + d) & (Points[i].Y >= mPos.Y - d)))
                {
                    MovingPointIndex = i;
                    break;
                }
                i += 1;
            }
            // if manual adding mode - placing new point
            if (ManualPointIndex > -1)
            {
                Array.Resize(ref Points, ManualPointIndex + 1);
                Points[ManualPointIndex] = PixToGrid(new Point((ManualPointIndex + 1) * scale + O.X, e.Y));
                RefreshTable(Points);
                ManualPointIndex++;

                if (ManualPointIndex == N)
                    ManualPointIndex = -1;
            } 
            this.Invalidate();
        }

        private void fmGraph_MouseMove(object sender, MouseEventArgs e)
        {
            if (MovingPointIndex > -1)
            {
                //if got moving point, locking cursor by Y axis
                float tmpX = Points[MovingPointIndex].X;
                Cursor.Clip = this.RectangleToScreen(new Rectangle((int)(tmpX * scale + O.X), 0, 1, O.Y * 2));

                Points[MovingPointIndex] = PixToGrid(new PointF(e.X, e.Y));
                Points[MovingPointIndex].X = tmpX;
                RefreshTable(Points);

                this.Invalidate();
            }
            // if manual adding mode, locking cursor by Y axis in X position 
            if (ManualPointIndex > -1)
                Cursor.Clip = this.RectangleToScreen(new Rectangle((int)((ManualPointIndex+1) * scale + O.X), 0, 1, O.Y * 2));
            else
                Cursor.Clip = Rectangle.Empty;
        }

        private void fmGraph_MouseUp(object sender, MouseEventArgs e)
        {        
                if (MovingPointIndex > -1)
                {
                    //releasing cursor, leaving point
                    Cursor.Clip = Rectangle.Empty;
                    MovingPointIndex = -1;
                    this.Invalidate();
                }
                this.Invalidate();           
        }

        private void fmGraph_Shown(object sender, EventArgs e)
        {
            SetCenter();
            // sample values
            Points[0] = new PointF(1, 3);
            Points[1] = new PointF(2, 2);
            Points[2] = new PointF(3, 4);
            Points[3] = new PointF(4, 5);
            Points[4] = new PointF(5, 5);

            CreateTable(Points);

            this.Invalidate();
        }

        private void grPoints_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            
            RefreshPoints(Points);
            this.Invalidate();
        }
        private void grPoints_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //Handling type error  
        }

        private void miAddPoints_Click(object sender, EventArgs e)
        {
            if (ManualPointIndex == -1)
            {
                for (int i = 0; i < Points.Length; i++)
                    grPoints[1, i].Value = "0";
                ManualPointIndex = 0;
                Array.Resize(ref Points, ManualPointIndex);
            }
            this.Invalidate();
        }

    }  

}
