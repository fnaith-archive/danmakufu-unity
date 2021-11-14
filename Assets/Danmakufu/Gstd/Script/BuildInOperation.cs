using System;
using System.Diagnostics;

namespace Gstd
{
    namespace Script
    {
        class BuildInOperation
        {
            public static Function[] Operations = {
                new Function("true", new Callback(true_), 0),
                new Function("false", new Callback(false_), 0),
                new Function("pi", new Callback(pi), 0),
                new Function("length", new Callback(length), 1),
                new Function("not", new Callback(not_), 1),
                new Function("negative", new Callback(negative), 1),
                new Function("predecessor", new Callback(predecessor), 1),
                new Function("successor", new Callback(successor), 1),
                new Function("round", new Callback(round), 1),
                new Function("trunc", new Callback(truncate), 1),
                new Function("truncate", new Callback(truncate), 1),
                new Function("ceil", new Callback(ceil), 1),
                new Function("floor", new Callback(floor), 1),
                new Function("absolute", new Callback(absolute), 1),
                new Function("add", new Callback(add), 2),
                new Function("subtract", new Callback(subtract), 2),
                new Function("multiply", new Callback(multiply), 2),
                new Function("divide", new Callback(divide), 2),
                new Function("remainder", new Callback(remainder), 2),
                new Function("modc", new Callback(modc), 2),
                new Function("power", new Callback(power), 2),
                new Function("index_", new Callback(index), 2),
                new Function("index!", new Callback(indexWritable), 2),
                new Function("slice", new Callback(slice), 3),
                new Function("erase", new Callback(erase), 2),
                new Function("append", new Callback(append), 2),
                new Function("concatenate", new Callback(concatenate), 2),
                new Function("compare", new Callback(compare), 2),
                new Function("assert", new Callback(assert_), 2)
            };
            private static Value true_(ScriptMachine machine, int argc, Value[] argv)
            {
                return new Value(machine.Engine.GetBooleanType(), true);
            }
            private static Value false_(ScriptMachine machine, int argc, Value[] argv)
            {
                return new Value(machine.Engine.GetBooleanType(), false);
            }
            private static Value pi(ScriptMachine machine, int argc, Value[] argv)
            {
                return new Value(machine.Engine.GetRealType(), 3.14159265358979323846);
            }
            private static Value length(ScriptMachine machine, int argc, Value[] argv)
            {
                Debug.Assert(argc == 1);
                return new Value(machine.Engine.GetRealType(), argv[0].LengthAsArray());
            }
            private static Value not_(ScriptMachine machine, int argc, Value[] argv)
            {
                return new Value(machine.Engine.GetBooleanType(), !argv[0].AsBoolean());
            }
            private static Value negative(ScriptMachine machine, int argc, Value[] argv)
            {
                return new Value(machine.Engine.GetRealType(), -argv[0].AsReal());
            }
            private static Value predecessor(ScriptMachine machine, int argc, Value[] argv)
            {
                Debug.Assert(argc == 1);
                Debug.Assert(argv[0].HasData());
                switch (argv[0].GetDataType().Kind)
                {
                    case TypeKind.tk_real:
                        return new Value(argv[0].GetDataType(), argv[0].AsReal() - 1);

                    case TypeKind.tk_char:
                        {
                            char c = argv[0].AsChar();
                            --c;
                            return new Value(argv[0].GetDataType(), c);
                        }
                    case TypeKind.tk_boolean:
                        return new Value(argv[0].GetDataType(), false);
                    default:
                        {
                            string error = "This variables does not allow predecessor\r\n";
                            machine.RaiseError(error);
                            return new Value();
                        }
                }
            }
            private static Value successor(ScriptMachine machine, int argc, Value[] argv)
            {
                Debug.Assert(argc == 1);
                Debug.Assert(argv[0].HasData());
                switch (argv[0].GetDataType().Kind)
                {
                    case TypeKind.tk_real:
                        return new Value(argv[0].GetDataType(), argv[0].AsReal() + 1);

                    case TypeKind.tk_char:
                        {
                            char c = argv[0].AsChar();
                            ++c;
                            return new Value(argv[0].GetDataType(), c);
                        }
                    case TypeKind.tk_boolean:
                        return new Value(argv[0].GetDataType(), true);
                    default:
                        {
                            string error = "This variables does not allow successor\r\n";
                            machine.RaiseError(error);
                            return new Value();
                        }
                }
            }
            private static Value round(ScriptMachine machine, int argc, Value[] argv)
            {
                double r = Math.Floor(argv[0].AsReal() + 0.5);
                return new Value(machine.Engine.GetRealType(), r);
            }
            private static Value truncate(ScriptMachine machine, int argc, Value[] argv)
            {
                double r = argv[0].AsReal();
                r = (r > 0) ? Math.Floor(r) : Math.Ceiling(r);
                return new Value(machine.Engine.GetRealType(), r);
            }
            private static Value ceil(ScriptMachine machine, int argc, Value[] argv)
            {
                return new Value(machine.Engine.GetRealType(), Math.Ceiling(argv[0].AsReal()));
            }
            private static Value floor(ScriptMachine machine, int argc, Value[] argv)
            {
                return new Value(machine.Engine.GetRealType(), Math.Floor(argv[0].AsReal()));
            }
            private static Value absolute(ScriptMachine machine, int argc, Value[] argv)
            {
                double r = Math.Abs(argv[0].AsReal());
                return new Value(machine.Engine.GetRealType(), r);
            }
            private static Value add(ScriptMachine machine, int argc, Value[] argv)
            {
                Debug.Assert(argc == 2);
                if (argv[0].GetDataType().Kind == TypeKind.tk_array)
                {
                    if (argv[0].GetDataType() != argv[1].GetDataType())
                    {
                        string error = "variable type mismatch\r\n";
                        machine.RaiseError(error);
                        return new Value();
                    }
                    if (argv[0].LengthAsArray() != argv[1].LengthAsArray())
                    {
                        string error = "array length mismatch\r\n";
                        machine.RaiseError(error);
                        return new Value();
                    }
                    Value result = new Value();
                    for (int i = 0; i < argv[1].LengthAsArray(); ++i)
                    {
                        Value[] v = new Value[2];
                        v[0] = argv[0].IndexAsArray(i);
                        v[1] = argv[1].IndexAsArray(i);
                        result.Append(argv[1].GetDataType(), add(machine, 2, v));
                    }
                    return result;
                }
                else
                {
                    return new Value(machine.Engine.GetRealType(), argv[0].AsReal() + argv[1].AsReal());
                }
            }
            private static Value subtract(ScriptMachine machine, int argc, Value[] argv)
            {
                Debug.Assert(argc == 2);
                if (argv[0].GetDataType().Kind == TypeKind.tk_array)
                {
                    if (argv[0].GetDataType() != argv[1].GetDataType())
                    {
                        string error = "variable type mismatch\r\n";
                        machine.RaiseError(error);
                        return new Value();
                    }
                    if (argv[0].LengthAsArray() != argv[1].LengthAsArray())
                    {
                        string error = "array length mismatch\r\n";
                        machine.RaiseError(error);
                        return new Value();
                    }
                    Value result = new Value();
                    for (int i = 0; i < argv[1].LengthAsArray(); ++i)
                    {
                        Value[] v = new Value[2];
                        v[0] = argv[0].IndexAsArray(i);
                        v[1] = argv[1].IndexAsArray(i);
                        result.Append(argv[1].GetDataType(), subtract(machine, 2, v));
                    }
                    return result;
                }
                else
                {
                    return new Value(machine.Engine.GetRealType(), argv[0].AsReal() - argv[1].AsReal());
                }
            }
            private static Value multiply(ScriptMachine machine, int argc, Value[] argv)
            {
                return new Value(machine.Engine.GetRealType(), argv[0].AsReal() * argv[1].AsReal());
            }
            private static Value divide(ScriptMachine machine, int argc, Value[] argv)
            {
                return new Value(machine.Engine.GetRealType(), argv[0].AsReal() / argv[1].AsReal());
            }
            private static Value remainder(ScriptMachine machine, int argc, Value[] argv)
            {
                double x = argv[0].AsReal();
                double y = argv[1].AsReal();
                return new Value(machine.Engine.GetRealType(), x % y); // TODO fmodl2
            }
            private static Value modc(ScriptMachine machine, int argc, Value[] argv)
            {
                double x = argv[0].AsReal();
                double y = argv[1].AsReal();
                return new Value(machine.Engine.GetRealType(), x % y); // TODO fmodl
            }
            private static Value power(ScriptMachine machine, int argc, Value[] argv)
            {
                return new Value(machine.Engine.GetRealType(), Math.Pow(argv[0].AsReal(), argv[1].AsReal()));
            }
            private static Value index(ScriptMachine machine, int argc, Value[] argv)
            {
                Debug.Assert(argc == 2);

                if (argv[0].GetDataType().Kind != TypeKind.tk_array)
                {
                    string error = "This variables does not allow to array index operation.\r\n";
                    machine.RaiseError(error);
                    return new Value();
                }

                double index = argv[1].AsReal();

                if (index != (int)(index))
                {
                    string error = "Array index access does not allow to period.\r\n";
                    machine.RaiseError(error);
                    return new Value();
                }

                if (index < 0 || index >= argv[0].LengthAsArray())
                {
                    string error = "Array index out of bounds.\r\n";
                    machine.RaiseError(error);
                    return new Value();
                }

                Value result = argv[0].IndexAsArray((int)(index));
                return new Value(result);
            }
            private static Value indexWritable(ScriptMachine machine, int argc, Value[] argv)
            {
                Debug.Assert(argc == 2);

                if (argv[0].GetDataType().Kind != TypeKind.tk_array)
                {
                    string error = "This variables does not allow to array index operation.\r\n";
                    machine.RaiseError(error);
                    return new Value();
                }

                double index = argv[1].AsReal();

                if (index != (int)(index))
                {
                    string error = "Array index access does not allow to period.\r\n";
                    machine.RaiseError(error);
                    return new Value();
                }

                if (index < 0 || index >= argv[0].LengthAsArray())
                {
                    string error = "Array index out of bounds.\r\n";
                    machine.RaiseError(error);
                    return new Value();
                }

                Value result = argv[0].IndexAsArray((int)(index));
                result.Unique();
                return new Value(result);
            }
            private static Value slice(ScriptMachine machine, int argc, Value[] argv)
            {
                Debug.Assert(argc == 3);

                if (argv[0].GetDataType().Kind != TypeKind.tk_array)
                {
                    string error = "This variables does not allow to array slice operation.\r\n";
                    machine.RaiseError(error);
                    return new Value();
                }

                double index_1 = argv[1].AsReal();

                if (index_1 != (int)(index_1))
                {
                    string error = "Array slicing does not allow to period.\r\n";
                    machine.RaiseError(error);
                    return new Value();
                }

                double index_2 = argv[2].AsReal();

                if (index_2 != (int)(index_2))
                {
                    string error = "Array slicing does not allow to period.\r\n";
                    machine.RaiseError(error);
                    return new Value();
                }

                if (index_1 < 0 || index_1 > index_2 || index_2 > argv[0].LengthAsArray())
                {
                    string error = "Array index out of bounds.\r\n";
                    machine.RaiseError(error);
                    return new Value();
                }

                Value result = new Value(argv[0].GetDataType(), "");

                for (int i = (int)(index_1); i < (int)(index_2); ++i)
                {
                    result.Append(result.GetDataType(), argv[0].IndexAsArray(i));
                }

                return result;
            }
            private static Value erase(ScriptMachine machine, int argc, Value[] argv)
            {
                Debug.Assert(argc == 2);

                if (argv[0].GetDataType().Kind != TypeKind.tk_array)
                {
                    string error = "This variables does not allow to array erase operation.\r\n";
                    machine.RaiseError(error);
                    return new Value();
                }

                double index_1 = argv[1].AsReal();
                double length = argv[0].LengthAsArray();

                if (index_1 != (int)(index_1))
                {
                    string error = "Array erasing does not allow to period.\r\n";
                    machine.RaiseError(error);
                    return new Value();
                }

                if (index_1 < 0 || index_1 >= argv[0].LengthAsArray())
                {
                    string error = "Array index out of bounds.\r\n";
                    machine.RaiseError(error);
                    return new Value();
                }

                Value result = new Value(argv[0].GetDataType(), "");

                for (int i = 0; i < (int)(index_1); ++i)
                {
                    result.Append(result.GetDataType(), argv[0].IndexAsArray(i));
                }
                for (int i = (int)(index_1) + 1; i < (int)(length); ++i)
                {
                    result.Append(result.GetDataType(), argv[0].IndexAsArray(i));
                }
                return result;
            }
            private static Value append(ScriptMachine machine, int argc, Value[] argv)
            {
                Debug.Assert(argc == 2);

                if (argv[0].GetDataType().Kind != TypeKind.tk_array)
                {
                    string error = "This variables does not allow to array append operation.\r\n";
                    machine.RaiseError(error);
                    return new Value();
                }

                if (argv[0].LengthAsArray() > 0 && argv[0].GetDataType().Element != argv[1].GetDataType())
                {
                    string error = "variable type mismatch\r\n";
                    machine.RaiseError(error);
                    return new Value();
                }

                Value result = new Value(argv[0]);
                result.Append(machine.Engine.GetArrayType(argv[1].GetDataType()), argv[1]);
                return new Value(result);
            }
            private static Value concatenate(ScriptMachine machine, int argc, Value[] argv)
            {
                Debug.Assert(argc == 2);

                if (argv[0].GetDataType().Kind != TypeKind.tk_array || argv[1].GetDataType().Kind != TypeKind.tk_array)
                {
                    string error = "This variables does not allow to array concatenate operation.\r\n";
                    machine.RaiseError(error);
                    return new Value();
                }

                if (argv[0].LengthAsArray() > 0 && argv[1].LengthAsArray() > 0 && argv[0].GetDataType() != argv[1].GetDataType())
                {
                    string error = "variable type mismatch\r\n";
                    machine.RaiseError(error);
                    return new Value();
                }

                Value result = new Value(argv[0]);
                result.Concatenate(argv[1]);
                return new Value(result);
            }
            private static Value compare(ScriptMachine machine, int argc, Value[] argv)
            {
                if (argv[0].GetDataType() == argv[1].GetDataType())
                {
                    int r = 0;

                    switch (argv[0].GetDataType().Kind)
                    {
                        case TypeKind.tk_real:
                            {
                                double a = argv[0].AsReal();
                                double b = argv[1].AsReal();
                                r = (a == b) ? 0 : ((a < b) ? -1 : 1);
                            }
                            break;

                        case TypeKind.tk_char:
                            {
                                char a = argv[0].AsChar();
                                char b = argv[1].AsChar();
                                r = (a == b) ? 0 : ((a < b) ? -1 : 1);
                            }
                            break;

                        case TypeKind.tk_boolean:
                            {
                                bool a = argv[0].AsBoolean();
                                bool b = argv[1].AsBoolean();
                                r = (a == b) ? 0 : ((!a) ? -1 : 1);
                            }
                            break;

                        case TypeKind.tk_array:
                            {
                                for (int i = 0; i < argv[0].LengthAsArray(); ++i)
                                {
                                    if (i >= argv[1].LengthAsArray())
                                    {
                                        r = +1;	//"123" > "12"
                                        break;
                                    }

                                    Value[] v = new Value[2];
                                    v[0] = argv[0].IndexAsArray(i);
                                    v[1] = argv[1].IndexAsArray(i);
                                    r = (int)(compare(machine, 2, v).AsReal());
                                    if (r != 0)
                                    {
                                        break;
                                    }
                                }
                                if (r == 0 && argv[0].LengthAsArray() < argv[1].LengthAsArray())
                                {
                                    r = -1;	//"12" < "123"
                                }
                            }
                            break;

                        default:
                            Debug.Assert(false);
                            break;
                    }
                    return new Value(machine.Engine.GetRealType(), r);
                }
                else
                {
                    string error = "Variables of different types are being compared\r\n";
                    machine.RaiseError(error);
                    return new Value();
                }
            }
            private static Value assert_(ScriptMachine machine, int argc, Value[] argv)
            {
                Debug.Assert(argc == 2);
                if (!argv[0].AsBoolean())
                {
                    machine.RaiseError(argv[1].AsString());
                }
                return new Value();
            }
        }
    }
}
