namespace Title.PlayerData
{
    /// <summary>
    /// プレイヤー名が適正かを判断する
    /// </summary>
    public static class PlayerNameValidator
    {
        public const int MaxPlayerNameLength = 8;

        public static string  IsValid(string playerName)
        {
            //Nullを除外
            if (string.IsNullOrWhiteSpace(playerName))
                return $"プレイヤー名が入力されていません";

            //長さ確認
            if (playerName.Length == 0 || playerName.Length > MaxPlayerNameLength)
                return $"1文字以上{MaxPlayerNameLength}文字以下にしてください";

            return "";
        }
    }
}