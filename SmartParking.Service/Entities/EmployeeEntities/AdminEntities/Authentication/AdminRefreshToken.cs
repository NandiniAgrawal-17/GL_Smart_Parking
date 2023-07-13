﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParking.Service.Entities.AdminEntities.Authentication
{
    public class AdminRefreshToken
    {
        public int AdminId { get; set; }
        public string TokenAdmin { get; set; }
        public DateTime Created { get; set; }
        public DateTime Expire { get; set; }
        public string role { get; set; }
    }
}
