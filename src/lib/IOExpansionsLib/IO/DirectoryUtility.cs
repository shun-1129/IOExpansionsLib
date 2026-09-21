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
            return Directory
                .GetDirectories ( path , "*" , SearchOption.TopDirectoryOnly )
                .Select ( dir => new DirectoryEntityInfo ( dir ) );
        }

        /// <summary>
        /// ディレクトリを取得する（ジェネリック版）
        /// </summary>
        /// <typeparam name="T">ディレクトリ情報の型</typeparam>
        /// <param name="path">検索するディレクトリのパス</param>
        /// <returns>ディレクトリのコレクション</returns>
        public static IEnumerable<T> GetDirectories<T> ( string path ) where T : DirectoryEntityInfo , new ()
        {
            return Directory
                .GetDirectories ( path , "*" , SearchOption.TopDirectoryOnly )
                .Select ( dir => new T { DirectoryPath = dir } );
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
        /// ディレクトリを取得する（ジェネリック版）
        /// </summary>
        /// <typeparam name="T">ディレクトリ情報の型</typeparam>
        /// <param name="path">検索するディレクトリのパス</param>
        /// <param name="sortPattern">並び替えパターン</param>
        /// <returns>ディレクトリのコレクション</returns>
        public static IEnumerable<T> GetDirectories<T> ( string path , SortOrderPattern sortPattern = SortOrderPattern.None ) where T : DirectoryEntityInfo , new ()
        {
            IEnumerable<T> directories = GetDirectories<T> ( path );
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
        /// ディレクトリを取得する（ジェネリック版）
        /// </summary>
        /// <typeparam name="T">ディレクトリ情報の型</typeparam>
        /// <param name="path">検索するディレクトリのパス</param>
        /// <param name="searchPattern">検索パターン</param>
        /// <returns>ディレクトリのコレクション</returns>
        public static IEnumerable<T> GetDirectories<T> ( string path , string searchPattern ) where T : DirectoryEntityInfo , new ()
        {
            return Directory
                .GetDirectories ( path , NormalizeSearchPattern ( searchPattern ) , SearchOption.TopDirectoryOnly )
                .Select ( dir => new T { DirectoryPath = dir } );
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
        /// ディレクトリを取得する（ジェネリック版）
        /// </summary>
        /// <typeparam name="T">ディレクトリ情報の型</typeparam>
        /// <param name="path">検索するディレクトリのパス</param>
        /// <param name="searchPattern">検索パターン</param>
        /// <param name="sortPattern">並び替えパターン</param>
        /// <returns>ディレクトリのコレクション</returns>
        public static IEnumerable<T> GetDirectories<T> ( string path , string searchPattern , SortOrderPattern sortPattern = SortOrderPattern.None ) where T : DirectoryEntityInfo , new ()
        {
            IEnumerable<T> directories = GetDirectories<T> ( path , searchPattern );
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
        /// ディレクトリを取得する（ジェネリック版）
        /// </summary>
        /// <typeparam name="T">ディレクトリ情報の型</typeparam>
        /// <param name="path">検索するディレクトリのパス</param>
        /// <param name="searchOption">検索オプション</param>
        /// <returns>ディレクトリのコレクション</returns>
        public static IEnumerable<T> GetDirectories<T> ( string path , SearchOption searchOption ) where T : DirectoryEntityInfo , new ()
        {
            return Directory.GetDirectories ( path , "*" , searchOption ).Select ( dir => new T { DirectoryPath = dir } );
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
        /// ディレクトリを取得する（ジェネリック版）
        /// </summary>
        /// <typeparam name="T">ディレクトリ情報の型</typeparam>
        /// <param name="path">検索するディレクトリのパス</param>
        /// <param name="searchOption">検索オプション</param>
        /// <param name="sortPattern">並び替えパターン</param>
        /// <returns>ディレクトリのコレクション</returns>
        public static IEnumerable<T> GetDirectories<T> ( string path , SearchOption searchOption , SortOrderPattern sortPattern = SortOrderPattern.None ) where T : DirectoryEntityInfo , new ()
        {
            IEnumerable<T> directories = GetDirectories<T> ( path , searchOption );
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
        /// ディレクトリを取得する（ジェネリック版）
        /// </summary>
        /// <typeparam name="T">ディレクトリ情報の型</typeparam>
        /// <param name="path">検索するディレクトリのパス</param>
        /// <param name="searchPattern">検索パターン</param>
        /// <param name="searchOption">検索オプション</param>
        /// <returns>ディレクトリのコレクション</returns>
        public static IEnumerable<T> GetDirectories<T> ( string path , string searchPattern , SearchOption searchOption ) where T : DirectoryEntityInfo , new ()
        {
            return Directory.GetDirectories ( path , NormalizeSearchPattern ( searchPattern ) , searchOption ).Select ( dir => new T { DirectoryPath = dir } );
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
        /// ディレクトリを取得する（ジェネリック版）
        /// </summary>
        /// <typeparam name="T">ディレクトリ情報の型</typeparam>
        /// <param name="path">検索するディレクトリのパス</param>
        /// <param name="searchPattern">検索パターン</param>
        /// <param name="searchOption">検索オプション</param>
        /// <param name="sortPattern">並び替えパターン</param>
        /// <returns>ディレクトリのコレクション</returns>
        public static IEnumerable<T> GetDirectories<T> ( string path , string searchPattern , SearchOption searchOption , SortOrderPattern sortPattern = SortOrderPattern.None ) where T : DirectoryEntityInfo , new ()
        {
            IEnumerable<T> directories = GetDirectories<T> ( path , searchPattern , searchOption );
            return SortDirectories ( directories , sortPattern );
        }

        /// <summary>
        /// ディレクトリコレクションを並び替える
        /// </summary>
        /// <param name="directories">ディレクトリのコレクション</param>
        /// <param name="sortPattern">並び替えパターン</param>
        /// <returns>並び替え後のディレクトリコレクション</returns>
        private static IEnumerable<T> SortDirectories<T> ( IEnumerable<T> directories , SortOrderPattern sortPattern ) where T : DirectoryEntityInfo , new ()
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
        public static void Move ( DirectoryEntityInfo sourceDirectory , string destinationPath )
        {
            if ( !Directory.Exists ( sourceDirectory.DirectoryPath ) )
            {
                throw new DirectoryNotFoundException ( $"Source directory '{sourceDirectory.DirectoryPath}' does not exist." );
            }

            if ( !Directory.Exists ( destinationPath ) )
            {
                throw new DirectoryNotFoundException ( $"Destination directory '{destinationPath}' does not exist." );
            }

            if ( !IsValidName ( sourceDirectory.DirectoryName ) )
            {
                throw new ArgumentException ( $"Invalid directory name '{sourceDirectory.DirectoryName}'." );
            }

            string destinationDirectoryPath = Path.Combine ( destinationPath , sourceDirectory.DirectoryName );

            if ( Directory.Exists ( destinationDirectoryPath ) )
            {
                throw new IOException ( $"Destination directory '{destinationDirectoryPath}' already exists." );
            }

            Directory.Move ( sourceDirectory.DirectoryPath , destinationDirectoryPath );
        }

        /// <summary>
        /// ディレクトリを移動する（ジェネリック版）
        /// </summary>
        /// <typeparam name="T">ディレクトリ情報の型</typeparam>
        /// <param name="sourceDirectory">移動するディレクトリの情報</param>
        /// <param name="destinationPath">移動先のパス</param>
        public static void Move<T> ( T sourceDirectory , string destinationPath ) where T : DirectoryEntityInfo , new ()
        {
            DirectoryEntityInfo srcDir = new DirectoryEntityInfo ( sourceDirectory.DirectoryPath );
            Move ( srcDir , destinationPath );
        }

        /// <summary>
        /// ディレクトリを移動する（既存のディレクトリが存在する場合はタイムスタンプを付加して移動）
        /// </summary>
        /// <param name="sourceDirectory">移動するディレクトリの情報</param>
        /// <param name="destinationPath">移動先のパス</param>
        /// <returns>操作結果</returns>
        public static void MoveWithTimestampIfExists ( DirectoryEntityInfo sourceDirectory , string destinationPath )
        {
            if ( !Directory.Exists ( sourceDirectory.DirectoryPath ) )
            {
                throw new DirectoryNotFoundException ( $"Source directory '{sourceDirectory.DirectoryPath}' does not exist." );
            }

            if ( !Directory.Exists ( destinationPath ) )
            {
                throw new DirectoryNotFoundException ( $"Destination directory '{destinationPath}' does not exist." );
            }

            if ( !IsValidName ( sourceDirectory.DirectoryName ) )
            {
                throw new ArgumentException ( $"Invalid directory name '{sourceDirectory.DirectoryName}'." );
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

        /// <summary>
        /// ディレクトリを移動する（既存のディレクトリが存在する場合はタイムスタンプを付加して移動、ジェネリック版）
        /// </summary>
        /// <typeparam name="T">ディレクトリ情報の型</typeparam>
        /// <param name="sourceDirectory">移動するディレクトリの情報</param>
        /// <param name="destinationPath">移動先のパス</param>
        public static void MoveWithTimestampIfExists<T> ( T sourceDirectory , string destinationPath ) where T : DirectoryEntityInfo , new ()
        {
            DirectoryEntityInfo srcDir = new DirectoryEntityInfo ( sourceDirectory.DirectoryPath );
            MoveWithTimestampIfExists ( srcDir , destinationPath );
        }
        #endregion

        #region ディレクトリコピー
        /// <summary>
        /// ディレクトリをコピーする
        /// </summary>
        /// <param name="sourceDirectory">コピー元のディレクトリ情報</param>
        /// <param name="destinationPath">コピー先のディレクトリパス</param>
        /// <exception cref="DirectoryNotFoundException">ディレクトリが存在しない場合にスローされます</exception>
        /// <exception cref="ArgumentException">無効なディレクトリ名の場合にスローされます</exception>
        /// <exception cref="IOException">コピー先のディレクトリが既に存在する場合にスローされます</exception>
        public static void Copy ( DirectoryEntityInfo sourceDirectory , string destinationPath )
        {
            if ( !Directory.Exists ( sourceDirectory.DirectoryPath ) )
            {
                throw new DirectoryNotFoundException ( $"Source directory '{sourceDirectory.DirectoryPath}' does not exist." );
            }

            if ( !Directory.Exists ( destinationPath ) )
            {
                throw new DirectoryNotFoundException ( $"Destination directory '{destinationPath}' does not exist." );
            }

            if ( !IsValidName ( sourceDirectory.DirectoryName ) )
            {
                throw new ArgumentException ( $"Invalid directory name '{sourceDirectory.DirectoryName}'." );
            }

            string destinationDirectoryPath = Path.Combine ( destinationPath , sourceDirectory.DirectoryName );
            if ( Directory.Exists ( destinationDirectoryPath ) )
            {
                throw new IOException ( $"Destination directory '{destinationDirectoryPath}' already exists." );
            }

            CopyDirectoryRecursively ( sourceDirectory , destinationDirectoryPath );
        }

        /// <summary>
        /// ディレクトリをコピーする（ジェネリック版）
        /// </summary>
        /// <typeparam name="T">ディレクトリ情報の型</typeparam>
        /// <param name="sourceDirectory">コピー元のディレクトリ情報</param>
        /// <param name="destinationPath">コピー先のディレクトリパス</param>
        public static void Copy<T> ( T sourceDirectory , string destinationPath ) where T : DirectoryEntityInfo , new ()
        {
            DirectoryEntityInfo srcDir = new DirectoryEntityInfo ( sourceDirectory.DirectoryPath );
            Copy ( srcDir , destinationPath );
        }

        /// <summary>
        /// ディレクトリをコピーする（既存のディレクトリが存在する場合はタイムスタンプを付加してコピー）
        /// </summary>
        /// <param name="sourceDirectory">コピー元のディレクトリ情報</param>
        /// <param name="destinationPath">コピー先のディレクトリパス</param>
        /// <exception cref="DirectoryNotFoundException">ディレクトリが存在しない場合にスローされます</exception>
        /// <exception cref="ArgumentException">無効なディレクトリ名の場合にスローされます</exception>
        public static void CopyWithTimestampIfExists ( DirectoryEntityInfo sourceDirectory , string destinationPath )
        {
            if ( !Directory.Exists ( sourceDirectory.DirectoryPath ) )
            {
                throw new DirectoryNotFoundException ( $"Source directory '{sourceDirectory.DirectoryPath}' does not exist." );
            }

            if ( !Directory.Exists ( destinationPath ) )
            {
                throw new DirectoryNotFoundException ( $"Destination directory '{destinationPath}' does not exist." );
            }

            if ( !IsValidName ( sourceDirectory.DirectoryName ) )
            {
                throw new ArgumentException ( $"Invalid directory name '{sourceDirectory.DirectoryName}'." );
            }

            string destinationDirectoryPath = Path.Combine ( destinationPath , sourceDirectory.DirectoryName );
            if ( !Directory.Exists ( destinationDirectoryPath ) )
            {
                CopyDirectoryRecursively ( sourceDirectory , destinationDirectoryPath );
                return;
            }

            // ディレクトリが存在する場合、タイムスタンプを付加してコピー
            string timestamp = DateTime.Now.ToString ( "yyyyMMddHHmmssfff" );
            string newDirectoryName = $"{sourceDirectory.DirectoryName}_{timestamp}";
            string newDestinationDirectoryPath = Path.Combine ( destinationPath , newDirectoryName );
            CopyDirectoryRecursively ( sourceDirectory , newDestinationDirectoryPath );
        }

        /// <summary>
        /// ディレクトリをコピーする（既存のディレクトリが存在する場合はタイムスタンプを付加してコピー、ジェネリック版）
        /// </summary>
        /// <typeparam name="T">ディレクトリ情報の型</typeparam>
        /// <param name="sourceDirectory">コピー元のディレクトリ情報</param>
        /// <param name="destinationPath">コピー先のディレクトリパス</param>
        public static void CopyWithTimestampIfExists<T> ( T sourceDirectory , string destinationPath ) where T : DirectoryEntityInfo , new ()
        {
            DirectoryEntityInfo srcDir = new DirectoryEntityInfo ( sourceDirectory.DirectoryPath );
            CopyWithTimestampIfExists ( srcDir , destinationPath );
        }

        /// <summary>
        /// 再帰的にディレクトリをコピーする
        /// </summary>
        /// <param name="sourceDirectoryEntityInfo">コピー元のディレクトリ情報</param>
        /// <param name="destinationDir">コピー先のディレクトリパス</param>
        private static void CopyDirectoryRecursively ( DirectoryEntityInfo sourceDirectoryEntityInfo , string destinationDir )
        {
            Directory.CreateDirectory ( destinationDir );

            IEnumerable<FileEntityInfo> files = FileUtility.GetFiles ( sourceDirectoryEntityInfo.DirectoryPath , SearchOption.TopDirectoryOnly );

            // コピー元ディレクトリ内のファイルをコピー
            foreach ( FileEntityInfo file in files )
            {
                FileUtility.Copy ( file , destinationDir );
            }

            IEnumerable<DirectoryEntityInfo> subDirectories = GetDirectories ( sourceDirectoryEntityInfo.DirectoryPath , SearchOption.TopDirectoryOnly );
            foreach ( DirectoryEntityInfo subDirectory in subDirectories )
            {
                string subDestinationDir = Path.Combine ( destinationDir , subDirectory.DirectoryName );
                CopyDirectoryRecursively ( subDirectory , subDestinationDir );
            }
        }
        #endregion

        #region ディレクトリ削除
        /// <summary>
        /// ディレクトリを削除する
        /// </summary>
        /// <param name="directory">削除するディレクトリ情報</param>
        /// <param name="recursive">サブディレクトリも再帰的に削除するかどうか</param>
        /// <exception cref="DirectoryNotFoundException">ディレクトリが存在しない場合にスローされます</exception>
        public static void Delete ( DirectoryEntityInfo directory , bool recursive = false )
        {
            if ( !Directory.Exists ( directory.DirectoryPath ) )
            {
                throw new DirectoryNotFoundException ( $"Directory '{directory.DirectoryPath}' does not exist." );
            }

            Directory.Delete ( directory.DirectoryPath , recursive );
        }

        /// <summary>
        /// ディレクトリを削除する（ジェネリック版）
        /// </summary>
        /// <typeparam name="T">ディレクトリ情報の型</typeparam>
        /// <param name="directory">削除するディレクトリ情報</param>
        /// <param name="recursive">サブディレクトリも再帰的に削除するかどうか</param>
        public static void Delete<T> ( T directory , bool recursive = false ) where T : DirectoryEntityInfo , new ()
        {
            DirectoryEntityInfo dir = new DirectoryEntityInfo ( directory.DirectoryPath );
            Delete ( dir , recursive );
        }

        /// <summary>
        /// 複数のディレクトリを削除する
        /// </summary>
        /// <param name="directories">削除するディレクトリ情報のコレクション</param>
        /// <param name="recursive">サブディレクトリも再帰的に削除するかどうか</param>
        public static void DeleteDirectories ( IEnumerable<DirectoryEntityInfo> directories , bool recursive = false )
        {
            foreach ( DirectoryEntityInfo directory in directories )
            {
                Delete ( directory , recursive );
            }
        }

        /// <summary>
        /// 複数のディレクトリを削除する（ジェネリック版）
        /// </summary>
        /// <typeparam name="T">ディレクトリ情報の型</typeparam>
        /// <param name="directories">削除するディレクトリ情報のコレクション</param>
        /// <param name="recursive">サブディレクトリも再帰的に削除するかどうか</param>
        public static void DeleteDirectories<T> ( IEnumerable<T> directories , bool recursive = false ) where T : DirectoryEntityInfo , new ()
        {
            foreach ( T directory in directories )
            {
                Delete ( directory , recursive );
            }
        }
        #endregion
    }
}
