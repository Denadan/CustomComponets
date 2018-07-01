﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace CustomComponents
{
    public class CustomComponentSettings
    {
        public bool LoadDefaultValidators = true;
        public string LogLevel = "Debug";
        public List<CategoryDescriptor> Categories = new List<CategoryDescriptor>();
    }
}
