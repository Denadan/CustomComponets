﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace CustomComponents
{
    public class CustomComponentSettings
    {
        public string LogLevel = "Debug";
        public List<CategoryDescriptor> Categories = new List<CategoryDescriptor>();
        public bool TestEnableAllTags = true;
    }
}
