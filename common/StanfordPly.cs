// author: Josef Pelikan

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Text;
using OpenTK;
using System.Threading;

namespace Scene3D
{
    /// <summary>
    /// Stanford Triangle Format / Polygon File Format / PLY.
    /// </summary>
    /// <see cref="http://paulbourke.net/dataformats/ply/">File-format overview</see>
    public class StanfordPly
    {
        #region Local types

        /// <summary>
        /// File-subformat (text | binary).
        /// </summary>
        public enum Format
        {
            ASCII,
            BINARY_LE,
            BINARY_BE
        }

        /// <summary>
        /// Element property types.
        /// </summary>
        public enum PropertyType
        {
            /// 1 byte signer integer
            CHAR,
            /// 1 byte unsigned integer
            UCHAR,
            /// 2 byte signed integer
            SHORT,
            /// Two byte unsigned integer
            USHORT,
            /// Four byte signed integer
            INT,
            /// Four byte unsigned integer
            UINT,
            /// four byte floating point number
            FLOAT,
            /// Eight byte byte floating point number
            DOUBLE
        }

        private static Dictionary<string, PropertyType> NameToPropertyType;

        public static PropertyType PropertyTypeFromName(string name)
        {
            if (NameToPropertyType.ContainsKey(name))
                return NameToPropertyType[name];
            return PropertyType.INT;
        }

        /// <summary>
        /// Element property description object.
        /// </summary>
        class PropertyDescr
        {
            /// <summary>
            /// Property name.
            /// </summary>
            public string Name { get; set; }

            public PropertyType DataType { get; set; }

            public PropertyDescr(string name, string type)
            {
                Name = name;
                DataType = PropertyTypeFromName(type);
            }
        }

        /// <summary>
        /// Element description object.
        /// </summary>
        class ElementDescr
        {
            /// <summary>
            /// Element name.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// List of properties which belong to this element type
            /// </summary>
            public List<PropertyDescr> Properties { get; set; }

            /// <summary>
            /// Number of element entries
            /// </summary>
            public int Count { get; set; }

            public ElementDescr(int count, string name)
            {
                Properties = new List<PropertyDescr>();
                Count = count;
                Name = name;
            }

            public void FinishProperties()
            {
                propMap.Clear();
                for (int i = 0; i < Properties.Count; i++)
                    propMap[Properties[i].Name] = i;
            }

            public int PropIndex(string name)
            {
                int ind = 0;
                if (propMap.TryGetValue(name, out ind))
                    return ind;
                return -1;
            }

            private Dictionary<string, int> propMap = new Dictionary<string, int>();
        }

        #endregion

        #region Constants

        /// <summary>
        /// The first line (including the ending character CR) is used as a 4-byte magic number.
        /// </summary>
        const string HEADER = "ply";

        /// <summary>
        /// Comments are introduced by this string.
        /// </summary>
        const string COMMENT = "comment";

        const string FORMAT = "format";

        const string FORMAT_TEXT = "format ascii 1.0";

        const string FORMAT_BINARY_LE = "format binary_little_endian 1.0";

        const string FORMAT_BINARY_BE = "format binary_big_endian 1.0";

        const string ELEMENT = "element";

        const string VERTEX = "vertex";

        const string FACE = "face";

        const string PROPERTY = "property";

        const string NORMAL_X = "nx";

        const string NORMAL_Y = "ny";

        const string NORMAL_Z = "nz";

        const string TEXTURE_S = "s";

        const string TEXTURE_T = "t";

        const string COLOR_R = "r";

        const string COLOR_G = "g";

        const string COLOR_B = "b";

        const string COLOR_RED = "red";

        const string COLOR_GREEN = "green";

        const string COLOR_BLUE = "blue";

        const string END_HEADER = "end_header";

        public static char[] DELIMITERS = { ' ', '\t' };

        static StanfordPly()
        {
            NameToPropertyType = new Dictionary<string, PropertyType>();
            NameToPropertyType["char"] = PropertyType.CHAR;
            NameToPropertyType["uchar"] = PropertyType.UCHAR;
            NameToPropertyType["short"] = PropertyType.SHORT;
            NameToPropertyType["ushort"] = PropertyType.USHORT;
            NameToPropertyType["int"] = PropertyType.INT;
            NameToPropertyType["uint"] = PropertyType.UINT;
            NameToPropertyType["float"] = PropertyType.FLOAT;
            NameToPropertyType["double"] = PropertyType.DOUBLE;
            NameToPropertyType["int8"] = PropertyType.CHAR;
            NameToPropertyType["uint8"] = PropertyType.UCHAR;
            NameToPropertyType["int16"] = PropertyType.SHORT;
            NameToPropertyType["uint16"] = PropertyType.USHORT;
            NameToPropertyType["int32"] = PropertyType.INT;
            NameToPropertyType["uint32"] = PropertyType.UINT;
            NameToPropertyType["float32"] = PropertyType.FLOAT;
            NameToPropertyType["float64"] = PropertyType.DOUBLE;
        }

