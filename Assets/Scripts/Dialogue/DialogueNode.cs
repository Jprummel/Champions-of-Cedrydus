using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

namespace Dialogue
{
    public class DialogueNode
    {
        [XmlElement("NodeID")]
        public int NodeID { get; set; }
        
        [XmlElement("DialogueSource")]
        public string DialogueSource { get; set; }
        
        [XmlElement("Text")]
        public string Text { get; set; }

        [XmlElement("DestinationNodeID")]
        public int NodeDestination;
    }
}