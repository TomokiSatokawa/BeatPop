using System.Collections.Generic;
using Common.UI;
using TMPro;
using UnityEngine;
namespace Title.SongSelect
{
    public class SongSelectGenerator : ScrollViewBase
    {
        [SerializeField] private TextMeshProUGUI _titlePrefab;
        [SerializeField] private ContentsList _contentsListPrefab;
        [SerializeField] private SongUIControl _listSongPrefab;

        private List<GameObject> _clonedObject = new();
        public void InitialView()
        {
            DeleteChild();
            AddTitle("‚¨‚·‚·‚ß");
            AddContentsList(SongRecommender.I.GetRecommendation());
        }
        private void AddTitle(string text)
        {
            var title = InstantiateContent(_titlePrefab);
            title.text = text;
            _clonedObject.Add(title.gameObject);
        }
        private void AddContentsList(IReadOnlyList<SongSelectData> datas)
        {
            var list = InstantiateContent(_contentsListPrefab);
            list.ViewList(datas, _listSongPrefab);
            _clonedObject.Add(list.gameObject);
        }
        protected override void DeleteChild()
        {
            base.DeleteChild();
            _clonedObject.Clear();
        }
    }
}
