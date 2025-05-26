DROP TABLE IF EXISTS Partners;
DROP TABLE IF EXISTS EventBookings;
DROP TABLE IF EXISTS TeamBookings;
DROP TABLE IF EXISTS CourtBookings;
DROP TABLE IF EXISTS Teams;
DROP TABLE IF EXISTS Trainers;
DROP TABLE IF EXISTS Announcements;
DROP TABLE IF EXISTS Activities;
DROP TABLE IF EXISTS Events;
DROP TABLE IF EXISTS Courts;
DROP TABLE IF EXISTS Members;

CREATE TABLE Members (
	ID int identity(1, 1) UNIQUE,
	Name varchar(256) not null,
	PassWord varchar(256) not null,
	Address varchar(256) not null,
	Email varchar(256) UNIQUE not null,
	Phone varchar(16) not null,
	Sex varchar(8) not null,
	DoB Date not null,
	Bio varchar(1024) not null,
	IsAdmin bit not null,
	Image varchar(256),

	CONSTRAINT sexOptions
	CHECK(Sex IN ('Herre', 'Dame', 'Andet')),

	PRIMARY KEY (Email, PassWord)
);

CREATE TABLE Courts (
	ID int identity(1, 1) PRIMARY KEY not null,
	Type varchar(32) not null,
    Name varchar(32) not null
);

CREATE TABLE Events (
	ID int identity(1, 1) PRIMARY KEY not null,
    Title varchar(64) not null,
    Description varchar(1024) not null,
	StartTime DATETIME not null,
	EndTime TIME not null,
	Location varchar(64) not null,
	Capacity int
);

CREATE TABLE Activities (
	ID int identity(1, 1) PRIMARY KEY not null,
	EventID int not null,
	Description varchar(512) not null,
	Title varchar(32) not null,
	StartTime DATETIME not null,
	EndTime TIME not null,

	FOREIGN KEY (EventID) REFERENCES Events(ID) ON DELETE CASCADE,
);

CREATE TABLE Announcements (
    ID int identity(1, 1) PRIMARY KEY NOT NULL,
    MemberID int NOT NULL,
    Title varchar(32) NOT NULL,
    Text varchar(1024) NOT NULL,
    Upload DATE NOT NULL,
    Type varchar(16) NOT NULL,
    Actual BIT NULL,

    CONSTRAINT typeConstraint CHECK (Type IN ('Service', 'Partner', 'General')),
    CONSTRAINT CK_Announcements_Actual_OnlyForService
        CHECK (
            Actual IS NULL OR Type = 'Service'
        ),

    FOREIGN KEY (MemberID) REFERENCES Members(ID) ON DELETE CASCADE
);

CREATE TABLE Trainers (
	ID int identity(1, 1) PRIMARY KEY not null,
	Name varchar(64) not null,
	Phone varchar(16) not null,
	Email varchar(64) not null,
	Image varchar(64)
);

CREATE TABLE Teams (
	ID int identity(1, 1) PRIMARY KEY not null,
	Name varchar(32) not null,
    Description varchar(256) not null,
	TrainerID int not null,
	Capacity int not null,
	ActiveDay varchar(8) not null,
    Price int not null,

    CONSTRAINT DayOptions
    CHECK(
		ActiveDay IN (
            'Mandag', 'Tirsdag', 'Onsdag', 'Torsdag', 'Fredag', 'Lørdag', 'Søndag'
        )
	),

	FOREIGN KEY (TrainerID) REFERENCES Trainers(ID) ON DELETE CASCADE
);

CREATE TABLE CourtBookings (
	ID int identity(1, 1) not null UNIQUE,
	BookingDate Date not null,
	CourtID int not null,
	TimeSlot int not null,
	TeamID int,
	MemberID int,
	EventID int,

	FOREIGN KEY (CourtID) REFERENCES Courts(ID) ON DELETE CASCADE,
	FOREIGN KEY (TeamID) REFERENCES Teams(ID) ON DELETE CASCADE,
	FOREIGN KEY (MemberID) REFERENCES Members(ID) ON DELETE CASCADE,
	FOREIGN KEY (EventID) REFERENCES Events(ID) ON DELETE CASCADE,

	CONSTRAINT idBooking
	CHECK(
	(TeamID IS not null AND MemberID is null AND EventID is null) OR
	(MemberID IS not null AND TeamID IS null AND EventID is null) OR
	(EventID IS not null AND TeamID IS null AND MemberID is null)
	),

	PRIMARY KEY (CourtID, TimeSlot, BookingDate)
);

