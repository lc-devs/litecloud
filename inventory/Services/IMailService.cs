using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using inventory.Models;

namespace inventory.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
