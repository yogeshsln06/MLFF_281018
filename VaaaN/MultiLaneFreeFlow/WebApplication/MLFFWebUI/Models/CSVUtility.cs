using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace MLFFWebUI.Models
{
    public class CSVUtility
    {
        public static MemoryStream GetCSV(DataTable dt)
        {
            MemoryStream stream = new MemoryStream();
            if (dt.Rows.Count > 0)
            {
                StreamWriter sw = new StreamWriter(stream);
                int iColCount = dt.Columns.Count;
                for (int i = 0; i < iColCount; i++)
                {
                    sw.Write(dt.Columns[i].ToString());
                    if (i < iColCount)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
                foreach (DataRow dr in dt.Rows)
                {
                    for (int i = 0; i < iColCount; i++)
                    {
                        if (!Convert.IsDBNull(dr[i]))
                        {
                            sw.Write(dr[i].ToString().Replace("\r\n", " "));
                        }
                        if (i < iColCount)
                        {
                            sw.Write(",");
                        }
                    }
                    sw.Write(sw.NewLine);
                }
                sw.Close();
            }
            dt.Rows.Clear();
            return stream;
        }

        public static void CreateCsvOld(string fpath, DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                StreamWriter sw = new StreamWriter(fpath, false);
                int iColCount = dt.Columns.Count;
                for (int i = 0; i < iColCount; i++)
                {
                    sw.Write(dt.Columns[i].ToString());
                    if (i < iColCount)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
                foreach (DataRow dr in dt.Rows)
                {
                    for (int i = 0; i < iColCount; i++)
                    {
                        if (!Convert.IsDBNull(dr[i]))
                        {
                            sw.Write(dr[i].ToString().Replace("\r\n", " "));
                        }
                        if (i < iColCount)
                        {
                            sw.Write(",");
                        }
                    }
                    sw.Write(sw.NewLine);
                }
                sw.Close();
            }
            dt.Rows.Clear();
        }

        public static Int16 CreateCsv(string fpath, DataTable dt)
        {
            Int16 RetVal = 0;
            if (dt.Rows.Count > 0)
            {
                StreamWriter sw = new StreamWriter(fpath, false);
                int iColCount = dt.Columns.Count;
                for (int i = 0; i < iColCount; i++)
                {
                    sw.Write(dt.Columns[i].ToString().Replace("_"," ").Replace("999","/"));
                    if (i < iColCount)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
                foreach (DataRow dr in dt.Rows)
                {
                    for (int i = 0; i < iColCount; i++)
                    {
                        if (!Convert.IsDBNull(dr[i]))
                        {
                            sw.Write(dr[i].ToString().Replace("\r\n", " "));
                        }
                        if (i < iColCount)
                        {
                            sw.Write(",");
                        }
                    }
                    sw.Write(sw.NewLine);
                }
                sw.Close();
                RetVal = 1;
            }
            dt.Rows.Clear();
            return RetVal;
        }

    }
}