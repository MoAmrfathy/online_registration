using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    D_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    D_name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.D_id);
                });

            migrationBuilder.CreateTable(
                name: "GraduationPlans",
                columns: table => new
                {
                    GraduationPlanId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    D_id = table.Column<int>(type: "int", nullable: false),
                    RequiredCredits = table.Column<int>(type: "int", nullable: false),
                    g_name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GraduationPlans", x => x.GraduationPlanId);
                    table.ForeignKey(
                        name: "FK_GraduationPlans_Departments_D_id",
                        column: x => x.D_id,
                        principalTable: "Departments",
                        principalColumn: "D_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    S_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    S_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GPA = table.Column<double>(type: "float", nullable: false),
                    TotalCreditAchievement = table.Column<int>(type: "int", nullable: false),
                    IsGraduate = table.Column<bool>(type: "bit", nullable: false),
                    College = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Semester = table.Column<int>(type: "int", nullable: false),
                    Reg_no = table.Column<long>(type: "bigint", nullable: false),
                    PIN = table.Column<long>(type: "bigint", nullable: false),
                    isfired = table.Column<bool>(type: "bit", nullable: false),
                    D_id = table.Column<int>(type: "int", nullable: false),
                    GraduationPlanId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.S_id);
                    table.ForeignKey(
                        name: "FK_Students_Departments_D_id",
                        column: x => x.D_id,
                        principalTable: "Departments",
                        principalColumn: "D_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Students_GraduationPlans_GraduationPlanId",
                        column: x => x.GraduationPlanId,
                        principalTable: "GraduationPlans",
                        principalColumn: "GraduationPlanId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Terms",
                columns: table => new
                {
                    TermId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Term_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GraduationPlanId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Terms", x => x.TermId);
                    table.ForeignKey(
                        name: "FK_Terms_GraduationPlans_GraduationPlanId",
                        column: x => x.GraduationPlanId,
                        principalTable: "GraduationPlans",
                        principalColumn: "GraduationPlanId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    C_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    C_code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    C_Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Credits = table.Column<int>(type: "int", nullable: false),
                    D_id = table.Column<int>(type: "int", nullable: false),
                    TermId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.C_id);
                    table.ForeignKey(
                        name: "FK_Courses_Departments_D_id",
                        column: x => x.D_id,
                        principalTable: "Departments",
                        principalColumn: "D_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Courses_Terms_TermId",
                        column: x => x.TermId,
                        principalTable: "Terms",
                        principalColumn: "TermId");
                });

            migrationBuilder.CreateTable(
                name: "Lectures",
                columns: table => new
                {
                    LectureId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LectureName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Day = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LectureStartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    LectureEndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    C_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lectures", x => x.LectureId);
                    table.ForeignKey(
                        name: "FK_Lectures_Courses_C_id",
                        column: x => x.C_id,
                        principalTable: "Courses",
                        principalColumn: "C_id");
                });

            migrationBuilder.CreateTable(
                name: "Prerequisites",
                columns: table => new
                {
                    PrerequisiteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    C_id = table.Column<int>(type: "int", nullable: false),
                    RequiredCourseId = table.Column<int>(type: "int", nullable: true),
                    MinimumGPA = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prerequisites", x => x.PrerequisiteId);
                    table.ForeignKey(
                        name: "FK_Prerequisites_Courses_C_id",
                        column: x => x.C_id,
                        principalTable: "Courses",
                        principalColumn: "C_id");
                    table.ForeignKey(
                        name: "FK_Prerequisites_Courses_RequiredCourseId",
                        column: x => x.RequiredCourseId,
                        principalTable: "Courses",
                        principalColumn: "C_id");
                });

            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    SectionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SectionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Day = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sectionStartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    sectionEndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    C_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.SectionId);
                    table.ForeignKey(
                        name: "FK_Sections_Courses_C_id",
                        column: x => x.C_id,
                        principalTable: "Courses",
                        principalColumn: "C_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Waitlist",
                columns: table => new
                {
                    WaitlistId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    S_id = table.Column<int>(type: "int", nullable: false),
                    C_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Waitlist", x => x.WaitlistId);
                    table.ForeignKey(
                        name: "FK_Waitlist_Courses_C_id",
                        column: x => x.C_id,
                        principalTable: "Courses",
                        principalColumn: "C_id");
                    table.ForeignKey(
                        name: "FK_Waitlist_Students_S_id",
                        column: x => x.S_id,
                        principalTable: "Students",
                        principalColumn: "S_id");
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    GroupId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    MaxCapacity = table.Column<int>(type: "int", nullable: false),
                    LectureId = table.Column<int>(type: "int", nullable: false),
                    SectionId = table.Column<int>(type: "int", nullable: false),
                    C_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.GroupId);
                    table.ForeignKey(
                        name: "FK_Groups_Courses_C_id",
                        column: x => x.C_id,
                        principalTable: "Courses",
                        principalColumn: "C_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Groups_Lectures_LectureId",
                        column: x => x.LectureId,
                        principalTable: "Lectures",
                        principalColumn: "LectureId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Groups_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "SectionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Enrollments",
                columns: table => new
                {
                    EnrollmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    S_id = table.Column<int>(type: "int", nullable: false),
                    C_id = table.Column<int>(type: "int", nullable: false),
                    GraduationPlanId = table.Column<int>(type: "int", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    CourseC_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrollments", x => x.EnrollmentId);
                    table.ForeignKey(
                        name: "FK_Enrollments_Courses_C_id",
                        column: x => x.C_id,
                        principalTable: "Courses",
                        principalColumn: "C_id");
                    table.ForeignKey(
                        name: "FK_Enrollments_Courses_CourseC_id",
                        column: x => x.CourseC_id,
                        principalTable: "Courses",
                        principalColumn: "C_id");
                    table.ForeignKey(
                        name: "FK_Enrollments_GraduationPlans_GraduationPlanId",
                        column: x => x.GraduationPlanId,
                        principalTable: "GraduationPlans",
                        principalColumn: "GraduationPlanId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Enrollments_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Enrollments_Students_S_id",
                        column: x => x.S_id,
                        principalTable: "Students",
                        principalColumn: "S_id");
                });

            migrationBuilder.CreateTable(
                name: "SelectedCourses",
                columns: table => new
                {
                    SelectedCourseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    C_id = table.Column<int>(type: "int", nullable: false),
                    S_id = table.Column<int>(type: "int", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    SelectionDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelectedCourses", x => x.SelectedCourseId);
                    table.ForeignKey(
                        name: "FK_SelectedCourses_Courses_C_id",
                        column: x => x.C_id,
                        principalTable: "Courses",
                        principalColumn: "C_id");
                    table.ForeignKey(
                        name: "FK_SelectedCourses_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SelectedCourses_Students_S_id",
                        column: x => x.S_id,
                        principalTable: "Students",
                        principalColumn: "S_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_D_id",
                table: "Courses",
                column: "D_id");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_TermId",
                table: "Courses",
                column: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_C_id",
                table: "Enrollments",
                column: "C_id");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_CourseC_id",
                table: "Enrollments",
                column: "CourseC_id");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_GraduationPlanId",
                table: "Enrollments",
                column: "GraduationPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_GroupId",
                table: "Enrollments",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_S_id",
                table: "Enrollments",
                column: "S_id");

            migrationBuilder.CreateIndex(
                name: "IX_GraduationPlans_D_id",
                table: "GraduationPlans",
                column: "D_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_C_id",
                table: "Groups",
                column: "C_id");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_LectureId",
                table: "Groups",
                column: "LectureId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_SectionId",
                table: "Groups",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Lectures_C_id",
                table: "Lectures",
                column: "C_id");

            migrationBuilder.CreateIndex(
                name: "IX_Prerequisites_C_id",
                table: "Prerequisites",
                column: "C_id");

            migrationBuilder.CreateIndex(
                name: "IX_Prerequisites_RequiredCourseId",
                table: "Prerequisites",
                column: "RequiredCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_C_id",
                table: "Sections",
                column: "C_id");

            migrationBuilder.CreateIndex(
                name: "IX_SelectedCourses_C_id",
                table: "SelectedCourses",
                column: "C_id");

            migrationBuilder.CreateIndex(
                name: "IX_SelectedCourses_GroupId",
                table: "SelectedCourses",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_SelectedCourses_S_id",
                table: "SelectedCourses",
                column: "S_id");

            migrationBuilder.CreateIndex(
                name: "IX_Students_D_id",
                table: "Students",
                column: "D_id");

            migrationBuilder.CreateIndex(
                name: "IX_Students_GraduationPlanId",
                table: "Students",
                column: "GraduationPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Terms_GraduationPlanId",
                table: "Terms",
                column: "GraduationPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Waitlist_C_id",
                table: "Waitlist",
                column: "C_id");

            migrationBuilder.CreateIndex(
                name: "IX_Waitlist_S_id",
                table: "Waitlist",
                column: "S_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Enrollments");

            migrationBuilder.DropTable(
                name: "Prerequisites");

            migrationBuilder.DropTable(
                name: "SelectedCourses");

            migrationBuilder.DropTable(
                name: "Waitlist");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Lectures");

            migrationBuilder.DropTable(
                name: "Sections");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Terms");

            migrationBuilder.DropTable(
                name: "GraduationPlans");

            migrationBuilder.DropTable(
                name: "Departments");
        }
    }
}