CREATE TABLE TeamBookings (
	ID int identity(1, 1) UNIQUE not null,
	MemberID int not null,
	TeamID int not null,

	FOREIGN KEY (MemberID) REFERENCES Members(ID) ON DELETE CASCADE,
	FOREIGN KEY (TeamID) REFERENCES Teams(ID) ON DELETE CASCADE,

	PRIMARY KEY (MemberID, TeamID)
);

CREATE TABLE EventBookings (
	ID int identity(1, 1) UNIQUE not null,
	MemberID int not null,
	EventID int not null,

	FOREIGN KEY (MemberID) REFERENCES Members(ID) ON DELETE CASCADE,
	FOREIGN KEY (EventID) REFERENCES Events(ID) ON DELETE CASCADE,

	PRIMARY KEY (MemberID, EventID)
);

CREATE TABLE Partners (
	MemberID int not null,
	BookingID int not null,

	FOREIGN KEY (MemberID) REFERENCES Members(ID),
	FOREIGN KEY (BookingID) REFERENCES CourtBookings(ID) ON DELETE CASCADE,

	PRIMARY KEY (MemberID, BookingID)
);

INSERT INTO Members
VALUES ('Børge', 'Pass123', 'VesterVej 40', 'Børge@DanskMail.dk', '+45 50 50 50 50', 'Herre', '1980-04-29', 'Jeg er Børge fra Vestervej 40, Jeg har ikke spillet tennis før', 0, 'person4.jpg');
INSERT INTO Members
VALUES ('Bendte', '2B', 'Mestervej 32', 'Bendte@Mail.dk', '+45 29 75 10 76', 'Dame', '1962-04-28', 'Danmarks Mester 1983', 0, 'person5.jpg');
INSERT INTO Members
VALUES ('Janne', '123456789', 'Odinsvej 4', 'i@i.dk', '+45 12 36 78 90', 'Andet', '1961-01-01', 'Hvem tror du jeg er?', 1, 'person6.jpg');
INSERT INTO Members
VALUES ('Johnson', 'Humaniora', 'Gydevej 34', 'Johnjohn@mail.dk', '+45 25 75 43 22', 'Herre', '1995-08-02', 'Jeg kommer fra gyden', 1, 'person8.jpg');
INSERT INTO Members
VALUES ('Kurt', 'Klud','Midgaardsvej 22', 'kurtsklud@gg.dk','+45 12 46 18 35', 'Andet', '1972-05-03', 'Du kender mig', 0, 'person7.jpg')

INSERT INTO Courts
VALUES ('Udendørs Tennis', 'Bane 1')
INSERT INTO Courts
VALUES ('Udendørs Tennis', 'Bane 2')
INSERT INTO Courts
VALUES ('Udendørs Tennis', 'Bane 3')
INSERT INTO Courts
VALUES ('Udendørs Tennis', 'Bane 4')
INSERT INTO Courts
VALUES ('Udendørs Tennis', 'Bane 5')
INSERT INTO Courts
VALUES ('Indendørs Tennis', '')
INSERT INTO Courts
VALUES ('Indendørs Tennis', 'Vores 2. Indendørs Tennis Bane')
INSERT INTO Courts
VALUES ('Indendørs Tennis', 'Den 3. Indendørs Tennis Bane')
INSERT INTO Courts
VALUES ('Udendørs Padel', '')

