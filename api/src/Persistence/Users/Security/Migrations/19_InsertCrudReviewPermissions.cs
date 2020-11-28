using System;
using System.Linq;
using Dapper;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users.Security;
using FluentMigrator;

namespace DetailingArsenal.Persistence.Users.Security.Migrations {
    [Migration(2020_1_14_0, "Insert crud review permissions")]
    public class InsertCrudReviewPermissions : Migration {
        const string Scope = "reviews";

        public override void Up() {
            this.AddCrudPermissions(Scope);
        }

        public override void Down() {
            this.RemoveCrudPermissions(Scope);
        }
    }
}