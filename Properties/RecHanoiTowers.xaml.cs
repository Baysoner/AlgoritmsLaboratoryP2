﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LaboratoryP2.Properties
{
    public partial class RecHanoiTowers : Window
    {
        private int diskCount;
        private int stepCount;
        private List<Stack<Rectangle>> towers;

        public RecHanoiTowers()
        {
            InitializeComponent();
            towers = new List<Stack<Rectangle>> { new Stack<Rectangle>(), new Stack<Rectangle>(), new Stack<Rectangle>() };
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(DiscCountTextBox.Text, out diskCount) && diskCount > 0)
            {
                StepsTextBlock.Text = string.Empty;
                stepCount = 0; // Сброс счётчика шагов
                InitializeTowers();
                await SolveHanoi(diskCount, 0, 2, 1);
                StepsTextBlock.Text = stepCount.ToString(); // Выводим количество шагов
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите корректное количество дисков.");
            }
        }

        private void InitializeTowers()
        {
            Tower1.Children.Clear();
            Tower2.Children.Clear();
            Tower3.Children.Clear();
            for (int i = diskCount; i > 0; i--)
            {
                Rectangle disk = new Rectangle
                {
                    Width = i * 20, // Размер диска зависит от текущего индекса
                    Height = 20,
                    Fill = new SolidColorBrush(GetDiskColor(diskCount - i)), // Цвет диска
                    Margin = new Thickness(0, 0, 0, 5)
                };
                Tower1.Children.Add(disk); // Добавляем диск в первую башню
                towers[0].Push(disk);
            }
        }

        // Метод для выбора цвета дисков
        private Color GetDiskColor(int index)
        {
            switch (index)
            {
                case 0: return Colors.Red;
                case 1: return Colors.Green;
                case 2: return Colors.Blue;
                case 3: return Colors.Yellow;
                case 4: return Colors.Purple;
                // Добавьте больше цветов по мере необходимости
                default: return Colors.Black;
            }
        }


        private async Task SolveHanoi(int n, int from, int to, int aux)
        {
            if (n == 1)
            {
                await MoveDisk(from, to);
                stepCount++; // Увеличиваем количество шагов
                return;
            }

            await SolveHanoi(n - 1, from, aux, to);
            await MoveDisk(from, to);
            stepCount++; // Увеличиваем количество шагов
            await SolveHanoi(n - 1, aux, to, from);
        }


        private async Task MoveDisk(int from, int to)
        {
            if (towers[from].Count > 0)
            {
                Rectangle disk = towers[from].Peek(); // Получаем верхний диск

                // Проверяем, можно ли переместить диск на целевую башню
                if (towers[to].Count == 0 || towers[to].Peek().Width > disk.Width)
                {
                    // Удаляем диск из исходной башни
                    towers[from].Pop();

                    double targetY = towers[to].Count * 25; // Определяем целевую Y позицию

                    // Задержка для визуализации
                    await Task.Delay(500);

                    // Удаляем диск из текущего родителя
                    if (disk.Parent is Panel currentParent)
                    {
                        currentParent.Children.Remove(disk);
                    }

                    // Обновляем позицию диска и добавляем его в целевую башню
                    disk.Margin = new Thickness(0, targetY, 0, 0);
                    towers[to].Push(disk); // Добавляем диск в целевую башню
                    GetTowerPanel(to).Children.Add(disk); // Добавляем диск в визуальный элемент
                }
                else
                {
                    MessageBox.Show("Нельзя положить диск большего размера на меньший.");
                }
            }
        }



        private StackPanel GetTowerPanel(int index)
        {
            if (index == 0)
            {
                return Tower1;
            }
            else if (index == 1)
            {
                return Tower2;
            }
            else if (index == 2)
            {
                return Tower3;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Invalid tower index.");
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            StepsTextBlock.Text = string.Empty;
            DiscCountTextBox.Text = string.Empty;
            Tower1.Children.Clear();
            Tower2.Children.Clear();
            Tower3.Children.Clear();
            towers.Clear();
            towers.Add(new Stack<Rectangle>());
            towers.Add(new Stack<Rectangle>());
            towers.Add(new Stack<Rectangle>());
        }
    }
}
