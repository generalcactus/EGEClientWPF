using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EgeClient
{
    public partial class ExamWindow : Window
    {
        private void ClearTable()
        {
            // Удаляем все элементы (ячейки) из Grid
            AnswerTableGrid.Children.Clear();

            // Удаляем все определения строк
            AnswerTableGrid.RowDefinitions.Clear();
        }
        private void GenerateTableCells(int requiredRows)
        {
            // 1. Обязательно очищаем перед новой генерацией
            ClearTable();

            // 2. Создаем определения строк (RowDefinitions)
            // Row 0 - это заголовки, поэтому всего rowCount + 1 строка
            for (int i = 0; i <= requiredRows; i++)
            {
                AnswerTableGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }

            // 3. Добавляем заголовки (Row 0)
            var header1 = new TextBlock { Text = "1", HorizontalAlignment = HorizontalAlignment.Center, FontWeight = FontWeights.Bold, Margin = new Thickness(0, 5, 0, 5) };
            Grid.SetRow(header1, 0);
            Grid.SetColumn(header1, 1);
            AnswerTableGrid.Children.Add(header1);

            var header2 = new TextBlock { Text = "2", HorizontalAlignment = HorizontalAlignment.Center, FontWeight = FontWeights.Bold, Margin = new Thickness(0, 5, 0, 5) };
            Grid.SetRow(header2, 0);
            Grid.SetColumn(header2, 2);
            AnswerTableGrid.Children.Add(header2);
            // Начинаем с Row 1, так как Row 0 - это заголовки
            for (int i = 1; i <= requiredRows; i++)
            {
                // 1. Нумерация (Колонка 0)
                var numBlock = new TextBlock { Text = i.ToString(), HorizontalAlignment = HorizontalAlignment.Center };
                var numBorder = CreateTableCellBorder(numBlock);
                Grid.SetRow(numBorder, i);
                Grid.SetColumn(numBorder, 0);
                AnswerTableGrid.Children.Add(numBorder);

                // 2. Поле ввода 1 (Колонка 1)
                var textBox1 = new TextBox { BorderThickness = new Thickness(0), Padding = new Thickness(0) };
                var border1 = CreateTableCellBorder(textBox1);
                Grid.SetRow(border1, i);
                Grid.SetColumn(border1, 1);
                AnswerTableGrid.Children.Add(border1);

                // 3. Поле ввода 2 (Колонка 2)
                var textBox2 = new TextBox { BorderThickness = new Thickness(0), Padding = new Thickness(0) };
                var border2 = CreateTableCellBorder(textBox2);
                Grid.SetRow(border2, i);
                Grid.SetColumn(border2, 2);
                AnswerTableGrid.Children.Add(border2);
            }
        }

        // Вспомогательный метод для создания границ ячеек
        private Border CreateTableCellBorder(UIElement child)
        {
            return new Border
            {
                BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#dee2e6"),
                BorderThickness = new Thickness(1),
                Padding = new Thickness(5),
                Child = child
            };
        }
    }
}
