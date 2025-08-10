using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Core.Models;

namespace Library.Core.Repositories
{
    public interface IMemberMasterRepository
    {
     List<MemberMaster> GetAllMembers();
        MemberMaster GetMemberById(int memberId);
        int AddMember(MemberMaster member);
        bool UpdateMember(MemberMaster member);
        bool DeleteMember(int memberId);
        List<MemberMaster> GetActiveMembers();
    
    }
}
