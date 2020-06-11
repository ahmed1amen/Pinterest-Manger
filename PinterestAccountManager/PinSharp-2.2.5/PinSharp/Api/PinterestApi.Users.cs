﻿using System.Collections.Generic;
using System.Threading.Tasks;
using PinSharp.Models;

namespace PinSharp.Api
{
    internal partial class PinterestApi : IUsersApi
    {
        public Task<dynamic> GetUserAsync(string userName, IEnumerable<string> fields)
        {
            return GetAsync<dynamic>($"users/{userName}", new RequestOptions(fields));
        }

        public Task<IDetailedUser> GetUserAsync(string userName)
        {
            return GetAsync<IDetailedUser>($"users/{userName}", new RequestOptions(UserFields));
        }
    }
}
