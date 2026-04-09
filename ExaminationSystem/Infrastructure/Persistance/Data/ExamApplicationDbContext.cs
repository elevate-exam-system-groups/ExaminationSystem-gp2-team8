using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Data
{
    public class ExamApplicationDbContext(DbContextOptions<ExamApplicationDbContext> options):DbContext(options)
    {

    }
}
