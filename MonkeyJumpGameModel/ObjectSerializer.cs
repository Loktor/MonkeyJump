using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Runtime.Serialization;
using System.IO;


namespace MonkeyJumpGameModel
{
    /// <summary>
    /// Class to serialize/deserialize objects of type T to an stream
    /// </summary>
    public class ObjectSerializer<T>
    {
        public void Serialize(Stream streamObject,T objToSerialize)
        {
            if (streamObject == null) return;

            DataContractSerializer ser = new DataContractSerializer(typeof(T));
            ser.WriteObject(streamObject, objToSerialize);
        }

        public T Deserialize(Stream streamObject)
        {
            if (streamObject == null) return default(T);

            DataContractSerializer ser = new DataContractSerializer(typeof(T));
            return (T)ser.ReadObject(streamObject);
        }
    }
}
