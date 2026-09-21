namespace IOExpansionsLib.Models
{
    /// <summary>
    /// ディレクトリの情報を表すクラスです。
    /// </summary>
    public class DirectoryEntityInfo
    {
        /// <summary>
        /// ディレクトリ名
        /// </summary>
        public string DirectoryName => Path.GetFileName ( DirectoryPath ) ?? string.Empty;

        /// <summary>
        /// ディレクトリパス
        /// </summary>
        public string DirectoryPath { get; set; }

        /// <summary>
        /// 親ディレクトリのパス
        /// </summary>
        public string ParentDirectoryPath => Path.GetDirectoryName ( DirectoryPath ) ?? string.Empty;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DirectoryEntityInfo ()
        {
            DirectoryPath = string.Empty;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="directoryPath">ディレクトリパス</param>
        public DirectoryEntityInfo ( string directoryPath )
        {
            DirectoryPath = directoryPath;
        }
    }
}
