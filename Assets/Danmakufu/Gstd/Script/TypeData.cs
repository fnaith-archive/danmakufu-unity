namespace Gstd
{
    namespace Script
    {
        class TypeData
        {
            private TypeKind kind;
            private TypeData element;
            public TypeKind Kind
            {
                get => kind;
            }
            public TypeData Element
            {
                get => element;
            }
            public TypeData(TypeKind kind, TypeData element=null)
            {
                this.kind = kind;
                this.element = element;
            }
            public TypeData(TypeData source) : this(source.kind, source.element)
            {
            }
        }
    }
}
