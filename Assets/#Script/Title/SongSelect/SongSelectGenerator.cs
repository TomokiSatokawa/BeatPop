using System.Collections.Generic;
using Common.UI;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Title.SongSelect
{
    public class SongSelectGenerator : ScrollViewBase
    {
        [SerializeField] private TextMeshProUGUI _titlePrefab;
        [SerializeField] private RectTransform _layoutGroup;
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
            ClonedActive();
        }

        private void AddContents(string title, IReadOnlyList<SongSelectData> songSelectDatas)
        {
            if (songSelectDatas == null || songSelectDatas.Count == 0) return;

            AddTitle(title);
            AddContentsList(songSelectDatas);
        }


        public void KeywordView(string keyword)
        {
            DeleteChildren();
            AddContentsList(SongRecommender.I.GetKeywordSong(keyword));
            ClonedActive();
        }

        public void UpdateSelect()
        {
            OnChangeSearchField(_keyword);
            ClonedActive();
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

        private void ClonedActive()
        {
            foreach (var obj in _clonedObject)
                obj.gameObject.SetActive(true);

            LayoutRebuilder.ForceRebuildLayoutImmediate(_layoutGroup);
        }

        private void AddTitle(string text)
        {
            var title = InstantiateContent(_titlePrefab);
            title.text = text;
            title.gameObject.SetActive(false);
            _clonedObject.Add(title.gameObject);
        }

        private void AddContentsList(IReadOnlyList<SongSelectData> datas)
        {
            var list = InstantiateContent(_contentsListPrefab);
            list.ViewList(datas, _listSongPrefab, OnSelect);
            list.gameObject.SetActive(false);
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