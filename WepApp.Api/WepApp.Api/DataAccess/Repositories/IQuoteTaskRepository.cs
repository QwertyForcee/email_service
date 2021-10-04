﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WepApp.Api.DataAccess.Repositories
{
    public interface IQuoteTaskRepository<T> where T:class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetUserTasksAsync(int userId);
        Task<T> GetAsync(int id);
        Task<int> AddAsync(T quoteTask);
        Task<int> DeleteAsync(int id);
        Task<int> UpdateAsync(T quoteTask);
    }
}