        #endregion

        #region Instance data

        /// <summary>
        /// General set of 'elements'. Recognized elements are: 'vertex' and 'face'.
        /// </summary>
        private List<ElementDescr> elements;

        /// <summary>
        /// Format of the file can be
        /// Ascii 1.0, format binary_little_endian 1.0, format binary_big_endian 1.0
        /// </summary>
        private Format format;

        /// <summary>
        /// Reported number of mesh vertices.
        /// </summary>
        private int vertices;

        public bool DoNormals { get; set; }

        public bool DoTxtCoords { get; set; }

        public bool DoColors { get; set; }

        public Matrix4 matrix = Matrix4.Identity;

        /// <summary>
        /// Change axis orientation in import/export.
        /// </summary>
        public bool Orientation { get; set; }

        public bool TextFormat { get; set; }

        public bool NativeNewLine { get; set; }


        #endregion

        public StanfordPly()
        {
            Orientation = false;
            TextFormat = true;
            NativeNewLine = true;
            DoNormals = false;
            DoTxtCoords = false;
            DoColors = false;
        }

        /// <summary>
        /// Reads one data element using the provided reader.
        /// </summary>
        /// <returns>True if ok.</returns>
        private bool ReadElementText(StreamReader reader, ElementDescr descr, List<object> element)
        {
            string line = reader.ReadLine();

            if (line == null)
                return false;

            element.Clear();

            string[] tokens = line.Split(DELIMITERS, StringSplitOptions.RemoveEmptyEntries);
            int vali;
            double vald;
            int count = descr.Properties.Count;
            int i;

            // list element
            if (count > 1 &&
                 descr.Properties[0].Name == "list")
            {
                if (tokens.Length > 0 &&
                     int.TryParse(tokens[0], out count) &&
                     tokens.Length == count + 1)
                {
                    if (descr.Properties[1].DataType == PropertyType.FLOAT ||
                         descr.Properties[1].DataType == PropertyType.DOUBLE)
                    {
                        for (i = 1; i <= count; i++)
                            if (double.TryParse(tokens[i], NumberStyles.Float, CultureInfo.InvariantCulture, out vald))
                                element.Add(vald);
                    }
                    else
                    {
                        for (i = 1; i <= count; i++)
                            if (int.TryParse(tokens[i], out vali))
                                element.Add(vali);
                    }
                }
            }
            else
            if (count == tokens.Length)
            {
                for (i = 0; i < count; i++)
                    if (descr.Properties[i].DataType == PropertyType.FLOAT ||
                         descr.Properties[i].DataType == PropertyType.DOUBLE)
                    {
                        if (double.TryParse(tokens[i], NumberStyles.Float, CultureInfo.InvariantCulture, out vald))
                            element.Add(vald);
                    }
                    else
                    {
                        if (int.TryParse(tokens[i], out vali))
                            element.Add(vali);
                    }
            }
            else
                return false;

            return true;
        }

