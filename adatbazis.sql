-- Database
CREATE DATABASE IF NOT EXISTS car_rental
CHARACTER SET utf8mb4
COLLATE utf8mb4_hungarian_ci;

USE car_rental;

-- Type tables
CREATE TABLE transmission_types (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE fuel_types (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE air_conditioning_types (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(50) NOT NULL UNIQUE
);

-- Branches
CREATE TABLE branches (
    id INT PRIMARY KEY,
    city VARCHAR(100) NOT NULL,
    address VARCHAR(255) NOT NULL,
    email VARCHAR(150) NOT NULL,
    phone_number VARCHAR(30) NOT NULL
);

-- Users
CREATE TABLE users (
    id INT PRIMARY KEY,
    last_name VARCHAR(100) NOT NULL,
    first_name VARCHAR(100) NOT NULL,
    email VARCHAR(150) NOT NULL UNIQUE,
    birth_date DATE NOT NULL
);

-- Cars
CREATE TABLE cars (
    id INT PRIMARY KEY,
    brand VARCHAR(50) NOT NULL,
    model VARCHAR(50) NOT NULL,
    year YEAR NOT NULL,
    number_of_seats SMALLINT NOT NULL,
    price INT NOT NULL,
    license_plate VARCHAR(20) NOT NULL UNIQUE,
    branch_id INT NOT NULL,
    transmission_id INT NOT NULL,
    fuel_type_id INT NOT NULL,
    FOREIGN KEY (branch_id) REFERENCES branches(id),
    FOREIGN KEY (transmission_id) REFERENCES transmission_types(id),
    FOREIGN KEY (fuel_type_id) REFERENCES fuel_types(id)
);

-- Additional equipment
CREATE TABLE additional_equipment (
    id INT PRIMARY KEY,
    car_id INT NOT NULL,
    parking_sensors BOOLEAN NOT NULL,
    air_conditioning_id INT NOT NULL,
    heated_seats BOOLEAN NOT NULL,
    navigation BOOLEAN NOT NULL,
    leather_seats BOOLEAN NOT NULL,
    FOREIGN KEY (car_id) REFERENCES cars(id),
    FOREIGN KEY (air_conditioning_id) REFERENCES air_conditioning_types(id)
);

-- Rentals
CREATE TABLE rentals (
    id INT PRIMARY KEY,
    car_id INT NOT NULL,
    user_id INT NOT NULL,
    start_date DATE NOT NULL,
    end_date DATE NOT NULL,
    FOREIGN KEY (car_id) REFERENCES cars(id),
    FOREIGN KEY (user_id) REFERENCES users(id)
);

-- Types
INSERT INTO transmission_types (name) VALUES
('automata'),
('manuális');

INSERT INTO fuel_types (name) VALUES
('benzines'),
('dízel'),
('elektromos'),
('hibrid');

INSERT INTO air_conditioning_types (name) VALUES
('automata'),
('manuális');

-- Branches
INSERT INTO branches VALUES
(1,'Budapest','1138 Budapest, Váci út 123.','budapest@autoker.hu','+36 1 555 1234'),
(2,'Győr','9027 Győr, Szent István út 45.','gyor@autoker.hu','+36 96 555 678'),
(3,'Szeged','6724 Szeged, Kossuth Lajos sugárút 78.','szeged@autoker.hu','+36 62 555 987'),
(4,'Debrecen','4025 Debrecen, Piac utca 12.','debrecen@autoker.hu','+36 52 555 111'),
(5,'Székesfehérvár','8000 Székesfehérvár, Palotai út 33.','fehervar@autoker.hu','+36 22 555 222'),
(6,'Pécs','7621 Pécs, Rákóczi út 50.','pecs@autoker.hu','+36 72 555 333'),
(7,'Miskolc','3525 Miskolc, Széchenyi utca 88.','miskolc@autoker.hu','+36 46 555 444'),
(8,'Zalaegerszeg','8900 Zalaegerszeg, Kossuth Lajos utca 21.','zalaegerszeg@autoker.hu','+36 92 555 555');

-- Users
INSERT INTO users VALUES
(1,'Kovács','András','kovacs.andras@email.hu','1990-05-12'),
(2,'Nagy','Eszter','nagy.eszter@email.hu','1988-11-03'),
(3,'Tóth','Bence','toth.bence@email.hu','1995-07-21'),
(4,'Szabó','Lilla','szabo.lilla@email.hu','1992-02-14'),
(5,'Varga','Dávid','varga.david@email.hu','1985-09-30'),
(6,'Kiss','Anna','kiss.anna@email.hu','1998-12-08'),
(7,'Molnár','Gábor','molnar.gabor@email.hu','1993-04-17'),
(8,'Horváth','Zsófia','horvath.zsofia@email.hu','1991-06-25');

-- Cars
INSERT INTO cars VALUES
(1,'Toyota','Corolla',2021,5,8990000,'AB-CD-101',1,2,3),
(2,'Volkswagen','Golf',2019,5,6590000,'EF-GH-214',2,1,1),
(3,'BMW','320d',2020,5,11290000,'IJ-KL-335',4,2,2),
(4,'Ford','Focus',2018,5,5290000,'MN-OP-442',3,1,1),
(5,'Audi','A4',2022,5,13490000,'QR-ST-558',5,2,2),
(6,'Hyundai','Tucson',2021,5,10490000,'UV-WX-671',6,2,3),
(7,'Skoda','Octavia',2020,5,7890000,'YZ-AB-782',7,1,1),
(8,'Suzuki','Vitara',2019,5,5990000,'CD-EF-893',8,1,1),
(9,'Mercedes','C220',2022,5,15990000,'GH-IJ-904',1,2,2),
(10,'Kia','Sportage',2021,5,10990000,'KL-MN-315',4,2,3),
(11,'Opel','Astra',2019,5,5490000,'OP-QR-426',6,1,1),
(12,'Renault','Megane',2020,5,6390000,'ST-UV-537',3,1,2),
(13,'Tesla','Model 3',2023,5,17990000,'WX-YZ-648',2,2,4),
(14,'Peugeot','3008',2021,5,9890000,'AA-BB-759',5,2,2),
(15,'Nissan','Qashqai',2022,5,11990000,'CC-DD-860',7,2,1),
(16,'Mazda','CX-5',2020,5,10490000,'EE-FF-971',8,2,1);

-- Additional Equipment
INSERT INTO additional_equipment VALUES
(1,1,TRUE,2,TRUE,TRUE,FALSE),
(2,2,FALSE,1,FALSE,TRUE,FALSE),
(3,3,TRUE,2,TRUE,TRUE,TRUE),
(4,4,TRUE,1,FALSE,FALSE,FALSE),
(5,5,TRUE,2,TRUE,TRUE,TRUE),
(6,6,TRUE,2,TRUE,TRUE,FALSE),
(7,7,FALSE,1,FALSE,TRUE,FALSE),
(8,8,TRUE,1,FALSE,FALSE,FALSE),
(9,9,TRUE,2,TRUE,TRUE,TRUE),
(10,10,TRUE,2,TRUE,TRUE,FALSE),
(11,11,FALSE,1,FALSE,FALSE,FALSE),
(12,12,TRUE,1,FALSE,TRUE,FALSE),
(13,13,TRUE,2,TRUE,TRUE,TRUE),
(14,14,TRUE,2,TRUE,TRUE,FALSE),
(15,15,TRUE,2,TRUE,TRUE,FALSE),
(16,16,TRUE,2,TRUE,TRUE,FALSE);

-- Rentals
INSERT INTO rentals VALUES
(1,3,1,'2025-03-01','2025-03-05'),
(2,8,2,'2025-03-02','2025-03-04'),
(3,13,3,'2025-03-10','2025-03-15'),
(4,5,4,'2025-04-01','2025-04-07'),
(5,1,5,'2025-04-12','2025-04-18'),
(6,10,6,'2025-05-05','2025-05-09'),
(7,15,7,'2025-05-20','2025-05-25'),
(8,6,8,'2025-06-01','2025-06-03');