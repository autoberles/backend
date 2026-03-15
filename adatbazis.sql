-- Database
CREATE DATABASE IF NOT EXISTS car_rental
CHARACTER SET utf8mb4
COLLATE utf8mb4_hungarian_ci;

USE car_rental;

-- Transmission types
CREATE TABLE transmission_types (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(50) NOT NULL UNIQUE
);

-- Fuel types
CREATE TABLE fuel_types (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(50) NOT NULL UNIQUE
);

-- Air conditioning types
CREATE TABLE air_conditioning_types (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(50) NOT NULL UNIQUE
);

-- Wheel drive types
CREATE TABLE wheel_drive_types (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(50) NOT NULL UNIQUE
);

-- Car categories
CREATE TABLE car_categories (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(50) NOT NULL UNIQUE
);

-- Branches
CREATE TABLE branches (
    id INT PRIMARY KEY,
    city VARCHAR(100) NOT NULL,
    address VARCHAR(255) NOT NULL UNIQUE,
    email VARCHAR(150) NOT NULL UNIQUE,
    phone_number VARCHAR(30) NOT NULL UNIQUE
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
    availability BOOLEAN NOT NULL,
    brand VARCHAR(50) NOT NULL,
    model VARCHAR(50) NOT NULL,
    year YEAR NOT NULL,
    color VARCHAR(50) NOT NULL,
    own_weight INT NOT NULL,
    total_weight INT NOT NULL,
    number_of_seats SMALLINT NOT NULL,
    number_of_doors SMALLINT NOT NULL,
    price INT NOT NULL,
    license_plate VARCHAR(20) NOT NULL UNIQUE,
    mileage INT NOT NULL,
    luggage_capacity INT NOT NULL,
    cubic_capacity INT NOT NULL,
    tank_capacity INT,
    battery_capacity INT,
    performance_kw INT NOT NULL,
    performance_hp INT NOT NULL,
    last_service_date DATE NOT NULL,
    inspection_expiry_date DATE NOT NULL,
    branch_id INT NOT NULL,
    transmission_id INT NOT NULL,
    fuel_type_id INT NOT NULL,
    wheel_drive_type_id INT NOT NULL,
    car_category_id INT NOT NULL,
    default_price_per_day INT NOT NULL,
    FOREIGN KEY (branch_id) REFERENCES branches(id),
    FOREIGN KEY (transmission_id) REFERENCES transmission_types(id),
    FOREIGN KEY (fuel_type_id) REFERENCES fuel_types(id),
    FOREIGN KEY (wheel_drive_type_id) REFERENCES wheel_drive_types(id),
    FOREIGN KEY (car_category_id) REFERENCES car_categories(id)
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
    tempomat BOOLEAN NOT NULL,
    FOREIGN KEY (car_id) REFERENCES cars(id),
    FOREIGN KEY (air_conditioning_id) REFERENCES air_conditioning_types(id)
);

-- Rentals
CREATE TABLE rentals (
    id INT PRIMARY KEY,
    car_id INT NOT NULL,
    user_id INT NOT NULL,
    fuel_level_start INT NOT NULL,
    fuel_level_end INT NOT NULL,
    start_date DATE NOT NULL,
    end_date DATE NOT NULL,
    full_price INT NOT NULL,
    FOREIGN KEY (car_id) REFERENCES cars(id),
    FOREIGN KEY (user_id) REFERENCES users(id)
);

-- Transmission types
INSERT INTO transmission_types (name) VALUES
('automata'),
('dupla kuplungos'),
('fokozatmentes automata'),
('manuális');

-- Fuel types
INSERT INTO fuel_types (name) VALUES
('benzines'),
('dízel'),
('elektromos'),
('hibrid');

-- Air conditioning types
INSERT INTO air_conditioning_types (name) VALUES
('automata'),
('manuális');

-- Wheel drive types
INSERT INTO wheel_drive_types (name) VALUES
('elsőkerék-meghajtású'),
('hátsókerék-meghajtású'),
('összkerékmeghajtású');

-- Car categories
INSERT INTO car_categories (name) VALUES
('gazdaságos kisautó'),
('középkategóriás családi autó'),
('nagykategóriás autó'),
('sportautó'),
('városi terepjáró');

