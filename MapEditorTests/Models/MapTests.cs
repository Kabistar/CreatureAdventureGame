using Microsoft.VisualStudio.TestTools.UnitTesting;
using CreatureGameMapEditor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace CreatureGameMapEditor.Models.Tests
{
    [TestClass()]
    public class MapTests
    {
        [TestMethod()]
        public void SerializationTest1()
        {
            Map map = new Map(20, 35, 35);

            FileStream stream = File.Create("SerializationTest1.xml");
            XmlSerializer xml = new XmlSerializer(map.GetType());
            xml.Serialize(stream, map);
            stream.Close();

            stream = File.Open("SerializationTest1.xml",FileMode.Open);
            Map mapRead = (Map)xml.Deserialize(stream);
            stream.Close();

            Assert.Inconclusive();
        }
    }
}