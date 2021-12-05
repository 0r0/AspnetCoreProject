using AspnetCoreProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspnetCoreProject.Services
{
    public interface IMailService
    {
        Task sendEmailAsync(MailRequest mailRequest);
    }
}
