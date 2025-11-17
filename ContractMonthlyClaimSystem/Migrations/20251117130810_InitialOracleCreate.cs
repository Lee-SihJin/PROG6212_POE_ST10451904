using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContractMonthlyClaimSystem.Migrations
{
    /// <inheritdoc />
    public partial class InitialOracleCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ACADEMIC_MANAGERS",
                columns: table => new
                {
                    MANAGER_ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    FIRST_NAME = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    LAST_NAME = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    EMAIL = table.Column<string>(type: "NVARCHAR2(256)", maxLength: 256, nullable: false),
                    IS_ACTIVE = table.Column<int>(type: "NUMBER(1)", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACADEMIC_MANAGERS", x => x.MANAGER_ID);
                });

            migrationBuilder.CreateTable(
                name: "LECTURERS",
                columns: table => new
                {
                    LECTURER_ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    FIRST_NAME = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    LAST_NAME = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    EMAIL = table.Column<string>(type: "NVARCHAR2(256)", maxLength: 256, nullable: false),
                    EMPLOYEE_NUMBER = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    PHONE_NUMBER = table.Column<string>(type: "NVARCHAR2(15)", maxLength: 15, nullable: false),
                    HOURLY_RATE = table.Column<decimal>(type: "NUMBER(10,2)", nullable: false),
                    CONTRACT_START_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    CONTRACT_END_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    IS_ACTIVE = table.Column<int>(type: "NUMBER(1)", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LECTURERS", x => x.LECTURER_ID);
                });

            migrationBuilder.CreateTable(
                name: "PROGRAMME_COORDINATORS",
                columns: table => new
                {
                    COORDINATOR_ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    FIRST_NAME = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    LAST_NAME = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    EMAIL = table.Column<string>(type: "NVARCHAR2(256)", maxLength: 256, nullable: false),
                    DEPARTMENT = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    IS_ACTIVE = table.Column<int>(type: "NUMBER(1)", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROGRAMME_COORDINATORS", x => x.COORDINATOR_ID);
                });

            migrationBuilder.CreateTable(
                name: "ROLES",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Name = table.Column<string>(type: "NVARCHAR2(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "NVARCHAR2(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROLES", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MONTHLY_CLAIMS",
                columns: table => new
                {
                    CLAIM_ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    LECTURER_ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    CLAIM_MONTH = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    SUBMISSION_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    COORDINATOR_REVIEW_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    MANAGER_REVIEW_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    COORDINATOR_APPROVAL_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    MANAGER_APPROVAL_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    PAYMENT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    TOTAL_HOURS = table.Column<decimal>(type: "NUMBER(8,2)", nullable: false, defaultValue: 0m),
                    TOTAL_AMOUNT = table.Column<decimal>(type: "NUMBER(10,2)", nullable: false, defaultValue: 0m),
                    STATUS = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    COORDINATOR_COMMENTS = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: true),
                    MANAGER_COMMENTS = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: true),
                    COORDINATOR_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    MANAGER_ID = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MONTHLY_CLAIMS", x => x.CLAIM_ID);
                    table.ForeignKey(
                        name: "FK_MONTHLY_CLAIMS_ACADEMIC_MANAGERS_MANAGER_ID",
                        column: x => x.MANAGER_ID,
                        principalTable: "ACADEMIC_MANAGERS",
                        principalColumn: "MANAGER_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MONTHLY_CLAIMS_LECTURERS_LECTURER_ID",
                        column: x => x.LECTURER_ID,
                        principalTable: "LECTURERS",
                        principalColumn: "LECTURER_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MONTHLY_CLAIMS_PROGRAMME_COORDINATORS_COORDINATOR_ID",
                        column: x => x.COORDINATOR_ID,
                        principalTable: "PROGRAMME_COORDINATORS",
                        principalColumn: "COORDINATOR_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "USERS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    FIRST_NAME = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    LAST_NAME = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    USER_TYPE = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    LECTURER_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    COORDINATOR_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    MANAGER_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    IS_ACTIVE = table.Column<int>(type: "NUMBER(1)", nullable: false, defaultValue: 1),
                    UserName = table.Column<string>(type: "NVARCHAR2(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "NVARCHAR2(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "NVARCHAR2(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "NVARCHAR2(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    PasswordHash = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TIMESTAMP(7) WITH TIME ZONE", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USERS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_USERS_ACADEMIC_MANAGERS_MANAGER_ID",
                        column: x => x.MANAGER_ID,
                        principalTable: "ACADEMIC_MANAGERS",
                        principalColumn: "MANAGER_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_USERS_LECTURERS_LECTURER_ID",
                        column: x => x.LECTURER_ID,
                        principalTable: "LECTURERS",
                        principalColumn: "LECTURER_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_USERS_PROGRAMME_COORDINATORS_COORDINATOR_ID",
                        column: x => x.COORDINATOR_ID,
                        principalTable: "PROGRAMME_COORDINATORS",
                        principalColumn: "COORDINATOR_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ROLE_CLAIMS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    RoleId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ClaimType = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ClaimValue = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROLE_CLAIMS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ROLE_CLAIMS_ROLES_RoleId",
                        column: x => x.RoleId,
                        principalTable: "ROLES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CLAIM_ITEMS",
                columns: table => new
                {
                    ITEM_ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CLAIM_ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    WORK_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DESCRIPTION = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    HOURS_WORKED = table.Column<decimal>(type: "NUMBER(4,2)", nullable: false),
                    HOURLY_RATE = table.Column<decimal>(type: "NUMBER(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CLAIM_ITEMS", x => x.ITEM_ID);
                    table.ForeignKey(
                        name: "FK_CLAIM_ITEMS_MONTHLY_CLAIMS_CLAIM_ID",
                        column: x => x.CLAIM_ID,
                        principalTable: "MONTHLY_CLAIMS",
                        principalColumn: "CLAIM_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SUPPORTING_DOCUMENTS",
                columns: table => new
                {
                    DOCUMENT_ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CLAIM_ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    FILE_NAME = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false),
                    ORIGINAL_FILE_NAME = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    DOCUMENT_TYPE = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    FILE_DATA = table.Column<byte[]>(type: "BLOB", nullable: true),
                    FILE_SIZE = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    CONTENT_TYPE = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    UPLOAD_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DESCRIPTION = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SUPPORTING_DOCUMENTS", x => x.DOCUMENT_ID);
                    table.ForeignKey(
                        name: "FK_SUPPORTING_DOCUMENTS_MONTHLY_CLAIMS_CLAIM_ID",
                        column: x => x.CLAIM_ID,
                        principalTable: "MONTHLY_CLAIMS",
                        principalColumn: "CLAIM_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "USER_CLAIMS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    UserId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ClaimType = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ClaimValue = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER_CLAIMS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_USER_CLAIMS_USERS_UserId",
                        column: x => x.UserId,
                        principalTable: "USERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "USER_LOGINS",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UserId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER_LOGINS", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_USER_LOGINS_USERS_UserId",
                        column: x => x.UserId,
                        principalTable: "USERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "USER_ROLES",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    RoleId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER_ROLES", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_USER_ROLES_ROLES_RoleId",
                        column: x => x.RoleId,
                        principalTable: "ROLES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_USER_ROLES_USERS_UserId",
                        column: x => x.UserId,
                        principalTable: "USERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "USER_TOKENS",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    LoginProvider = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    Value = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER_TOKENS", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_USER_TOKENS_USERS_UserId",
                        column: x => x.UserId,
                        principalTable: "USERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MANAGERS_EMAIL",
                table: "ACADEMIC_MANAGERS",
                column: "EMAIL",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CLAIM_ITEMS_CLAIM",
                table: "CLAIM_ITEMS",
                column: "CLAIM_ID");

            migrationBuilder.CreateIndex(
                name: "IX_CLAIM_ITEMS_WORKDATE",
                table: "CLAIM_ITEMS",
                column: "WORK_DATE");

            migrationBuilder.CreateIndex(
                name: "IX_LECTURERS_EMAIL",
                table: "LECTURERS",
                column: "EMAIL",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LECTURERS_EMP_NUM",
                table: "LECTURERS",
                column: "EMPLOYEE_NUMBER",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MONTHLY_CLAIMS_COORDINATOR",
                table: "MONTHLY_CLAIMS",
                column: "COORDINATOR_ID");

            migrationBuilder.CreateIndex(
                name: "IX_MONTHLY_CLAIMS_LECTURER",
                table: "MONTHLY_CLAIMS",
                column: "LECTURER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_MONTHLY_CLAIMS_MANAGER",
                table: "MONTHLY_CLAIMS",
                column: "MANAGER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_MONTHLY_CLAIMS_MONTH",
                table: "MONTHLY_CLAIMS",
                column: "CLAIM_MONTH");

            migrationBuilder.CreateIndex(
                name: "IX_MONTHLY_CLAIMS_STATUS",
                table: "MONTHLY_CLAIMS",
                column: "STATUS");

            migrationBuilder.CreateIndex(
                name: "IX_COORDINATORS_EMAIL",
                table: "PROGRAMME_COORDINATORS",
                column: "EMAIL",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ROLE_CLAIMS_RoleId",
                table: "ROLE_CLAIMS",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "ROLES",
                column: "NormalizedName",
                unique: true,
                filter: "\"NormalizedName\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SUPPORTING_DOCS_CLAIM",
                table: "SUPPORTING_DOCUMENTS",
                column: "CLAIM_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SUPPORTING_DOCS_TYPE",
                table: "SUPPORTING_DOCUMENTS",
                column: "DOCUMENT_TYPE");

            migrationBuilder.CreateIndex(
                name: "IX_USER_CLAIMS_UserId",
                table: "USER_CLAIMS",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_USER_LOGINS_UserId",
                table: "USER_LOGINS",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_USER_ROLES_RoleId",
                table: "USER_ROLES",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "USERS",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_USERS_COORDINATOR",
                table: "USERS",
                column: "COORDINATOR_ID",
                unique: true,
                filter: "\"COORDINATOR_ID\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_USERS_LECTURER",
                table: "USERS",
                column: "LECTURER_ID",
                unique: true,
                filter: "\"LECTURER_ID\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_USERS_MANAGER",
                table: "USERS",
                column: "MANAGER_ID",
                unique: true,
                filter: "\"MANAGER_ID\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_USERS_TYPE",
                table: "USERS",
                column: "USER_TYPE");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "USERS",
                column: "NormalizedUserName",
                unique: true,
                filter: "\"NormalizedUserName\" IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CLAIM_ITEMS");

            migrationBuilder.DropTable(
                name: "ROLE_CLAIMS");

            migrationBuilder.DropTable(
                name: "SUPPORTING_DOCUMENTS");

            migrationBuilder.DropTable(
                name: "USER_CLAIMS");

            migrationBuilder.DropTable(
                name: "USER_LOGINS");

            migrationBuilder.DropTable(
                name: "USER_ROLES");

            migrationBuilder.DropTable(
                name: "USER_TOKENS");

            migrationBuilder.DropTable(
                name: "MONTHLY_CLAIMS");

            migrationBuilder.DropTable(
                name: "ROLES");

            migrationBuilder.DropTable(
                name: "USERS");

            migrationBuilder.DropTable(
                name: "ACADEMIC_MANAGERS");

            migrationBuilder.DropTable(
                name: "LECTURERS");

            migrationBuilder.DropTable(
                name: "PROGRAMME_COORDINATORS");
        }
    }
}
