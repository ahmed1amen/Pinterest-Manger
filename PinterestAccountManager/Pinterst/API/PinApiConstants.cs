using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinterestAccountManager.Pinterst.API
{
internal static class PinApiConstants
{
    /*
     Login
    */
    public const string RESOURCE_LOGIN                      = "resource/UserSessionResource/create/";
    public const string CSRFTOKEN                           = "csrftoken";
    /*
     Password
    */
    public const string RESOURCE_RESET_PASSWORD_SEND_LINK   =   "resource/UserResetPasswordResource/create/";
    public const string RESOURCE_RESET_PASSWORD_UPDATE      =   "resource/ResetPasswordFromEmailResource/update/";
    /*
     Boards
    */
    public const string RESOURCE_GET_BOARDS                 = "resource/BoardsResource/get/";
    public const string RESOURCE_GET_BOARD                  = "resource/BoardResource/get/";
    public const string RESOURCE_GET_BOARD_FEED             = "resource/BoardFeedResource/get/";
    public const string RESOURCE_PROFILE_BOARDS             = "resource/ProfileBoardsResource/get/";
    public const string RESOURCE_FOLLOW_BOARD               = "resource/BoardFollowResource/create/";
    public const string RESOURCE_UNFOLLOW_BOARD             = "resource/BoardFollowResource/delete/";
    public const string RESOURCE_DELETE_BOARD               = "resource/BoardResource/delete/";
    public const string RESOURCE_CREATE_BOARD               = "resource/BoardResource/create/";
    public const string RESOURCE_UPDATE_BOARD               = "resource/BoardResource/update/";
    public const string RESOURCE_BOARD_FOLLOWERS            = "resource/BoardFollowersResource/get/";
    public const string RESOURCE_FOLLOWING_BOARDS           = "resource/BoardFollowingResource/get/";
    public const string RESOURCE_TITLE_SUGGESTIONS          = "resource/BoardTitleSuggestionsResource/get";
    public const string RESOURCE_BOARDS_INVITES             = "resource/BoardInvitesResource/get/";
    public const string RESOURCE_CREATE_USER_ID_INVITE      = "resource/BoardInviteResource/create";
    public const string RESOURCE_CREATE_EMAIL_INVITE        = "resource/BoardEmailInviteResource/create/";
    public const string RESOURCE_ACCEPT_INVITE              = "resource/BoardInviteResource/update/";
    public const string RESOURCE_DELETE_INVITE              = "resource/BoardInviteResource/delete/";
    public const string RESOURCE_LEAVE_BOARD                = "resource/BoardCollaboratorResource/delete/";
    /*
    * Board section
    */
public const string RESOURCE_GET_BOARD_SECTIONS     = "resource/BoardSectionsResource/get/";
public const string RESOURCE_ADD_BOARD_SECTION      = "resource/BoardSectionResource/create/";
public const string RESOURCE_EDIT_BOARD_SECTION     = "resource/BoardSectionEditResource/create/";
public const string RESOURCE_DELETE_BOARD_SECTION   = "resource/BoardSectionEditResource/delete/";
    /*
        Pins
    */
public const string RESOURCE_CREATE_PIN          =  "resource/PinResource/create/";
public const string RESOURCE_UPDATE_PIN          =  "resource/PinResource/update/";
public const string RESOURCE_REPIN               =  "resource/RepinResource/create/";
public const string RESOURCE_USER_FOLLOWERS      =  "resource/UserFollowersResource/get/";
public const string RESOURCE_DELETE_PIN          =  "resource/PinResource/delete/";
public const string RESOURCE_LIKE_PIN            =  "resource/PinLikeResource/create/";
public const string RESOURCE_UNLIKE_PIN          =  "resource/PinLikeResource/delete/";
public const string RESOURCE_COMMENT_PIN         =  "resource/AggregatedCommentResource/create/";
public const string RESOURCE_COMMENT_DELETE_PIN  =  "resource/AggregatedCommentResource/delete/";
public const string RESOURCE_PIN_INFO            =  "resource/PinResource/get/";
public const string RESOURCE_DOMAIN_FEED         =  "resource/DomainFeedResource/get/";
public const string RESOURCE_ACTIVITY            =  "resource/AggregatedActivityFeedResource/get/";
public const string RESOURCE_USER_FEED           =  "resource/UserHomefeedResource/get/";
public const string RESOURCE_RELATED_PINS        =  "resource/RelatedPinFeedResource/get/";
public const string RESOURCE_VISUAL_SIMILAR_PINS  =  "resource/VisualLiveSearchResource/get/";
public const string RESOURCE_BULK_COPY           =  "resource/BulkEditResource/create/";
public const string RESOURCE_BULK_MOVE           =  "resource/BulkEditResource/update/";
public const string RESOURCE_BULK_DELETE         =  "resource/BulkEditResource/delete/";
public const string RESOURCE_EXPLORE_PINS        =  "resource/ExploreSectionFeedResource/get/";
public const string RESOURCE_PIN_ANALYTICS       =  "resource/OnPinAnalyticsResource/get/";
public const string RESOURCE_TRY_PIN_CREATE      =  "resource/DidItActivityResource/create/";
public const string RESOURCE_TRY_PIN_EDIT        =  "resource/DidItActivityResource/update/";
public const string RESOURCE_TRY_PIN_DELETE      =  "resource/DidItActivityResource/delete/";
public const string RESOURCE_TRY_PIN_IMAGE_UPLOAD  =  "resource/DidItImageUploadResource/create/";
public const string RESOURCE_SHARE_VIA_SOCIAL    =   "resource/CreateExternalInviteResource/create/";
    /*
     Pinners
    */
    public const string RESOURCE_FOLLOW_USER = "resource/UserFollowResource/create/";
    public const string RESOURCE_UNFOLLOW_USER = "resource/UserFollowResource/delete/";
    public const string RESOURCE_USER_INFO = "resource/UserResource/get/";
    public const string RESOURCE_USER_FOLLOWING = "resource/UserFollowingResource/get/";
    public const string RESOURCE_USER_PINS = "resource/UserPinsResource/get/";
    public const string RESOURCE_USER_LIKES = "resource/UserLikesResource/get/";
    public const string RESOURCE_USER_TRIED = "resource/DidItUserFeedResource/get/";
    public const string RESOURCE_CONTACTS_REQUESTS = "resource/ContactRequestsResource/get";
    public const string RESOURCE_CONTACT_REQUEST_ACCEPT = "resource/ContactRequestAcceptResource/update/";
    public const string RESOURCE_CONTACT_REQUEST_IGNORE = "resource/ContactRequestIgnoreResource/delete/";
    /*
     Search
    */
    public const string RESOURCE_SEARCH = "resource/BaseSearchResource/get/";
    public const string RESOURCE_SEARCH_WITH_PAGINATION = "resource/SearchResource/get/";
    public const string RESOURCE_TYPE_AHEAD_SUGGESTIONS = "resource/AdvancedTypeaheadResource/get";
    public const string RESOURCE_HASHTAG_TYPE_AHEAD_SUGGESTIONS = "resource/HashtagTypeaheadResource/get";
    /*
     Interests.
    */
    public const string RESOURCE_FOLLOW_INTEREST = "resource/InterestFollowResource/create/";
    public const string RESOURCE_UNFOLLOW_INTEREST = "resource/InterestFollowResource/delete/";
    public const string RESOURCE_FOLLOWING_INTERESTS = "resource/InterestFollowingResource/get/";
    /*
      Conversations.
    */
    public const string RESOURCE_SEND_MESSAGE = "resource/ConversationsResource/create/";
    public const string RESOURCE_GET_CONVERSATION_MESSAGES = "resource/ConversationMessagesResource/get/";
    public const string RESOURCE_GET_LAST_CONVERSATIONS = "resource/ConversationsResource/get/";
    /*
      UserSettings
    */
    public const string RESOURCE_UPDATE_USER_SETTINGS = "resource/UserSettingsResource/update/";
    public const string RESOURCE_GET_USER_SETTINGS = "resource/UserSettingsResource/get/";
    public const string RESOURCE_CHANGE_PASSWORD = "resource/UserPasswordResource/update/";
    public const string RESOURCE_DEACTIVATE_ACCOUNT = "resource/DeactivateAccountResource/create/";
    public const string RESOURCE_BLOCK_USER = "resource/UserBlockResource/create/";
    public const string RESOURCE_CLEAR_SEARCH_HISTORY = "resource/TypeaheadClearRecentResource/delete/";
    public const string RESOURCE_SESSIONS_HISTORY = "resource/UserSessionStoreResource/get/";
    public const string RESOURCE_AVAILABLE_LOCALES = "resource/LocalesResource/get/";
    public const string RESOURCE_AVAILABLE_COUNTRIES = "resource/CountriesResource/get/";
    public const string RESOURCE_AVAILABLE_ACCOUNT_TYPES = "resource/BusinessTypesResource/get/";
    /*
     News
    */
    public const string RESOURCE_GET_LATEST_NEWS = "resource/NetworkStoriesResource/get/";
    public const string RESOURCE_GET_NOTIFICATIONS = "resource/NewsHubResource/get/";
    public const string RESOURCE_GET_NEWS_HUB_DETAILS = "resource/NewsHubDetailsResource/get/";
    /*
     Registration
    */
    public const string RESOURCE_CREATE_REGISTER             =  "resource/UserRegisterResource/create/";
    public const string RESOURCE_CHECK_EMAIL                 =  "resource/EmailExistsResource/get/";
    public const string RESOURCE_SET_ORIENTATION             =  "resource/OrientationContextResource/create/";
    public const string RESOURCE_UPDATE_REGISTRATION_TRACK   =  "resource/UserRegisterTrackActionResource/update/";
    public const string RESOURCE_REGISTRATION_COMPLETE       =  "resource/UserExperienceCompletedResource/update/";
    public const string RESOURCE_CONVERT_TO_BUSINESS         =  "resource/PartnerResource/update/";
    /*
     Uploads
    */
    public const string IMAGE_UPLOAD = "upload-image/";
    /*
     Categories
    */
    public const string RESOURCE_GET_CATEGORIES = "resource/CategoriesResource/get/";
    public const string RESOURCE_GET_CATEGORY = "resource/CategoryResource/get/";
    public const string RESOURCE_GET_CATEGORIES_RELATED = "resource/RelatedInterestsResource/get/";
    public const string RESOURCE_GET_CATEGORY_FEED = "resource/CategoryFeedResource/get/";
    /*
     Topics
    */
    public const string RESOURCE_GET_TOPIC_FEED = "resource/TopicFeedResource/get/";
    public const string RESOURCE_GET_TOPIC = "resource/TopicResource/get/";
    public const string RESOURCE_EXPLORE_SECTIONS = "resource/ExploreSectionsResource/get/";
    /*
     Invite
    */
    public const string RESOURCE_INVITE = "resource/EmailInviteSentResource/create/";
    public const string URL_BASE = "https://www.pinterest.com/";
    public const string HOST = "www.pinterest.com";
    public const string FOLLOWING_INTERESTS = "interests";
    public const string FOLLOWING_PEOPLE = "people";
    public const string FOLLOWING_BOARDS = "boards";



        /*

                scad

        */

        public const string scad = "{0},{1},{2}";



        /*
         UserAgents
         */

        public const string UserAgent_Defualt = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_12_2) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.95 Safari/537.36";
}
}
