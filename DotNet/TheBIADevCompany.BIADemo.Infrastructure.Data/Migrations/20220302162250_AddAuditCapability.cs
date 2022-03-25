using Microsoft.EntityFrameworkCore.Migrations;

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations
{
    public partial class AddAuditCapability : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE TABLE [dbo].[Events](
	                [Id] [int] IDENTITY(1,1) NOT NULL,
	                [JsonData] [nvarchar](max) NOT NULL,
	                [LastUpdatedDate] [datetime2](7) NOT NULL DEFAULT(GETUTCDATE()),
	                [EventType] [nvarchar](100) NOT NULL,
	                [UserId] [int] NOT NULL,
                 CONSTRAINT [PK_Events] PRIMARY KEY CLUSTERED 
                (
	                [Id] ASC
                )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
                GO

                ALTER TABLE [dbo].[Events] WITH CHECK ADD CONSTRAINT [FK_Events_Users] FOREIGN KEY([UserId])
                REFERENCES [dbo].[Users] ([Id])
                GO

                ALTER TABLE [dbo].[Events] CHECK CONSTRAINT [FK_Events_Users]
                GO
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Events]') AND type in (N'U'))
                DROP TABLE [dbo].[Events]
                GO
            ");
        }
    }
}
