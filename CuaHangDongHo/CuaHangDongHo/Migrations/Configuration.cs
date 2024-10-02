namespace CuaHangDongHo.Migrations
{
    using CuaHangDongHo.Defines;
    using CuaHangDongHo.Models;
    using CuaHangDongHo.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CuaHangDongHo.Models.EntrySetContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        private EntrySetContext db = new EntrySetContext();

        protected override void Seed(CuaHangDongHo.Models.EntrySetContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            //context.Users.AddOrUpdate(x =>
            //new User()
            //{
            //    x.Id,
            //    x.FullName,
            //    x.Access,
            //    x.Email,
            //    x.Phone,
            //    x.Gender,
            //    x.Password,
            //    x.Status,
            //    x.UserName,
            //    //Id = 1,
            //    //FullName = "Nguyễn Công Trịnh",
            //    //Access = (int)Enums.RoleType.Admin,
            //    //Email = "trinhnguyen@email.com",
            //    //Phone = "0123456789",
            //    //Gender = (Enums.GenderType)0,
            //    //Password = "123456789",
            //    //Status = (Enums.StatusAccountType)0,
            //    //UserName = "abc123444",
            //});

            // Your code...
            // Could also be before try if you know the exception occurs in SaveChanges

            Encryptor encryptor = new Encryptor();

            for (int i = 0; i < 10; i++)
            {
                Random random1 = new Random();
                int gender = random1.Next(0, 2);

                Random random2 = new Random();
                int role = random2.Next(0, 2);

                Random random3 = new Random();
                int status = random3.Next(0, 2);

                User user = new User
                {
                    FullName = "FullName" + i,
                    UserName = "username" + i,
                    Phone = "012345678" + i,
                    Gender = (Enums.GenderType)gender,
                    Password = encryptor.MD5Hash("123456789"),
                    Access = (Enums.RoleType)role,
                    Email = String.Format("email{0}@email.com", i),
                    Status = (Enums.StatusAccountType)status
                };

                db.Users.AddOrUpdate(p => new
                {
                    p.FullName,
                    p.Access,
                    p.Email,
                    p.Phone,
                    p.Gender,
                    p.Password,
                    p.UserName
                }, user);

                db.SaveChanges();
            }

        }


    }
}

