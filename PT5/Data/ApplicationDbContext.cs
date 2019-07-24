using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MPW.Utilities;
using MPW.Data;

namespace MPW.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        #region Constructor

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Sets up the relationships for the model. Called by the Framework.
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<AppUser>()
                .HasOne(a => a.Mentor)
                .WithOne(m => m.AppUser)
                .HasForeignKey<Mentor>(m => m.AppUserId);

            builder.Entity<AppUser>()
                .HasOne(b => b.Protege)
                .WithOne(p => p.AppUser)
                .HasForeignKey<Protege>(p => p.AppUserId);

            builder.Entity<AppUser>()
                .HasOne(b => b.Client)
                .WithOne(c => c.AppUser)
                .HasForeignKey<Client>(c => c.AppUserId);

            builder.Entity<Pair>()
                .HasOne(p => p.Mentor)
                .WithMany(m => m.Pairs)
                .IsRequired(false);

            builder.Entity<Pair>()
                .HasOne(p => p.Protege)
                .WithMany(m => m.Pairs)
                .IsRequired(false);

            builder.Entity<Pair>()
                .HasOne(p => p.Client)
                .WithMany(m => m.Pairs)
                .IsRequired(false);

            builder.Entity<Course>()
                .HasOne(c => c.Pair)
                .WithMany(p => p.Courses);

            builder.Entity<Assignment>()
                .HasMany(a => a.Comments)
                .WithOne(c => c.Assignment)
                .HasForeignKey(c => c.AssignmentID);

            builder.Entity<Assignment>()
                .HasMany(a => a.Feedback)
                .WithOne(f => f.Assignment)
                .HasForeignKey(a => a.AssignmentID);

            builder.Entity<Assignment>()
                   .HasMany(a => a.Documents)
                   .WithOne(d => d.Assignment)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Session>()
                .HasOne(s => s.Agenda)
                .WithOne(a => a.Session)
                .HasForeignKey<Agenda>(a => a.SessionID);

            builder.Entity<Document>()
                   .HasOne(d => d.Assignment)
                   .WithMany(a => a.Documents)
                   .IsRequired(false);

            builder.Entity<Document>()
                   .HasOne(d => d.Event)
                   .WithMany(e => e.Documents)
                   .IsRequired(false);

            builder.Entity<Document>()
                   .HasOne(d => d.Session)
                   .WithMany(s => s.Documents)
                   .IsRequired(false);

            builder.Entity<Document>()
                .HasOne(d => d.Resource)
                .WithMany(s => s.Documents)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

        }

        public DbSet<Mentor> Mentor { get; set; }
        public DbSet<Protege> Protege { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<StrategicDomain> StrategicDomains { get; set; }
        public DbSet<Trello> Objective { get; set; }
        public DbSet<Certificate> Certificate { get; set; }
        public DbSet<Pair> Pair { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<Session> Session { get; set; }
        public DbSet<Event> Event { get; set; }
        public DbSet<Assignment> Assignment { get; set; }
        public DbSet<Document> Document { get; set; }
        public DbSet<Resource> Resource { get; set; }
        public DbSet<Agenda> Agenda { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Gets the current app user that is logged in
        /// </summary>
        /// <returns></returns>
        public virtual async Task<AppUser> GetAppUserAsync(string username)
        {
            var user = await Users
               .Where(u => u.UserName == username)
                   .Include(u => u.Mentor)
                   .Include(u => u.Protege)
                   .Include(u => u.Client)
               .SingleOrDefaultAsync();

            return user;
        }

        /// <summary>
        /// Adds the about section to the AppUser's mentor about section
        /// </summary>
        /// <param name="mentor"></param>
        /// <param name="about"></param>
        /// <returns></returns>
        public virtual async Task<Data.Mentor> AddMentorAboutAsync(Data.Mentor mentor, string about)
        {
            mentor.About = about;
            await SaveChangesAsync();

            return mentor;
        }

        /// <summary>
        /// Returns an app user with that is a Mentor with all of its Navigational Properties
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public virtual async Task<AppUser> GetMentorAsync(string username)
        {
            var user = await Users
               .Where(u => u.UserName == username)
                   .Include(u => u.Mentor)
                    .ThenInclude(m => m.Address)
                   .Include(u => u.Mentor)
                    .ThenInclude(m => m.Certificates)
                   .Include(u => u.Mentor)
                    .ThenInclude(m => m.StrategicDomains)
               .SingleOrDefaultAsync();

            return user;
        }

        /// <summary>
        /// Updates the Mentor's Address Navigational Property
        /// </summary>
        /// <param name="mentor"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public virtual async Task<Mentor> UpdateMentor(Mentor mentor, Address address)
        {
            mentor.Address = address;
            await SaveChangesAsync();
            return mentor;
        }

        /// <summary>
        /// Updates the Mentor's <paramref name="strategicDomain"/>'s Navigational Property
        /// </summary>
        /// <param name="mentor"></param>
        /// <param name="strategicDomain"></param>
        /// <returns></returns>
        public virtual async Task<Mentor> UpdateMentor(Mentor mentor, StrategicDomain strategicDomain)
        {
            mentor.StrategicDomains.Add(strategicDomain);
            await SaveChangesAsync();
            return mentor;
        }

        /// <summary>
        /// Updates the Mentor's <paramref name="certificate"/>'s Navigational Property
        /// </summary>
        /// <param name="mentor"></param>
        /// <param name="certificate"></param>
        /// <returns></returns>
        public virtual async Task<Mentor> UpdateMentor(Mentor mentor, Certificate certificate)
        {
            if (mentor.Certificates == null)
            {
                mentor.Certificates = new List<Certificate>();
            }

            mentor.Certificates.Add(certificate);
            await SaveChangesAsync();
            return mentor;
        }

        /// <summary>
        /// Adds the <paramref name="pair"/> to the Database
        /// </summary>
        /// <param name="pair"></param>
        /// <returns></returns>
        public virtual async Task AddPairAsync(Pair pair)
        {
            await this.Pair.AddAsync(pair);
            await this.SaveChangesAsync();
        }

        /// <summary>
        /// Gets each Pair the Mentor belongs to using the <paramref name="MentorID"/>
        /// </summary>
        /// <param name="MentorID"></param>
        /// <returns></returns>
        public virtual async Task<IList<Pair>> GetPairsForMentorAsync(int MentorID)
        {
            var pair = await this.Pair
                .Where(p => p.MentorID == MentorID)
                .Include(p => p.Mentor)
                    .ThenInclude(m => m.AppUser)
                .Include(p => p.Protege)
                    .ThenInclude(p => p.AppUser)
                .Include(p => p.Client)
                    .ThenInclude(c => c.AppUser)
                .ToListAsync();

            return pair;
        }

        /// <summary>
        /// Gets the current app user that is logged in
        /// </summary>
        /// <returns></returns>
        public virtual async Task<AppUser> GetProtegeAppUserAsync(string username)
        {
            var user = await Users
               .Where(u => u.UserName == username)
                   .Include(u => u.Protege)
               .SingleOrDefaultAsync();

            return user;
        }

        /// <summary>
        /// Adds the about section to the AppUser's protege about section
        /// </summary>
        /// <param name="protege"></param>
        /// <param name="about"></param>
        /// <returns></returns>
        public virtual async Task<Data.Protege> AddProtegeAboutAsync(Data.Protege protege, string about)
        {
            protege.About = about;
            await SaveChangesAsync();

            return protege;
        }

        /// <summary>
        /// Returns an app user with that is a protege with all of its Navigational Properties
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public virtual async Task<AppUser> GetProtegeAsync(string username)
        {
            var user = await Users
               .Where(u => u.UserName == username)
                   .Include(u => u.Protege)
                    .ThenInclude(p => p.Address)
                   .Include(u => u.Protege)
                    .ThenInclude(p => p.Certificates)
                   .Include(u => u.Protege)
                    .ThenInclude(p => p.AreasOfImprovement)
               .SingleOrDefaultAsync();

            return user;
        }

        /// <summary>
        /// Updates the Protege's Address Navigational Property
        /// </summary>
        /// <param name="protege"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public virtual async Task<Protege> UpdateProtege(Protege protege, Address address)
        {
            protege.Address = address;
            await SaveChangesAsync();
            return protege;
        }

        /// <summary>
        /// Updates the protege's <paramref name="areasOfImprovement"/>'s Navigational Property
        /// </summary>
        /// <param name="protege"></param>
        /// <param name="areasOfImprovement"></param>
        /// <returns></returns>
        public virtual async Task<Protege> UpdateProtege(Protege protege, AreasOfImprovement areasOfImprovement)
        {
            protege.AreasOfImprovement.Add(areasOfImprovement);
            await SaveChangesAsync();
            return protege;
        }

        /// <summary>
        /// Updates the Protege's <paramref name="certificate"/>'s Navigational Property
        /// </summary>
        /// <param name="protege"></param>
        /// <param name="certificate"></param>
        /// <returns></returns>
        public virtual async Task<Protege> UpdateProtege(Protege protege, Certificate certificate)
        {
            if (protege.Certificates == null)
            {
                protege.Certificates = new List<Certificate>();
            }

            protege.Certificates.Add(certificate);
            await SaveChangesAsync();
            return protege;
        }

        public virtual async Task<IList<Pair>> GetPairsForProtege(int ProtegeID)
        {
            var pair = await this.Pair
                .Where(p => p.ProtegeID == ProtegeID)
                .Include(p => p.Mentor)
                    .ThenInclude(m => m.AppUser)
                .Include(p => p.Protege)
                    .ThenInclude(p => p.AppUser)
                .Include(p => p.Client)
                    .ThenInclude(c => c.AppUser)
                .ToListAsync();

            return pair;
        }

        /// <summary>
        /// Gets all possible Courses for the Pair using the <paramref name="pairID"/>
        /// </summary>
        /// <param name="pairID"></param>
        /// <returns></returns>
        public virtual async Task<IList<Course>> GetCoursesForPairAsync(int pairID)
        {
            var courses = await this.Course
                .Where(c => c.PairID == pairID)
                .ToListAsync();

            return courses;
        }

        public virtual async Task<Course> GetCourseAsync(int courseID)
        {
            var courses = await this.Course
                .Where(c => c.CourseID == courseID)
                .Include(c => c.Events)
                .Include(c => c.Sessions)
                    .ThenInclude(s => s.Assignments)
                .Include(c => c.Resources)
                .Include(c => c.Objectives)
                .Include(c => c.Pair)
                .FirstOrDefaultAsync();

            return courses;
        }
        

        /// <summary>
        /// Gets the current app user that is logged in
        /// </summary>
        /// <returns></returns>
        public virtual async Task<AppUser> GetClientAppUserAsync(string username)
        {
            var user = await Users
               .Where(u => u.UserName == username)
                   .Include(u => u.Client)
               .SingleOrDefaultAsync();

            return user;
        }

        /// <summary>
        /// Adds the about section to the AppUser's Client about section
        /// </summary>
        /// <param name="client"></param>
        /// <param name="about"></param>
        /// <returns></returns>
        public virtual async Task<Data.Client> AddClientAboutAsync(Data.Client client, string about)
        {
            client.About = about;
            await SaveChangesAsync();

            return client;
        }

        /// <summary>
        /// Returns an app user with that is a Client with all of its Navigational Properties
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public virtual async Task<AppUser> GetClientAsync(string username)
        {
            var user = await Users
               .Where(u => u.UserName == username)
                   .Include(u => u.Client)
                    .ThenInclude(c => c.Address)
               .SingleOrDefaultAsync();

            return user;
        }

        /// <summary>
        /// Updates the Client's Address Navigational Property
        /// </summary>
        /// <param name="client"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public virtual async Task<Client> UpdateClient(Client client, Address address)
        {
            client.Address = address;
            await SaveChangesAsync();
            return client;
        }

        /// <summary>
        /// Gets all of the Courses for the Client
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns></returns>
        public virtual async Task<IList<Pair>> GetPairsForClient(int ClientID)
        {
            var pair = await this.Pair
                .Where(p => p.ClientID == ClientID)
                .Include(p => p.Mentor)
                    .ThenInclude(m => m.AppUser)
                .Include(p => p.Protege)
                    .ThenInclude(p => p.AppUser)
                .Include(p => p.Client)
                    .ThenInclude(c => c.AppUser)
                .ToListAsync();

            return pair;
        }

        /// <summary>
        /// Adds the Trello <paramref name="objective"/> to the Database
        /// </summary>
        /// <param name="objective"></param>
        /// <returns></returns>
        public virtual async Task AddObjectiveAsync(Trello objective)
        {
            await this.Objective.AddAsync(objective);
            await this.SaveChangesAsync();
        }

        /// <summary>
        /// Get all Trello Assigned to the Course
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns>
        /// </returns>
        public virtual async Task<IList<Trello>> GetObjectivesAsync(int courseID)
        {
            var objectives = await this.Objective
                .Include(o => o.Course)
                .Where(o => o.CourseID == courseID)
                .ToListAsync();

            return objectives;
        }


        /// <summary>
        /// Gets the objective async using the <paramref name="objectiveID"/>.
        /// </summary>
        /// <returns>The objective async.</returns>
        /// <param name="objectiveID">Trello identifier.</param>
        public virtual async Task<Trello> GetObjectiveAsync(int objectiveID)
        {
            var objective = await this.Objective
                .FirstOrDefaultAsync(m => m.ID == objectiveID);

            return objective;
        }

        /// <summary>
        /// Updates the objective async.
        /// </summary>
        /// <returns>The objective async.</returns>
        /// <param name="objective">Objective.</param>
        public virtual async Task<bool> UpdateObjectiveAsync(Trello objective)
        {
            var success = true;
            this.Attach(objective);
            this.Entry(objective).Property(p => p.DateCompleted).IsModified = true;
            this.Entry(objective).Property(p => p.EstimatedDateCompleted).IsModified = true;
            this.Entry(objective).Property(p => p.Name).IsModified = true;

            try
            {
                await this.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                success = false;
            }

            return success;
        }

        /// <summary>
        /// Deletes the objective using the <paramref name="objectiveID"/>.
        /// </summary>
        /// <returns>Successfully deleted the objective.</returns>
        /// <param name="objectiveID">Trello identifier.</param>
        public virtual async Task<bool> DeleteObjective(int objectiveID)
        {
            var objective = await this.Objective.FindAsync(objectiveID);
            var success = true;

            try
            {
                if (objective != null)
                {
                    this.Objective.Remove(objective);
                    await this.SaveChangesAsync();
                }
            }
            catch
            {
                success = false;
            }

            return success;

        }

        /// <summary>
        /// Uploads the document using <paramref name="file"/>.
        /// </summary>
        /// <returns>The document.</returns>
        /// <param name="file">File.</param>
        public virtual async Task<bool> UploadDocument(FileUpload file)
        {
            var success = true;

            try
            {
                using(var stream = file.Document.OpenReadStream())
            {
                    var memoryStream = new MemoryStream();
                    await stream.CopyToAsync(memoryStream);

                    var document = new Document
                    {
                        FileSize = file.Document.Length,
                        FileType = file.Document.ContentType,
                        Name = file.Document.FileName,
                        Category = file.Category,
                        UploadDate = DateTime.Now,
                        File = await memoryStream.ToArrayAsync()
                    };

                    await this.AddAsync(document);

                    await this.SaveChangesAsync();
                }
            }
            catch
            {
                success = false;
            }

            return success;
        }

        /// <summary>
        /// Gets the document async using the document <paramref name="id"/>.
        /// </summary>
        /// <returns>The document async.</returns>
        /// <param name="id">Identifier.</param>
        public virtual async Task<Document> GetDocumentAsync(int id)
        {
            var document = await this.Document
                                        .Where(d => d.DocumentID == id)
                                        .Include(d => d.Assignment)
                                        .Include(d => d.Event)
                                        .FirstOrDefaultAsync();

            return document;
        }

        /// <summary>
        /// Creates the session async.
        /// </summary>
        /// <returns>The session async.</returns>
        /// <param name="Session">Session.</param>
        public virtual async Task<bool> CreateSessionAsync(Session Session)
        {
            var success = true;
            try
            {
                await this.Session.AddAsync(Session);
                await this.SaveChangesAsync();
            }
            catch (Exception)
            {

                success = false;
            }

            return success;
        }

        /// <summary>
        /// Gets the session async.
        /// </summary>
        /// <returns>The session async.</returns>
        /// <param name="id">Identifier.</param>
        public virtual async Task<Session> GetSessionAsync(int id)
        {
            var session = await this.Session
                .Include(s => s.Course)
                    .ThenInclude(c => c.Pair)
                .Include(s => s.Agenda)
                .Include(s => s.Documents)
                .Include(s => s.Events)
                .Include(s => s.Assignments)
                .FirstOrDefaultAsync(m => m.SessionID == id);

            return session;
        }

        /// <summary>
        /// Adds the document async.
        /// </summary>
        /// <returns>The document async.</returns>
        /// <param name="document">Document.</param>
        public virtual async Task<bool> AddDocumentAsync(Document document)
        {
            var success = true;
            try
            {
                await this.Document.AddAsync(document);
                await this.SaveChangesAsync();
            }
            catch
            {
                success = false;
            }

            return success;
        }

        /// <summary>
        /// Adds the assingment async.
        /// </summary>
        /// <returns>The assingment async.</returns>
        /// <param name="assignment">Assignment.</param>
        public virtual async Task<bool> AddAssingmentAsync(Assignment assignment)
        {
            var success = true;
            try
            {
                await this.Assignment.AddAsync(assignment);
                await this.SaveChangesAsync();
            }
            catch
            {
                success = false;
            }

            return success;

        }

        /// <summary>
        /// Gets the assignment async.
        /// </summary>
        /// <returns>The assignment async.</returns>
        /// <param name="id">Identifier.</param>
        public virtual async Task<Assignment> GetAssignmentAsync(int id)
        {
            var assingment = await this.Assignment
                    .Where(a => a.AssignmentID == id)
                    .Include(a => a.Session)
                        .ThenInclude(s => s.Course)
                            .ThenInclude(c => c.Pair)
                    .FirstOrDefaultAsync();

            return assingment;
        }

        /// <summary>
        /// Gets the latest document with assingmnet IDA sync.
        /// </summary>
        /// <returns>The latest document with assingmnet IDA sync.</returns>
        /// <param name="id">Identifier.</param>
        public virtual async Task<Document> GetLatestDocumentWithAssingmnetIDAsync(int id)
        {

            var document = await Document
                .Where(d => d.AssignmentID == id)
                .OrderByDescending(d => d.DocumentID)
                .FirstOrDefaultAsync();

            return document;
        }

        /// <summary>
        /// Deletes the assignment async.
        /// </summary>
        /// <returns>The assignment async.</returns>
        /// <param name="id">Identifier.</param>
        public virtual async Task<bool> DeleteAssignmentAsync(int id)
        {
            var success = true;
            try
            {
                var assignment = await this.GetAssignmentAsync(id);
                this.Assignment.Remove(assignment);
                await this.SaveChangesAsync();


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                success = false;
            }

            return success;
        }

        /// <summary>
        /// Updates the assignment async.
        /// </summary>
        /// <returns>The assignment async.</returns>
        /// <param name="assignment">Assignment.</param>
        public virtual async Task<bool> UpdateAssignmentAsync(Assignment assignment)
        {
            var success = true;
            try
            {
                var dbAssignment = await GetAssignmentAsync(assignment.AssignmentID);
                this.Attach(dbAssignment);

                dbAssignment.Description = assignment.Description;
                dbAssignment.DueDate = assignment.DueDate;
                dbAssignment.Title = assignment.Title;
                dbAssignment.StartDate = assignment.StartDate;
                dbAssignment.DateCompleted = assignment.DateCompleted;

                this.Entry(dbAssignment).Property(p => p.DateCompleted).IsModified = true;
                this.Entry(dbAssignment).Property(p => p.Description).IsModified = true;
                this.Entry(dbAssignment).Property(p => p.Title).IsModified = true;
                this.Entry(dbAssignment).Property(p => p.StartDate).IsModified = true;
                this.Entry(dbAssignment).Property(p => p.DateCompleted).IsModified = true;

                await this.SaveChangesAsync();
            }
            catch
            {
                success = false;   
            }

            return success;
        }

        /// <summary>
        /// Adds the agenda async.
        /// </summary>
        /// <returns>The agenda async.</returns>
        /// <param name="agenda">Agenda.</param>
        public virtual async Task<bool> AddAgendaAsync(Agenda agenda)
        {
            var success = true;
            try
            {
                await Agenda.AddAsync(agenda);
                await SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                success = false;
            }

            return success;
        }

        /// <summary>
        /// Deletes the agenda async.
        /// </summary>
        /// <returns>The agenda async.</returns>
        /// <param name="agendaID">Agenda identifier.</param>
        public virtual async Task<bool> DeleteAgendaAsync(int agendaID)
        {
            var success = true;
            try
            {
                var agenda = await Agenda.FirstOrDefaultAsync(a => a.AgendaID == agendaID);
                Agenda.Remove(agenda);
                await SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                success = false;
            }

            return success;
        }

        /// <summary>
        /// Gets the agenda async.
        /// </summary>
        /// <returns>The agenda async.</returns>
        /// <param name="agendaID">Agenda identifier.</param>
        public virtual async Task<Agenda> GetAgendaAsync(int agendaID)
        {
            var agenda = await Agenda
                .Include(a => a.Document)
                .Include(a => a.Session)
                    .ThenInclude(s => s.Course)
                        .ThenInclude(c => c.Pair)
                .FirstOrDefaultAsync(a => a.AgendaID == agendaID);

            return agenda;
        }

        /// <summary>
        /// Deletes the document async.
        /// </summary>
        /// <returns>The document async.</returns>
        /// <param name="documentID">Document identifier.</param>
        public virtual async Task<bool> DeleteDocumentAsync(int documentID)
        {
            var success = true;

            try
            {
                var document = await GetDocumentAsync(documentID);
                Document.Remove(document);
                await SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                success = false;
            }

            return success;
        }

        /// <summary>
        /// Gets the resource and the Course and documents related to the resouce
        /// </summary>
        /// <param name="id"></param>
        /// <returns>resource</returns>
        public virtual async Task<Resource> GetResourceAsync(int id)
        {
            var resource = await this.Resource
                .Include(r => r.Course)
                .Include(d => d.Documents)
                .FirstOrDefaultAsync(m => m.ResourceID == id);

            return resource;
        }

        /// <summary>
        /// Adds the resource to the database
        /// </summary>
        /// <param name="Resource"></param>
        /// <returns>success</returns>
        public virtual async Task<bool> CreateResourceAsync(Resource Resource)
        {
            var success = true;
            try
            {
                await this.Resource.AddAsync(Resource);
                await this.SaveChangesAsync();
            }
            catch (Exception)
            {

                success = false;
            }

            return success;
        }

        /// <summary>
        /// Deletes the resouce from the database based on the id being passed in
        /// </summary>
        /// <param name="resourceID"></param>
        /// <returns>success</returns>
        public virtual async Task<bool> DeleteResource(int resourceID)
        {
            var resource = await this.Resource.FindAsync(resourceID);
            var success = true;

            try
            {
                if (resource != null)
                {
                    this.Resource.Remove(resource);
                    await this.SaveChangesAsync();
                }
            }
            catch
            {
                success = false;
            }

            return success;

        }

        /// <summary>
        /// Adds the course async.
        /// </summary>
        /// <returns>The course async.</returns>
        /// <param name="course">Course.</param>
        public virtual async Task<bool> AddCourseAsync(Course course)
        {
            var success = true;
            try
            {
                await Course.AddAsync(course);
                await SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                success = false;
            }

            return success;
        }

        /// <summary>
        /// Gets the course async.
        /// </summary>
        /// <returns>The course async.</returns>
        /// <param name="course">Course.</param>
        public virtual async Task<Course> GetCourseAsync(Course course)
        {
            var dbCourse = await this
                .Course
                .Include(c => c.Pair)
                .FirstOrDefaultAsync(c => c.CourseName == course.CourseName
                                    && c.EndDate == course.EndDate
                                     && c.StartDate == course.StartDate);

            return dbCourse;
        }

        /// <summary>
        /// Gets the pair async.
        /// </summary>
        /// <returns>The pair async.</returns>
        /// <param name="mentorID">Mentor identifier.</param>
        /// <param name="joinCode">Join code.</param>
        public virtual async Task<Pair> GetPairAsync(int mentorID, string joinCode)
        {
            var pair = await this.Pair.FirstOrDefaultAsync(p => p.MentorID == mentorID
                                                && p.JoinCode == joinCode);
            return pair;
        }

        /// <summary>
        /// Gets the pair async.
        /// </summary>
        /// <returns>The pair async.</returns>
        /// <param name="pairID">Pair identifier.</param>
        public virtual async Task<Pair> GetPairAsync(int pairID)
        {
            var pair = await Pair
                .Include(p => p.Courses)
                .FirstOrDefaultAsync(p => p.PairID == pairID);
            return pair;
        }

        /// <summary>
        /// Gets the event async.
        /// </summary>
        /// <returns>The event async.</returns>
        /// <param name="eventID">Event identifier.</param>
        public virtual async Task<Event> GetEventAsync(int eventID)
        {
            var dbEvent = await Event
                .Include(e => e.Course)
                    .ThenInclude(c => c.Pair)
                .FirstOrDefaultAsync(e => e.EventID == eventID);

            return dbEvent;
        }

        /// <summary>
        /// Adds the event async.
        /// </summary>
        /// <returns>The event async.</returns>
        /// <param name="userEvent">User event.</param>
        public virtual async Task<bool> AddEventAsync(Event userEvent)
        {
            var success = true;
            try
            {
                Event.Add(userEvent);
                await SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                success = false;
            }

            return success;
        }

        /// <summary>
        /// Updates the event async.
        /// </summary>
        /// <returns>The event async.</returns>
        /// <param name="userEvent">User event.</param>
        public virtual async Task<bool> UpdateEventAsync(Event userEvent)
        {
            var success = true;
            try
            {
                var dbEvent = await GetEventAsync(userEvent.EventID);

                dbEvent.EventName = userEvent.EventName;
                dbEvent.StartDate = userEvent.StartDate;
                dbEvent.Attended = userEvent.Attended;
                dbEvent.EndTime = userEvent.EndTime;
                dbEvent.Type = userEvent.Type;

                this.Entry(dbEvent).Property(p => p.EventName).IsModified = true;
                this.Entry(dbEvent).Property(p => p.StartDate).IsModified = true;
                this.Entry(dbEvent).Property(p => p.Attended).IsModified = true;
                this.Entry(dbEvent).Property(p => p.EndTime).IsModified = true;
                this.Entry(dbEvent).Property(p => p.Type).IsModified = true;

                await this.SaveChangesAsync();
            }
            catch
            {
                success = false;
            }

            return success;
        }

        /// <summary>
        /// Deletes the event async.
        /// </summary>
        /// <returns>The event async.</returns>
        /// <param name="eventID">Event identifier.</param>
        public virtual async Task<bool> DeleteEventAsync(int eventID)
        {
            var success = true;

            try
            {
                var dbEvent = await Event.FirstOrDefaultAsync(e => e.EventID == eventID);
                Event.Remove(dbEvent);
                await SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                success = false;
            }

            return success;
        }

        /// <summary>
        /// Gets the public protege async.
        /// </summary>
        /// <returns>The public protege async.</returns>
        /// <param name="protegeUsername">Protege username.</param>
        public virtual async Task<Protege> GetPublicProtegeAsync(string protegeUsername)
        {
            //Querys the database to find the protege account linked to the user
            //and gets the proteges certificates, areasofimporvement, and address
            var protege = await Protege
                .Include(m => m.AppUser)
                .Include(m => m.Certificates)
                .Include(m => m.AreasOfImprovement)
                .Include(m => m.Address)
                .FirstOrDefaultAsync(m => m.AppUser.UserName == protegeUsername);

            return protege;
        }

        /// <summary>
        /// Gets the public mentor async.
        /// </summary>
        /// <returns>The public mentor async.</returns>
        /// <param name="mentorUsername">Mentor username.</param>
        public virtual async Task<Mentor> GetPublicMentorAsync(string mentorUsername)
        {
            var mentor = await Mentor
                .Include(m => m.AppUser)
                .Include(m => m.Certificates)
                .Include(m => m.StrategicDomains)
                .Include(m => m.Address)
                .FirstOrDefaultAsync(m => m.AppUser.UserName == mentorUsername);

            return mentor;
        }

        /// <summary>
        /// Gets the public client async.
        /// </summary>
        /// <returns>The public client async.</returns>
        /// <param name="clientUsername">Client username.</param>
        public async Task<Client> GetPublicClientAsync(string clientUsername)
        {
            //Gets the Client from the database plus the appuser and their address
            //based off of their username.
            var client = await Client
                .Include(m => m.AppUser)
                .Include(m => m.Address)
                .FirstOrDefaultAsync(m => m.AppUser.UserName == clientUsername);

            return client;
        }

        /// <summary>
        /// Gets the protege by IDA sync.
        /// </summary>
        /// <returns>The protege by IDA sync.</returns>
        /// <param name="protegeID">Protege identifier.</param>
        public virtual async Task<Protege> GetProtegeByIDAsync(int protegeID)
        {
            var protege = await Protege
                .Include(p => p.AppUser)
                .FirstOrDefaultAsync(p => p.ID == protegeID);

            return protege;
        }

        /// <summary>
        /// Gets the mentor by IDA sync.
        /// </summary>
        /// <returns>The mentor by IDA sync.</returns>
        /// <param name="mentorID">Mentor identifier.</param>
        public virtual async Task<Mentor> GetMentorByIDAsync(int mentorID)
        {
            var mentor = await Mentor
                .Include(m => m.AppUser)
                .FirstOrDefaultAsync(m => m.ID == mentorID);

            return mentor;
        }

        /// <summary>
        /// Gets the client by its ID
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        public virtual async Task<Client> GetClientByIDAsync(int clientID)
        {
            var client = await Client
                .Include(c => c.AppUser)
                .FirstOrDefaultAsync(m => m.ID == clientID);

            return client;
        }

        public virtual async Task<bool> UpdateSessionAsync(Session session)
        {
            var success = true;
            try
            {
                var dbSession = await GetSessionAsync(session.SessionID);
                dbSession.Description = session.Description;
                dbSession.EndDate = session.EndDate;
                dbSession.Name = session.Name;
                dbSession.StartDate = session.StartDate;

                await SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                success = false;           
            }

            return success;
        }

        #endregion

        #region Seed
        public static IList<AppUser> GetSeedAppUsers()
        {
            var users = new List<AppUser>
            {
                new AppUser
                {
                    CompanyName = "Test Company",
                    DateCreate = new DateTime(2018, 11, 20),
                    Email = "test@example.com",
                    Field = "Technology",
                    Id = "abc-123-def-456",
                    NormalizedEmail = "TEST@EXAMPLE.COM",
                    NormalizedUserName = "TEST@EXAMPLE.COM",
                    PasswordHash = "Password123!@#",
                    UserName = "test@example.com"

                },

            };

            return users;
        }


        public static IList<AppUser> GetSeedMentor()
        {
            var users = GetSeedAppUsers();
            var mentors = new List<Data.Mentor>();

            foreach (var user in users)
            {
                var mentor = new Data.Mentor
                {
                    ID = new Random().Next(1, 10),
                    Certificates = new List<Certificate>(),
                    Address = new Address(),
                    StrategicDomains = new List<StrategicDomain>()
                };
                user.Mentor = mentor;
                mentors.Add(mentor);
            }

            return users;
        }

        public static IList<string> GetSeedAbout()
        {
            var abouts = new List<string>
            {
                "We are a technology company"
            };

            return abouts;
        }

        public static IList<Address> GetSeedAddresses()
        {
            var addresses = new List<Address>
            {
                new Address
                {
                    ID = new Random().Next(2000),
                    State = "TX", StreetAddress =
                    "100 Congress Avenue",
                    StreetAddress2 = "Suite 205",
                    City = "Austin",
                    ZipCode = "78701"
                }
            };

            return addresses;
        }

        public static IList<Certificate> GetSeedCertificates()
        {
            var certificates = new List<Certificate>
            {
                new Certificate
                {
                    Description = "This is a test cert",
                    ID = new Random().Next(1, 10),
                    Name = "AWS Cloud Architect",
                    Type = "Cloud"
                },

                new Certificate
                {
                    Description = "Cert Test 2",
                    ID = new Random().Next(10, 20),
                    Name = "GCP Cloud Architect",
                    Type = "Cloud"
                }
            };

            return certificates;
        }

        public static IList<StrategicDomain> GetSeedStrategicDomains()
        {
            var sd = new List<StrategicDomain>
            {
                new StrategicDomain
                {
                    Description = "Technology Cloud Infrastructure",
                    ID = new Random().Next(0, 10),
                    Name = "Technology Cloud Infrastructure",
                    Type = "Cloud"
                },

                new StrategicDomain
                {
                    Description = "Managerial Accounting expertise through new developments",
                    ID = new Random().Next(10, 20),
                    Name = "Managerial Accounting",
                    Type = "Accounting - Business"
                }
            };

            return sd;
        }

        public static IList<Pair> GetSeedPairs()
        {
            var pairs = new List<Pair>
            {
                new Pair
                {
                    JoinCode = "abcd-1234-efgh-5678",
                    DateCreated = new DateTime(2000, 1, 1),
                    PairID = 1
                }
            };

            return pairs;
        }

        public static IList<Course> GetSeedCourses()
        {
            var courses = new List<Course>
            {
                new Course
                {
                    CourseID = 1,
                    StartDate = DateTime.Now,
                    CourseName = "Course1"
                }
            };

            return courses;
        }

        public static IList<Trello> GetSeedObjectives()
        {
            var objectives = new List<Trello>
            {
               new Trello
               {
                   ID = 1,
                   DateAdded = new DateTime(2000, 1, 1),
                   Link = "Https://trello.com",
                   Name = "Test Objective 1",
                   DateCompleted = new DateTime(2018, 11, 20)
               }
            };

            return objectives;
        }

        public static IList<Session> GetSeedSessions()
        {
            var sessions = new List<Session>
            {
                new Session
                {
                   SessionID = 1,
                   Description = "This is Test Session 1",
                   Name = "Test Session 1",
                   StartDate = new DateTime(2000, 1, 1),
                   EndDate = new DateTime(2018, 11, 20)
                }
            };

            return sessions;
        }

        public static IList<Event> GetSeedEvents()
        {
            var events = new List<Event>();
            for (int i = 0; i < 1; i++)
            {
                var iString = i.ToString();
                var seedEvent = new Event
                {
                    EventID = 1,
                    StartDate = new DateTime(2000, 1, 1),
                    Type = "Netowrking",
                    Attended = false,
                    EventName = "Test Event " + iString
                };

                events.Add(seedEvent);
            }

            return events;
        }

        public static IList<Resource> GetSeedResource()
        {
            var resources = new List<Resource>
            {
                new Resource
                {
                    ResourceID = 1,
                    Name = "Resource 1",
                    Category = "Business Plan",
                    Type = "Website",
                    Link = "http://test/com",
                    DateAdded = new DateTime(2000, 1, 1)
                }
            };

            return resources;
        }

        public static IList<Document> GetSeedDocuments()
        {
            var documents = new List<Document>
            {
                new Document
                {
                    DocumentID = 1,
                    FileType = "PDF",
                    Name = "Test Document 1",
                    UploadDate = new DateTime(2000, 1, 1),
                    Category = "Assignment"
                }
            };

            return documents;
        }

        public static IList<Assignment> GetSeedAssignments()
        {
            var assignments = new List<Assignment>();

            for (int i = 0; i < 1; i++)
            {
                var iString = i.ToString();
                var assignment = new Assignment
                {
                    AssignmentID = i,
                    DateCompleted = new DateTime(2018, 11, 20),
                    Description = "This is Test Assignment " + iString,
                    DueDate = new DateTime(2018, 11, 20),
                    Title = "Test Assingment " + iString,
                    StartDate = new DateTime(2000, 1, 1)
                };

                assignments.Add(assignment);
            }

            return assignments;
        }

        public static IList<Feedback> GetSeedFeedbacks()
        {
            var feedbacks = new List<Feedback>();
            for (int i = 0; i < 1; i++)
            {
                var feedback = new Feedback
                {
                    FeedbackID = i,
                    Content = "This is a Test Feedback 1",
                    DateCreated = new DateTime(2000, 1, 1),
                };

                feedbacks.Add(feedback);
            }

            return feedbacks;
        }

        public static IList<Comment> GetSeedComments()
        {
            var comments = new List<Comment>();
            for (int i = 0; i < 1; i++)
            {
                var comment = new Comment
                {
                    CommentID = i,
                    Content = "This is a Test Feedback 1",
                    DateCreated = new DateTime(2000, 1, 1)
                };

                comments.Add(comment);
            }

            return comments;
        }

        public static IList<Note> GetSeedNotes()
        {
            var notes = new List<Note>();

            for (int i = 0; i < 1; i++)
            {
                var iString = i.ToString();
                var note = new Note
                {
                    NoteID = i,
                    Content = "This is Test Note " + iString,
                    CreationDate = new DateTime(2000, 1, 1),
                    Name = "Test Note " + iString
                };

                notes.Add(note);
            }

            return notes;   
        }

        public static IList<Agenda> GetSeedAgenda()
        {
            var agendas = new List<Agenda>();

            for (int i = 0; i < 1; i++)
            {
                var iString = i.ToString();
                var agenda = new Agenda
                {
                    AgendaID = i,
                    CreationDate = new DateTime(2000, 1, 1),
                    Name = "Test Agenda " + iString
                };

                agendas.Add(agenda);
            }

            return agendas;
        }


        public static IList<AppUser> GetSeedProtegeAppUsers()
        {
            var users = new List<AppUser>
            {
                new AppUser
                {
                    CompanyName = "Test Company 2",
                    DateCreate = new DateTime(2018, 11, 25),
                    Email = "testp@example.com",
                    Field = "Technology",
                    Id = "abc-456-def-789",
                    NormalizedEmail = "TESTP@EXAMPLE.COM",
                    NormalizedUserName = "TESTP@EXAMPLE.COM",
                    PasswordHash = "Password456!%$",
                    UserName = "testp@example.com"

                },

            };

            return users;
        }


        public static IList<AppUser> GetSeedProtege()
        {
            var users = GetSeedProtegeAppUsers();
            var proteges = new List<Data.Protege>();

            foreach (var user in users)
            {
                var protege = new Data.Protege
                {
                    ID = new Random().Next(1, 10),
                    Certificates = new List<Certificate>(),
                    Address = new Address(),
                    AreasOfImprovement = new List<AreasOfImprovement>()
                };
                user.Protege = protege;
                proteges.Add(protege);
            }

            return users;
        }

        public static IList<string> GetSeedProtegeAbout()
        {
            var abouts = new List<string>
            {
                "We are a technology testing company"
            };

            return abouts;
        }

        public static IList<Address> GetSeedProtegeAddresses()
        {
            var protegeaddresses = new List<Address>
            {
                new Address
                {
                    ID = new Random().Next(2000),
                    State = "TX", StreetAddress =
                    "515 6th Street",
                    StreetAddress2 = "Suite 2500",
                    City = "Austin",
                    ZipCode = "78701"
                }
            };

            return protegeaddresses;
        }

        public static IList<Certificate> GetProtegeSeedCertificates()
        {
            var protegeCertificates = new List<Certificate>
            {
                new Certificate
                {
                    Description = "test certification",
                    ID = new Random().Next(1, 10),
                    Name = "AWS Cloud Soultion Architect",
                    Type = "Cloud"
                },

                new Certificate
                {
                    Description = "Certification Test",
                    ID = new Random().Next(10, 20),
                    Name = "Lean Six Sigma",
                    Type = "Project Management"
                }
            };

            return protegeCertificates;
        }

        public static IList<AreasOfImprovement> GetSeedAreasOfImprovement()
        {
            var aoi = new List<AreasOfImprovement>
            {
                new AreasOfImprovement
                {
                    Description = "Need help with Managerial Accounting practices",
                    ID = new Random().Next(0, 10),
                    Name = "Managerial Accounting",
                    Type = "Accounting - Business"
                },

                new AreasOfImprovement
                {
                    Description = "I am trying to enter the cloud technology market",
                    ID = new Random().Next(10, 20),
                    Name = "Entering Cloud Technology",
                    Type = "cloud"
                }
            };

            return aoi;
        }



        public static IList<AppUser> GetSeedClientAppUsers()
        {
            var users = new List<AppUser>
            {
                new AppUser
                {
                    CompanyName = "Test Company 3",
                    DateCreate = new DateTime(2018, 11, 25),
                    Email = "testc@example.com",
                    Field = "Technology",
                    Id = "abc-789-def-123",
                    NormalizedEmail = "TESTC@EXAMPLE.COM",
                    NormalizedUserName = "TESTC@EXAMPLE.COM",
                    PasswordHash = "Password789^&*",
                    UserName = "testc@example.com"

                },

            };

            return users;
        }


        public static IList<AppUser> GetSeedClient()
        {
            var users = GetSeedClientAppUsers();
            var clients = new List<Data.Client>();

            foreach (var user in users)
            {
                var client = new Data.Client
                {
                    ID = new Random().Next(1, 10),
                    Address = new Address()
                };
                user.Client = client;
                clients.Add(client);
            }

            return users;
        }

        public static IList<string> GetSeedClientAbout()
        {
            var abouts = new List<string>
            {
                "We are a HUB Coordinator for technology businesses"
            };

            return abouts;
        }

        public static IList<Address> GetSeedClientAddresses()
        {
            var clientaddresses = new List<Address>
            {
                new Address
                {
                    ID = new Random().Next(2000),
                    State = "TX", StreetAddress =
                    "515 6th Street",
                    StreetAddress2 = "Suite 2300",
                    City = "Austin",
                    ZipCode = "78701"
                }
            };

            return clientaddresses;
        }

        public DbSet<MPW.Data.AreasOfImprovement> AreasOfImprovement { get; set; }
        #endregion
    }
}