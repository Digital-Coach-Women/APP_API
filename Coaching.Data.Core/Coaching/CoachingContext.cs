using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Coaching.Data.Core.Coaching.Entities;

namespace Coaching.Data.Core.Coaching
{
    public partial class CoachingContext : DbContext
    {
        public CoachingContext()
        {
        }

        public CoachingContext(DbContextOptions<CoachingContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admin> Admin { get; set; } = null!;
        public virtual DbSet<Chat> Chat { get; set; } = null!;
        public virtual DbSet<ChatBot> ChatBot { get; set; } = null!;
        public virtual DbSet<ChatBotSession> ChatBotSession { get; set; } = null!;
        public virtual DbSet<ChatSession> ChatSession { get; set; } = null!;
        public virtual DbSet<Course> Course { get; set; } = null!;
        public virtual DbSet<NotificationUser> NotificationUser { get; set; } = null!;
        public virtual DbSet<Speciality> Speciality { get; set; } = null!;
        public virtual DbSet<SpecialityLevel> SpecialityLevel { get; set; } = null!;
        public virtual DbSet<SpecialityLevelCertificate> SpecialityLevelCertificate { get; set; } = null!;
        public virtual DbSet<SuccessStoires> SuccessStoires { get; set; } = null!;
        public virtual DbSet<User> User { get; set; } = null!;
        public virtual DbSet<UserCourse> UserCourse { get; set; } = null!;
        public virtual DbSet<UserSpecialityLevel> UserSpecialityLevel { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("username");
            });

            modelBuilder.Entity<Chat>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.LastCommunicateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("last_communicate_date");

                entity.Property(e => e.UserId1).HasColumnName("user_id_1");

                entity.Property(e => e.UserId2).HasColumnName("user_id_2");

                entity.HasOne(d => d.UserId1Navigation)
                    .WithMany(p => p.ChatUserId1Navigation)
                    .HasForeignKey(d => d.UserId1)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Chat_User");

                entity.HasOne(d => d.UserId2Navigation)
                    .WithMany(p => p.ChatUserId2Navigation)
                    .HasForeignKey(d => d.UserId2)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Chat_User1");
            });

            modelBuilder.Entity<ChatBot>(entity =>
            {
                entity.ToTable("Chat_Bot");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_date");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ChatBot)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Chat_Bot_User");
            });

            modelBuilder.Entity<ChatBotSession>(entity =>
            {
                entity.ToTable("Chat_Bot_Session");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ChatBotId).HasColumnName("chat_bot_id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_date");

                entity.Property(e => e.IsBot).HasColumnName("is_bot");

                entity.Property(e => e.Message)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("message");

                entity.HasOne(d => d.ChatBot)
                    .WithMany(p => p.ChatBotSession)
                    .HasForeignKey(d => d.ChatBotId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Chat_Bot_Session_Chat_Bot");
            });

            modelBuilder.Entity<ChatSession>(entity =>
            {
                entity.ToTable("Chat_Session");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ChatId).HasColumnName("chat_id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_date");

                entity.Property(e => e.Message)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("message");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Chat)
                    .WithMany(p => p.ChatSession)
                    .HasForeignKey(d => d.ChatId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Chat_Session_Chat");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ChatSession)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Chat_Session_User");
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .HasMaxLength(800)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Order).HasColumnName("order");

                entity.Property(e => e.Process)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("process");

                entity.Property(e => e.SpecialityLevelId).HasColumnName("speciality_level_id");

                entity.Property(e => e.Title)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("title");

                entity.Property(e => e.Video)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("video");

                entity.HasOne(d => d.SpecialityLevel)
                    .WithMany(p => p.Course)
                    .HasForeignKey(d => d.SpecialityLevelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Course_Speciality_Level");
            });

            modelBuilder.Entity<NotificationUser>(entity =>
            {
                entity.ToTable("Notification_User");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.SendAdvice).HasColumnName("send_advice");

                entity.Property(e => e.SendCourses).HasColumnName("send_courses");

                entity.Property(e => e.SendFollow).HasColumnName("send_follow");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.NotificationUser)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Notification_User_User");
            });

            modelBuilder.Entity<Speciality>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Image)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("image");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<SpecialityLevel>(entity =>
            {
                entity.ToTable("Speciality_Level");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CupImage)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("cup_image");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Order).HasColumnName("order");

                entity.Property(e => e.SpecialityId).HasColumnName("speciality_id");

                entity.HasOne(d => d.Speciality)
                    .WithMany(p => p.SpecialityLevel)
                    .HasForeignKey(d => d.SpecialityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Speciality_Level_Speciality");
            });

            modelBuilder.Entity<SpecialityLevelCertificate>(entity =>
            {
                entity.ToTable("Speciality_Level_Certificate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Company)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("company");

                entity.Property(e => e.SpecialityLevelId).HasColumnName("speciality_level_id");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("title");

                entity.Property(e => e.Uri)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("uri");

                entity.HasOne(d => d.SpecialityLevel)
                    .WithMany(p => p.SpecialityLevelCertificate)
                    .HasForeignKey(d => d.SpecialityLevelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Speciality_Level_Certificate_Speciality_Level");
            });

            modelBuilder.Entity<SuccessStoires>(entity =>
            {
                entity.ToTable("Success_Stoires");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Age).HasColumnName("age");

                entity.Property(e => e.Appointment)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("appointment");

                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("city");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Profession)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("profession");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Birthdate)
                    .HasColumnType("datetime")
                    .HasColumnName("birthdate");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.FcmToken)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("fcm_token");

                entity.Property(e => e.LastName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("last_name");

                entity.Property(e => e.Linkedin)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("linkedin");

                entity.Property(e => e.MotherLastName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("mother_last_name");

                entity.Property(e => e.Names)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("names");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.Token)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("token");

                entity.Property(e => e.UserLevel).HasColumnName("user_level");
            });

            modelBuilder.Entity<UserCourse>(entity =>
            {
                entity.ToTable("User_Course");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CourseId).HasColumnName("course_id");

                entity.Property(e => e.IsFinish).HasColumnName("isFinish");

                entity.Property(e => e.Time).HasColumnName("time");

                entity.Property(e => e.UserSpecialityLevelId).HasColumnName("user_speciality_level_id");

                entity.HasOne(d => d.UserSpecialityLevel)
                    .WithMany(p => p.UserCourse)
                    .HasForeignKey(d => d.UserSpecialityLevelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Course_User_Speciality_Level");
            });

            modelBuilder.Entity<UserSpecialityLevel>(entity =>
            {
                entity.ToTable("User_Speciality_Level");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IsFinish).HasColumnName("is_finish");

                entity.Property(e => e.SpecialityId).HasColumnName("speciality_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Speciality)
                    .WithMany(p => p.UserSpecialityLevel)
                    .HasForeignKey(d => d.SpecialityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Speciality_Level_Speciality_Level");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserSpecialityLevel)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Speciality_Level_User");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
