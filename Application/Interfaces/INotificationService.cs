﻿using Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface INotificationService
    {
        Task<ResponseModel> SendMail(SendMailModel request);
    }
}
