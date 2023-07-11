using System.Text.Json.Serialization;

namespace ECommerce.Application.DTOs.Facebook;

public class FacebookUserValidation
{
    [JsonPropertyName("data")]
    public FBUserValidationData Data { get; set; }
    
    // [JsonPropertyName("data.app_id")]
    //  public string AppId { get; set; }
    //  [JsonPropertyName("data.type")]
    //  public string Type { get; set; }
    //  [JsonPropertyName("data.application")]
    //  public string Application { get; set; }
    //  [JsonPropertyName("data.data_access_expires_at")]
    //  public string DataAccessExpiresAt { get; set; }
    //  [JsonPropertyName("data.expires_at")]
    //  public string ExpiresAt { get; set; }
    //  [JsonPropertyName("data.is_valid")]
    //  public bool IsValid { get; set; }
    //  [JsonPropertyName("data.scopes")]
    //  public string[] Scopes { get; set; }
    //  [JsonPropertyName("data.user_id")]
    //  public string UserId { get; set; }
}

public class FBUserValidationData
{
    [JsonPropertyName("app_id")]
    public string AppId { get; set; }
    [JsonPropertyName("type")]
    public string Type { get; set; }
    [JsonPropertyName("application")]
    public string Application { get; set; }
    [JsonPropertyName("data_access_expires_at")]
    public string DataAccessExpiresAt { get; set; }
    [JsonPropertyName("expires_at")]
    public string ExpiresAt { get; set; }
    [JsonPropertyName("is_valid")]
    public bool IsValid { get; set; }
    [JsonPropertyName("scopes")]
    public string[] Scopes { get; set; }
    [JsonPropertyName("user_id")]
    public string UserId { get; set; }
}