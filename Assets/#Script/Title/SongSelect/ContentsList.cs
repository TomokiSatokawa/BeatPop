using System.Collections.Generic;
using Common.UI;

public class ContentsList : ScrollViewBase
{
    public void ViewList(IReadOnlyList<IReadOnlySongData> songs,SongUIControl prefab)
    {
        DeleteChild();
        foreach(var song in songs)
        {
            InstantiateContent(prefab).SetData(song);
        }
    }
}
