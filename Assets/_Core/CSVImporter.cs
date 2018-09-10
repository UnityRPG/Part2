using System.IO;

namespace RPG.Core
{
    public class CSVImporter
    {
        string path;
        char[] seperators;
        string[] headers;
        string[][] cells;

        public CSVImporter(string path, char[] seperators = null)
        {
            this.path = path;
            if (seperators == null)
            {
                seperators = new char[] { ',' };
            }
            this.seperators = seperators;
        }

        public void Load()
        {
            string[] lines = File.ReadAllLines(path);
            headers = null;
            cells = new string[lines.Length - 1][];
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                string[] lineItems = line.Split(seperators);
                if (i == 0)
                {
                    headers = lineItems;
                    continue;
                }

                cells[i - 1] = lineItems;
            }
        }

        public string GetCell(string header, int row)
        {
            int column = GetIndexForHeader(header);
            if (column == -1)
            {
                return null;
            }

            return cells[row][column];
        }

        public int GetLength()
        {
            return cells.Length;
        }

        public int GetIndexForHeader(string header)
        {
            for (int i = 0; i < headers.Length; i++)
            {
                if (header == headers[i])
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
