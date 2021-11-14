using System.Collections.Generic;

namespace Gstd
{
    namespace ScriptClient
    {
        sealed class ScriptFileLineMap
        {
            private List<Entry> listEntry;
            public ScriptFileLineMap()
            {
                listEntry = new List<Entry>();
            }
			public void AddEntry(string path, int lineAdd, int lineCount)
            {
                Entry entryNew = new Entry();
                entryNew.Path = path;
                entryNew.LineStartOriginal = 1;
                entryNew.LineEndOriginal = lineCount;
                entryNew.LineStart = lineAdd;
                entryNew.LineEnd = entryNew.LineStart + lineCount - 1;
                if (listEntry.Count == 0)
                {
                    listEntry.Add(entryNew);
                    return;
                }

                Entry pEntryDivide = null;
                int itrInsert = 0;
                foreach (Entry entry in listEntry)
                {
                    if (lineAdd >= entry.LineStart && lineAdd <= entry.LineEnd)
                    {
                        pEntryDivide = entry;
                        break;
                    }
                    ++itrInsert;
                }
                
                Entry entryDivide = new Entry(pEntryDivide);
                if (entryDivide.LineStart == lineAdd)
                {
                    entryDivide.LineStartOriginal++;
                    listEntry.Insert(itrInsert, entryNew);
                }
                else if (entryDivide.LineEnd == lineAdd)
                {
                    entryDivide.LineEnd--;
                    entryDivide.LineEndOriginal--;

                    listEntry.Insert(itrInsert, entryNew);
                    itrInsert++;
                }
                else
                {
                    Entry entryNew2 = new Entry(entryDivide);
                    entryDivide.LineEnd = lineAdd - 1;
                    entryDivide.LineEndOriginal = lineAdd - entryDivide.LineStart;

                    entryNew2.LineStartOriginal = entryDivide.LineEndOriginal + 2;
                    entryNew2.LineStart = entryNew.LineEnd + 1;
                    entryNew2.LineEnd += lineCount - 1;
                    
                    if (itrInsert < listEntry.Count)
                    {
                        itrInsert++;
                    }
                    listEntry.Insert(itrInsert, entryNew);
                    listEntry.Insert(itrInsert, entryNew2);
                }

                for(; itrInsert < listEntry.Count; ++itrInsert)
                {
                    Entry entry = listEntry[itrInsert];
                    entry.LineStart += lineCount - 1;
                    entry.LineEnd += lineCount - 1;
                }
            }
			public Entry GetEntry(int line)
            {
                foreach (Entry entry in listEntry)
                {
                    if (line >= entry.LineStart && line <= entry.LineEnd)
                    {
                        return entry;
                    }
                }
                return new Entry();
            }
			public string GetPath(int line)
            {
                Entry entry = GetEntry(line);
                return entry.Path;
            }
			public List<Entry> GetEntryList()
            {
                return listEntry;
            }
        }
    }
}
