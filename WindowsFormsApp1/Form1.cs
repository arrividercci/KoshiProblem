using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeChart();

        }
        private double F(double t, double y)
        {
            return t - y * y;
        }
        private double[,] RungeKuttaTwo(double a, double b, double y0, double epsilone)
        {
            var n = (b - a) / epsilone;
            double[,] result = new double[(int)n, 2];
            var h = (b - a) / n;
            result[0, 0] = a;
            result[0, 1] = y0;
            for (int i = 0; i < n - 1; i++)
            {
                result[i + 1, 0] = result[i, 0] + h;//Xi+1
                var k1 = h * F(result[i, 0], result[i, 1]);
                var k2 = h * F(result[i, 0] + h / 2, result[i, 1] + k1 / 2);
                result[i + 1, 1] = result[i, 1] + k2;
            }
            return result;
        }
        private double[,] EulerMethod4(double a, double b, double y0, int n, double h)
        {
            double[,] result = new double[n, 2];
            result[0, 0] = a;
            result[0, 1] = y0;
            for (int i = 0; i < 3; i++)
            {
                result[i + 1, 0] = result[i, 0] + h;
                result[i + 1, 1] = result[i, 1] + h * F(result[i, 0], result[i, 1]);
            }
            return result;
        }


        private double[,] AdamsBashforthMoulton(double a, double b, double y0, double eps)
        {
            var n = (b - a) / eps;
            var h = (b - a) / n;
            var result = EulerMethod4(a, b, y0, (int)n, h);
            for (int i = 3; i < n - 1; i++)
            {
                var p = result[i, 1] + h / 24 * (-9 * F(result[i - 3, 0], result[i - 3, 1]) + 37 * F(result[i - 2, 0], result[i - 2, 1]) - 59 * F(result[i - 1, 0], result[i - 1, 1]) + 55 * F(result[i, 0], result[i, 1]));
                result[i + 1, 0] = result[i, 0] + h;
                result[i + 1, 1] = result[i, 1] + h / 24 * (F(result[i - 2, 0], result[i - 2, 1]) - 5 * F(result[i - 1, 0], result[i - 1, 1]) + 19 * F(result[i, 0], result[i, 1]) + 9 * F(result[i + 1, 0], p));
            }

            return result;
        }
        private void InitializeChart()
        {
            // Создаем объект для графика
            Chart chart1 = new Chart();
            chart1.Dock = DockStyle.Left;
            
            Chart chart2 = new Chart();
            chart2.Dock = DockStyle.Right;
            // Создаем область для графика
            ChartArea chartArea1 = new ChartArea();
            ChartArea chartArea2 = new ChartArea();
            chart1.ChartAreas.Add(chartArea1);
            chart2.ChartAreas.Add(chartArea2);

            // Создаем ряд для графика
            Series series1 = new Series();
            Series series2 = new Series();
            
            series1.ChartType = SeriesChartType.Line;
            series1.Color = Color.Red;
            series2.ChartType = SeriesChartType.Line;
            series2.Color = Color.Blue;
            double a = 1;
            double b = 4;
            double eps = 1e-4;
            double y0 = 1;
            var rungeKutta = RungeKuttaTwo(a, b, y0, eps);
            var adamsBashforthMoulton = AdamsBashforthMoulton(a, b, y0, eps);
            for(int i = 0; i < adamsBashforthMoulton.GetLength(0); i++)
            {
                series2.Points.Add(new DataPoint(adamsBashforthMoulton[i,0], adamsBashforthMoulton[i, 1]));
            }
            for (int i = 0; i < rungeKutta.GetLength(0); i++)
            {
                series1.Points.Add(new DataPoint(rungeKutta[i,0], rungeKutta[i,1]));
            }
            // Добавляем точки на график

            // Добавляем ряд на график
            chart1.Series.Add(series1);
            chart2.Series.Add(series2);

            // Добавляем график на форму
            Controls.Add(chart1);
            Controls.Add(chart2);
        }
    }
}
