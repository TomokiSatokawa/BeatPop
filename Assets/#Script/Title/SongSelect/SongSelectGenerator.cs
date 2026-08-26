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
        [SerializeField] private SongInfoControl _songInfoControl;

        private List<GameObject> _clonedObject = new();
        private string _keyword = "";

        public void InitialView()
        {
            DeleteChildren();
            AddContents("Ç®Ç∑Ç∑Çﬂ", SongRecommender.I.GetRecommendation());
            AddContents("ç≈ãﬂÇÃÉvÉåÉC", SongRecommender.I.GetPlayHistory());
        }

        private void AddContents(string title , IReadOnlyList<SongSelectData> songSelectDatas)
        {
            if (songSelectDatas == null || songSelectDatas.Count == 0) return;

            AddTitle(title);
            AddContentsList(songSelectDatas);
        }


        public void KeywordView(string keyword)
        {
            DeleteChildren();
            AddContentsList(SongRecommender.I.GetKeywordSong(keyword));
        }

        public void UpdateSelect()
        {
            OnChangeSearchField(_keyword);
        }

        public void OnChangeSearchField(string text)
        {
            if (text == "")
            {
                InitialView();
            }
            else
            {
                KeywordView(text);
            }

            _keyword = text;
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
            list.ViewList(datas, _listSongPrefab, OnSelect);
            _clonedObject.Add(list.gameObject);
        }

        private void OnSelect(SongSelectData data)
        {
            _songInfoControl.ShowInfo(data);
        }

        protected override void OnDeletedChildren()
        {
            _clonedObject.Clear();
        }
    }
}