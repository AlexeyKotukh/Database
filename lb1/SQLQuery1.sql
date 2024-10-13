CREATE DATABASE Charity;
GO
USE Charity;
GO

CREATE TABLE Volunteers (
    VolunteerID INT PRIMARY KEY IDENTITY(1,1), 
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL
);


CREATE TABLE Donors (
    DonorID INT PRIMARY KEY IDENTITY(1,1), 
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL
);


CREATE TABLE Projects (
    ProjectID INT PRIMARY KEY IDENTITY(1,1), 
    ProjectName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255)
);


CREATE TABLE Donations (
    DonationID INT PRIMARY KEY IDENTITY(1,1), 
    DonorID INT NOT NULL, 
    ProjectID INT NOT NULL, 
    Amount DECIMAL(10, 2) NOT NULL, 
    DonationDate DATE NOT NULL, 
    FOREIGN KEY (DonorID) REFERENCES Donors(DonorID),
    FOREIGN KEY (ProjectID) REFERENCES Projects(ProjectID) 
);


CREATE TABLE VolunteerProjects (
    VolunteerID INT NOT NULL, 
    ProjectID INT NOT NULL,
    PRIMARY KEY (VolunteerID, ProjectID),
    FOREIGN KEY (VolunteerID) REFERENCES Volunteers(VolunteerID), 
    FOREIGN KEY (ProjectID) REFERENCES Projects(ProjectID)
);

INSERT INTO Volunteers (FirstName, LastName, Email) VALUES
('Ivan', 'Ivanov', 'ivanov@example.com'),
('Petro', 'Petrenko', 'petrenko@example.com');


INSERT INTO Donors (FirstName, LastName, Email) VALUES
('Anna', 'Shevchenko', 'shevchenko@example.com'),
('Dmytro', 'Koval', 'koval@example.com');


INSERT INTO Projects (ProjectName, Description) VALUES
('Food Distribution', 'Distribution of food for refugees'),
('Medical Supplies', 'Provision of medical supplies to hospitals');


INSERT INTO Donations (DonorID, ProjectID, Amount, DonationDate) VALUES
(1, 1, 100.50, '2024-09-25'),
(2, 2, 250.00, '2024-09-27');


INSERT INTO VolunteerProjects (VolunteerID, ProjectID) VALUES
(1, 1),
(2, 2);
