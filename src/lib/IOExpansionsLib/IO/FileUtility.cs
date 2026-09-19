using IOExpansionsLib.Models;
using static IOExpansionsLib.Commons.Constants;
using static IOExpansionsLib.Commons.Functions;

namespace IOExpansionsLib.IO
{
    /// <summary>
    /// ファイル操作に関するユーティリティクラスです。
    /// </summary>
    public static class FileUtility
    {
        #region ファイル取得
        /// <summary>
        /// ファイルを取得する
        /// </summary>
        /// <param name="path">検索するディレクトリのパス</param>
        /// <returns>ファイルのコレクション</returns>
        public static IEnumerable<FileEntityInfo> GetFiles ( string path )
        {
            return Directory
                .GetFiles ( path , "*" , SearchOption.TopDirectoryOnly )
                .Select ( file => new FileEntityInfo ( file ) );
        }

        /// <summary>
        /// ファイルを取得する
        /// </summary>
        /// <param name="path">検索するディレクトリのパス</param>
        /// <param name="sortPattern">並び替えパターン</param>
        /// <returns>ファイルのコレクション</returns>
        public static IEnumerable<FileEntityInfo> GetFiles ( string path , SortOrderPattern sortPattern = SortOrderPattern.None )
        {
            IEnumerable<FileEntityInfo> files = GetFiles ( path );
            return SortFiles ( files , sortPattern );
        }

        /// <summary>
        /// ファイルを取得する
        /// </summary>
        /// <param name="path">検索するディレクトリのパス</param>
        /// <param name="searchPattern">検索パターン</param>
        /// <returns>ファイルのコレクション</returns>
        public static IEnumerable<FileEntityInfo> GetFiles ( string path , string searchPattern )
        {
            return Directory
                .GetFiles ( path , NormalizeSearchPattern ( searchPattern ) , SearchOption.TopDirectoryOnly )
                .Select ( file => new FileEntityInfo ( file ) );
        }

        /// <summary>
        /// ファイルを取得する
        /// </summary>
        /// <param name="path">検索するディレクトリのパス</param>
        /// <param name="searchPattern">検索パターン</param>
        /// <param name="sortPattern">並び替えパターン</param>
        /// <returns>ファイルのコレクション</returns>
        public static IEnumerable<FileEntityInfo> GetFiles ( string path , string searchPattern , SortOrderPattern sortPattern = SortOrderPattern.None )
        {
            IEnumerable<FileEntityInfo> files = GetFiles ( path , searchPattern );
            return SortFiles ( files , sortPattern );
        }

        /// <summary>
        /// ファイルを取得する
        /// </summary>
        /// <param name="path">検索するディレクトリのパス</param>
        /// <param name="searchOption">検索オプション</param>
        /// <returns>ファイルのコレクション</returns>
        public static IEnumerable<FileEntityInfo> GetFiles ( string path , SearchOption searchOption )
        {
            return Directory
                .GetFiles ( path , "*" , searchOption )
                .Select ( file => new FileEntityInfo ( file ) );
        }

        /// <summary>
        /// ファイルを取得する
        /// </summary>
        /// <param name="path">検索するディレクトリのパス</param>
        /// <param name="searchOption">検索オプション</param>
        /// <param name="sortPattern">並び替えパターン</param>
        /// <returns>ファイルのコレクション</returns>
        public static IEnumerable<FileEntityInfo> GetFiles ( string path , SearchOption searchOption , SortOrderPattern sortPattern = SortOrderPattern.None )
        {
            IEnumerable<FileEntityInfo> files = GetFiles ( path , searchOption );
            return SortFiles ( files , sortPattern );
        }

        /// <summary>
        /// ファイルを取得する
        /// </summary>
        /// <param name="path">検索するディレクトリのパス</param>
        /// <param name="searchPattern">検索パターン</param>
        /// <param name="searchOption">検索オプション</param>
        /// <returns>ファイルのコレクション</returns>
        public static IEnumerable<FileEntityInfo> GetFiles ( string path , string searchPattern , SearchOption searchOption )
        {
            return Directory
                .GetFiles ( path , NormalizeSearchPattern ( searchPattern ) , searchOption )
                .Select ( file => new FileEntityInfo ( file ) );
        }

        /// <summary>
        /// ファイルを取得する
        /// </summary>
        /// <param name="path">検索するディレクトリのパス</param>
        /// <param name="searchPattern">検索パターン</param>
        /// <param name="searchOption">検索オプション</param>
        /// <param name="sortPattern">並び替えパターン</param>
        /// <returns>ファイルのコレクション</returns>
        public static IEnumerable<FileEntityInfo> GetFiles ( string path , string searchPattern , SearchOption searchOption , SortOrderPattern sortPattern = SortOrderPattern.None )
        {
            IEnumerable<FileEntityInfo> files = GetFiles ( path , searchPattern , searchOption );
            return SortFiles ( files , sortPattern );
        }

