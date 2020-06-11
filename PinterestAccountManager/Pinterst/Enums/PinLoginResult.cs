using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinterestAccountManager.Pinterst.Enums
{
    public enum PinLoginResult
    {

        Success,
        BadPassword,
        InvalidUser,
        TwoFactorRequired,
        Exception,
        ChallengeRequired,
        LimitError,
        InactiveUser,
        CheckpointLoggedOut
    }
}