INSERT INTO Events
VALUES ('Standerhejsning og klargøring af banerne', 'Standerhejsning og klargøring af banerne
Vejrguderne og nye lollandske banefolk har været med os i år. Banerne bliver dermed gjort klar til sæsonens åbning allerede nu på lørdag den 24. maj.
Vi mødes kl. 10 til den sidste klargøring og alle er velkomne til at give en hånd med.
Den officielle standerhejsning er kl. 12 med muligvis rørstrømsk tale af formanden samt hurtige kulhydrater og lidt til slukke tørsten med.
Så find tennisgrejet frem og kig forbi. Vi glæder os til at indånde det røde grus og håber at se jer til at skyde sæsonen igang.
Der er fortsat plads på nogle af vores træningshold!', '2025-05-24 10:00:00', '12:00:00', 'Gadevangsvej 145B - 3400 Hillerød', 70)
INSERT INTO Events
VALUES ('Grill og Tennis', '', '2025-06-15 16:00:00', '22:00:00', 'Gadevangsvej 145B - 3400 Hillerød', 70)
INSERT INTO Events
VALUES ('Introdag', 'Introdag, hvor alle nye medlemmer kan blive introduceret til sporten', '2025-06-03 10:00:00', '16:00:00', 'Gadevangsvej 145B - 3400 Hillerød', 40)
INSERT INTO Events
VALUES ('Klubmesterskab', 'Sæsonen afsluttes som sædvanlig med vores årlige turnering, hvor der spilles om klubmesterskabet i rækkerne A, B og C niveauer', '2025-06-26 09:00:00', '17:00:00', 'Gadevangsvej 145B - 3400 Hillerød', 100)
INSERT INTO Events
VALUES ('Arbejdsdag', 'Gadevang Tennisklub er et lokalt forankret idrætsklub, der drives 100 % af frivillige kræfter. Derfor holder vi nu vores første af vores to årlige arbejdsdage, hvor alle kan komme og bidrage til at holde klubben vedlige og fortsætte med at gøre den til et rart og ryddeligt sted for alle vores medlemmer', '2025-06-20 10:00:00', '14:00:00', 'Gadevangsvej 145B - 3400 Hillerød', 100)
INSERT INTO Events
VALUES ('Generalforsamling 2025', 'Dagsorden (jf. vedtægterne)
1. Valg af dirigent
2. Formandens beretning
3. Fremlæggelse af det reviderede regnskab til godkendelse
4. Godkendelse af budget og kontingent
5. Valg af bestyrelsesmedlemmer 
6. Valg af bestyrelsessuppleant
7. Valg af 2 revisorer og en revisorsuppleant
8. Forslag fra medlemmerne.
	- Sådanne forslag skal fremsættes skriftligt til formanden senest 10 dage før generalforsamlingen
9. Forslag fra bestyrelsen
10. Eventuelt    
Håber vi ses til en hyggelig aften.
De bedste hilsner,
Bestyrelsen i Gadevang Tennisklub', '2025-06-18 20:00:00', '21:30:00', 'Gadevangsvej 145B - 3400 Hillerød', null)
INSERT INTO Events
VALUES ('Store Flødebolle Dag', '', '2025-06-18 10:00:00', '14:00:00', 'Klubhuset', '100')

INSERT INTO Activities
VALUES(1, '', 'Klargøring af banerne', '2025-05-24 10:00:00', '2025-05-24 11:00:00')
INSERT INTO Activities
VALUES(1, '', 'Standerhejsning', '2025-05-24 11:00:00', '2025-05-24 12:00:00')
INSERT INTO Activities
VALUES(4, '', 'Dame Double', '2025-06-26 09:00:00', '2025-06-26 11:00:00')
INSERT INTO Activities
VALUES(4, '', 'Dame Single', '2025-06-26 11:00:00', '2025-06-26 13:00:00')
INSERT INTO Activities
VALUES(4, '', 'Herre Double', '2025-06-26 13:00:00', '2025-06-26 15:00:00')
INSERT INTO Activities
VALUES(4, '', 'Herre Single', '2025-06-26 15:00:00', '2025-06-26 17:00:00')
INSERT INTO Activities
VALUES(7, 'Alt om flødebollers oprindelse og om hvorfor det er så vigtigt at indtage dem', 'Foredrag og flødeboller', '2025-06-18 10:00:00', '2025-06-18 11:00:00')
INSERT INTO Activities
VALUES(7, 'Der skal spises flødeboller', 'Flødebolleædning', '2025-06-18 11:00:00', '2025-06-18 14:00:00')

INSERT INTO Announcements
VALUES (1, 'Søger Partner', 'Vil gerne spille noget tennis på tirsdag, er der nogen der er interesseret?', '2024-01-03', 'Partner',null)
INSERT INTO Announcements
VALUES (3, 'Help', 'Jeg er ikke sikker på hvad jeg skal gøre.', '2001-01-02', 'General', null)
INSERT INTO Announcements
VALUES (1, '???', 'Mortalis es fatum. Nalyë, caurelye istya.', '2025-04-29', 'General', null)
INSERT INTO Announcements
VALUES (3, 'Måger', 'Der er måger der angriber alt og alle på bane 2', '2024-08-19', 'Service', 0)
INSERT INTO Announcements
VALUES (4, 'Boldmaskine', 'Boldmaskinen er ude af drift pg.a nogen fyldte den med æbler', '2025-05-12', 'Service', 1)
INSERT INTO Announcements
VALUES (2, 'Skur', 'Døren til skuret binder fordi der er nogen som super limede den.', '2025-05-14', 'Service', 1)
INSERT INTO Announcements
VALUES (5, 'Brug for spiller tirsdag', 'Mangler partner til at spille tennis om tirsdagen klokken 18', '2025-05-24', 'Service', null)

INSERT INTO Trainers
VALUES ('Kenneth', '+45 12 33 21 65', 'Kenneth@mail.dk', 'person1.jpg')
INSERT INTO Trainers
VALUES ('Smith', '+45 56 56 56 56', 'Smith@mail.dk', 'person2.jpg')
INSERT INTO Trainers
VALUES ('Daffy', '+45 66 55 54 52', 'Daffy@mail.us', 'person3.jpg')


INSERT INTO Teams
VALUES ('Turnering Træning', 'Træning til opkommende turnering', 1, 3, 'Fredag', 1750)
INSERT INTO Teams
VALUES ('Mini', 'Minitræning for børn i alderen 5-7 år', 3, 8, 'Onsdag', 450)
INSERT INTO Teams
VALUES ('Junior 1', 'Junior 10-14 år', 3, 8, 'Torsdag', 550)
INSERT INTO Teams
VALUES ('Junior 2', 'Junior 10-14 år', 1, 8, 'Torsdag', 550)
INSERT INTO Teams
VALUES ('Senior let øvet', 'Seniortræning for let øvede spillere', 1, 8, 'Tirsdag', 750)
INSERT INTO Teams
VALUES ('Senior øvet', 'Seniortræning for øvede spillere', 2, 8, 'Mandag', 750)
INSERT INTO Teams
VALUES ('Senior rutineret', 'Seniortræning for rutinerede spillere', 2, 5, 'Mandag', 750)

INSERT INTO CourtBookings
VALUES ('2025-06-15', 2, 1, null, 2, null)
INSERT INTO CourtBookings
VALUES ('2025-07-22', 2, 3, null, 1, null)
INSERT INTO CourtBookings
VALUES ('2025-06-11', 1, 4, null, 3, null)
INSERT INTO CourtBookings
VALUES ('2025-05-21', 1, 9, 2, null, null)
INSERT INTO CourtBookings
VALUES ('2025-08-06', 3, 1, 3, null, null)
INSERT INTO CourtBookings
VALUES ('2025-06-13', 2, 8, 1, null, null)
INSERT INTO CourtBookings
VALUES ('2025-07-8', 3, 3, null, null, 1)
INSERT INTO CourtBookings
VALUES ('2025-05-25', 1, 11, null, null, 1)
INSERT INTO CourtBookings
VALUES ('2025-08-19', 2, 4, null, null, 2)

INSERT INTO TeamBookings
VALUES (1, 5)
INSERT INTO TeamBookings
VALUES (4, 5)
INSERT INTO TeamBookings
VALUES (3, 7)
INSERT INTO TeamBookings
VALUES (4, 1)
INSERT INTO TeamBookings
VALUES (5, 1)
INSERT INTO TeamBookings
VALUES (1, 1)
INSERT INTO TeamBookings
VALUES (5, 7)
INSERT INTO TeamBookings
VALUES (4, 7)
INSERT INTO TeamBookings
VALUES (3, 5)
INSERT INTO TeamBookings
VALUES (3, 6)

INSERT INTO EventBookings
VALUES (1, 1)
INSERT INTO EventBookings
VALUES (2, 3)
INSERT INTO EventBookings
VALUES (4, 3)
INSERT INTO EventBookings
VALUES (1, 5)
INSERT INTO EventBookings
VALUES (2, 7)
INSERT INTO EventBookings
VALUES (1, 7)
INSERT INTO EventBookings
VALUES (4, 7)

INSERT INTO Partners
VALUES (1, 3)
INSERT INTO Partners
VALUES (1, 1)
INSERT INTO Partners
VALUES (3, 1)
INSERT INTO Partners
VALUES (4, 1)
INSERT INTO Partners
VALUES (3, 2)
