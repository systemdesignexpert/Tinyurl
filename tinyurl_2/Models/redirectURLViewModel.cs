namespace tinyurl_2.Models;


public class RedirectURLViewModel
{
    public string? url { get; set; }

    public bool ShowURL => !string.IsNullOrEmpty(url);
    
}

