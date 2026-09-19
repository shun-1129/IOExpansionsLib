namespace IOExpansionsLib.Models
{
    /// <summary>
    /// ファイルの情報を表すクラスです。
    /// </summary>
    public class FileEntityInfo
    {
        /// <summary>
        /// ファイル名
        /// </summary>
        public string FileName => Path.GetFileName ( FilePath );

        /// <summary>
        /// ファイル名（拡張子なし）
        /// </summary>
        public string FileNameWithoutExtension => Path.GetFileNameWithoutExtension ( FilePath );

        /// <summary>
        /// ファイル拡張子
        /// </summary>
        public string FileExtension => Path.GetExtension ( FilePath );

        /// <summary>
        /// ファイルパス
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath"></param>
        public FileEntityInfo ( string filePath )
        {
            FilePath = filePath;
        }
    }
}
