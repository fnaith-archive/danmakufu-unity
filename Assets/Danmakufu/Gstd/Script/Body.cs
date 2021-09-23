using System.Collections.Generic;

namespace Gstd
{
    
namespace Script
{

class Body
{
    private int refCount;
    private TypeData type;
    private List<Value> arrayValue; // TODO lightweight_vector
    // TODO union
    private double realValue;
    private char charValue;
    private bool booleanValue;
    public int RefCount
    {
        get => refCount;
        set => refCount = value;
    }
    public TypeData Type
    {
        get => type;
        set => type = value;
    }
    public List<Value> ArrayValue
    {
        get => arrayValue;
        set => arrayValue = value;
    }
    public double RealValue
    {
        get => realValue;
        set => realValue = value;
    }
    public char CharValue
    {
        get => charValue;
        set => charValue = value;
    }
    public bool BooleanValue
    {
        get => booleanValue;
        set => booleanValue = value;
    }
    public Body()
    {
        arrayValue = new List<Value>();
    }
    public Body(Body source)
    {
        refCount = source.refCount;
        type = source.type;
        arrayValue = source.arrayValue;
        realValue = source.realValue;
        charValue = source.charValue;
        booleanValue = source.booleanValue;
    }
}

}

}
