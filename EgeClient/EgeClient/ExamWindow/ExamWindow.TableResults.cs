using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace EgeClient
{
    public partial class ExamWindow : Window
    {
        public string SerializeGridAnswers(Grid AnswerTableGrid, int requiredRows)
        {
            var col1Values = new List<string>();
            var col2Values = new List<string>();

            // Ищем TextBox в каждой строке с данными (начиная со строки 1)
            for (int row = 1; row <= requiredRows; row++)
            {
                string val1 = GetTextBoxValue(AnswerTableGrid, row, 1); // Столбец 1
                string val2 = GetTextBoxValue(AnswerTableGrid, row, 2); // Столбец 2

                col1Values.Add(string.IsNullOrWhiteSpace(val1) ? "missed" : val1);
                col2Values.Add(string.IsNullOrWhiteSpace(val2) ? "missed" : val2);
            }

            // Объединяем значения столбцов в одну строку, разделяя их символом ';'
            return string.Join(" ", col1Values) + ";" + string.Join(" ", col2Values);
        }

        // Вспомогательная функция для поиска TextBox по позиции
        private string GetTextBoxValue(Grid grid, int row, int col)
        {
            foreach (UIElement element in grid.Children)
            {
                // Проверяем, находится ли элемент в нужной ячейке
                if (Grid.GetRow(element) == row && Grid.GetColumn(element) == col)
                {
                    // Элементы ввода у нас обернуты в Border
                    if (element is Border border)
                    {
                        // Ищем TextBox внутри Border
                        if (border.Child is TextBox textBox)
                        {
                            return textBox.Text.Trim();
                        }
                    }
                    // Если элемент TextBox был добавлен напрямую (без Border)
                    else if (element is TextBox directTextBox)
                    {
                        return directTextBox.Text.Trim();
                    }
                }
            }
            return null; // Значение не найдено
        }

        public void DeserializeGridAnswers(Grid AnswerTableGrid, int requiredRows, string serializedData)
        {
            if (string.IsNullOrWhiteSpace(serializedData)) return;

            // 1. Разделяем строку на данные для Колонок 1 и 2
            string[] parts = serializedData.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 2) return; // Неверный формат

            // 2. Разделяем каждую колонку на отдельные значения
            string[] col1Values = parts[0].Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string[] col2Values = parts[1].Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // 3. Проверяем, что количество данных соответствует количеству строк
            if (col1Values.Length < requiredRows || col2Values.Length < requiredRows)
            {
                // Недостаточно данных для восстановления всех строк
                return;
            }

            // 4. Заполняем TextBox в каждой строке
            for (int row = 1; row <= requiredRows; row++)
            {
                // Индекс массива на 1 меньше, чем индекс строки (так как массивы с 0)
                int dataIndex = row - 1;

                // Заполняем столбец 1
                SetTextBoxValue(AnswerTableGrid, row, 1, col1Values[dataIndex]);

                // Заполняем столбец 2
                SetTextBoxValue(AnswerTableGrid, row, 2, col2Values[dataIndex]);
            }
        }

        // Вспомогательная функция для поиска и установки значения TextBox
        private void SetTextBoxValue(Grid grid, int row, int col, string value)
        {
            // Заменяем 'missed' на пустую строку
            string textToSet = (value.ToLower() == "missed") ? string.Empty : value;

            foreach (UIElement element in grid.Children)
            {
                // Ищем элемент по позиции
                if (Grid.GetRow(element) == row && Grid.GetColumn(element) == col)
                {
                    // Элементы ввода у нас обернуты в Border
                    if (element is Border border)
                    {
                        if (border.Child is TextBox textBox)
                        {
                            textBox.Text = textToSet;
                            return;
                        }
                    }
                    // Если TextBox был добавлен напрямую
                    else if (element is TextBox directTextBox)
                    {
                        directTextBox.Text = textToSet;
                        return;
                    }
                }
            }
        }
    }
}
