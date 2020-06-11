using PinterestAccountManager.Pinterst.API;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PinterestAccountManager.Pinterst.Helpers
{
    class UriCreator
    {

        private static readonly Uri BasePinterestUri = new Uri(PinApiConstants.URL_BASE);



        public static Uri GetLoginUri()
        {
            if (!Uri.TryCreate(BasePinterestUri, PinApiConstants.RESOURCE_LOGIN, out var PinUri))
                throw new Exception("Cant create URI for Login");
            return PinUri;
        }

        public static Uri GetUploadUri()
        {
            if (!Uri.TryCreate(BasePinterestUri, PinApiConstants.IMAGE_UPLOAD, out var PinUri))
                throw new Exception("Cant create URI for Image Upload");
            return PinUri;
        }


        public static Uri GetUserSettingUri()
        {
            if (!Uri.TryCreate(BasePinterestUri, PinApiConstants.RESOURCE_GET_USER_SETTINGS, out var PinUri))
                throw new Exception("Cant create URI for GET User Settings");
            return PinUri;
        }




        public static string GetBoardsUri(NameValueCollection query)
        {
            try
            {
                var builder = new UriBuilder(BasePinterestUri + PinApiConstants.RESOURCE_GET_BOARDS);
                builder.Port = -1;
          
               builder.Query = query.ToString();
                string url = builder.ToString();
                return url;

            }
            catch (Exception e)
            {
                throw new Exception("Cant create URI for RESOURCE_GET_BOARDS");
            }

         
           
        }


        public static Uri GetSearchPinsURL()
        {
            if (!Uri.TryCreate(BasePinterestUri, PinApiConstants.RESOURCE_SEARCH, out var PinUri))
                throw new Exception("Cant create URI for RESOURCE_SEARCH");
            return PinUri;
        }
        //
        public static Uri GetPostPinUri()
        {
            if (!Uri.TryCreate(BasePinterestUri, PinApiConstants.RESOURCE_CREATE_PIN, out var PinUri))
                throw new Exception("Cant create URI for RESOURCE_CREATE_PIN");
            return PinUri;
        }


        public static Uri GetBoardInfoUri()
        {
            if (!Uri.TryCreate(BasePinterestUri, PinApiConstants.RESOURCE_GET_BOARD, out var PinUri))
                throw new Exception("Cant create URI for RESOURCE_GET_BOARD");
            return PinUri;
        }



        public static Uri GetBoardPinsUri()        {
            if (!Uri.TryCreate(BasePinterestUri, PinApiConstants.RESOURCE_GET_BOARD_FEED, out var PinUri))
                throw new Exception("Cant create URI for RESOURCE_GET_BOARD_FEED");
            return PinUri;
        }

    }
}
