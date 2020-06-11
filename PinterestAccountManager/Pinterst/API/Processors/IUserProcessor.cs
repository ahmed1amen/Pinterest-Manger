using PinterestAccountManager.Pinterst.Classes;
using PinterestAccountManager.Pinterst.Classes.Models;
using PinterestAccountManager.Pinterst.Classes.Models.Pins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinterestAccountManager.Pinterst.API.Processors
{
    public interface IUserProcessor
    {


        /// <summary>
        ///     Get currently logged in user info asynchronously
        /// </summary>
        /// <returns>
        ///     <see cref="InstaCurrentUser" />
        /// </returns>
        Task<IResult<PinUser>> GetCurrentUserAsync();

        Task<IResult<List<BoardItem>>> GetUserBoards(string Username = "");
        Task<IResult<PinsSearchResultMain>> SearchPins(string Query, string Cursor);
        Task<IResult<PinsSearchResultMain>>  SearchPins(string Query);

        Task<IResult<PinsSearchResultMain>> GetBoardPins(string BoardUrl);
        Task<IResult<PinsSearchResultMain>> GetBoardPins(string BoardUrl, string Cursor);

        Task<IResult<Owner>> GetBoardInfo(string BoardUri);

    }
}
