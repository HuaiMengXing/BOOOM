namespace LitJson
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using LitJson;
    using System;
    using UnityEditor;
    using UnityEngine.Tilemaps;

    public class LitJsonExtend
    {
        public LitJsonExtend()
        {
            joinV3Type();
            joinV3IntType();

        }
        static void joinV3Type()
        {
            Action<Vector3, JsonWriter> writeType = (v, w) => {
                w.WriteObjectStart();//开始写入对象

                w.WritePropertyName("x");//写入属性名
                w.Write((double)v.x);//写入值

                w.WritePropertyName("y");
                w.Write((double)v.y);

                w.WritePropertyName("z");
                w.Write((double)v.z);

                w.WriteObjectEnd();
            };

            JsonMapper.RegisterExporter<Vector3>((v, w) => {
                writeType(v, w);
            });
        }


        /// <summary>
        /// List<Vector3Int> 序列化Vector3Int
        /// </summary>
        static void joinV3IntType()
        {
            Action<Vector3Int, JsonWriter> writeType = (v, w) => {
                w.WriteObjectStart();//开始写入对象

                w.WritePropertyName("x");//写入属性名
                w.Write(v.x.ToString());//写入值

                w.WritePropertyName("y");
                w.Write(v.y.ToString());

                w.WritePropertyName("z");
                w.Write(v.z.ToString());

                w.WriteObjectEnd();
            };

            JsonMapper.RegisterExporter<Vector3Int>((v, w) => {
                writeType(v, w);
            });
        }
    }
}

