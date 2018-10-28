using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace VaaaN.MLFF.Libraries.CommonLibrary.Serialization
{
    public class Serialization
    {
        /// <summary>
        /// Writes the object value in to the specified xml file.
        /// </summary>
        /// <param name="type">Type of the object</param>
        /// <param name="fileName">File name to which value to be written.</param>
        /// <param name="obj">Object whoes value to be written in to the xml file.</param>
        public static void Serialize(Type type, string fileName, object obj)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(type);
                using (TextWriter writer = new StreamWriter(fileName))
                {
                    serializer.Serialize(writer, obj);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Reads the value from the specified xml file in to the class object.
        /// </summary>
        /// <param name="type">Type of the object.</param>
        /// <param name="fileName">File name from which data to be read.</param>
        /// <returns>Returns the object with values from the xml file.</returns>
        public static object Deserialize(Type type, string fileName)
        {
            try
            {
                object obj = new object();
                XmlSerializer serializer = new XmlSerializer(type);

                using (FileStream fs = new FileStream(fileName, FileMode.Open))
                {
                    obj = serializer.Deserialize(fs);
                }

                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
