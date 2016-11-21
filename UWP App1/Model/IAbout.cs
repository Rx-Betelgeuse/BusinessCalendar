﻿using System;
using System.Collections.Generic;

namespace UWP_App1.Model
{
    public interface IAbout
    {
        string Name { get; }
        string Publisher { get; }
        string PackageFamilyName { get; }
        string ProductId { get; }
        string Version { get; }
        Uri PrivacyStatement { get; }
        Uri TermsOfUse { get; }
        IEnumerable<Link> Links { get; }
        IEnumerable<Credit> Credits { get; }
    }
}