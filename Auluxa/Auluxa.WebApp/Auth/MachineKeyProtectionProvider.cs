using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Microsoft.Owin.Security.DataProtection;

namespace Auluxa.WebApp.Auth
{
    public class MachineKeyProtectionProvider : IDataProtectionProvider
    {
        public IDataProtector Create(params string[] purposes) => new MachineKeyDataProtector(purposes);
    }

    public class MachineKeyDataProtector : IDataProtector
    {
        private readonly string[] _purposes;

        public MachineKeyDataProtector(string[] purposes)
        {
            _purposes = purposes;
        }

        public byte[] Protect(byte[] userData) => MachineKey.Protect(userData, _purposes);

        public byte[] Unprotect(byte[] protectedData) => MachineKey.Unprotect(protectedData, _purposes);
    }
}