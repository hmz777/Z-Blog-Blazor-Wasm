﻿using HMZSoftwareBlazorWebAssembly.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace HMZSoftwareBlazorWebAssembly.Shared
{
    public partial class ProjectCards : ComponentBase
    {
        [Inject] HttpClient HttpClient { get; set; }

        private List<GitRepo> GitHubProjects { get; set; } = new List<GitRepo>();

        private bool DataLoaded { get; set; } = false;

        [Inject] IJSRuntime JS { get; set; }

        protected override async Task OnInitializedAsync()
        {
            //#if DEBUG
            //            await Task.Delay(10000);
            //#endif

            HttpClient.BaseAddress = new Uri("https://api.github.com");

            HttpRequestMessage GitData = new HttpRequestMessage(HttpMethod.Get, "/users/hmz777/repos");
            GitData.Headers.Accept.Clear();
            GitData.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));

            var data = await HttpClient.SendAsync(GitData, HttpCompletionOption.ResponseContentRead);

            if (!data.IsSuccessStatusCode)
                throw new Exception("GitHub data fetch failed!");

            var json = await data.Content.ReadAsStringAsync();
            var projects = JsonSerializer.Deserialize<JsonElement>(json);

            foreach (var project in projects.EnumerateArray())
            {
                if (!project.GetProperty("fork").GetBoolean())
                {
                    GitHubProjects.Add(new GitRepo()
                    {
                        Name = project.GetProperty("name").GetString(),
                        Description = project.GetProperty("description").GetString(),
                        Url = project.GetProperty("html_url").GetString(),
                        Forks = project.GetProperty("forks_count").GetInt32(),
                        Stars = project.GetProperty("stargazers_count").GetInt32(),
                        Watchers = project.GetProperty("watchers_count").GetInt32(),
                        Topics = await GetProjectTopics(project.GetProperty("name").GetString())
                    });
                }
            }

            DataLoaded = true;
        }

        private async Task<string[]> GetProjectTopics(string ProjectName)
        {
            HttpRequestMessage TopicsData = new HttpRequestMessage(HttpMethod.Get, $"/repos/hmz777/{ProjectName}/topics");
            TopicsData.Headers.Accept.Clear();
            TopicsData.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/vnd.github.mercy-preview+json"));

            var topics = await HttpClient.SendAsync(TopicsData, HttpCompletionOption.ResponseContentRead);

            if (!topics.IsSuccessStatusCode)
                throw new Exception("Topics fetch failed!");

            var json = await topics.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<JsonElement>(json).GetProperty("names").EnumerateArray().Select(item => item.ToString()).ToArray();
        }

        [JSInvokable]
        public void OnScroll(ScrollEventArgs args)
        {
            // TODO: Render the project cards when the user scrolled to the component.         

            // Scroll logic...

            StateHasChanged();
        }

        public class ScrollEventArgs
        {
            public DOMRect ContainerRect { get; set; }
            public DOMRect ContentRect { get; set; }
        }

        public class DOMRect
        {
            public double Top { get; set; }
            public double Bottom { get; set; }
            public double Left { get; set; }
            public double Right { get; set; }
            public double Width { get; set; }
            public double Height { get; set; }
        }
    }
}