using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BlogTemplate._1.Models;

namespace BlogTemplate._1.Services
{
    public class SlugGenerator
    {
        private static Regex AllowList = new Regex("([^A-Za-z0-9-])");
        private static TimeSpan Timeout = TimeSpan.FromSeconds(1);

        private BlogDataStore _dataStore;

        public SlugGenerator(BlogDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public string CreateSlug(string title)
        {
            string tempTitle = title;
            tempTitle = tempTitle.Replace(" ", "-");
            Task<string> cleanupTask = RemoveInvalidChars(tempTitle);
            cleanupTask.ConfigureAwait(false);
            if (cleanupTask.Wait(Timeout))
            {
                string slug = cleanupTask.Result;
                return slug;
            }

            throw new TimeoutException("Failed to filter slug within the required timeout");
        }

        private async Task<string> RemoveInvalidChars(string slug)
        {
            return await Task.FromResult(AllowList.Replace(slug, ""));
        }
    }
}

