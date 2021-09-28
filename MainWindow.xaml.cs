using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            canvas1.Focus();
        }
        private double x1, y1, x2, y2, dx, dy, x, y, e;
        int click;
        List<double> coordinates = new List<double>();
        bool isBresenheim = false;
        private void Debug_Click(object sender, RoutedEventArgs e)
        {
            if(Debug.IsChecked == true)
            {
                MoveBack.Visibility = Visibility.Visible;
                MoveForward.Visibility = Visibility.Visible;
                click = 0;
            }
            else
            {
                MoveBack.Visibility = Visibility.Hidden;
                MoveForward.Visibility = Visibility.Hidden;
            }
        }

        private void canvas1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Space)
            {
                foreach (var el in canvas1.Children.OfType<Ellipse>().ToList())
                    canvas1.Children.Remove(el);
            }
        }

        private void MoveBack_Click(object sender, RoutedEventArgs e)
        {
            if (click <= 0 || canvas1.Children.OfType<Ellipse>().ToList().Count <= 0)
            {
                click = 0;
                return;
            }
            canvas1.Children.Remove(canvas1.Children.OfType<Ellipse>().ToList()[canvas1.Children.OfType<Ellipse>().ToList().Count - 1]);
            if (Vu_L.IsChecked == true)
            {
                click-=4;
            }
            else
            {
                click -= 2;
            }
        }

        private void MoveForward_Click(object sender, RoutedEventArgs e)
        {
            if (click == coordinates.Count)
                return;
            if (Vu_L.IsChecked == true)
            {
                x = coordinates.ElementAt(click);
                y = coordinates.ElementAt(click + 1);
                if (isBresenheim)
                {
                    DrawPoint();
                    click += 2;
                    return;
                }
                DrawPointWithBrightness(x, y, coordinates.ElementAt(click+2), 
                    (int)coordinates.ElementAt(click+3), (int)coordinates.ElementAt(click+4));
                click+=5;
            }
            else
            {
                x = coordinates.ElementAt(click);
                click++;
                y = coordinates.ElementAt(click);
                DrawPoint();
                click++;
            }
        }

        private void cnv_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            x1 = (double)Math.Round(e.GetPosition(null).X);
            y1 = (double)Math.Round(e.GetPosition(null).Y);
        }
        private void cnv_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            x2 = (double)Math.Round(e.GetPosition(null).X);
            y2 = (double)Math.Round(e.GetPosition(null).Y);
            if (CDA_L.IsChecked == true)
            {
                CDA();
            }
            else
                if (Brezenhem_L.IsChecked == true)
            {
                Brengheim();
            }
            else
                Vu();
        }

        private void CDA()
        {
            coordinates.Clear();
            click = 0;
            double length = Math.Max(Math.Abs(x2 - x1), Math.Abs(y2 - y1));
            if (length == 0)
                length++;
            dx = (x2 - x1) / length;
            dy = (y2 - y1) / length;
            x = (int)Math.Round(x1 + 0.5 * Math.Sign(dx));
            y = (int)Math.Round(y1 + 0.5 * Math.Sign(dy));
            Ellipse el = new Ellipse();
            if (Debug.IsChecked == false)
                DrawPoint();
            else
            {
                coordinates.Add(x);
                coordinates.Add(y);
            }
            int i = 0;
            while (i < length)
            {
                x += dx;
                y += dy;
                if(Debug.IsChecked == false)
                DrawPoint();
                else
                {
                    coordinates.Add(x);
                    coordinates.Add(y);
                }
                i++;
            }
        }

        private void Brengheim()
        {
            coordinates.Clear();
            click = 0;
            int i = 1, signx = 1, signy = 1;
            if (x2 < x1 && y2 < y1)
            {
                double reverse = x1;
                x1 = x2;
                x2 = reverse;
                reverse = y1;
                y1 = y2;
                y2 = reverse;
            }
            dx = x2 - x1;
            dy = y2 - y1;
            if (x2 < x1)
            {
                signx *= -1;
                dx *= -1;
            }
            if (y2 < y1)
            {
                signy *= -1;
                dy *= -1;
            }
            x = x1;
            y = y1;
            if (Debug.IsChecked == false)
                DrawPoint();
            else
            {
                coordinates.Add(x);
                coordinates.Add(y);
            }
            if (dx >= dy)
            {
                e = 2 * dy - dx;
                while (i <= dx)
                {
                    if (e >= 0)
                    {
                        y += signy;
                        e -= 2 * dx;
                    }
                    x += signx;
                    e += 2 * dy;
                    if (Debug.IsChecked == false)
                        DrawPoint();
                    else
                    {
                        coordinates.Add(x);
                        coordinates.Add(y);
                    }
                    i++;
                }
            }
            else
            {
                e = 2 * dx - dy;
                while (i <= dy)
                {
                    {
                        if (e >= 0)
                        {
                            x += signx;
                            e -= 2 * dy;
                        }
                        y += signy;
                        e += 2 * dx;
                        if (Debug.IsChecked == false)
                            DrawPoint();
                        else
                        {
                            coordinates.Add(x);
                            coordinates.Add(y);
                        }
                        i++;
                    }
                }
            }
        }

        private void Vu()
        {
            isBresenheim = false;
            coordinates.Clear();
            click = 0;
            int i = 1, signX = 1, signY = 1;
            if (x2 < x1 && y2 < y1)
            {
                double reverse = x1;
                x1 = x2;
                x2 = reverse;
                reverse = y1;
                y1 = y2;
                y2 = reverse;
            }
            dx = x2 - x1;
            dy = y2 - y1;
            if (x2 < x1)
            {
                signX *= -1;
                dx *= -1;
            }
            if (y2 < y1)
            {
                signY *= -1;
                dy *= -1;
            }
            double e = 2 * dy - dx;
            x = x1;
            y = y1;
            if (Debug.IsChecked == false)
                DrawPointWithBrightness(x, y, e, signX, signY);
            else
            {
                coordinates.Add(x);
                coordinates.Add(y);
                coordinates.Add(e);
                coordinates.Add(signX);
                coordinates.Add(signY);
            }
            if (x1 == x2 || y1 == y2)
            {
                if (Debug.IsChecked == false)
                    Brengheim();
                else
                {
                    isBresenheim = true;
                    Brengheim();
                }
                return;
            }
            if (dx >= dy)
            {
                e = dy / dx + 0.5;
                while (i <= dx)
                {
                    if (e >= 0)
                    {
                        y += signY;
                        e--;
                    }
                    x += signX;
                    e += dy / dx;
                    if (Debug.IsChecked == false)
                        DrawPointWithBrightness(x, y, e, signX, signY);
                    else
                    {
                        coordinates.Add(x);
                        coordinates.Add(y);
                        coordinates.Add(e);
                        coordinates.Add(signX);
                        coordinates.Add(signY);
                    }
                    i++;
                }
            }
            else
            {
                e = dx / dy - 0.5;
                while (i <= dy)
                {
                    if (e >= 0)
                    {
                        x += signX;
                        e--;
                    }
                    y += signY;
                    e += dx / dy;
                    if (Debug.IsChecked == false)
                        DrawPointWithBrightness(x, y, e, signX, signY);
                    else
                    {
                        coordinates.Add(x);
                        coordinates.Add(y);
                        coordinates.Add(e);
                        coordinates.Add(signX);
                        coordinates.Add(signY);
                    }
                    i++;
                }
            }

        }

        private void DrawPoint()
        {
            Ellipse point = new Ellipse();
            point.Width = 1;
            point.Height = 1;
            point.StrokeThickness = 1;
            point.Stroke = Brushes.Black;
            point.Margin = new Thickness(x, y, x, y);
            canvas1.Children.Add(point);
        }

        private void DrawPointWithBrightness(double x, double y, double e, int signX, int signY)
        {
            DrawPoint();
            double br = Math.Abs(e);
            byte rgbByte = 0;
            rgbByte = (byte)Math.Round(255 * (1 - br));
            Brush B = new SolidColorBrush(Color.FromRgb(rgbByte, rgbByte, rgbByte));
            Ellipse point = new Ellipse();
            point.Width = 1;
            point.Height = 1;
            point.StrokeThickness = 1;
            point.Stroke = B;
            if (e < 0)
                point.Margin = new Thickness(x - signX, y - signY, x - signX, y - signY);
            else
                point.Margin = new Thickness(x + signX, y + signY, x + signX, y + signY);
            canvas1.Children.Add(point);
        }
    }
}
