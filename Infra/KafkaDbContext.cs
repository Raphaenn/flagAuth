// using App.Dto;
// using Domain.Entities;
// using Infra.Models;
// using Microsoft.EntityFrameworkCore;
//
// namespace Infra;
//
// public sealed class KafkaDbContext : DbContext, IUnitOfWork
// {
//     public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
//
//     public KafkaDbContext(DbContextOptions<KafkaDbContext> opt) : base(opt) { }
//
//     protected override void OnModelCreating(ModelBuilder b)
//     {
//         b.Entity<OutboxMessage>().HasKey(x => x.Id);
//         b.Entity<ProcessedMessage>().HasKey(x => x.EventId);
//     }
// }