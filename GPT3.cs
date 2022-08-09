using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace GPT3
{
    public static class GPT3Api
    {
        public static async Task<Response> Completions(Request request)
        {
            using var client = new System.Net.Http.HttpClient();

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {System.Environment.GetEnvironmentVariable("OPENAI_API_KEY")}");

            var content = JsonContent.Create(request);

            var httpResponse = await client.PostAsync("https://api.openai.com/v1/completions", content);

            // var json = await httpResponse.Content.ReadAsStringAsync();

            // Console.WriteLine(json);

            return await httpResponse.Content.ReadFromJsonAsync<Response>() ?? throw new Exception("Invalid response");
            // return System.Text.Json.JsonSerializer.Deserialize<Response>(json) ?? throw new Exception("Invalid response");
        }
    }

    public class Request
    {
        [JsonPropertyName("model")]
        public string Model { get; set; } = "text-davinci-002";

        [JsonPropertyName("prompt")]
        public string Prompt { get; set; } = "";

        [JsonPropertyName("temperature")]
        public double Temperature { get; set; } = 0.7;

        [JsonPropertyName("max_tokens")]
        public int MaxTokens { get; set; } = 256;

        [JsonPropertyName("top_p")]
        public int TopP { get; set; } = 1;

        [JsonPropertyName("frequency_penalty")]
        public int FrequencyPenalty { get; set; } = 0;

        [JsonPropertyName("presence_penalty")]
        public int PresencePenalty { get; set; } = 0;
    }

    public class Response
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = "";

        [JsonPropertyName("object")]
        public string Object { get; set; } = "";

        [JsonPropertyName("created")]
        public int Created { get; set; } = 0;

        [JsonPropertyName("model")]
        public string Model { get; set; } = "";

        [JsonPropertyName("choices")]
        public IEnumerable<Choice> Choices { get; set; } = Enumerable.Empty<Choice>();

        [JsonPropertyName("usage")]
        public ResponseUsage Usage { get; set; } = new ResponseUsage();

        public class Choice
        {
            [JsonPropertyName("text")]
            public string Text { get; set; } = "";

            [JsonPropertyName("index")]
            public int Index { get; set; } = 0;

            [JsonPropertyName("logprobs")]
            public object logprobs { get; set; } = new object();

            [JsonPropertyName("finish_reason")]
            public string FinishReason { get; set; } = "";
        }
        public class ResponseUsage
        {
            [JsonPropertyName("prompt_tokens")]
            public int PromptTokens { get; set; } = 0;

            [JsonPropertyName("completion_tokens")]
            public int CompletionTokens { get; set; } = 0;

            [JsonPropertyName("total_tokens")]
            public int TotalTokens { get; set; } = 0;
        }
    }
}
