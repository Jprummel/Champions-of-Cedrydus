using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

namespace Dialogue
{
    [XmlRoot("Nodes")]
    public class NodeDeserializer
    {
        //put all messages in one list
        [XmlArray("DialogueNodes")]
        [XmlArrayItem("DialogueNode")]
        public List<DialogueNode> DialogueNodes = new List<DialogueNode>();

        //Function to load the file and deserialize the xml file
        public static NodeDeserializer Load(TextAsset xmlFile)
        {
            TextAsset xml = xmlFile;

            if (xml == null)
            {
                Debug.LogError("No XML file found!");
            }

            //create XmlSerializer
            XmlSerializer serializer = new XmlSerializer(typeof(NodeDeserializer));

            //create StringReader
            StringReader reader = new StringReader(xml.text);

            //store and deserialize dialogues into a dialogues
            NodeDeserializer dialogues = serializer.Deserialize(reader) as NodeDeserializer;

            //close reader
            reader.Close();

            //return messages
            return dialogues;
        }
    }

}