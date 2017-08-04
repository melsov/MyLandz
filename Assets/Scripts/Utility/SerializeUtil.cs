using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

// credit: stackoverflow.com/questions/2861722/binary-serialization-and-deserialization-without-creating-files-via-strings
public static class SerializeUtil {

    public static string ToSerializedString(object o) {
        if(!o.GetType().IsSerializable) {
            return null;
        }
        using (MemoryStream stream = new MemoryStream()) {
            new BinaryFormatter().Serialize(stream, o);
            return Convert.ToBase64String(stream.ToArray());
        }
    }

    public static object FromString(string str) {
        byte[] bytes = Convert.FromBase64String(str);
        using(MemoryStream stream = new MemoryStream(bytes)) {
            return new BinaryFormatter().Deserialize(stream);
        }
    }
}