-- Branches
INSERT INTO branches VALUES
(1,'Budapest','1044 Budapest, Váci út 123.','budapest@autoker.hu','+36 15 555 123'),
(2,'Győr','9022 Győr, Szent István út 45.','gyor@autoker.hu','+36 96 555 678'),
(3,'Szeged','6724 Szeged, Kossuth Lajos sugárút 78.','szeged@autoker.hu','+36 62 555 987'),
(4,'Debrecen','4026 Debrecen, Piac utca 12.','debrecen@autoker.hu','+36 52 555 111'),
(5,'Székesfehérvár','8000 Székesfehérvár, Palotai út 33.','fehervar@autoker.hu','+36 22 555 222'),
(6,'Pécs','7621 Pécs, Rákóczi út 50.','pecs@autoker.hu','+36 72 555 333'),
(7,'Miskolc','3711 Miskolc, Széchenyi utca 88.','miskolc@autoker.hu','+36 46 555 444'),
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
(1,FALSE,'Toyota','Corolla',2021,'fehér',1350,1850,5,4,8990000,'AB-CD-101',45000,470,1798,50,NULL,97,132,'2025-09-15','2026-06-30',2,3,2,1,2,15000),
(2,TRUE,'Volkswagen','Golf',2019,'fekete',1280,1780,5,5,6590000,'EF-GH-214',80000,380,1498,50,NULL,85,116,'2024-07-10','2026-09-09',1,1,1,1,1,14000),
(3,FALSE,'BMW','320d',2020,'kék',1500,2050,5,4,11290000,'IJ-KL-335',60000,480,1995,57,NULL,140,190,'2025-01-20','2026-05-09',2,2,2,2,3,20000),
(4,TRUE,'Ford','Focus',2018,'ezüst',1320,1820,5,5,5290000,'MN-OP-442',90000,375,1496,52,NULL,92,125,'2024-05-05','2027-03-19',3,1,1,1,2,13000),
(5,FALSE,'Audi','A4',2022,'szürke',1550,2100,5,4,13490000,'QR-ST-558',30000,460,1984,54,NULL,150,204,'2026-01-18','2026-06-30',2,2,2,1,3,21000),
(6,FALSE,'Hyundai','Tucson',2021,'fehér',1650,2200,5,5,10490000,'UV-WX-671',40000,513,1598,52,13,132,179,'2025-08-12','2026-08-30',2,3,3,1,4,18000),
(7,TRUE,'Skoda','Octavia',2020,'fekete',1380,1880,5,5,7890000,'YZ-AB-782',70000,600,1968,50,NULL,110,150,'2024-12-30','2026-05-05',1,1,1,1,2,15000),
(8,FALSE,'Suzuki','Vitara',2019,'kék',1200,1700,5,5,5990000,'CD-EF-893',85000,375,1373,47,NULL,103,140,'2024-06-15','2026-07-18',1,1,1,1,4,14000),
(9,TRUE,'Mercedes','C220',2022,'ezüst',1600,2150,5,4,15990000,'GH-IJ-904',25000,455,1950,66,NULL,143,194,'2026-02-01','2026-05-30',2,4,2,2,3,23000),
(10,FALSE,'Kia','Sportage',2021,'szürke',1680,2230,5,5,10990000,'KL-MN-315',50000,503,1598,54,14,132,179,'2025-09-01','2026-08-31',4,2,3,1,4,18000),
(11,TRUE,'Opel','Astra',2019,'fehér',1250,1750,5,5,5490000,'OP-QR-426',95000,370,1399,48,NULL,81,110,'2024-07-20','2026-09-01',6,1,1,1,1,13000),
(12,TRUE,'Renault','Megane',2020,'fekete',1320,1820,5,5,6390000,'ST-UV-537',75000,440,1332,50,NULL,97,132,'2025-02-10','2027-03-12',3,1,2,1,2,14000),
(13,FALSE,'Tesla','Model 3',2023,'piros',1840,2250,5,4,17990000,'WX-YZ-648',15000,425,0,NULL,60,208,283,'2026-03-01','2027-06-30',2,4,4,3,3,25000),
(14,TRUE,'Peugeot','3008',2021,'kék',1600,2100,5,5,9890000,'AA-BB-759',40000,520,1598,53,NULL,121,165,'2025-08-20','2026-10-30',5,2,2,1,4,17000),
(15,TRUE,'Porsche','911 Carrera',2022,'piros',1505,1850,2,2,45990000,'GG-HH-123',10000,132,2981,NULL,70,283,385,'2026-02-15','2026-08-22',2,2,4,2,5,40000),
(16,FALSE,'Nissan','Qashqai',2022,'szürke',1500,2000,5,5,11990000,'CC-DD-860',35000,504,1332,55,NULL,116,158,'2026-01-25','2026-06-26',7,2,1,1,4,18000),
(17,TRUE,'Mazda','CX-5',2020,'ezüst',1620,2140,5,5,10490000,'EE-FF-971',55000,506,2488,58,NULL,143,194,'2025-03-10','2026-09-15',8,2,1,1,4,18000);

-- Additional Equipment
INSERT INTO additional_equipment VALUES
(1,1,TRUE,2,TRUE,TRUE,FALSE,FALSE),
(2,2,FALSE,1,FALSE,TRUE,FALSE,FALSE),
(3,3,TRUE,2,TRUE,TRUE,TRUE,TRUE),
(4,4,TRUE,1,FALSE,FALSE,FALSE,FALSE),
(5,5,TRUE,2,TRUE,TRUE,TRUE,TRUE),
(6,6,TRUE,2,TRUE,TRUE,FALSE,FALSE),
(7,7,FALSE,1,FALSE,TRUE,FALSE,FALSE),
(8,8,TRUE,1,FALSE,FALSE,FALSE,FALSE),
(9,9,TRUE,2,TRUE,TRUE,TRUE,TRUE),
(10,10,TRUE,2,TRUE,TRUE,FALSE,FALSE),
(11,11,FALSE,1,FALSE,FALSE,FALSE,FALSE),
(12,12,TRUE,1,FALSE,TRUE,FALSE,FALSE),
(13,13,TRUE,2,TRUE,TRUE,TRUE,TRUE),
(14,14,TRUE,2,TRUE,TRUE,FALSE,FALSE),
(15,15,TRUE,2,TRUE,TRUE,TRUE,TRUE),
(16,16,TRUE,2,TRUE,TRUE,FALSE,FALSE),
(17,17,FALSE,1,FALSE,TRUE,FALSE,FALSE);

-- Rentals
INSERT INTO rentals VALUES
(1,3,1,85,60,'2025-04-01','2025-04-05',100000),
(2,8,2,90,95,'2025-04-02','2025-04-04',42000),
(3,13,3,70,80,'2025-04-10','2025-04-15',150000),
(4,5,4,95,50,'2025-05-01','2025-05-07',147000),
(5,1,5,60,65,'2025-05-12','2025-05-18',105000),
(6,10,6,80,75,'2025-05-05','2025-05-09',90000),
(7,16,7,50,70,'2025-05-20','2025-05-25',108000),
(8,6,8,75,40,'2025-06-01','2025-06-03',54000);