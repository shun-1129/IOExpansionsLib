using IOExpansionsLib.Models;
using static IOExpansionsLib.Commons.Constants;
using static IOExpansionsLib.Commons.Functions;

namespace IOExpansionsLib.IO
{
    /// <summary>
    /// ディレクトリ操作に関するユーティリティクラスです。
    /// </summary>
    public static class DirectoryUtility
    {
        #region ディレクトリ取得
        /// <summary>
        /// ディレクトリを取得する
        /// </summary>
        /// <param name="path">検索するディレクトリのパス</param>
        /// <returns>ディレクトリのコレクション</returns>
        public static IEnumerable<DirectoryEntityInfo> GetDirectories ( string path )
        {
            return Directory.GetDirectories ( path , "*" , SearchOption.TopDirectoryOnly ).Select ( dir => new DirectoryEntityInfo ( dir ) );
        }

        /// <summary>
        /// ディレクトリを取得する
        /// </summary>
        /// <param name="path">検索するディレクトリのパス</param>
        /// <param name="sortPattern">並び替えパターン</param>
        /// <returns>ディレクトリのコレクション</returns>
        public static IEnumerable<DirectoryEntityInfo> GetDirectories ( string path , SortOrderPattern sortPattern = SortOrderPattern.None )
        {
            IEnumerable<DirectoryEntityInfo> directories = GetDirectories ( path );
            return SortDirectories ( directories , sortPattern );
        }

        /// <summary>
        /// ディレクトリを取得する
        /// </summary>
        /// <param name="path">検索するディレクトリのパス</param>
        /// <param name="searchPattern">検索パターン</param>
        /// <returns>ディレクトリのコレクション</returns>
        public static IEnumerable<DirectoryEntityInfo> GetDirectories ( string path , string searchPattern )
        {
            return Directory
                .GetDirectories ( path , NormalizeSearchPattern ( searchPattern ) , SearchOption.TopDirectoryOnly )
                .Select ( dir => new DirectoryEntityInfo ( dir ) );
        }

        /// <summary>
        /// ディレクトリを取得する
        /// </summary>
        /// <param name="path">検索するディレクトリのパス</param>
        /// <param name="searchPattern">検索パターン</param>
        /// <param name="sortPattern">並び替えパターン</param>
        /// <returns>ディレクトリのコレクション</returns>
        public static IEnumerable<DirectoryEntityInfo> GetDirectories ( string path , string searchPattern , SortOrderPattern sortPattern = SortOrderPattern.None )
        {
            IEnumerable<DirectoryEntityInfo> directories = GetDirectories ( path , searchPattern );
            return SortDirectories ( directories , sortPattern );
        }

        /// <summary>
        /// ディレクトリを取得する
        /// </summary>
        /// <param name="path">検索するディレクトリのパス</param>
        /// <param name="searchOption">検索オプション</param>
        /// <returns>ディレクトリのコレクション</returns>
        public static IEnumerable<DirectoryEntityInfo> GetDirectories ( string path , SearchOption searchOption )
        {
            return Directory.GetDirectories ( path , "*" , searchOption ).Select ( dir => new DirectoryEntityInfo ( dir ) );
        }

        /// <summary>
        /// ディレクトリを取得する
        /// </summary>
        /// <param name="path">検索するディレクトリのパス</param>
        /// <param name="searchOption">検索オプション</param>
        /// <param name="sortPattern">並び替えパターン</param>
        /// <returns>ディレクトリのコレクション</returns>
        public static IEnumerable<DirectoryEntityInfo> GetDirectories ( string path , SearchOption searchOption , SortOrderPattern sortPattern = SortOrderPattern.None )
        {
            IEnumerable<DirectoryEntityInfo> directories = GetDirectories ( path , searchOption );
            return SortDirectories ( directories , sortPattern );
        }

        /// <summary>
        /// ディレクトリを取得する
        /// </summary>
        /// <param name="path">検索するディレクトリのパス</param>
        /// <param name="searchPattern">検索パターン</param>
        /// <param name="searchOption">検索オプション</param>
        /// <returns>ディレクトリのコレクション</returns>
        public static IEnumerable<DirectoryEntityInfo> GetDirectories ( string path , string searchPattern , SearchOption searchOption )
        {
            return Directory.GetDirectories ( path , NormalizeSearchPattern ( searchPattern ) , searchOption ).Select ( dir => new DirectoryEntityInfo ( dir ) );
        }

