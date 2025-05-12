DROP TABLE Partners;
DROP TABLE CourtBookings;
DROP TABLE TeamBookings;
DROP TABLE EventBookings;
DROP TABLE Teams;
DROP TABLE Trainers;
DROP TABLE Announcements;
DROP TABLE Members;
DROP TABLE Courts;
DROP TABLE Activities;
DROP TABLE Events;

CREATE TABLE Members (
	ID int identity(0, 1) UNIQUE,
	Name varchar(256) not null,
	PassWord varchar(256) not null,
	Address varchar(256) not null,
	Email varchar(256) not null,
	Phone varchar(16) not null,
	Sex varchar(8) not null,
	DoB Date not null,
	Bio varchar(1024) not null,
	IsAdmin bit not null,
	Image varchar(64),

	CONSTRAINT sexOptions
	CHECK(Sex IN ('Herre', 'Dame', 'Andet')),

	PRIMARY KEY (Email, PassWord)
);

CREATE TABLE Courts (
	ID int identity(0, 1) PRIMARY KEY not null,
	Type varchar(32) not null,
    Name varchar(32) 
);

CREATE TABLE Events (
	ID int identity(0, 1) PRIMARY KEY not null,
    Title varchar(64) not null,
    Description varchar(1024) not null,
	StartTime DATETIME not null,
	EndTime TIME not null,
	Location varchar(64) not null,
	Capacity int
);

CREATE TABLE Activities (
	ID int identity(0, 1) PRIMARY KEY not null,
	EventID int not null,
	Description varchar(512) not null,
	Title varchar(32) not null,
	StartTime DATE not null,
	EndTime DATE not null,

	FOREIGN KEY (EventID) REFERENCES Events(ID)
);

CREATE TABLE Announcements (
	ID int identity(0, 1) PRIMARY KEY not null,
	MemberID int not null,
	Title varchar(32) not null,
	Text varchar(1024) not null,
	Upload DATE not null,
	Type varchar(16) not null,

	CONSTRAINT typeConstraint
	CHECK(Type in ('Service','Partner','General')),

	FOREIGN KEY (MemberID) REFERENCES Members(ID)
);

CREATE TABLE Trainers (
	ID int identity(0, 1) PRIMARY KEY not null,
	Name varchar(64) not null,
	Phone varchar(16) not null,
	Email varchar(64) not null,
	Image varchar(64)
);

CREATE TABLE Teams (
	ID int identity(0, 1) PRIMARY KEY not null,
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

	FOREIGN KEY (TrainerID) REFERENCES Trainers(ID)
);

CREATE TABLE CourtBookings (
	ID int identity(0, 1) not null UNIQUE,
	BookingDate Date not null,
	CourtID int not null,
	TimeSlot int not null,
	TeamID int,
	MemberID int,
	EventID int,

	FOREIGN KEY (CourtID) REFERENCES Courts(ID),
	FOREIGN KEY (TeamID) REFERENCES Teams(ID),
	FOREIGN KEY (MemberID) REFERENCES Members(ID),
	FOREIGN KEY (EventID) REFERENCES Events(ID),

	CONSTRAINT idBooking
	CHECK(
	(TeamID IS not null AND MemberID is null AND EventID is null) OR
	(MemberID IS not null AND TeamID IS null AND EventID is null) OR
	(EventID IS not null AND TeamID IS null AND MemberID is null)
	),

	PRIMARY KEY (CourtID, TimeSlot, BookingDate)
);

CREATE TABLE TeamBookings (
	ID int identity(0, 1) not null,
	MemberID int not null,
	TeamID int not null,

	FOREIGN KEY (MemberID) REFERENCES Members(ID),
	FOREIGN KEY (TeamID) REFERENCES Teams(ID),

	PRIMARY KEY (MemberID, TeamID)
);

CREATE TABLE EventBookings (
	ID int identity(0, 1) not null,
	MemberID int not null,
	EventID int not null,

	FOREIGN KEY (MemberID) REFERENCES Members(ID),
	FOREIGN KEY (EventID) REFERENCES Events(ID),

	PRIMARY KEY (MemberID, EventID)
);

CREATE TABLE Partners (
	MemberID int not null,
	BookingID int not null,

	FOREIGN KEY (MemberID) REFERENCES Members(ID),
	FOREIGN KEY (BookingID) REFERENCES CourtBookings(ID),

	PRIMARY KEY (MemberID, BookingID)
);

INSERT INTO Members
VALUES ('Børge', 'Pass123', 'VesterVej 40', 'Børge@DanskMail.dk', '50505050', 'Herre', '1980-04-29', 'Jeg er Børge fra Vestervej 40, Jeg har ikke spillet tennis før', 0, null);
INSERT INTO Members
VALUES ('Bendte', '2B', 'Mestervej 32', 'Bendte@Mail.dk', '29751076', 'Dame', '1962-04-28', 'Danmarks Mester 1983', 0, null);
INSERT INTO Members
VALUES ('Janne', '123456789', 'Her og der', 'i@i.dk', '+45 12 36 78 90', 'Andet', '0001-01-01', 'Hvem tror du jeg er?', 1, null);
INSERT INTO Members
VALUES ('Johnson', 'Humaniora', 'Gyden', 'Johnjohn@mail.dk', '+45 25 75 43 22', 'Herre', '1204-08-02', 'Jeg kommer fra gyden', 1, null);

