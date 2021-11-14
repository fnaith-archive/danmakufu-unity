using System;
using System.Diagnostics;
using System.Text;
#if _TRACE_VALUE
using System;
#endif

namespace Gstd
{
    namespace Script
    {
        sealed class Value : System.IDisposable// TODO prevent pass TypeData
        {
#if _TRACE_VALUE
            static int counter = 0;
            public int id;
#endif
            private Body data;
            public Value()
            {
#if _TRACE_VALUE
                id = -1;
#endif
            }
            public Value(TypeData t, double v)
            {
                data = new Body();
                data.RefCount = 1;
                data.Type = t;
                data.RealValue = v;
#if _TRACE_VALUE
                id = counter++;
#endif
            }
            public Value(TypeData t, char v)
            {
                data = new Body();
                data.RefCount = 1;
                data.Type = t;
                data.CharValue = v;
#if _TRACE_VALUE
                id = counter++;
#endif
            }
            public Value(TypeData t, bool v)
            {
                data = new Body();
                data.RefCount = 1;
                data.Type = t;
                data.BooleanValue = v;
#if _TRACE_VALUE
                id = counter++;
#endif
            }
            public Value(TypeData t, string v)
            {
                data = new Body();
                data.RefCount = 1;
                data.Type = t;
                foreach (char ch in v.ToCharArray())
                {
                    data.ArrayValue.Add(new Value(t.Element, ch));
                }
#if _TRACE_VALUE
                id = counter++;
#endif
            }
            public Value(Value source)
            {
#if _TRACE_VALUE
                if (source.IsTarget())
                {
                    Console.WriteLine("V:Copy source " + source.id + "=" + source.AsString());
                }
                id = 10000 + source.id;
#endif
                data = source.data;
                if (data != null)
                {
                    ++(data.RefCount);
                }
            }
            public void Dispose()
            {
                Release();
            }
            public void CopyFrom(Value source)
            {
#if _TRACE_VALUE
                if (IsTarget() || source.IsTarget())
                {
                    Console.WriteLine("V:Assign this " + id + "=" + AsString());
                    Console.WriteLine("V:Assign source " + source.id + "=" + source.AsString());
                }
                id = source.id;
#endif
                if (source.data != null)
                {
                    ++(source.data.RefCount);
                }
                Release();
                data = source.data;
            }
            public bool HasData()
            {
                return data != null;
            }
#if _TRACE_VALUE
            private bool IsTarget()
            {
                return false;//id == 10001;
            }
#endif
            public void Set(TypeData t, double v)
            {
                Unique();
                data.Type = t;
                data.RealValue = v;
#if _TRACE_VALUE
                if (IsTarget())
                {
                    Console.WriteLine("V:set double " + AsString());
                }
#endif
            }
            public void Set(TypeData t, bool v)
            {
                Unique();
                data.Type = t;
                data.BooleanValue = v;
#if _TRACE_VALUE
                if (IsTarget())
                {
                    Console.WriteLine("V:set bool " + AsString());
                }
#endif
            }
            public void Append(TypeData t, Value x)
            {
                Unique();
                data.Type = t;
                data.ArrayValue.Add(x);
#if _TRACE_VALUE
                if (IsTarget())
                {
                    Console.WriteLine("V:Append " + AsString());
                }
#endif
            }
            public void Concatenate(Value x)
            {
                Unique();
                if (data.ArrayValue.Count == 0)
                {
                    data.Type = x.data.Type;
                }
                data.ArrayValue.AddRange(x.data.ArrayValue);
#if _TRACE_VALUE
                if (IsTarget())
                {
                    Console.WriteLine("V:Concatenate " + AsString());
                }
#endif
            }
            public double AsReal()
            {
                if (data == null)
                {
                    return 0;
                }
                switch (data.Type.Kind)
                {
                    case TypeKind.tk_real: return data.RealValue;
                    case TypeKind.tk_char: return data.CharValue;
                    case TypeKind.tk_boolean: return data.BooleanValue ? 1 : 0;
                    case TypeKind.tk_array:
                        double number;
                        if (data.Type.Element.Kind == TypeKind.tk_char && double.TryParse(AsString(), out number))
                        {
                            return number;
                        }
                        return 0;
                    default:
                        Debug.Assert(false);
                        return 0;
                }
            }
            public char AsChar()
            {
                if (data == null)
                {
                    return '\0';
                }
                switch (data.Type.Kind)
                {
                    case TypeKind.tk_real: return (char) data.RealValue;
                    case TypeKind.tk_char: return data.CharValue;
                    case TypeKind.tk_boolean: return data.BooleanValue ? '1' : '0';
                    case TypeKind.tk_array: return '\0';
                    default:
                        Debug.Assert(false);
                        return '\0';
                }
            }
            public bool AsBoolean()
            {
                if (data == null)
                {
                    return false;
                }
                switch (data.Type.Kind)
                {
                    case TypeKind.tk_real: return data.RealValue != 0;
                    case TypeKind.tk_char: return data.CharValue != '\0';
                    case TypeKind.tk_boolean: return data.BooleanValue;
                    case TypeKind.tk_array: return data.ArrayValue.Count != 0;
                    default:
                        Debug.Assert(false);
                        return false;
                }
            }
            public string AsString()
            {
                if (data == null)
                {
                    return "(VOID)";
                }
                switch (data.Type.Kind)
                {
                    case TypeKind.tk_real: return data.RealValue.ToString();
                    case TypeKind.tk_char: return data.CharValue.ToString();
                    case TypeKind.tk_boolean: return data.BooleanValue ? "true" : "false";
                    case TypeKind.tk_array:
                        StringBuilder sb = new StringBuilder();
                        if (data.Type.Element.Kind == TypeKind.tk_char)
                        {
                            foreach (Value v in data.ArrayValue)
                            {
                                sb.Append(v.AsChar());
                            }
                        }
                        else
                        {
                            sb.Append('[');
                            bool afterFirstValue = false;
                            foreach (Value v in data.ArrayValue)
                            {
                                if (afterFirstValue)
                                {
                                    sb.Append(',');
                                }
                                sb.Append(v.AsString());
                                afterFirstValue = true;
                            }
                            sb.Append(']');
                        }
                        return sb.ToString();
                    default:
                        Debug.Assert(false);
                        return "(INTERNAL-ERROR)";
                }
            }
            public int LengthAsArray()
            {
                Debug.Assert(data != null && data.Type.Kind == TypeKind.tk_array);
                return data.ArrayValue.Count;
            }
            public Value IndexAsArray(int i)
            {
                Debug.Assert(data != null && data.Type.Kind == TypeKind.tk_array);
                Debug.Assert(i < data.ArrayValue.Count);
                return data.ArrayValue[i];
            }
            public TypeData GetDataType()
            {
                Debug.Assert(data != null);
                return data.Type;
            }
            public void Unique()
            {
                if (data == null)
                {
                    data = new Body();
                    data.RefCount = 1;
                    data.Type = null;
                }
                else if (data.RefCount > 1)
                {
                    --(data.RefCount);
                    data = new Body(data);
                    data.RefCount = 1;
                }
#if _TRACE_VALUE
                if (IsTarget())
                {
                    Console.WriteLine("V:Unique " + AsString());
                }
#endif
            }
            public void Overwrite(Value source)
            {
#if _TRACE_VALUE
                if (IsTarget() || source.IsTarget())
                {
                    Console.WriteLine("V:Overwrite this " + id + "=" + AsString());
                    Console.WriteLine("V:Overwrite source " + source.id + "=" + source.AsString());
                }
                id = source.id;
#endif
                Debug.Assert(data != null);
                if (data == source.data)
                {
                    return;
                }
                Release();
                data.Assign(source.data);
                data.RefCount = 2;
            }
            private void Release() // TODO remove
            {
                if (data != null)
                {
                    --(data.RefCount);
                    if (data.RefCount == 0)
                    {
                        data = null;
                    }
                }
            }
        }
    }
}
