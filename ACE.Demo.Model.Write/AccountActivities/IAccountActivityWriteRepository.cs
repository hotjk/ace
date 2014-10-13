using ACE.Demo.Model.AccountActivities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Demo.Model.Write.AccountActivities
{
    public interface IAccountActivityWriteRepository
    {
        bool Save(AccountActivity activity);
    }
}
