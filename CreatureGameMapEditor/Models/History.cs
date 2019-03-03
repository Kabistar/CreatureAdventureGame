using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatureGameMapEditor.Models
{
    public class History
    {
        List<HistoryEntry> historyEntries;
        List<int> historyMarkers;
        int historyCapacity;
        int currentHistorySize;
        Map map;

        public History(Map map, int maxsize = 50000)
        {
            if (maxsize < 0) throw new ArgumentException("History maxsize must be greater than zero.");
            if (map == null) throw new ArgumentException("Map cannot be null.");
            this.map = map;
            historyEntries = new List<HistoryEntry>(maxsize);
            historyMarkers = new List<int>();
            historyCapacity = maxsize;
            currentHistorySize = 0;
        }

        public void AddMarker()
        {
            historyMarkers.Add(currentHistorySize);
        }

        public void AddEntry(ushort x, ushort y, byte newID, byte newFlags)
        {
            Tile prev = map.GetTile(x, y);
            if (prev != null)
            {
                // Only update history if we are actually changing something
                if (prev.TileID != newID || prev.Flags != newFlags)
                {
                    historyEntries.Insert(currentHistorySize++, new HistoryEntry(x, y, prev.TileID, prev.Flags, newID, newFlags));

                    if (currentHistorySize > historyCapacity)
                    {
                        historyEntries.RemoveAt(0);
                        currentHistorySize--;
                        for (int i = 0; i < historyMarkers.Count; i++)
                        {
                            historyMarkers[i]--;
                            if (historyMarkers[i] < 0)
                            {
                                historyMarkers.RemoveAt(i);
                                i--;
                            }
                        }
                    }
                    // Remove old entries
                    for (int i = currentHistorySize; i < historyEntries.Count; i++)
                    {
                        historyEntries.RemoveAt(currentHistorySize);
                    }
                    for(int i = 0; i < historyMarkers.Count; i++)
                    {
                        if (historyMarkers[i] > currentHistorySize)
                        {
                            historyMarkers.RemoveAt(i);
                            i--;
                        }
                    }
                }
            }
        }

        public void Undo(bool microStep)
        {
            if (currentHistorySize > 0)
            {
                if (microStep)
                {
                    HistoryEntry entry = historyEntries[--currentHistorySize];
                    map.SetTile(entry.X, entry.Y, entry.PreviousTile, entry.PreviousFlags);
                }
                else
                {
                    int highestMarker = 0;
                    // Get the highest marker that is lower than our current historySize
                    for(int i = 0; i < historyMarkers.Count; i++)
                    {
                        if (historyMarkers[i] >= currentHistorySize) break;
                        highestMarker = historyMarkers[i];
                    }
                    for(int i = currentHistorySize; i > highestMarker; i--)
                    {
                        HistoryEntry entry = historyEntries[--currentHistorySize];
                        map.SetTile(entry.X, entry.Y, entry.PreviousTile, entry.PreviousFlags);
                    }
                }
            }
        }

        public void Redo(bool microStep)
        {
            if (currentHistorySize < historyEntries.Count)
            {
                if (microStep)
                {
                    HistoryEntry entry = historyEntries[currentHistorySize++];
                    map.SetTile(entry.X, entry.Y, entry.NewTile, entry.NewFlags);
                }
                else
                {
                    int lowestMarker = historyEntries.Count;
                    // Get the highest marker that is lower than our current historySize
                    for (int i = historyMarkers.Count - 1; i >= 0; i--)
                    {
                        if (historyMarkers[i] <= currentHistorySize) break;
                        lowestMarker = historyMarkers[i];
                    }
                    for (int i = currentHistorySize; i < lowestMarker; i++)
                    {
                        HistoryEntry entry = historyEntries[currentHistorySize++];
                        map.SetTile(entry.X, entry.Y, entry.NewTile, entry.NewFlags);
                    }
                }
            }
        }

        public void Clear()
        {
            currentHistorySize = 0;
            historyEntries.Clear();
            historyMarkers.Clear();
        }

        public struct HistoryEntry
        {
            public ushort X { get; set; }
            public ushort Y { get; set; }

            public byte PreviousTile { get; private set; }
            public byte PreviousFlags { get; private set; }
            public byte NewTile { get; private set; }
            public byte NewFlags { get; private set; }

            public HistoryEntry(ushort x, ushort y, byte previousTile, byte previousFlags, byte newTile, byte newFlags)
            {
                X = x;
                Y = y;
                PreviousTile = previousTile;
                PreviousFlags = previousFlags;
                NewTile = newTile;
                NewFlags = newFlags;
            }
        }
    }
}
