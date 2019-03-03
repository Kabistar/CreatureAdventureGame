using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace CreatureGameMapEditor.Models
{
    [Serializable]
    public class Encounter : BaseModel, IXmlSerializable
    {
        public const int NumberOfEncounters = 6;


        #region Private Members
        private List<ushort> creatureIDs; // ID for each creature represented in this encounter.
        private List<ushort> relativeWeights; // Chance that any one creature is selected relative to the others.
        private ushort encounterChance; // Higher Numbers are more likely. Represents a chance out of ushort max value.
        #endregion

        #region Public Properties
        public List<ushort> CreatureIDs { get { return creatureIDs; } private set { creatureIDs = value; ChangeProperty(this, "CreatureIDs"); } }
        public List<ushort> RelativeWeights { get { return relativeWeights; } private set { relativeWeights = value; ChangeProperty(this, "RelativeWeights"); } }
        public ushort EncounterChance { get { return encounterChance; } set { encounterChance = value; ChangeProperty(this, "EncounterChance"); } }
        #endregion

        public Encounter(ushort encounterChance = 0)
        {
            this.creatureIDs = new List<ushort>(NumberOfEncounters);
            this.relativeWeights = new List<ushort>(NumberOfEncounters);
            this.encounterChance = encounterChance;

            for(int i = 0; i < NumberOfEncounters; i++)
            {
                creatureIDs.Add(ushort.MaxValue);
                relativeWeights.Add(ushort.MaxValue);
            }
        }


        private Encounter() { }


        #region Public Functions
        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("EncounterChance", encounterChance + "");
            for(int i = 0; i < Encounter.NumberOfEncounters; i++)
            {
                writer.WriteStartElement("Instance");
                writer.WriteAttributeString("CreatureID", creatureIDs[i] + "");
                writer.WriteAttributeString("Weight", relativeWeights[i] + "");
                writer.WriteEndElement();
            }
        }
        #endregion

        #region Private Functions
        #endregion
    }
}
