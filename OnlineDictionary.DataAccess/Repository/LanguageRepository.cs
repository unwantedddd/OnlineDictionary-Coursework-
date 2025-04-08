using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineDictionary.Models;
using OnlineDictionary.DataAccess.Data;
using OnlineDictionary.DataAccess.Repository.IRepository;

namespace OnlineDictionary.DataAccess.Repository
{
    public class LanguageRepository : Repository<Language>, ILanguageRepository
    {
        private ApplicationDbContext _db;
        public LanguageRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        void ILanguageRepository.Update(Language obj)
        {
            _db.Languages.Update(obj);
        }
    }
}
