using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace IOExpansionsLib.IO
{
    /// <summary>
    /// Jsonユーティリティクラスの定義を表します。
    /// </summary>
    /// <remarks>
    /// セクション名には、階層を表す「:」区切りの指定（例：<c>Parent:Child</c>）が可能です。
    /// </remarks>
    public static class JsonUtility
    {
        /// <summary>
        /// セクション名の階層区切り文字を表します。
        /// </summary>
        private const char SECTION_SEPARATOR = ':';

        /// <summary>
        /// ストリームから読み込んだ場合の取得元名（例外メッセージ用）を表します。
        /// </summary>
        private const string STREAM_SOURCE = "(ストリーム)";

        /// <summary>
        /// デシリアライズ時に使用する既定のオプションを表します。
        /// </summary>
        private static readonly JsonSerializerOptions _defaultOptions = new ()
        {
            PropertyNameCaseInsensitive = true
        };

        /// <summary>
        /// シリアライズ時に使用する既定のオプションを表します。
        /// </summary>
        private static readonly JsonSerializerOptions _defaultWriteOptions = new ( _defaultOptions )
        {
            WriteIndented = true
        };

        /// <summary>
        /// 書き込み時に使用する既定のエンコーディング（BOMなしUTF-8）を表します。
        /// </summary>
        private static readonly Encoding _defaultWriteEncoding = new UTF8Encoding ( false );

        #region 読み込み（同期）

        /// <summary>
        /// 指定されたパスのJsonファイルを読み込み、指定された型のオブジェクトに変換します。
        /// </summary>
        /// <typeparam name="T">変換先のモデルクラスの型</typeparam>
        /// <param name="path">読み込むJsonファイルのパス</param>
        /// <param name="options">デシリアライズ時に使用するオプション</param>
        /// <param name="encoding">ファイルのエンコーディング（省略時はUTF-8）</param>
        /// <returns>変換されたモデルクラスのインスタンス</returns>
        /// <exception cref="ArgumentException">pathがnullまたは空文字の場合にスローされます。</exception>
        /// <exception cref="FileNotFoundException">指定されたパスにファイルが存在しない場合にスローされます。</exception>
        /// <exception cref="JsonException">Jsonの解析、またはデシリアライズに失敗した場合にスローされます。</exception>
        /// <exception cref="IOException">ファイルの読み込みに失敗した場合にスローされます。</exception>
        /// <exception cref="UnauthorizedAccessException">ファイルへのアクセス権限がない場合にスローされます。</exception>
        public static T? ReadJsonFile<T> ( string path , JsonSerializerOptions? options = null , Encoding? encoding = null )
        {
            ValidateReadPath ( path );

            string jsonText = File.ReadAllText ( path , encoding ?? Encoding.UTF8 );
            return DeserializeJson<T> ( jsonText , path , options );
        }

        /// <summary>
        /// 指定されたパスのJsonファイルを読み込み、指定されたセクション内の情報を指定された型のオブジェクトに変換します。
        /// </summary>
        /// <typeparam name="T">変換先のモデルクラスの型</typeparam>
        /// <param name="path">読み込むJsonファイルのパス</param>
        /// <param name="sectionName">取得するセクションの名前（「:」区切りで階層指定可能）</param>
        /// <param name="options">デシリアライズ時に使用するオプション</param>
        /// <param name="encoding">ファイルのエンコーディング（省略時はUTF-8）</param>
        /// <returns>変換されたモデルクラスのインスタンス</returns>
        /// <exception cref="ArgumentException">pathまたはsectionNameがnull・空文字・不正な形式の場合にスローされます。</exception>
        /// <exception cref="FileNotFoundException">指定されたパスにファイルが存在しない場合にスローされます。</exception>
        /// <exception cref="JsonException">Jsonの解析、またはデシリアライズに失敗した場合にスローされます。</exception>
        /// <exception cref="KeyNotFoundException">指定されたセクションがJson内に存在しない場合にスローされます。</exception>
        /// <exception cref="IOException">ファイルの読み込みに失敗した場合にスローされます。</exception>
        /// <exception cref="UnauthorizedAccessException">ファイルへのアクセス権限がない場合にスローされます。</exception>
        public static T? ReadJsonFile<T> ( string path , string sectionName , JsonSerializerOptions? options = null , Encoding? encoding = null )
        {
            string[] sectionNames = SplitSectionName ( sectionName );
            ValidateReadPath ( path );

            string jsonText = File.ReadAllText ( path , encoding ?? Encoding.UTF8 );
            return DeserializeSection<T> ( jsonText , path , sectionNames , options );
        }

        #endregion

        #region 読み込み（非同期）

        /// <summary>
        /// 指定されたパスのJsonファイルを非同期で読み込み、指定された型のオブジェクトに変換します。
        /// </summary>
        /// <typeparam name="T">変換先のモデルクラスの型</typeparam>
        /// <param name="path">読み込むJsonファイルのパス</param>
        /// <param name="options">デシリアライズ時に使用するオプション</param>
        /// <param name="encoding">ファイルのエンコーディング（省略時はUTF-8）</param>
        /// <param name="cancellationToken">キャンセルトークン</param>
        /// <returns>変換されたモデルクラスのインスタンス</returns>
        /// <exception cref="ArgumentException">pathがnullまたは空文字の場合にスローされます。</exception>
        /// <exception cref="FileNotFoundException">指定されたパスにファイルが存在しない場合にスローされます。</exception>
        /// <exception cref="JsonException">Jsonの解析、またはデシリアライズに失敗した場合にスローされます。</exception>
        /// <exception cref="IOException">ファイルの読み込みに失敗した場合にスローされます。</exception>
        /// <exception cref="UnauthorizedAccessException">ファイルへのアクセス権限がない場合にスローされます。</exception>
        /// <exception cref="OperationCanceledException">操作がキャンセルされた場合にスローされます。</exception>
        public static async Task<T?> ReadJsonFileAsync<T> ( string path , JsonSerializerOptions? options = null , Encoding? encoding = null , CancellationToken cancellationToken = default )
        {
            ValidateReadPath ( path );

            string jsonText = await File.ReadAllTextAsync ( path , encoding ?? Encoding.UTF8 , cancellationToken ).ConfigureAwait ( false );
            return DeserializeJson<T> ( jsonText , path , options );
        }

        /// <summary>
        /// 指定されたパスのJsonファイルを非同期で読み込み、指定されたセクション内の情報を指定された型のオブジェクトに変換します。
        /// </summary>
        /// <typeparam name="T">変換先のモデルクラスの型</typeparam>
        /// <param name="path">読み込むJsonファイルのパス</param>
        /// <param name="sectionName">取得するセクションの名前（「:」区切りで階層指定可能）</param>
        /// <param name="options">デシリアライズ時に使用するオプション</param>
        /// <param name="encoding">ファイルのエンコーディング（省略時はUTF-8）</param>
        /// <param name="cancellationToken">キャンセルトークン</param>
        /// <returns>変換されたモデルクラスのインスタンス</returns>
        /// <exception cref="ArgumentException">pathまたはsectionNameがnull・空文字・不正な形式の場合にスローされます。</exception>
        /// <exception cref="FileNotFoundException">指定されたパスにファイルが存在しない場合にスローされます。</exception>
        /// <exception cref="JsonException">Jsonの解析、またはデシリアライズに失敗した場合にスローされます。</exception>
        /// <exception cref="KeyNotFoundException">指定されたセクションがJson内に存在しない場合にスローされます。</exception>
        /// <exception cref="IOException">ファイルの読み込みに失敗した場合にスローされます。</exception>
        /// <exception cref="UnauthorizedAccessException">ファイルへのアクセス権限がない場合にスローされます。</exception>
        /// <exception cref="OperationCanceledException">操作がキャンセルされた場合にスローされます。</exception>
        public static async Task<T?> ReadJsonFileAsync<T> ( string path , string sectionName , JsonSerializerOptions? options = null , Encoding? encoding = null , CancellationToken cancellationToken = default )
        {
            string[] sectionNames = SplitSectionName ( sectionName );
            ValidateReadPath ( path );

            string jsonText = await File.ReadAllTextAsync ( path , encoding ?? Encoding.UTF8 , cancellationToken ).ConfigureAwait ( false );
            return DeserializeSection<T> ( jsonText , path , sectionNames , options );
        }

        #endregion

        #region 読み込み（例外を投げない）

        /// <summary>
        /// 指定されたパスのJsonファイルの読み込みを試み、成否を返します。
        /// </summary>
        /// <typeparam name="T">変換先のモデルクラスの型</typeparam>
        /// <param name="path">読み込むJsonファイルのパス</param>
        /// <param name="result">変換されたモデルクラスのインスタンス（失敗時は既定値）</param>
        /// <param name="options">デシリアライズ時に使用するオプション</param>
        /// <param name="encoding">ファイルのエンコーディング（省略時はUTF-8）</param>
        /// <returns>読み込みに成功した場合はtrue、それ以外はfalse</returns>
        public static bool TryReadJsonFile<T> ( string path , out T? result , JsonSerializerOptions? options = null , Encoding? encoding = null )
        {
            try
            {
                result = ReadJsonFile<T> ( path , options , encoding );
                return true;
            }
            catch ( Exception ex ) when ( IsHandledReadException ( ex ) )
            {
                result = default;
                return false;
            }
        }

        /// <summary>
        /// 指定されたパスのJsonファイルの指定されたセクションの読み込みを試み、成否を返します。
        /// </summary>
        /// <typeparam name="T">変換先のモデルクラスの型</typeparam>
        /// <param name="path">読み込むJsonファイルのパス</param>
        /// <param name="sectionName">取得するセクションの名前（「:」区切りで階層指定可能）</param>
        /// <param name="result">変換されたモデルクラスのインスタンス（失敗時は既定値）</param>
        /// <param name="options">デシリアライズ時に使用するオプション</param>
        /// <param name="encoding">ファイルのエンコーディング（省略時はUTF-8）</param>
        /// <returns>読み込みに成功した場合はtrue、それ以外はfalse</returns>
        public static bool TryReadJsonFile<T> ( string path , string sectionName , out T? result , JsonSerializerOptions? options = null , Encoding? encoding = null )
        {
            try
            {
                result = ReadJsonFile<T> ( path , sectionName , options , encoding );
                return true;
            }
            catch ( Exception ex ) when ( IsHandledReadException ( ex ) )
            {
                result = default;
                return false;
            }
        }

        #endregion

        #region ストリーム

        /// <summary>
        /// ストリームからJsonを読み込み、指定された型のオブジェクトに変換します。ストリームは閉じられません。
        /// </summary>
        /// <typeparam name="T">変換先のモデルクラスの型</typeparam>
        /// <param name="stream">読み込み元のストリーム</param>
        /// <param name="options">デシリアライズ時に使用するオプション</param>
        /// <param name="encoding">ストリームのエンコーディング（省略時はUTF-8）</param>
        /// <returns>変換されたモデルクラスのインスタンス</returns>
        /// <exception cref="ArgumentNullException">streamがnullの場合にスローされます。</exception>
        /// <exception cref="JsonException">Jsonの解析、またはデシリアライズに失敗した場合にスローされます。</exception>
        public static T? ReadJsonStream<T> ( Stream stream , JsonSerializerOptions? options = null , Encoding? encoding = null )
        {
            ArgumentNullException.ThrowIfNull ( stream );

            string jsonText = ReadAllText ( stream , encoding );
            return DeserializeJson<T> ( jsonText , STREAM_SOURCE , options );
        }

        /// <summary>
        /// ストリームからJsonを読み込み、指定されたセクション内の情報を指定された型のオブジェクトに変換します。ストリームは閉じられません。
        /// </summary>
        /// <typeparam name="T">変換先のモデルクラスの型</typeparam>
        /// <param name="stream">読み込み元のストリーム</param>
        /// <param name="sectionName">取得するセクションの名前（「:」区切りで階層指定可能）</param>
        /// <param name="options">デシリアライズ時に使用するオプション</param>
        /// <param name="encoding">ストリームのエンコーディング（省略時はUTF-8）</param>
        /// <returns>変換されたモデルクラスのインスタンス</returns>
        /// <exception cref="ArgumentNullException">streamがnullの場合にスローされます。</exception>
        /// <exception cref="ArgumentException">sectionNameがnull・空文字・不正な形式の場合にスローされます。</exception>
        /// <exception cref="JsonException">Jsonの解析、またはデシリアライズに失敗した場合にスローされます。</exception>
        /// <exception cref="KeyNotFoundException">指定されたセクションがJson内に存在しない場合にスローされます。</exception>
        public static T? ReadJsonStream<T> ( Stream stream , string sectionName , JsonSerializerOptions? options = null , Encoding? encoding = null )
        {
            ArgumentNullException.ThrowIfNull ( stream );
            string[] sectionNames = SplitSectionName ( sectionName );

            string jsonText = ReadAllText ( stream , encoding );
            return DeserializeSection<T> ( jsonText , STREAM_SOURCE , sectionNames , options );
        }

        /// <summary>
        /// ストリームからJsonを非同期で読み込み、指定された型のオブジェクトに変換します。ストリームは閉じられません。
        /// </summary>
        /// <typeparam name="T">変換先のモデルクラスの型</typeparam>
        /// <param name="stream">読み込み元のストリーム</param>
        /// <param name="options">デシリアライズ時に使用するオプション</param>
        /// <param name="encoding">ストリームのエンコーディング（省略時はUTF-8）</param>
        /// <param name="cancellationToken">キャンセルトークン</param>
        /// <returns>変換されたモデルクラスのインスタンス</returns>
        /// <exception cref="ArgumentNullException">streamがnullの場合にスローされます。</exception>
        /// <exception cref="JsonException">Jsonの解析、またはデシリアライズに失敗した場合にスローされます。</exception>
        /// <exception cref="OperationCanceledException">操作がキャンセルされた場合にスローされます。</exception>
        public static async Task<T?> ReadJsonStreamAsync<T> ( Stream stream , JsonSerializerOptions? options = null , Encoding? encoding = null , CancellationToken cancellationToken = default )
        {
            ArgumentNullException.ThrowIfNull ( stream );

            string jsonText = await ReadAllTextAsync ( stream , encoding , cancellationToken ).ConfigureAwait ( false );
            return DeserializeJson<T> ( jsonText , STREAM_SOURCE , options );
        }

        /// <summary>
        /// ストリームからJsonを非同期で読み込み、指定されたセクション内の情報を指定された型のオブジェクトに変換します。ストリームは閉じられません。
        /// </summary>
        /// <typeparam name="T">変換先のモデルクラスの型</typeparam>
        /// <param name="stream">読み込み元のストリーム</param>
        /// <param name="sectionName">取得するセクションの名前（「:」区切りで階層指定可能）</param>
        /// <param name="options">デシリアライズ時に使用するオプション</param>
        /// <param name="encoding">ストリームのエンコーディング（省略時はUTF-8）</param>
        /// <param name="cancellationToken">キャンセルトークン</param>
        /// <returns>変換されたモデルクラスのインスタンス</returns>
        /// <exception cref="ArgumentNullException">streamがnullの場合にスローされます。</exception>
        /// <exception cref="ArgumentException">sectionNameがnull・空文字・不正な形式の場合にスローされます。</exception>
        /// <exception cref="JsonException">Jsonの解析、またはデシリアライズに失敗した場合にスローされます。</exception>
        /// <exception cref="KeyNotFoundException">指定されたセクションがJson内に存在しない場合にスローされます。</exception>
        /// <exception cref="OperationCanceledException">操作がキャンセルされた場合にスローされます。</exception>
        public static async Task<T?> ReadJsonStreamAsync<T> ( Stream stream , string sectionName , JsonSerializerOptions? options = null , Encoding? encoding = null , CancellationToken cancellationToken = default )
        {
            ArgumentNullException.ThrowIfNull ( stream );
            string[] sectionNames = SplitSectionName ( sectionName );

            string jsonText = await ReadAllTextAsync ( stream , encoding , cancellationToken ).ConfigureAwait ( false );
            return DeserializeSection<T> ( jsonText , STREAM_SOURCE , sectionNames , options );
        }

        /// <summary>
        /// 指定されたオブジェクトをJson形式に変換し、ストリームへ書き込みます。ストリームは閉じられません。
        /// </summary>
        /// <typeparam name="T">書き込み対象のモデルクラスの型</typeparam>
        /// <param name="stream">書き込み先のストリーム</param>
        /// <param name="value">書き込むオブジェクト</param>
        /// <param name="options">シリアライズ時に使用するオプション</param>
        /// <param name="encoding">ストリームのエンコーディング（省略時はBOMなしUTF-8）</param>
        /// <exception cref="ArgumentNullException">streamまたはvalueがnullの場合にスローされます。</exception>
        public static void WriteJsonStream<T> ( Stream stream , T value , JsonSerializerOptions? options = null , Encoding? encoding = null )
        {
            ArgumentNullException.ThrowIfNull ( stream );

            byte[] bytes = EncodeText ( ToJson ( value , options ) , encoding );
            stream.Write ( bytes , 0 , bytes.Length );
            stream.Flush ();
        }

        /// <summary>
        /// 指定されたオブジェクトをJson形式に変換し、ストリームへ非同期で書き込みます。ストリームは閉じられません。
        /// </summary>
        /// <typeparam name="T">書き込み対象のモデルクラスの型</typeparam>
        /// <param name="stream">書き込み先のストリーム</param>
        /// <param name="value">書き込むオブジェクト</param>
        /// <param name="options">シリアライズ時に使用するオプション</param>
        /// <param name="encoding">ストリームのエンコーディング（省略時はBOMなしUTF-8）</param>
        /// <param name="cancellationToken">キャンセルトークン</param>
        /// <returns>非同期操作を表すタスク</returns>
        /// <exception cref="ArgumentNullException">streamまたはvalueがnullの場合にスローされます。</exception>
        /// <exception cref="OperationCanceledException">操作がキャンセルされた場合にスローされます。</exception>
        public static async Task WriteJsonStreamAsync<T> ( Stream stream , T value , JsonSerializerOptions? options = null , Encoding? encoding = null , CancellationToken cancellationToken = default )
        {
            ArgumentNullException.ThrowIfNull ( stream );

            byte[] bytes = EncodeText ( ToJson ( value , options ) , encoding );
            await stream.WriteAsync ( bytes , cancellationToken ).ConfigureAwait ( false );
            await stream.FlushAsync ( cancellationToken ).ConfigureAwait ( false );
        }

        #endregion

        #region 文字列変換

        /// <summary>
        /// Json文字列を指定された型のオブジェクトに変換します。
        /// </summary>
        /// <typeparam name="T">変換先のモデルクラスの型</typeparam>
        /// <param name="jsonText">Json文字列</param>
        /// <param name="options">デシリアライズ時に使用するオプション</param>
        /// <returns>変換されたモデルクラスのインスタンス</returns>
        /// <exception cref="ArgumentNullException">jsonTextがnullの場合にスローされます。</exception>
        /// <exception cref="JsonException">Jsonの解析、またはデシリアライズに失敗した場合にスローされます。</exception>
        public static T? ParseJson<T> ( string jsonText , JsonSerializerOptions? options = null )
        {
            ArgumentNullException.ThrowIfNull ( jsonText );

            return DeserializeJson<T> ( jsonText , "(文字列)" , options );
        }

        /// <summary>
        /// Json文字列内の指定されたセクションを、指定された型のオブジェクトに変換します。
        /// </summary>
        /// <typeparam name="T">変換先のモデルクラスの型</typeparam>
        /// <param name="jsonText">Json文字列</param>
        /// <param name="sectionName">取得するセクションの名前（「:」区切りで階層指定可能）</param>
        /// <param name="options">デシリアライズ時に使用するオプション</param>
        /// <returns>変換されたモデルクラスのインスタンス</returns>
        /// <exception cref="ArgumentNullException">jsonTextがnullの場合にスローされます。</exception>
        /// <exception cref="ArgumentException">sectionNameがnull・空文字・不正な形式の場合にスローされます。</exception>
        /// <exception cref="JsonException">Jsonの解析、またはデシリアライズに失敗した場合にスローされます。</exception>
        /// <exception cref="KeyNotFoundException">指定されたセクションがJson内に存在しない場合にスローされます。</exception>
        public static T? ParseJson<T> ( string jsonText , string sectionName , JsonSerializerOptions? options = null )
        {
            ArgumentNullException.ThrowIfNull ( jsonText );
            string[] sectionNames = SplitSectionName ( sectionName );

            return DeserializeSection<T> ( jsonText , "(文字列)" , sectionNames , options );
        }

        /// <summary>
        /// オブジェクトをJson文字列に変換します。
        /// </summary>
        /// <typeparam name="T">変換元のモデルクラスの型</typeparam>
        /// <param name="value">変換するオブジェクト</param>
        /// <param name="options">シリアライズ時に使用するオプション</param>
        /// <returns>Json文字列</returns>
        /// <exception cref="ArgumentNullException">valueがnullの場合にスローされます。</exception>
        public static string ToJson<T> ( T value , JsonSerializerOptions? options = null )
        {
            if ( null == value )
            {
                throw new ArgumentNullException ( nameof ( value ) );
            }

            return JsonSerializer.Serialize ( value , options ?? _defaultWriteOptions );
        }

        #endregion

        #region 書き込み（同期）

        /// <summary>
        /// 指定されたオブジェクトをJson形式に変換し、指定されたパスにファイルとして書き込みます。
        /// 書き込みは一時ファイル経由で行い、書き込み途中のファイル破損を防ぎます。
        /// </summary>
        /// <typeparam name="T">書き込み対象のモデルクラスの型</typeparam>
        /// <param name="path">書き込み先のJsonファイルのパス</param>
        /// <param name="value">書き込むオブジェクト</param>
        /// <param name="options">シリアライズ時に使用するオプション</param>
        /// <param name="encoding">ファイルのエンコーディング（省略時はBOMなしUTF-8）</param>
        /// <exception cref="ArgumentNullException">指定されたオブジェクトがnullの場合にスローされます。</exception>
        /// <exception cref="ArgumentException">pathがnullまたは空文字の場合にスローされます。</exception>
        /// <exception cref="IOException">ファイルの書き込みに失敗した場合にスローされます。</exception>
        /// <exception cref="UnauthorizedAccessException">ファイルへのアクセス権限がない場合にスローされます。</exception>
        public static void WriteJsonFile<T> ( string path , T value , JsonSerializerOptions? options = null , Encoding? encoding = null )
        {
            ValidateWriteArguments ( path , value );

            string jsonText = JsonSerializer.Serialize ( value , options ?? _defaultWriteOptions );
            WriteBytesAtomically ( path , EncodeText ( jsonText , encoding ) );
        }

        /// <summary>
        /// 指定されたオブジェクトをJson形式に変換し、指定されたパスのJsonファイル内の指定されたセクションに書き込みます。
        /// 既にファイルが存在し、同名のセクションが存在する場合はそのセクションを上書きし、存在しない場合はセクションを追加します。
        /// ファイルが存在しない場合は新規にファイルを作成します。
        /// </summary>
        /// <typeparam name="T">書き込み対象のモデルクラスの型</typeparam>
        /// <param name="path">書き込み先のJsonファイルのパス</param>
        /// <param name="sectionName">書き込むセクションの名前（「:」区切りで階層指定可能）</param>
        /// <param name="value">書き込むオブジェクト</param>
        /// <param name="options">シリアライズ時に使用するオプション</param>
        /// <param name="encoding">ファイルのエンコーディング（省略時は、読み込みはUTF-8、書き込みはBOMなしUTF-8）</param>
        /// <exception cref="ArgumentNullException">指定されたオブジェクトがnullの場合にスローされます。</exception>
        /// <exception cref="ArgumentException">pathまたはsectionNameがnull・空文字・不正な形式の場合にスローされます。</exception>
        /// <exception cref="JsonException">既存ファイルのJson解析に失敗した場合、または階層の途中がオブジェクトでない場合にスローされます。</exception>
        /// <exception cref="IOException">ファイルの読み書きに失敗した場合にスローされます。</exception>
        /// <exception cref="UnauthorizedAccessException">ファイルへのアクセス権限がない場合にスローされます。</exception>
        public static void WriteJsonFile<T> ( string path , string sectionName , T value , JsonSerializerOptions? options = null , Encoding? encoding = null )
        {
            string[] sectionNames = SplitSectionName ( sectionName );
            ValidateWriteArguments ( path , value );

            string? existingJsonText = File.Exists ( path ) ? File.ReadAllText ( path , encoding ?? Encoding.UTF8 ) : null;
            string jsonText = BuildSectionJson ( existingJsonText , path , sectionNames , value , options );
            WriteBytesAtomically ( path , EncodeText ( jsonText , encoding ) );
        }

        #endregion

        #region 書き込み（非同期）

        /// <summary>
        /// 指定されたオブジェクトをJson形式に変換し、指定されたパスにファイルとして非同期で書き込みます。
        /// 書き込みは一時ファイル経由で行い、書き込み途中のファイル破損を防ぎます。
        /// </summary>
        /// <typeparam name="T">書き込み対象のモデルクラスの型</typeparam>
        /// <param name="path">書き込み先のJsonファイルのパス</param>
        /// <param name="value">書き込むオブジェクト</param>
        /// <param name="options">シリアライズ時に使用するオプション</param>
        /// <param name="encoding">ファイルのエンコーディング（省略時はBOMなしUTF-8）</param>
        /// <param name="cancellationToken">キャンセルトークン</param>
        /// <returns>非同期操作を表すタスク</returns>
        /// <exception cref="ArgumentNullException">指定されたオブジェクトがnullの場合にスローされます。</exception>
        /// <exception cref="ArgumentException">pathがnullまたは空文字の場合にスローされます。</exception>
        /// <exception cref="IOException">ファイルの書き込みに失敗した場合にスローされます。</exception>
        /// <exception cref="UnauthorizedAccessException">ファイルへのアクセス権限がない場合にスローされます。</exception>
        /// <exception cref="OperationCanceledException">操作がキャンセルされた場合にスローされます。</exception>
        public static async Task WriteJsonFileAsync<T> ( string path , T value , JsonSerializerOptions? options = null , Encoding? encoding = null , CancellationToken cancellationToken = default )
        {
            ValidateWriteArguments ( path , value );

            string jsonText = JsonSerializer.Serialize ( value , options ?? _defaultWriteOptions );
            await WriteBytesAtomicallyAsync ( path , EncodeText ( jsonText , encoding ) , cancellationToken ).ConfigureAwait ( false );
        }

        /// <summary>
        /// 指定されたオブジェクトをJson形式に変換し、指定されたパスのJsonファイル内の指定されたセクションに非同期で書き込みます。
        /// 既にファイルが存在し、同名のセクションが存在する場合はそのセクションを上書きし、存在しない場合はセクションを追加します。
        /// ファイルが存在しない場合は新規にファイルを作成します。
        /// </summary>
        /// <typeparam name="T">書き込み対象のモデルクラスの型</typeparam>
        /// <param name="path">書き込み先のJsonファイルのパス</param>
        /// <param name="sectionName">書き込むセクションの名前（「:」区切りで階層指定可能）</param>
        /// <param name="value">書き込むオブジェクト</param>
        /// <param name="options">シリアライズ時に使用するオプション</param>
        /// <param name="encoding">ファイルのエンコーディング（省略時は、読み込みはUTF-8、書き込みはBOMなしUTF-8）</param>
        /// <param name="cancellationToken">キャンセルトークン</param>
        /// <returns>非同期操作を表すタスク</returns>
        /// <exception cref="ArgumentNullException">指定されたオブジェクトがnullの場合にスローされます。</exception>
        /// <exception cref="ArgumentException">pathまたはsectionNameがnull・空文字・不正な形式の場合にスローされます。</exception>
        /// <exception cref="JsonException">既存ファイルのJson解析に失敗した場合、または階層の途中がオブジェクトでない場合にスローされます。</exception>
        /// <exception cref="IOException">ファイルの読み書きに失敗した場合にスローされます。</exception>
        /// <exception cref="UnauthorizedAccessException">ファイルへのアクセス権限がない場合にスローされます。</exception>
        /// <exception cref="OperationCanceledException">操作がキャンセルされた場合にスローされます。</exception>
        public static async Task WriteJsonFileAsync<T> ( string path , string sectionName , T value , JsonSerializerOptions? options = null , Encoding? encoding = null , CancellationToken cancellationToken = default )
        {
            string[] sectionNames = SplitSectionName ( sectionName );
            ValidateWriteArguments ( path , value );

            string? existingJsonText = null;
            if ( File.Exists ( path ) )
            {
                existingJsonText = await File.ReadAllTextAsync ( path , encoding ?? Encoding.UTF8 , cancellationToken ).ConfigureAwait ( false );
            }
            else
            {
                // 既存ファイルが存在しないため、新規作成として扱う
            }

            string jsonText = BuildSectionJson ( existingJsonText , path , sectionNames , value , options );
            await WriteBytesAtomicallyAsync ( path , EncodeText ( jsonText , encoding ) , cancellationToken ).ConfigureAwait ( false );
        }

        #endregion

        #region 内部処理（検証）

        /// <summary>
        /// 読み込み対象のパスを検証します。
        /// </summary>
        /// <param name="path">読み込むJsonファイルのパス</param>
        /// <exception cref="ArgumentException">pathがnullまたは空文字の場合にスローされます。</exception>
        /// <exception cref="FileNotFoundException">指定されたパスにファイルが存在しない場合にスローされます。</exception>
        private static void ValidateReadPath ( string path )
        {
            ArgumentException.ThrowIfNullOrEmpty ( path );

            if ( !File.Exists ( path ) )
            {
                throw new FileNotFoundException ( $"指定されたファイルが見つかりません。 : {path}" , path );
            }
        }

        /// <summary>
        /// 書き込み時の引数を検証します。
        /// </summary>
        /// <typeparam name="T">書き込み対象の型</typeparam>
        /// <param name="path">書き込み先のJsonファイルのパス</param>
        /// <param name="value">書き込むオブジェクト</param>
        /// <exception cref="ArgumentNullException">valueがnullの場合にスローされます。</exception>
        /// <exception cref="ArgumentException">pathがnullまたは空文字の場合にスローされます。</exception>
        private static void ValidateWriteArguments<T> ( string path , T value )
        {
            if ( null == value )
            {
                throw new ArgumentNullException ( nameof ( value ) );
            }

            ArgumentException.ThrowIfNullOrEmpty ( path );
        }

        /// <summary>
        /// セクション名を階層ごとに分割します。
        /// </summary>
        /// <param name="sectionName">セクション名（「:」区切りで階層指定可能）</param>
        /// <returns>階層ごとのセクション名</returns>
        /// <exception cref="ArgumentException">sectionNameがnull・空文字、または空の階層を含む場合にスローされます。</exception>
        private static string[] SplitSectionName ( string sectionName )
        {
            ArgumentException.ThrowIfNullOrEmpty ( sectionName );

            string[] sectionNames = sectionName.Split ( SECTION_SEPARATOR );
            if ( sectionNames.Any ( string.IsNullOrEmpty ) )
            {
                throw new ArgumentException ( $"セクション名の形式が不正です。 : {sectionName}" , nameof ( sectionName ) );
            }

            return sectionNames;
        }

        /// <summary>
        /// Try系メソッドで握りつぶす対象の例外かどうかを判定します。
        /// </summary>
        /// <param name="exception">発生した例外</param>
        /// <returns>握りつぶす対象の場合はtrue</returns>
        private static bool IsHandledReadException ( Exception exception )
        {
            return exception is ArgumentException
                or IOException
                or UnauthorizedAccessException
                or NotSupportedException
                or JsonException
                or KeyNotFoundException;
        }

        #endregion

        #region 内部処理（読み込み）

        /// <summary>
        /// ストリームの内容を文字列として読み込みます。ストリームは閉じられません。
        /// </summary>
        /// <param name="stream">読み込み元のストリーム</param>
        /// <param name="encoding">エンコーディング（省略時はUTF-8）</param>
        /// <returns>読み込んだ文字列</returns>
        private static string ReadAllText ( Stream stream , Encoding? encoding )
        {
            using StreamReader reader = new ( stream , encoding ?? Encoding.UTF8 , true , -1 , true );
            return reader.ReadToEnd ();
        }

        /// <summary>
        /// ストリームの内容を文字列として非同期で読み込みます。ストリームは閉じられません。
        /// </summary>
        /// <param name="stream">読み込み元のストリーム</param>
        /// <param name="encoding">エンコーディング（省略時はUTF-8）</param>
        /// <param name="cancellationToken">キャンセルトークン</param>
        /// <returns>読み込んだ文字列</returns>
        private static async Task<string> ReadAllTextAsync ( Stream stream , Encoding? encoding , CancellationToken cancellationToken )
        {
            using StreamReader reader = new ( stream , encoding ?? Encoding.UTF8 , true , -1 , true );
            return await reader.ReadToEndAsync ( cancellationToken ).ConfigureAwait ( false );
        }

        /// <summary>
        /// Json文字列を指定された型のオブジェクトに変換します。
        /// </summary>
        /// <typeparam name="T">変換先の型</typeparam>
        /// <param name="jsonText">Json文字列</param>
        /// <param name="source">対象の取得元（例外メッセージ用）</param>
        /// <param name="options">デシリアライズ時に使用するオプション</param>
        /// <returns>変換されたオブジェクト</returns>
        /// <exception cref="JsonException">Jsonの解析、またはデシリアライズに失敗した場合にスローされます。</exception>
        private static T? DeserializeJson<T> ( string jsonText , string source , JsonSerializerOptions? options )
        {
            try
            {
                return JsonSerializer.Deserialize<T> ( jsonText , options ?? _defaultOptions );
            }
            catch ( JsonException ex )
            {
                throw new JsonException ( $"Jsonの解析、またはデシリアライズに失敗しました。 : {source}" , ex );
            }
        }

        /// <summary>
        /// Json文字列から指定されたセクションを取り出し、指定された型のオブジェクトに変換します。
        /// </summary>
        /// <typeparam name="T">変換先の型</typeparam>
        /// <param name="jsonText">Json文字列</param>
        /// <param name="source">対象の取得元（例外メッセージ用）</param>
        /// <param name="sectionNames">階層ごとのセクション名</param>
        /// <param name="options">デシリアライズ時に使用するオプション</param>
        /// <returns>変換されたオブジェクト</returns>
        /// <exception cref="JsonException">Jsonの解析、またはデシリアライズに失敗した場合にスローされます。</exception>
        /// <exception cref="KeyNotFoundException">指定されたセクションが存在しない場合にスローされます。</exception>
        private static T? DeserializeSection<T> ( string jsonText , string source , string[] sectionNames , JsonSerializerOptions? options )
        {
            using JsonDocument jsonDocument = ParseJsonDocument ( jsonText , source );

            if ( !TryFindSection ( jsonDocument.RootElement , sectionNames , out JsonElement sectionElement ) )
            {
                throw new KeyNotFoundException ( $"指定されたセクションが見つかりません。 : {string.Join ( SECTION_SEPARATOR , sectionNames )}" );
            }

            return DeserializeJson<T> ( sectionElement.GetRawText () , source , options );
        }

        /// <summary>
        /// Json要素から階層指定されたセクションを検索します。
        /// </summary>
        /// <param name="rootElement">検索の起点となる要素</param>
        /// <param name="sectionNames">階層ごとのセクション名</param>
        /// <param name="sectionElement">見つかったセクションの要素</param>
        /// <returns>見つかった場合はtrue</returns>
        private static bool TryFindSection ( JsonElement rootElement , string[] sectionNames , out JsonElement sectionElement )
        {
            sectionElement = rootElement;

            foreach ( string name in sectionNames )
            {
                if ( JsonValueKind.Object != sectionElement.ValueKind || !sectionElement.TryGetProperty ( name , out sectionElement ) )
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Json文字列を解析してJsonDocumentを生成します。
        /// </summary>
        /// <param name="jsonText">Json文字列</param>
        /// <param name="source">対象の取得元（例外メッセージ用）</param>
        /// <returns>生成されたJsonDocument</returns>
        /// <exception cref="JsonException">Jsonの解析に失敗した場合にスローされます。</exception>
        private static JsonDocument ParseJsonDocument ( string jsonText , string source )
        {
            try
            {
                return JsonDocument.Parse ( jsonText );
            }
            catch ( JsonException ex )
            {
                throw new JsonException ( $"Jsonの解析に失敗しました。 : {source}" , ex );
            }
        }

        #endregion

        #region 内部処理（書き込み）

        /// <summary>
        /// 既存のJson文字列に指定されたセクションを上書き（または追加）したJson文字列を生成します。
        /// </summary>
        /// <typeparam name="T">書き込み対象の型</typeparam>
        /// <param name="existingJsonText">既存のJson文字列（存在しない場合はnull）</param>
        /// <param name="path">対象のJsonファイルのパス（例外メッセージ用）</param>
        /// <param name="sectionNames">階層ごとのセクション名</param>
        /// <param name="value">書き込むオブジェクト</param>
        /// <param name="options">シリアライズ時に使用するオプション</param>
        /// <returns>生成されたJson文字列</returns>
        /// <exception cref="JsonException">既存Jsonの解析に失敗した場合、またはルートや階層の途中がオブジェクトでない場合にスローされます。</exception>
        private static string BuildSectionJson<T> ( string? existingJsonText , string path , string[] sectionNames , T value , JsonSerializerOptions? options )
        {
            JsonSerializerOptions writeOptions = options ?? _defaultWriteOptions;
            JsonObject rootObject = ParseRootObject ( existingJsonText , path );

            JsonObject parentObject = rootObject;
            foreach ( string parentName in sectionNames.Take ( sectionNames.Length - 1 ) )
            {
                parentObject = GetOrCreateChildObject ( parentObject , parentName , path );
            }

            parentObject[sectionNames[^1]] = JsonSerializer.SerializeToNode ( value , writeOptions );
            return rootObject.ToJsonString ( writeOptions );
        }

        /// <summary>
        /// 既存のJson文字列からルートオブジェクトを生成します。
        /// 既存のJson文字列がnullまたは空の場合は、空のオブジェクトを返します。
        /// </summary>
        /// <param name="existingJsonText">既存のJson文字列</param>
        /// <param name="path">対象のJsonファイルのパス（例外メッセージ用）</param>
        /// <returns>ルートオブジェクト</returns>
        /// <exception cref="JsonException">Jsonの解析に失敗した場合、またはルートがオブジェクトでない場合にスローされます。</exception>
        private static JsonObject ParseRootObject ( string? existingJsonText , string path )
        {
            if ( string.IsNullOrWhiteSpace ( existingJsonText ) )
            {
                return new JsonObject ();
            }

            JsonNode? rootNode;
            try
            {
                rootNode = JsonNode.Parse ( existingJsonText );
            }
            catch ( JsonException ex )
            {
                throw new JsonException ( $"Jsonの解析に失敗しました。 : {path}" , ex );
            }

            if ( rootNode is not JsonObject rootObject )
            {
                throw new JsonException ( $"Jsonのルートがオブジェクトではありません。 : {path}" );
            }

            return rootObject;
        }

        /// <summary>
        /// 指定されたオブジェクト内の子オブジェクトを取得します。存在しない場合は新規に作成します。
        /// </summary>
        /// <param name="parentObject">親オブジェクト</param>
        /// <param name="name">子オブジェクトの名前</param>
        /// <param name="path">対象のJsonファイルのパス（例外メッセージ用）</param>
        /// <returns>子オブジェクト</returns>
        /// <exception cref="JsonException">同名の要素が存在し、オブジェクトでない場合にスローされます。</exception>
        private static JsonObject GetOrCreateChildObject ( JsonObject parentObject , string name , string path )
        {
            if ( !parentObject.TryGetPropertyValue ( name , out JsonNode? childNode ) || childNode is null )
            {
                JsonObject createdObject = new ();
                parentObject[name] = createdObject;
                return createdObject;
            }

            if ( childNode is not JsonObject childObject )
            {
                throw new JsonException ( $"階層の途中の要素がオブジェクトではありません。 : {name} ({path})" );
            }

            return childObject;
        }

        /// <summary>
        /// 文字列を指定されたエンコーディングでバイト列に変換します。
        /// エンコーディングにプリアンブル（BOM）がある場合は先頭に付与します。
        /// </summary>
        /// <param name="text">変換する文字列</param>
        /// <param name="encoding">エンコーディング（省略時はBOMなしUTF-8）</param>
        /// <returns>バイト列</returns>
        private static byte[] EncodeText ( string text , Encoding? encoding )
        {
            Encoding targetEncoding = encoding ?? _defaultWriteEncoding;
            return targetEncoding.GetPreamble ().Concat ( targetEncoding.GetBytes ( text ) ).ToArray ();
        }

        /// <summary>
        /// 一時ファイル経由でバイト列をファイルへ書き込み、完了後に置き換えます。
        /// </summary>
        /// <param name="path">書き込み先のファイルパス</param>
        /// <param name="bytes">書き込むバイト列</param>
        private static void WriteBytesAtomically ( string path , byte[] bytes )
        {
            EnsureDirectoryExists ( path );

            string temporaryPath = CreateTemporaryPath ( path );
            try
            {
                File.WriteAllBytes ( temporaryPath , bytes );
                File.Move ( temporaryPath , path , true );
            }
            finally
            {
                DeleteIfExists ( temporaryPath );
            }
        }

        /// <summary>
        /// 一時ファイル経由でバイト列をファイルへ非同期で書き込み、完了後に置き換えます。
        /// </summary>
        /// <param name="path">書き込み先のファイルパス</param>
        /// <param name="bytes">書き込むバイト列</param>
        /// <param name="cancellationToken">キャンセルトークン</param>
        /// <returns>非同期操作を表すタスク</returns>
        private static async Task WriteBytesAtomicallyAsync ( string path , byte[] bytes , CancellationToken cancellationToken )
        {
            EnsureDirectoryExists ( path );

            string temporaryPath = CreateTemporaryPath ( path );
            try
            {
                await File.WriteAllBytesAsync ( temporaryPath , bytes , cancellationToken ).ConfigureAwait ( false );
                File.Move ( temporaryPath , path , true );
            }
            finally
            {
                DeleteIfExists ( temporaryPath );
            }
        }

        /// <summary>
        /// 書き込み先と同じディレクトリに作成する一時ファイルのパスを生成します。
        /// </summary>
        /// <param name="path">書き込み先のファイルパス</param>
        /// <returns>一時ファイルのパス</returns>
        private static string CreateTemporaryPath ( string path )
        {
            return $"{path}.{Guid.NewGuid ():N}.tmp";
        }

        /// <summary>
        /// 指定されたファイルが存在する場合に削除します。
        /// </summary>
        /// <param name="path">削除対象のファイルパス</param>
        private static void DeleteIfExists ( string path )
        {
            if ( File.Exists ( path ) )
            {
                File.Delete ( path );
            }
            else
            {
                // 既に置き換え済みのため、削除は不要
            }
        }

        /// <summary>
        /// 指定されたファイルパスの親ディレクトリが存在しない場合に作成します。
        /// </summary>
        /// <param name="path">対象のファイルパス</param>
        private static void EnsureDirectoryExists ( string path )
        {
            string? directoryPath = Path.GetDirectoryName ( path );
            if ( string.IsNullOrEmpty ( directoryPath ) || Directory.Exists ( directoryPath ) )
            {
                return;
            }

            Directory.CreateDirectory ( directoryPath );
        }

        #endregion
    }
}
