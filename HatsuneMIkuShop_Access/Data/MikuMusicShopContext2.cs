using Microsoft.EntityFrameworkCore;

namespace HatsuneMIkuShop.Access.Data
{
    public partial class MikuMusicShopContext2 : MikuMusicShopContext
    {
        public MikuMusicShopContext2(DbContextOptions<MikuMusicShopContext> options)
            : base(options)
        {
        }
    }
}
