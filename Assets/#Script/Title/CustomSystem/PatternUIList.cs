using Common.UI;
using UnityEngine;

public class PatternUIList : ScrollViewBase
{
    [SerializeField] private PatternUIControl _prefab;

    public async void ShowList()
    {
        if (!SongInfoControl.I.CurrentData .HasValue) return;
        int songID = SongInfoControl.I.CurrentData.Value.SongData.SongID;
        DeleteChild();
        var patternData = await CustomManifestLoader.I.GetCustomPattern(songID);
        if (patternData == null)
        {
            Debug.LogError($"SongID{songID} ‚ª–³Œø‚Ü‚½‚Í Pattern‚ª‘¶İ‚µ‚Ü‚¹‚ñ");
            return;
        }

        foreach(var pattern in patternData)
        {
            var patternUI = InstantiateContent(_prefab);

            patternUI.SetData(pattern);
        }
    }
}