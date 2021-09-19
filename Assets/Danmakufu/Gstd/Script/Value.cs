using System;
using System.Text;

namespace Gstd
{
    
namespace Script
{
class Value
{
    private Body data;
    public Value()
    {
        data = null;
    }
    public Value(TypeData t, double v)
    {
        data = new Body();
        data.RefCount = 1;
        data.Type = t;
        data.RealValue = v;
    }
    public Value(TypeData t, char v)
    {
        data = new Body();
        data.RefCount = 1;
        data.Type = t;
        data.CharValue = v;
    }
    public Value(TypeData t, bool v)
    {
        data = new Body();
        data.RefCount = 1;
        data.Type = t;
        data.BooleanValue = v;
    }
    public Value(Value source)
    {
        data = source.data;
        if (data != null)
        {
            ++(data.RefCount);
        }
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
    }
    ~Value()
    {
        Release();
    }
    public void Assign(Value source)
    {
        if (source.data != null)
        {
            ++(source.data.RefCount);
        }
        Release();
        data = source.data;
    }
    public void Set(TypeData t, double v)
    {
        Unique();
        data.Type = t;
        data.RealValue = v;
    }
    public void Set(TypeData t, bool v)
    {
        Unique();
        data.Type = t;
        data.BooleanValue = v;
    }
    public bool HasData()
    {
        return data != null;
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
    }
    public void Append(TypeData t, Value x)
    {
        Unique();
        data.Type = t;
        data.ArrayValue.Add(x);
    }
    public void Concatenate(Value x)
    {
        Unique();
        int l = data.ArrayValue.Count;
        int r = x.data.ArrayValue.Count;
        int t = l + r;
        if (l == 0)
        {
            data.Type = x.data.Type;
        }
        foreach (Value v in x.data.ArrayValue)
        {
            data.ArrayValue.Add(v);
        }
    }
    public double AsReal()
    {
        if (data == null)
        {
            return 0;
        }
        switch(data.Type.Kind)
        {
            case TypeKind.TK_REAL: return data.RealValue;
            case TypeKind.TK_CHAR: return (double) data.CharValue;
            case TypeKind.TK_BOOLEAN: return data.BooleanValue ? 1 : 0;
            case TypeKind.TK_ARRAY:
                double number;
                if (data.Type.Element.Kind == TypeKind.TK_CHAR && Double.TryParse(AsString(), out number))
                {
                    return number;
                }
                return 0;
            default: return 0;
        }
    }
    public char AsChar()
    {
        if (data == null)
        {
            return '\0';
        }
        switch(data.Type.Kind)
        {
            case TypeKind.TK_REAL: return (char) data.RealValue;
            case TypeKind.TK_CHAR: return data.CharValue;
            case TypeKind.TK_BOOLEAN: return data.BooleanValue ? '1' : '0';
            case TypeKind.TK_ARRAY: return '\0';
            default: return '\0';
        }
    }
    public bool AsBoolean()
    {
        if (data == null)
        {
            return false;
        }
        switch(data.Type.Kind)
        {
            case TypeKind.TK_REAL: return data.RealValue != 0;
            case TypeKind.TK_CHAR: return data.CharValue != '\0';
            case TypeKind.TK_BOOLEAN: return data.BooleanValue;
            case TypeKind.TK_ARRAY: return data.ArrayValue.Count != 0;
            default: return false;
        }
    }
    public string AsString()
    {
        if (data == null)
        {
            return "(Void)";
        }
        switch(data.Type.Kind)
        {
            case TypeKind.TK_REAL: return data.RealValue.ToString();
            case TypeKind.TK_CHAR: return data.CharValue.ToString();
            case TypeKind.TK_BOOLEAN: return data.BooleanValue ? "true" : "false";
            case TypeKind.TK_ARRAY:
                StringBuilder sb = new StringBuilder();
                if (data.Type.Element.Kind == TypeKind.TK_CHAR)
                {
                    foreach (Value v in data.ArrayValue)
                    {
                        sb.Append(v.AsChar());
                    }
                }
                else
                {
                    sb.Append('[');
                    int i = 0;
                    foreach (Value v in data.ArrayValue)
                    {
                        sb.Append(v.AsChar());
                        ++i;
                        if (i != data.ArrayValue.Count)
                        {
                            sb.Append(',');
                        }
                    }
                    sb.Append(']');
                }
                return sb.ToString();
            default: return "(INTERNAL-ERROR)";
        }
    }
    public int LengthAsArray()
    {
        return data.ArrayValue.Count;
    }
    public Value IndexAsArray(int i)
    {
        return data.ArrayValue[i];
    }
    public TypeData GetDataType()
    {
        return data.Type;
    }
    public void overwrite(Value source)
    {
        if (data == source.data)
        {
            return;
        }
        Release();
        data = source.data;
        data.RefCount = 2;
    }
    private void Release()
    {
        if (data != null)
        {
            --(data.RefCount);
            data = null;
        }
    }
}

}

}
