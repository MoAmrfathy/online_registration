using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class ApplicationDataContext : DbContext
    {
        public ApplicationDataContext(DbContextOptions<ApplicationDataContext> options)
            : base(options)
        {
        }

        // DbSets for each entity
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<GraduationPlan> GraduationPlans { get; set; }
        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<Prerequisite> Prerequisites { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Term> Terms { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Waitlist> Waitlist { get; set; }
        public DbSet<SelectedCourse> SelectedCourses { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define primary keys
            modelBuilder.Entity<Student>().HasKey(s => s.S_id);
            modelBuilder.Entity<Course>().HasKey(c => c.C_id);
            modelBuilder.Entity<Department>().HasKey(d => d.D_id);
            modelBuilder.Entity<Enrollment>().HasKey(e => e.EnrollmentId);
            modelBuilder.Entity<GraduationPlan>().HasKey(g => g.GraduationPlanId);
            modelBuilder.Entity<Lecture>().HasKey(l => l.LectureId);
            modelBuilder.Entity<Prerequisite>().HasKey(p => p.PrerequisiteId);
            modelBuilder.Entity<Section>().HasKey(sec => sec.SectionId);
            modelBuilder.Entity<Term>().HasKey(t => t.TermId);
            modelBuilder.Entity<Group>().HasKey(g => g.GroupId);
            modelBuilder.Entity<Waitlist>().HasKey(w => w.WaitlistId);
            modelBuilder.Entity<SelectedCourse>().HasKey(sc => sc.SelectedCourseId);

            // Define relationships and foreign keys


            // Enrollment -> Student (Many-to-One)
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.S_id)
                .OnDelete(DeleteBehavior.NoAction);

            // Enrollment -> Course (Many-to-One)
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany()
                .HasForeignKey(e => e.C_id)
                .OnDelete(DeleteBehavior.NoAction);

            // Enrollment -> Group (Many-to-One)
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Group)
                .WithMany()
                .HasForeignKey(e => e.GroupId)
                .OnDelete(DeleteBehavior.Restrict);

            // Enrollment -> GraduationPlan (Many-to-One)
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.GraduationPlan)
                .WithMany()
                .HasForeignKey(e => e.GraduationPlanId)
                .OnDelete(DeleteBehavior.Restrict);

            // Student -> Department (Many-to-One)
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Department)
                .WithMany(d => d.Students)
                .HasForeignKey(s => s.D_id)
                .OnDelete(DeleteBehavior.Cascade);

            // // grduation plan -> studens (one-to-many)
            modelBuilder.Entity<Student>()
            .HasOne(s => s.GraduationPlan)
            .WithMany(g => g.Students)
            .HasForeignKey(s => s.GraduationPlanId)
            .OnDelete(DeleteBehavior.Restrict);


            // GraduationPlan -> Department (One-to-One)
            modelBuilder.Entity<GraduationPlan>()
                .HasOne(g => g.Department)
                .WithOne(d => d.GraduationPlan)
                .HasForeignKey<GraduationPlan>(g => g.D_id)
                .OnDelete(DeleteBehavior.Cascade);

            // Course -> Department (Many-to-One)
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Department)
                .WithMany(d => d.Courses)
                .HasForeignKey(c => c.D_id);

            // Prerequisite -> Course (Many-to-One)
            modelBuilder.Entity<Prerequisite>()
                .HasOne(p => p.Course)
                .WithMany(c => c.Prerequisites)
                .HasForeignKey(p => p.C_id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Prerequisite>()
                .Property(p => p.RequiredCourseId)
                .IsRequired(false);

            modelBuilder.Entity<Prerequisite>()
                .Property(p => p.MinimumGPA)
                .IsRequired(false);

            // GraduationPlan -> Term (One-to-Many)
            modelBuilder.Entity<GraduationPlan>()
                .HasMany(g => g.Terms)
                .WithOne(t => t.GraduationPlan)
                .HasForeignKey(t => t.GraduationPlanId);

            //Course -> term 
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Term)
                .WithMany(t => t.Courses)
                .HasForeignKey(c => c.TermId)
                .OnDelete(DeleteBehavior.NoAction);

            // Section -> Course (Many-to-One)
            modelBuilder.Entity<Section>()
                .HasOne(sec => sec.Course)
                .WithMany(c => c.Sections)
                .HasForeignKey(sec => sec.C_id);

            // Lecture -> Course (Many-to-One)
            modelBuilder.Entity<Lecture>()
                .HasOne(l => l.Course)
                .WithMany(c => c.Lectures)
                .HasForeignKey(l => l.C_id)
                .OnDelete(DeleteBehavior.NoAction);

            // Group -> Lecture (One-to-One)
            modelBuilder.Entity<Group>()
                .HasOne(g => g.Lecture)
                .WithMany()
                .HasForeignKey(g => g.LectureId)
                .OnDelete(DeleteBehavior.Restrict);

            // Group -> Section (One-to-One)
            modelBuilder.Entity<Group>()
                .HasOne(g => g.Section)
                .WithMany()
                .HasForeignKey(g => g.SectionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Group>()
            .HasOne(g => g.Course)
            .WithMany(c => c.Groups)
            .HasForeignKey(g => g.C_id)
            .OnDelete(DeleteBehavior.Cascade);

            // Waitlist -> Student (Many-to-One)
            modelBuilder.Entity<Waitlist>()
                .HasOne(w => w.Student)
                .WithMany()
                .HasForeignKey(w => w.S_id)
                .OnDelete(DeleteBehavior.NoAction);

            // Waitlist -> Course (Many-to-One)
            modelBuilder.Entity<Waitlist>()
                .HasOne(w => w.Course)
                .WithMany()
                .HasForeignKey(w => w.C_id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Lecture>()
            .Property(l => l.Day)
            .HasConversion<string>();

            modelBuilder.Entity<Section>()
            .Property(l => l.Day)
            .HasConversion<string>();

            base.OnModelCreating(modelBuilder);

             modelBuilder.Entity<SelectedCourse>()
            .HasOne(sc => sc.Student)
            .WithMany(s => s.SelectedCourses) 
            .HasForeignKey(sc => sc.S_id)
            .OnDelete(DeleteBehavior.Cascade); 

          
            modelBuilder.Entity<SelectedCourse>()
                .HasOne(sc => sc.Course)
                .WithMany(c => c.SelectedCourses) 
                .HasForeignKey(sc => sc.C_id)
                .OnDelete(DeleteBehavior.NoAction);

           
            modelBuilder.Entity<SelectedCourse>()
                .HasOne(sc => sc.Group)
                .WithMany() 
                .HasForeignKey(sc => sc.GroupId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
