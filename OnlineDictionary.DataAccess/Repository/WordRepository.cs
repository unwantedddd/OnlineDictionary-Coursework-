using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineDictionary.DataAccess.Data;
using OnlineDictionary.DataAccess.Repository.IRepository;
using OnlineDictionary.Models;

namespace OnlineDictionary.DataAccess.Repository
{
    public class WordRepository : Repository<Word>, IWordRepository
    {
        private ApplicationDbContext _db;
        public WordRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        void IWordRepository.Update(Word obj)
        {
            _db.Words.Update(obj);
        }
    }
}
