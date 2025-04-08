using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineDictionary.DataAccess.Data;
using OnlineDictionary.DataAccess.Repository.IRepository;

namespace OnlineDictionary.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            ApplicationUser = new ApplicationUserRepository(_db);
            Word = new WordRepository(_db);
            Language = new LanguageRepository(_db);
        }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public IWordRepository Word { get; }
        public ILanguageRepository Language { get; }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
