//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NotesMarketPlace.Context
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class database1Entities : DbContext
    {
        public database1Entities()
            : base("name=database1Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ContactUs> ContactUs { get; set; }
        public virtual DbSet<Countries> Countries { get; set; }
        public virtual DbSet<Downloads> Downloads { get; set; }
        public virtual DbSet<NoteCategories> NoteCategories { get; set; }
        public virtual DbSet<NoteDetail> NoteDetail { get; set; }
        public virtual DbSet<NotesAttachments> NotesAttachments { get; set; }
        public virtual DbSet<NotesReview> NotesReview { get; set; }
        public virtual DbSet<NoteTypes> NoteTypes { get; set; }
        public virtual DbSet<ReferenceData> ReferenceData { get; set; }
        public virtual DbSet<SpamReport> SpamReport { get; set; }
        public virtual DbSet<Stats> Stats { get; set; }
        public virtual DbSet<SystemConfiguration> SystemConfiguration { get; set; }
        public virtual DbSet<UserProfileDetail> UserProfileDetail { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<AdminDetail> AdminDetail { get; set; }
    }
}
