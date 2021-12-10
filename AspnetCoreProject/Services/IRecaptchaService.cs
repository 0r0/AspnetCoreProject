using AspnetCoreProject.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspnetCoreProject.Services
{
    interface IRecaptchaService
    {
        ReCaptchaSettings Configs { get; }
        bool ValidateRecaptcha(string token);
    }
}
