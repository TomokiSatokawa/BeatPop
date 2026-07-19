using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// インスペクターで表示できるDictionary
/// </summary>
[Serializable]
public class SerializableDictionary<TKey, TValue> : ISerializationCallbackReceiver
{
    [Serializable]
    public struct KeyPair
    {
        public TKey Key;
        public TValue Value;

        public KeyPair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }

    [SerializeField]
    private List<KeyPair> _items = new();

    // 内部的な実データ
    private Dictionary<TKey, TValue> _dictionary = new();

    // ランタイム（ゲーム実行中）にスクリプトから変更があったかを追跡するフラグ
    private bool _isDirty = false;

    public IReadOnlyList<KeyPair> Items => _items;


    public void OnBeforeSerialize()
    {
        // ゲーム実行中にスクリプト側（コード）から追加・変更があった場合のみ、List側に逆同期する
        if (Application.isPlaying && _isDirty)
        {
            _items.Clear();
            foreach (var kvp in _dictionary)
            {
                _items.Add(new KeyPair(kvp.Key, kvp.Value));
            }
            _isDirty = false;
        }
    }

    public void OnAfterDeserialize()
    {
        // インスペクターでの追加・削除・編集を、実行時にDictionaryへ正しくロードする
        _dictionary.Clear();
        foreach (var item in _items)
        {
            if (item.Key != null && !_dictionary.ContainsKey(item.Key))
            {
                _dictionary[item.Key] = item.Value;
            }
        }
    }

    // --- ディクショナリ操作 ---

    public Dictionary<TKey, TValue> ToDictionary()
    {
        return new Dictionary<TKey, TValue>(_dictionary);
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        return _dictionary.TryGetValue(key, out value);
    }

    public TValue this[TKey key]
    {
        get
        {
            if (_dictionary.TryGetValue(key, out var value))
            {
                return value;
            }
            throw new KeyNotFoundException($"Key '{key}' が見つかりません。");
        }
        set
        {
            _dictionary[key] = value;
            _isDirty = true; // スクリプトから変更されたのでフラグを立てる
        }
    }

    // コード側から要素を削除したい場合用
    public bool Remove(TKey key)
    {
        if (_dictionary.Remove(key))
        {
            _isDirty = true; // スクリプトから変更されたのでフラグを立てる
            return true;
        }
        return false;
    }
}