        /// <summary>
        /// ファイルコレクションを並び替える
        /// </summary>
        /// <param name="files">ファイルのコレクション</param>
        /// <param name="sortPattern">並び替えパターン</param>
        /// <returns>並び替え後のファイルコレクション</returns>
        private static IEnumerable<FileEntityInfo> SortFiles ( IEnumerable<FileEntityInfo> files , SortOrderPattern sortPattern )
        {
            return sortPattern switch
            {
                SortOrderPattern.Ascending => files.OrderBy ( file => file.FileName , StringComparer.Ordinal ),
                SortOrderPattern.Descending => files.OrderByDescending ( file => file.FileName , StringComparer.Ordinal ),
                SortOrderPattern.PathDepthAscending => files.OrderBy ( file => GetPathDepth ( file.FilePath ) ),
                SortOrderPattern.PathDepthDescending => files.OrderByDescending ( file => GetPathDepth ( file.FilePath ) ),
                _ => files
            };
        }
        #endregion

        #region ファイル移動
        /// <summary>
        /// ファイルを移動する
        /// </summary>
        /// <remarks>
        /// ファイルが存在する場合は上書きされず、移動は失敗します。<br/>
        /// 上書きを許可する場合は、isOverwrite パラメータを true に設定してください。
        /// </remarks>
        /// <param name="sourceFile">移動するファイルの情報</param>
        /// <param name="destinationPath">移動先のディレクトリパス</param>
        /// <param name="isOverwrite">既存のファイルを上書きするかどうか</param>
        /// <exception cref="FileNotFoundException">コピー元のファイルが存在しない場合にスローされます。</exception>
        /// <exception cref="DirectoryNotFoundException">コピー先のディレクトリが存在しない場合にスローされます。</exception>
        /// <exception cref="ArgumentException">無効なファイル名の場合にスローされます。</exception>
        /// <exception cref="IOException">コピー先のファイルが既に存在する場合にスローされます。</exception>
        public static void Move ( FileEntityInfo sourceFile , string destinationPath , bool isOverwrite = false )
        {
            if ( !File.Exists ( sourceFile.FilePath ) )
            {
                throw new FileNotFoundException ( $"Source file '{sourceFile.FilePath}' does not exist." );
            }

            if ( !Directory.Exists ( destinationPath ) )
            {
                throw new DirectoryNotFoundException ( $"Destination directory '{destinationPath}' does not exist." );
            }

            if ( !IsValidName ( sourceFile.FileName ) )
            {
                throw new ArgumentException ( $"Invalid file name '{sourceFile.FileName}'." );
            }

            string destinationFilePath = Path.Combine ( destinationPath , sourceFile.FileName );

            if ( File.Exists ( destinationFilePath ) && !isOverwrite )
            {
                throw new IOException ( $"Destination file '{destinationFilePath}' already exists." );
            }

            File.Move ( sourceFile.FilePath , destinationFilePath , isOverwrite );
        }

        /// <summary>
        /// ファイルを移動する（既存のファイルが存在する場合はタイムスタンプを付加して移動）
        /// </summary>
        /// <param name="sourceFile">移動するファイルの情報</param>
        /// <param name="destinationPath">移動先のディレクトリパス</param>
        /// <exception cref="FileNotFoundException">コピー元のファイルが存在しない場合にスローされます。</exception>
        /// <exception cref="DirectoryNotFoundException">コピー先のディレクトリが存在しない場合にスローされます。</exception>
        /// <exception cref="ArgumentException">無効なファイル名の場合にスローされます。</exception>
        public static void MoveWithTimestampIfExists ( FileEntityInfo sourceFile , string destinationPath )
        {
            if ( !File.Exists ( sourceFile.FilePath ) )
            {
                throw new FileNotFoundException ( $"Source file '{sourceFile.FilePath}' does not exist." );
            }

            if ( !Directory.Exists ( destinationPath ) )
            {
                throw new DirectoryNotFoundException ( $"Destination directory '{destinationPath}' does not exist." );
            }

            string destinationFilePath = Path.Combine ( destinationPath , sourceFile.FileName );

            if ( !IsValidName ( sourceFile.FileName ) )
            {
                throw new ArgumentException ( $"Invalid file name '{sourceFile.FileName}'." );
            }

            if ( !File.Exists ( destinationFilePath ) )
            {
                File.Move ( sourceFile.FilePath , destinationFilePath );
                return;
            }

            // ファイルが存在する場合、タイムスタンプを付加して移動
            string timestamp = DateTime.Now.ToString ( "yyyyMMddHHmmssfff" );
            string newFileName = $"{sourceFile.FileNameWithoutExtension}_{timestamp}{sourceFile.FileExtension}";
            string newDestinationFilePath = Path.Combine ( destinationPath , newFileName );

            File.Move ( sourceFile.FilePath , newDestinationFilePath );
        }
        #endregion

