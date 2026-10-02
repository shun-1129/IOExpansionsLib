# IOExpansionsLib

入出力まわりの拡張機能を提供するライブラリです（.NET 8）。

## JsonUtility

`IOExpansionsLib.IO.JsonUtility` は、Jsonファイルの読み書きを行う静的クラスです。

### モデルクラスの例

プロパティ名とJsonのキーが異なる場合は、`[JsonPropertyName]` をモデル側（利用側アプリ）で指定してください。

```csharp
public class AppSettings
{
    [JsonPropertyName ( "user_name" )]
    public string UserName { get; set; } = string.Empty;

    public int Age { get; set; }
}
```

### 読み込み

```csharp
// ファイル全体
AppSettings? settings = JsonUtility.ReadJsonFile<AppSettings> ( "appsettings.json" );

// セクション指定（「:」区切りで階層指定可能）
AppSettings? section = JsonUtility.ReadJsonFile<AppSettings> ( "appsettings.json" , "Parent:Child" );

// 非同期
AppSettings? asyncResult = await JsonUtility.ReadJsonFileAsync<AppSettings> ( "appsettings.json" , cancellationToken: token );

// 例外を投げない
if ( JsonUtility.TryReadJsonFile<AppSettings> ( "appsettings.json" , out AppSettings? result ) )
{
    // 読み込み成功
}
```

### 書き込み

```csharp
// ファイル全体（一時ファイル経由で書き込み、途中破損を防止）
JsonUtility.WriteJsonFile ( "appsettings.json" , settings );

// セクション指定（既存の他セクションは保持。存在しない階層は自動作成）
JsonUtility.WriteJsonFile ( "appsettings.json" , "Parent:Child" , settings );

// 非同期
await JsonUtility.WriteJsonFileAsync ( "appsettings.json" , settings , cancellationToken: token );
```

### 文字列

```csharp
AppSettings? parsed = JsonUtility.ParseJson<AppSettings> ( jsonText );
AppSettings? parsedSection = JsonUtility.ParseJson<AppSettings> ( jsonText , "Parent:Child" );
string json = JsonUtility.ToJson ( settings );
```

### ストリーム

ストリームは呼び出し側で管理し、メソッド内では閉じません。

```csharp
using FileStream stream = File.OpenRead ( "appsettings.json" );
AppSettings? fromStream = JsonUtility.ReadJsonStream<AppSettings> ( stream );

using MemoryStream memory = new ();
JsonUtility.WriteJsonStream ( memory , settings );
```

### 仕様のまとめ

| 項目 | 内容 |
|------|------|
| 既定のデシリアライズ | プロパティ名の大文字小文字を区別しない |
| 既定のシリアライズ | インデントあり（`WriteIndented = true`） |
| 既定のエンコーディング | 読み込み：UTF-8 ／ 書き込み：BOMなしUTF-8（`encoding` 引数で変更可能） |
| セクション名 | 「:」区切りで階層指定（例：`Parent:Child`） |
| 書き込み方式 | 一時ファイルに書き込み後、置き換え |
| 例外メッセージ | Json解析失敗時は対象のパスを含む `JsonException` |
