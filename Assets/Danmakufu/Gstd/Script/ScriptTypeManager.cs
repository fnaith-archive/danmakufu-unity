using System.Collections.Generic;

namespace Gstd
{
    namespace Script
    {
        sealed class ScriptTypeManager
        {
            private List<TypeData> types;
            public TypeData RealType { get; }
            public TypeData CharType { get; }
            public TypeData BooleanType { get; }
            public TypeData StringType { get; }
            public ScriptTypeManager()
            {
                RealType = new TypeData(TypeKind.tk_real);
                CharType = new TypeData(TypeKind.tk_char);
                BooleanType = new TypeData(TypeKind.tk_boolean);
                StringType = new TypeData(TypeKind.tk_array, CharType);
                types = new List<TypeData>();
                types.Add(RealType);
                types.Add(CharType);
                types.Add(BooleanType);
                types.Add(StringType);
            }
            public TypeData GetArrayType(TypeData element)
            {
                foreach (TypeData typeData in types)
                {
                    if (typeData.Kind == TypeKind.tk_array && typeData.Element == element)
                    {
                        return typeData;
                    }
                }
                TypeData type = new TypeData(TypeKind.tk_array, element);
                types.Add(type);
                return type;
            }
        }
    }
}
