using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Utilities
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] protected bool isModifiable;

        [SerializeField]
        private List<TKey> keys = new ();
     
        [SerializeField]
        private List<TValue> values = new ();
        // save the dictionary to lists
        public virtual void OnBeforeSerialize()
        {
            if(isModifiable) return;
            
            keys.Clear();
            values.Clear();
            foreach(KeyValuePair<TKey, TValue> pair in this)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }
     
        // load dictionary from lists
        public virtual void OnAfterDeserialize()
        {
            this.Clear();

            for(int i = 0; i < Mathf.Min(keys.Count,values.Count); i++)
                this.Add(keys[i], values[i]);
        }
    }

    public class SerializedDictionaryNonEditorModifiable<TKey, TValue> : SerializableDictionary<TKey, TValue>
    {
        [Header("You can not modify in editor, if you want to modify use SerializableDictionary instead.")]
        public bool NonModifiable = true;
        
        public override void OnBeforeSerialize()
        {
            isModifiable = false;
            NonModifiable = true;
            base.OnBeforeSerialize();
        }
    }
}