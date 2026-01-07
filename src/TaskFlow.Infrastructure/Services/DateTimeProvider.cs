using System;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Infrastructure.Services
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
