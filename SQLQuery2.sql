DROP TABLE IF EXISTS UserRegister;

CREATE TABLE UserRegister
(
   UserId    VARCHAR(10) PRIMARY KEY,
   UserPw    VARBINARY(50) NOT NULL,
   FirstName  VARCHAR(50) NOT NULL,
   LastName   VARCHAR(50) NULL,
   Email     VARCHAR(50) NOT NULL,
   ContactNo VARCHAR(8) NOT NULL,
   StreetAddress VARCHAR(50) NOT NULL,
   UnitNo VARCHAR(10) NOT NULL,
   Postal VARCHAR(50) NOT NULL,
   Region VARCHAR(20) NOT NULL,
   UserRole  VARCHAR(10) NOT NULL,
   LastLogin DATETIME NULL
);

INSERT INTO UserRegister(UserId, UserPw, FirstName, LastName, Email, ContactNo, StreetAddress, UnitNo, Postal, Region, UserRole) VALUES
('riyas',  HASHBYTES('SHA1', '123456'),   'Mohamed', 'Riyas', '18037796@myrp.edu.sg', '84559484', 'Blk 461 Crawford Lane', '#12-75','190461', 'Central', 'manager');

