-- Add Users table
CREATE TABLE [Users] (
    [Id] uniqueidentifier NOT NULL PRIMARY KEY DEFAULT NEWID(),
    [Username] nvarchar(100) NOT NULL UNIQUE,
    [PasswordHash] nvarchar(500) NOT NULL,
    [FullName] nvarchar(200) NOT NULL,
    [Email] nvarchar(200) NOT NULL,
    [PhoneNumber] nvarchar(20) NULL,
    [Role] nvarchar(50) NOT NULL DEFAULT 'Staff',
    [IsActive] bit NOT NULL DEFAULT 1,
    [CreatedAt] datetime2 NOT NULL DEFAULT GETUTCDATE(),
    [LastLoginAt] datetime2 NULL,
    [RefreshToken] nvarchar(500) NULL,
    [RefreshTokenExpiryTime] datetime2 NULL
);

-- Create index for faster lookups
CREATE INDEX [IX_Users_Username] ON [Users] ([Username]);
CREATE INDEX [IX_Users_Email] ON [Users] ([Email]);

-- Seed default users
-- Password for all: Admin@123
INSERT INTO [Users] ([Id], [Username], [PasswordHash], [FullName], [Email], [Role], [IsActive], [CreatedAt])
VALUES 
    (NEWID(), 'admin', 'kMl0qLF8qQWzF4VZZ7jQxw==:yH8wjvKGxMfnfZ+QxNQVQTOJPp9J3DQk8XLFYPg5xKo=', N'Quản trị viên', 'admin@yakihouse.com', 'Admin', 1, GETUTCDATE()),
    (NEWID(), 'manager', 'kMl0qLF8qQWzF4VZZ7jQxw==:yH8wjvKGxMfnfZ+QxNQVQTOJPp9J3DQk8XLFYPg5xKo=', N'Quản lý', 'manager@yakihouse.com', 'Manager', 1, GETUTCDATE()),
    (NEWID(), 'staff1', 'kMl0qLF8qQWzF4VZZ7jQxw==:yH8wjvKGxMfnfZ+QxNQVQTOJPp9J3DQk8XLFYPg5xKo=', N'Nhân viên 1', 'staff1@yakihouse.com', 'Staff', 1, GETUTCDATE()),
    (NEWID(), 'kitchen1', 'kMl0qLF8qQWzF4VZZ7jQxw==:yH8wjvKGxMfnfZ+QxNQVQTOJPp9J3DQk8XLFYPg5xKo=', N'Bếp trưởng', 'kitchen1@yakihouse.com', 'Kitchen', 1, GETUTCDATE());

GO
