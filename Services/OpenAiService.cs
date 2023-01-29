﻿using CosmosDB_ChatGPT.Models;
using System.Text;
using System.Text.Json;

namespace CosmosDB_ChatGPT.Services
{
    public class OpenAiService
    {
        
        private readonly HttpClient httpClient;

        public OpenAiService(IConfiguration configuration) 
        {
            string uri = configuration["OpenAiUri"];
            string key = configuration["OpenAiKey"];
            string deployment = configuration["OpenAiDeployment"];
            string serviceUri = uri + "/openai/deployments/" + deployment + "/completions?api-version=2022-12-01";

            httpClient = new()
            {
                BaseAddress = new Uri(serviceUri)
            };

            httpClient.DefaultRequestHeaders.Add("api-key", key); //auth key
            httpClient.DefaultRequestHeaders.Add("Content-Type", "application-json");
        }


        public async Task<string> PostAsync(string Prompt, string ChatSession)
        {
            using StringContent jsonContent = new(
                JsonSerializer.Serialize(new
                {
                    prompt = Prompt
                }),
                Encoding.UTF8,
                "application/json");


            using HttpResponseMessage response = await httpClient.PostAsync(
                "Cosmos-ChatGPT",
                jsonContent);

            response.EnsureSuccessStatusCode();

            string jsonResponse = await response.Content.ReadAsStringAsync();

            return jsonResponse;

        }
        
    }
}
