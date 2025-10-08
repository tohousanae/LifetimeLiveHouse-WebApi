//using System;
//using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LifetimeLiveHouse.Access.Data;

public partial class LifetimeLiveHouseSysDBContext2 : LifetimeLiveHouseSysDBContext
{
    public LifetimeLiveHouseSysDBContext2(DbContextOptions<LifetimeLiveHouseSysDBContext> options)
        : base(options)
    {
    }

    //public int GetRoomServiceCount()
    //{
    //    return RoomService.CountAsync().Result;
    //}

    //public async Task<List<ViewModels.MemberWithTel>> CallTest222Async()
    //{

    //    return await this.Set<ViewModels.MemberWithTel>()
    //        .FromSqlRaw("EXEC getMemberWithTel", "A0001")
    //        .ToListAsync();
    //}
}