INSERT INTO Courts
VALUES ('Udendørs Tennis', 'Bane 1')
INSERT INTO Courts
VALUES ('Udendørs Tennis', 'Bane 2')
INSERT INTO Courts
VALUES ('Indendørs Tennis', '')
INSERT INTO Courts
VALUES ('Indendørs Tennis', 'Vores 2. IndendørsTennisBane')
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
VALUES(0, '', 'Klargøring af banerne', '2025-05-24 10:00:00', '2025-05-24 11:00:00')
INSERT INTO Activities
VALUES(0, '', 'Standerhejsning', '2025-05-24 11:00:00', '2025-05-24 12:00:00')
INSERT INTO Activities
VALUES(3, '', 'Dame Double', '2025-06-26 09:00:00', '2025-06-26 11:00:00')
INSERT INTO Activities
VALUES(3, '', 'Dame Single', '2025-06-26 11:00:00', '2025-06-26 13:00:00')
INSERT INTO Activities
VALUES(3, '', 'Herre Double', '2025-06-26 13:00:00', '2025-06-26 15:00:00')
INSERT INTO Activities
VALUES(3, '', 'Herre Single', '2025-06-26 15:00:00', '2025-06-26 17:00:00')
INSERT INTO Activities
VALUES(6, 'Alt om flødebollers oprindelse og om hvorfor det er så vigtigt at indtage dem', 'Foredrag og flødeboller', '2025-06-18 10:00:00', '2025-06-18 11:00:00')
INSERT INTO Activities
VALUES(6, 'Der skal spises flødeboller', 'Flødebolleædning', '2025-06-18 11:00:00', '2025-06-18 14:00:00')

INSERT INTO Announcements
VALUES (0, 'Søger Partner', 'Vil gerne spille noget tennis på tirsdag, er der nogen der er interesseret?', '2024-01-03', 'Partner')
INSERT INTO Announcements
VALUES (2, 'Help', 'Jeg er ikke sikker på hvad jeg skal gøre.', '0001-01-02', 'General')
INSERT INTO Announcements
VALUES (2, '???', 'Mortalis es fatum. Nalyë, caurelye istya.', '2025-04-29', 'General')

INSERT INTO Trainers
VALUES ('Kenneth', '12332165', 'Kenneth@mail.dk', null)
INSERT INTO Trainers
VALUES ('Smith', '+45 56 56 56 56', 'Smith@mail.dk', null)
INSERT INTO Trainers
VALUES ('Daffy', '+45 56 56 56 56', 'Daffy@mail.us', null)


INSERT INTO Teams
VALUES ('test', 'test', 1, 2, 'Fredag', 1750)
INSERT INTO Teams
VALUES ('Mini', 'Minitræning for børn i alderen 5-7 år', 1, 8, 'Onsdag', 450)
INSERT INTO Teams
VALUES ('Junior 1', 'Junior 10-14 år', 1, 8, 'Torsdag', 550)
INSERT INTO Teams
VALUES ('Junior 2', 'Junior 10-14 år', 1, 8, 'Torsdag', 550)
INSERT INTO Teams
VALUES ('Senior let øvet', 'Seniortræning for let øvede spillere', 1, 8, 'Tirsdag', 750)
INSERT INTO Teams
VALUES ('Senior øvet', 'Seniortræning for øvede spillere', 2, 8, 'Mandag', 750)
INSERT INTO Teams
VALUES ('Senior rutineret', 'Seniortræning for rutinerede spillere', 2, 5, 'Mandag', 750)

INSERT INTO CourtBookings
VALUES ('2025-06-15', 1, 1, null, 1, null)
INSERT INTO CourtBookings
VALUES ('2025-07-22', 1, 3, null, 0, null)
INSERT INTO CourtBookings
VALUES ('2025-06-11', 0, 4, null, 2, null)
INSERT INTO CourtBookings
VALUES ('2025-05-21', 0, 9, 1, null, null)
INSERT INTO CourtBookings
VALUES ('2025-08-06', 2, 1, 2, null, null)
INSERT INTO CourtBookings
VALUES ('2025-06-13', 1, 8, 0, null, null)
INSERT INTO CourtBookings
VALUES ('2025-07-8', 2, 3, null, null, 0)
INSERT INTO CourtBookings
VALUES ('2025-05-25', 0, 11, null, null, 1)
INSERT INTO CourtBookings
VALUES ('2025-08-19', 1, 4, null, null, 2)

INSERT INTO TeamBookings
VALUES (0, 1)
INSERT INTO TeamBookings
VALUES (1, 1)
INSERT INTO TeamBookings
VALUES (2, 1)
INSERT INTO TeamBookings
VALUES (3, 0)
INSERT INTO TeamBookings
VALUES (2, 0)
INSERT INTO TeamBookings
VALUES (0, 0)

INSERT INTO EventBookings
VALUES (0, 0)
INSERT INTO EventBookings
VALUES (1, 2)
INSERT INTO EventBookings
VALUES (3, 2)
INSERT INTO EventBookings
VALUES (0, 4)
INSERT INTO EventBookings
VALUES (1, 6)
INSERT INTO EventBookings
VALUES (0, 6)
INSERT INTO EventBookings
VALUES (3, 6)

INSERT INTO Partners
VALUES (0, 2)
INSERT INTO Partners
VALUES (0, 0)
INSERT INTO Partners
VALUES (2, 0)
INSERT INTO Partners
VALUES (3, 0)
INSERT INTO Partners
VALUES (2, 1)
