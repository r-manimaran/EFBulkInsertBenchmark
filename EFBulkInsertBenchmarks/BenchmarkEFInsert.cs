using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Bogus;

namespace EFBulkInsertBenchmarks;

[MemoryDiagnoser]
public class BenchmarkEFInsert
{
    private static readonly Faker _faker = new Faker();
    [Params(values: 100)]
    public int BulkInsertBatchSize { get; set; }

    /// <summary>
    /// Add one by one and save to Database
    /// </summary>
    /// <returns></returns>
    [Benchmark]
    public async Task EFAddOneAndSave()
    {
        using var context = new ApplicationDbContext();
        foreach (var user in GetUsers())
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Add one by one and save all at once to Database
    /// </summary>
    /// <returns></returns>
    [Benchmark]
    public async Task EFAddOneByOne()
    {
        using var context = new ApplicationDbContext();
        foreach (var user in GetUsers())
        {
            context.Users.Add(user);
        }
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Add Using Range Function
    /// </summary>
    /// <returns></returns>
    public async Task EFAddRange()
    {
        using var context = new ApplicationDbContext();
        context.Users.AddRange(GetUsers());
        await context.SaveChangesAsync();
    }

    #region Helpers
    private User[] GetUsers()
    {
        return Enumerable.Range(0, BulkInsertBatchSize).Select(x => new User
        {
            Email = _faker.Internet.Email(),
            FirstName = _faker.Name.FirstName(),
            LastName = _faker.Name.LastName(),
            PhoneNumber = _faker.Phone.PhoneNumber(),
            CreatedOn = DateTime.Now,

        }).ToArray();
    }
}
#endregion


