namespace Gstd
{
    namespace Script
    {
        sealed class TypeData
        {
            public TypeKind Kind { get; }
            public TypeData Element{ get; }
            public TypeData(TypeKind kind, TypeData element=null)
            {
                Kind = kind;
                Element = element;
            }
            public TypeData(TypeData source) : this(source.Kind, source.Element)
            {
            }
        }
    }
}
