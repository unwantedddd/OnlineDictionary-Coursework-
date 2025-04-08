﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineDictionary.Models;

namespace OnlineDictionary.DataAccess.Repository.IRepository
{
    public interface ILanguageRepository : IRepository<Language>
    {
        public void Update(Language obj);
    }
}
