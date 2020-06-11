﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PinSharp.Models;

namespace PinSharp.Api
{
    internal partial class PinterestApi : IPinsApi
    {
        public Task<dynamic> GetPinAsync(string id, params string[] fields)
        {
            return GetPinAsync(id, fields.AsEnumerable());
        }

        public Task<dynamic> GetPinAsync(string id, IEnumerable<string> fields)
        {
            return GetAsync<dynamic>($"pins/{id}", new RequestOptions(fields));
        }

        public Task<IPin> GetPinAsync(string id)
        {
            return GetAsync<IPin>($"pins/{id}", new RequestOptions(PinFields));
        }

        public Task<IPin> CreatePinAsync(string board, string imageUrl, string note, string link = null)
        {
            if (!IsValidUrl(imageUrl))
                throw new ArgumentException($"'{imageUrl}' is not a valid URL", nameof(imageUrl));

            return PostAsync<IPin>("pins", new {board, note, link, image_url = imageUrl}, new RequestOptions(PinFields));
        }

        public Task<IPin> CreatePinFromBase64Async(string board, string imageBase64, string note, string link = null)
        {
            if (!IsBase64String(imageBase64))
                throw new ArgumentException("The string is not valid base64", nameof(imageBase64));

            return PostAsync<IPin>("pins", new { board, note, link, image_base64 = imageBase64 }, new RequestOptions(PinFields));
        }

        public Task DeletePinAsync(string id)
        {
            return DeleteAsync($"pins/{id}");
        }

        public Task<IPin> UpdatePinAsync(string id, string board, string note, string link)
        {
            // TODO: Pin id needs to be included in content as well - maybe we need to do this in other places when updating
            return PatchAsync<IPin>($"pins/{id}", new { pin = id, board, note, link }, new RequestOptions(PinFields));
        }

        private static bool IsBase64String(string s)
        {
            s = s.Trim();
            return (s.Length % 4 == 0) && Regex.IsMatch(s, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
        }

        private static bool IsValidUrl(string url)
        {
            Uri uri;
            return Uri.TryCreate(url, UriKind.Absolute, out uri);
        }
    }
}
