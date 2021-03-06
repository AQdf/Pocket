﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sho.Pocket.Application.Utils.Csv
{
    public class CsvWriter
    {
        private const string DELIMITER = ",";

        public string Write<T>(IList<T> list, bool includeHeader = true)
        {
            StringBuilder sb = new StringBuilder();

            Type type = typeof(T);

            PropertyInfo[] properties = type.GetProperties();

            if (includeHeader)
            {
                sb.AppendLine(CreateCsvHeaderLine(properties));
            }

            foreach (var item in list)
            {
                sb.AppendLine(CreateCsvLine(item, properties));
            }

            return sb.ToString();
        }

        public string Write<T>(IList<T> list, string fileName, bool includeHeader = true)
        {
            string csv = Write(list, includeHeader);

            WriteFile(fileName, csv);

            return csv;
        }

        private string CreateCsvHeaderLine(PropertyInfo[] properties)
        {
            List<string> propertyValues = new List<string>();

            foreach (PropertyInfo prop in properties)
            {
                string value = prop.Name;
                Attribute attribute = prop.GetCustomAttribute(typeof(DisplayAttribute));

                if (attribute != null)
                {
                    value = (attribute as DisplayAttribute).Name;
                }

                CreateCsvStringItem(propertyValues, value);
            }

            return CreateCsvLine(propertyValues);
        }

        private string CreateCsvLine<T>(T item, PropertyInfo[] properties)
        {
            List<string> propertyValues = new List<string>();

            foreach (PropertyInfo prop in properties)
            {
                object value = prop.GetValue(item, null);

                if (prop.PropertyType == typeof(string))
                {
                    CreateCsvStringItem(propertyValues, value);
                }
                else if (prop.PropertyType == typeof(string[]))
                {
                    CreateCsvStringArrayItem(propertyValues, value);
                }
                else if (prop.PropertyType == typeof(List<string>))
                {
                    CreateCsvStringListItem(propertyValues, value);
                }
                else
                {
                    CreateCsvItem(propertyValues, value);
                }
            }

            return CreateCsvLine(propertyValues);
        }

        private string CreateCsvLine(IList<string> list)
        {
            return string.Join(DELIMITER, list);
        }

        private void CreateCsvItem(List<string> propertyValues, object value)
        {
            if (value != null)
            {
                propertyValues.Add(value.ToString());
            }
            else
            {
                propertyValues.Add(string.Empty);
            }
        }

        private void CreateCsvStringListItem(List<string> propertyValues, object value)
        {
            string formatString = "\"{0}\"";
            if (value != null)
            {
                value = CreateCsvLine((List<string>)value);
                propertyValues.Add(string.Format(formatString, ProcessStringEscapeSequence(value)));
            }
            else
            {
                propertyValues.Add(string.Empty);
            }
        }

        private void CreateCsvStringArrayItem(List<string> propertyValues, object value)
        {
            string formatString = "\"{0}\"";
            if (value != null)
            {
                value = CreateCsvLine(((string[])value).ToList());
                propertyValues.Add(string.Format(formatString, ProcessStringEscapeSequence(value)));
            }
            else
            {
                propertyValues.Add(string.Empty);
            }
        }

        private void CreateCsvStringItem(List<string> propertyValues, object value)
        {
            string formatString = "\"{0}\"";
            if (value != null)
            {
                propertyValues.Add(string.Format(formatString, ProcessStringEscapeSequence(value)));
            }
            else
            {
                propertyValues.Add(string.Empty);
            }
        }

        private string ProcessStringEscapeSequence(object value)
        {
            return value.ToString().Replace("\"", "\"\"");
        }

        public bool WriteFile(string fileName, string csv)
        {
            bool fileCreated = false;

            if (!string.IsNullOrWhiteSpace(fileName))
            {
                File.WriteAllText(fileName, csv);

                fileCreated = true;
            }

            return fileCreated;
        }
    }
}