        public int ReadBrep(string fileName, SceneBrep scene)
        {
            Debug.Assert(scene != null);

            if (string.IsNullOrEmpty(fileName))
                throw new IOException("Invalid file name");

            List<object> element = new List<object>();
            int i, handle;

            bool gzipped = fileName.EndsWith(".gz");
            using (StreamReader reader = gzipped ? new StreamReader(new GZipStream(new FileStream(fileName, FileMode.Open), CompressionMode.Decompress)) :
                                                    new StreamReader(new FileStream(fileName, FileMode.Open)))
            {
                // parse header
                if (!ParseHeader(reader))
                    return -1;

                scene.Reserve(vertices);
                Dictionary<int, int> vertexMap = new Dictionary<int, int>();

                if (format == Format.ASCII)
                {
                    foreach (var el in elements)
                    {
                        // one element batch read from text file
                        for (i = 0; i < el.Count; i++)
                            if (ReadElementText(reader, el, element))
                                if (el.Name == VERTEX)
                                {
                                    // transfer vertex data into SceneBrep:
                                    if (el.PropIndex("x") >= 0)
                                    {
                                        handle = scene.AddVertex(new Vector3(Convert.ToSingle(element[el.PropIndex("x")]),
                                                                               Convert.ToSingle(element[el.PropIndex("y")]),
                                                                               Convert.ToSingle(element[el.PropIndex("z")])));
                                        vertexMap[i] = handle;
                                        if (el.PropIndex(NORMAL_X) >= 0)
                                            scene.SetNormal(handle, new Vector3(Convert.ToSingle(element[el.PropIndex(NORMAL_X)]),
                                                                                  Convert.ToSingle(element[el.PropIndex(NORMAL_Y)]),
                                                                                  Convert.ToSingle(element[el.PropIndex(NORMAL_Z)])));
                                        if (el.PropIndex(TEXTURE_S) >= 0)
                                            scene.SetTxtCoord(handle, new Vector2(Convert.ToSingle(element[el.PropIndex(TEXTURE_S)]),
                                                                                    Convert.ToSingle(element[el.PropIndex(TEXTURE_T)])));
                                        if (el.PropIndex(COLOR_R) >= 0)
                                            if (el.Properties[el.PropIndex(COLOR_R)].DataType == PropertyType.UCHAR)
                                                scene.SetColor(handle, new Vector3(Convert.ToSingle(element[el.PropIndex(COLOR_R)]) / 255.0f,
                                                                                     Convert.ToSingle(element[el.PropIndex(COLOR_G)]) / 255.0f,
                                                                                     Convert.ToSingle(element[el.PropIndex(COLOR_B)]) / 255.0f));
                                            else
                                                scene.SetColor(handle, new Vector3(Convert.ToSingle(element[el.PropIndex(COLOR_R)]),
                                                                                     Convert.ToSingle(element[el.PropIndex(COLOR_G)]),
                                                                                     Convert.ToSingle(element[el.PropIndex(COLOR_B)])));
                                    }
                                }
                                else
                                if (el.Name == FACE)
                                {
                                    // set vertex indices of the face:
                                    // !!! TODO: beyond triangle ???
                                    if (element.Count >= 3)
                                    {
                                        int A = Convert.ToInt32(element[0]);
                                        int B = Convert.ToInt32(element[1]);
                                        int C = Convert.ToInt32(element[2]);
                                        if (vertexMap.ContainsKey(A) &&
                                             vertexMap.ContainsKey(B) &&
                                             vertexMap.ContainsKey(C))
                                            scene.AddTriangle(vertexMap[A],
                                                               vertexMap[B],
                                                               vertexMap[C]);
                                    }
                                }
                    }
                }
                else
                {
                    reader.Close();
                    using (BinaryReader breader = gzipped ? new BinaryReader(new GZipStream(new FileStream(fileName, FileMode.Open), CompressionMode.Decompress)) :
                                                             new BinaryReader(new FileStream(fileName, FileMode.Open)))
                    {
                        // skip the header (END_HEADER):

                        foreach (var el in elements)
                        {
                            // one element batch read from binary file
                        }
                    }
                }
            }

            return scene.Triangles;
        }

        /// <summary>
        /// Reads the header of .ply file and prepares data structures
        /// for data filling.
        /// </summary>
        private bool ParseHeader(StreamReader reader)
        {
            // .ply file header must start with "ply"
            string magic = reader.ReadLine();
            if (magic != HEADER) return false;

            vertices = 0;
            elements = new List<ElementDescr>();

            // current element receives parsed properties..
            ElementDescr element = null;

            string line;
            string[] tokens;
            while ((line = reader.ReadLine()) != null)
            {
                // ignore comments
                if (line.StartsWith(COMMENT)) continue;

                if (line.StartsWith(FORMAT))
                {
                    if (line == FORMAT_TEXT)
                        format = Format.ASCII;
                    else
                    if (line == FORMAT_BINARY_LE)
                        format = Format.BINARY_LE;
                    else
                    if (line == FORMAT_BINARY_BE) format = Format.BINARY_BE;
                }
                else
                if (line.StartsWith(ELEMENT))
                {
                    tokens = line.Split(DELIMITERS, StringSplitOptions.RemoveEmptyEntries);
                    string name = tokens[1];
                    int count = 0;
                    int.TryParse(tokens[2], out count);

                    if (name == VERTEX)
                    {
                        element = new ElementDescr(count, VERTEX);
                        elements.Add(element);
                        vertices = count;
                    }
                    else
                    if (name == FACE)
                    {
                        element = new ElementDescr(count, FACE);
                        elements.Add(element);
                    }
                }
                else
                if (line.StartsWith(PROPERTY))
                {
                    tokens = line.Split(DELIMITERS, StringSplitOptions.RemoveEmptyEntries);

                    // list properties define a number of vertices in the 'face' element, example:
                    // property list uchar int vertex_index
                    // 0        1    2     3   4
                    if (tokens[1] == "list")
                    {
                        if (tokens.Length > 4)
                        {
                            PropertyDescr property1 = new PropertyDescr(tokens[1], tokens[2]);
                            PropertyDescr property2 = new PropertyDescr(tokens[4], tokens[3]);

                            // add properties to currently read element
                            element.Properties.Add(property1);
                            element.Properties.Add(property2);
                        }
                    }
                    // basic property defines element with a single value
                    // property float x
                    // 0        1     2
                    else
                    {
                        if (tokens.Length > 2)
                        {
                            switch (tokens[2])
                            {
                                case NORMAL_X:
                                    DoNormals = true;
                                    break;

                                case TEXTURE_S:
                                    DoTxtCoords = true;
                                    break;

                                case COLOR_R:
                                case COLOR_RED:
                                    DoColors = true;
                                    tokens[2] = COLOR_R;
                                    break;

                                case COLOR_GREEN:
                                    tokens[2] = COLOR_G;
                                    break;

                                case COLOR_BLUE:
                                    tokens[2] = COLOR_B;
                                    break;
                            }

                            PropertyDescr property = new PropertyDescr(tokens[2], tokens[1]);

                            element.Properties.Add(property);
                        }
                    }
                }
                else
                  if (line.StartsWith(END_HEADER))
                    break;
            }

            // finish element definitions:
            foreach (var el in elements)
                el.FinishProperties();

            return true;
        }