        #region ファイルコピー
        /// <summary>
        /// ファイルをコピーする
        /// </summary>
        /// <param name="sourceFile">コピーするファイルの情報</param>
        /// <param name="destinationPath">コピー先のディレクトリパス</param>
        /// <param name="isOverwrite">既存のファイルを上書きするかどうか</param>
        /// <exception cref="FileNotFoundException">コピー元のファイルが存在しない場合にスローされます。</exception>
        /// <exception cref="DirectoryNotFoundException">コピー先のディレクトリが存在しない場合にスローされます。</exception>
        /// <exception cref="ArgumentException">無効なファイル名の場合にスローされます。</exception>
        /// <exception cref="IOException">コピー先のファイルが既に存在する場合にスローされます。</exception>
        public static void Copy ( FileEntityInfo sourceFile , string destinationPath , bool isOverwrite = false )
        {
            if ( !File.Exists ( sourceFile.FilePath ) )
            {
                throw new FileNotFoundException ( $"Source file '{sourceFile.FilePath}' does not exist." );
            }

            if ( !Directory.Exists ( destinationPath ) )
            {
                throw new DirectoryNotFoundException ( $"Destination directory '{destinationPath}' does not exist." );
            }

            if ( !IsValidName ( sourceFile.FileName ) )
            {
                throw new ArgumentException ( $"Invalid file name '{sourceFile.FileName}'." );
            }

            string destinationFilePath = Path.Combine ( destinationPath , sourceFile.FileName );

            if ( File.Exists ( destinationFilePath ) && !isOverwrite )
            {
                throw new IOException ( $"Destination file '{destinationFilePath}' already exists." );
            }

            File.Copy ( sourceFile.FilePath , destinationFilePath , isOverwrite );
        }

        /// <summary>
        /// ファイルをコピーする（既存のファイルが存在する場合はタイムスタンプを付加してコピー）
        /// </summary>
        /// <param name="sourceFile">コピーするファイルの情報</param>
        /// <param name="destinationPath">コピー先のディレクトリパス</param>
        /// <exception cref="FileNotFoundException">コピー元のファイルが存在しない場合にスローされます。</exception>
        /// <exception cref="DirectoryNotFoundException">コピー先のディレクトリが存在しない場合にスローされます。</exception>
        /// <exception cref="ArgumentException">無効なファイル名の場合にスローされます。</exception>
        public static void CopyWithTimestampIfExists ( FileEntityInfo sourceFile , string destinationPath )
        {
            if ( !File.Exists ( sourceFile.FilePath ) )
            {
                throw new FileNotFoundException ( $"Source file '{sourceFile.FilePath}' does not exist." );
            }

            if ( !Directory.Exists ( destinationPath ) )
            {
                throw new DirectoryNotFoundException ( $"Destination directory '{destinationPath}' does not exist." );
            }

            string destinationFilePath = Path.Combine ( destinationPath , sourceFile.FileName );

            if ( !IsValidName ( sourceFile.FileName ) )
            {
                throw new ArgumentException ( $"Invalid file name '{sourceFile.FileName}'." );
            }

            if ( !File.Exists ( destinationFilePath ) )
            {
                File.Copy ( sourceFile.FilePath , destinationFilePath );
                return;
            }

            // ファイルが存在する場合、タイムスタンプを付加してコピー
            string timestamp = DateTime.Now.ToString ( "yyyyMMddHHmmssfff" );
            string newFileName = $"{sourceFile.FileNameWithoutExtension}_{timestamp}{sourceFile.FileExtension}";
            string newDestinationFilePath = Path.Combine ( destinationPath , newFileName );
            File.Copy ( sourceFile.FilePath , newDestinationFilePath );
        }
        #endregion

        #region ファイル削除
        /// <summary>
        /// ファイルを削除する
        /// </summary>
        /// <param name="file">削除するファイルの情報</param>
        /// <exception cref="FileNotFoundException">指定されたファイルが存在しない場合にスローされます。</exception>
        public static void Delete ( FileEntityInfo file )
        {
            if ( !File.Exists ( file.FilePath ) )
            {
                throw new FileNotFoundException ( $"File '{file.FilePath}' does not exist." );
            }

            File.Delete ( file.FilePath );
        }
        #endregion
    }
}
