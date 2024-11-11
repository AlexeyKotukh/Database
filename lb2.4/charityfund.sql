-- Create Database
CREATE DATABASE CharityFundDB;
GO

USE CharityFundDB;
GO

-- Create Entities

-- Donors Table
CREATE TABLE Donors (
    DonorID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    Phone NVARCHAR(15) NOT NULL
);

-- Projects Table
CREATE TABLE Projects (
    ProjectID INT PRIMARY KEY IDENTITY(1,1),
    ProjectName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255) NOT NULL,
    GoalAmount DECIMAL(18, 2) NOT NULL
);

-- Donations Table
CREATE TABLE Donations (
    DonationID INT PRIMARY KEY IDENTITY(1,1),
    DonorID INT FOREIGN KEY REFERENCES Donors(DonorID),
    ProjectID INT FOREIGN KEY REFERENCES Projects(ProjectID),
    Amount DECIMAL(18, 2) NOT NULL,
    DonationDate DATETIME NOT NULL DEFAULT GETDATE()
);

-- Volunteers Table
CREATE TABLE Volunteers (
    VolunteerID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    Phone NVARCHAR(15) NOT NULL
);

-- Volunteer Projects Table
CREATE TABLE VolunteerProjects (
    VolunteerProjectID INT PRIMARY KEY IDENTITY(1,1),
    VolunteerID INT FOREIGN KEY REFERENCES Volunteers(VolunteerID),
    ProjectID INT FOREIGN KEY REFERENCES Projects(ProjectID),
    HoursWorked INT NOT NULL
);

-- Insert Sample Data
INSERT INTO Donors (Name, Email, Phone) VALUES
('John Doe', 'john.doe@example.com', '123-456-7890'),
('Jane Smith', 'jane.smith@example.com', '987-654-3210');

INSERT INTO Projects (ProjectName, Description, GoalAmount) VALUES
('Help the Homeless', 'A project to provide shelter and food to the homeless.', 5000.00),
('Clean Water Initiative', 'Providing clean drinking water to rural areas.', 2000.00);

INSERT INTO Donations (DonorID, ProjectID, Amount) VALUES
(1, 1, 1000.00),
(2, 2, 500.00);

INSERT INTO Volunteers (Name, Email, Phone) VALUES
('Alice Johnson', 'alice.johnson@example.com', '555-123-4567'),
('Bob Brown', 'bob.brown@example.com', '555-765-4321');

INSERT INTO VolunteerProjects (VolunteerID, ProjectID, HoursWorked) VALUES
(1, 1, 10),
(2, 2, 5);