        /// <summary>
        /// ディレクトリを取得する
        /// </summary>
        /// <param name="path">検索するディレクトリのパス</param>
        /// <param name="searchPattern">検索パターン</param>
        /// <param name="searchOption">検索オプション</param>
        /// <param name="sortPattern">並び替えパターン</param>
        /// <returns>ディレクトリのコレクション</returns>
        public static IEnumerable<DirectoryEntityInfo> GetDirectories ( string path , string searchPattern , SearchOption searchOption , SortOrderPattern sortPattern = SortOrderPattern.None )
        {
            IEnumerable<DirectoryEntityInfo> directories = GetDirectories ( path , searchPattern , searchOption );
            return SortDirectories ( directories , sortPattern );
        }

        /// <summary>
        /// ディレクトリコレクションを並び替える
        /// </summary>
        /// <param name="directories">ディレクトリのコレクション</param>
        /// <param name="sortPattern">並び替えパターン</param>
        /// <returns>並び替え後のディレクトリコレクション</returns>
        private static IEnumerable<DirectoryEntityInfo> SortDirectories ( IEnumerable<DirectoryEntityInfo> directories , SortOrderPattern sortPattern )
        {
            return sortPattern switch
            {
                SortOrderPattern.Ascending => directories.OrderBy ( dir => dir.DirectoryName , StringComparer.Ordinal ),
                SortOrderPattern.Descending => directories.OrderByDescending ( dir => dir.DirectoryName , StringComparer.Ordinal ),
                SortOrderPattern.PathDepthAscending => directories.OrderBy ( dir => GetPathDepth ( dir.DirectoryPath ) ),
                SortOrderPattern.PathDepthDescending => directories.OrderByDescending ( dir => GetPathDepth ( dir.DirectoryPath ) ),
                _ => directories
            };
        }
        #endregion

        #region ディレクトリ移動
        /// <summary>
        /// ディレクトリを移動する
        /// </summary>
        /// <param name="sourceDirectory">移動するディレクトリの情報</param>
        /// <param name="destinationPath">移動先のパス</param>
        public static void MoveDirectory ( DirectoryEntityInfo sourceDirectory , string destinationPath )
        {
            if ( !Directory.Exists ( sourceDirectory.DirectoryPath ) )
            {
                throw new DirectoryNotFoundException ( $"Source directory '{sourceDirectory.DirectoryPath}' does not exist." );
            }

            if ( !Directory.Exists ( destinationPath ) )
            {
                throw new DirectoryNotFoundException ( $"Destination directory '{destinationPath}' does not exist." );
            }

            string destinationDirectoryPath = Path.Combine ( destinationPath , sourceDirectory.DirectoryName );

            if ( Directory.Exists ( destinationDirectoryPath ) )
            {
                throw new IOException ( $"Destination directory '{destinationDirectoryPath}' already exists." );
            }

            Directory.Move ( sourceDirectory.DirectoryPath , destinationDirectoryPath );
        }

        /// <summary>
        /// ディレクトリを移動する（既存のディレクトリが存在する場合はタイムスタンプを付加して移動）
        /// </summary>
        /// <param name="sourceDirectory">移動するディレクトリの情報</param>
        /// <param name="destinationPath">移動先のパス</param>
        /// <returns>操作結果</returns>
        public static void MoveDirectoryWithTimestampIfExists ( DirectoryEntityInfo sourceDirectory , string destinationPath )
        {
            if ( !Directory.Exists ( sourceDirectory.DirectoryPath ) )
            {
                throw new DirectoryNotFoundException ( $"Source directory '{sourceDirectory.DirectoryPath}' does not exist." );
            }

            if ( !Directory.Exists ( destinationPath ) )
            {
                throw new DirectoryNotFoundException ( $"Destination directory '{destinationPath}' does not exist." );
            }

            string destinationDirectoryPath = Path.Combine ( destinationPath , sourceDirectory.DirectoryName );

            if ( !Directory.Exists ( destinationDirectoryPath ) )
            {
                Directory.Move ( sourceDirectory.DirectoryPath , destinationDirectoryPath );
                return;
            }

            // ディレクトリが存在する場合、タイムスタンプを付加して移動
            string timestamp = DateTime.Now.ToString ( "yyyyMMddHHmmssfff" );
            string newDirectoryName = $"{sourceDirectory.DirectoryName}_{timestamp}";
            string newDestinationDirectoryPath = Path.Combine ( destinationPath , newDirectoryName );
            Directory.Move ( sourceDirectory.DirectoryPath , newDestinationDirectoryPath );
        }
        #endregion
    }
}
