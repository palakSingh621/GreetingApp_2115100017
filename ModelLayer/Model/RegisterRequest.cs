﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Model
{
    public class RegisterRequest
    {
        public string UserName {  get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ToString()
        {
            return $"Welcome! {UserName} \n {Email}";
        }
    }
}
