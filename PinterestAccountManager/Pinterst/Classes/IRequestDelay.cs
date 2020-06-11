

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinterestAccountManager.Pinterst.Classes
{
    public interface IRequestDelay
    {
        TimeSpan Value { get; }
        bool Exist { get; }
        void Enable();
        void Disable();
    }
}
