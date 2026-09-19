namespace IOExpansionsLib.Commons
{
    /// <summary>
    /// 定数をまとめたクラスです。
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// 並び替え順序のパターンを表す列挙型です。
        /// </summary>
        public enum SortOrderPattern
        {
            /// <summary>
            /// 並び替え順序が指定されていないことを表します。
            /// </summary>
            None,
            /// <summary>
            /// 昇順を表します。
            /// </summary>
            Ascending,
            /// <summary>
            /// 降順を表します。
            /// </summary>
            Descending,
            /// <summary>
            /// パスの深さに基づいて昇順で並び替えることを表します。
            /// </summary>
            PathDepthAscending,
            /// <summary>
            /// パスの深さに基づいて降順で並び替えることを表します。
            /// </summary>
            PathDepthDescending
        }
    }
}
