namespace Skyve.Compatibility.Domain;

public struct ApiResponse
{
	public bool Success { get; set; }
	public string? Message { get; set; }
	public object Data { get; set; }
}