        /// <summary>
        /// Writes the whole B-rep scene to a given text stream (uses text variant of Stanford PLY format).
        /// </summary>
        /// <param name="writer">Already open text writer</param>
        /// <param name="scene">Scene to write</param>
        public void WriteBrep(StreamWriter writer, SceneBrep scene)
        {
            DoNormals = true;
            DoTxtCoords = true;
            DoColors = true;

            if (scene == null ||
                 scene.Triangles < 1)
                return;

            Debug.Assert(TextFormat);
            if (!NativeNewLine)
                writer.NewLine = "\r";     // CR only

            bool writeNormals = DoNormals && scene.HasNormals();
            bool writeTxtCoords = DoTxtCoords && scene.HasTxtCoords();
            bool writeColors = DoColors && scene.HasColors();

            writer.WriteLine(HEADER);
            writer.WriteLine(FORMAT_TEXT);

            // vertex-header:
            writer.WriteLine("{0} {1} {2}", ELEMENT, VERTEX, scene.Vertices);
            writer.WriteLine("{0} float x", PROPERTY);
            writer.WriteLine("{0} float {1}", PROPERTY, Orientation ? 'z' : 'y');
            writer.WriteLine("{0} float {1}", PROPERTY, Orientation ? 'y' : 'z');
            if (writeNormals)
            {
                writer.WriteLine("{0} float {1}", PROPERTY, NORMAL_X);
                writer.WriteLine("{0} float {1}", PROPERTY, Orientation ? NORMAL_Z : NORMAL_Y);
                writer.WriteLine("{0} float {1}", PROPERTY, Orientation ? NORMAL_Y : NORMAL_Z);
            }
            if (writeTxtCoords)
            {
                writer.WriteLine("{0} float {1}", PROPERTY, TEXTURE_S);
                writer.WriteLine("{0} float {1}", PROPERTY, TEXTURE_T);
            }
            if (writeColors)
            {
                writer.WriteLine("{0} float {1}", PROPERTY, COLOR_R);
                writer.WriteLine("{0} float {1}", PROPERTY, COLOR_G);
                writer.WriteLine("{0} float {1}", PROPERTY, COLOR_B);
            }

            // face-header:
            writer.WriteLine("{0} {1} {2}", ELEMENT, FACE, scene.Triangles);
            writer.WriteLine("{0} list uchar int vertex_indices", PROPERTY);

            writer.WriteLine(END_HEADER);

            // vertex-data:
            int i;
            Vector3 v3;
            Vector2 v2;
            StringBuilder sb = new StringBuilder();
            for (i = 0; i < scene.Vertices; i++)
            {
                v3 = scene.GetVertex(i);
                sb.Clear();
                sb.AppendFormat(CultureInfo.InvariantCulture, "{0} {1} {2}", v3.X, v3.Y, v3.Z);
                if (writeNormals)
                {
                    v3 = scene.GetNormal(i);
                    sb.AppendFormat(CultureInfo.InvariantCulture, " {0} {1} {2}", v3.X, v3.Y, v3.Z);
                }
                if (writeTxtCoords)
                {
                    v2 = scene.GetTxtCoord(i);
                    sb.AppendFormat(CultureInfo.InvariantCulture, " {0} {1}", v2.X, v2.Y);
                }
                if (writeColors)
                {
                    v3 = scene.GetColor(i);
                    sb.AppendFormat(CultureInfo.InvariantCulture, " {0} {1} {2}", v3.X, v3.Y, v3.Z);
                }
                writer.WriteLine(sb.ToString());
            }

            // face-data:
            int A, B, C;
            for (i = 0; i < scene.Triangles; i++)
            {
                scene.GetTriangleVertices(i, out A, out B, out C);
                writer.WriteLine("3 {0} {1} {2}", A, Orientation ? C : B, Orientation ? B : C);
            }
        }
    }
}
