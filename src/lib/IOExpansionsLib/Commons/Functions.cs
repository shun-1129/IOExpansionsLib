namespace IOExpansionsLib.Commons
{
    /// <summary>
    /// 共通の関数をまとめたクラスです。
    /// </summary>
    public static class Functions
    {
        /// <summary>
        /// パスの階層の深さを取得する
        /// </summary>
        /// <param name="path">対象のパス</param>
        /// <returns>階層の深さ</returns>
        public static int GetPathDepth ( string path )
        {
            return path.Split (
                new[] {
                    Path.DirectorySeparatorChar ,
                    Path.AltDirectorySeparatorChar } ,
                StringSplitOptions.RemoveEmptyEntries ).Length;
        }

        /// <summary>
        /// 検索パターンを正規化する
        /// </summary>
        /// <param name="searchPattern">検索パターン</param>
        /// <returns>正規化された検索パターン</returns>
        public static string NormalizeSearchPattern ( string searchPattern ) => string.IsNullOrEmpty ( searchPattern ) ? "*" : searchPattern;

        /// <summary>
        /// 名称が有効かどうかを判定する
        /// </summary>
        /// <param name="name">判定する名称</param>
        /// <returns>
        /// 有効な名称：<see langword="true"/><br/>
        /// 無効な名称：<see langword="false"/>
        /// </returns>
        internal static bool IsValidName ( string name )
        {
            if ( string.IsNullOrWhiteSpace ( name ) )
            {
                return false;
            }

            char[] invalidChars = Path.GetInvalidFileNameChars ();

            if ( name.Any ( invalidChars.Contains ) )
            {
                return false;
            }

            if ( name.EndsWith ( '.' ) || name.EndsWith ( ' ' ) )
            {
                return false;
            }

            return true;
        }
    }
}
