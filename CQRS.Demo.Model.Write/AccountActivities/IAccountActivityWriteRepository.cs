using CQRS.Demo.Model.AccountActivities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Demo.Model.Write.AccountActivities
{
    public interface IAccountActivityWriteRepository
    {
        bool Save(AccountActivity activity);
    }
}
