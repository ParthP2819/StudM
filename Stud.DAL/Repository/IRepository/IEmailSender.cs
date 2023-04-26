using Stud.Model.ForEmail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stud.DAL.Repository.IRepository
{
    public interface IEmailSender
    {
        void SendEmail(Message message);
    }
}
