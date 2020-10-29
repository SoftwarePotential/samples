/*
 * Copyright 2013-2021 (c) Inish Technology Ventures Limited.  All rights reserved.
 * 
 * This code is licensed under the BSD 3-Clause License included with this source
 * 
 * ALSO SEE: https://github.com/SoftwarePotential/samples/wiki/License
 * 
 */

// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.

using System;
using System.IdentityModel.Tokens;
using System.IO;
using System.Text;
using System.Xml;

namespace LicenseManagement
{
    public static class SecurityTokenExtension
    {
        public static string ToTokenXmlString(this SecurityToken token)
        {
            var genericToken = token as GenericXmlSecurityToken;
            if (genericToken != null)
            {
                return genericToken.ToTokenXmlString();
            }
            var handler = SecurityTokenHandlerCollection.CreateDefaultSecurityTokenHandlerCollection();
            return token.ToTokenXmlString(handler);
        }

        public static string ToTokenXmlString(this SecurityToken token, SecurityTokenHandlerCollection handler)
        {
            if (handler.CanWriteToken(token))
            {
                var sb = new StringBuilder(128);
                handler.WriteToken(new XmlTextWriter(new StringWriter(sb)), token);
                return sb.ToString();
            }
            else
            {
                throw new InvalidOperationException("Token type not suppoted");
            }
        }
    }
}
