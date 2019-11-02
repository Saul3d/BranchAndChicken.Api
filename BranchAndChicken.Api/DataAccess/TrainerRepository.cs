using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BranchAndChicken.Api.Commands;
using BranchAndChicken.Api.Controllers;
using BranchAndChicken.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Dapper;

namespace BranchAndChicken.Api.DataAccess
{
    public class TrainerRepository
    {
        string _connectionString = "Server = localhost;Database=BranchAndChicken;Trusted_Connection=True";

        public List<Trainer> GetAll()
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var trainers = db.Query<Trainer>(@"Select * From Trainer");
                //can use AsList() instead of ToList(), AsList() checks to see if IEnumerable is already a LIst
                return trainers.ToList();
            }

        }

        public Trainer Get(string name)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var sql = $@"Select *  
                             from Trainer
                             where Trainer.Name = @trainerName";

                var Trainer = db.QueryFirst<Trainer>(sql, new {trainerName = name});
                return Trainer;
            }
        }

        public bool Remove(string name)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var sql = @"delete
                            from Trainer as t
                            where t.Name = @name";

                return db.Execute(sql, new {name}) == 1;

            }
        }

        public Trainer Update(Trainer updatedTrainer, int id)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var sql = @"
                    UPDATE [dbo].[Trainer]
                        SET [Name] = @name
                            ,[YearsOfExperience] = @yearsOfExperience
                            ,[Specialty] = @specialty
                    output inserted.*
                    WHERE id = @id";

                updatedTrainer.Id = id;

                var trainer = db.QueryFirst<Trainer>(sql, updatedTrainer);
                
                return trainer;
            }
        }

        public Trainer Add(Trainer newTrainer)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var sql = @"
                    INSERT INTO [dbo].[Trainer]
                               ([Name]
                               ,[YearsOfExperience]
                               ,[Specialty])
	                     output inserted.*
                         VALUES
                               (@name
                               ,@YearsOfExperience
                               ,@Specialty)";

                return db.QueryFirst<Trainer>(sql, newTrainer);
            }
        }
    }
}
