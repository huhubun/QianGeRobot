using Newtonsoft.Json;
using QianGeRobot.Core;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace QianGeRobot.Services
{
    public class DingTalkService
    {
        private readonly HttpClient _httpClient;
        private readonly QianGeConfigs _configs;


        public DingTalkService(
            HttpClient httpClient,
            QianGeConfigs configs
            )
        {
            _httpClient = httpClient;
            _configs = configs;
        }

        public void SendText(string content, IEnumerable<string> atList = null, bool atAll = false)
        {
            var requestBody = JsonConvert.SerializeObject(new
            {
                msgtype = "text",
                text = new
                {
                    content
                },
                at = new
                {
                    atMobiles = atList,
                    isAtAll = atAll
                }
            });

            var requestBodyContent = new StringContent(requestBody, Encoding.UTF8, "application/json");

            var response = _httpClient.PostAsync(_configs.DingTalk.RobotUrl, requestBodyContent).Result;
            Console.WriteLine($"[{nameof(SendText)}] {response.Content.ReadAsStringAsync().Result}");
        }

        public void SendMarkdown(string title, string content, IEnumerable<string> atList = null, bool atAll = false)
        {
            var requestBody = JsonConvert.SerializeObject(new
            {
                msgtype = "markdown",
                markdown = new
                {
                    title,
                    text = content
                },
                at = new
                {
                    atMobiles = atList,
                    isAtAll = atAll
                }
            });

            var requestBodyContent = CreateStringContent(requestBody);

            var response = _httpClient.PostAsync(_configs.DingTalk.RobotUrl, requestBodyContent).Result;
            Console.WriteLine($"[{nameof(SendMarkdown)}] {response.Content.ReadAsStringAsync().Result}");
        }

        private HttpContent CreateStringContent(string content)
        {
            return new StringContent(content, Encoding.UTF8, "application/json");
        }
    }
}
