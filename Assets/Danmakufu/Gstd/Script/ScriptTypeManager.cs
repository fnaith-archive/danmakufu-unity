using System.Collections.Generic;

namespace Gstd
{
    namespace Script
    {
        class ScriptTypeManager
        {
            private List<TypeData> types;
            private TypeData realType;
            private TypeData charType;
            private TypeData booleanType;
            private TypeData stringType;
            public TypeData RealType
            {
                get => realType;
            }
            public TypeData CharType
            {
                get => charType;
            }
            public TypeData BooleanType
            {
                get => booleanType;
            }
            public TypeData StringType
            {
                get => stringType;
            }
            public ScriptTypeManager()
            {
                realType = new TypeData(TypeKind.TK_REAL);
                charType = new TypeData(TypeKind.TK_CHAR);
                booleanType = new TypeData(TypeKind.TK_BOOLEAN);
                stringType = new TypeData(TypeKind.TK_ARRAY, charType);
                types = new List<TypeData>();
                types.Add(realType);
                types.Add(charType);
                types.Add(booleanType);
                types.Add(stringType);
            }

            public TypeData GetArrayType(TypeData element)
            {
                foreach (TypeData typeData in types)
                {
                    if (typeData.Kind == TypeKind.TK_ARRAY && typeData.Element == element)
                    {
                        return typeData;
                    }
                }
                TypeData type = new TypeData(TypeKind.TK_ARRAY, element);
                types.Add(type);
                return type;
            }
        }
    }
}
