using PinterestAccountManager.Pinterst.Classes;
using PinterestAccountManager.Pinterst.Classes.Models.Pins;
using PinterestAccountManager.Pinterst.Helpers;
using PinterestAccountManager.Pinterst.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinterestAccountManager.Pinterst.API.Processors
{

    public interface IPinProcessor
    {

        Task<IResult<PinImage>> UploadPhotoAsync(string PathOfImage);
        Task<IResult<bool>> PostPinAsync(string ImageUrl, string BoardID, string Description, string Link, string Title, string SectionID = "");

        //   Task<IResult<bool>> PostPin(string PathOfImage);

    }
}
