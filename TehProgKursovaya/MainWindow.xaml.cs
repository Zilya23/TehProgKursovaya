using System;
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
using System.Windows.Threading;

namespace TehProgKursovaya
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Ellipse redCircle;
        private Ellipse blueCircle;
        private double canvasWidth;
        private bool isRedCircleActive = true;
        private DispatcherTimer timer;
        private bool isFirstCycleCompleted = false; // Флаг для отслеживания завершения первого цикла
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeCircles(); // Создание кругов
            StartRace(); // Запуск анимации
        }

        private void InitializeCircles()
        {
            // Проверяем, что Canvas существует
            if (MyCanvas == null)
            {
                MessageBox.Show("Canvas не найден!");
                return;
            }

            canvasWidth = MyCanvas.ActualWidth;

            // Создаем красный круг
            redCircle = new Ellipse
            {
                Width = 50,
                Height = 50,
                Fill = Brushes.Red
            };
            Canvas.SetLeft(redCircle, 0);
            Canvas.SetTop(redCircle, (MyCanvas.ActualHeight - redCircle.Height) / 2);
            MyCanvas.Children.Add(redCircle);

            // Создаем синий круг
            blueCircle = new Ellipse
            {
                Width = 50,
                Height = 50,
                Fill = Brushes.Blue,
                Visibility = Visibility.Hidden
            };
            Canvas.SetLeft(blueCircle, canvasWidth - blueCircle.Width);
            Canvas.SetTop(blueCircle, (MyCanvas.ActualHeight - blueCircle.Height) / 2);
            MyCanvas.Children.Add(blueCircle);
        }

        private void StartRace()
        {
            // Проверяем, что круги созданы
            if (redCircle == null || blueCircle == null)
            {
                MessageBox.Show("Круги не были созданы!");
                return;
            }

            // Настраиваем таймер для анимации
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(20) // Интервал обновления
            };
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (isRedCircleActive)
            {
                MoveCircle(redCircle, 5); // Движение красного круга вправо
                if (Canvas.GetLeft(redCircle) + redCircle.Width >= canvasWidth)
                {
                    // Красный круг достиг финиша
                    redCircle.Visibility = Visibility.Hidden;
                    blueCircle.Visibility = Visibility.Visible;
                    isRedCircleActive = false;
                }
            }
            else
            {
                MoveCircle(blueCircle, -5); // Движение синего круга влево
                if (Canvas.GetLeft(blueCircle) <= 0)
                {
                    // Синий круг достиг финиша
                    blueCircle.Visibility = Visibility.Hidden;
                    redCircle.Visibility = Visibility.Visible;
                    isRedCircleActive = true;

                    // Если первый цикл завершен, останавливаем таймер
                    if (!isFirstCycleCompleted)
                    {
                        isFirstCycleCompleted = true;
                        timer.Stop(); // Останавливаем таймер
                        MessageBox.Show("Эстафета завершена!"); // Уведомление о завершении
                    }
                }
            }
        }

        private void MoveCircle(Ellipse circle, double step)
        {
            double left = Canvas.GetLeft(circle);
            left += step;
            Canvas.SetLeft(circle, left);
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            isFirstCycleCompleted = false; // Сбрасываем флаг
            Canvas.SetLeft(redCircle, 0); // Возвращаем красный круг в начальную позицию
            Canvas.SetLeft(blueCircle, canvasWidth - blueCircle.Width); // Возвращаем синий круг в начальную позицию
            redCircle.Visibility = Visibility.Visible; // Показываем красный круг
            blueCircle.Visibility = Visibility.Hidden; // Скрываем синий круг
            isRedCircleActive = true; // Начинаем с красного круга
            timer.Start(); // Запускаем таймер
        }
    }
}
