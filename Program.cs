using System;
using System.IO;
using System.Text;

namespace ОбработчикCSV
{
    public enum DataType
    {
        Целое,
        Вещественное,
        ОднострочныйТекст,
        МнострочныйТекст,
        СсылкаВСетиИнтернет,
        Булево,
        ДатаВремя,
        Тег,
        Пустое
    }

    public class CsvColumn
    {
        public DataType DataType { get; set; }
        public bool IsFinalized { get; set; }

        public CsvColumn(DataType dataType)
        {
            DataType = dataType;
            /*IsFinalized = false;*/
        }
    }

    public class CsvFileProcessor
    {
        public static void ProcessCsvFile(string filePath)
        {
            using (var reader = new StreamReader(filePath, Encoding.UTF8))
            {
                var headers = reader.ReadLine()?.Split(';');
                var dataTypes = new CsvColumn[headers.Length];

                for (int i = 0; i < headers.Length; i++)
                {
                    dataTypes[i] = new CsvColumn(DataType.Пустое);
                }
                /*dataTypes[i].IsFinalized = true;*/
                //Вместо i впишите номер столбца, обработку которого хотите пропустить

                var count = 0;
                while (!reader.EndOfStream && count < 1000)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');

                    for (int i = 0; i < values.Length; i++)
                    {
                        /*if (!dataTypes[i].IsFinalized)
                        {*/
                            if (IsNull(values[i]))
                            {
                                continue;
                            }
                            else if (IsInteger(values[i]))
                            {
                                if (dataTypes[i].DataType == DataType.МнострочныйТекст)
                                {
                                    continue;
                                }
                                else if (dataTypes[i].DataType == DataType.Вещественное)
                                {
                                    continue;
                                }
                                else if (dataTypes[i].DataType != DataType.Целое && dataTypes[i].DataType != DataType.Пустое)
                                {
                                    dataTypes[i].DataType = DataType.ОднострочныйТекст;
                                }
                                else
                                {
                                    dataTypes[i].DataType = DataType.Целое;
                                }
                            }
                            else if (IsFloat(values[i]))
                            {
                                if (dataTypes[i].DataType == DataType.МнострочныйТекст)
                                {
                                    continue;
                                }
                                else if (dataTypes[i].DataType != DataType.Целое && dataTypes[i].DataType != DataType.Вещественное && dataTypes[i].DataType != DataType.Пустое)
                                {
                                    dataTypes[i].DataType = DataType.ОднострочныйТекст;
                                }
                                else
                                {
                                    dataTypes[i].DataType = DataType.Вещественное;
                            }
                        }
                            else if (IsInternetLink(values[i]))
                            {
                                if (dataTypes[i].DataType == DataType.МнострочныйТекст)
                                {
                                    continue;
                                }
                                else if (dataTypes[i].DataType != DataType.СсылкаВСетиИнтернет && dataTypes[i].DataType != DataType.Пустое)
                                {
                                    dataTypes[i].DataType = DataType.ОднострочныйТекст;
                                }
                                else
                                {
                                    dataTypes[i].DataType = DataType.СсылкаВСетиИнтернет;
                                }
                            }
                            else if (IsBoolean(values[i]))
                            {
                                if (dataTypes[i].DataType == DataType.МнострочныйТекст)
                                {
                                    continue;
                                }
                                else if (dataTypes[i].DataType != DataType.Булево && dataTypes[i].DataType != DataType.Пустое)
                                {
                                    dataTypes[i].DataType = DataType.ОднострочныйТекст;
                                }
                                else
                                {
                                    dataTypes[i].DataType = DataType.Булево;
                                }
                        }
                            else if (IsDateTime(values[i]))
                            {
                                if (dataTypes[i].DataType == DataType.МнострочныйТекст)
                                {
                                    continue;
                                }
                                else if (dataTypes[i].DataType != DataType.ДатаВремя && dataTypes[i].DataType != DataType.Пустое )
                                {
                                    dataTypes[i].DataType = DataType.ОднострочныйТекст;
                                }
                                else
                                {
                                    dataTypes[i].DataType = DataType.ДатаВремя;
                                }
                            }
                            else if (IsTag(values[i]))
                            {
                                if (dataTypes[i].DataType == DataType.МнострочныйТекст)
                                {
                                    continue;
                                }
                                else if (dataTypes[i].DataType != DataType.Тег && dataTypes[i].DataType != DataType.Пустое)
                                {
                                    dataTypes[i].DataType = DataType.ОднострочныйТекст;
                                }
                                else
                                {
                                    dataTypes[i].DataType = DataType.Тег;
                                }
                            }
                            else if (IsSingleLineText(values[i]))
                            {
                                if (dataTypes[i].DataType == DataType.МнострочныйТекст)
                                {
                                    continue;
                                }
                                else
                                {
                                    dataTypes[i].DataType = DataType.ОднострочныйТекст;
                                }
                            }
                            else if (IsMultiLineText(values[i]))
                            {
                                dataTypes[i].DataType = DataType.МнострочныйТекст;
                            }
                       /* }*/
                    }

                    count++;
                }

                for (int i = 0; i < dataTypes.Length; i++)
                {
                    Console.WriteLine($"Колонка '{headers[i]}' имеет тип данных {dataTypes[i].DataType}");
                }
            }
        }

        private static bool IsNull(string value)
        {
            if(value == "")
            {
                return true;
            }
            else
            {
                return false;   
            }
        }
        private static bool IsInteger(string value)
        {
            int result;
            return int.TryParse(value, out result);
        }

        private static bool IsFloat(string value)
        {
            float result;
            return float.TryParse(value, out result);
        }

        private static bool IsSingleLineText(string value)
        {
            return !string.IsNullOrEmpty(value) && !value.Contains(Environment.NewLine);
        }

        private static bool IsMultiLineText(string value)
        {
            return !string.IsNullOrEmpty(value) && value.Contains(Environment.NewLine);
        }

        private static bool IsInternetLink(string value)
        {
            Uri uriResult;
            return Uri.TryCreate(value, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        private static bool IsBoolean(string value)
        {
            bool result;
            return bool.TryParse(value, out result);
        }

        private static bool IsDateTime(string value)
        {
            DateTime dateResult;
            return DateTime.TryParse(value, out dateResult);
        }

        private static bool IsTag(string value)
        {
            return !string.IsNullOrEmpty(value) && value.StartsWith("#");
        }
    }


    internal class Program
    {
        static void Main(string[] args)
        {
            string filePath = "Путь к файлу";
            CsvFileProcessor.ProcessCsvFile(filePath);
            Console.ReadLine();
        }
    }
}