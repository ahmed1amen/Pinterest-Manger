using System;
using System.Net.Http;


namespace PinterestAccountManager.Pinterst.Logger
{
    public interface IPinLogger
    {
        void LogRequest(HttpRequestMessage request);
        void LogRequest(Uri uri);
        void LogResponse(HttpResponseMessage response);
        void LogException(Exception exception);
        void LogInfo(string info);
     
        
    }
}