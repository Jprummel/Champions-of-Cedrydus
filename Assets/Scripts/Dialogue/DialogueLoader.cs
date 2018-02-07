using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    public class DialogueLoader : MonoBehaviour
    {
        [SerializeField] private List<TextAsset> _xmlFiles;
        private static Dictionary<string, NodeDeserializer> _nodeDictionary = new Dictionary<string, NodeDeserializer>();

        void OnEnable()
        {
            DeserializeAllFiles();
        }

        private void DeserializeAllFiles()
        {
            for (int i = 0; i < _xmlFiles.Count; i++)
            {
                if (!_nodeDictionary.ContainsKey(_xmlFiles[i].name))
                {
                    _nodeDictionary.Add(_xmlFiles[i].name, NodeDeserializer.Load(_xmlFiles[i]));
                }
            }
        }

        public static DialogueNode LoadDialogueNode(string fileName, int id)
        {
            foreach(DialogueNode dn in _nodeDictionary[fileName].DialogueNodes)
            {
                if (dn.NodeID == id)
                    return dn;
            }
            Debug.LogError("Node not found.");
            return null;
        }
    }
}